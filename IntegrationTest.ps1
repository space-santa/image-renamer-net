function ResultOutput {
    param (
        $arg
    )
    if($arg -eq $True) {
        Write-Host "success" -ForegroundColor Green
    } else {
        Write-Host "fail" -ForegroundColor Red
    }
}

function CheckFile {
    param (
        $path
    )
    Write-Host "Checking" $path " " -NoNewline
    $result = Test-Path $path
    ResultOutput($result)
}

Copy-Item ./ImageRenamer.UnitTests/testdata -Destination tmptest -Recurse
dotnet publish ImageRenamer/ImageRenamer.csproj -c 'Release'
dotnet ./ImageRenamer/bin/Release/netcoreapp2.1/publish/ImageRenamer.dll .\tmptest\*

Write-Host
Write-Host "Checking..."
Write-Host

CheckFile(".\tmptest\blarg")
CheckFile(".\tmptest\2018-06-13_15.23.30.mp4")
CheckFile(".\tmptest\2018-06-13_15.23.30.jpg")
CheckFile(".\tmptest\2018-06-13_15.23.30_(1).jpg")

Write-Host "Checking file count " -NoNewline
$count = (Get-ChildItem ".\tmptest\" | Measure-Object).Count -eq 4
ResultOutput($count)

Remove-Item -Recurse -Force ./tmptest
