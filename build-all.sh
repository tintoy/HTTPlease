#!/bin/bash

if [ "$TRAVIS_BUILD_NUMBER" != "" ] && [ "$HTTPLEASE_BUILD_VERSION" != "" ]; then
	DNX_BUILD_VERSION="${HTTPLEASE_BUILD_VERSION}-${TRAVIS_BUILD_NUMBER}"
else
	DNX_BUILD_VERSION="dev"
fi

echo "Building all projects with build version '${DNX_BUILD_VERSION}'."

dnu build ./src/HTTPlease* ./test/HTTPlease* --quiet
