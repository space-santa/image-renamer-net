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

    public static class FileRenamer
    {
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
