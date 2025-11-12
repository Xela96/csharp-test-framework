using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class File
    {
        public static void MoveDirectoryFiles(string sourceFolder, string destinationFolder)
        {

            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            string[] files = Directory.GetFiles(sourceFolder);

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);

                string destPath = Path.Combine(destinationFolder, fileName);

                System.IO.File.Move(filePath, destPath);
            }
        }
    }
}
