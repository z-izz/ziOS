using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ziOS.Libs
{
    public class FScmds
    {
        public static void LS()
        {
            if (!tuldrv.DisableFS)
            {
                string[] Dirs = Directory.GetDirectories(tuldrv.Syspath);
                string[] Files = Directory.GetFiles(tuldrv.Syspath);
                LibTerm.ForegroundC = PrismGraphics.Color.GoogleBlue;
                foreach (string Dir in Dirs)
                {
                    LibTerm.WriteLine(Dir);
                }
                LibTerm.ForegroundC = PrismGraphics.Color.White;
                foreach  (string File in Files)
                {
                    if (File.EndsWith(".zis"))
                    {
                        LibTerm.ForegroundC = PrismGraphics.Color.GoogleYellow;
                        LibTerm.WriteLine(File);
                        LibTerm.ForegroundC = PrismGraphics.Color.White;
                    }
                    else
                        LibTerm.WriteLine(File);
                }
            } else
            {
                tuldrv.Error("The file system is disabled.","DIR");
            }
        }
        public static void cat(string[] args)
        {
            if (!tuldrv.DisableFS)
            {
                if (args.Length == 2)
                {
                    if (File.Exists(tuldrv.Syspath + args[1]))
                    {
                        LibTerm.WriteLine(File.ReadAllText(tuldrv.Syspath + args[1]));
                    } else
                    {
                        tuldrv.Error("Tried to read from non-existing file!","CAT");
                    }
                }
                else if (args.Length == 3)
                {
                    if (args[2] == "-x")
                    {
                        if (File.Exists(tuldrv.Syspath + args[1]))
                        {
                            LibTerm.WriteLine(tuldrv.ConvertToHex(File.ReadAllText(tuldrv.Syspath + args[1])) + "\n");
                            LibTerm.WriteLine(File.ReadAllBytes(tuldrv.Syspath + args[1]).Length.ToString() + " Bytes.");
                        }
                        else
                        {
                            tuldrv.Error("Tried to read from non-existing file!", "CAT");
                        }
                    } else
                    {
                        tuldrv.Error("Invalid use of CAT command!", "CAT");
                    }
                }
            }
            else
            {
                tuldrv.Error("The file system is disabled.","CAT");
            }
        }
        
        public static void mkdir(string[] args)
        {
            if (!tuldrv.DisableFS)
            {
                if (args.Length == 2)
                {
                    Directory.CreateDirectory(tuldrv.Syspath + args[1]);
                    tuldrv.Success();
                } else
                {
                    tuldrv.Error("Invalid use of MKDIR command!", "MKDIR");
                }
            } else
            {
                tuldrv.Error("The file system is disabled.", "MKDIR");
            }
        }

        public static void CD(string[] args)
        {
            if (!tuldrv.DisableFS)
            {
                if (args.Length == 2)
                {
                    if (Directory.Exists(tuldrv.Syspath + args[1]))
                    {
                        tuldrv.Syspath += args[1] + "\\";
                    } else
                    {
                        tuldrv.Error("Tried to access directory that doesn't exist", "CD");
                    }
                    
                }
                else
                {
                    tuldrv.Error("Invalid use of CD command!", "CD");
                }
            }
            else
            {
                tuldrv.Error("The file system is disabled.", "CD");
            }
        }

        public static void CDdotdot()
        {
            if (!tuldrv.DisableFS)
            {
                if (tuldrv.Syspath.EndsWith(":\\"))
                {
                    tuldrv.Error("No.", "CD..");
                }
                else
                {
                    tuldrv.Syspath = tuldrv.Syspath.TrimEnd('\\');
                    int lastIndex = tuldrv.Syspath.LastIndexOf('\\');
                    tuldrv.Syspath = tuldrv.Syspath.Substring(0, lastIndex) + '\\';
                }
            }
            else
            {
                tuldrv.Error("File System is Disabled.", "CD..");
            }
        }
        public static void Fwrite(string[] args)
        {
            if (!tuldrv.DisableFS)
            {
                if (args.Length < 3)
                {
                    tuldrv.Error("Invalid use of fwrite command.", "FWRITE");
                }
                else
                {
                    var arglist = args.ToList();
                    arglist.RemoveAt(0);
                    arglist.RemoveAt(0);
                    File.WriteAllText(tuldrv.Syspath + args[1], string.Join(' ', arglist.ToArray()));
                    tuldrv.Success();
                }
            }
            else
            {
                tuldrv.Error("File System is Disabled.", "FWRITE");
            }
        }

        public static void remove(string[] args)
        {
            if (!tuldrv.DisableFS)
            {
                if (args.Length == 3)
                {
                    if (args[2] == "-r")
                    {
                        if (Directory.Exists(tuldrv.Syspath + args[1]))
                        {
                            var filelist = Directory.GetFiles(tuldrv.Syspath + args[1]);
                            foreach (var file in filelist)
                            {
                                File.Delete(tuldrv.Syspath + args[1] + "\\" + file);
                            }
                            Directory.Delete(tuldrv.Syspath + args[1]);
                            tuldrv.Success();
                        }
                        else
                        {
                            tuldrv.Error("Tried to delete what doesn't exist.", "RM");
                        }
                    }
                    else
                    {
                        tuldrv.Error("Invalid use of rm command.", "RM");
                    }
                }
                else if (args.Length == 2)
                {
                    if (File.Exists(tuldrv.Syspath + args[1]))
                    {
                        File.Delete(tuldrv.Syspath + args[1]);
                        tuldrv.Success();
                    }
                    else
                    {
                        tuldrv.Error("Tried to delete what doesn't exist.", "RM");
                    }
                }
                else
                {
                    tuldrv.Error("Invalid use of rm command.", "RM");
                }
            }
            else
            {
                tuldrv.Error("File System is Disabled.", "RM");
            }
        }
    }
}
