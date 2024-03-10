using HelloJobBackEnd.Entities;
using System.Diagnostics.CodeAnalysis;

namespace HelloJobBackEnd.Utilities.Comparer
{
    public class CvComparer : IEqualityComparer<Cv>
    {
        public bool Equals(Cv? x, Cv? y)
        {
            if (Equals(x?.Id, y?.Id)) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] Cv obj)
        {
            throw new NotImplementedException();
        }
    }
}
