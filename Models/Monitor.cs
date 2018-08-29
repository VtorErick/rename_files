using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;

namespace HPSB_Automation.Models
{
    public class Monitor
    {
        private string host;
        private string path;
        private string filename;
        private SshClient client;
        public System.Timers.Timer timer = new System.Timers.Timer();
        public int counter = 0;
        private Ticket ticket;


        public string Host { get => host; set => host = value; }
        public string Path { get => path; set => path = value; }
        public string Filename { get => filename; set => filename = value; }

        public Monitor(string host, string path, string filename)
        {
            this.host = host;
            this.path = path;
            this.filename = filename;
        }
        public Monitor()
        {
            
        }

  
        public void monitorUnixLinuxFileGone(SshClient client, Ticket ticket)
        {
          

            this.client = client;
            timer.Interval = 60000; // 60 seconds 
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();
            

        }

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            
            Log log = new Log("C:\\duplicatefilelog.txt");

            string cmdCheck = "ls -l " + this.path + "/" + this.filename + ";";
            var output = client.RunCommand(cmdCheck);
            counter++;
            if (output.ExitStatus == 0 )
            {
                log.writeLog("file arrived to its destination, closing HPSM ticket "+ ticket.IncidentId);
                timer.Stop();
                counter = 0;
                timer.Enabled = false;
                timer.Dispose();
            }
            else if(counter<10 && output.ExitStatus!=0)
            {
                log.writeLog("file haven't arrived yet trying again in 1 minute");
            }
            else if(counter >= 10)
            {
                log.writeLog("file haven't arrived after 10 min this needs futhers checks from ops team ");
                timer.Stop();
                counter = 0;
                timer.Enabled = false;
                timer.Dispose();
            }


               
            

            

        }
    }
}