$dotnet = Get-Command dotnet

Function Invoke-DnxTests([string] $ProjectName) {
	& $dotnet test ".\test\$ProjectName"
}

Invoke-DnxTests -ProjectName HTTPlease.Core.Tests
Invoke-DnxTests -ProjectName HTTPlease.Formatters.Tests
