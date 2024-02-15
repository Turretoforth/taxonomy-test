using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Taxonomy.ApiModels;
using Taxonomy.Business.Interface;
using Taxonomy.Exceptions;

namespace Taxonomy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxonomyController(ILogger<TaxonomyController> logger, ITaxonomyBusiness business) : ControllerBase
    {
        private readonly ILogger<TaxonomyController> _logger = logger;
        private readonly ITaxonomyBusiness _business = business;

        /// <summary>
        /// Method to create a new emplyee in the tree.
        /// </summary>
        /// <param name="newEmployee">The employee to create</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateEmployee(Employee newEmployee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Employee createdEmployee = _business.CreateNewEmployee(newEmployee);
                return Created("", createdEmployee);
            }
            catch (TaxonomyException te)
            {
                return BadRequest(te.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception in CreateNode : {exception}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Method to get the subordinates of an employee
        /// </summary>
        /// <param name="employeeGuid">The GUID of the employee in question</param>
        /// <returns></returns>
        [HttpGet, Route("subordinates/{employeeGuid}")]
        public IActionResult GetSubordinates(Guid employeeGuid)
        {
            try
            {
                List<Employee> subordinates = _business.GetSubordinates(employeeGuid);
                return Ok(subordinates);
            }
            catch (TaxonomyException te)
            {
                return BadRequest(te.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception in GetChildNodes : {exception}", ex);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Method to change the manager of an employee
        /// </summary>
        /// <param name="employee">GUID of the employee to make the change on</param>
        /// <param name="newManager">GUID of the new manager</param>
        /// <returns></returns>
        [HttpPost, Route("{employee}")]
        public IActionResult ChangeManager(Guid employee, [Required] Guid newManager)
        {
            try
            {
                _business.ChangeManager(employee, newManager);
                return Ok();
            }
            catch (TaxonomyException te)
            {
                return BadRequest(te.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception in ChangeManager : {exception}", ex);
                return StatusCode(500);
            }
        }
    }
}
