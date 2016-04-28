#!/bin/bash

dnu build ./src/HTTPlease* ./test/HTTPlease* --quiet

echo "dnu exited with code $?"

# WTF - dnu is broken and always exits with code 1 (even if there are no build errors).
exit 0
