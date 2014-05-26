using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Raytracer.Rml
{
    public class Parser
    {
        const char OpenTag = '{', CloseTag = '}';
        int currentChar = 0;
        Element current;
        public void Parse(string ttp)
        {
           string filtered = ttp.Replace(" ", "").Replace(System.Environment.NewLine, "");
           current = getElement(filtered, currentChar);
        }

        Element getElement(string s, int start)
        {
            char c = s[start];
            return null;
        }
    }
}
