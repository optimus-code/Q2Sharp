using Jake2.Qcommon;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    sealed class LayoutParser
    {
        int tokenPos;
        int tokenLength;
        int index;
        int length;
        string data;
        public LayoutParser()
        {
            Init(null);
        }

        public void Init(string layout)
        {
            tokenPos = 0;
            tokenLength = 0;
            index = 0;
            data = (layout != null) ? layout : "";
            length = (layout != null) ? layout.Length : 0;
        }

        public bool HasNext()
        {
            return !IsEof();
        }

        public void Next()
        {
            if (data == null)
            {
                tokenLength = 0;
                return;
            }

            while (true)
            {
                Skipwhites();
                if (IsEof())
                {
                    tokenLength = 0;
                    return;
                }

                if (Getchar() == '/')
                {
                    if (Nextchar() == '/')
                    {
                        Skiptoeol();
                        continue;
                    }
                    else
                    {
                        Prevchar();
                        break;
                    }
                }
                else
                    break;
            }

            int c;
            int len = 0;
            if (Getchar() == '\\')
            {
                Nextchar();
                tokenPos = index;
                while (true)
                {
                    c = Getchar();
                    Nextchar();
                    if (c == '\\' || c == 0)
                    {
                        tokenLength = len;
                        return;
                    }

                    if (len < Defines.MAX_TOKEN_CHARS)
                    {
                        ++len;
                    }
                }
            }

            c = Getchar();
            tokenPos = index;
            do
            {
                if (len < Defines.MAX_TOKEN_CHARS)
                {
                    ++len;
                }

                c = Nextchar();
            }
            while (c > 32);
            if (len == Defines.MAX_TOKEN_CHARS)
            {
                Com.Printf("Token exceeded " + Defines.MAX_TOKEN_CHARS + " chars, discarded.\\n");
                len = 0;
            }

            tokenLength = len;
            return;
        }

        public bool TokenEquals(string other)
        {
            if (tokenLength != other.Length)
                return false;
            return data.RegionMatches(tokenPos, other, 0, tokenLength);
        }

        public int TokenAsInt()
        {
            if (tokenLength == 0)
                return 0;
            return Atoi();
        }

        public string Token()
        {
            if (tokenLength == 0)
                return "";
            return data.Substring(tokenPos, tokenPos + tokenLength);
        }

        private int Atoi()
        {
            int result = 0;
            bool negative = false;
            int i = 0, max = tokenLength;
            string s = data;
            int limit;
            int multmin;
            int digit;
            if (max > 0)
            {
                if (s[tokenPos] == '-')
                {
                    negative = true;
                    limit = int.MinValue;
                    i++;
                }
                else
                {
                    limit = -int.MaxValue;
                }

                multmin = limit / 10;
                if (i < max)
                {
                    digit = Lib.Digit(s[tokenPos + i++], 10);
                    if (digit < 0)
                    {
                        return 0;
                    }
                    else
                    {
                        result = -digit;
                    }
                }

                while (i < max)
                {
                    digit = Lib.Digit(s[tokenPos + i++], 10);
                    if (digit < 0)
                    {
                        return 0;
                    }

                    if (result < multmin)
                    {
                        return 0;
                    }

                    result *= 10;
                    if (result < limit + digit)
                    {
                        return 0;
                    }

                    result -= digit;
                }
            }
            else
            {
                return 0;
            }

            if (negative)
            {
                if (i > 1)
                {
                    return result;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -result;
            }
        }

        private char Getchar()
        {
            if (index < length)
            {
                return data[index];
            }

            return (char)0;
        }

        private char Nextchar()
        {
            ++index;
            if (index < length)
            {
                return data[index];
            }

            return (char)0;
        }

        private char Prevchar()
        {
            if (index > 0)
            {
                --index;
                return data[index];
            }

            return (char)0;
        }

        private bool IsEof()
        {
            return index >= length;
        }

        private char Skipwhites()
        {
            char c = (char)0;
            while (index < length && ((c = data[index]) <= ' ') && c != 0)
                ++index;
            return c;
        }

        private char Skiptoeol()
        {
            char c = (char)0;
            while (index < length && (c = data[index]) != '\\' && c != 0)
                ++index;
            return c;
        }
    }
}