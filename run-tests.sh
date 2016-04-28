#!/bin/bash

testProjects=`ls -d1 ./test/HTTPlease*`
for testProject in $testProjects; do
	echo "Running tests for project \"$testProject\"."
	dnx -p "$testProject" test -appveyor
done

echo "Done."
