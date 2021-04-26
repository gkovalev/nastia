using System;
using System.Web.UI.WebControls;
using AdvantShop.Configuration;
using AdvantShop.Mails;
using AdvantShop;

public partial class Admin_UserControls_Settings_MailChimpSettings : System.Web.UI.UserControl
{
    public string ErrMessage = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadData();
    }

    private void LoadData()
    {
        txtMailChimpId.Text = SettingsMailChimp.MailChimpId;
        ckbActive.Checked = SettingsMailChimp.MailChimpActive;
        var lists = MailChimp.GetLists(SettingsMailChimp.MailChimpId);
        if (lists != null && lists.Data != null && lists.Data.Count > 0)
        {
            ddlMailChimpListsReg.DataSource = lists.Data;
            ddlMailChimpListsUnReg.DataSource = lists.Data;
            //ddlMailChimpListsAll.DataSource = lists.Data;

            ddlMailChimpListsReg.DataBind();
            ddlMailChimpListsUnReg.DataBind();
            //ddlMailChimpListsAll.DataBind();

            ddlMailChimpListsReg.SelectedValue = SettingsMailChimp.MailChimpRegUsersList.IsNotEmpty() ? SettingsMailChimp.MailChimpRegUsersList : "0";
            ddlMailChimpListsUnReg.SelectedValue = SettingsMailChimp.MailChimpUnRegUsersList.IsNotEmpty() ? SettingsMailChimp.MailChimpUnRegUsersList : "0";
            //ddlMailChimpListsAll.SelectedValue = SettingsMailChimp.MailChimpAllUsersList.IsNotEmpty() ? SettingsMailChimp.MailChimpAllUsersList : "0";
        }
        else
        {
            ddlMailChimpListsReg.Items.Clear();
            ddlMailChimpListsUnReg.Items.Clear();
            ddlMailChimpListsReg.Items.Add(new ListItem
            {
                Text = Resources.Resource.Admin_MailChimpSettings_NoListLinking,
                Value = @"0"
            });
            ddlMailChimpListsUnReg.Items.Add(new ListItem
            {
                Text = Resources.Resource.Admin_MailChimpSettings_NoListLinking,
                Value = @"0"
            });
            ddlMailChimpListsReg.DataBind();
            ddlMailChimpListsUnReg.DataBind();
        }
    }

    public bool SaveData()
    {
        if (!ValidateData())
            return false;

        SettingsMailChimp.MailChimpActive = ckbActive.Checked;
        if (SettingsMailChimp.MailChimpId != txtMailChimpId.Text)
        {
            SettingsMailChimp.MailChimpRegUsersList = string.Empty;
            SettingsMailChimp.MailChimpUnRegUsersList = string.Empty;
            SettingsMailChimp.MailChimpId = txtMailChimpId.Text;
        }
        else
        {
            if (!ckbActive.Checked)
                return true;


            SettingsMailChimp.MailChimpId = txtMailChimpId.Text;
            //if (ddlMailChimpListsAll.SelectedValue == "0")
            //{
            //    MailChimp.UnsubscribeListMembers(SettingsMailChimp.MailChimpId, SettingsMailChimp.MailChimpAllUsersList);
            //}
            //else
            //{
            //    MailChimp.SubscribeListMember(SettingsMailChimp.MailChimpId, ddlMailChimpListsAll.SelectedValue,
            //                                 SubscribeService.SubscribeGetAllCustomerEmails());
            //}

            if (ddlMailChimpListsReg.SelectedValue == "0")
            {
                MailChimp.UnsubscribeListMembers(SettingsMailChimp.MailChimpId, SettingsMailChimp.MailChimpRegUsersList);
            }
            else
            {
                MailChimp.SubscribeListMember(SettingsMailChimp.MailChimpId, ddlMailChimpListsReg.SelectedValue,
                                              SubscribeService.SubscribeGetRegCustomerEmails());
            }

            if (ddlMailChimpListsUnReg.SelectedValue == "0")
            {
                MailChimp.UnsubscribeListMembers(SettingsMailChimp.MailChimpId,
                                                 SettingsMailChimp.MailChimpUnRegUsersList);
            }
            else
            {
                MailChimp.SubscribeListMember(SettingsMailChimp.MailChimpId, ddlMailChimpListsUnReg.SelectedValue,
                                              SubscribeService.SubscribeGetUnRegCustomerEmails());
            }

            SettingsMailChimp.MailChimpRegUsersList = ddlMailChimpListsReg.SelectedValue;
            SettingsMailChimp.MailChimpUnRegUsersList = ddlMailChimpListsUnReg.SelectedValue;
            //SettingsMailChimp.MailChimpAllUsersList = ddlMailChimpListsAll.SelectedValue;
        }
        LoadData();
        return true;
    }

    private bool ValidateData()
    {
        var valid = true;

        valid &= !ckbActive.Checked || txtMailChimpId.Text.IsNotEmpty();
        if (valid && ckbActive.Checked)
        {
            valid &= txtMailChimpId.Text.Contains("-") &&
                     txtMailChimpId.Text.LastIndexOf("-") + 1 < txtMailChimpId.Text.Length;
            valid &= MailChimp.GetAccountDetails(txtMailChimpId.Text) != null;
        }
        return valid;
    }
}