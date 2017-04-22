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

        private readonly Uri serverUrl;
        public bool Available { get; set; }

        public WorkoutService()
        {
            if (Device.OS == TargetPlatform.Android)
                serverUrl = new Uri("http://10.0.3.2:65175/");
            else
                serverUrl = new Uri("http://127.0.0.1:65175/");
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.Timeout = TimeSpan.FromSeconds(30);
        }
        
        #region GET
        private async Task<T> GetAsync<T>(Uri uri)
        {
            string json = "";
            try
            {
                var response = await client.GetAsync(uri);
                json = await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                DependencyService.Get<INotification>()
                        .ShowNotification("Nem áll rendelkezésre hálózat!");
            }
            T result = JsonConvert.DeserializeObject<T>(json);
            return result;
        }        public async Task<List<DailyWorkout>> GetDailyWorkoutsAsync()
        {
            return await GetAsync<List<DailyWorkout>>(new Uri(serverUrl, "api/DailyWorkouts"));
        }        public async Task<List<WeeklyWorkout>> GetWeeklyWorkoutsAsync()
        {
            return await GetAsync<List<WeeklyWorkout>>(new Uri(serverUrl, "api/WeeklyWorkouts"));
        }

        public async Task<List<Exercise>> GetExercisesAsync()
        {
            return await GetAsync<List<Exercise>>(new Uri(serverUrl, "api/Exercises"));
        }
        public async Task<List<MonthlyWorkout>> GetMonthlyWorkoutsAsync()
        {
            return await GetAsync<List<MonthlyWorkout>>(new Uri(serverUrl, "api/MonthlyWorkouts"));
        }

        #endregion
        #region POST
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
        }
        public async Task PostDailyWorkout(DailyWorkout dw)
        {
            await PostAsync<DailyWorkout>(
                new Uri(serverUrl, $"api/DailyWorkouts"), dw);
        }        public async Task PostWeeklyWorkout(WeeklyWorkout dw)
        {
            await PostAsync<WeeklyWorkout>(
                new Uri(serverUrl, $"api/WeeklyWorkouts"), dw);
        }

        public async Task PostMonthlyWorkout(MonthlyWorkout saveInstance)
        {
            await PostAsync<MonthlyWorkout>(
                new Uri(serverUrl, $"api/MonthlyWorkouts"), saveInstance);
        }
        #endregion
        #region PUT
        private async Task PutAsync<T>(Uri uri,int id, T data)
        {

            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(data);
                var httpcontent = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PutAsync(uri+$"/{id}", httpcontent);
            }
        }
        public async Task PutDailyWorkout(DailyWorkout dw)
        {
            await PutAsync<DailyWorkout>(serverUrl, dw.Id, dw);
        }
        public async Task PutMonthlyWorkout(MonthlyWorkout mw)
        {
            await PutAsync<MonthlyWorkout>(serverUrl, mw.Id, mw);
        }
        #endregion
        #region DELETE
        public async Task DeleteAsync<T>(Uri uri, int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync(uri + $"/{id}");
            }
        }        public async Task DeleteDailyWorkout(DailyWorkout dw)
        {
            await DeleteAsync<DailyWorkout>(new Uri(serverUrl, $"api/DailyWorkouts"), dw.Id);
        }
        #endregion


    }
}
