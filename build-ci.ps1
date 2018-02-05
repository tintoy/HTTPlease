$VersionSuffix = (Get-Content "$PWD/build-version-suffix.txt")
If ($VersionSuffix) {
	$VersionSuffix = $VersionSuffix.Trim()
}
If ($VersionSuffix) {
	$AppVeyorBuildNumber = $env:APPVEYOR_BUILD_NUMBER
	
	If (!$AppVeyorBuildNumber) {
		$AppVeyorBuildNumber = '0'
	}
	$AppVeyorBuildNumber = $AppVeyorBuildNumber.PadLeft(4, '0')

	$VersionSuffix += "%build$AppVeyorBuildNumber"

	Write-Host "Version suffix is '$VersionSuffix'."
} Else {
	Write-Host 'No version suffix.'
}

Write-Host 'Restoring packages...'

dotnet restore /p:VersionSuffix="$VersionSuffix"

Write-Host 'Building...'

dotnet build /p:VersionSuffix="$VersionSuffix"

Write-Host 'Packing...'

$PackagesDir = "$PWD/src/artifacts/packages"
MkDir $PackagesDir -EA SilentlyContinue | Out-Null

dotnet pack /p:VersionSuffix="$VersionSuffix" -o $PackagesDir
