using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risika.D365.Core
{
    public class Utility
    {
        public static string ServerUrl { get; private set; }
        public static string ServiceToken { get; private set; }

        static Utility()
        {
            if (ConfigurationManager.AppSettings["Binding"] != null)
            {
                ServerUrl = ConfigurationManager.AppSettings["Binding"];
            }
            else
            {
                ServerUrl = "https://api.risika.dk/v1.2/";
            }

            if (ConfigurationManager.AppSettings["Server"] != null)
            {
                ServiceToken = ConfigurationManager.AppSettings["Server"];
            }
            else
            {
                ServiceToken = "JWT eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6NzU4LCJ0eXBlIjoic2luZ2xlLXVzZXIiLCJncmFudF9wZXJtaXQiOm51bGwsImxhbmd1YWdlIjoiZGFfREsiLCJjb21wYW55IjoxODgsImlhdCI6MTU1NjU2NjgxMywibmJmIjoxNTU2NTY2ODEzLCJleHAiOjE1ODgxODkyMTN9.wRO3F4yY_Z-4IDlwQw3JbmoX2IlVSWYdo4EhS8LN1y8"; 
            }
        } 
    }
}
