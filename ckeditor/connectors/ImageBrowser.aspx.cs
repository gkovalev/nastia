using System;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.IO;
using AdvantShop;
using AdvantShop.Helpers;
using Resources;

public partial class ImageBrowserPage : Page
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
                string.Format(
                    "var name = prompt('{0}:'); if (name == null || name == '') return false; document.getElementById('{1}').value = name;",
                    Resource.Admin_CKEditor_EnterFolderName, NewDirectoryName.ClientID);
        }

        ResizeMessage.Text = "";
        Page.ClientScript.RegisterStartupScript(GetType(), "FocusImageList", string.Format("window.setTimeout(\"document.getElementById('{0}').focus();\", 1);", ImageList.ClientID), true);
    }

    protected void BindDirectoryList()
    {
        ImageFolder = "";
        DirectoryList.DataSource = Directory.GetDirectories(FileImageFolder).Select(directory => directory.Substring(directory.LastIndexOf('\\') + 1));
        DirectoryList.DataBind();
        DirectoryList.Items.Insert(0, "userfiles/images");
    }

    protected void ChangeDirectory(object sender, EventArgs e)
    {
        DeleteDirectoryButton.Enabled = (DirectoryList.SelectedIndex != 0);
        ImageFolder = (DirectoryList.SelectedIndex == 0) ? "" : DirectoryList.SelectedValue + "/";
        SearchTerms.Text = "";
        BindImageList();
        SelectImage(null, null);
    }

    protected void DeleteFolder(object sender, EventArgs e)
    {
        Directory.Delete(FileImageFolder, true);
        BindDirectoryList();
        ChangeDirectory(null, null);
    }

    protected void CreateFolder(object sender, EventArgs e)
    {
        string name = UniqueDirectory(NewDirectoryName.Value);
        Directory.CreateDirectory(FileImageFolderRoot + name);
        BindDirectoryList();
        DirectoryList.SelectedValue = name;
        ChangeDirectory(null, null);
    }

    protected void BindImageList()
    {
        ImageList.Items.Clear();
        var files = Directory.GetFiles(FileImageFolder, "*" + SearchTerms.Text.Replace(" ", "*") + "*").Where(FileHelpers.CheckImageExtension);
        foreach (string file in files)
        {
            ImageList.Items.Add(file.Substring(file.LastIndexOf('\\') + 1));
        }

        if (ImageList.Items.Count > 0)
            ImageList.SelectedIndex = 0;
    }

    protected void Search(object sender, EventArgs e)
    {
        BindImageList();
    }

    protected void SelectImage(object sender, EventArgs e)
    {
        RenameImageButton.Enabled = (ImageList.Items.Count != 0);
        DeleteImageButton.Enabled = (ImageList.Items.Count != 0);
        ResizeImageButton.Enabled = (ImageList.Items.Count != 0);
        ResizeWidth.Enabled = (ImageList.Items.Count != 0);
        ResizeHeight.Enabled = (ImageList.Items.Count != 0);

        if (ImageList.Items.Count == 0)
        {
            Image1.ImageUrl = "";
            ResizeWidth.Text = "";
            ResizeHeight.Text = "";
            return;
        }

        Image1.ImageUrl = ImageFolder+ ImageList.SelectedValue + "?" + new Random().Next(1000);
        ImageMedia img = ImageMedia.Create(File.ReadAllBytes(FileImageFolder + ImageList.SelectedValue));
        ResizeWidth.Text = img.Width.ToString(CultureInfo.InvariantCulture);
        ResizeHeight.Text = img.Height.ToString(CultureInfo.InvariantCulture);
        ImageAspectRatio.Value = (img.Width / (float)img.Height).ToString(CultureInfo.InvariantCulture);

        int pos = ImageList.SelectedItem.Text.LastIndexOf('.');
        if (pos == -1)
            return;
        RenameImageButton.OnClientClick =
            string.Format(
                "var name = prompt('{0}:','{1}'); if (name == null || name == '') return false; $('#{2}').val(name + '{3}');",
                Resource.Admin_CKEditor_EnterFileName,
                ImageList.SelectedItem.Text.Substring(0, pos),
                NewImageName.ClientID,
                ImageList.SelectedItem.Text.Substring(pos));
        OkButton.OnClientClick =
            string.Format(
                "window.top.opener.CKEDITOR.dialog.getCurrent().setValueOf('info', 'txtUrl', encodeURI('{0}{1}')); window.top.close(); window.top.opener.focus();",
                ImageFolder, ImageList.SelectedValue.Replace("'", "\\'"));
    }

    protected void RenameImage(object sender, EventArgs e)
    {
        string filename = UniqueFilename(NewImageName.Value);
        File.Move(FileImageFolder + ImageList.SelectedValue, FileImageFolder + filename);
        BindImageList();
        ImageList.SelectedValue = filename;
        SelectImage(null, null);
    }

    protected void DeleteImage(object sender, EventArgs e)
    {
        File.Delete(FileImageFolder + ImageList.SelectedValue);
        BindImageList();
        SelectImage(null, null);
    }

    protected void ResizeWidthChanged(object sender, EventArgs e)
    {
        var ratio = ImageAspectRatio.Value.TryParseDecimal();
        int width = ResizeWidth.Text.TryParseInt();
        ResizeHeight.Text = "" + (int)(width / ratio);
    }

    protected void ResizeHeightChanged(object sender, EventArgs e)
    {
        var ratio = ImageAspectRatio.Value.TryParseDecimal();
        int height = ResizeHeight.Text.TryParseInt();

        ResizeWidth.Text = "" + (int)(height * ratio);
    }

    protected void ResizeImage(object sender, EventArgs e)
    {
        int width = ResizeWidth.Text.TryParseInt();
        int height = ResizeHeight.Text.TryParseInt();

        ImageMedia img = ImageMedia.Create(File.ReadAllBytes(FileImageFolder + ImageList.SelectedValue));
        img.Resize(width, height);
        File.Delete(FileImageFolder + ImageList.SelectedValue);
        File.WriteAllBytes(FileImageFolder + ImageList.SelectedValue, img.ToByteArray());

        ResizeMessage.Text = Resource.Admin_CKEditor_ImageResized;
        SelectImage(null, null);
    }

    protected void Upload(object sender, EventArgs e)
    {
        if (FileHelpers.CheckImageExtension(UploadedImageFile.FileName))
        {
            string filename = UniqueFilename(UploadedImageFile.FileName);
            UploadedImageFile.SaveAs(FileImageFolder + filename);

            byte[] data = ImageMedia.Create(UploadedImageFile.FileBytes).ToByteArray();
            FileStream file = File.Create(FileImageFolder + filename);
            file.Write(data, 0, data.Length);
            file.Close();

            BindImageList();
            ImageList.SelectedValue = filename;
            SelectImage(null, null);
        }
    }

    protected void Clear(object sender, EventArgs e)
    {
        Session.Remove("viewstate");
    }

    //util methods
    protected string UniqueFilename(string filename)
    {
        string newfilename = filename;

        for (int i = 1; File.Exists(FileImageFolder + newfilename); i++)
        {
            newfilename = filename.Insert(filename.LastIndexOf('.'), string.Format("({0})", i));
        }

        return newfilename;
    }

    protected string UniqueDirectory(string directoryname)
    {
        string newdirectoryname = directoryname;

        for (int i = 1; Directory.Exists(FileImageFolderRoot + newdirectoryname); i++)
        {
            newdirectoryname = directoryname + string.Format("({0})", i);
        }

        return newdirectoryname;
    }

    //properties
    protected string ImageFolderRoot
    {
        get { return ResolveUrl("~/userfiles/image/"); }
    }

    protected string FileImageFolderRoot
    {
        get { return Server.MapPath("~/userfiles/image/"); }
    }

    protected string ImageFolder
    {
        get { return ImageFolderRoot + ViewState["folder"]; }
        set { ViewState["folder"] = value; }
    }

    protected string FileImageFolder
    {
        get { return Server.MapPath(ImageFolder) + @"\"; }
    }
}
