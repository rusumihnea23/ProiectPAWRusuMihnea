using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPAWRusuMihnea
{
    internal class Route
    {
        public string departure;
        public string destination;
        public int routeId;
        public Company company;


        public Route(string departure, string destination, int routeId, Company company)
        {
            this.departure = departure;
            this.destination = destination;
            this.routeId = routeId;
            this.company = company;
        }

    }
}
