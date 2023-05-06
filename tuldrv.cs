using Cosmos.Core.Memory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ziOS.Libs;
using ziOS.ziSH;

namespace ziOS
{
    public class tuldrv
    {
        public static string Syspath = "0:\\";

        public static List<string> CommandHistory = new List<string>();

        public static List<string> SyscallContext = new List<string>();
        public static List<string> SyscallOut = new List<string>();

        public static bool DisableFS = false;

        /// <summary>
        /// Displays text in VGA text mode with color using one statement.
        /// </summary>
        /// <param name="s">The text you want to color.</param>
        /// <param name="c">The color you want the text to be colored in</param>
        public static void ColorTextMode(string s, ConsoleColor c)
        {
            ConsoleColor oldc = Console.ForegroundColor;
            Console.ForegroundColor = c;
            Console.Write(s);
            Console.ForegroundColor = oldc;
        }

        /// <summary>
        /// A yes/no menu.
        /// </summary>
        /// <returns>Boolean saying if user said Y or N.</returns>
        public static bool YNmenu()
        {
            while (true)
            {
                var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.N)
                    return false;

                if (key.Key == ConsoleKey.Y)
                    return true;
            }
        }
        /// <summary>
        /// choosing menu. 9 MAX.
        /// </summary>
        /// <param name="Options">The options you want the user to choose from.</param>
        /// <returns>What the user chose.</returns>
        public static byte MenuNoGraphics(string[] Options)
        {
            Console.WriteLine("Please choose a number from 0 to " + Options.Length);
            for (int i = 0; i < Options.Length; i++)
            {
                Console.WriteLine($"{i}. {Options[i]}");
            }
            while (true)
            {
                byte Outp = 255;
                var key = System.Console.ReadKey(true);
                byte.TryParse(key.KeyChar.ToString(), out Outp);
                if (Outp > Options.Length) 
                { 

                }
                else
                {
                    return Outp;
                }
            }
        }

        public static void KernelPanic(Exception e)
        {
            if (Kernel.canvas == null || Kernel.canvas.Width <= 0 || Kernel.canvas.Height <= 0)
            {
                Console.WriteLine("ziOS X Critical System Error");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.GetType().Name);
                Console.WriteLine("The system cannot continue.");
                while (true) { }
            }
            else
            {
                Kernel.canvas.Clear(PrismGraphics.Color.Black);
                LibTerm.UImode = false;
                LibTerm.WriteLine("       ..                     ziOS has CRASHed!");
                LibTerm.WriteLine("        .*+:          :       ");
                LibTerm.WriteLine("          +#*#*    -*%+       Error: " + e.Message);
                LibTerm.WriteLine("       .:  *%%%=..+%##:       ");
                LibTerm.WriteLine("       *%***:::+**##+:        ");
                LibTerm.WriteLine("        .-+=:#+****#-         If you continue to get this error even after a reboot, try these following tips:");
                LibTerm.WriteLine("     +=+=++**+=-:-=*#-        ");
                LibTerm.WriteLine("     -++#******++=***#        1. Disable or remove any hardware that is not necessary.");
                LibTerm.WriteLine("         .-++=---=***#:       This can include USB/PCI devices. Networking is not available yet.");
                LibTerm.WriteLine("         .++=--*#***##.       2. Disable any BIOS options that modern operating systems might use.");
                LibTerm.WriteLine("        .**###%###**#=        Because this OS is about on par with ancient Linux.");
                LibTerm.WriteLine("       -***##%%%**##=         3. If you need to disable a feature, such as the file system, restart");
                LibTerm.WriteLine("        --:==######-          your computer and hold shift, then choose an option and ziOS will start in that mode.");
                LibTerm.WriteLine("             ++**#:           4. go back to 1992 linux and fix os youself >:]");
                LibTerm.WriteLine("             ==**+            ");
                LibTerm.WriteLine("            :**+*#            ");
                LibTerm.WriteLine("           :##+=+#.           ");
                LibTerm.WriteLine("          :+##+=*#:           ");
                LibTerm.WriteLine("         :=*%#+=+*=           Hope you don't have to meet crash in this OS again!");
                LibTerm.WriteLine("         -++#%*==*#-          ");
                LibTerm.WriteLine("         =++#%**++*#          ");
                LibTerm.WriteLine("          .:## :+**:          Error type: " + e.GetType().Name);
                LibTerm.WriteLine("    ::.   -*##:.:=#=          ");
                LibTerm.WriteLine("  -##############%%%:         ");
                LibTerm.WriteLine(" :+++====*##########*         Crash Bandicoot lies in crashed OS.");
                LibTerm.WriteLine("          :       :=+=.       The system is halted.");
                while (true) { }
            }
        }

        public static void GenerateConfigFile()
        {
            string[] lines =
            {
                "// Default Hostname",
                "omniverse"
            };
            File.WriteAllLines(@"0:\zios.cfg",lines);
        }

        public static void Error(string s, string caller)
        {
            LibTerm.ForegroundC = PrismGraphics.Color.GoogleRed;
            LibTerm.WriteLine(caller + ": " + s);
            LibTerm.ForegroundC = PrismGraphics.Color.White;
        }

        public static void Success()
        {
            LibTerm.ForegroundC = PrismGraphics.Color.GoogleGreen;
            LibTerm.WriteLine("The operation completed successfully.");
            LibTerm.ForegroundC = PrismGraphics.Color.White;
        }

        /// <summary>
        /// Converts an input string into a hex string. It returns the hex string.
        /// </summary>
        /// <param name="input">What you want to convert to hex.</param>
        /// <returns></returns>
        public static string ConvertToHex(string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);

            string hexString = BitConverter.ToString(bytes);
            hexString = hexString.Replace("-", " ");
            return hexString;
        }

        public static void ParseShellScript(string path)
        {
            if (File.Exists(path))
            {
                string[] AutoExecLines = File.ReadAllLines(path);
                foreach (string AutoExecCmd in AutoExecLines)
                {
                    CmdHandler.Handle(AutoExecCmd);
                    Heap.Collect();
                }
            } else
            {
                throw new Exception("\"" + path +"\" doesn't exist");
            }
        }
    }
}
