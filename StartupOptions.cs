using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ziOS
{
    public class StartupOptions
    {
        public static void Entry()
        {
            switch (tuldrv.MenuNoGraphics(new string[] { "Disable File system", "Disable the OS" }))
            {
                case 0:
                    tuldrv.DisableFS = true;
                    break;
                case 1:
                    while (true) { Console.WriteLine("ok"); }
            }

        }
    }
}
