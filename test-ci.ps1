$testProjects = Get-ChildItem test/HTTPlease*.csproj -File -Recurse

ForEach ($testProject In $testProjects) {
	dotnet test $testProject
}
