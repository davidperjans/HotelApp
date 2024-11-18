using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Classes
{
    public class Invoice
    {
        public int FakturaId { get; set; }
        public string GästNamn { get; set; }
        public int RumsNummer { get; set; }
        public DateTime Incheckning { get; set; }
        public DateTime Utcheckning { get; set; }
        public decimal TotalKostnad { get; set; }
    }
}
