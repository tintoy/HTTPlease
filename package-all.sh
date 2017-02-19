#!/bin/bash

if [ "$TRAVIS_BUILD_NUMBER" != "" ] && [ "$HTTPLEASE_BUILD_VERSION" != "" ]; then
	BuildVersion="${HTTPLEASE_BUILD_VERSION}-${TRAVIS_BUILD_NUMBER}"
else
	BuildVersion="dev"
fi

echo "Building all packages with build version '${BuildVersion}'."

projects=$(find src -name 'HTTPlease*.csproj')
for project in $projects; do
	echo "Packing \"$project\"."
	dotnet pack "$project" --version-suffix $BuildVersion
done

echo "Done."
