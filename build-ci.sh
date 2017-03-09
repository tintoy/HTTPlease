#!/bin/bash

set -e

############################
# Build script for Travis CI
############################

BUILD_ID=${TRAVIS_JOB_ID:=dev}
BUILD_VERSION_SUFFIX="beta4-${BUILD_ID}"

# Build outputs go here.
artifactsDirectory="$PWD/artifacts"
if [ -d $artifactsDirectory ]; then
	rm -rf $artifactsDirectory
fi

echo ''
echo 'Restoring packages...'
echo ''

dotnet restore /p:VersionSuffix=${BUILD_VERSION_SUFFIX}

echo ''
echo 'Building...'
echo ''

dotnet build --version-suffix ${BUILD_VERSION_SUFFIX}

echo ''
echo 'Testing...'
echo ''

testProjects=$(find ./test -name 'HTTPlease.*Tests.csproj')
for testProject in $testProjects; do
	dotnet test $testProject
done

echo ''
echo 'Packing...'
echo ''

dotnet pack --version-suffix ${BUILD_VERSION_SUFFIX} --output $artifactsDirectory
