using HelloJobBackEnd.Entities;
using HelloJobBackEnd.ViewModel;

namespace HelloJobBackEnd.Services.Interface
{
    public interface IBusinessTitleService
    {
        List<BusinessTitle> GetAllBusinessTitlesWithAreas();
        BusinessTitleVM EditedTItle(int id);
        BusinessTitle GetTitleById(int id);
    }
}
