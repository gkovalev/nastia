using System;
using System.Globalization;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Mails;
using Resources;

public partial class Admin_MailFormatDetail : Page
{
    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }
    
    protected bool AddingNew
    {
        get { return Request["id"] == null || Request["id"].ToLower() == "add"; }
    }

    private int _mailFormatId;
    public int MailFormatId
    {
        get
        {
            return _mailFormatId == 0 ? Int32.Parse(Request["id"]) : _mailFormatId;
        }
        set
        {
            _mailFormatId = value;
        }
    }

    protected void Page_Load()
    {
        CKEditorControl1.Language = CultureInfo.CurrentCulture.ToString();
        if (!IsPostBack)
        {
            MailFormatId = 0;
            lblMessage.Text = "";
            lblMessage.Visible = false;
            MsgErr(true);

            if (AddingNew)
            {
                ddlTypes.DataBind();
                ShowMailFormatTypeDescription();
            }
            else
            {
                btnSave.Text = Resource.Admin_Update;
                lblSubHead.Text = Resource.Admin_MailFormatDetail_Edit;

                MailFormatId = Convert.ToInt32(Request["id"]);
                LoadMailFormat();
            }
        }
    }

    protected void ddlTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowMailFormatTypeDescription();
    }

    private void ShowMailFormatTypeDescription()
    {
        int mailFormatTypeId = Int32.Parse(ddlTypes.SelectedValue);
        MailFormatType mailFormatType = MailFormatService.GetMailFormatType(mailFormatTypeId);
        txtDescription.Text = mailFormatType.Comment;

        Page.Title = string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, Resource.Admin_MailFormat_Header);
    }

    private void LoadMailFormat()
    {
        if (!AddingNew)
        {
            ddlTypes.DataBind();
            MailFormat mailFormat = MailFormatService.GetMailFormat(MailFormatId);
            
            txtName.Text = mailFormat.FormatName;
            lblHead.Text = mailFormat.FormatName;
            CKEditorControl1.Text = mailFormat.FormatText;
            chkActive.Checked = mailFormat.Enable;
            txtSortOrder.Text = mailFormat.SortOrder.ToString(CultureInfo.InvariantCulture);
            ddlTypes.SelectedValue = ((int)mailFormat.FormatType).ToString(CultureInfo.InvariantCulture);
            ShowMailFormatTypeDescription();

            Page.Title = string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, mailFormat.FormatName);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtName.Text.Trim().Length == 0)
        {
            MsgErr(Resource.Admin_MailFormat_NoName);
            return;
        }

        if (CKEditorControl1.Text.Trim().Length == 0)
        {
            MsgErr(Resource.Admin_MailFormat_NoText);
            return;
        }

        int result = 0;
        if (!Int32.TryParse(txtSortOrder.Text, out result))
        {
            MsgErr(Resource.Admin_MailFormat_SortNotNum);
            return;
        }

        MailFormat mailFormat = AddingNew ? new MailFormat() : MailFormatService.GetMailFormat(MailFormatId);
        mailFormat.FormatName = txtName.Text.Trim();
        mailFormat.FormatText = CKEditorControl1.Text.Trim();
        mailFormat.Enable = chkActive.Checked;
        mailFormat.SortOrder = Int32.Parse(txtSortOrder.Text);
        mailFormat.FormatType = (MailType)Int32.Parse(ddlTypes.SelectedValue);

        if (AddingNew)
        {
            MailFormatService.InsertMailFormat(mailFormat);
            Response.Redirect("MailFormat.aspx");
        }
        else
        {
            lblMessage.Text = Resource.Admin_MailFormatDetail_Saved + "<br />";
            lblMessage.Visible = true;
            MailFormatService.UpdateMailFormat(mailFormat);
            LoadMailFormat();
        }
    }

    private void MsgErr(bool clean)
    {
        if (clean)
        {
            Message.Visible = false;
            Message.Text = "";
        }
        else
        {
            Message.Visible = false;
        }
    }

    private void MsgErr(string messageText)
    {
        Message.Visible = true;
        Message.Text = "<br/>" + messageText;
    }

    protected void SqlDataSource1_Init(object sender, EventArgs e)
    {
        SqlDataSource1.ConnectionString = Connection.GetConnectionString();
    }
}