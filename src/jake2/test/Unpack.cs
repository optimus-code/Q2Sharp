using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp
{
    class Unpack
    {
        static readonly int IDPAKHEADER = (('K' << 24) + ('C' << 16) + ('A' << 8) + 'P');
        static int IntSwap(int i)
        {
            int a, b, c, d;
            a = i & 255;
            b = (i >> 8) & 255;
            c = (i >> 16) & 255;
            d = (i >> 24) & 255;
            return (a << 24) + (b << 16) + (c << 8) + d;
        }

        static bool PatternMatch(string pattern, string s)
        {
            int index;
            int remaining;
            if (pattern.Equals(s))
            {
                return true;
            }

            index = pattern.IndexOf('*');
            if (index == -1)
            {
                return false;
            }

            if (!pattern.RegionMatches(0, s, 0, index))
            {
                return false;
            }

            index += 1;
            remaining = pattern.Length - index;
            if (s.Length < remaining)
            {
                return false;
            }

            if (!pattern.RegionMatches(index, s, s.Length - remaining, remaining))
            {
                return false;
            }

            return true;
        }

        static void Usage()
        {
            System.Diagnostics.Debug.WriteLine("Usage: unpack <packfile> <match> <basedir>");
            System.Diagnostics.Debug.WriteLine("   or: unpack -list <packfile>");
            System.Diagnostics.Debug.WriteLine("<match> may contain a single * wildcard");
            System.Exit(1);
        }

        public static void Main(String[] args)
        {
            int ident;
            int dirofs;
            int dirlen;
            int i;
            int numLumps;
            byte[] name = new byte[56];
            string nameString;
            int filepos;
            int filelen;
            FileStream readLump;
            DataInputStream directory;
            string pakName;
            string pattern;
            if (args.length == 2)
            {
                if (!args[0].Equals("-list"))
                {
                    Usage();
                }

                pakName = args[1];
                pattern = null;
            }
            else if (args.length == 3)
            {
                pakName = args[0];
                pattern = args[1];
            }
            else
            {
                pakName = null;
                pattern = null;
                Usage();
            }

            try
            {
                directory = new DataInputStream(new FileInputStream(pakName));
                readLump = new FileStream(pakName, "r");
                ident = IntSwap(directory.ReadInt32());
                dirofs = IntSwap(directory.ReadInt32());
                dirlen = IntSwap(directory.ReadInt32());
                if (ident != IDPAKHEADER)
                {
                    System.Diagnostics.Debug.WriteLine(pakName + " is not a pakfile.");
                    System.Exit(1);
                }

                directory.SkipBytes(dirofs - 12);
                numLumps = dirlen / 64;
                System.Diagnostics.Debug.WriteLine(numLumps + " lumps in " + pakName);
                for (i = 0; i < numLumps; i++)
                {
                    directory.ReadFully(name);
                    filepos = IntSwap(directory.ReadInt32());
                    filelen = IntSwap(directory.ReadInt32());
                    nameString = new string (name);
                    nameString = nameString.Substring(0, nameString.IndexOf(0));
                    if (pattern == null)
                    {
                        System.Diagnostics.Debug.WriteLine(nameString + " : " + filelen + "bytes");
                    }
                    else if (PatternMatch(pattern, nameString))
                    {
                        File writeFile;
                        DataOutputStream writeLump;
                        byte[] buffer = new byte[filelen];
                        StringBuffer fixedString;
                        string finalName;
                        int index;
                        System.Diagnostics.Debug.WriteLine("Unpaking " + nameString + " " + filelen + " bytes");
                        readLump.Seek(filepos);
                        readLump.ReadFully(buffer);
                        fixedString = new StringBuffer(args[2] + File.separator + nameString);
                        for (index = 0; index < fixedString.Length; index++)
                        {
                            if (fixedString[index] == '/')
                            {
                                fixedString.Se[index, File.separatorChar];
                            }
                        }

                        finalName = fixedString.ToString();
                        index = finalName.LastIndexOf(File.separatorChar);
                        if (index != -1)
                        {
                            string finalPath;
                            File writePath;
                            finalPath = finalName.Substring(0, index);
                            writePath = new File(finalPath);
                            writePath.Mkdirs();
                        }

                        writeFile = new File(finalName);
                        writeLump = new DataOutputStream(new FileOutputStream(writeFile));
                        writeLump.Write(buffer);
                        writeLump.Close();
                    }
                }

                readLump.Close();
                directory.Close();
            }
            catch (IOException e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
    }
}