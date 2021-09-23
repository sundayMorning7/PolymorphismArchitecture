using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using PolymorphismArchitecture.Storages;

namespace PolymorphismArchitecture.Serializaion
{
    class JsonSerialization : ISerialize
    {
        public List<Storage> Load()
        {
            List<Storage> priceList;
            using (var fs = new FileStream("settings.json", FileMode.Open, FileAccess.Read))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Storage>));
                priceList = (List<Storage>)jsonFormatter.ReadObject(fs);
                fs.Close();
            }
            return priceList;
        }

        public void Save(List<Storage> storage)
        {
            using (FileStream fs = new FileStream("settings.json", FileMode.Create,FileAccess.Write))
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Storage>));
                jsonFormatter.WriteObject(fs, storage);
                fs.Close();
            }
        }
    }
}
