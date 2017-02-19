$dotnet = Get-Command dotnet

$projectFiles = Get-ChildItem -File -Recurse 'test\HTTPlease*.csproj'
ForEach ($projectFile in $projectFiles) {
	& $dotnet test $projectFile
}
