using Taxonomy.ApiModels;

namespace Taxonomy.Business.Interface
{
    public interface ITaxonomyBusiness
    {
        void ChangeManager(Guid employee, Guid newManager);
        Employee CreateNewEmployee(Employee newEmployee);
        List<Employee> GetSubordinates(Guid employeeGuid);
    }
}
