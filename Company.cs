using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPAWRusuMihnea
{
    public class Company
    {
        public long companyId { set; get; }
        public string companyName {  set; get; }

        public Company(string companyName)
        {
           
            this.companyName = companyName;
        }
        public Company(long companyId,string companyName)
        {
            this.companyId = companyId;
            this.companyName = companyName;
        }

        public override string ToString()
        {
            return companyName+"Co" ;
        }
    }
}
