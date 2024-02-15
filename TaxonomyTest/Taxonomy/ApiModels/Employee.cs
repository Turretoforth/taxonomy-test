using System.ComponentModel.DataAnnotations;

namespace Taxonomy.ApiModels
{
    public class Employee
    {
        public Guid Identifier { get; set; }

        [Required]
        public string Name { get; set; }

        public uint Height { get; set; }

        [Required]
        public string Role { get; set; }

        public Guid? Manager { get; set; } = null;

        public string? Department { get; set; } = null;

        public string? ProgrammingLanguage { get; set; } = null;
    }
}
