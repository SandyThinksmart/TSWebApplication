using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TSWebApplication.Application.IServices
{
    public interface IEmailSenderService
    {
        Task SendEmail(string txtfileName);
    }
}
