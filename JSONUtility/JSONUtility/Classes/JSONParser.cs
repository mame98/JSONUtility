using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONUtility.Classes
{
    class JSONParser
    {
        protected string data;

        public static JSONParser FromFilename(string filename)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(filename);
            string data = reader.ReadToEnd();
            reader.Close();
            return JSONParser.FromString(data);
        }

        public static JSONParser FromString(string data)
        {
            return new JSONParser(data);
        }

        public JSONParser(string data)
        {
            /* Thanks to: http://stackoverflow.com/a/6112179/3700391 */
            this.data = System.Text.RegularExpressions.Regex.Replace(data,
                 @"[\ \n\r\t]       # Match a space (brackets for legibility)
                 (?=          # Assert that the string after the current position matches...
                  [^""]*      # any non-quote characters
                   (?:        # followed by...
                    ""[^""]*  # one quote, followed by 0+ non-quotes
                    ""[^""]*  # a second quote and 0+ non-quotes
                   )*         # any number of times, ensuring an even number of quotes
                  $           # until the end of the string
                 )            # End of lookahead",
                 "", System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);
        }

        protected int getMatchingBracketFor(int pos, string str = null, char up='{', char down='}')
        {
            int lvl = 1;

            if (str == null)
            {
                str = this.data;
            }

            for (int x = pos + 1; x < str.Length; x++)
            {
                if (str[x] == up)
                    lvl++;
                else if (str[x] == down)
                    lvl--;

                if (lvl == 0)
                    return x;
            }
            return -1;
        }

        protected List<int> getSplitPointsForObject(string obj)
        {
            List<int> ret = new List<int>();
            bool active = true;
            for (int x = 0; x < obj.Length; x++)
            {

                if (obj[x] == '{' && active)
                    x = this.getMatchingBracketFor(x, obj);

                if (obj[x] == '[' && active)
                    x = this.getMatchingBracketFor(x, obj, '[', ']');

                if (obj[x] == '"')
                {
                    if (x - 1 >= 0)
                        if (obj[x - 1] == '\\')
                            active = !active;
                    active = !active;
                }

                if (x < 0)
                    throw new ArgumentException("Invalid JSON (not sure why)");

                if (obj[x] == ',' && active)
                {
                    ret.Add(x);
                }
            }
            if(!active)
            {
                throw new ArgumentException("Reached end of file while searching for \" (Invalid JSON) ");
            }
            return ret;
        }

        protected JSONNode parseArray(string str, JSONNode parent=null)
        {
            int close = this.getMatchingBracketFor(0, str, '[', ']');
            string substr = str.Substring(1, close - 1);

            List<int> splits = this.getSplitPointsForObject(substr);
            List<string> items = new List<string>();
            int last = 0;
            foreach (int split in splits)
            {
                items.Add(substr.Substring(last, split - last));
                last = split + 1;
            }
            items.Add(substr.Substring(last));


            for(int x =  0; x < items.Count; x++)
            {
                JSONNode node = new JSONNode(parent, x.ToString());
                node.setRawData(items[x]);
                JSONNodeType type = node.guessType();
                if(type == JSONNodeType.OBJECT)
                {
                    this.parseObject(node.getRawData(), node);
                }
                else if(type == JSONNodeType.ARRAY)
                {
                    this.parseArray(node.getRawData(), node);
                }

                parent.addChild(node);
            }


            return parent;
        }

        protected JSONNode parseObject(string str, JSONNode parent=null)
        {
            int close = this.getMatchingBracketFor(0, str);
            string substr = str.Substring(1, close - 1);

            List<int> splits = this.getSplitPointsForObject(substr);
            List<string> pairs = new List<string>();
            int last = 0;
            foreach (int split in splits)
            {
                pairs.Add(substr.Substring(last, split - last));
                last = split + 1;
            }
            pairs.Add(substr.Substring(last));

            foreach (string pair in pairs)
            {
                string[] keyValue = pair.Split(new char[] { ':' }, 2);
                JSONNode node = new JSONNode(parent, keyValue[0]);
                node.setRawData(keyValue[1]);
                JSONNodeType type = node.guessType();
                if(type == JSONNodeType.OBJECT)
                {
                    this.parseObject(node.getRawData(), node);
                }
                else if(type == JSONNodeType.ARRAY)
                {
                    this.parseArray(node.getRawData(), node);
                }
                parent.addChild(node);
            }

            return parent;
        }

        public JSONNode parse()
        {

            if (this.data[0] != '{')
            {
                throw new ArgumentException("The given JSON is not valid (missing '{')", "data/filename");
            }

            JSONNode root = new JSONNode(null, "JSON");
            return this.parseObject(this.data, root);
        }
    }
}
