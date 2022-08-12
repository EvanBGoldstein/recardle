using System;

namespace ReCardle.Models
{
    public class State
    {
        public DateTime Date { get; set; }
        public int Guess { get; set; }

        public string Status { get; set; }

        internal void Reset()
        {
            Date = DateTime.Today;
            Guess = 1;
            Status = "";
        }
    }
}
