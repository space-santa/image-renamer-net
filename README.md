# ImageRenamer

I want the filenames of my photos (and videos) to contain the creation timestamp.
```
YYYY-MM-dd_HH.mm.ss.jpg  # e.g. 1234-12-12_12.34.56.jpg
```
It uses the EXIF OrigDateTime tag of `.jpg` files. If there is no EXIF tag it will use the filename to extract datetime information. `.mp4` files also get their filename parsed.
