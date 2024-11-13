using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Classes
{
    public class Reservation
    {
        public DateTime ReservationDate { get; set; }
        private Room room;
        public int Duration { get; set; }
        private Guest guest;
    }
}
