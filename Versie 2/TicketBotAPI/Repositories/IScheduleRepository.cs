// Repositories/IScheduleRepository.cs
using System.Collections.Generic;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public interface IScheduleRepository
	{
		List<Schedule> GetAll();
		Schedule GetById(int id);
		Schedule Add(Schedule schedule);
		void Update(Schedule schedule);
		void Delete(int id);
	}
}
