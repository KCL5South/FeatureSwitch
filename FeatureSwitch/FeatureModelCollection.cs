using System.Xml.Serialization;
using System.Collections.Generic;

namespace FS
{   
    /// <summary>
    ///     The collection of features in a configuration.
    /// </summary>
    [XmlRootAttribute("Features", Namespace="https://www.kcl-data.com", IsNullable = false)]
    public class FeatureModelCollection
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public FeatureModelCollection()
        {
            Items = new List<FeatureModel>();
        }

        /// <summary>
        ///     The collection of single features.
        /// </summary>
        [XmlElement("Feature")]
        public List<FeatureModel> Items { get; set; }

        /// <summary>
        ///     Call this method to create a FeatureModelCollection from a stream.
        /// </summary>
        public static FeatureModelCollection Deserialize(System.IO.Stream stream)
        {
            if(stream == null)
                throw new System.ArgumentNullException("stream");

            XmlSerializer serializer = new XmlSerializer(typeof(FeatureModelCollection));
            return serializer.Deserialize(stream) as FeatureModelCollection;
        }
    }
}
