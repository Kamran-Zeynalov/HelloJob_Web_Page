using HelloJobBackEnd.Entities;
using HelloJobBackEnd.ViewModel;

namespace HelloJobBackEnd.Services.Interface
{
    public interface ICvPageService
    {
        IQueryable<Cv> GetAllCvs();
        Task<List<Cv>> GetSortedCvs(string sort);
        Task<List<Cv>> GetFilteredData(int[] businessIds, int[] modeIds, int[] educationIds, int[] experienceIds, bool? hasDriverLicense);
        Cv Details(int id);
        CvVM? EditedModelCv(int id);
        public void CheckCv();
    }
}
