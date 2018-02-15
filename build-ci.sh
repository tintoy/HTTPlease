#!/bin/bash

set -euo pipefail

############################
# Build script for Travis CI
############################

echo 'Computing build version...'

mono $PWD/tools/GitVersion/GitVersion.exe
mono $PWD/tools/GitVersion/GitVersion.exe > $PWD/version-info.json

BUILD_BASEVERSION=$(cat $PWD/version-info.json | jq -r .MajorMinorPatch)
BUILD_VERSION_SUFFIX=$(cat $PWD/version-info.json | jq -r .NuGetPreReleaseTagV2)
BUILD_INFORMATIONAL_VERSION=$(cat $PWD/version-info.json | jq -r .InformationalVersion)

echo ''
if [ -z "$BUILD_VERSION_SUFFIX" ]; then
	echo "Build version is '$BUILD_BASEVERSION'."
else
	echo "Build version is '$BUILD_BASEVERSION-$BUILD_VERSION_SUFFIX'."
fi
echo ''

# Build outputs go here.
ARTIFACTS_DIRECTORY="$PWD/artifacts"
if [ -d $ARTIFACTS_DIRECTORY ]; then
	rm -rf $ARTIFACTS_DIRECTORY
fi

echo ''
echo 'Restoring packages...'
echo ''

dotnet restore /p:VersionPrefix="$BUILD_BASEVERSION" /p:VersionSuffix="$BUILD_VERSION_SUFFIX" /p:AssemblyInformationalVersion="$BUILD_INFORMATIONAL_VERSION"

echo ''
echo 'Building...'
echo ''

dotnet build /p:VersionPrefix="$BUILD_BASEVERSION" /p:VersionSuffix="$BUILD_VERSION_SUFFIX" /p:AssemblyInformationalVersion="$BUILD_INFORMATIONAL_VERSION"

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

dotnet pack /p:VersionPrefix="$BUILD_BASEVERSION" /p:VersionSuffix="$BUILD_VERSION_SUFFIX" /p:AssemblyInformationalVersion="$BUILD_INFORMATIONAL_VERSION" -o $ARTIFACTS_DIRECTORY
