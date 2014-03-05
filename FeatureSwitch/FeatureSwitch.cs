using System.Collections.Generic;
using System.Linq;

namespace FS
{
    /// <summary>
    ///     This class contains the logic used to 
    ///     parse feature configuration and report
    ///     on feature availability.
    /// </summary>
    public class FeatureSwitch : IFeatureSwitch
    {
        internal IDictionary<string, bool> Features { get; private set; }

        internal FeatureSwitch() 
        { 
            Features = new Dictionary<string, bool>();
        }

        internal void ParseFeatureSwitches(FeatureModelCollection collection)
        {
            if(collection == null)
                throw new System.ArgumentNullException("collection");
            
            foreach(FeatureModel model in collection)
            {
                if(Features.ContainsKey(model.Key))
                    Features[model.Key] = model.Enabled;
                else
                    Features.Add(model.Key, model.Enabled);
            }
        }

#region IFeatureSwitch Members

        bool IFeatureSwitch.this[string key]
        {
            get
            {
                bool result = true;
                string[] keys = key.Split('.');
                for(int i = 0; i < keys.Length; i++)
                {
                    string parentKey = string.Join(".", keys.Take(i + 1).ToArray());
                    if(Features.ContainsKey(parentKey))
                        result &= Features[parentKey];
                }

                return result;
            }
        }

#endregion

        /// <summary>
        ///     Creates an instance of <see href="IFeatureSwitch"/>.
        ///
        ///     The sole parameter is a stream that represents the 
        ///     feature configuration file.
        /// </summary>
        public static IFeatureSwitch Create(System.IO.Stream stream)
        {
            if(stream == null)
                throw new System.ArgumentNullException("stream");

            FeatureSwitch result = new FeatureSwitch();
            result.ParseFeatureSwitches(FeatureModelCollection.Deserialize(stream));
            return result;
        }
    }
}
