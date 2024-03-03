using System.Threading.Tasks;

namespace prod.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}