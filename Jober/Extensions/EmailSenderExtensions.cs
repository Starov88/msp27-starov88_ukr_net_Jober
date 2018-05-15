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
            return emailSender.SendEmailAsync(email, "������������ �����������",
                $"���������� ����������� ����������� � ������� Jober.\n" +
                $"��� ������������� ��������� �� <a href='{HtmlEncoder.Default.Encode(link)}'>���� ������.</a>\n" +
                $"���� �� �� ���������������� � ������� Jober, ������ �������������� ��� ������.");
        }
    }
}
