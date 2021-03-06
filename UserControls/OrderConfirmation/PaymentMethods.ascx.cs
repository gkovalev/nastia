using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Shipping;

public partial class UserControls_PaymentMethods : UserControl
{
    private int _selectedIndex = -1;
    private int _selectedId;
    private Dictionary<string, MethodInfo> _methodInfo;
    private bool _loaded = false;

    public int ShippingMethodId { get; set; }

    public int SelectedID
    {
        get
        {
            if (_methodInfo.ContainsKey(hfPaymentMethodId.Value))
                _selectedId = Convert.ToInt32(hfPaymentMethodId.Value);
            return _selectedId;
        }
        set
        {
            _selectedId = value;
            hfPaymentMethodId.Value = value.ToString();
        }
    }
    public int SelectedIndex
    {
        get { return lvPaymentMethod.SelectedIndex; }
        set { _selectedIndex = value; }
    }

    public ShippingOptionEx Ext { get; set; }

    public PaymentType SelectedType
    {
        get { return _methodInfo.ContainsKey(SelectedID.ToString()) ? _methodInfo[SelectedID.ToString()].PaymentType : 0; }
    }
    public string SelectedText
    {
        get { return _methodInfo.ContainsKey(SelectedID.ToString()) ? _methodInfo[SelectedID.ToString()].MethodName : string.Empty; }
    }
    public ProcessType SelectedProcessType
    {
        get { return _methodInfo.ContainsKey(SelectedID.ToString()) ? _methodInfo[SelectedID.ToString()].ProcessType : 0; }
    }

    public bool HasMethods
    {
        get { if (!_loaded) LoadMethods(string.Empty, string.Empty, false, false); return lvPaymentMethod.Items.Count > 0; }
    }
    protected void Page_Init(object sender, EventArgs e)
    {
        if (ShoppingCartService.CurrentShoppingCart.Count(item => item.ItemType == EnumItemType.Product) > 0)
        {
            _methodInfo = PaymentService.GetAllPaymentMethods(true).ToDictionary(pm => pm.PaymentMethodID.ToString(),
                                     pm =>
                                     new MethodInfo
                                     {
                                         PaymentType = pm.Type,
                                         ProcessType = pm.ProcessType,
                                         MethodName = pm.Name
                                     });
        }
        else
        {
            _methodInfo =
                PaymentService.GetAllPaymentMethods(true).Where(
                    item => item.Type != PaymentType.Cash && item.Type != PaymentType.CashOnDelivery).ToDictionary(
                        pm => pm.PaymentMethodID.ToString(),
                        pm =>
                        new MethodInfo
                            {
                                PaymentType = pm.Type,
                                ProcessType = pm.ProcessType,
                                MethodName = pm.Name
                            });
        }
    }

    public void LoadMethods(string countryName, string cityName, bool displayCertificateMetod, bool hideCashMetod)
    {
        var returnPayment = new List<PaymentMethod>();
        if (displayCertificateMetod)
        {
            var certificateMethod = PaymentService.GetPaymentMethodByType(PaymentType.GiftCertificate);
            if (certificateMethod == null)
            {
                certificateMethod = new PaymentGiftCertificate()
                    {
                        Enabled = true,
                        Name = Resources.Resource.Client_GiftCertificate,
                        Description = Resources.Resource.Payment_GiftCertificateDescription,
                        SortOrder = 0
                    };

                PaymentService.AddPaymentMethod(certificateMethod);
            }
            returnPayment.Add(certificateMethod);
        }
        else
        {
            foreach (var method in PaymentService.GetAllPaymentMethods(true))
            {
                if (method.Type == PaymentType.GiftCertificate)
                    continue;

                if (hideCashMetod && (method.Type == PaymentType.Cash || method.Type == PaymentType.CashOnDelivery))
                    continue;

                if (ShippingMethodId !=0 && ShippingMethodService.IsPaymentNotUsed(ShippingMethodId, method.PaymentMethodID))
                    continue;

                if (Ext == null)
                {
                    if (method.Type != PaymentType.CashOnDelivery && method.Type != PaymentType.PickPoint)
                        returnPayment.Add(method);
                }
                else
                {
                    switch (method.Type)
                    {
                        case PaymentType.CashOnDelivery:
                            if (Ext.Type == ExtendedType.CashOnDelivery && Ext.ShippingId == int.Parse(method.Parameters[CashOnDelivery.ShippingMethodTemplate]))
                            {
                                method.Description = CashOnDelivery.GetDecription(Ext);
                                returnPayment.Add(method);
                            }
                            break;
                        case PaymentType.PickPoint:
                            if (Ext.Type == ExtendedType.Pickpoint && Ext.ShippingId == int.Parse(method.Parameters[PickPoint.ShippingMethodTemplate]))
                            {
                                method.Description = Ext.PickpointAddress;
                                returnPayment.Add(method);
                            }
                            break;
                        default:
                            returnPayment.Add(method);
                            break;
                    }
                }
            }
        }

        var paymentMethods = UseGeoMapping(returnPayment, countryName, cityName);
        _selectedIndex = paymentMethods.FindIndex(item => item.PaymentMethodID == SelectedID);
        lvPaymentMethod.DataSource = paymentMethods;
        lvPaymentMethod.SelectedIndex = _selectedIndex != -1 ? _selectedIndex : _selectedIndex = 0;
        lvPaymentMethod.DataBind();
        if (paymentMethods.Count > 0)
        {
            hfPaymentMethodId.Value = paymentMethods[_selectedIndex].PaymentMethodID.ToString();
        }
        _loaded = true;
    }

    private List<PaymentMethod> UseGeoMapping(IEnumerable<PaymentMethod> listMethods, string countryName, string cityName)
    {
        var items = new List<PaymentMethod>();
        foreach (var elem in listMethods)
        {
            if (ShippingPaymentGeoMaping.IsExistGeoPayment(elem.PaymentMethodID))
            {
                if (ShippingPaymentGeoMaping.CheckPaymentEnabledGeo(elem.PaymentMethodID, countryName, cityName)) items.Add(elem);
            }
            else
            {
                items.Add(elem);
            }
        }

        return items;
    }

    internal class MethodInfo
    {
        public PaymentType PaymentType;
        public ProcessType ProcessType;
        public string MethodName;
    }
}