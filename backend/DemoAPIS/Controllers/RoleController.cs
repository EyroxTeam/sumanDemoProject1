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
    public class RoleController : GenericController
    {

    private readonly IUnitOfWork _unitOfWork;
    private readonly IRoleRepository emprepositoy;

    public RoleController(IUnitOfWork unitOfWork, IRoleRepository emprepositoy)
    {
      _unitOfWork = unitOfWork;
      emprepositoy = emprepositoy;
    }

    [HttpGet(nameof(GetRoleList))]
    public IActionResult GetRoleList(int skipCount, int maxResultCount, string search)
    {
      if (maxResultCount == 0)
      {
        maxResultCount = 10;
      }
      string test = string.Empty;
      search = search?.ToLower();
      int totalRecord = _unitOfWork.Roles.GetAll().Result.Count();
      if (totalRecord > 0)
      {
        var Roles = new List<Role>();
        if (!string.IsNullOrEmpty(search))
        {
          Roles = _unitOfWork.Roles.GetAll().Result.Where(a => a.Name.ToLower().Contains(search)
             ).OrderBy(a => a.Id).Skip(skipCount).Take(maxResultCount).ToList().Where(x => x.IsDeleted == false).ToList();
          return StatusCode(StatusCodes.Status200OK, new ResponseBack<List<Role>> { Status = "Ok", Message = "RecordFound", Data = Roles });

        }
        else
        {
          Roles = _unitOfWork.Roles.GetAll().Result.OrderBy(a => a.Id).Skip(skipCount).Take(maxResultCount).ToList().Where(x => x.IsDeleted == false).ToList(); 
          return StatusCode(StatusCodes.Status200OK, new ResponseBack<List<Role>> { Status = "Ok", Message = "RecordFound", Data = Roles });
        }

      }
      else
      {

        return StatusCode(StatusCodes.Status404NotFound, new ResponseBack<List<Role>> { Status = "Error", Message = "RecordNotFound", Data = null });

      }
      return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<List<Role>> { Status = "Error", Message = "Bad Request", Data = null });

    }

    [HttpPost(nameof(CreateRole))]
    public IActionResult CreateRole(Role obj)
    {
      if (!ModelState.IsValid)
      {
        var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return BadRequest(message);
      }
      if (obj.IsDeleted == true)
      {
        return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Employee> { Status = "Error", Message = "You Cannot Delete Person Before Creating", Data = null });
      }
      if (obj.CreatedBy == null || obj.CreatedBy == 0)
      {
        return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Role> { Status = "Error", Message = "User Id Required", Data = null });
      }
      var GetRoles = new List<Role>();

      if (!string.IsNullOrEmpty(obj.Name))

        GetRoles = _unitOfWork.Roles.GetAll().Result.ToList();
      if (GetRoles != null)
      {
        var GetEmp = GetRoles.Where(m => m.Name == obj.Name).FirstOrDefault();
        if (GetEmp != null)
        {
          return StatusCode(StatusCodes.Status200OK, new ResponseBack<Role> { Status = "Ok", Message = "Role Already Exists", Data = null });
        }
        else
        {
          obj.CreatedDate = DateTime.Now;
          obj.IsDeleted = false;
          var result = _unitOfWork.Roles.Add(obj);
          var Record = _unitOfWork.Complete();
          if (result is not null) return StatusCode(StatusCodes.Status200OK, new ResponseBack<Role> { Status = "Ok", Message = "Role Added Successfully", Data = null });
          else return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Role> { Status = "Error", Message = "Error In Role Creating", Data = null });
        }
      }
      else
      {
        obj.CreatedDate = DateTime.Now;
        obj.IsDeleted = false;
        var result = _unitOfWork.Roles.Add(obj);
        _unitOfWork.Complete();
        if (result is not null) return StatusCode(StatusCodes.Status200OK, new ResponseBack<Role> { Status = "Ok", Message = "Role Added Successfully", Data = null });
        else return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Role> { Status = "Error", Message = "Error In Role Creating", Data = null });
      }
      return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Role> { Status = "Error", Message = "Error In Role Creating", Data = null });
    }

    [HttpPut(nameof(UpdateRole))]
    public IActionResult UpdateRole(Role obj)
    {
      if (!ModelState.IsValid)
      {
        var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return BadRequest(message);
      }
      if (obj.ModifiedBy == null || obj.ModifiedBy == 0)
      {
        return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Role> { Status = "Error", Message = "User Id Required", Data = null });
      }
      obj.ModifiedDate = DateTime.Now;
      _unitOfWork.Roles.Update(obj);
      _unitOfWork.Complete();
      return StatusCode(StatusCodes.Status200OK, new ResponseBack<Role> { Status = "Ok", Message = "Role Updated Successfully", Data = null });
    }

    [HttpDelete(nameof(DeleteRole))]
    public IActionResult DeleteRole(int id)
    {
      if (!ModelState.IsValid)
      {
        var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return BadRequest(message);
      }

      var obj = _unitOfWork.Roles.Get(id);
      obj.Result.DeletedDate = DateTime.Now;
      obj.Result.IsDeleted = true;
      obj.Result.DeletedByUserId = 2;
      _unitOfWork.Roles.Update(obj.Result);
      _unitOfWork.Complete();
      return StatusCode(StatusCodes.Status200OK, new ResponseBack<Role> { Status = "Ok", Message = "Role Deleted Successfully", Data = null });
    }
    [HttpGet(nameof(RoleGetByID))]
    public IActionResult RoleGetByID(int id)
    {
      if (!ModelState.IsValid)
      {
        var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        return BadRequest(message);
      }

      var GetRole = _unitOfWork.Roles.Get(id);
      if (GetRole.Result != null && GetRole.Result.IsDeleted != true)
      {
        return StatusCode(StatusCodes.Status200OK, new ResponseBack<Role> { Status = "Ok", Message = "Role Found Successfully", Data = GetRole.Result });
      }
      else {
        return StatusCode(StatusCodes.Status404NotFound, new ResponseBack<Role> { Status = "Error", Message = "Role NotFound Successfully", Data = null });
      }
   
    }

  }
}
