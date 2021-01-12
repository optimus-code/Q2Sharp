using J2N.Text;
using Jake2.Qcommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Game
{
    public class Info
    {
        public static string Info_ValueForKey(string s, string key)
        {
            StringTokenizer tk = new StringTokenizer(s, "\\\\");
            while (tk.RemainingTokens > 0)
            {
                tk.MoveNext();
                
                string key1 = tk.Current;
                if (tk.RemainingTokens == 0)
                {
                    Com.Printf("MISSING VALUE\\n");
                    return s;
                }

                tk.MoveNext();

                string value1 = tk.Current;
                if (key.Equals(key1))
                    return value1;
            }

            return "";
        }

        public static string Info_SetValueForKey(string s, string key, string value)
        {
            if (value == null || value.Length == 0)
                return s;
            if (key.IndexOf('\\') != -1 || value.IndexOf('\\') != -1)
            {
                Com.Printf("Can't use keys or values with a \\\\\\n");
                return s;
            }

            if (key.IndexOf(';') != -1)
            {
                Com.Printf("Can't use keys or values with a semicolon\\n");
                return s;
            }

            if (key.IndexOf('"') != -1 || value.IndexOf('"') != -1)
            {
                Com.Printf("Can't use keys or values with a \\\"\\n");
                return s;
            }

            if (key.Length > Defines.MAX_INFO_KEY - 1 || value.Length > Defines.MAX_INFO_KEY - 1)
            {
                Com.Printf("Keys and values must be < 64 characters.\\n");
                return s;
            }

            StringBuffer sb = new StringBuffer(Info_RemoveKey(s, key));
            if (sb.Length + 2 + key.Length + value.Length > Defines.MAX_INFO_STRING)
            {
                Com.Printf("Info string length exceeded\\n");
                return s;
            }

            sb.Append('\\').Append(key).Append('\\').Append(value);
            return sb.ToString();
        }

        public static string Info_RemoveKey(string s, string key)
        {
            StringBuffer sb = new StringBuffer(512);
            if (key.IndexOf('\\') != -1)
            {
                Com.Printf("Can't use a key with a \\\\\\n");
                return s;
            }

            StringTokenizer tk = new StringTokenizer(s, "\\\\");
            while (tk.RemainingTokens > 0)
            {
                tk.MoveNext();
                string key1 = tk.Current;
                if (tk.RemainingTokens == 0)
                {
                    Com.Printf("MISSING VALUE\\n");
                    return s;
                }

                tk.MoveNext();

                string value1 = tk.Current;
                if (!key.Equals(key1))
                    sb.Append('\\').Append(key1).Append('\\').Append(value1);
            }

            return sb.ToString();
        }

        public static bool Info_Validate(string s)
        {
            return !((s.IndexOf('"') != -1) || (s.IndexOf(';') != -1));
        }

        private static string fillspaces = "                     ";
        public static void Print(string s)
        {
            StringBuffer sb = new StringBuffer(512);
            StringTokenizer tk = new StringTokenizer(s, "\\\\");
            while (tk.RemainingTokens  > 0)
            {
                tk.MoveNext();
                string key1 = tk.Current;
                if (tk.RemainingTokens == 0)
                {
                    Com.Printf("MISSING VALUE\\n");
                    return;
                }

                tk.MoveNext();
                string value1 = tk.Current;
                sb.Append(key1);
                int len = key1.Length;
                if (len < 20)
                {
                    sb.Append(fillspaces.Substring(len));
                }

                sb.Append('=').Append(value1).Append('\\');
            }

            Com.Printf(sb.ToString());
        }
    }
}