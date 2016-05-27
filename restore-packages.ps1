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
& $dotnet restore
