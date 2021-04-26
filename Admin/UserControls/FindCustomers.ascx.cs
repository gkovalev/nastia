//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI.WebControls;
using AdvantShop.Core.FieldFilters;
using AdvantShop.Customers;
using Resources;

public partial class Admin_UserControls_FindCustomers : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        UpdateCustomerNavigatePanel();
    }

    protected void lbNextPage_Click(object sender, EventArgs e)
    {
        var temp = Convert.ToInt32(currentPage.Value) + 1;
        currentPage.Value = temp.ToString();
        spdsCustomers.CurrentPageIndex = temp;
    }

    protected void lbPreviousPage_Click(object sender, EventArgs e)
    {
        var temp = Convert.ToInt32(currentPage.Value) - 1;
        currentPage.Value = temp.ToString();
        spdsCustomers.CurrentPageIndex = temp;
    }

    protected void ddlCurrentPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        spdsCustomers.CurrentPageIndex = Convert.ToInt32(ddlCurrentPage.SelectedValue);
    }
    private void UpdateCustomerNavigatePanel()
    {
        if (CustomerSession.CurrentCustomer.CustomerRole == Role.Moderator)
        {
            spdsCustomers.Fields["CustomerRole"].Filter = new EqualFieldFilter() { ParamName = "@customerRole", Value = ((int)Role.User).ToString() };
        }

        rCustomers.DataSource = spdsCustomers.Items;
        rCustomers.DataBind();

        if (rCustomers.Controls.Count < 3)
        {
            var tr = new TableRow();
            tr.Controls.Add(new TableCell() { Text = Resource.Admin_ViewCustomer_CustomerNotFound });
            rCustomers.Controls.AddAt(1, tr);
        }

        if (spdsCustomers.CurrentPageIndex > 1)
        {
            lbPreviousPage.Enabled = true;
        }

        if (spdsCustomers.PageCount > 1 && spdsCustomers.CurrentPageIndex < spdsCustomers.PageCount)
        {
            lbNextPage.Enabled = true;
        }

        ddlCurrentPage.Items.Clear();

        for (int i = 1; i <= spdsCustomers.PageCount; i++)
        {
            var itm = new ListItem(i.ToString(), i.ToString());
            if (i == spdsCustomers.CurrentPageIndex)
            {
                itm.Selected = true;
            }
            ddlCurrentPage.Items.Add(itm);
        }
    }
    protected void btnFindCustomer_Click(object sender, EventArgs e)
    {
        spdsCustomers.Fields["EMail"].Filter = !string.IsNullOrEmpty(txtSEmail.Text) ? new EqualFieldFilter() { ParamName = "@email", Value = txtSEmail.Text } : null;
        spdsCustomers.Fields["FirstName"].Filter = !string.IsNullOrEmpty(txtSFirstName.Text) ? new EqualFieldFilter() { ParamName = "@firstName", Value = txtSFirstName.Text } : null;
        spdsCustomers.Fields["LastName"].Filter = !string.IsNullOrEmpty(txtSLastName.Text) ? new EqualFieldFilter() { ParamName = "@lastName", Value = txtSLastName.Text } : null;
        spdsCustomers.CurrentPageIndex = 1;
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSEmail.Text = string.Empty;
        txtSFirstName.Text = string.Empty;
        txtSLastName.Text = string.Empty;
        btnFindCustomer_Click(sender, e);
    }

    protected void rCustomers_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.Equals("DeleteCustomer"))
        {
            CustomerService.DeleteCustomer(Guid.Parse((string)e.CommandArgument));
        }
    }
}