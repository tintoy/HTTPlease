#!/bin/bash

set -e

############################
# Build script for Travis CI
############################

BUILD_ID=${TRAVIS_JOB_ID:=dev}
BUILD_VERSION_SUFFIX="beta4-${BUILD_ID}"

# Build outputs go here.
artifactsDirectory="$PWD/artifacts""
if [-d $artifactsDirectory]; then
	rm -rf $artifactsDirectory
fi

echo 'Build:'

dotnet restore /p:VersionSuffix=${BUILD_VERSION_SUFFIX}
dotnet build --version-suffix ${BUILD_VERSION_SUFFIX}

echo 'Test:'

testProjects=$(find ./tests -name 'HTTPlease.*Tests.csproj')
for testProject in $testProjects; do
	dotnet test $testProject
done

echo 'Pack:'

dotnet pack --version-suffix ${BUILD_VERSION_SUFFIX} --output $artifactsDirectory
