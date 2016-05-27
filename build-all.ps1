Param(
	[string] $BuildVersion
)

If (!$BuildVersion) {
	$BuildVersion = 'dev'
}

$dotnet = Get-Command dotnet
& $dotnet build '.\src\HTTPlease*' '.\test\HTTPlease*' --version-suffix $BuildVersion
