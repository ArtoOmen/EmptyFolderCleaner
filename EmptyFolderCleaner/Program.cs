using System;
using System.Collections.Generic;
using System.IO;

namespace EmptyFolderCleaner
{
    public class Program
    {
        static void Main(string[] args)
        {
            var path = Environment.CurrentDirectory;
            Console.WriteLine(path);

            if (Array.IndexOf(args, "-test") > -1) 
            {
                //Если в агрументах прописан ключ "-test".
                CreateTestDirectories(path);
                return;
            }

            var emptyDirectories = new List<string>();
            do
            {
                emptyDirectories.Clear();
                FillEmptyDirectoriesRecursive(path, emptyDirectories);
                foreach (var dir in emptyDirectories)
                {
                    var dirInfo = new DirectoryInfo(dir);
                    dirInfo.Attributes = FileAttributes.Normal; //Для невидимых и read-only папок.
                    Directory.Delete(dir, recursive: true);
                    Console.WriteLine(dir);
                }
            } while (emptyDirectories.Count != 0);
        }

        static void CreateTestDirectories(string path)
        {
            Directory.CreateDirectory(Path.Combine(path, "1"));
            Directory.CreateDirectory(Path.Combine(path, "1", "1.1"));
            Directory.CreateDirectory(Path.Combine(path, "1", "1.1", "1.2"));
            Directory.CreateDirectory(Path.Combine(path, "2"));

            var dir2Info = new DirectoryInfo(Path.Combine(path, "2"));
            dir2Info.Attributes = FileAttributes.Hidden;

            Directory.CreateDirectory(Path.Combine(path, "3"));
            Directory.CreateDirectory(Path.Combine(path, "3", "3.1"));
            Directory.CreateDirectory(Path.Combine(path, "4"));
            Directory.CreateDirectory(Path.Combine(path, "4", "4.1"));

            var dir4Info = new DirectoryInfo(Path.Combine(path, "4"));
            dir4Info.Attributes = FileAttributes.Hidden;

            Directory.CreateDirectory(Path.Combine(path, "5"));
            Directory.CreateDirectory(Path.Combine(path, "5", "5.1"));

            var dir5Info = new DirectoryInfo(Path.Combine(path, "5", "5.1"));
            dir5Info.Attributes = FileAttributes.Hidden;

            Directory.CreateDirectory(Path.Combine(path, "6"));
            File.CreateText(Path.Combine(path, "6", "6.txt")).Close();

        }

        static void FillEmptyDirectoriesRecursive(string path, List<string> emptyDirectories)
        {
            if (IsEmpty(path))
            {
                emptyDirectories.Add(path);
                return;
            }
            var subdirectories = Directory.GetDirectories(path);
            foreach (var subdir in subdirectories)
            {
                FillEmptyDirectoriesRecursive(subdir, emptyDirectories);
            }

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
    }
}