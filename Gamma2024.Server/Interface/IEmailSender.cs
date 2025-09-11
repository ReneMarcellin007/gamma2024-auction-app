namespace Gamma2024.Server.Interface
{
    public interface IEmailSender
    {
        void Send(string to, string subject, string html, byte[]? attachment = null, string? attachmentName = null, string? from = null);
    }
}
