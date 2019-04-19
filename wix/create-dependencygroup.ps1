heat.exe dir ..\ImageRenamer.Wpf\bin\Release\netcoreapp3.0\win10-x64\publish -cg DependencyGroup -out DependencyGroup.wxs

$XMLfile = '.\DependencyGroup.wxs'
[XML]$xml = Get-Content $XMLfile
$target = $xml.Wix.Fragment.DirectoryRef | Where-Object { $_.id -eq "TARGETDIR" }
$targetId = $target.directory.id

$cleanGuids = (Get-Content .\DependencyGroup.wxs) | ForEach-Object {
    $_ -replace 'PUT-GUID-HERE', [guid]::NewGuid()
}

$cleanDirs = $cleanGuids | ForEach-Object {
    $_ -replace $targetId, "INSTALLDIR"
}

$cleanDirs | ForEach-Object {
    $_ -replace 'SourceDir', "..\ImageRenamer.Wpf\bin\Release\netcoreapp3.0\win10-x64\publish"
} | Set-Content .\DependencyGroup.wxs

# Now remove the targetdir fragment.
