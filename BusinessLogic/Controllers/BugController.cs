using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BusinessLogic.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        string Baseurl = "https://localhost:44308/";
        HttpHelper.BugHttpClient helper = new HttpHelper.BugHttpClient("https://localhost:44308/", "api/Bugs", "api/Bugs/", "api/Bugs/", "api/Bugs/", "api/Bugs/");

        // GET: api/Bug
        [HttpGet]
        public async Task<List<Bug>> Get()
        {
            List<Bug> BugInfo = new List<Bug>();
            try
            {
                var Response = await helper.GetAllEvents();
                BugInfo = JsonConvert.DeserializeObject<List<Bug>>(Response);
            }
            catch (Exception ex)
            {
                throw new Exception("getDetails failed with error " + ex.Message.ToString());
            }

            return BugInfo;
        }
            
        
        // GET: api/Bug/5

        [HttpGet("{id}", Name = "Get")]
        public async Task<Bug> Get(Guid? id)
        {
            Bug Event = await GetDetail(id);

            return Event;
            
        }

        public async Task<Bug> GetDetail(Guid? id)
        {
            Bug Event = new Bug();
            try
            {
                 var response = await helper.Details(id.ToString());
                 dynamic json = JValue.Parse(response);
                Event = JsonConvert.DeserializeObject<Bug>(json.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("getDetails failed with error " + ex.Message.ToString());
            }

            return Event;

        }


       

        // POST: api/Bug
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Bug value)
        {
            Bug bug = new Bug();
            Guid Id = value.TaskId;
            string bugObject = JsonConvert.SerializeObject(value);

            try
            {
                var response = await helper.Create(value.TaskId.ToString(), bugObject);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }

            return Ok(value);

        }
        

        // PUT: api/Bug/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Bug bug)
        {
            string data = JsonConvert.SerializeObject(bug);

            try
            {
                var response = await helper.Edit(bug.TaskId.ToString(), data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
            return Ok(bug);
        }

        // DELETE: api/Bug/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            try
            {
                var res = await helper.Delete(id);
            }
            catch (Exception ex)
            {
                //return appropriate error 
            }
            return Ok();
        }
       

    }
}
