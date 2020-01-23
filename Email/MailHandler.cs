using MailKit.Net.Smtp;
using OtpNet;
using System;

namespace Email
{
    public class SmsTest
    {
        string toMail;
        readonly string fromMail = "knut.noreply@gmail.com";
        readonly string password = "thispasswordis123";
        string message;
        string otp;
        byte[] secretKey;
        bool port; //false = TLS, true = SSL

        public SmsTest(string toMail, string fromMail, string password)
        {
            this.toMail = toMail;
            this.fromMail = fromMail;
            this.password = password;
        }

        public SmsTest(string toMail, string message)
        {
            this.toMail = toMail;
            this.message = message;
        }

        public void sendMail(SmsTest mail)
        {
            
        }

        public string createOtp()
        {
            Random random = new Random();
            string temp = "";
            for(int i = 0; i < 5; i++)
            {
                temp += random.Next(255);
            }

            byte[] secretKey = { 11, 22, 33, 44 };
            var totp = new Totp(secretKey);
            Console.WriteLine(totp);
            var totpCode = totp.ComputeTotp(DateTime.UtcNow);
            return otp;
        }


        public bool createConnection()
        {
            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.gmail.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(fromMail, password); //fromMail = knut.noreply@gmail.com, password = thispasswordis123

                //client.Send(message);
                client.Disconnect(true);
            }

            return true;
        }
    }


}
