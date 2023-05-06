using System;
using System.Collections.Generic;
using System.Text;
using Sys=Cosmos.System;
using System.Threading.Tasks;
using Cosmos.System.Graphics;
using System.IO;

namespace ziOS.Libs
{
    public class libinput
    {
        public static char ReadKey()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (char.IsLetterOrDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar) || key.KeyChar == ' ')
            {
                return key.KeyChar;
            }
            else if (key.Key == ConsoleKey.Backspace) { return '\b'; }
            else if (key.Key == ConsoleKey.Enter) { return '\n'; }
            else { return '\r'; }
        }
        public static string ReadShellLine()
        {
            LibTerm.UImode = true;
            string line = "";
            short CmdHistroyPtr = 0; // not really a pointer lmao
            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && line.Length > 0)
                {
                    line = line.Substring(0,line.Length - 1);
                    LibTerm.Write("\b");
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    LibTerm.WriteLine();
                    LibTerm.UImode = false;
                    break;
                }
                else if (key.Key == ConsoleKey.UpArrow && tuldrv.CommandHistory.Count != 0 && CmdHistroyPtr != tuldrv.CommandHistory.Count)
                {
                    while (line.Length > 0)
                    {
                        line = line.Substring(0, line.Length - 1);
                        LibTerm.Write("\b");
                    }
                    CmdHistroyPtr++;
                    line = tuldrv.CommandHistory[tuldrv.CommandHistory.Count-CmdHistroyPtr];
                    LibTerm.Write(line);
                }
                else if (key.Key == ConsoleKey.DownArrow && tuldrv.CommandHistory.Count != 0)
                {
                    if (CmdHistroyPtr == 0) // this check might be useless lmao
                    {
                        while (line.Length > 0)
                        {
                            line = line.Substring(0, line.Length - 1);
                            LibTerm.Write("\b");
                        }
                    }
                    else
                    {
                        while (line.Length > 0)
                        {
                            line = line.Substring(0, line.Length - 1);
                            LibTerm.Write("\b");
                        }
                        CmdHistroyPtr--;
                        line = tuldrv.CommandHistory[tuldrv.CommandHistory.Count-1 - CmdHistroyPtr];
                        LibTerm.Write(line);
                    }
                }
                else if (key.Key == ConsoleKey.Tab)
                {
                    string[] SplitLine = line.Split(' ');
                    if (SplitLine.Length == 1)
                    {
                        // god help me
                        if ("help".StartsWith(SplitLine[0])) 
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "help"; LibTerm.Write(line);
                        }
                        else if ("shutdown".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "shutdown"; LibTerm.Write(line);
                        }
                        else if ("clear".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "clear"; LibTerm.Write(line);
                        }
                        else if ("echo".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "echo"; LibTerm.Write(line);
                        }
                        else if ("ls".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "ls"; LibTerm.Write(line);
                        }
                        else if ("dir".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "dir"; LibTerm.Write(line);
                        }
                        else if ("fread".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "fread"; LibTerm.Write(line);
                        }
                        else if ("ver".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "ver"; LibTerm.Write(line);
                        }
                        else if ("mkdir".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "mkdir"; LibTerm.Write(line);
                        }
                        else if ("cd".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "cd"; LibTerm.Write(line);
                        }
                        else if ("cd..".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "cd.."; LibTerm.Write(line);
                        }
                        else if ("fwrite".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "fwrite"; LibTerm.Write(line);
                        }
                        else if ("rm".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "rm"; LibTerm.Write(line);
                        }
                        else if ("ziv".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "ziv"; LibTerm.Write(line);
                        }
                        else if ("ringfetch".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "ringfetch"; LibTerm.Write(line);
                        }
                        else if ("screensaver".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "screensaver"; LibTerm.Write(line);
                        }
                        else if ("setres".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "setres"; LibTerm.Write(line);
                        }
                        else if ("zish".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "zish"; LibTerm.Write(line);
                        }
                        else if ("termtest".StartsWith(SplitLine[0]))
                        {
                            while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                            line = "termtest"; LibTerm.Write(line);
                        }
                        else
                        {
                            Console.Beep(250,100);
                        }
                    } else if (SplitLine.Length == 2)
                    {
                        if (!tuldrv.DisableFS)
                        {
                            string[] Files = Directory.GetFiles(tuldrv.Syspath);
                            bool filefound = false;
                            foreach(string File in Files)
                            {
                                if (File.StartsWith(SplitLine[1]))
                                {
                                    filefound = true;
                                    string BaseCmd = SplitLine[0];
                                    while (line.Length > 0) { line = line.Substring(0, line.Length - 1); LibTerm.Write("\b"); }
                                    line = BaseCmd + " " + File; LibTerm.Write(line);
                                }
                            }
                            if (!filefound)
                            {
                                Console.Beep(250, 100);
                            }
                        }
                        else
                        {
                            Console.Beep(250, 100);
                        }
                    }
                    else
                    {
                        Console.Beep(250, 100);
                    }
                }

                /*else if (key.Key == ConsoleKey.Tab) who uses this
                {
                    line += "    ";
                    LibTerm.Write("\t");
                }*/
                else if (char.IsLetterOrDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar) || char.IsSymbol(key.KeyChar) || key.KeyChar == ' ')
                {
                    line += key.KeyChar;
                    LibTerm.Write(key.KeyChar.ToString());
                }
            }

            return line;
        }

        public static string ReadstdLine()
        {
            LibTerm.UImode = true;
            string line = "";
            ConsoleKeyInfo key;

            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace && line.Length > 0)
                {
                    line = line.Substring(0, line.Length - 1);
                    LibTerm.Write("\b");
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    LibTerm.WriteLine();
                    LibTerm.UImode = false;
                    break;
                }
                else if (char.IsLetterOrDigit(key.KeyChar) || char.IsPunctuation(key.KeyChar) || char.IsSymbol(key.KeyChar) || key.KeyChar == ' ')
                {
                    line += key.KeyChar;
                    LibTerm.Write(key.KeyChar.ToString());
                }
            }

            return line;
        }
    }
}
