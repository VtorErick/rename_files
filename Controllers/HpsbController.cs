using HPSB_Automation.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HPSB_Automation.Controllers
{
    
    public class HpsbController : ApiController
    {
        Log log = new Log("C:\\hpsblog.txt");
        [HttpPost]
        public Ticket JsonString(JObject json)
        {
            if (json != null)
            {
                log.writeLog("*************************New request found begin of request**************************");
                //Fills ticket object with json received
                Ticket ticket = new Ticket();
                ticket = ticket.PopulateTicket(json);          
                log.writeLog(json.ToString());
                log.writeLog("*******************************End of request****************************************");
                //Recoveries will start only if root text is not null
                if ( ticket.RootText!= null)
                {
                    //Start to check for dup files ( Win/Lx/Ux )
                    DuplicatedFile dupFile = new DuplicatedFile();
                    bool exit=dupFile.CheckDuplicate(ticket);
                    if (exit==true)
                    {
                        log.writeLog("Ticket was solved");
                    }
                    else
                    {
                        log.writeLog("Ticket wasnt solved");
                    }

                    return ticket;
                }
                
            }
            return null;
        }
    }
}

//DATABASE CONNECTION
/*try
{
    SqlConnection con = new SqlConnection(WebConfigurationManager.ConnectionStrings["ConnDb"].ConnectionString);
    con.Open();

}
catch(Exception e)
{

}*/
