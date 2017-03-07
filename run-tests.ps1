$dotnet = Get-Command dotnet

Write-Host "Running all tests..."

$projectFiles = Get-ChildItem -File -Recurse 'test\HTTPlease*.csproj'
ForEach ($projectFile in $projectFiles) {
	& $dotnet test $projectFile
}

Write-Host "Done."