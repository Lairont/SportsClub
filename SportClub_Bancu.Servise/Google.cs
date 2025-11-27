using System;
using System.IO;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;

namespace SportClub_Bancu.Servise
{
    internal class Google
    {
        public async Task SendEmail(string email, string confirmationCode)
        {
            try
            {

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Администрация сайта", "TurAgen@bk.ru"));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = "Добро пожаловать!";


                var htmlBody = $@"
 <html>
 <head>
    <style>
        body {{ font-family: Arial, sans-serif; background-color: #f2f2f2; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 10px; box-shadow: 0px 0px 10px rgba(0,0,0,0.1); }}
        .header {{ text-align: center; margin-bottom: 20px; }}
        .message {{ font-size: 16px; line-height: 1.6; }}
        .code {{ background-color: #f0f0f0; padding: 5px; border-radius: 5px; font-weight: bold; }}
        .container .code {{ text-align: center; }}
    </style>
 </head>
 <body>
    <div class='container'>
        <div class='header'><h1>Добро пожаловать на сайт Туристическое агентство!</h1></div>
        <div class='message'>
            <p>Пожалуйста, введите данный код на сайте, чтобы подтвердить ваш email и завершить регистрацию:</p>
            <p class='code'>{confirmationCode}</p>
        </div>
    </div>
 </body>
 </html>";

                emailMessage.Body = new TextPart("html") { Text = htmlBody };

                string passwordFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "work", "practice", "materials", "passwordPractice.txt");

                if (!File.Exists(passwordFilePath))
                {
                    throw new FileNotFoundException($"Файл с паролем не найден: {passwordFilePath}");
                }

                string password;
                using (var reader = new StreamReader(passwordFilePath))
                {
                    password = await reader.ReadToEndAsync().ConfigureAwait(false);
                }


                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, true).ConfigureAwait(false);
                    await client.AuthenticateAsync("tany.podsekina.82@gmail.com", password).ConfigureAwait(false);
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {

                throw new InvalidOperationException("Не удалось отправить письмо.", ex);
            }
        }

    }
}
