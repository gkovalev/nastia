//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Modules.Interfaces
{
    public interface IModuleDetails : IModule
    {
        string RenderToRightColumn();

        string RenderToProductInformation();

        string RenderToBottom();
    }
}
