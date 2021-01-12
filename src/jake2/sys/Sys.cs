using J2N.Text;
using Q2Sharp.Client;
using Q2Sharp.Qcommon;
using Q2Sharp.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Q2Sharp.Sys
{
    public sealed class CoreSys : Defines
    {
        public static void Error(string error)
        {
            CL.Shutdown();            
            new Exception(error).PrintStackTrace();
            if (!Globals.appletMode)
            {
                Environment.Exit(1);
            }
        }

        public static void Quit()
        {
            CL.Shutdown();
            if (!Globals.appletMode)
            {
                Environment.Exit(0);
            }
        }

        public static string[] FindAll(string path)
        {
            int index = path.LastIndexOf('/');
            if (index != -1)
            {
                findbase = path.Substring(0, index);
                findpattern = path.Substring(index + 1, path.Length);
            }
            else
            {
                findbase = path;
                findpattern = "*";
            }

            if (findpattern.Equals("*.*"))
            {
                findpattern = "*";
            }

            if (!Directory.Exists(findbase))
                return null;

            return Directory.GetFiles(findbase, findpattern, SearchOption.AllDirectories);
        }

        static string[] fdir;
        static int fileindex;
        static string findbase;
        static string findpattern;
        public static string FindFirst(string path)
        {
            if (fdir != null)
                CoreSys.Error("Sys_BeginFind without close");
            fdir = FindAll(path);
            fileindex = 0;
            if (fdir == null)
                return null;
            return FindNext();
        }

        public static string FindNext()
        {
            if (fileindex >= fdir.Length)
                return null;
            return fdir[fileindex++];
        }

        public static void FindClose()
        {
            fdir = null;
        }

        public static void SendKeyEvents()
        {
            Globals.re.GetKeyboardHandler().Update();
            Globals.sys_frame_time = Timer.Milliseconds();
        }

        public static string GetClipboardData()
        {
            return null;
        }

        public static void ConsoleOutput(string msg)
        {
            if (Globals.nostdout != null && Globals.nostdout.value != 0)
                return;
            System.Console.Write(msg);
        }
    }
}