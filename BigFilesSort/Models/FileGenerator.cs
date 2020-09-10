using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BigFilesSort.Models
{
    public static class FileGenerator
    {
        public static void CreateRandomIntFile(string path, int count)
        {
            using (var SW = new StreamWriter(path))
            {
                var rnd = new Random();
                while (count > 1)
                {
                    SW.WriteLine(rnd.Next(10_001));
                    count--;
                }
                SW.Write(new Random().Next(10_001));
            }
        }
    }
}
