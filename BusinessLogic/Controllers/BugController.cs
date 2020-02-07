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
            List<Bug> EmpInfo = new List<Bug>();

            using (var client = new HttpClient())
            {
            
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await client.GetAsync("api/Bugs");

              
                if (Res.IsSuccessStatusCode)
                { 
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    EmpInfo = JsonConvert.DeserializeObject<List<Bug>>(EmpResponse);

                }
               
                return EmpInfo;
            }
            }
        // https://localhost:44339/api/Bug/2da13d09-daf7-4f88-a9a7-5fd88f446df2

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
                 var response = await htpDetails(id.ToString());
              //  var response = await helper.Details(id.ToString());
                dynamic json = JValue.Parse(response);
                // var jsonmessage = json.message;

                Event = JsonConvert.DeserializeObject<Bug>(json.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("getDetails failed with error " + ex.Message.ToString());
            }

            return Event;

        }


        public async Task<String> htpDetails(string id)
        {
            String response = null;
            HttpClient client;
            client = new HttpClient();
            client.BaseAddress = new Uri(Baseurl);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            try
            {
                HttpResponseMessage Res = await client.GetAsync("api/Bugs/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    response = await Res.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Details failed with error " + ex.Message.ToString());
            }

            return response;

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
                var response = await helperCreate(value.TaskId.ToString(), bugObject);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }

            return Ok(value);

        }
        public async Task<String> helperCreate(String Id, String data)
        {
            String response = null;
            HttpClient client;
            client = new HttpClient();
            client.BaseAddress = new Uri(Baseurl);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PostAsync("api/Bugs/", content);

                if (Res.IsSuccessStatusCode)
                {
                    response = await Res.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Create failed with error " + ex.Message.ToString());
            }

            return response;
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
                var res = await helperDelete(id.ToString());
            }
            catch (Exception ex)
            {
                //return appropriate error 
            }
            return Ok();
        }
        public async Task<String> helperDelete(String Id)
        {

            String response = null;
            HttpClient client;
            client = new HttpClient();
            client.BaseAddress = new Uri(Baseurl);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage Res = await client.DeleteAsync("api/Bugs/" + Id);
                if (Res.IsSuccessStatusCode)
                {
                    response = await Res.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Delete failed with error " + ex.Message.ToString());
            }

            return response;
        }

    }
}
