using CoreBot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBot.Services
{
    public class ScheduleService
    {
        public static async Task<List<Schedule>> GetSchedulesByMovieIdAsync(int scheduleId)
        {
            return await ApiService<List<Schedule>>.GetAsync($"/movies/{scheduleId}/schedules");
        }
    }
}
