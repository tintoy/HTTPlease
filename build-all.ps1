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
& $dnu build 'src\HTTPlease*' 'test\HTTPlease*' "$quietSwitch"

# Override dnu exit code (it's borked for rc1-update2 and always returns a non-zero exit code when building multiple projects).
$LASTEXITCODE = 0
