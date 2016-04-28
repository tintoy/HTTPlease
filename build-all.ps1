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

& $dnu build 'src\HTTPlease.*' 'test\HTTPlease.*' $quietFlag
