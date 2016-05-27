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

$dotnet = Get-Command dotnet
$projectDirectories = Dir -Directory 'src\HTTPlease*'
ForEach ($projectDirectory in $projectDirectories) {
    & dotnet pack "$projectDirectory" -o "$outputFolder" --version-suffix $BuildVersion
}
