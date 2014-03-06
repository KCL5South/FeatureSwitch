using System.Xml.Serialization;
using System.Xml.Linq;
using NUnit.Framework;
using System.Linq;

//  These tests are here to double check that the XmlSerializer is going to work for our purposes.
//  These two tests do not test any FeatureSwitch code.

namespace FS
{
    [XmlRootAttribute("Features", Namespace="https://www.kcl-data.com", IsNullable = false)]
    public class STestCollection : System.Collections.Generic.List<STest> { }

    [XmlRootAttribute("Features", Namespace="https://www.kcl-data.com", IsNullable = false)]
    public class STestCollection1
    {
        public STestCollection1()
        {
            Items = new System.Collections.Generic.List<STest>();
        }

        //[XmlArray(ElementName="Feature", Namespace="https://www.kcl-data.com", IsNullable=false)]
        [XmlElement("Feature")]
        public System.Collections.Generic.List<STest> Items { get; set; }
    }

    public class STest
    {
        [XmlAttribute]
        public string Key { get; set; }
        [XmlAttribute]
        public bool Enabled { get; set; }
    }

    [TestFixture]
    public class XmlSerializerTests
    {
        [Test]
        public void SerializeTest()
        {
            STest target = new STest()
            {
                Key = "TestKey",
                Enabled = true
            };
            STest target2 = new STest()
            {
                Key = "TestKey2",
                Enabled = false
            }; 

            STestCollection1 collection = new STestCollection1();
            collection.Items.Add(target);
            collection.Items.Add(target2);

            XmlSerializer serializer = new XmlSerializer(typeof(STestCollection1));
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            System.IO.TextWriter writer = new System.IO.StreamWriter(stream);

            serializer.Serialize(writer, collection);

            writer.Flush();
            stream.Position = 0;
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);

            //System.Console.WriteLine(reader.ReadToEnd());
            
            XDocument doc = XDocument.Load(reader);


            Assert.AreEqual(XName.Get("Features", "https://www.kcl-data.com"), doc.Root.Name);

            XElement root = doc.Root;
            Assert.AreEqual(XName.Get("Feature", "https://www.kcl-data.com"), root.Elements().First().Name);
            Assert.AreEqual(XName.Get("Feature", "https://www.kcl-data.com"), root.Elements().Skip(1).First().Name);

            Assert.AreEqual("TestKey", root.Elements().First().Attributes().First().Value);
            Assert.AreEqual("TestKey2", root.Elements().Skip(1).First().Attributes().First().Value);

            Assert.IsTrue(System.Boolean.Parse(root.Elements().First().Attributes().Skip(1).First().Value));
            Assert.IsFalse(System.Boolean.Parse(root.Elements().Skip(1).First().Attributes().Skip(1).First().Value));
        }

        [Test]
        public void DeserializeTest()
        {
            string xml = @"<Features xmlns=""https://www.kcl-data.com"">
    <Feature Key=""TestKey100"" Enabled=""false""/>
    <Feature Key=""TestKey101"" Enabled=""true""/>
</Features>";

            XmlSerializer serializer = new XmlSerializer(typeof(STestCollection1));
            System.IO.MemoryStream stream = new System.IO.MemoryStream(System.Text.Encoding.Default.GetBytes(xml));

            STestCollection1 target = serializer.Deserialize(stream) as STestCollection1;

            Assert.IsNotNull(target);
            Assert.AreEqual(2, target.Items.Count);
            Assert.AreEqual("TestKey100", target.Items[0].Key);
            Assert.AreEqual("TestKey101", target.Items[1].Key);
            Assert.IsFalse(target.Items[0].Enabled);
            Assert.IsTrue(target.Items[1].Enabled);
        }
    }
}
