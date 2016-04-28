#!/bin/bash

if [ "$TRAVIS_BUILD_NUMBER" == "" ] || [ "$HTTPLEASE_BUILD_VERSION" == "" ]; then
	DNX_BUILD_VERSION="dev"
else
	DNX_BUILD_VERSION="${HTTPLEASE_BUILD_VERSION}-${TRAVIS_BUILD_NUMBER}"
fi

echo "Building all packages with build version '${DNX_BUILD_VERSION}'."

projects=`ls -d1 ./src/HTTPlease*`
for project in $projects; do
	echo "Packing \"$project\"."
	dnu pack "$project"
done

echo "Done."
