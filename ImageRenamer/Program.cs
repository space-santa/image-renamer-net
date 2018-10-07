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
                var path = Path.Combine(Directory.GetCurrentDirectory().ToString(), arg);

                if (File.Exists(path))
                {
                    paths.Add(path);
                }
            }

            var renamer = new FileRenamer(new Mover());
            renamer.RenameFiles(paths);
        }
    }
}
