using System;
using System.IO;
using ziOS;
using ziOS.Libs;

namespace ziOS.Apps.ziv
{
    public class Editor
    {
        public static PrismGraphics.Color[] colors = { PrismGraphics.Color.GoogleBlue, PrismGraphics.Color.GoogleRed, PrismGraphics.Color.GoogleGreen, PrismGraphics.Color.GoogleYellow };
        public static void Entry(string file)
        {
            Kernel.canvas.Clear(PrismGraphics.Color.Black);
            LibTerm.CursorX = 0; LibTerm.CursorY = 0;
            PrismGraphics.Color fore = colors[Program.rng.Next(0, colors.Length - 1)];
            LibTerm.ForegroundC = fore;
            LibTerm.Write("ziv - " + file + " - Type $-wq on 1 line to save and quit.");
            Kernel.canvas.DrawFilledRectangle(LibTerm.CursorX, LibTerm.CursorY, (ushort)(Kernel.Scrwidth - LibTerm.CursorX), 16, 0, fore);
            LibTerm.WriteLine("\n");
            LibTerm.ForegroundC = PrismGraphics.Color.White;
            MainLoop(file);
        }

        public static void MainLoop(string file)
        {
            LibTerm.Write(Program.buffer);
            while (true)
            {
                string Buffer2 = libinput.ReadstdLine();
                if (Buffer2 == "$-wq") // Write, then quit
                {
                    LibTerm.Clear();
                    LibTerm.WriteLine("Saving file to disk...");
                    File.WriteAllText(file, Program.buffer);
                    LibTerm.WriteLine("I finished! Have a good day/night!");
                    break;
                }
                else if (Buffer2 == "$-q") // quit
                {
                    LibTerm.Clear();
                    break;
                }
                else if (Buffer2 == "$-waq") // Write as, then quit
                {
                    LibTerm.Clear();
                    LibTerm.Write("Where do you want to save this text? (Don't do a full path) ");
                    string dest = libinput.ReadstdLine();
                    LibTerm.WriteLine("Saving file to disk...");
                    File.WriteAllText(tuldrv.Syspath + dest, Program.buffer);
                    LibTerm.WriteLine("I finished! Have a good day/night!");
                    break;
                }
                else
                {
                    Program.buffer += Buffer2 + "\n";
                }
            }
        }
    }
}