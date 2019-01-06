using ExifLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace ImageRenamer
{
    public class MissingTagException : Exception
    {
        public MissingTagException()
        : base("File has no DateTimeOriginal tag.")
        {
        }
    }

    public class TimestampException : Exception
    {
        public TimestampException(string message)
        : base(message)
        {
        }
    }

    public class FileRenamer
    {
        IMover _mover;

        public FileRenamer(IMover mover)
        {
            _mover = mover;
        }

        public void RenameFiles(List<string> paths)
        {
            foreach (string path in paths)
            {
                MoveFileIfValidMedia(path);
            }
        }

        private void MoveFileIfValidMedia(string path)
        {
            try
            {
                var to = GetNewFullPath(path);
                _mover.Move(path, to);
            }
            catch (IOException ex)
            {
                // This we want to log. Moving the file should cause no problem.
                Console.WriteLine(ex);
            }
            catch (Exception)
            {
                // Missing EXIF data, bad filename, so most likely not a media file that we want to rename.
            }
        }

        public static string GetNewFullPath(string path)
        {
            string oldName = Path.GetFileNameWithoutExtension(path);
            string newName = GetNewName(path);
            string extension = Path.GetExtension(path).ToLower();
            string folder = Directory.GetParent(path).ToString();
            return Path.Combine(folder, $"{newName}{extension}");
        }

        public static string GetNewName(string path)
        {
            DateTime origTimestamp;

            try
            {
                origTimestamp = GetOrigDateTime(path);
            }
            catch (Exception ex)
            {
                if (ex is MissingTagException ||
                    ex is ArgumentException ||
                    ex is EndOfStreamException ||
                    ex is ExifLibException)
                {
                    origTimestamp = GetDateTimeFromFilename(path);
                }
                else
                {
                    throw ex;
                }
            }

            return FormatDateTime(origTimestamp);
        }

        private static string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd_HH.mm.ss");
        }

        private static DateTime GetDateTimeFromFilename(string fileName)
        {
            if (fileName.Length == 0)
            {
                throw new ArgumentException("Can't process empty string.");
            }


            fileName = Path.GetFileName(fileName);

            List<string> formats = new List<string>();
            formats.Add("yyyy\\:MM\\:dd HH\\:mm\\:ss");
            formats.Add("I\\MG_yyyyMMdd_HHmmss.jp\\g");
            formats.Add("VID_yyyyMMdd_HHmmss.\\mp4");
            formats.Add("yyyyMMdd_HHmmss.\\mp4");

            foreach (string format in formats)
            {
                try
                {
                    DateTime dt = DateTime.ParseExact(fileName, format, CultureInfo.InvariantCulture);
                    return dt;
                }
                catch (FormatException)
                {
                    Debug.WriteLine($"{fileName} doesn't match {format}");
                }
            }

            throw new TimestampException($"String {fileName} doesn't match any format.");
        }

        private static DateTime GetOrigDateTime(string path)
        {
            using (ExifReader reader = new ExifReader(path))
            {
                // Extract the tag data using the ExifTags enumeration
                DateTime datePictureTaken;
                if (reader.GetTagValue<DateTime>(ExifTags.DateTimeOriginal, out datePictureTaken))
                {
                    return datePictureTaken;
                }

                throw new MissingTagException();
            }
        }
    }
}
