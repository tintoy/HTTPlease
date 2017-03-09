#!/bin/bash

set -e

############################
# Build script for Travis CI
############################

BUILD_BASEVERSION=$(cat $PWD/build-version-suffix.txt)
BUILD_ID=${TRAVIS_JOB_ID:=dev}
BUILD_VERSION_SUFFIX="${BUILD_BASEVERSION}-${BUILD_ID}"

echo ''
echo "Version suffix is '$BUILD_VERSION_SUFFIX'."
echo ''

# Build outputs go here.
ARTIFACTS_DIRECTORY="$PWD/artifacts"
if [ -d $ARTIFACTS_DIRECTORY ]; then
	rm -rf $ARTIFACTS_DIRECTORY
fi

echo ''
echo 'Restoring packages...'
echo ''

dotnet restore /p:VersionSuffix=$BUILD_VERSION_SUFFIX

echo ''
echo 'Building...'
echo ''

dotnet build --version-suffix $BUILD_VERSION_SUFFIX

echo ''
echo 'Testing...'
echo ''

testProjects=$(find ./test -name 'HTTPlease.*Tests.csproj')
for testProject in $testProjects; do
	dotnet test $testProject
done

echo ''
echo "Packing into '$ARTIFACTS_DIRECTORY'..."
echo ''

dotnet pack --version-suffix $BUILD_VERSION_SUFFIX --output $ARTIFACTS_DIRECTORY
