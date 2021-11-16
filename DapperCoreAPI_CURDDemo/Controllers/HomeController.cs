using Dapper;
using DapperCoreAPI_CURDDemo.Services;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Providers.Entities;

namespace DapperCoreAPI_CURDDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IDapper _dapper;
        public HomeController(IDapper dapper)
        {
            _dapper = dapper;
        }
        [HttpGet(nameof(GetById))]
        public async Task<Emp> GetById(string Id)
        {
            var result = await Task.FromResult(_dapper.Get<Emp>($"Select * from Emp where PS_Number = {Id}", null, commandType: CommandType.Text));
            return result;
        }
        [Authorize(Roles = "40015554")]
        [HttpGet(nameof(GetAllEmp))]
        public List<Emp> GetAllEmp()
        {
            var result = _dapper.GetAll<Emp>($"Select * from Emp", null, commandType: CommandType.Text);
            return result;
        }
        //add new Emp
        [HttpPost(nameof(Create))]
        public async Task<int> Create(Emp data)
        {
            var dbparams = new DynamicParameters();
            dbparams.Add("Users_Name", data.Users_Name, DbType.String);
            dbparams.Add("Users_Email", data.Users_Email, DbType.String);
            dbparams.Add("PS_Number", data.PS_Number, DbType.String);
            dbparams.Add("Role_ID", data.Role_ID, DbType.Int32);
            var result = await Task.FromResult(_dapper.Insert<int>($"INSERT INTO Emp (Users_Name, Users_Email, PS_Number, Role_ID) VALUES(@Users_Name, @Users_Email, @PS_Number, @Role_ID); "
                , dbparams,
                commandType: CommandType.Text));
            return result;
        }
        //update Role ID 
        [HttpPatch(nameof(Update))]
        public Task<int> Update(Emp data)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Role_Id", data.Role_ID);
            dbPara.Add("PS_Number", data.PS_Number, DbType.String);

            var updateArticle = Task.FromResult(_dapper.Update<int>($"UPDATE Emp SET Role_ID = {data.Role_ID} where PS_Number = {data.PS_Number}; ",
                            dbPara,
                            commandType: CommandType.Text));
            return updateArticle;
        }
        //delete using PS number
        [HttpDelete(nameof(Delete))]
        public async Task<int> Delete(Emp data)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("PS_Number", data.PS_Number, DbType.String);
            var result = await Task.FromResult(_dapper.Execute($"Delete From Emp Where PS_Number = {data.PS_Number}", null, commandType: CommandType.Text));
            return result;
        }
    }
}
