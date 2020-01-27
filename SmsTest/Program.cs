using System;
using System.Linq;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using OtpNet;
using SmsTest;

namespace MailTest
{
    class Program
    {
        static void Main()
        {

            byte[] secretKey = { 11, 22, 33, 44 };
            var totp = new Totp(secretKey);
            Console.WriteLine(totp);
            var totpCode = totp.ComputeTotp(DateTime.UtcNow);
            Console.WriteLine(totpCode);
            var remainingSeconds = totp.RemainingSeconds(DateTime.UtcNow);
            Console.WriteLine(remainingSeconds);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Knut Erik", "knut.noreply@gmail.com"));
            message.To.Add(new MailboxAddress("Knut E", "knute_knut@hotmail.com"));
            message.Subject = "How you doin'?";

            message.Body = new TextPart("plain")
            {
                Text = @"Hey Chandler,
I just wanted to let you know that Monica and I were going to go play some paintball, you in?
-- Joey" + " " + totp + " " + totpCode + " " + remainingSeconds
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("knut.noreply@gmail.com", "thispasswordis123");

                client.Send(message);
                client.Disconnect(true);
            }

            using (var db = new UserContext())
            {
                // Create
                Console.WriteLine("Inserting a new user");
                db.Add(new User { UserName = "admin" });
                db.SaveChanges();

                // Read
                Console.WriteLine("Querying for a user");
                var user = db.Users
                    .OrderBy(b => b.UserName)
                    .First();
                Console.WriteLine(user.UserName);

                // Update
                Console.WriteLine("Updating the user and adding a password");
                user.UserName = "superAdmin";
                user.UserPassword = totpCode;
                db.SaveChanges();
                Console.WriteLine(user.UserPassword);

                // Delete
                Console.WriteLine("Delete the user");
                db.Remove(user);
                db.SaveChanges();
            }
        }
    }
}