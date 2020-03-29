using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic;

namespace GameBacklogSelector.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // TODO: Add a way to mark a game as complete
            // TODO: Add a way to mark a game as in progress (or now playing)
            // TODO: Store results from complete and now playing
            // TODO: Add a way to skip games that are marked as complete or now playing
            // TODO: Add a way to exclude files (blacklist) from file
            // TODO: Add a way to exclude directories from file
            // TODO: Add message "You are currently playing X games" and list the game titles
            // TODO: Add a message "You have completed X games" and list the game titles
            // TODO: Find a way to get the game's title and box art to display (for UI, not console app)

            if (args == null) throw new ArgumentNullException(nameof(args));
            if (args.Length == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(args));

            // Parse out the command line arguments and create a list of directories to search
            var directories = args.ToList();
            var gameInfoList = new List<KeyValuePair<string, string>>();

            // Loop through the list of search directories            
            foreach (var rootDirectory in directories)
            {
                foreach (var directory in Directory.EnumerateDirectories(rootDirectory))
                {
                    // Create a DirectoryInfo object to process the root directory
                    Stack<string> dirs = new Stack<string>();
                    dirs.Push(directory);

                    while (dirs.Count > 0)
                    {
                        var currentDir = dirs.Pop();

                        if ((File.GetAttributes(currentDir) & FileAttributes.Hidden) == FileAttributes.Hidden ||
                            (File.GetAttributes(currentDir) & FileAttributes.System) == FileAttributes.System)
                        {
                            continue;
                        }

                        if (currentDir.Contains("downloading", StringComparison.OrdinalIgnoreCase) ||
                            currentDir.Contains("shadercache", StringComparison.OrdinalIgnoreCase) ||
                            currentDir.Contains("temp", StringComparison.OrdinalIgnoreCase) ||
                            currentDir.Contains(@"\Mono\", StringComparison.OrdinalIgnoreCase) ||
                            currentDir.Contains(@"StreamingAssets", StringComparison.OrdinalIgnoreCase) ||
                            currentDir.Contains(@"_windows", StringComparison.OrdinalIgnoreCase) ||
                            currentDir.Contains(@"\jre\", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        var subdirectories = Directory.GetDirectories(currentDir);

                        string[] files = null;
                        files = Directory.GetFiles(currentDir, "*.exe");

                        foreach (var file in files)
                        {
                            // TODO: Put these files in a "blacklist" config file
                            if (file.Contains("setup", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("install", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("crash", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("launcher", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("java", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("bootstrap", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("vc_redist", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("wow_helper", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("smcs", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("vcredist", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("dotNetFx40", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("CefSharp", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("mono-", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("gacutil", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("csharp", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("sql", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("svcutil", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("xsd", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("wsdl", StringComparison.OrdinalIgnoreCase) ||
                                file.Contains("xbuild", StringComparison.OrdinalIgnoreCase) ||
                                file.Equals("lc.exe") || file.Equals("Application-steam-x32.exe") ||
                                file.Equals("UnrealCEFSubProcess.exe") || file.Equals("breakpad_server.exe"))
                                continue;

                            if ((File.GetAttributes(file) & FileAttributes.Hidden) == FileAttributes.Hidden ||
                                (File.GetAttributes(file) & FileAttributes.System) == FileAttributes.System)
                            {
                                continue;
                            }

                            var fi = new FileInfo(file);
                            // System.Console.WriteLine($"{fi.Name} ({fi.DirectoryName})");
                            gameInfoList.Add(new KeyValuePair<string, string>(fi.Name, fi.DirectoryName));
                        }

                        foreach (var str in subdirectories)
                            dirs.Push(str);
                    }
                }
            }

            var random = new Random();
            var index = random.Next(gameInfoList.Count);

            System.Console.WriteLine();
            System.Console.WriteLine("================================================");
            System.Console.WriteLine($"You have a total of {gameInfoList.Count} games installed");
            System.Console.WriteLine($"Your next game is {gameInfoList[index]}, enjoy!");
            System.Console.WriteLine("================================================");
            System.Console.WriteLine();
            System.Console.WriteLine("FINISHED");
            System.Console.ReadLine();
        }
    }
}