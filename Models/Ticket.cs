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
        private string archive;
        private string incidentId;
        private string sub_buffers_url;
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
        public string Archive { get => archive; set => archive = value; }
        public string Sub_buffers_url { get => sub_buffers_url; set => sub_buffers_url = value; }

        public Ticket(JObject json)
        {
            this.json = json;
            this.RootText = (string)json.SelectToken("root_text");
            this.Archive = (string)json.SelectToken("archive");
            this.HostArray = json.SelectToken("host_names").ToString().Split(',');
            this.AssignmentGroupName = (string)json.SelectToken("Incidents[0].AssignmentGroupName");
            this.AffectedConfigurationItem = (string)json.SelectToken("Incidents[0].AffectedConfigurationItem.Name");
            this.Description = (string)json.SelectToken("Incidents[0].Description");
            this.CompanyCode = (string)json.SelectToken("MessageHeader.UserContext.CompanyCode");
            this.IncidentId = (string)json.SelectToken("Incidents[0].IncidentId");

        }

        /*public Ticket PopulateTicket(JObject json)
        {
            
        }*/
        
    }
}