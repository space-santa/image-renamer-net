# ImageRenamer

I want the filenames of my photos (and videos) to contain the creation timestamp.
```
YYYY-MM-dd_HH.mm.ss.jpg  # e.g. 1234-12-12_12.34.56.jpg
```
It uses the EXIF OrigDateTime tag of `.jpg` files. If there is no EXIF tag it will use the filename to extract datetime information. `.mp4` files also get their filename parsed.

To publish a release ready to run as a self contained executable just 

```
cd ImageRenamer.Wpf
dotnet publish -c Release
```

Then copy the entire resulting publish folder somewhere and create a shortcut to the desktop or not. "Self-contained" and "singlefile" are slightly missleading. All the other stuff in the publish folder are required to run. (Not the .pdb files if you don't want to debug.)