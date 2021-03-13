using System;
using System.Collections.Generic;
using System.IO;

namespace FileManager
{
    enum ViewMode
    {
        ShowDirContent,
        ShowFileContent
    }

    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo dir = new DirectoryInfo(@"G:\Games\2term\PP2\Labs");
            Stack<Layer> history = new Stack<Layer>();
            history.Push(
                new Layer
                {
                    Content = dir.GetFileSystemInfos()
                }
            );

            ViewMode viewMode = ViewMode.ShowDirContent;
            bool esc = false;
            while (!esc)
            {
                if (viewMode == ViewMode.ShowDirContent)
                {
                    history.Peek().Draw();
                }
                ConsoleKeyInfo consoleKeyInfo = Console.ReadKey();
                switch (consoleKeyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        history.Peek().ScrollUp();
                        break;

                    case ConsoleKey.DownArrow:
                        history.Peek().ScrollDown();
                        break;

                    case ConsoleKey.Enter:
                        int index = history.Peek().SelectedItem;
                        FileSystemInfo fsi = history.Peek().Content[index];

                        if (fsi.GetType() == typeof(DirectoryInfo))
                        {
                            viewMode = ViewMode.ShowDirContent;
                            DirectoryInfo selectedDir = fsi as DirectoryInfo;
                            history.Push(new Layer { Content = selectedDir.GetFileSystemInfos() });
                        }
                        else
                        {
                            viewMode = ViewMode.ShowFileContent;
                            using (FileStream fs = new FileStream(fsi.FullName, FileMode.Open, FileAccess.Read))
                            {
                                using (StreamReader sr = new StreamReader(fs))
                                {
                                    Console.BackgroundColor = ConsoleColor.White;
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Black;
                                    Console.WriteLine(sr.ReadToEnd());
                                }
                            }
                        }
                        break;

                    case ConsoleKey.Delete:
                        int index2 = history.Peek().SelectedItem;
                        FileSystemInfo fsi2 = history.Peek().Content[index2];
                        history.Peek().SelectedItem--;

                        if (fsi2.GetType() == typeof(DirectoryInfo))
                        {
                            DirectoryInfo directoryInfo = fsi2 as DirectoryInfo;
                            Directory.Delete(fsi2.FullName, true);
                            history.Peek().Content = directoryInfo.Parent.GetFileSystemInfos();
                        }
                        else
                        {
                            FileInfo fileInfo = fsi2 as FileInfo;
                            File.Delete(fsi2.FullName);
                            history.Peek().Content = fileInfo.Directory.GetFileSystemInfos();
                        }
                        break;

                    case ConsoleKey.Backspace:
                        if (viewMode == ViewMode.ShowDirContent)
                        {
                            history.Pop();
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.White;
                            viewMode = ViewMode.ShowDirContent;
                        }
                        break;

                    case ConsoleKey.Home:
                        history.Peek().GoBegin();
                        break;

                    case ConsoleKey.End:
                        history.Peek().GoEnd();
                        break;

                    case ConsoleKey.Escape:
                        esc = true;
                        break;
                }
            }
        }
    }
}
