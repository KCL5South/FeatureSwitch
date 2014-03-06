using NUnit.Framework;

namespace FS
{
    [TestFixture]
    public class FeatureSwitchTests
    {
        FeatureModelCollection collection = new FeatureModelCollection();

        [TestFixtureSetUp]
        public void SetupFixture()
        {
            collection.Items.Add(new FeatureModel() { Key = "Feature1", Enabled = false });
            collection.Items.Add(new FeatureModel() { Key = "Feature1.SubFeature1", Enabled = true });
            collection.Items.Add(new FeatureModel() { Key = "Feature2", Enabled = true });
            collection.Items.Add(new FeatureModel() { Key = "Feature2.SubFeature2", Enabled = false });
            collection.Items.Add(new FeatureModel() { Key = "Feature2.SubFeature2.SubFeature3", Enabled = true });
        }

        [Test]
        public void Constructor_FeaturesPopulated()
        {
            var target = new FeatureSwitch();
            Assert.IsNotNull(target.Features);
        }

        [Test]
        public void ParseFeatureSwitches_NullArg()
        {
            try
            {
                (new FeatureSwitch()).ParseFeatureSwitches(null);
                Assert.Fail("An ArgumentNullException was expected.");
            }
            catch (System.ArgumentException) { }
        }

        [Test]
        public void ParseFeatureSwitches_NumberOfItems()
        {
            var target = new FeatureSwitch();
            target.ParseFeatureSwitches(collection);

            Assert.AreEqual(5, target.Features.Count);
        }

        [Test]
        public void IsEnabled_Feature1()
        {
            var target = new FeatureSwitch();
            target.ParseFeatureSwitches(collection);

            Assert.IsFalse((target as IFeatureSwitch)["Feature1"]);
        }

        [Test]
        public void IsEnabled_SubFeature1()
        {
            var target = new FeatureSwitch();
            target.ParseFeatureSwitches(collection);

            Assert.IsFalse((target as IFeatureSwitch)["Feature1.SubFeature1"]);
        }

        [Test]
        public void IsEnabled_Feature2()
        {
            var target = new FeatureSwitch();
            target.ParseFeatureSwitches(collection);

            Assert.IsTrue((target as IFeatureSwitch)["Feature2"]);
        }

        [Test]
        public void IsEnabled_SubFeature3()
        {
            var target = new FeatureSwitch();
            target.ParseFeatureSwitches(collection);

            Assert.IsFalse((target as IFeatureSwitch)["Feature2.SubFeature2.SubFeature3"]);
        }

        [Test]
        public void Create_Null()
        {
            try
            {
                FeatureSwitch.Create(null);
                Assert.Fail("An ArgumentNullException was expected.");
            }
            catch(System.ArgumentNullException) { }

        }

        [Test]
        public void EndToEnd_False()
		{
			string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>

<Features xmlns=""https://www.kcl-data.com"">
    <Feature Key=""Login"" Enabled=""false""/>
</Features>";

			System.IO.MemoryStream stream = new System.IO.MemoryStream(System.Text.Encoding.Default.GetBytes(xml));

			IFeatureSwitch fs = FeatureSwitch.Create(stream);

			Assert.IsFalse(fs["Login"]);
		}

        [Test]
        public void EndToEnd_True()
		{
			string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>

<Features xmlns=""https://www.kcl-data.com"">
    <Feature Key=""Login"" Enabled=""true""/>
</Features>";

			System.IO.MemoryStream stream = new System.IO.MemoryStream(System.Text.Encoding.Default.GetBytes(xml));

			IFeatureSwitch fs = FeatureSwitch.Create(stream);

			Assert.IsTrue(fs["Login"]);
		}
    }
}
