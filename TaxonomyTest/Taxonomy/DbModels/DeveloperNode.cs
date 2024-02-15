using System.ComponentModel.DataAnnotations;

namespace Taxonomy.DbModels
{
    public class DeveloperNode : EmployeeNode
    {
        [Required]
        public string ProgrammingLanguage { get; set; }
    }
}
