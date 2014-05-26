using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Disque.Raytracer.Rml
{
    public class Element
    {
        string name;
        bool hasElements = false, hasAttributes = false;
        public readonly List<Element> Elements = new List<Element>();
        public readonly ParametersCollection Parameters = new ParametersCollection();

        public Element(string na)
        {
            name = na;
        }

        public bool HasElements
        {
            get
            {
                return hasElements;
            }
            set
            {
                hasElements = value;
            }
        }

        public bool HasAttributes
        {
            get
            {
                return hasAttributes;
            }
            set
            {
                hasAttributes = value;
            }
        }

        public bool HasElement(string name)
        {
            foreach (Element ele in Elements)
                if (ele.Name == name) return true;
            return false;
        }

        public bool HasParameter(string name)
        {
            return Parameters.ContainsKey(name);
        }

        public Element GetElement(string p)
        {
            foreach (Element ele in Elements)
                if (ele.Name == p)
                    return ele;
            throw new Exception("No Element with the matching name " + p + " found.");
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
    }
}
