Param(
	[string] $BuildVersion = $null
)

If (!$BuildVersion) {
	$BuildVersion = 'dev'
}

# AF: No idea why the globbing is more sensitive on Windows than on OSX / Linux.

$dotnet = Get-Command dotnet
& $dotnet build '.\src\HTTPlease*\project.json' '.\test\HTTPlease*\project.json' --version-suffix $BuildVersion
