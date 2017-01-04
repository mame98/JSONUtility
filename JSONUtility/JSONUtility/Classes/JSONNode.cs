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
    class JSONNode
    {
        public bool primitive = false;
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

            switch(this.raw[0])
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
                .Replace("\\b", "\b").Replace("\\f", "\f").Replace("\\n", "\n").Replace("\\r", "\r");
        }

        public dynamic getData()
        {
            switch(this.guessType())
            {
                case JSONNodeType.BOOL:
                    return this.raw == "true";
                case JSONNodeType.NUMBER:
                    return Convert.ToDouble(this.raw);
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
            return "JSON Node (" + this.getName() + ") [" + this.guessType().ToString() + "]";
        }
    }
}
