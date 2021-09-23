using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using DzNet6.Storages;

namespace DzNet6.Serialization
{
    class BinarySerialization : ISerialize
    {
        public List<Storage> Load()
        {
            List<Storage> priceList;
            using (var stream = new FileStream("settings.bin", FileMode.Open, FileAccess.Read))
            {
                var formatter = new BinaryFormatter();
                priceList = (List<Storage>)formatter.Deserialize(stream);
                stream.Close();
            }
            return priceList;
        }

        public void Save(List<Storage> storage)
        {
            try
            {
                using (Stream stream = new FileStream(@"settings.bin", FileMode.Create, FileAccess.Write))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, storage);
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
