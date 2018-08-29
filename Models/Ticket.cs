using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPSB_Automation.Models
{
    public class Ticket
    {

        private string rootText;
        private string incidentId;
        private string assignmentGroupName;
        private string affectedConfigurationItem;
        private string description;
        private string companyCode;
        private JObject json;
        private String[] hostArray;

        public string AssignmentGroupName { get => assignmentGroupName; set => assignmentGroupName = value; }
        public string AffectedConfigurationItem { get => affectedConfigurationItem; set => affectedConfigurationItem = value; }
        public string Description { get => description; set => description = value; }
        public string CompanyCode { get => companyCode; set => companyCode = value; }
        public string IncidentId { get => incidentId; set => incidentId = value; }
        public string RootText { get => rootText; set => rootText = value; }
        public string[] HostArray { get => hostArray; set => hostArray = value; }

        public Ticket()
        {

        }

        public Ticket PopulateTicket(JObject json)
        {
            this.json = json;
            Ticket ticket = new Ticket();
            ticket.RootText = (string)json.SelectToken("root_text");
            //ticket.HostArray =  JsonConvert.DeserializeObject<String[]>(json.SelectToken("host_names");
            ticket.HostArray = json.SelectToken("host_names").ToString().Split(',');
            ticket.AssignmentGroupName=(string)json.SelectToken("Incidents[0].AssignmentGroupName");
            ticket.AffectedConfigurationItem = (string)json.SelectToken("Incidents[0].AffectedConfigurationItem.Name");
            ticket.Description = (string)json.SelectToken("Incidents[0].Description");
            ticket.CompanyCode = (string)json.SelectToken("MessageHeader.UserContext.CompanyCode");
            ticket.IncidentId = (string)json.SelectToken("Incidents[0].IncidentId");
            return ticket;
        }
        
    }
}