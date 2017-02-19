#!/bin/bash

echo "Running all tests..."

testProjects=$(find test -name HTTPlease\*.csproj)
for testProject in $testProjects; do
	echo "Running tests for project \"$testProject\"."
	dotnet test "$testProject"
done

echo "Done."
