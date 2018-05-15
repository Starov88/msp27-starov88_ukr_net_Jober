using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Jober.Services;

namespace Jober.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Подверждение регистрации",
                $"Пожалуйста подтвердите регистрацию в сервисе Jober.\n" +
                $"Для подтверждения перейдите по <a href='{HtmlEncoder.Default.Encode(link)}'>этой ссылке.</a>\n" +
                $"Если Вы не регистрировались в сервисе Jober, просто проигнорируйте это письмо.");
        }
    }
}
