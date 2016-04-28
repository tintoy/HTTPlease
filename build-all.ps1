Param(
    [switch] $Verbose
)

$args = "build 'src\HTTPlease*' 'test\HTTPlease*'"

If (!$Verbose) {
    $args += ' --quiet'
}

$dnu = Get-Command dnu
& $dnu $args

Return 0
