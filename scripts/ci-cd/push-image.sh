#!/bin/bash
#
# Publishes a Docker image.
#
# Environment variables:
#
#   REGISTRY_HOST - Docker Hub username or Azure Container Registry hostname
#   REGISTRY_USERNAME - Username for your registry
#   REGISTRY_PASSWORD - Password for your registry
#   IMAGE_NAME - The name of the image
#   IMAGE_VERSION - The image's version number (tag)
#
# Usage:
#
#       ./scripts/cicd/push-image.sh
#

set -u # or set -o nounset
: "$REGISTRY_HOST"
: "$REGISTRY_USERNAME"
: "$REGISTRY_PASSWORD"
: "$IMAGE_NAME"
: "$IMAGE_VERSION"

echo "Logging in to registry $REGISTRY_HOST"
echo "$REGISTRY_PASSWORD" | docker login $REGISTRY_HOST --username $REGISTRY_USERNAME --password-stdin

echo "Pushing image $REGISTRY_HOST/$IMAGE_NAME:$IMAGE_VERSION"
docker push $REGISTRY_HOST/$IMAGE_NAME:$IMAGE_VERSION