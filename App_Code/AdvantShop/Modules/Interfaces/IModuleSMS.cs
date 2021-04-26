namespace AdvantShop.Modules.Interfaces
{
    public interface IModuleSms : IModule
    {
        bool IsActive { get; }

        void SendSms(string phoneNumber, string text);
    }
}