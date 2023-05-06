using System;
using System.Collections.Generic;
using System.Text;
using ziOS.Libs;

namespace ziOS.ziSH
{
    public class Prompt
    {
        public static string ShowPrompt()
        {
            LibTerm.ForegroundC = PrismGraphics.Color.LightPurple;
            LibTerm.Write(tuldrv.Syspath);
            LibTerm.ForegroundC = PrismGraphics.Color.White;
            LibTerm.Write("-");
            LibTerm.ForegroundC = PrismGraphics.Color.StackOverflowOrange;
            LibTerm.Write(")");
            LibTerm.ForegroundC = PrismGraphics.Color.White;
            return libinput.ReadShellLine();
        }
    }
}
