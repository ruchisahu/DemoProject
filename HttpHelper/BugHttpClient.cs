
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HttpHelper
{
    public class BugHttpClient
    {
        string Baseurl;
        string apikey;
        string getallEndPoint;
        string createEndpoint;
        string deleteEndpoint;
        string updateEndpoint;
        string detailEndpoint;

        HttpClient client;

        public BugHttpClient(String baseUrl, string getallEndPoint, string createEndpoint, string deleteEndpoint, string updateEndpoint, string detailEndpoint)
        {
            this.Baseurl = baseUrl;
           
            this.createEndpoint = createEndpoint;
            this.deleteEndpoint = deleteEndpoint;
            this.updateEndpoint = updateEndpoint;
            this.detailEndpoint = detailEndpoint;
            this.getallEndPoint = getallEndPoint;

            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
         /*   if (!String.IsNullOrEmpty(apikey))
            {
                client.DefaultRequestHeaders.Add("X-API-KEY", apikey);
            }
            */
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }
        public async Task<String> GetAllEvents()
        {
            String response = null;
            try
            {
                HttpResponseMessage Res = await client.GetAsync(getallEndPoint);
                if (Res.IsSuccessStatusCode)
                {
                    response = await Res.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetAllEvents failed with error " + ex.Message.ToString());
            }

            return response;
        }
        public async Task<String> Create(String Id, String data)
        {
         
            String response = null;
          
            try
            {
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PostAsync(createEndpoint, content);

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


        public async Task<String> Delete(Guid? Id)
        {

            String response = null;
            try
            {
                HttpResponseMessage Res = await client.DeleteAsync(deleteEndpoint + Id);
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
        public async Task<String> Edit(String Id, String data)
        {

            String response = null;
            try
            {
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PutAsync(updateEndpoint + Id, content);
                if (Res.IsSuccessStatusCode)
                {
                    response = await Res.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Edit failed with error " + ex.Message.ToString());
            }

            return response;
        }

        public async Task<String> Details(string id)
        {
               String response = null;
               try
               {
                   HttpResponseMessage Res = await client.GetAsync(detailEndpoint + id);

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
    }
}

