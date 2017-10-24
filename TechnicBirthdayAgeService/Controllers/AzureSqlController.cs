using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Data.SqlClient;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechnicBirthdayAgeService.Controllers
{
    [Route("api/[controller]")]
    public class AzureSqlController : Controller
    {
        private IHttpContextAccessor httpContextAccessor;
        private ApplicationSettings authSettings { get; set; }

        public AzureSqlController(IHttpContextAccessor httpContextAcc, IOptions<ApplicationSettings> settings)
        {
            httpContextAccessor = httpContextAcc;
            authSettings = settings.Value;
        }

        [HttpGet]
        public JsonResult Get()
        {
            JsonResult retVal = null;

            AuthenticationResult authResult = AuthenticationHelper.GetAuthenticationResult(httpContextAccessor, authSettings);

            if (authResult != null)
            {
                string queryString = "SELECT * FROM SalesLT.Product";

                using (SqlConnection connection = new SqlConnection(authSettings.ConnectionString))
                {
                    connection.AccessToken = authResult.AccessToken;
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(queryString, connection);
                        SqlDataAdapter adapter = new SqlDataAdapter(command);

                        DataTable table = new DataTable();
                        adapter.Fill(table);

                        retVal = new JsonResult(table);

                    }
                    catch (SqlException ex)
                    {
                    }
                }
            }
            return retVal;
        }
                // GET: api/values
        //        [HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
