Param(
    [switch] $Verbose
)

If ($Verbose) {
    $quietSwitch += ''
}
Else {
    $quietSwitch += '--quiet'
}

$dnu = Get-Command dnu
& $dnu build '.\src\HTTPlease*' '.\test\HTTPlease*' "$quietSwitch"
