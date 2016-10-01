using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigTree.Services
{
    public interface IMailService
    {

        void SendMail(string to, string from, string subject, string body);
    }
}
