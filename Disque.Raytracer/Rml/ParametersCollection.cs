using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Disque.Raytracer.Rml
{
    public class ParametersCollection
    {
        public Element Parent;

        readonly Dictionary<string, Parameter> Parameters = new Dictionary<string, Parameter>();

        public void Add(string a, Parameter p)
        {
            Parameters.Add(a, p);
        }

        public bool ContainsKey(string k)
        {
            return Parameters.ContainsKey(k);
        }

        public Parameter this[string a]
        {
            get
            {
                if (Parameters.ContainsKey(a))
                    return Parameters[a];
                else
                {
                    throw new Exception("Parameter " + a + " not found in " + Parent.Name);
                }
            }
            set
            {
                Parameters[a] = value;
            }
        }
    }

}
