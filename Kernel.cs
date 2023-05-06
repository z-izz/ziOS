using System;
using System.Collections.Generic;
using System.Text;
using Sys = Cosmos.System;
using PrismGraphics;
using System.Threading;
using ziOS.Libs;
using IL2CPU.API.Attribs;
using ziOS.ziSH;
using System.IO;
using Cosmos.Core.Memory;

namespace ziOS
{
    public class Kernel : Sys.Kernel
    {
        // VERSION INFORMATION
        public static readonly int BuildNum = 230506;
        public static readonly string BuildString = "ziOS 2.2.0.4 [Build " + BuildNum + "]";
        // version scheme is REWRITE.MAJOR.MINOR.PATCH: ADD TO PATCH WHEN YOU PATCH, MINOR WHEN YOU ADD NEW FEATURE. MAJOR WHEN MINOR REACHES 16. REWRITE WHEN YOU REWRITE OR WHEN MAJOR REACHES 30.

        // CRITICAL VARIABLES
        public static PrismGraphics.Extentions.VMWare.SVGAIICanvas canvas;
        public static short Scrwidth = 1024, Scrheight = 768;
        Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();

        // RESOURCES INIT
        [ManifestResourceStream(ResourceName = "ziOS.Resources.zios_logo.bmp")]
        static byte[] LogoBytes;
        public static Image Logo;

        [ManifestResourceStream(ResourceName = "ziOS.Resources.wateringcan.bmp")]
        static byte[] WateringCanBytes;
        public static Image Wateringcan;

        // CONFIGURABLE INFO
        public static string Hostname = "omniverse";

        protected override void BeforeRun()
        {
            try
            {
                Console.Clear();
                Console.Write("Welcome to ");
                tuldrv.ColorTextMode(BuildString + "\n", ConsoleColor.Cyan);

                Console.Write("Hold shift NOW to get advanced startup options");
                Thread.Sleep(666);
                Console.Write(".");
                Thread.Sleep(666);
                Console.Write(".");
                Thread.Sleep(666);
                Console.WriteLine(".");
                if (Sys.KeyboardManager.ShiftPressed)
                {
                    StartupOptions.Entry();
                }

                if (!Sys.VMTools.IsVMWare)
                {
                    //                |                                                                                |
                    Console.WriteLine("That's an X in my book!\n");
                    Console.WriteLine("You are running ziOS on REAL HARDWARE! Or you aren't using VMWare, why though?");
                    Console.WriteLine("There is about a 110% chance that ziOS will burn into flames on this computer.");
                    Console.WriteLine("That can include performance or stability issues, and/or even data loss.");
                    Console.WriteLine("Do you want to continue? (Y/N)");
                    if (!tuldrv.YNmenu()) { Sys.Power.Shutdown(); }
                }

                // START OF "CANVAS LAYER"
                canvas = new((ushort)Scrwidth,(ushort)Scrheight);
                if (canvas == null || canvas.Width <= 0 || canvas.Height <= 0) { throw new Exception("Canvas init didn't really do anything."); }
                Logo = Image.FromBitmap(LogoBytes);
                canvas.Clear(Color.Black);
                canvas.DrawImage(0,15,Logo);
                canvas.Update();
                LibTerm.SetCursorPos(0, 90);
                //            |                           |
                LibTerm.Write("Initializing canvas...     ");
                LibTerm.ColorWrite("[OK]\n", Color.GoogleGreen);
                if (!tuldrv.DisableFS)
                {
                    //            |                           |
                    LibTerm.Write("Initializing FS...         ");
                    try
                    {
                        Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
                        LibTerm.ColorWrite("[OK]\n", Color.GoogleGreen);
                    }
                    catch (Exception e)
                    {
                        LibTerm.ColorWrite("[FAIL: " + e.Message + "]\n", Color.GoogleRed);
                    }

                    //            |                           |
                    LibTerm.Write("Parsing autoexec.zis...  ");
                    try
                    {
                        tuldrv.ParseShellScript("0:\\autoexec.zis");
                        LibTerm.ColorWrite("[OK]\n", Color.GoogleGreen);
                        canvas.Clear(Color.Black);
                        canvas.DrawImage(0, 15, Logo);
                        canvas.Update();
                        LibTerm.SetCursorPos(0, 90);
                    }
                    catch (Exception e)
                    {
                        LibTerm.ColorWrite("[FAIL: " + e.Message + "]\n", Color.GoogleRed);
                    }
                }

                //            |                           |
                LibTerm.Write("Initializing Bitmaps...    ");
                try
                {
                    Wateringcan = Image.FromBitmap(WateringCanBytes);
                    LibTerm.ColorWrite("[OK]\n", Color.GoogleGreen);
                }
                catch (Exception e)
                {
                    LibTerm.ColorWrite("[FAIL: " + e.Message + "]\n", Color.GoogleRed);
                }

                LibTerm.WriteLine();
                LibTerm.WriteLine(BuildString + " " + Hostname + " svga\n"); // ADD HOST/USERNAMES!!!
            }
            catch (Exception e)
            {
                tuldrv.KernelPanic(e);
            }
        }

        protected override void Run()
        {
            try
            {
                string Command = Prompt.ShowPrompt();
                CmdHandler.Handle(Command);
                Heap.Collect();
            } catch (Exception e)
            {
                tuldrv.KernelPanic(e);
            }
        }
    }
}
