using System;
using System.IO;

namespace ImageRenamer
{
    public interface IMover
    {
        void Move(string from, string to);
    }

    public class Mover : IMover
    {
        private static string ChangeNameIfExists(string path)
        {
            string oldName = Path.GetFileNameWithoutExtension(path);
            string newName = path;
            string extension = Path.GetExtension(path).ToLower();
            string folder = Directory.GetParent(path).ToString();

            int i = 1;
            while (File.Exists(newName))
            {
                newName = Path.Combine(folder, $"{oldName}_({i}){extension}");
                i += 1;
            }

            return newName;
        }

        public void Move(string from, string to)
        {
            to = ChangeNameIfExists(to);
            Console.WriteLine($"Rename {from} -> {to}");
            
            System.IO.File.Move(from, to);
        }
    }

    public class MoverMock : IMover
    {
        public string from { get; set; }
        public string to { get; set; }

        public void Move(string from, string to)
        {
            this.from = from;
            this.to = to;
        }
    }
}