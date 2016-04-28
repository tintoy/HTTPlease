Param(
    [switch] $Verbose
)

If (!$Verbose) {
    $quietFlag = '--quiet'
}
Else {
    $quietFlag = ''
}

$dnu = Get-Command dnu

& $dnu build 'src\HTTPlease*' 'test\HTTPlease*' $quietFlag

# Ugh - dnu build returns a non-zero exit code when building multiple projects, even if they all succeed.

Return 0
