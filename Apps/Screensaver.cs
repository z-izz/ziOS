using Cosmos.Core.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ziOS.Apps
{
    internal class Screensaver
    {
        public static void Entry(string[] args)
        {
            DVDstyle(args);
        }

        public static void DVDstyle(string[] args)
        {
            const int LogoWidth = 188;
            const int LogoHeight = 65;
            int ScreenWidth = Kernel.Scrwidth;
            int ScreenHeight = Kernel.Scrheight;

            Random random = new Random();

            int logoX = random.Next(ScreenWidth - LogoWidth);
            int logoY = random.Next(ScreenHeight - LogoHeight);

            int velocityX = random.Next(-5, 5);
            int velocityY = random.Next(-5, 5);

            long beat = 0;

            bool enableVerbose = (args.Length == 2);
            while (!Cosmos.System.KeyboardManager.ShiftPressed)
            {
                beat++;
                if ((beat % 750) == 0)
                {
                    velocityX = random.Next(-5, 5);
                    velocityY = random.Next(-5, 5);
                }
                logoX += velocityX;
                logoY += velocityY;

                if (logoX < 0 || logoX + LogoWidth > ScreenWidth)
                {
                    velocityX = -velocityX;
                }

                if (logoY < 0 || logoY + LogoHeight > ScreenHeight)
                {
                    velocityY = -velocityY;
                }

                Kernel.canvas.DrawImage(logoX, logoY, Kernel.Logo, false);
                if (enableVerbose)
                {
                    Kernel.canvas.DrawFilledRectangle(0, 0, (ushort)Kernel.Scrwidth, 15, 0, PrismGraphics.Color.Black);
                    Kernel.canvas.DrawString(0, 0, "FPS: " + Kernel.canvas.GetFPS() + " - RAM used: " + Cosmos.Core.GCImplementation.GetUsedRAM() / 1000 + " KB - Currently on beat " + beat, Libs.LibTerm.Termfont, PrismGraphics.Color.GoogleRed);
                }
                Kernel.canvas.Update();
                Heap.Collect();
                //System.Threading.Thread.Sleep(1);
            }
        }

    }
}
