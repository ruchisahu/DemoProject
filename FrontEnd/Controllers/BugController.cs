﻿using System;
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

            try
            {
                var Response = await helper.GetAllEvents();
                BugInfo = JsonConvert.DeserializeObject<List<Bug>>(Response);
            }
            catch (Exception ex)
            {
                throw new Exception("Index failed with error " + ex.Message.ToString());
            }

            return View(BugInfo);
            
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
        // GET: Bug/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            Bug Event = await GetDetail(id);

            return View(Event);
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
          
            string data = JsonConvert.SerializeObject(bug);

            try
            {
                var response = await helper.Edit(bug.TaskId.ToString(), data);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
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