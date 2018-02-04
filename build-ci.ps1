Write-Host 'Building...'

dotnet build /p:PublicRelease=true

Write-Host 'Packing...'

$PackagesDir = "$PWD/src/artifacts/packages"
MkDir $PackagesDir -EA SilentlyContinue | Out-Null

dotnet pack /p:PublicRelease=true -o $PackagesDir
