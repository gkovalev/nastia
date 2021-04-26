//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using Advantshop_Tools;

public partial class Tools_core_Updater : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var versInf = UpdaterService.GetLastVersionInformation();
        lblLastVersion.Text = versInf.lastVersion;

        if (!string.IsNullOrEmpty(versInf.versionHistory))
        {
            lblVersionInformation.Text = versInf.versionHistory;
        }
        else
        {
            divMoreInf.Visible = false;
            btn_update.Visible = false;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!(ckbUpdate.Checked && ckbUpdate1.Checked))
        {
            return;
        }

        //1. �������� �� ����������� ����������
        var compareCodeInf = UpdaterService.CompareCodeVersions();
        if (!string.IsNullOrEmpty(compareCodeInf))
        {
            // ������� �������
            return;
        }
        var compareBaseInf = UpdaterService.CompareBaseVersions();
        if (!string.IsNullOrEmpty(compareBaseInf))
        {
            // ������� �������
            return;
        }
        
        //2. �������� �������
        UpdaterService.CreateAdvantshopBackups();
        
        //3. �������� ��������� ������ ����� � ���������
        UpdaterService.UpdateAvantshop();
    }
}