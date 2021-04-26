using System.Collections.Generic;
using AdvantShop;
using AdvantShop.Controls;
using AdvantShop.Payment;

public partial class Admin_UserControls_PaymentMethods_MasterBank : ParametersControl
{
    public override Dictionary<string, string> Parameters
    {
        get
        {
            return _valid ||
                   ValidateFormData(new[] { txtTerminal }, null, null)
                       ? new Dictionary<string, string>
                             {
                                 {MasterBankTemplate.Terminal, txtTerminal.Text}
                             }
                       : null;
        }
        set
        {
            txtTerminal.Text = value.ElementOrDefault(MasterBankTemplate.Terminal);

        }
    }

}