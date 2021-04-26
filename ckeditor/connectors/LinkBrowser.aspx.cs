using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using AdvantShop.Helpers;
using Resources;

public partial class LinkBrowserPage : Page
{

    protected void Page_Init(object sender, EventArgs e)
    {
        AdvantShop.Security.Secure.VerifySessionForErrors();
        AdvantShop.Security.Secure.VerifyAccessLevel();
        CommonHelper.DisableBrowserCache();
    }

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDirectoryList();
            ChangeDirectory(null, null);

            NewDirectoryButton.OnClientClick =
                string.Format("var name = prompt('{0}:'); if (name == null || name == '') return false; $('#{1}').val(name)", Resource.Admin_CKEditor_EnterFolderName, NewDirectoryName.ClientID);
        }

        Page.ClientScript.RegisterStartupScript(GetType(), "FocusImageList", string.Format("window.setTimeout(\"$('#{0}').focus();\", 1);", ImageList.ClientID), true);
    }

    protected void BindDirectoryList()
    {
        LinkFolder = "";
        DirectoryList.DataSource = Directory.GetDirectories(FileLinkFolder).Select(directory => directory.Substring(directory.LastIndexOf('\\') + 1));
        DirectoryList.DataBind();
        DirectoryList.Items.Insert(0, "userfiles/file");
    }

    protected void ChangeDirectory(object sender, EventArgs e)
    {
        DeleteDirectoryButton.Enabled = (DirectoryList.SelectedIndex != 0);
        LinkFolder = (DirectoryList.SelectedIndex == 0) ? "" : DirectoryList.SelectedValue + "/";
        SearchTerms.Text = "";
        BindImageList();
        SelectImage(null, null);
    }

    protected void DeleteFolder(object sender, EventArgs e)
    {
        Directory.Delete(FileLinkFolder, true);
        BindDirectoryList();
        ChangeDirectory(null, null);
    }

    protected void CreateFolder(object sender, EventArgs e)
    {
        string name = UniqueDirectory(NewDirectoryName.Value);
        Directory.CreateDirectory(FileLinkFolderRoot + name);
        BindDirectoryList();
        DirectoryList.SelectedValue = name;
        ChangeDirectory(null, null);
    }

    protected void BindImageList()
    {
        ImageList.Items.Clear();
        string[] files = Directory.GetFiles(FileLinkFolder, "*" + SearchTerms.Text.Replace(" ", "*") + "*");

        foreach (string file in files)
            ImageList.Items.Add(file.Substring(file.LastIndexOf('\\') + 1));

        if (files.Length > 0)
            ImageList.SelectedIndex = 0;
    }

    protected void Search(object sender, EventArgs e)
    {
        BindImageList();
    }

    protected void SelectImage(object sender, EventArgs e)
    {
        int pos = ImageList.SelectedValue.LastIndexOf('.');
        if (pos == -1)
            return;
        RenameImageButton.OnClientClick =
            string.Format(
                "var name = prompt('{0}:','{1}'); if (name == null || name == '') return false; $('#{2}').val(name + '{3}');",
                Resource.Admin_CKEditor_EnterFileName,
                ImageList.SelectedValue.Substring(0, pos),
                NewImageName.ClientID, ImageList.SelectedValue.Substring(pos));

        string link = LinkFolder + ImageList.SelectedValue;
        OkButton.OnClientClick = string.Format("window.top.opener.CKEDITOR.dialog.getCurrent().setValueOf('info', '{0}', '{1}'); window.top.close(); window.top.opener.focus();return false;", 
            link.ToLower().EndsWith(".swf") ? "src" : "url",
            link);
    }

    protected void RenameImage(object sender, EventArgs e)
    {
        string filename = UniqueFilename(NewImageName.Value);
        File.Move(FileLinkFolder + ImageList.SelectedValue, FileLinkFolder + filename);
        BindImageList();
        ImageList.SelectedValue = filename;
        SelectImage(null, null);
    }

    protected void DeleteImage(object sender, EventArgs e)
    {
        File.Delete(FileLinkFolder + ImageList.SelectedValue);
        BindImageList();
        SelectImage(null, null);
    }

    protected void Upload(object sender, EventArgs e)
    {
        string filename = UniqueFilename(UploadedImageFile.FileName);
        UploadedImageFile.SaveAs(FileLinkFolder + filename);

        BindImageList();
        ImageList.SelectedValue = filename;
        SelectImage(null, null);
    }

    protected void Clear(object sender, EventArgs e)
    {
        Session.Remove("viewstate");
    }

    protected void SelectPage(object sender, EventArgs e)
    {
        OkButton.OnClientClick = string.Format("window.top.opener.SetUrl(encodeURI('{0}{1}')); window.top.close(); window.top.opener.focus();", FileLinkFolder, ImageList.SelectedValue);
    }

    //util methods
    protected string UniqueFilename(string filename)
    {
        string newfilename = filename;

        for (int i = 1; File.Exists(FileLinkFolder + newfilename); i++)
        {
            newfilename = filename.Insert(filename.LastIndexOf('.'), string.Format("({0})", i));
        }

        return newfilename;
    }

    protected string UniqueDirectory(string directoryname)
    {
        string newdirectoryname = directoryname;

        for (int i = 1; Directory.Exists(FileLinkFolderRoot + newdirectoryname); i++)
        {
            newdirectoryname = directoryname + string.Format("({0})", i);
        }

        return newdirectoryname;
    }

    //properties
    protected string LinkFolderRoot
    {
        get { return ResolveUrl("~/userfiles/file/"); }
    }

    protected string FileLinkFolderRoot
    {
        get { return Server.MapPath("~/userfiles/file/"); }
    }

    private string LinkFolder
    {
        get { return LinkFolderRoot + ViewState["folder"]; }
        set { ViewState["folder"] = value; }
    }

    private string FileLinkFolder
    {
        get
        {
            if (ViewState["folder"] != null)
                return FileLinkFolderRoot + ViewState["folder"].ToString().Replace("/", "\\");

            return FileLinkFolderRoot;
        }
    }
}
