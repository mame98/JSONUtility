using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JSONUtility.Classes;


namespace JSONUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public string simpleJSON = "{ \"key\": \"Value\", \"number\": 3.14 }";
        public string nested = "{ \"key\": \"Value\", \"array\": [1,2,3,[4,5,6],{\"arrayKey\": 42 }]}";

        [TestMethod]
        public void TestMethod1()
        {
            JSONNode node = JSONParser.FromString(simpleJSON).parse();
            Assert.AreEqual("JSON", node.name);
            Assert.AreEqual("Value", node.getChildValue("key"));
            Assert.AreEqual(3.14, node.getChildValue("number"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            JSONNode node = JSONParser.FromString(nested).parse();
            Assert.AreEqual("JSON", node.name);
            Assert.AreEqual(5, node.getChildValue("array")[3].getData()[1].getData());
        }

    }
}
