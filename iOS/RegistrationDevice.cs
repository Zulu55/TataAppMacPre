using TataAppMac.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(TataAppMac.iOS.RegistrationDevice))]

namespace TataAppMac.iOS
{
    public class RegistrationDevice : IRegisterDevice
    {
        public void RegisterDevice()
        {
        }
    }
}
