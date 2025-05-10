using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectPAWRusuMihnea
{
    internal class Company
    {
        public int companyId { set; get; }
        public string companyName {  set; get; }

        public Company(int companyId,string companyName)
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
