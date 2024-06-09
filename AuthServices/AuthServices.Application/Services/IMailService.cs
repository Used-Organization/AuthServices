using Microsoft.AspNetCore.Http;

namespace AuthServices.Application.Services
{
    public interface IMailService
    {
        Task SendEmailWithAttachmentAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null);
        Task SendEmailAsync(string mailTo, string subject, string body);
    }
}
