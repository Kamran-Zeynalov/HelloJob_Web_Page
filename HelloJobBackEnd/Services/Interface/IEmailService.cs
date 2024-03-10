namespace HelloJobBackEnd.Services.Interface
{
    public interface IEmailService
    {
        void SendEmail(string recipientEmail, string subject, string body);
    }
}
