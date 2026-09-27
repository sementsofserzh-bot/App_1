using System;
using System.Collections.Generic;
using System.Text;

namespace App_Model
{
    public interface IRepository<T> where T : class, IDomainObject
    {
        void Add(T item);
        void Delete(int id);
        IEnumerable<T> List();
        T? ReadById(int id);
        void Update(T item);
    }
}
