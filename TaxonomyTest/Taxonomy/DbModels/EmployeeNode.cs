using System.ComponentModel.DataAnnotations;

namespace Taxonomy.DbModels
{
    public abstract class EmployeeNode
    {
        [Key]
        public Guid Identifier { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public uint Height { get; set; }

        public EmployeeNode? Manager { get; set; }

        public ICollection<EmployeeNode> Subordinates { get; set; }
    }
}
