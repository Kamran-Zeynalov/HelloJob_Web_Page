using HelloJobBackEnd.Entities;
using HelloJobBackEnd.ViewModel;

namespace HelloJobBackEnd.Services.Interface
{
    public interface ICompanyService
    {
        List<Company> GetTopAcceptedCompaniesWithVacans(int? count = null);
        Company? GetCompanyWithVacansById(int id);
        CompanyVM GetCompanyById(int id);
    }
}
