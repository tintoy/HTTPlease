Param(
    [string] $BuildVersion
)

If ($BuildVersion) {
    $env:DNX_BUILD_VERSION = $BuildVersion
}
Else {
    $env:DNX_BUILD_VERSION = 'dev'
}

$dotnet = Get-Command dotnet

Function Invoke-DnxTests([string] $ProjectName) {
	& $dotnet test ".\test\$ProjectName"
}

Invoke-DnxTests -ProjectName HTTPlease.Core.Tests
Invoke-DnxTests -ProjectName HTTPlease.Formatters.Tests
