using Dapper;
using DapperCoreAPI_CURDDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperCoreAPI_CURDDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IDapper roleDapper;
        public RoleController(IDapper dapper)
        {
            roleDapper = dapper;
        }
        [HttpGet(nameof(GetRoleById))]
        public async Task<Role> GetRoleById(int Id)
        {
            var result = await Task.FromResult(roleDapper.Get<Role>($"Select * from Roles where Role_ID = {Id}", null, commandType: CommandType.Text));
            return result;
        }
        [HttpGet(nameof(GetAllRole))]
        public  List<Role> GetAllRole()
        {
            var result =  roleDapper.GetAll<Role>($"Select * from Roles", null, commandType: CommandType.Text);
            return result;
        }
        //add new role
        [HttpPost(nameof(addRole))]
        public async Task<int> addRole(Role data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Role_ID", data.Role_ID, DbType.Int32);
            dbparams.Add("Role_Name", data.Role_Name, DbType.String);
            var result = await Task.FromResult(roleDapper.Insert<int>($"INSERT INTO Roles (Role_ID, Role_Name) VALUES(@Role_ID, @Role_Name); "
                , dbparams,
                commandType: CommandType.Text));
            return result;
        }
        //update Role
        [HttpPatch(nameof(UpdateRole))]
        public Task<int> UpdateRole(Role data)
        {
            var dbPara = new DynamicParameters();

            dbPara.Add("Role_Name", data.Role_Name, DbType.String);
            dbPara.Add("Role_ID", data.Role_ID);

            var updateArticle = Task.FromResult(roleDapper.Update<int>($"UPDATE Roles SET Role_ID = {data.Role_ID} where Role_Name = {data.Role_Name}; ",
                            dbPara,
                            commandType: CommandType.Text));
            return updateArticle;
        }
        //delete role using role id
        [HttpDelete(nameof(deleteRole))]
        public async Task<int> deleteRole(Role data)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Role_ID", data.Role_ID);
            var result = await Task.FromResult(roleDapper.Execute($"Delete From Roles Where Role_ID = {data.Role_ID}", null, commandType: CommandType.Text));
            return result;
        }
    }
}
