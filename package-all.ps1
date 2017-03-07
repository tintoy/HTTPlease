Param(
	[string] $BuildVersion
)

If (!$BuildVersion) {
	$BuildVersion = 'dev'
}

$outputFolderPath = '.\src\artifacts\packages'
$outputFolder = Get-Item $outputFolderPath -EA SilentlyContinue
If (!$outputFolder) {
    $outputFolder = MkDir $outputFolderPath
}

Write-Host "Building all packages with version suffix '$BuildVersion'..."

dotnet pack --version-suffix $BuildVersion --output "$outputFolder"

Write-Host "Done (packages created in '$PWD/release/packages')."
