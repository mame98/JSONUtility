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
            JSONNode root = JSONParser.FromFilename("C:\\\\Users\\Marius\\Documents\\OpenTrack\\Assets\\Scripts\\TrackBuilder\\test.json").parse();
        }
    }
}
