using DemoAPIS.Configurations;
using DemoData.Models;
using DemoDomain.DTOs;
using DemoDomain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoAPIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : GenericController
    {
        private readonly IEmployeeRepository emprepositoy;
        public EmployeeController(IEmployeeRepository emprepositoy)
        {
            this.emprepositoy = emprepositoy;
        }

        [HttpGet(nameof(GetEmployeeList))]
        public IActionResult GetEmployeeList(int skipCount, int maxResultCount, string search)
        {
            try
            {
               
                var Employee = emprepositoy.GetAllEmployee(skipCount, maxResultCount, search);
                if (Employee != null && Employee.Count>0)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseBack<List<Employee>> { Status = "Ok", Message = "RecordFound", Data = Employee });
                }
                return StatusCode(StatusCodes.Status404NotFound, new ResponseBack<List<Employee>> { Status = "Ok", Message = "RecordNotFound", Data = null });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost(nameof(CreateEmployee))]
        public IActionResult CreateEmployee(Employee obj)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            string result = emprepositoy.AddEmployee(obj);
            if (result == "You Cannot delete person before Creating")
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Error", Message = "You Cannot Delete Person Before Creating", Data = null });
            }
            if (result == "Employee Already Exists")
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseBack<Employee> { Status = "Ok", Message = "Employee Already Exists", Data = null });
            }
            else if (result == "Employee Added Succeffully")
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBack<Employee> { Status = "Ok", Message = "Employee Added Succeffully", Data = null });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Ok", Message = "Unable to Add due to error", Data = null });

            }
        }
        [HttpPut(nameof(UpdateEmployee))]
        public IActionResult UpdateEmployee(Employee obj)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            string result = emprepositoy.UpdateEmployee(obj);
            if (result== "Employee Updated Successfully")
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBack<Employee> { Status = "Ok", Message = "Employee Updated Successfully", Data = null });
            }
            if (result== "Modifiedby Id Required")
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Error", Message = "UserId Id Required", Data = null });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Error", Message = "Employee Deleted Successfully", Data = null });
        }

        [HttpDelete(nameof(DeleteEmployee))]
        public IActionResult DeleteEmployee(int id)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(message);
            }
            string result = emprepositoy.DeleteEmployee(id);
            if (result == "Employee Deleted Successfully")
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBack<Employee> { Status = "Ok", Message = "Employee Deleted Successfully", Data = null });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Error", Message = "Unable To Delete Due to Error", Data = null });
        }
        [HttpGet(nameof(EmployeeGetByID))]
        public IActionResult EmployeeGetByID(int id)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var GetEmployee = emprepositoy.EmployeeGetById(id);
            if (GetEmployee.Result != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBack<Employee> { Status = "Ok", Message = "Employee Found Successfully", Data = GetEmployee.Result });
            }
            return StatusCode(StatusCodes.Status404NotFound, new ResponseBack<Employee> { Status = "Error", Message = "Employee NotFound", Data = null });
        }
    }
}
