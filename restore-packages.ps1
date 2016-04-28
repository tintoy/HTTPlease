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

If ($Verbose) {
    $quietSwitch += ''
}
Else {
    $quietSwitch += '--quiet'
}

$dnu = Get-Command dnu
& $dnu restore "$quietSwitch"
