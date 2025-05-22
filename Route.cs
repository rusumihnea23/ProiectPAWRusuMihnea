using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPAWRusuMihnea
{
    [Serializable]
    public class Route
    {
        public string departure;
        public string destination;
        public long routeId;
        public Company company;

        public Route() { }
        public Route(string departure, string destination, long routeId, Company company)
        {
            this.departure = departure;
            this.destination = destination;
            this.routeId = routeId;
            this.company = company;
        }
        public Route(string departure, string destination, Company company)
        {
            this.departure = departure;
            this.destination = destination;
          
            this.company = company;
        }
        public override string ToString()
        {
            return $"{departure} -> {destination} ";
        }
    }
}
