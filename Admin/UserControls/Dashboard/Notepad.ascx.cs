using System;
using System.Globalization;
using System.IO;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

public partial class Admin_UserControls_Notepad : System.Web.UI.UserControl
{
    private string _filename;
    protected void Page_Load(object sender, EventArgs e)
    {
        CKEditorControl1.Language = CultureInfo.CurrentCulture.ToString();
        string path = Server.MapPath("~/App_Data/notepad/");
        FileHelpers.CreateDirectory(path);
        _filename = path + "notepad.html";
        FileHelpers.CreateFile(_filename);
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        CKEditorControl1.Text = GetNotepad();
    }
    protected string GetNotepad()
    {
        try
        {
            Message.Visible = false;

            FileHelpers.CreateFile(_filename);
            using (var sr = new StreamReader(_filename))
            {
                String line = sr.ReadToEnd();
                return line;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            Message.Text = Resources.Resource.Admin_Notepad_LoadError;
            Message.Visible = true;
            return string.Empty;
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SetNotepad(CKEditorControl1.Text);
    }

    protected void SetNotepad(string text)
    {
        try
        {
            Message.Visible = false;
            FileHelpers.CreateFile(_filename);
            using (var wr = new StreamWriter(_filename))
            {
                wr.Write(text);
                Message.Text = Resources.Resource.Admin_Notepad_SaveComplete;
                Message.Visible = true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            Message.Text = Resources.Resource.Admin_Notepad_SaveError;
            Message.Visible = true;
        }
    }
}