#!/bin/bash

if [ "$TRAVIS_BUILD_NUMBER" != "" ] && [ "$HTTPLEASE_BUILD_VERSION" != "" ]; then
	BuildVersion="${HTTPLEASE_BUILD_VERSION}-${TRAVIS_BUILD_NUMBER}"
else
	BuildVersion="dev"
fi

echo "Building all projects with build version '${BuildVersion}'."

dotnet build --version-suffix $BuildVersion
