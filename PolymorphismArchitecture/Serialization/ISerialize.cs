using System.Collections.Generic;
using DzNet6.Storages;

namespace DzNet6.Serialization
{
    interface ISerialize
    {
        void Save(List<Storage> storage);
        List<Storage> Load();
    }
}
