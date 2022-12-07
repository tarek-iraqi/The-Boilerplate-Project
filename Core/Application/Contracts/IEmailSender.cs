using System.Threading.Tasks;

namespace Application.Contracts;

public interface IEmailSender
{
    Task SendSingleEmail(string to, string subject, string message, string from = null);
    Task SendSingleEmail(string to, string subject, string templateFileName, object model, string from = null);
    Task SendMultipleEmails(string[] to, string subject, string message, string from = null);
    Task SendMultipleEmails(string[] to, string subject, string templateFileName, object model, string from = null);
}