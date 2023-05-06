using Cosmos.Core.Memory;
using Cosmos.System;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ziOS.Apps;
using ziOS.Libs;

namespace ziOS.ziSH
{
    public class Commands
    {
        public static void Help()
        {
            LibTerm.WriteLine("List of commands:\nThe '!' means it's an optional argument\n");
            LibTerm.WriteLine("help - Displays this menu.");
            LibTerm.WriteLine("shutdown or - [!r] - If arg isn't given, it will shutdown, if the r flag is '-r', it will restart.");
            LibTerm.WriteLine("clear, cls, or clr - Clear the screen.");
            LibTerm.WriteLine("echo [!message] - If arg isn't given, it will print a newline, if you input any message, it will echo it back.");
            LibTerm.WriteLine("ls, dir, or $ - List directories and files in the current directory.");
            LibTerm.WriteLine("fread or # [file] [!x] - If arg isn't given, it will output file, if the x flag is '-x', it will output file in hex.");
            LibTerm.WriteLine("ver - Shows version numbers of ziOS and included software");
            LibTerm.WriteLine("mkdir [name] - Creates a directory with the specified name.");
            LibTerm.WriteLine("cd or @ [folder] - Change current directory. What did you think it would do?");
            LibTerm.WriteLine("cd.. or @.. - change to parent directory");
            LibTerm.WriteLine("fwrite [file] [message] - Writes a file with the name of [file], with the contents of [message].");
            LibTerm.WriteLine("rm [obj] [!r] - if [r] is empty, it will delete a file [obj], if it's -r, then it will delete a folder [obj].");
            LibTerm.WriteLine("ziv [file] - Port of ziv text editor to ziOS");
            LibTerm.WriteLine("ringfetch - Cheap neofetch clone");
            LibTerm.WriteLine("screensaver - Shows a DVD-screensaver-style screensaver. Press shift to stop.");
            LibTerm.WriteLine("setres [width] [height] - Sets display resolution.");
            LibTerm.WriteLine("zish [script] - Runs a shell script with the ziSH command interpreter.");
            LibTerm.WriteLine("termtest - text rendering tests for libterm");
        }

        public static void shutdown(string[] args)
        {
            if (args.Length == 2 && args[1] == "-r")
            {
                LibTerm.WriteLine("Hasta Luego!");
                Cosmos.System.Power.Reboot();
            }
            else if (args.Length == 1)
            {
                LibTerm.WriteLine("Adios!");
                Cosmos.System.Power.Shutdown();
            }
        }

        public static void Echo(string[] args)
        {
            if (args.Length == 1)
            {
                LibTerm.WriteLine();
            }
            else
            {
                var arglist = args.ToList();
                arglist.RemoveAt(0);
                LibTerm.WriteLine(string.Join(' ', arglist.ToArray()));
            }
        }

        public static void ver()
        {
            LibTerm.WriteLine(Kernel.BuildString); // add when new software gets bundled
            LibTerm.WriteLine("ziv " + ziOS.Apps.ziv.Program.version);
        }

        public static void ringfetch()
        {
            int oldy = LibTerm.CursorY;
            Kernel.canvas.DrawImage(0, LibTerm.CursorY, Kernel.Logo);
            LibTerm.CursorX = 200;
            LibTerm.Write(Kernel.BuildString);
            LibTerm.CursorX = 200;
            LibTerm.CursorY += 16;
            LibTerm.Write("CPU: " + Cosmos.Core.CPU.GetCPUBrandString());
            LibTerm.CursorX = 200;
            LibTerm.CursorY += 16;
            LibTerm.Write("RAM: " + Cosmos.Core.GCImplementation.GetUsedRAM() / 1000000 + "/" + Cosmos.Core.GCImplementation.GetAvailableRAM() + " MB");
            LibTerm.CursorX = 200;
            LibTerm.CursorY += 16;
            if (VMTools.IsVMWare)
            {
                LibTerm.Write("Virtualized: VMware");
            }
            else if (VMTools.IsQEMU)
            {
                LibTerm.Write("Virtualized: QEMU/KVM");
            }
            else if (VMTools.IsVirtualBox)
            {
                LibTerm.Write("Virtualized: VirtualBox");
            }
            else
            {
                LibTerm.Write("Virtualized: No (how???)");
            }
            LibTerm.CursorX = LibTerm.CRLeftMargin;
            LibTerm.CursorY = oldy + 65;
        }
        public static void setres(string[] args)
        {
            if (args.Length != 3)
            {
                tuldrv.Error("Invalid use of SETRES command.", "SETRES");
            } else
            {
                Kernel.canvas.Width = ushort.Parse(args[1]);
                Kernel.canvas.Height = ushort.Parse(args[2]);
                Kernel.Scrwidth = short.Parse(args[1]);
                Kernel.Scrheight = short.Parse(args[2]);
                LibTerm.Clear();
            }
        }

        public static void zish(string[] args)
        {
            if (args.Length != 2)
            {
                tuldrv.Error("Invalid use of ZISH command.", "ZISH");
            } else
            {
                tuldrv.ParseShellScript(tuldrv.Syspath + args[1]);
            }
        }
    }
    public class CmdHandler
    {
        public static void Handle(string s)
        {
            tuldrv.CommandHistory.Add(s);
            string[] args = s.Split(' ');
            // remember to also add to tab completion list in libinput.cs
            if (args[0] == "help") { Commands.Help(); }
            else if (args[0] == "shutdown" || args[0] == "-") { Commands.shutdown(args); }
            else if (args[0] == "clear" || args[0] == "cls" || args[0] == "clr") { LibTerm.Clear(); }
            else if (args[0] == "echo") { Commands.Echo(args); }
            else if (args[0] == "ls" || args[0] == "dir" || args[0] == "$") { FScmds.LS(); }
            else if (args[0] == "fread" || args[0] == "#") { FScmds.cat(args); }
            else if (args[0] == "ver") { Commands.ver(); }
            else if (args[0] == "mkdir") { FScmds.mkdir(args); }
            else if (args[0] == "cd" || args[0] == "@") { FScmds.CD(args); }
            else if (args[0] == "cd.." || args[0] == "@..") { FScmds.CDdotdot(); }
            else if (args[0] == "fwrite") { FScmds.Fwrite(args); }
            else if (args[0] == "rm") { FScmds.remove(args); }
            else if (args[0] == "ziv") { ziOS.Apps.ziv.Program.Main(args); }
            else if (args[0] == "ringfetch") { Commands.ringfetch(); }
            else if (args[0] == "screensaver") { Screensaver.Entry(args); }
            else if (args[0] == "setres") { Commands.setres(args); }
            else if (args[0] == "zish") { Commands.zish(args); }
            else if (args[0] == "termtest") { LibTerm.TestTerminal(); }
            else if (args[0] == "watercan") { Kernel.canvas.DrawImage(LibTerm.CursorX, LibTerm.CursorY, Kernel.Wateringcan); LibTerm.CursorY += 359; }
            else if (string.IsNullOrEmpty(args[0])) { }
            else
            {
                tuldrv.Error("The command \"" + args[0] + "\" is not part of the command dictionary.", "ziSH");
            }
        }
    }
}
