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
            collection.Add(new FeatureModel() { Key = "Feature1", Enabled = false });
            collection.Add(new FeatureModel() { Key = "Feature1.SubFeature1", Enabled = true });
            collection.Add(new FeatureModel() { Key = "Feature2", Enabled = true });
            collection.Add(new FeatureModel() { Key = "Feature2.SubFeature2", Enabled = false });
            collection.Add(new FeatureModel() { Key = "Feature2.SubFeature2.SubFeature3", Enabled = true });
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
    }
}
