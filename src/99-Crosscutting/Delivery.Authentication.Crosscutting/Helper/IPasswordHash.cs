using System.Text;

namespace Delivery.Authentication.Crosscutting.Helper
{
    public interface IPasswordHash
    {
        string Converter(string text, Encoding enc);
    }
}