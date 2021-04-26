namespace AdvantShop.Modules.Interfaces
{
    public interface IOrderConfirmation : IModule
    {
        bool IsActive { get; }
        string PageName { get; }
        string FileUserControlOrderConfirmation { get; }
    }
}