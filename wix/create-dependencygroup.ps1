heat.exe dir ..\ImageRenamer.Wpf\bin\Release\netcoreapp3.0\publish -cg DependencyGroup -dr INSTALLDIR -gg -g1 -out DependencyGroup.wxs

(Get-Content .\DependencyGroup.wxs) | ForEach-Object {
    $_ -replace 'SourceDir', "..\ImageRenamer.Wpf\bin\Release\netcoreapp3.0\publish"
} | Set-Content .\DependencyGroup.wxs
