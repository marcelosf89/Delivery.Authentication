using Delivery.Authentication.Crosscutting.Request.UserManagement;

namespace Delivery.Authentication.Application.Command
{
    public interface ICommand<T> where T : class
    {
        void Execute(T request);
    }
}