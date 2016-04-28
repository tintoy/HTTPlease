#!/bin/bash

# TODO: Get build version from command-line argument
DNX_BUILD_VERSION=dev

projects=`ls -d1 ./src/HTTPlease*`
for project in $projects; do
	echo "Packing \"$project\"."
	dnu pack "$project"
done

echo "Done."
