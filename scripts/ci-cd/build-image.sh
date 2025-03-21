#!/bin/bash
#
# Builds a Docker image.
#
# Environment variables:
#
#   REGISTRY_HOST - Docker Hub username or Azure Container Registry hostname
#   IMAGE_NAME - The name of the image to build
#   IMAGE_VERSION - The version number to tag the images with
#   MICROSERVICE_DIRECTORY - The directory from which to build the image
#   DOCKERFILE_NAME - The name of the Dockerfile (default: Dockerfile-prod)
#

set -u # or set -o nounset
: "$REGISTRY_HOST"
: "$IMAGE_NAME"
: "$IMAGE_VERSION"
: "$MICROSERVICE_DIRECTORY"

DOCKERFILE_NAME=${DOCKERFILE_NAME:-Dockerfile-prod}

echo "Building image $REGISTRY_HOST/$IMAGE_NAME:$IMAGE_VERSION"
docker build -t $REGISTRY_HOST/$IMAGE_NAME:$IMAGE_VERSION --file ./$MICROSERVICE_DIRECTORY/$DOCKERFILE_NAME ./$MICROSERVICE_DIRECTORY