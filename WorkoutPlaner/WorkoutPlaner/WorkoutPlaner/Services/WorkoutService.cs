using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlaner.Models;
using Xamarin.Forms;

namespace WorkoutPlaner.Services
{
    public class WorkoutService
    {

        private HttpClient client;
        
        private readonly Uri serverUrl ; 
        

        public WorkoutService()
        {
            if(Device.OS == TargetPlatform.Android)
                serverUrl =  new Uri("http://10.0.2.2:65175/");
            else
                serverUrl =  new Uri("http://127.0.0.1:65175/");
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }
        private async Task<T> GetAsync<T>(Uri uri)
        {            
           var response = await client.GetAsync(uri);
            Debug.WriteLine("nah mi a faszom van");
           var json = await response.Content.ReadAsStringAsync();
           T result = JsonConvert.DeserializeObject<T>(json);
           return result;
            
        }        public async Task<List<DailyWorkout>> GetDailyWorkoutsAsync()
        {
            return await GetAsync<List<DailyWorkout>>(new Uri(serverUrl, "api/DailyWorkouts"));
        }        
        public async Task<List<Exercise>> GetExercisesAsync()
        {
            return await GetAsync<List<Exercise>>(new Uri(serverUrl, "api/Exercises"));
        }

        public async Task PostAsync<T>(Uri url, T data)
        {

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(data);
                var httpcontent = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PostAsync(url, httpcontent);

            }
        }        public async Task PostDailyWorkout(DailyWorkout dw)
        {
            await PostAsync<DailyWorkout>(
                new Uri(serverUrl, $"api/DailyWorkouts"), dw);
        }
    }
}
