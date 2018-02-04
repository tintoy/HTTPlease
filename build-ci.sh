#!/bin/bash

set -e

############################
# Build script for Travis CI
############################

# Build outputs go here.
ARTIFACTS_DIRECTORY="$PWD/artifacts"
if [ -d $ARTIFACTS_DIRECTORY ]; then
	rm -rf $ARTIFACTS_DIRECTORY
fi

echo ''
echo 'Building...'
echo ''

dotnet build /p:DisableGitVersioning=true

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

dotnet pack /p:DisableGitVersioning=true -o "$ARTIFACTS_DIRECTORY"
