$ErrorActionPreference = 'Stop'

$versionInfo = .\tools\GitVersion\GitVersion.exe | ConvertFrom-Json

$versionPrefix = $versionInfo.MajorMinorPatch
$versionSuffix = $versionInfo.NuGetPreReleaseTagV2
$informationalVersion = $versionInfo.InformationalVersion

If ($versionSuffix) {
	Write-Host "Build version is '$versionPrefix-$versionSuffix'."
} Else {
	Write-Host "Build version is '$versionPrefix'."
}

Write-Host 'Restoring packages...'

dotnet restore /p:VersionPrefix="$versionPrefix" /p:VersionSuffix="$versionSuffix" /p:AssemblyInformationalVersion="$informationalVersion"

Write-Host 'Building...'

dotnet build /p:VersionPrefix="$versionPrefix" /p:VersionSuffix="$versionSuffix" /p:AssemblyInformationalVersion="$informationalVersion"

Write-Host 'Packing...'

$PackagesDir = "$PWD/src/artifacts/packages"
dotnet pack /p:VersionPrefix="$versionPrefix" /p:VersionSuffix="$versionSuffix" /p:AssemblyInformationalVersion="$informationalVersion" -o $PackagesDir --include-symbols
