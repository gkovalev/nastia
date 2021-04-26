using System.Collections.Generic;

namespace AdvantShop.Modules.Interfaces
{
    public interface IMyAccountControls : IModule
    {
        bool IsActiveModule { get; }
        IList<IMyAccountControl> Controls { get; }
    }

    public interface IMyAccountControl
    {
        string NameTab { get; }
        string File { get; }
    }
}