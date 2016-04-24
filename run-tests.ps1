Param(
    [switch] $Verbose
)

$dnx = Get-Command dnx

If ($Verbose) {
    $verboseSwitch = '-verbose'
}
Else {
    $verboseSwitch = ''
}

Function Invoke-DnxTests([string] $ProjectName) {
	& $dnx -p ".\test\$ProjectName" test $verboseSwitch
}

Invoke-DnxTests -ProjectName HTTPlease.Core.Tests
Invoke-DnxTests -ProjectName HTTPlease.Formatters.Tests
