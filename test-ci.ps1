$testDir = Join-Path $PSScriptRoot 'test'
$testProjects = Get-ChildItem $testDir -Recurse -File -Filter '*.Tests.csproj'

$failingProjects = @()
ForEach ($testProject In $testProjects) {
	dotnet test --no-build $testProject.FullName

	If ($LASTEXITCODE) {
		$failingProjects += $testProject.Name.Replace('.csproj', '')
	}
}

If ($failingProjects) {
	Throw "The following projects have one or more failing tests: [$([string]::Join(', ', $failingProjects))]."
}