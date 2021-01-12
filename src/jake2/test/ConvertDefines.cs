using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyApp
{
    public class ConvertDefines
    {
        public static string ConvertDefine(string in_renamed)
        {
            StringBuffer out_renamed = new StringBuffer();
            StringTokenizer tk = new StringTokenizer(in_renamed);
            while (tk.HasMoreElements())
            {
                string token = tk.NextToken();
                if (token.Equals("#define"))
                {
                    out_renamed.Append("\tpublic final static int ");
                    out_renamed.Append(tk.NextToken());
                    out_renamed.Append("= ");
                    out_renamed.Append(tk.NextToken());
                    out_renamed.Append(";\\t");
                    while (tk.HasMoreElements())
                    {
                        out_renamed.Append(tk.NextToken());
                        out_renamed.Append(" ");
                    }
                }
                else
                {
                    out_renamed.Append(token);
                    out_renamed.Append(" ");
                }
            }

            return out_renamed.ToString();
        }

        public static void Main(string[] args)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("\\n".Trim().Length);
                string line;
                string filename;
                bool m_doc = true;
                if (args.length == 0)
                {
                    filename = "jake2/Defines.java";
                }
                else
                    filename = args[0];
                if (filename.StartsWith("jake2/game/M_"))
                    m_doc = true;
                else
                    m_doc = false;
                FileWriter fw = new FileWriter(filename + ".new");
                FileReader fr = new FileReader(filename);
                BufferedReader br = new BufferedReader(fr);
                while (br.Ready())
                {
                    line = br.ReadLine();
                    if (line.IndexOf("#define") != -1)
                        fw.Write(ConvertDefine(line) + "\\n");
                    else if (m_doc && line.Trim().StartsWith("mframe_t ") && line.IndexOf("new") == -1)
                    {
                        fw.Write(" static " + line + " new mframe_t[] \\n");
                        while (br.Ready())
                        {
                            line = br.ReadLine();
                            if (line.IndexOf("{") != -1)
                                fw.Write(line + "\\n");
                            else if (line.IndexOf("}") != -1)
                            {
                                fw.Write(line + "\\n");
                                break;
                            }
                            else if (line.Trim().Length == 0)
                                fw.Write("\\n");
                            else
                            {
                                string comma = "";
                                string line1 = line;
                                if (line.EndsWith(","))
                                {
                                    line1 = line.Substring(0, line1.Length - 1);
                                    comma = ",";
                                }

                                fw.Write("\\tnew mframe_t (" + line1 + ")" + comma + "\\n");
                            }
                        }
                    }
                    else if (m_doc && line.Trim().StartsWith("mmove_t"))
                    {
                        int pos1 = line.IndexOf("{");
                        int pos2 = line.IndexOf("}");
                        string seg1 = line.Substring(0, pos1);
                        string seg2 = line.Substring(pos1 + 1, pos2);
                        string seg3 = line.Substring(pos2 + 1, line.Length);
                        fw.Write("static " + seg1 + " new mmove_t (" + seg2 + ")" + seg3 + "\\n\\n");
                    }
                    else
                        fw.Write(line + "\\n");
                }

                fr.Close();
                fw.Close();
            }
            catch (Exception e)
            {
                System.err.Println("Exception:" + e);
            }
        }
    }
}