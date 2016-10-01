using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BigTree.Services
{
    public class DebugMailService : IMailService
    {


        public void SendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"SendingMail: To: {0}" , to  + "From: {1}", from + "Subject: {2}", subject);
        }


    }
}

