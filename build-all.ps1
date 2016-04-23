Param(
	[switch] $Verbose
)

$dnu = Get-Command dnu

If (!$Verbose) {
	$quietFlag = '--quiet'
}
Else
{
	$quietFlag = ''
}

& $dnu build 'src\HTTPlease.*' 'test\HTTPlease.*' $quietFlag
