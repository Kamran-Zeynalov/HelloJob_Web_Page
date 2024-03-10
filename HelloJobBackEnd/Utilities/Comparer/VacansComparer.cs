using HelloJobBackEnd.Entities;
using System.Diagnostics.CodeAnalysis;

namespace HelloJobBackEnd.Utilities.Comparer
{
    public class VacansComparer : IEqualityComparer<Vacans>
    {
        public bool Equals(Vacans? x, Vacans? y)
        {
            if (Equals(x?.Id, y?.Id)) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] Vacans obj)
        {
            throw new NotImplementedException();
        }
    }
}
