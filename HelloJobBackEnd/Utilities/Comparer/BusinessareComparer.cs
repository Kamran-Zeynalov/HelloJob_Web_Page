using HelloJobBackEnd.Entities;
using System.Diagnostics.CodeAnalysis;

namespace HelloJobBackEnd.Utilities.Comparer
{
    public class BusinessareComparer : IEqualityComparer<BusinessArea>
    {
        public bool Equals(BusinessArea? x, BusinessArea? y)
        {
            if (Equals(x?.Id, y?.Id)) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] BusinessArea obj)
        {
            throw new NotImplementedException();
        }
    }
}
