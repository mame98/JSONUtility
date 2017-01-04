using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace JSONUtility.Classes
{

    public enum JSONNodeType
    {
        OBJECT = 0,
        ARRAY,
        STRING,
        NUMBER,
        BOOL,
        NULL,
        ROOT
    }

    [DebuggerDisplay("{ToString()}")]
    public class JSONNode
    {
        public List<JSONNode> children = new List<JSONNode>();
        public string name;
        public JSONNode parent;

        protected string raw = null;

        public JSONNode(JSONNode parent, string name)
        {
            this.parent = parent;
            this.name = name;
        }

        public void setRawData(string data)
        {
            this.raw = data;
        }

        public string getRawData()
        {
            return this.raw;
        }

        public JSONNodeType guessType()
        {
            if (this.raw == null)
                return JSONNodeType.ROOT;

            switch (this.raw[0])
            {
                case '"':
                    return JSONNodeType.STRING;
                case '[':
                    return JSONNodeType.ARRAY;
                case '{':
                    return JSONNodeType.OBJECT;
                case 't':
                case 'f':
                    return JSONNodeType.BOOL;
                case 'n':
                    return JSONNodeType.NULL;
                default:
                    return JSONNodeType.NUMBER;
            }
        }

        protected string unescape(string input)
        {
            return input.Replace("\\t", "\t").Replace("\\\\", "\\").Replace("\\\"", "\"").Replace("\\/", "/")
                .Replace("\\b", "\b").Replace("\\f", "\f").Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\\"", "\"");
        }

        public dynamic getData(JSONNodeType type=JSONNodeType.ROOT)
        {
            if (type == JSONNodeType.ROOT)
                type = this.guessType();
            switch (type)
            {
                case JSONNodeType.BOOL:
                    return bool.Parse(this.raw);
                case JSONNodeType.NUMBER:
                    return double.Parse(this.raw, System.Globalization.CultureInfo.InvariantCulture);
                case JSONNodeType.STRING:
                    return this.unescape(this.raw.Substring(1, this.raw.Length - 2));
                case JSONNodeType.OBJECT:
                    return this.children;
                case JSONNodeType.ARRAY:
                    return this.children;
                default:
                    return null;
            }
        }

        public void addChild(JSONNode child)
        {
            this.children.Add(child);
        }

        public string getName()
        {
            if (this.name[0] == '"')
                return this.name.Substring(1, this.name.Length - 2);
            else
                return this.name;
        }

        public override string ToString()
        {
            return "JSON Node (" + this.getName() + ") <" + this.guessType().ToString() + ">";
        }

        protected void dumper(int lvl, JSONNode node, bool color)
        {
            string prefix = " ";
            for (int x = 0; x < lvl; x++)
            {
                prefix = " - " + prefix;
            }

            if (color)
                Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write(prefix);

            if (color)
                Console.ForegroundColor = ConsoleColor.Cyan;

            Console.Write(node.name + ": ");



            if (node.guessType() != JSONNodeType.ARRAY && node.guessType() != JSONNodeType.OBJECT)
            {
                if (color)
                    Console.ForegroundColor = ConsoleColor.White;

                Console.Write("" + node.getData());
            }

            if (color)
                Console.ForegroundColor = ConsoleColor.DarkGreen;

            Console.WriteLine(" <" + node.guessType().ToString().ToLower() + ">");


            foreach (JSONNode n in node.children)
                dumper(lvl + 1, n, color);
        }

        public void dumpAllToConsole(bool colors = false)
        {
            this.dumper(0, this, colors);
        }

        public JSONNode getChildByName(string name)
        {
            foreach(JSONNode child in this.children)
            {
                if (child.getName() == name)
                    return child;
            }
            return null;
        }

        public dynamic getChildValue(string name)
        {
            JSONNode child = this.getChildByName(name);
            if (child == null)
                return null;
            else
                return child.getData();
        }
    }
}
