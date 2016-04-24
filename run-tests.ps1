$dnx = Get-Command dnx

Function Invoke-DnxTests([string] $ProjectName) {
	& $dnx -p ".\test\$ProjectName" test
}

Invoke-DnxTests -ProjectName HTTPlease.Core.Tests
Invoke-DnxTests -ProjectName HTTPlease.Formatters.Tests
