$VersionSuffix = (Get-Content "$PWD/build-version-suffix.txt").Trim()

$AppVeyorBuildNumber = $env:APPVEYOR_BUILD_NUMBER
If (!$AppVeyorBuildNumber) {
	$AppVeyorBuildNumber = '0'
}
$AppVeyorBuildNumber = $AppVeyorBuildNumber.PadLeft(4, '0')

$VersionSuffix += "-$AppVeyorBuildNumber"

Write-Host "Version suffix is '$VersionSuffix'."

Write-Host 'Restoring packages...'

dotnet restore /p:VersionSuffix=$VersionSuffix

Write-Host 'Building...'

dotnet build --version-suffix $VersionSuffix

Write-Host 'Packing...'

$PackagesDir = "$PWD/src/artifacts/packages"
MkDir $PackagesDir -EA SilentlyContinue | Out-Null

dotnet pack --version-suffix $VersionSuffix --output $PackagesDir
