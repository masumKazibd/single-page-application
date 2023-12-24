using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeInfoTask.Models;

namespace EmployeeInfoTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeDbContext _context;

        public EmployeesController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
          if (_context.Employee == null)
          {
              return NotFound();
          }
            return await _context.Employee.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
          if (_context.Employee == null)
          {
              return NotFound();
          }
            var employee = await _context.Employee.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
          if (_context.Employee == null)
          {
              return Problem("Employee Information Not Found.");
          }
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeId }, employee);
        }
        //Test
        // POST: api/Employees/addNthEmp
        [HttpPost("addNthEmp/{nth}")]
        public async Task<ActionResult<String>> addNthEmp(int nth)
        {
            Employee employee = new Employee();
            if (nth <= 0)
            {
                return ("Could not save any Employee");
            }
            for(int i = 1; i <= nth; i++)
            {
                employee.Name = "emp name " + i;
                employee.BloodGroup = "A+";
                employee.Status = "Active";
                employee.Email = i + "@gmail.com";
                employee.Phone = "0170000000" + i;
                employee.Gender = "Male";
                employee.DepartmentId = 1;
                employee.DateOfBirth = DateTime.Now;
                employee.Education = "ssc";
                employee.FatherName = "Father name " + i;
                employee.MotherName = "Mother name " + i;
                employee.Username = "user" + i;

                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();
                employee = new Employee();//
            }

            return "Saved "  + nth +" Employee successfully!!";
        }


        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employee == null)
            {
                return NotFound();
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employee?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
