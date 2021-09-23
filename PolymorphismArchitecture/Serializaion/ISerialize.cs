using System.Collections.Generic;
using PolymorphismArchitecture.Storages;

namespace PolymorphismArchitecture.Serializaion
{
    interface ISerialize
    {
        void Save(List<Storage> storage);
        List<Storage> Load();
    }
}
