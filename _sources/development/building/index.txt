How to build HTTPlease
======================

Prerequisites
"""""""""""""

* .NET Core 1.0.0-rc1-update2 (coreclr/x64)

Steps
"""""

* Clone `the HTTPlease repository <https://github.com/tintoy/HTTPlease>`_:
  ``git clone https://github.com/tintoy/HTTPlease.git HTTPlease``
* Call the .NET Version Manager (dnvm) to use the correct runtime:
  ``dnvm use 1.0.0-rc1-update2 -r coreclr -arch x64``
* Restore packages:
  ``.\restore-packages.ps1`` (Windows) or ``.\restore-packages.sh`` (OSX / Linux)
* Build:
  ``.\build-all.ps1`` (Windows) or ``.\build-all.sh`` (OSX / Linux)
* Test:
  ``.\run-tests.ps1`` (Windows) or ``.\run-tests.sh`` (OSX / Linux)

If you get this far, you're good to go!
