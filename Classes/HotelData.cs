using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Classes
{
    public class HotelData
    {
        public List<Room> Rooms { get; set; } = new List<Room>();
        public List<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
