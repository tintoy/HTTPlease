Param(
	[string] $BuildVersion = $null
)

If (!$BuildVersion) {
	$BuildVersion = 'dev'
}

$dotnet = Get-Command dotnet
& $dotnet build --version-suffix $BuildVersion
