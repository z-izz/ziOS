using Cosmos.System.Graphics;
using PrismGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ziOS.Libs
{
    public class LibTerm
    {
        public static int CursorX = 0, CursorY = 0;
        /// <summary>
        /// Foreground color.
        /// </summary>
        public static PrismGraphics.Color ForegroundC = PrismGraphics.Color.White;
        /// <summary>
        /// The font that should be used.
        /// </summary>
        public static PrismGraphics.Fonts.Font Termfont = PrismGraphics.Fonts.Font.Fallback;
        public static int maxX = Kernel.Scrwidth, maxY = Kernel.Scrheight;
        /// <summary>
        /// Carridge return left margin, basically where should lines start horizontally.
        /// </summary>
        public static int CRLeftMargin = 0;
        /// <summary>
        /// Set this to true for backspaces to work properly, set to false when you are not using readlines, as it controls the spaces between letters which makes them look better
        /// </summary>
        public static bool UImode = false;
        /// <summary>
        /// Write text.
        /// </summary>
        /// <param name="s">String you want to be written.</param>
        public static void Write(string s)
        {
            foreach (char c in s)
            {
                if (c == '\r') { break; }
                else if (c == '\n') { if (CursorY + 32 > maxY) { Clear(); } CursorY += 16; CursorX = CRLeftMargin; } // NEWLINE
                else if (c == '\t') { CursorX += 32; }
                else if (c == '\b') // BACKSPACE
                { 
                    if (CursorX == CRLeftMargin)
                    {
                        break;
                    }
                    CursorX -= 8; // add exceptions to this rule
                    Kernel.canvas.DrawFilledRectangle(CursorX, CursorY, 8, 16, 0, Color.Black); // IMPLEMENT LIBTERM.BACKGROUNDC
                }
                else
                {
                    Kernel.canvas.DrawString(CursorX, CursorY, c.ToString(), Termfont, ForegroundC);
                    if (CursorX+16 > maxX) { CursorY += 16; CursorX = CRLeftMargin; }
                    if (UImode)
                    {
                        CursorX += 8;
                    }
                    else
                    {
                        if (c == 'i' || c == 't' || c == 'l' || c == 'I' || c == 'j' || c=='[' || c==']' || c=='!' || c=='.' || c=='|' || c=='`' || c==';' || c==':' || c=='\'' || c==',') { CursorX += 4; }
                        else if (c == 'r' || c == '/' || c == 'J' || c == 'f' || c == '{' || c == '}' || c=='k' || c == '*' || c=='"' || c=='\\') { CursorX += 6; }
                        else if (c == 'm' || c == 'M' || c == 'Q') { CursorX += 10; }
                        else if (c == 'w' || c == 'W' || c=='@') { CursorX += 12; }
                        else
                        {
                            CursorX += 8;
                        }
                    }
                }
            }
            Kernel.canvas.Update();
        }
        /// <summary>
        /// Write text, then add a \n to it!
        /// </summary>
        /// <param name="s">String you want to be written.</param>
        public static void WriteLine(string s)
        {
            Write(s + '\n');
        }
        public static void WriteLine()
        {
            Write("\n");
        }
        /// <summary>
        /// Set cursor position
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public static void SetCursorPos(int x, int y) { CursorX = x; CursorY = y; }
        /// <summary>
        /// Reset cursor pos to 0,0
        /// </summary>
        public static void ResetCursors() { CursorX = 0; CursorY = 122; }

        /// <summary>
        /// Just a quick test of the Terminal.
        /// </summary>
        public static void TestTerminal()
        {
            WriteLine("the quick brown fox jumps over the lazy dog.");// test all alphabet letters, lowercase
            WriteLine("THE QUICK BROWN FOX JUMPS OVER THE LAZY DOG.");// test all alphabet letters, uppercase
            WriteLine("ThE qUiCk BrOwN fOx JuMpS oVeR tHe LaZy DoG.");// test all alphabet letters, mixed case 1
            WriteLine("tHe QuIcK bRoWn FoX jUmPs OvEr ThE lAzY dOg.");// test all alphabet letters, mixed case 2
            int j = 32;
            while (j != 126)
            {
                Write(((char)j).ToString());
                j++;
            }
            for (int i = 0; i < 15;  i++)
            {
                Write("the quick brown fox jumps over the lazy dog.");// test text-wraparound
            }
        }

        public static void Clear()
        {
            Kernel.canvas.Clear(Color.Black);
            Kernel.canvas.DrawImage(0, 15, Kernel.Logo);
            LibTerm.SetCursorPos(0, 90);
            LibTerm.WriteLine(Kernel.BuildString + " " + Kernel.Hostname + " svga\n"); // ADD HOST/USERNAMES!!!
        }

        public static void ColorWrite(string s, Color c)
        {
            ForegroundC = c;
            Write(s);
            ForegroundC = Color.White;
        }
    }
}