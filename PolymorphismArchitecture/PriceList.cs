using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PolymorphismArchitecture.Enum;
using PolymorphismArchitecture.Log;
using PolymorphismArchitecture.Serializaion;
using PolymorphismArchitecture.Storages;

namespace DzNet6
{
    [DataContract]
    class PriceList : IEnumerable<Storage>
    {
        [DataMember]
        private static List<Storage> List1 { get; set; }
        public PriceList()
        {
            List1 = new List<Storage>(10);
        }
        //public Storage this[int index]
        //{
        //    get
        //    {
        //        if (index < 0 || index >= List1.Count) throw new ArgumentOutOfRangeException(nameof(index));
        //        return List1[index];
        //    }
        //}

        static PriceList()
        {
            //List1 = new List<Storage>(10);

            //Random rand = new Random();
            //List1.Add(new Dvd("DVD-1", "2", "3", 348, 200, 300, 400));
            //for (int i = 0; i < 30; i++)
            //{
            //    var num = rand.Next(1, 4);
            //    switch (num)
            //    {
            //        case 1:
            //            List1.Add(new Dvd("DVD" + i, "2", "3", 100, 200, 300, 400));
            //            break;
            //        case 2:
            //            List1.Add(new HDD("HDD" + i, "2", "3", 100, 200, 300, 400));
            //            break;
            //        case 3:
            //            List1.Add(new Flash("Flash" + i, "2", "3", 100, 200, 300, 400));
            //            break;
            //    }
            //}
            //List1.Add(new Flash("Flash40", "2", "3", 348, 200, 300, 400));
        }
        #region Add Methods
        private void Add(Storage carrier)
        {
            var index = List1.IndexOf(carrier);
            if (index == -1)
                List1.Add(carrier);
            else List1[index].Count += carrier.Count;
        }
        internal void AddDVD(string manufacturer, string model, string name, int capacity, int count, int readSpeed, int writeSpeed)
        {
            Add(new Dvd(manufacturer, model, name, capacity, count, readSpeed, writeSpeed));
        }
        internal void AddHDD(string manufacturer, string model, string name, int capacity, int count, int speed, int size)
        {
            Add(new HDD(manufacturer, model, name, capacity, count, speed, size));
        }
        internal void AddFlash(string manufacturer, string model, string name, int capacity, int count, int speed, int totalMemory)
        {
            Add(new Flash(manufacturer, model, name, capacity, count, speed, totalMemory));
        }
        #endregion
        #region Remove Methods
        public bool Remove(Storage strorage)
        {
            if (List1.Remove(strorage))
            {
                return true;
            }
            return false;
        }
        public void RemoveSequence(IEnumerable<Storage> storages)
        {
            List1 = List1.Except(storages).ToList();
        }
        #endregion

        public void Print(ILog log)
        {
            if (List1.Count == 0)
                return;
            int id = 1;
            foreach (var storage in List1)
            {
                log.Print($"\t{id++}");
                storage.Print(log);
            }
        }
        public void Save(ISerialize serializer)
        {
            serializer.Save(List1);
        }
        public void Load(ISerialize serializer)
        {
            List1 = serializer.Load();
        }

        public IEnumerable<Storage> Search(SearchCriterion searchCriteria, string lookFor)
        {
            if (List1.Count == 0)
            {
                return Enumerable.Empty<Storage>();
            }
            var carriers = Enumerable.Empty<Storage>();
            switch (searchCriteria)
            {
                case SearchCriterion.Manufacturer:
                {
                    carriers = List1.Where(c => c.Manufacturer == lookFor);
                }
                    break;
                case SearchCriterion.Model:
                {
                    carriers = List1.Where(c => c.Model == lookFor);
                }
                    break;
                case SearchCriterion.Name:
                {
                    carriers = List1.Where(c => c.Name == lookFor);
                }
                    break;
                case SearchCriterion.Count:
                {
                    int c;
                    if (Int32.TryParse(lookFor, out c))
                    {
                        carriers = List1.Where(co => c == co.Count);
                    }
                }
                    break;
                case SearchCriterion.Capacity:
                {
                    double d;
                    if (Double.TryParse(lookFor, out d))
                    {
                        carriers = List1.Where(c => Math.Abs(c.Capacity - d) < 0.1);
                    }
                }
                    break;

            }
            var enumerable = carriers as IList<Storage> ?? carriers.ToList();
            return enumerable;
        }

        public IEnumerator<Storage> GetEnumerator()
        {
            return List1.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return List1.GetEnumerator();
        }
    }

}
