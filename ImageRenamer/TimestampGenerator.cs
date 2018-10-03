using System;
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;

namespace ImageRenamer
{
    public class TimestampException : Exception
    {
        public TimestampException(string message)
        : base(message)
        {
        }
    }

    public static class TimestampGenerator
    {
        public static string Run(string orig)
        {
            if (orig.Length == 0)
            {
                throw new ArgumentException("Can't process empty string.");
            }

            List<string> formats = new List<string>();
            formats.Add("yyyy\\:MM\\:dd HH\\:mm\\:ss");
            formats.Add("I\\MG_yyyyMMdd_HHmmss.jp\\g");

            foreach (string format in formats)
            {
                try
                {
                    DateTime dt = DateTime.ParseExact(orig, format, CultureInfo.InvariantCulture);
                    return dt.ToString("yyyy-MM-dd_HH.mm.ss");
                }
                catch (FormatException)
                {
                    Debug.WriteLine("{0} doesn't match {1}", orig, format);
                }
            }

            throw new TimestampException("String " + orig + " doesn't match any format.");
        }
    }
}