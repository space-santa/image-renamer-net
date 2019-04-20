$version = "1.0.4"
$PathToPublish = "..\ImageRenamer.Wpf\bin\Release\netcoreapp3.0\publish"

dotnet publish ../ImageRenamer.Wpf/ImageRenamer.Wpf.csproj -c Release

[XML]$xml = Get-Content "$pwd\\main.wxs"
$xml.Wix.Product.SetAttribute("Version", $version)
$xml.Save("$pwd\\main.wxs")

heat.exe dir $PathToPublish -cg DependencyGroup -dr INSTALLDIR -gg -g1 -out DependencyGroup.wxs
(Get-Content .\DependencyGroup.wxs) | ForEach-Object {
    $_ -replace "SourceDir", $PathToPublish
} | Set-Content .\DependencyGroup.wxs

candle.exe main.wxs DependencyGroup.wxs
light.exe  -out "$pwd\\msi\\ImageRenamer-${version}.msi" main.wixobj DependencyGroup.wixobj
