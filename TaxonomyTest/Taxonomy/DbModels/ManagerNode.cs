using System.ComponentModel.DataAnnotations;

namespace Taxonomy.DbModels
{
    public class ManagerNode : EmployeeNode
    {
        [Required]
        public string DepartmentName { get; set; }
    }
}
