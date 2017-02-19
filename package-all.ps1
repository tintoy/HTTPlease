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
$projectFiles = Get-ChildItem -File -Recurse 'src\HTTPlease*.csproj'
ForEach ($projectFile in $projectFiles) {
    & dotnet pack "$projectFile" -o "$outputFolder" --version-suffix $BuildVersion
}
