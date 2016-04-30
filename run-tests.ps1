Param(
    [string] $BuildVersion,
	[switch] $Verbose
)

If ($BuildVersion) {
    $env:DNX_BUILD_VERSION = $BuildVersion
}
Else {
    $env:DNX_BUILD_VERSION = 'dev'
}

If (!$Verbose) {
	$quietFlag = '-appveyor'
}
Else {
	$quietFlag = '-verbose'
}

$dnx = Get-Command dnx

Function Invoke-DnxTests([string] $ProjectName) {
	& $dnx -p ".\test\$ProjectName" test $quietFlag
}

Invoke-DnxTests -ProjectName HTTPlease.Core.Tests
Invoke-DnxTests -ProjectName HTTPlease.Formatters.Tests
