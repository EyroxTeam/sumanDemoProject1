using DemoAPIS.Configurations;
using DemoData.Models;
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
    public class RoleController : GenericController
    {

        private readonly IRoleRepository rolerepositoy;
        public RoleController(IRoleRepository rolerepositoy)
        {
            this.rolerepositoy = rolerepositoy;
        }

        [HttpGet(nameof(GetRoleList))]
        public IActionResult GetRoleList(int skipCount, int maxResultCount, string search)
        {
            try
            {
                var Role = rolerepositoy.GetAllRole(skipCount, maxResultCount, search);
                if (Role != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new ResponseBack<List<Role>> { Status = "Ok", Message = "RecordFound", Data = Role });
                }
                return StatusCode(StatusCodes.Status200OK, new ResponseBack<List<Role>> { Status = "Ok", Message = "RecordNotFound", Data = null });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost(nameof(CreateRole))]
        public IActionResult CreateRole(Role obj)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(message);
            }
            string result = rolerepositoy.AddRole(obj);
            if (result == "Role Already Exists")
            {
                return StatusCode(StatusCodes.Status409Conflict, new ResponseBack<Role> { Status = "Ok", Message = "Role Already Exists", Data = null });
            }
            else if (result == "Role Added Succeffully")
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBack<Role> { Status = "Ok", Message = "Role Added Succeffully", Data = null });
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Role> { Status = "Ok", Message = "Unable to Add due to error", Data = null });

            }


        }

        [HttpPut(nameof(UpdateRole))]
        public IActionResult UpdateRole(Role obj)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            string result = rolerepositoy.UpdateRole(obj);
            if (result == "Role Updated Successfully")
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBack<Role> { Status = "Ok", Message = "Role Updated Successfully", Data = null });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Role> { Status = "Error", Message = "Role Deleted Successfully", Data = null });
        }

        [HttpDelete(nameof(DeleteRole))]
        public IActionResult DeleteRole(int id)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(message);
            }
            string result = rolerepositoy.DeleteRole(id);
            if (result == "Role Deleted Successfully")
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBack<Role> { Status = "Ok", Message = "Role Deleted Successfully", Data = null });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new ResponseBack<Role> { Status = "Error", Message = "Unable To Delete Due to Error", Data = null });
        }
        [HttpGet(nameof(RoleGetByID))]
        public IActionResult RoleGetByID(int id)
        {
            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(message);
            }

            var GetRole = rolerepositoy.RoleGetById(id);
            if (GetRole != null)
            {
                return StatusCode(StatusCodes.Status200OK, new ResponseBack<Role> { Status = "Ok", Message = "Role Found Successfully", Data = GetRole.Result });
            }
            return StatusCode(StatusCodes.Status404NotFound, new ResponseBack<Role> { Status = "Error", Message = "Role NotFound", Data = null });
        }

    }
}
