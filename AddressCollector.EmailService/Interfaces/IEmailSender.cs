using System;
using System.Collections.Generic;
using System.Text;

namespace AddressCollector.EmailService.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
