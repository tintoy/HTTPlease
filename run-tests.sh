#!/bin/bash

testProjects=`ls -d1 ./test/*.Tests`
for testProject in $testProjects; do
	echo "Running tests for project \"$testProject\"."
	dnx -p "$testProject" test
done

echo "Done."
