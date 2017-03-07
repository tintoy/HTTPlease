#!/bin/bash

if [ "$TRAVIS_BUILD_NUMBER" != "" ] && [ "$HTTPLEASE_BUILD_VERSION" != "" ]; then
	BuildVersion="${HTTPLEASE_BUILD_VERSION}-${TRAVIS_BUILD_NUMBER}"
else
	BuildVersion="dev"
fi

echo "Building all packages with version suffix '${BuildVersion}'..."

OutputFolder=$PWD/release/packages
mkdir -p $OutputFolder
dotnet pack --version-suffix $BuildVersion --output "$OutputFolder" /p:Configuration=Release

echo "Done (packages created in '$PWD/release/packages')."
