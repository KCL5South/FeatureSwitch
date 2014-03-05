using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FS
{   
    [CollectionDataContract(Name="Features", Namespace="https://www.kcl-data.com", ItemName="Feature")]
    internal class FeatureModelCollection : List<FeatureModel>
    {
        public static FeatureModelCollection Deserialize(System.IO.Stream stream)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(FeatureModelCollection));
            return serializer.ReadObject(stream) as FeatureModelCollection;
        }
    }
}
