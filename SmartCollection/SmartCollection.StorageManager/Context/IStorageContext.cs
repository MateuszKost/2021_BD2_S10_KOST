using SmartCollection.StorageManager.Containers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartCollection.StorageManager.Context
{
    public interface IStorageContext<TContainter>  where TContainter: IStorageContainer
    {
       public void AddAsync(TContainter containter, byte[] file, string name);
       public void DeleteAsync(TContainter containter, string name);
       public Task<byte[]> GetAsync(TContainter containter, string name);

    }
}
