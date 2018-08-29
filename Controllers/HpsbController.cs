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
                bool ticketSolved = false;
                log.writeLog("*************************New request found begin of request**************************");
                //Creates a new instance of ticket with json received from HTTPPOST 
                Ticket ticket = new Ticket(json);        
                log.writeLog(json.ToString());
                log.writeLog("*******************************End of request****************************************");
                //Starts procedures to resolve the ticket
                if ( ticket!= null)
                {
                    //Start to check for dup files ( Win/Lx/Ux )
                    DuplicatedFile dupFile = new DuplicatedFile();
                    ticketSolved = dupFile.CheckDuplicate(ticket);
                    if (ticketSolved == true)
                    {
                        log.writeLog("Ticket was solved");
                    }
                    else
                    {
                        //Start to check for passwd expired
                        PasswordExpired passwdExp = new PasswordExpired();
                        //passwdExp.CheckPassswd;
                        if (ticketSolved == true)
                        {
                            log.writeLog("Ticket: " +ticket.IncidentId + " was solved");
                        }
                        else
                        {
                            log.writeLog("Ticket: " + ticket.IncidentId + " was NOT solved");
                        }
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
