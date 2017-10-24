using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System.Threading;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.ServiceFabric.Services.Client;
using System.IO;
using Newtonsoft.Json.Linq;
using TechnicBirthdayAgeService.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace TechnicBirthdayAgeService.Controllers
{
    [Route("api/[controller]")]
    public class BirthdayCalculatorController : Controller
    {
        private readonly CatalogContext _catalogContext;
        private readonly CatalogSettings _settings;
        public BirthdayCalculatorController(CatalogContext context, IOptionsSnapshot<CatalogSettings> settings)
        {
            _catalogContext = context ?? throw new ArgumentNullException(nameof(context));
           
            _settings = settings.Value;
            ((DbContext)context).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        /// <summary>
        /// Get operation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation("GetValues")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(List<string>), "Birthday Details")]
        [Route("api/Get", Name = "Get")]
        public IEnumerable<string> Get()
        {
            return new string[] { "Azure Fabric Statefull service Values controller GET", "Welcome to reliable collection framework example" };
        }

        [HttpPost("{birthdatevalue}")]
        [SwaggerOperation("CalculateBirthday")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(BirthdayCelebration), "Birthday Details")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(BirthdayCelebration), "Exception")]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [Route("api/Calculate/{birthdatevalue}", Name = "Calculate")]
        public IActionResult Calculate(string birthdatevalue)
        {
            BirthdayCelebration birthdayResult = null; ;
            DateTime birthDate = DateTime.MinValue;
            try
            {
                //if (DateTime.TryParseExact(birthdatevalue, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out birthDate))
                //{

                //}
                birthDate = Convert.ToDateTime(birthdatevalue);
                if (birthDate > DateTime.MinValue)
                {
                    birthdayResult = new BirthdayCelebration(birthDate);
                    birthdayResult.CountOfGoodThought = (birthdayResult.GoodThought).Length;
                    //birthdayResult.CountOfGoodThought = GetCountOfThoughtsFromWordCountServiceAsync(birthdayResult.GoodThought).Result;
                    birthdayResult.CountOfCatalogItems = _catalogContext.CatalogItems.LongCountAsync().Result.ToString();
                    
                    return Ok(birthdayResult);
                }
                else
                {
                    return StatusCode(400, new BirthdayCelebration("Invalid Date"));
                }
              
               
            }
            catch (Exception ex)
            {
                return StatusCode(400, new BirthdayCelebration(ex.Message));
            }
        }
    }
}
