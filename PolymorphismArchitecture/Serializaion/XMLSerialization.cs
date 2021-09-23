using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using PolymorphismArchitecture.Storages;

namespace PolymorphismArchitecture.Serializaion
{
    class XmlSerialization : ISerialize
    {
        public List<Storage> Load()
        {
            List<Storage> priceList;
            using (var fs = new FileStream("settings.xml", FileMode.Open, FileAccess.Read))
            {
                var serializer = new XmlSerializer(typeof(List<Storage>));
                priceList = (List<Storage>)serializer.Deserialize(fs);
                fs.Close();
            }
            return priceList;
        }

        public void Save(List<Storage> storage)
        {
            using (var writer = new StreamWriter(new FileStream(@"settings.xml", FileMode.Create, FileAccess.Write)))
            {
                var serializer = new XmlSerializer(typeof(List<Storage>));
                serializer.Serialize(writer, storage);
                writer.Close();
            }
        }
    }
}
