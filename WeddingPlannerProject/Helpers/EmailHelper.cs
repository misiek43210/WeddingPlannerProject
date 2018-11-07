using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WeddingPlannerProject.Helpers
{
    public static class EmailHelper
    {

        public static void SendEmail(string message, string subject, string email)
        {
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            System.Net.NetworkCredential credentials =
            new System.Net.NetworkCredential("bluebinderss@gmail.com", "Misiek321!");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("bluebinderss@gmail.com");
            msg.To.Add(new MailAddress(email));
            msg.Subject = subject;
            
            msg.IsBodyHtml = true;
            msg.Body = message;

            client.Send(msg);

        }


    }
}