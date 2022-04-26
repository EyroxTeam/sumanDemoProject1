using DemoAPIS.Configurations;
using DemoDomain.Interfaces;
using DemoDomain.Models;
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
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeRepository emprepositoy;

    public EmployeeController(IUnitOfWork unitOfWork, IEmployeeRepository emprepositoy)
    {
      _unitOfWork = unitOfWork;
      emprepositoy = emprepositoy;
    }

    [HttpGet(nameof(GetEmployeeList))]
    public IActionResult GetEmployeeList(int skipCount, int maxResultCount, string search)
    {
      if (maxResultCount == 0)
      {
        maxResultCount = 10;
      }
      string test = string.Empty;
      search = search?.ToLower();
      int totalRecord = _unitOfWork.Employees.GetAll().Result.Count();
      if (totalRecord > 0)
      {
        var employees = new List<Employee>();
        if (!string.IsNullOrEmpty(search))
        {
          employees = _unitOfWork.Employees.GetAll().Result.Where(a => a.Name.ToLower().Contains(search) || a.Email.ToLower().Contains(search) || a.Department.StartsWith(search)
                    ).OrderBy(a => a.Id).Skip(skipCount).Take(maxResultCount).ToList().Where(x => x.IsDeleted == false).ToList();
          return StatusCode(StatusCodes.Status200OK, new ResponseBack<List<Employee>> { Status = "Ok", Message = "RecordFound", Data = employees }) ;

        }
        else
        {
          employees = _unitOfWork.Employees.GetAll().Result.OrderBy(a => a.Id).Skip(skipCount).Take(maxResultCount).ToList().Where(x => x.IsDeleted == false).ToList(); ;
          return StatusCode(StatusCodes.Status200OK, new ResponseBack<List<Employee>> { Status = "Ok", Message = "RecordFound", Data = employees });
        }
       
      }
      else {

        return StatusCode(StatusCodes.Status404NotFound, new ResponseBack<List<Employee>> { Status = "Error", Message = "RecordNotFound", Data = null });

      }
      return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<List<Employee>> { Status = "Error", Message = "Bad Request", Data = null });

    }
        
    [HttpPost(nameof(CreateEmployee))]
    public IActionResult CreateEmployee(Employee obj)
    {
      if (!ModelState.IsValid)
      {
        var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return BadRequest(message);
      }

      if (obj.IsDeleted==true)
      {
        return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Error", Message = "You Cannot Delete Person Before Creating", Data = null });
      }
      if (obj.CreatedBy == null || obj.CreatedBy == 0)
      {
        return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Error", Message = "User Id Required", Data = null });
      }
      var Getemployees = new List<Employee>();

      if (!string.IsNullOrEmpty(obj.Email))

        Getemployees = _unitOfWork.Employees.GetAll().Result.ToList();

      if (Getemployees != null)
      {
       
        var GetEmp = Getemployees.Where(m => m.Email == obj.Email).FirstOrDefault();
        if (GetEmp != null)
        {
          return StatusCode(StatusCodes.Status200OK, new ResponseBack<Employee> { Status = "Ok", Message = "Employee Already Exists", Data = null });
        }
        else
        {
          obj.CreatedDate = DateTime.Now;
          obj.IsDeleted = false;
          var result = _unitOfWork.Employees.Add(obj);
          var Record = _unitOfWork.Complete();
          if (result is not null) return StatusCode(StatusCodes.Status200OK, new ResponseBack<Employee> { Status = "Ok", Message = "Employee Added Successfully", Data = null });
          else return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Error", Message = "Error In Role Creating", Data = null });
        }
      }
      else
      {
        obj.CreatedDate = DateTime.Now;
        obj.IsDeleted = false;
        var result = _unitOfWork.Employees.Add(obj);
        _unitOfWork.Complete();
        if (result is not null) return StatusCode(StatusCodes.Status200OK, new ResponseBack<Employee> { Status = "Ok", Message = "Employee Added Successfully", Data = null });
        else return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Error", Message = "Error In Employee Creating", Data = null });
      }
      return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Error", Message = "Error In Employee Creating", Data = null });
    }

    [HttpPut(nameof(UpdateEmployee))]
    public IActionResult UpdateEmployee(Employee obj)
    {
      if (!ModelState.IsValid)
      {
        var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return BadRequest(message);
      }
      if (obj.ModifiedBy == null || obj.ModifiedBy == 0)
      {
        return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Error", Message = "User Id Required", Data = null });
      }
      obj.ModifiedDate = DateTime.Now;

      _unitOfWork.Employees.Update(obj);
      _unitOfWork.Complete();
      return StatusCode(StatusCodes.Status200OK, new ResponseBack<Employee> { Status = "Ok", Message = "Employee Updated Successfully", Data = null });
    }

    [HttpDelete(nameof(DeleteEmployee))]
    public IActionResult DeleteEmployee(int id)
    {
      if (!ModelState.IsValid)
      {
        var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return BadRequest(message);
      }

      var obj = _unitOfWork.Employees.Get(id);
      obj.Result.DeletedDate = DateTime.Now;
      obj.Result.IsDeleted = true;
      obj.Result.DeletedByUserId = 2;
      _unitOfWork.Employees.Update(obj.Result);
      _unitOfWork.Complete();
      return StatusCode(StatusCodes.Status200OK, new ResponseBack<Employee> { Status = "Ok", Message = "Employee Deleted Successfully", Data = null });
    }
    [HttpGet(nameof(EmployeeGetByID))]
    public IActionResult EmployeeGetByID(int id)
    {
      if (!ModelState.IsValid)
      {
        var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return BadRequest(message);
      }

      var GetEmployee = _unitOfWork.Employees.Get(id);
      if (GetEmployee.Result != null && GetEmployee.Result.IsDeleted!=true)
      {
        return StatusCode(StatusCodes.Status200OK, new ResponseBack<Employee> { Status = "Ok", Message = "Employee Found Successfully", Data = GetEmployee.Result });
      }
      else {
        return StatusCode(StatusCodes.Status404NotFound, new ResponseBack<Employee> { Status = "Error", Message = "Employee NotFound Successfully", Data = null });
      }

    }
  }
}
