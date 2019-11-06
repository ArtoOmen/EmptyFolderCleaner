using System;
using System.IO;

namespace EmptyFolderCleaner
{
    public class Program
    {
        static void Main(string[] args)
        {
            var path = Environment.CurrentDirectory;
            Console.WriteLine(path);
            var children = Directory.GetDirectories(path);
            PrintEmptyDirectoriesRecursive(path);

        }
        static bool IsEmpty(string path)
        {
            if (!Directory.Exists(path))
                return false;

            if (Directory.GetDirectories(path).Length > 0)
                return false;

            if (Directory.GetFiles(path).Length == 1)
            {
                var filename = Directory.GetFiles(path)[0];
                if (Path.GetFileName(filename) == "desktop.ini")
                    return true;
            }
            
            if (Directory.GetFiles(path).Length > 0)
                return false;



            return true;

        }
        static void PrintEmptyDirectoriesRecursive(string path)
        {
            if (IsEmpty(path))
            {
                Console.WriteLine(path);
                return;
            }
            var subdirectories = Directory.GetDirectories(path);
            foreach (var subdir in subdirectories)
            {
                PrintEmptyDirectoriesRecursive(subdir);
            }

        }
    }
}