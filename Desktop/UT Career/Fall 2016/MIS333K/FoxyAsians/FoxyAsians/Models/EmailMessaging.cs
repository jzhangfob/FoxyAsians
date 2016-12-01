using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Mail;
using System.Net;

namespace FoxyAsians.Models
{
    public class EmailMessaging
    {
        public static void SendEmail(String toEmailAddress, String emailSubject, String emailBody)
        {

            //Create an email client to send the emails
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("longhornmusic21@gmail.com", "katiegray"),
                EnableSsl = true
            };

            //Add anything that you need to the body of this message
            // /n is used for white space
            String finalMessage = emailBody + "\n\n This is a disclaimer that I hope you did not actually buy anything.";
                
            MailAddress senderEmail = new MailAddress("longhornmusic21@gmail.com", "Longhorn Music");

            MailMessage mm = new MailMessage();
            mm.Subject = emailSubject;
            mm.Sender = senderEmail;
            mm.From = senderEmail;
            mm.To.Add(new MailAddress(toEmailAddress));
            mm.Body = finalMessage;
            client.Send(mm);
        }
    }
}