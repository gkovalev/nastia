//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Orders
{
    public interface IOrder
    {
        int OrderID { get; set; }
        string Number { get; set; }
        string StatusComment { get; set; }
        IOrderCustomer GetOrderCustomer();
        IOrderStatus GetOrderStatus();
    }
}