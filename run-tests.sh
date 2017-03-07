#!/bin/bash

echo "Running all tests..."

testProjects=$(find test -name 'HTTPlease*.csproj')
for testProject in $testProjects; do
	dotnet test "$testProject"
done

echo "Done."
