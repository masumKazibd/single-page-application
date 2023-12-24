using EmployeeInfoTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace EmployeeInfoTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly EmployeeDbContext _context;

        public HomeController(HttpClient httpClient, EmployeeDbContext context)
        {
            _context = context;
            _httpClient = httpClient;

            _httpClient.BaseAddress = new Uri("http://localhost:5046/");
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Employee()
        {
            var departments = _context.Department.ToList();
            ViewBag.DepartmentId = new SelectList(departments, "DepartmentId", "DepartmentName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Employee(Employee model)            
        {
            var empId = model.EmployeeId;
            String messages = "";

            if (model.Name.IsNullOrEmpty() || model.FatherName.IsNullOrEmpty() || model.MotherName.IsNullOrEmpty() || model.Username.IsNullOrEmpty() || model.Email.IsNullOrEmpty() || model.Phone.IsNullOrEmpty() || model.Gender.IsNullOrEmpty() || model.BloodGroup.IsNullOrEmpty() || model.DateOfBirth.ToString().IsNullOrEmpty() || model.Status.IsNullOrEmpty() || model.DepartmentId.ToString().IsNullOrEmpty() || model.Education.IsNullOrEmpty())
            {
                TempData["errorMessage"] = "Failed to add employee.";
                Employee();
                return View();
            }
            if (model.Phone.Length !=11)
            {
                TempData["errorMessage"] = "Phone Number Should Be 11 Digit.";
                Employee();
                return View();
            }
            //check father name and mother name
            if (model.MotherName.ToLower() == model.FatherName.ToLower())
            {
                TempData["errorMessage"] = "Father name and Mother name should not same!!";
                Employee();
                return View();
            }
            HttpResponseMessage response;

            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new(data, Encoding.UTF8, "application/json");
                if (empId == 0)
                {
                    response = await _httpClient.PostAsync("/api/Employees", content);
                    messages = "Employee added Successfully!!";
                }
                else
                {
                    response = await _httpClient.PutAsync("/api/Employees/"+ empId, content);
                    messages = "Employee updated Successfully!!";

                }

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = messages;
                    return RedirectToAction("Employee");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    TempData["errorMessage"] = $"Failed to add employee. Status code: {response.StatusCode}. Content: {errorContent}";
                    //TempData["errorMessage"] = "Failed to add employee. Status code: " + response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return View();
        }
    }
}