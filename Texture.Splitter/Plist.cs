// https://www.codeproject.com/Tips/406235/A-Simple-PList-Parser-in-Csharp

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Texture.Splitter
{
    public class Plist : Dictionary<string, dynamic>
    {
        public Plist()
        {
        }

        public Plist(string file)
        {
            Load(file);
        }

        public void Load(string file)
        {
            Clear();

            var doc = XDocument.Load(file);
            var plist = doc.Element("plist");
            var dict = plist?.Element("dict");

            var dictElements = dict?.Elements();
            parse(this, dictElements);
        }

        private void parse(Plist dict, IEnumerable<XElement> elements)
        {
            var elementsArray = elements as XElement[] ?? elements.ToArray();

            for (var i = 0; i < elementsArray.Length; i += 2)
            {
                var key = elementsArray.ElementAt(i);
                var val = elementsArray.ElementAt(i + 1);

                dict[key.Value] = parseValue(val);
            }
        }

        private List<dynamic> parseArray(IEnumerable<XElement> elements) =>
            elements.Select(e => parseValue(e)).ToList();

        private dynamic parseValue(XElement val)
        {
            switch (val.Name.ToString())
            {
                case "string":
                    return val.Value;
                    
                case "integer":
                    return int.Parse(val.Value);
                    
                case "real":
                    return float.Parse(val.Value);
                    
                case "true":
                    return true;
                    
                case "false":
                    return false;
                    
                case "dict":
                    var plist = new Plist();
                    parse(plist, val.Elements());

                    return plist;
                    
                case "array":
                    var list = parseArray(val.Elements());

                    return list;
                
                default:
                    throw new ArgumentException($"Unsupported type. ({val.Value})");
            }
        }
    }
}