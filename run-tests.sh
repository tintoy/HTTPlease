#!/bin/bash

if [ "$TRAVIS_BUILD_NUMBER" == "" ] || [ "$HTTPLEASE_BUILD_VERSION" == "" ]; then
	DNX_BUILD_VERSION="dev"
else
	DNX_BUILD_VERSION="${HTTPLEASE_BUILD_VERSION}-${TRAVIS_BUILD_NUMBER}"
fi

echo "Running all tests with build version '${DNX_BUILD_VERSION}'."

testProjects=`ls -d1 ./test/HTTPlease*`
for testProject in $testProjects; do
	echo "Running tests for project \"$testProject\"."
	dnx -p "$testProject" test -appveyor
done

echo "Done."
