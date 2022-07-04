using API.ModelsResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Utils
{
    public class NationalDayServices
    {
        private readonly HttpClient httpClient;


        public NationalDayServices()
        {
            this.httpClient = new HttpClient
            {
                BaseAddress  = new Uri("https://api-harilibur.vercel.app/")
            };
        }

        public async Task<List<NationalDayResponse>> GetNationalDay()
        {
            
            var result = await httpClient.GetAsync("api");
            string response = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<NationalDayResponse>>(response);
        }

        public async Task<int> CountPotonganLibur()
        {
            DateTime startDate = new DateTime(2022, 8, 10);
            DateTime endDate = new DateTime(2022, 8, 18);

            TimeSpan diff = endDate - startDate;
            int Days = diff.Days;
            int totDays = 0;
            for (var i = 0; i <= Days; i++)
            {
                var testDate = startDate.AddDays(i);
                switch (testDate.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        totDays++;
                        break;
                    case DayOfWeek.Sunday:
                        totDays++;
                        break;
                }
            }

            List< NationalDayResponse> nationalDay = await GetNationalDay();
            nationalDay = nationalDay.Where(nat => nat.is_national_holiday == true).ToList();

            foreach(NationalDayResponse natday in nationalDay)
            {
                if(natday.holiday_date >= startDate && natday.holiday_date <= endDate )
                {
                    if(natday.holiday_date.DayOfWeek != DayOfWeek.Saturday && natday.holiday_date.DayOfWeek != DayOfWeek.Sunday) {
                        totDays++;
                    }   
                }
            }
            return totDays;
        }
    }
}
