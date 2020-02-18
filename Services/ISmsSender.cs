using System.Threading.Tasks;

namespace Terra.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}