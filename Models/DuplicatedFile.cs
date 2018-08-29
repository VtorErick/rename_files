using Microsoft.Win32.SafeHandles;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace HPSB_Automation.Models
{
    public class DuplicatedFile
    {
        private string host = "";
        private string path = "";
        private string user = "gonzalse";
        private string passwd = "C!heecowiizz1";
        private string fullFileName = "";
        private string fileName = "";
        private string extension = "";
        private string cmdRename = "";
        private string cmdCheck = "";
        bool exit = false;
        public DuplicatedFile()
        {

        }
        public bool CheckDuplicate(Ticket ticket)
        {
            // ticket.RootText =  "File already exists: /G/Apps/CSN/archive/sdi/Wrap/Archive/EVENT_FRD_GCSN_20170303052041.gz
            // already exists on the remote filesystem"
            int posPath = ticket.RootText.LastIndexOf("/");
            if (posPath >= 0)
                path = ticket.RootText.Substring(0, posPath);
            path = path.Substring(path.IndexOf("/"),path.Length - path.IndexOf("/"));
            //Path was converted to /G/Apps/CSN/archive/sdi/Wrap/Archive
            int x = 0;
            while (exit!=true)
            {
                host = ticket.HostArray[x];
                fullFileName = ticket.RootText.Substring(ticket.RootText.LastIndexOf("/"), ticket.RootText.Length - ticket.RootText.LastIndexOf("/"));
                fullFileName = fullFileName.Substring(0, fullFileName.IndexOf(" "));
                //Saves extension of the file if it has to be used later while renaming
                int filePos = fullFileName.IndexOf(".");
                if (filePos >= 0)
                    fileName = fullFileName.Substring(0, filePos);

                if (filePos != -1)
                    extension = fullFileName.Substring(filePos, fullFileName.Length - filePos);

                //LINUX CONNECTION
                try
                {
                    SshConnection con = new SshConnection(host, user, passwd);
                    SshClient sshCon = con.connect();
                    if (path.EndsWith("/"))
                        path = path.Substring(0, path.Length - 1);

                    string OS = sshCon.RunCommand("uname").Result;
                    if (OS == "Linux\n")
                    {
                        //Run the commmand to check if file exists and rename if necessary
                        cmdCheck = "/opt/pb/bin/pbrun bash -c 'ls -l " + path + fullFileName + ";'";
                        var command = sshCon.RunCommand(cmdCheck);
                        if (command.ExitStatus == 0)
                        {
                            cmdRename = "/opt/pb/bin/pbrun bash -c 'cd " + path + ";" + "mv " + path + fullFileName + " " + path + fileName + "_$(date +%s)" + extension + ";'";
                            if (sshCon.RunCommand(cmdRename).ExitStatus == 0)
                            {
                                exit = true;
                                return exit;
                            }

                        }
                    }
                    if (OS == "HP-UX\n")
                    {
                        //Run the commmand to check if file exists and rename if necessary
                        cmdCheck = "/opt/pb/bin/pbrun ksh -c 'ls -l " + path + fullFileName + ";'";
                        var command = sshCon.RunCommand(cmdCheck);
                        if (command.ExitStatus == 0)
                        {
                            cmdRename = "/opt/pb/bin/pbrun ksh -c 'cd " + path + ";" + "mv " + path + fullFileName + " " + path + fileName + "_$(date +%s)" + extension + ";'";
                            if (sshCon.RunCommand(cmdRename).ExitStatus == 0)
                            {
                                exit = true;
                            }

                        }
                    }
                }
                catch (Exception e)
                {

                }
                //WINDOWS CONNECTION
                try
                {
                    string DomainSvcAccount = "americas";
                    string winUserSvcAccount = "io1069go";
                    string winPasswdSvcAccount = "C!heecowiizz1";
                    ImpersonationHelper.Impersonate(DomainSvcAccount, winUserSvcAccount, winPasswdSvcAccount, delegate
                    {
                        //Your code here 
                        //If connection was done                         
                        if (File.Exists(@"\\\iten\hpsb.txt"))
                        {
                            string origin = "\\" + host + path.Replace(@"/",@"\") + fileName.Replace(@"/", @"\");
                            string destination = "\\" + host + path.Replace(@"/", @"\") + fileName.Replace(@"/", @"\") + "1";
                            File.Move(@"\\hc4w00433\iten\hpsb.txt", @"\\hc4w00433\iten\hpsb.txt1");
                            exit = true;
                        }
                    });
                }
                catch (Exception e)
                {

                }

                x++;


            }
            //Exit contains a vaule True if ticket was SOLVED
            return exit;
        }

        /************************************/
        /************************************/
        /************************************/
        /************************************/
        /************************************/

        public sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            private SafeTokenHandle()
                : base(true)
            {
            }

            [DllImport("kernel32.dll")]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            [SuppressUnmanagedCodeSecurity]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool CloseHandle(IntPtr handle);

            protected override bool ReleaseHandle()
            {
                return CloseHandle(handle);
            }
        }

        public class ImpersonationHelper
        {
            [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            private static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, out SafeTokenHandle phToken);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
            private extern static bool CloseHandle(IntPtr handle);

            [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
            public static void Impersonate(string domainName, string userName, string userPassword, Action actionToExecute)
            {
                SafeTokenHandle safeTokenHandle;
                try
                {

                    const int LOGON32_PROVIDER_DEFAULT = 0;
                    //This parameter causes LogonUser to create a primary token.
                    const int LOGON32_LOGON_INTERACTIVE = 2;

                    // Call LogonUser to obtain a handle to an access token.
                    bool returnValue = LogonUser(userName, domainName, userPassword,
                        LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                        out safeTokenHandle);
                    //Facade.Instance.Trace("LogonUser called.");

                    if (returnValue == false)
                    {
                        int ret = Marshal.GetLastWin32Error();
                        //Facade.Instance.Trace($"LogonUser failed with error code : {ret}");

                        throw new System.ComponentModel.Win32Exception(ret);
                    }

                    using (safeTokenHandle)
                    {
                        //Facade.Instance.Trace($"Value of Windows NT token: {safeTokenHandle}");
                        //Facade.Instance.Trace($"Before impersonation: {WindowsIdentity.GetCurrent().Name}");

                        // Use the token handle returned by LogonUser.
                        using (WindowsIdentity newId = new WindowsIdentity(safeTokenHandle.DangerousGetHandle()))
                        {
                            using (WindowsImpersonationContext impersonatedUser = newId.Impersonate())
                            {
                                //Facade.Instance.Trace($"After impersonation: {WindowsIdentity.GetCurrent().Name}");
                                //Facade.Instance.Trace("Start executing an action");

                                actionToExecute();

                                //Facade.Instance.Trace("Finished executing an action");
                            }
                        }
                        //Facade.Instance.Trace($"After closing the context: {WindowsIdentity.GetCurrent().Name}");
                    }

                }
                catch (Exception ex)
                {
                    //Facade.Instance.Trace("Oh no! Impersonate method failed.");
                    //ex.HandleException();
                    //On purpose: we want to notify a caller about the issue /Pavel Kovalev 9/16/2016 2:15:23 PM)/
                    throw;
                }
            }
        }


    }

    

}