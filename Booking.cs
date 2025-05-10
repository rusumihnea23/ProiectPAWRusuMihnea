using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPAWRusuMihnea
{
    internal class Booking
    {
        public int bookingId {  get; set; }
        public string CustomerName { get; set; }
        public Route route { get; set; }

        public Booking(int bookingId, string customerName, Route route)
        {
            this.bookingId = bookingId;
            CustomerName = customerName;
            this.route = route;
        }
    }
}
