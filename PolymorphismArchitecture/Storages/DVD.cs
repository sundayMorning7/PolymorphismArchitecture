using System;
using System.IO;
using System.Runtime.Serialization;
using PolymorphismArchitecture.Log;

namespace PolymorphismArchitecture.Storages
{
    [Serializable]
    [DataContract]
    public class Dvd : Storage
    {
        [DataMember]
        public int ReadSpeed { get; set; }
        [DataMember]
        public int WriteSpeed { get; set; }

        private Dvd() : base("", "", "", 44, 44)
        {

        }
        public Dvd(string manufacturer, string model, string name, double capacity, int count,int readSpeed,int writeSpeed) : base(manufacturer, model, name, capacity, count)
        {
            ReadSpeed = readSpeed;
            WriteSpeed = writeSpeed;
        }
        public override void Print(ILog log)
        {
            //base.Print(log);
            log.Print(this.ToString());
        }

        public override void Save()
        {
            base.Save();

            using (var writer = new BinaryWriter(new FileStream("DatabaseOut.bin", FileMode.Append, FileAccess.Write)))
            {
                writer.Write(ReadSpeed);
                writer.Write(WriteSpeed);
                writer.Close();
            }
        }

        public override void Load()
        {
            base.Load();
            using (var fileStream = new FileStream("DatabaseOut.bin", FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(fileStream))
                {
                    fileStream.Seek(sizeof(int)*(-2), SeekOrigin.End);

                    ReadSpeed = reader.ReadInt32();
                    WriteSpeed = reader.ReadInt32();
                    reader.Close();
                }
                fileStream.Close();
            }
        }

        public override string ToString()
        {
            var description = base.ToString();
                var newDescription = 
                string.Format(
                    "{0}Чтение: {1}" + Environment.NewLine +
                    "Запись: {2}" + Environment.NewLine,
                    description , ReadSpeed, WriteSpeed);
            return newDescription;
        }
    };
}
