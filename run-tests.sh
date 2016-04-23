#!/bin/bash

echo "Running tests..."
dnx -p ./test/HTTPlease.Core.Tests test
dnx -p ./test/HTTPlease.Formatters.Tests test
echo "Done."
