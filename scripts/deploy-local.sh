#!/bin/bash
#
# Deploys a microservice locally
# Usage: ./scripts/deploy-local.sh client 8
#

if [ $# -lt 2 ]; then
  echo "Usage: $0 <microservice-name> <version>"
  exit 1
fi

MICROSERVICE_NAME=$1
VERSION=$2
REGISTRY_HOST="abdriano"
IMAGE_NAME=$MICROSERVICE_NAME

# Handle different capitalization and suffixes based on microservice name
case $MICROSERVICE_NAME in
  "client")
    MICROSERVICE_DIRECTORY="MeTube-DevOps.Client"
    DOCKERFILE_NAME="Dockerfile-Client-prod"
    ;;
  "gateway")
    MICROSERVICE_DIRECTORY="MeTube-GateWay"
    DOCKERFILE_NAME="Dockerfile-Gateway-dev"
    ;;
  "user")
    MICROSERVICE_DIRECTORY="MeTube-DevOps.UserService"
    DOCKERFILE_NAME="Dockerfile-User-dev"
    ;;
  *)
    echo "Unknown microservice: $MICROSERVICE_NAME"
    exit 1
    ;;
esac

K8S_MANIFEST_PATH="./scripts/local-k8-cluster/$MICROSERVICE_NAME.yml"

echo "Using directory: $MICROSERVICE_DIRECTORY"
echo "Using Dockerfile: $DOCKERFILE_NAME"

# Export variables for the scripts
export REGISTRY_HOST
export REGISTRY_USERNAME=$REGISTRY_HOST
export IMAGE_NAME
export IMAGE_VERSION=$VERSION
export MICROSERVICE_NAME
export MICROSERVICE_DIRECTORY
export DOCKERFILE_NAME
export K8S_MANIFEST_PATH

# Simpler Docker login check
if ! docker system info > /dev/null 2>&1; then
  echo "Docker is not running or not installed"
  exit 1
fi

echo "Using Docker Hub registry: $REGISTRY_HOST"
echo "Building and deploying $MICROSERVICE_NAME version $VERSION"

# Capture Docker Hub password securely
read -s -p "Enter Docker Hub password: " REGISTRY_PASSWORD
echo
export REGISTRY_PASSWORD

# Run the scripts
echo "Building image..."
chmod +x ./scripts/ci-cd/build-image.sh
./scripts/ci-cd/build-image.sh
if [ $? -ne 0 ]; then
  echo "Build failed!"
  unset REGISTRY_PASSWORD
  exit 1
fi

echo "Pushing image..."
chmod +x ./scripts/ci-cd/push-image.sh
./scripts/ci-cd/push-image.sh
if [ $? -ne 0 ]; then
  echo "Push failed!"
  unset REGISTRY_PASSWORD
  exit 1
fi

echo "Deploying to Kubernetes..."
chmod +x ./scripts/ci-cd/deploy.sh
./scripts/ci-cd/deploy.sh
if [ $? -ne 0 ]; then
  echo "Deployment failed!"
  unset REGISTRY_PASSWORD
  exit 1
fi

echo "Successfully deployed $MICROSERVICE_NAME:$VERSION"
# Clean up
unset REGISTRY_PASSWORD