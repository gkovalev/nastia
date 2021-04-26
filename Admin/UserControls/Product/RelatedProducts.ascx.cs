using System;
using System.Linq;
using System.Web.UI.WebControls;
using AdvantShop.Catalog;

public partial class Admin_UserControls_RelatedProducts : System.Web.UI.UserControl
{
    public int ProductID { set; get; }
    public int RelatedType { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            popTree.ExceptId = ProductID;
            popTree.UpdateTree(ProductService.GetRelatedProducts(ProductID, (RelatedType)RelatedType).Select(rp => rp.ProductId));
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        rRelatedProducts.DataSource = ProductService.GetRelatedProducts(ProductID, (RelatedType)RelatedType);
        rRelatedProducts.DataBind();
    }

    protected void popTree_Selected(object sender, Admin_UserControls_PopupTreeView.TreeNodeSelectedArgs args)
    {
        foreach (var altId in args.SelectedValues)
        {
            ProductService.AddRelatedProduct(ProductID, Convert.ToInt32(altId), (RelatedType)RelatedType);    
        }
        popTree.UpdateTree(ProductService.GetRelatedProducts(ProductID, (RelatedType)RelatedType).Select(rp => rp.ProductId));
    }

    protected void lbAddRelatedProduct_Click(object sender, EventArgs e)
    {
        popTree.Show();
    }

   protected void rRelatedProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "DeleteRelatedProduct")
        {
            ProductService.DeleteRelatedProduct(ProductID, Convert.ToInt32(e.CommandArgument), (RelatedType)RelatedType);
        }
        popTree.UpdateTree(ProductService.GetRelatedProducts(ProductID, (RelatedType)RelatedType).Select(rp => rp.ProductId));
    }
}