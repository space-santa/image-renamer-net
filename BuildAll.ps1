$array = ("win10-x64", "osx-x64", "linux-x64")
$array |foreach {
    dotnet build ImageRenamer/ImageRenamer.csproj -r $_ 
    dotnet publish ImageRenamer/ImageRenamer.csproj -c release -r $_
}