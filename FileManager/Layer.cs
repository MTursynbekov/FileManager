using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileManager
{
    class Layer
    {
        public FileSystemInfo[] Content
        {
            get;
            set;
        }

        public int SelectedItem
        {
            get;
            set;
        }

        public void Draw()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            for (int i = 0; i < Content.Length; ++i)
            {
                if (i == SelectedItem)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write(Content[i].Name);

                    if (Content[i].GetType() == typeof(FileInfo))
                    {
                        FileInfo file = (FileInfo)Content[i];
                        Console.WriteLine("             " + file.Length + " Bytes");
                    }
                    else
                    {
                        DirectoryInfo dir = (DirectoryInfo)Content[i];
                        Console.WriteLine("             " + DirSize(dir) + " Bytes");
                    }
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine(Content[i].Name);
                }
            }
        }

        public void ScrollUp()
        {
            if (SelectedItem <= 0)
            {
                SelectedItem = Content.Length - 1;
            }
            else
            {
                SelectedItem--;
            }
        }

        public void ScrollDown()
        {
            if (SelectedItem >= Content.Length -1)
            {
                SelectedItem = 0;
            }
            else
            {
                SelectedItem++;
            }
        }

        public void GoBegin()
        {
            SelectedItem = 0;
        }

        public void GoEnd()
        {
            SelectedItem = Content.Length - 1;
        }

        public static long DirSize(DirectoryInfo dir)
        {
            long size = 0;
            
            FileInfo[] fis = dir.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }
            
            DirectoryInfo[] dis = dir.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }
            return size;
        }

    }
}
