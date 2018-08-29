using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HPSB_Automation.Models
{
    public class Log
    {
        private String fileName;
        private String textToWrite;


        public string FileName { get => fileName; set => fileName = value; }
        public string TextToWrite { get => TextToWrite; set => TextToWrite = value; }

        public Log(String fileName)
        {
            this.fileName = fileName;
           
        }
        public Log()
        {

        }



        public void writeLog(String text)
        {

            if (System.IO.File.Exists(fileName))
            {
                WriteLine(text, false);
            }
        }
        private void WriteLine(string text, bool append = true)
        {
            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(fileName, true, System.Text.Encoding.UTF8))
                {
                    if (text != "")
                    {
                        writer.WriteLine(text);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }


}