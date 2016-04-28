Param(
	[Parameter(Mandatory = $true)]
	[int] $BuildNumber
)

$ErrorActionPreference = 'Stop'

# AF - note that this doesn't currently update dependency versions (Powershell's JSON support sucks)

Function Update-NetCoreProjectVersion([string] $ProjectFile)
{
	$project = Get-Content $ProjectFile | ConvertFrom-Json | ? { $_ -ne $null } | select -First 1
    If (!$project) {
        Write-Warning "Project file '$ProjectFile' does not contain valid JSON."

		Return
    }
    If (!$project.version) {
		Write-Warning "Project file '$ProjectFile' does not contain a valid project version."

		Return
	}

	Write-Host "Updating project file '$ProjectFile'..."

	$oldVersion = $project.version
	$newVersion = $oldVersion + "-$BuildNumber"
    Add-Member -InputObject $project -NotePropertyName 'version' -NotePropertyValue $newVersion -Force

    $project | ConvertTo-Json | Set-Content -Path $ProjectFile

    Write-Host "Updated version for '$ProjectFile' from '$oldVersion' to '$newVersion'."

		# AF: What about dependencies?
}

# Update version info for all project files.
$projectFiles = Dir -File '.\src\HTTPlease*\project.json', '.\test\HTTPlease*\project.json'
ForEach ($projectFile In $projectFiles) {
	Update-NetCoreProjectVersion $projectFile
}
