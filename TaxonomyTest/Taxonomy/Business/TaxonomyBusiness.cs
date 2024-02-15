using Microsoft.EntityFrameworkCore;
using Taxonomy.ApiModels;
using Taxonomy.Business.Interface;
using Taxonomy.DbModels;
using Taxonomy.Exceptions;

namespace Taxonomy.Business
{
    public class TaxonomyBusiness(TaxonomyDbContext dbContext) : ITaxonomyBusiness
    {
        private readonly TaxonomyDbContext _database = dbContext;
        private readonly string[] availableRoles = ["CEO", "Developer", "Manager"];

        public Employee CreateNewEmployee(Employee newEmployee)
        {
            // Role is determining what object to create
            EmployeeNode employeeNode;
            if (newEmployee.Role == "CEO")
            {
                employeeNode = new CEONode() { Name = newEmployee.Name };
            }
            else if (newEmployee.Role == "Developer")
            {
                if (newEmployee.ProgrammingLanguage == null)
                    throw new TaxonomyException($"The programming language must be specified for a developer");
                employeeNode = new DeveloperNode() { Name = newEmployee.Name, ProgrammingLanguage = newEmployee.ProgrammingLanguage };
            }
            else if (newEmployee.Role == "Manager")
            {
                if (newEmployee.Department == null)
                    throw new TaxonomyException($"The department must be specified for a manager");
                employeeNode = new ManagerNode() { Name = newEmployee.Name, DepartmentName = newEmployee.Department };
            }
            else
            {
                throw new TaxonomyException($"The role '{newEmployee.Role}' is not a valid role. It must be one of these values: {string.Join(',', availableRoles)}");
            }

            // If a manager is specified, add it, if not it's either the root or an error
            if (newEmployee.Manager != null)
            {
                EmployeeNode? foundManager = _database.Employees.SingleOrDefault(e => e.Identifier == newEmployee.Manager)
                    ?? throw new TaxonomyException($"The given manager '{newEmployee.Manager}' does not exist");
                employeeNode.Manager = foundManager;
                employeeNode.Height = foundManager.Height + 1;
            }
            else if (!_database.Employees.Any())
            {
                // Add as a root
                employeeNode.Height = 0;
            }
            else
            {
                throw new TaxonomyException($"No manager was specified and a root node already exists. Please specify a manager.");
            }

            _database.Employees.Add(employeeNode);
            _database.SaveChanges();

            return GetEmployeeFromNode(employeeNode);
        }

        public List<Employee> GetSubordinates(Guid employeeGuid)
        {
            EmployeeNode? foundEmployee = _database.Employees.Include(e => e.Subordinates).SingleOrDefault(e => e.Identifier == employeeGuid)
                ?? throw new TaxonomyException($"The given employee '{employeeGuid}' does not exist");

            return foundEmployee.Subordinates.Select(e => GetEmployeeFromNode(e)).ToList();
        }

        public void ChangeManager(Guid employee, Guid newManager)
        {
            if (employee.Equals(newManager))
                throw new TaxonomyException($"An employee can't be its own manager");

            EmployeeNode? foundEmployee = _database.Employees.Include(e => e.Subordinates).SingleOrDefault(e => e.Identifier == employee)
                ?? throw new TaxonomyException($"The given employee '{employee}' does not exist");

            EmployeeNode? foundManager = _database.Employees.Include(e => e.Subordinates).SingleOrDefault(e => e.Identifier == newManager)
                ?? throw new TaxonomyException($"The given new manager '{newManager}' does not exist");

            // Do not allow changing the manager of an employee with subordinates to avoid inconsistencies
            if (foundEmployee.Subordinates.Count != 0)
            {
                throw new TaxonomyException($"Changing the manager of an employee with subordinates is not allowed at this time");
            }

            foundEmployee.Manager = foundManager;
            foundEmployee.Height = foundManager.Height + 1;
            _database.SaveChanges();
        }

        private static Employee GetEmployeeFromNode(EmployeeNode employeeNode)
        {
            string role = employeeNode switch
            {
                CEONode => "CEO",
                ManagerNode => "Manager",
                DeveloperNode => "Developer",
                _ => "UNKNOWN",
            };

            return new Employee()
            {
                Identifier = employeeNode.Identifier,
                Height = employeeNode.Height,
                Name = employeeNode.Name,
                Role = role,
                Department = (employeeNode as ManagerNode)?.DepartmentName,
                Manager = employeeNode.Manager?.Identifier,
                ProgrammingLanguage = (employeeNode as DeveloperNode)?.ProgrammingLanguage
            };
        }
    }
}
