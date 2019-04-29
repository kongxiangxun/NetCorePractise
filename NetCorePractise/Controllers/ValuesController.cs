using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using NetCorePractise.ConfigureExtensions;
using ServiceProvider.Interface;

namespace NetCorePractise.Controllers
{
    [Intercept(typeof(LogInterceptor))]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        #region 属性注入

        // public IServiceInfo serviceInfo { get; set; }
        //public ValuesController()
        //{

        //}

        #endregion


        #region 构造函数注入


        public IServiceInfo serviceInfo;

        public ValuesController(IServiceInfo serviceInfo)
        {
            this.serviceInfo = serviceInfo;
        }

        #endregion



        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            if (serviceInfo != null)
            {
                return new string[] { "value1", "value2", serviceInfo.GetName() };
            }
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
