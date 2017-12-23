using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UoftTimetableGenerator.Generator
{
    public interface IOrderedSet<T> where T: IComparable
    {
        void Show();
        IOrderedSet<T> MakeCopy();

        bool Contains(T item);
        bool CanAdd(T newItem);
        bool Add(T newItem);
        bool Delete(T item);

        List<T> GetContents();
    }
}
