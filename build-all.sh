#!/bin/bash

# Defaults
quietFlag="--quiet"

# Command-line arguments
while [[ $# > 0 ]]; do
	key="$1"

	case $key in
		-?|--help
			echo "Usage:\n\tbuild-all.sh [-v|--verbose]"
		;;
		
		-v|--verbose)
			quietFlag=""
		;;

		*)
			echo "Invalid argument '${key}' (try using -? or --help)."
		;;
	esac
done

dnu build src/HTTPlease.* test/HTTPlease.*.Tests $quietFlag
