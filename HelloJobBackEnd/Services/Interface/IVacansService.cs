using HelloJobBackEnd.Entities;
using HelloJobBackEnd.ViewModel;

namespace HelloJobBackEnd.Services.Interface
{
    public interface IVacansService
    {
        IQueryable<Vacans> GetAcceptedVacansWithRelatedData();
        Vacans? GetVacansWithRelatedEntitiesById(int id);
        void SaveChanges();
        void AddInfoWorks(Vacans vacans, string workInfo);
        void AddInfoEmployeers(Vacans vacans, string employeeInfo);
        VacansVM? GetEditedModelVC(int id);
        public void CheckVacans();
    }
}
