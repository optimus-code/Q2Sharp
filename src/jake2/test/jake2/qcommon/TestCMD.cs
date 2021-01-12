using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Q2Sharp.Qcommon
{
    public class TestCMD
    {
        public static void Main(string[] args)
        {
            try
            {
                Cmd.Init();
                Cmd.RemoveCommand("exec");
                BufferedReader br = new BufferedReader(new InputStreamReader(System.in_renamed));
                System.Diagnostics.Debug.WriteLine("Give some commands:");
                while (true)
                {
                    System.Diagnostics.Debug.WriteLine("#");
                    string line = br.ReadLine();
                    Cmd.ExecuteString(line);
                }
            }
            catch (Exception e)
            {
                e.PrintStackTrace();
            }
        }
    }
}