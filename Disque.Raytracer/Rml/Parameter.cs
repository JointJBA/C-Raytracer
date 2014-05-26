using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Raytracer.Rml
{
    public class Parameter
    {
        string name, value;
        Element parent;

        internal Parameter(string n, string v, Element p)
        {
            name = n;
            value = v;
            parent = p;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
        public string Value
        {
            get
            {
                return value;
            }
        }
        public Element Parent
        {
            get
            {
                return parent;
            }
        }
    }
}
