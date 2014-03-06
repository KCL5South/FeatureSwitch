using NUnit.Framework;
using System.IO;

namespace FS
{
    [TestFixture]
    public class FeatureModelCollectionTests
    {
        [Test]
        public void Deserialize_NullStream()
        {
            try
            {
                FeatureModelCollection.Deserialize(null);
                Assert.Fail("An ArgumentNullException was expected.");
            }
            catch (System.ArgumentNullException) { }
        }

        [Test]
        public void Deserialize()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Features xmlns=""https://www.kcl-data.com"">
    <Feature Key=""TestKey"" Enabled=""false""/>
</Features>";

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            writer.Write(xml);
            writer.Flush();

            stream.Position = 0;

            FeatureModelCollection collection = FeatureModelCollection.Deserialize(stream);

            Assert.IsNotNull(collection);
            Assert.AreEqual(1, collection.Items.Count);
            Assert.AreEqual("TestKey", collection.Items[0].Key);
            Assert.IsFalse(collection.Items[0].Enabled);
        }

        [Test]
        public void Deserialize_WithTwoFeatures()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Features xmlns=""https://www.kcl-data.com"">
    <Feature Key=""TestKey"" Enabled=""false""/>
    <Feature Key=""TestKeyAgain"" Enabled=""true""/>
</Features>";

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            StreamWriter writer = new StreamWriter(stream);

            writer.Write(xml);
            writer.Flush();

            stream.Position = 0;

            FeatureModelCollection collection = FeatureModelCollection.Deserialize(stream);

            Assert.IsNotNull(collection);
            Assert.AreEqual(2, collection.Items.Count);
            Assert.AreEqual("TestKey", collection.Items[0].Key);
            Assert.IsFalse(collection.Items[0].Enabled);
        }
    }
}
