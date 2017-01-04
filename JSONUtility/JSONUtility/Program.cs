using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSONUtility.Classes;

namespace JSONUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            string JSONExample = "{" + Environment.NewLine +
            "\t\"first\": {" + Environment.NewLine +
            "\t\t\"answer\": 42," + Environment.NewLine +
            "\t\t\"pi\": 3.14," + Environment.NewLine +
            "\t\t\"array\": [1,2,3,4,5, { \"name\": \"Anonymus Person\", \"age\": 42, \"subobject\": {\"a\": \"A Value\", \"b\": -1.124E-2} }  ,7,8, 9, 100]" + Environment.NewLine +
            "\t}," + Environment.NewLine +
            "\t\"one More\": \"Hello \\\"Nice\\\" World!\"," + Environment.NewLine +
            "\t\"last Child\": {" + Environment.NewLine +
            "\t\t\"some large key\": \"contanis, a, long, string, of, course\"," + Environment.NewLine +
            "\t\t\"betterPI\": 3.141," + Environment.NewLine +
            "\t\t\"sub\": {" + Environment.NewLine +
            "\t\t\t\"name\": \"e\"," + Environment.NewLine +
            "\t\t\t\"value\": 2.71828" + Environment.NewLine +
            "\t\t}" + Environment.NewLine +
            "\t}" + Environment.NewLine +
            "}";

            JSONNode root = JSONParser.FromString(JSONExample).parse();
            root.dumpAllToConsole(true);
            Console.ReadLine();
        }
    }
}
