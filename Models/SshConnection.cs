using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPSB_Automation.Models
{
    public class SshConnection
    {

        private string host = null;
        private string user = null;
        private string pass = null;
        private SshClient client;
        


        /* Construct Object */
        public SshConnection(string hostIP, string userName, string password) { host = hostIP; user = userName; pass = password; client = new SshClient(this.host, this.user, this.pass); }

        public SshConnection()
        {

        }


        public SshClient connect()
        {



            try
            {


                KeyboardInteractiveAuthenticationMethod kauth = new KeyboardInteractiveAuthenticationMethod(this.user);
                PasswordAuthenticationMethod pauth = new PasswordAuthenticationMethod(this.user, this.pass);

                kauth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);

                ConnectionInfo connectionInfo = new ConnectionInfo(this.host, 22, this.user, pauth, kauth);

                client = new SshClient(connectionInfo);
                client.Connect();



            }
            catch (Exception ex)
            {
                
            }


            void HandleKeyEvent(Object sender, AuthenticationPromptEventArgs e)
            {
                foreach (AuthenticationPrompt prompt in e.Prompts)
                {
                    if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                    {
                        prompt.Response = this.pass;
                    }
                }
            }






            return client;
        }
    }
}