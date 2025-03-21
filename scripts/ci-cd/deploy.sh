#!/bin/bash
#
# Deploys a microservice to Kubernetes.
#
# Environment variables:
#
#   REGISTRY_HOST - Docker Hub username or Azure Container Registry hostname
#   MICROSERVICE_NAME - The name of the microservice to deploy
#   IMAGE_NAME - The name of the image
#   IMAGE_VERSION - The version of the microservice being deployed
#   K8S_MANIFEST_PATH - Path to the Kubernetes manifest file
#
# Usage:
#
#   ./scripts/cicd/deploy.sh
#

set -u # or set -o nounset
: "$REGISTRY_HOST"
: "$MICROSERVICE_NAME"
: "$IMAGE_NAME"
: "$IMAGE_VERSION"
: "$K8S_MANIFEST_PATH"

# Create a temporary manifest with the updated image tag
TMP_MANIFEST=$(mktemp)
sed "s|image: $REGISTRY_HOST/$IMAGE_NAME:.*|image: $REGISTRY_HOST/$IMAGE_NAME:$IMAGE_VERSION|g" $K8S_MANIFEST_PATH > $TMP_MANIFEST

echo "Deploying $MICROSERVICE_NAME using manifest:"
cat $TMP_MANIFEST

kubectl apply -f $TMP_MANIFEST
kubectl rollout restart deployment/$MICROSERVICE_NAME

rm $TMP_MANIFEST