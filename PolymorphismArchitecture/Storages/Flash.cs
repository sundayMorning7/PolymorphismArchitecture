using System;
using System.IO;
using System.Runtime.Serialization;
using PolymorphismArchitecture.Log;

namespace PolymorphismArchitecture.Storages
{
    [Serializable]
    [DataContract]
    public class Flash : Storage
    {
        private Flash() : base("", "", "", 44, 44)
        {

        }
        [DataMember]
        public int Speed { get; set; }
        [DataMember]
        public int TotalMemory { get; set; }
        public Flash(string manufacturer, string model, string name, double capacity, int count, int speed, int totalMemory) : base(manufacturer, model, name, capacity, count)
        {
            Speed = speed;
            TotalMemory = totalMemory;
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
                writer.Write(Speed);
                writer.Write(TotalMemory);
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
                    fileStream.Seek(sizeof(int) * (-2), SeekOrigin.End);

                    Speed = reader.ReadInt32();
                    TotalMemory = reader.ReadInt32();
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
                    description, Speed, TotalMemory);
            return newDescription;
        }
    }
}
