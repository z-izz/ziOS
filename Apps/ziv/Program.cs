using System;
using System.IO;
using ziOS;
using ziOS.Libs;

namespace ziOS.Apps.ziv
{
    public class Program
    {
        public static Random rng = new Random(); // Global random object
        public static string version = "Milestone 1 (ziOS port)"; // Version string
        public static string buffer = ""; // Text buffer
        public static void Main(string[] args) // Runner, sets up the buffer
        {
            if (tuldrv.DisableFS)
            {
                tuldrv.Error("The file system is disabled.", "ziv");
            }
            else
            {
                try
                {
                    buffer = "";
                    bool ActuallyGoIntoEditor = true;
                    if (args.Length == 2) // If one argument is passed
                    {
                        if (File.Exists(tuldrv.Syspath + args[1])) // If the first argument exists as a file
                        {
                            buffer = File.ReadAllText(tuldrv.Syspath + args[1]); // Setup the buffer
                        }
                        else
                        {
                            if (args[1] == "--version" || args[1] == "-v")
                            {
                                LibTerm.WriteLine($"ziv " + version);
                                ActuallyGoIntoEditor = false;
                            }
                            else
                            {
                                LibTerm.WriteLine("ziv says: No files? I'll make one then!");
                            }
                        }
                        if (ActuallyGoIntoEditor)
                            Editor.Entry(tuldrv.Syspath + args[1]); // Call the actual ziv editor
                    }
                    else
                    {
                        LibTerm.WriteLine("ziv says: It would be really nice if actually gave me a file to edit.");
                    }
                }
                catch (Exception e)
                {
                    LibTerm.WriteLine("Oops! ziv has crashed! Sorry for the inconvenience!\n");
                    LibTerm.WriteLine("Error: " + e.Message);
                    LibTerm.WriteLine($"Error type: " + e.GetType().Name);
                }
            }
        }
    }
}