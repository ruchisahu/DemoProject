using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FrontEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FrontEnd.Controllers
{
    public class BugController : Controller
    {
        string Baseurl = "https://localhost:44339/";
        HttpHelper.BugHttpClient helper = new HttpHelper.BugHttpClient("https://localhost:44339/", "api/Bug", "api/Bug/", "api/Bug/", "api/Bug/", "api/Bug/");

        // GET: Bug
        public async Task<IActionResult> Index()
        {
            List<Bug> BugInfo = new List<Bug>();

            using (var client = new HttpClient())
            {
              
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllbug using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Bug");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    BugInfo = JsonConvert.DeserializeObject<List<Bug>>(EmpResponse);

                }
                //returning the employee list to view  
                return View(BugInfo);
            }
        }

        
        
            public async Task<Bug> GetDetail(Guid? id)
            {
            Bug Event = new Bug();
            try
            {
           // var response = await htpDetails(id.ToString());
                var response = await helper.Details(id.ToString());
                dynamic json = JValue.Parse(response);
            // var jsonmessage = json.message;

            Event = JsonConvert.DeserializeObject<Bug>(json.ToString());
            }
            catch (Exception ex)
            {
                //:todo show  eroor or shoa approrpaite view 
            }

            return Event;
           
        }
        // GET: Bug/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            Bug Event = await GetDetail(id);

            return View(Event);
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
                HttpResponseMessage Res = await client.GetAsync("api/Bug/" + id);

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

        // GET: Bug/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bug/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( Bug value)
        {
            if (ModelState.IsValid)
            {
                //  public async Task<ActionResult> Create([FromBody] Bug value)
                
                    Bug receivedEvent;
                    Guid obj = Guid.NewGuid();
                    value.TaskId = obj;
                    string postitem = JsonConvert.SerializeObject(value);
                    
                        var response = await helper.Create(value.TaskId.ToString(), postitem);
                        receivedEvent = JsonConvert.DeserializeObject<Bug>(response);
                    return RedirectToAction("Index");

                }
            return View(value);
        }
    

        // GET: Bug/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
           Bug item = new Bug();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await client.GetAsync("/api/Bug/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    item = JsonConvert.DeserializeObject<Bug>(apiResponse);
                }
            }
            return View(item);
        }

        // POST: Bug/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, Bug bug)
        {
            Bug receivedevent = new Bug();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string postitem = JsonConvert.SerializeObject(bug);
                StringContent content = new StringContent(postitem, Encoding.UTF8, "application/json");

                using (var response = await client.PutAsync("/api/Bug/" + id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.Message = apiResponse.ToString();
                        return View("Status");
                    }
                  
                    receivedevent = JsonConvert.DeserializeObject<Bug>(apiResponse);
                }
            }
            return View(bug);
        }

        // GET: Bug/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Bug Event = await GetDetail(id);

            return View(Event);
        }

        // POST: Bug/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid? id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                var res = await helper.Delete(id);
            }
            catch (Exception ex)
            {
                
            }
            return View();
        }

      



    }
}