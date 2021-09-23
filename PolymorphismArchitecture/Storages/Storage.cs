using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using PolymorphismArchitecture.Log;

namespace PolymorphismArchitecture.Storages
{
    [Serializable()]
    [DataContract]
    [KnownType(typeof(Dvd))]
    [KnownType(typeof(HDD))]
    [KnownType(typeof(Flash))]
    [XmlInclude(typeof(Dvd))]
    [XmlInclude(typeof(HDD))]
    [XmlInclude(typeof(Flash))]
    public abstract class Storage
    {
        [DataMember]
        public string Manufacturer { get; set; }
        [DataMember]
        public string Model { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public double Capacity { get; set; }


        public Storage(string manufacturer, string model, string name, double capacity, int count)
        {
            this.Manufacturer = manufacturer;
            this.Model = model;
            this.Name = name;
            this.Capacity = capacity;
            this.Count = count;
        }

        public abstract void Print(ILog log);

        public virtual void Save()
        {
            using (var writer = new BinaryWriter(new FileStream("DatabaseOut.bin", FileMode.Append,FileAccess.Write)))
            {
                writer.Write(Manufacturer);
                writer.Write(Name);
                writer.Write(Model);
                writer.Write(Capacity);
                writer.Write(Count);
                writer.Close();
            }
        }

        public virtual void Load()
        {
            using (var reader = new BinaryReader(new FileStream("DatabaseOut.bin", FileMode.Open, FileAccess.Read)))
            {
                Manufacturer = reader.ReadString();
                Name = reader.ReadString();
                Model = reader.ReadString();
                Capacity = reader.ReadDouble();
                Count = reader.ReadInt32();
                reader.Close();
            }
        }

        public override string ToString()
        {
            string description =
                string.Format(
                    "Производитель: {0}" + Environment.NewLine +
                    "Модель: {1,1}" + Environment.NewLine +
                    "Имя: {2,7}" + Environment.NewLine +
                    "Обьем: {3,5}" + Environment.NewLine +
                    "Кол-во: {4}" + Environment.NewLine,
                    Manufacturer, Model, Name, Capacity,Count);
            return description;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as Storage;
            if ((System.Object)p == null)
            {
                return false;
            }

            return  (Manufacturer == p.Manufacturer) && (Model == p.Model) && (Name == p.Name) ;
        }

        public override int GetHashCode()
        {
            return Manufacturer.Length ^ Model.Length ^ Name.Length;
        }
    }
}