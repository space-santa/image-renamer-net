using System;
using System.Collections.Generic;
using System.IO;

namespace ImageRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            var paths = new List<string>();

            foreach (string arg in args)
            {
                if (File.Exists(arg))
                {
                    paths.Add(arg);
                }
            }

            var renamer = new FileRenamer(new Mover());
        }
    }
}
