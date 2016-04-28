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

$outputFolderPath = '.\src\artifacts\packages'
$outputFolder = Get-Item $outputFolderPath -EA SilentlyContinue
If (!$outputFolder) {
    $outputFolder = MkDir $outputFolderPath
}

$dnu = Get-Command dnu
$projectDirectories = Dir -Directory 'src\HTTPlease*'
ForEach ($projectDirectory in $projectDirectories) {
    & $dnu pack "$projectDirectory" --out "$outputFolder" "$quietSwitch"
}
