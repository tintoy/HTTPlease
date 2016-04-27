$dnu = Get-Command dnu

& $dnu build 'src\HTTPlease.*' 'test\HTTPlease.*' --quiet
