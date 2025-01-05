﻿using System;

namespace CoreBot.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
