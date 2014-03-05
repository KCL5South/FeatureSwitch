using System.Runtime.Serialization;

namespace FS 
{   
    [DataContract(Namespace = "https://www.kcl-data.com")]
    internal class FeatureModel
    {
        [DataMember]
        public string Key { get; set; }
        [DataMember]
        public bool Enabled { get; set; }
    }
}
