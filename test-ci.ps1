$testDir = Join-Path $PSScriptRoot 'test'
$testProjects = Get-ChildItem $testDir -Recurse -File -Filter '*.Tests.csproj'

ForEach ($testProject In $testProjects) {
	dotnet test --no-build $testProject.FullName
}
