using System.Xml.Serialization;

namespace FS 
{   
    /// <summary>
    ///     The model representing a single feature's status.
    /// </summary>
    public class FeatureModel
    {
        /// <summary>
        ///     Gets or sets the Key of the feature.
        /// </summary>
        [XmlAttribute]
        public string Key { get; set; }
        /// <summary>
        ///     Gets or sets the Enabled status of the feature.
        /// </summary>
        [XmlAttribute]
        public bool Enabled { get; set; }
    }
}
