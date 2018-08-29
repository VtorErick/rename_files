using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPSB_Automation.Models
{
    public class PasswordExpired
    {

        public bool CheckPassswd()
        {
            string host = "c1t19440.itcs.hpicorp.net";
            string svcAccount = "acostadu";
            string passwd = "cat.bot-0";
            string userAffected = "hrsamba";
            string OS = "";
            string cmdUpdatePasswd = "passwd -n 7 -x 30 " + userAffected;
            bool exit = false;
            SshConnection con = new SshConnection(host, svcAccount, passwd);
            try
            {
                SshClient sshCon = con.connect();
                //Based on the OS will
                //Run the commmand to check if passwd expired and extends 30 days if necessary
                OS = sshCon.RunCommand("uname").Result;
                int diffDays = 0;
                int extendedDays = 0;
                if (OS == "Linux\n")
                {

                    string startDate = sshCon.RunCommand("passwd -S " + userAffected + " | awk '{print $3}'").Result;
                    DateTime startD = DateTime.Parse(startDate);
                    if (startDate != "\n")
                    {
                        string currentDate = sshCon.RunCommand("date +%Y/%m/%d").Result;
                        DateTime currentD = DateTime.Parse(currentDate);
                        diffDays = Convert.ToInt32((currentD - startD).TotalDays);
                        extendedDays = Convert.ToInt32(sshCon.RunCommand("passwd -S " + userAffected + " | awk '{print $5}' ").Result);
                        //Change passwd if current date is newer than last passwd change
                        //the diff is bigger than extended days and extendedDays is different than 30
                        if (diffDays > 0 && extendedDays < diffDays && extendedDays != 30)
                        {
                            sshCon.RunCommand(cmdUpdatePasswd);
                            return true;
                        }
                    }

                }
                if (OS == "HP-UX\n")
                {
                    string startDate = sshCon.RunCommand("passwd -s " + userAffected + "| awk '{print $3}';").Result;
                    DateTime startD = DateTime.Parse(startDate);
                    if (startDate != "\n")
                    {
                        string currentDate = sshCon.RunCommand("date +%d/%m/%Y").Result;
                        DateTime currentD = DateTime.Parse(currentDate);
                        diffDays = Convert.ToInt32((currentD - startD).TotalDays);
                        extendedDays = Convert.ToInt32(sshCon.RunCommand("passwd -S " + userAffected + " | awk '{print $5}' ").Result);
                        //Change passwd if current date is newer than last passwd change
                        //the diff is bigger than extended days and extendedDays is different than 30
                        if (diffDays > 0 && extendedDays < diffDays && extendedDays != 30)
                        {
                            sshCon.RunCommand(cmdUpdatePasswd);
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return exit;

        }
    }
}
