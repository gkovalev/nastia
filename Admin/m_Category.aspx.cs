//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.SEO;
using Resources;
using Image = System.Drawing.Image;

public partial class Admin_m_AddNewCategory : Page
{
    #region eCategoryMode enum

    public enum eCategoryMode
    {
        Edit,
        Create,
        Err
    }

    #endregion

    private eCategoryMode _mode = eCategoryMode.Create;
    private int _categoryId = -1;

    protected override void InitializeCulture()
    {
        AdvantShop.Localization.Culture.InitializeCulture();
    }

    private void MsgErr(bool clean)
    {
        if (clean)
        {
            lblError.Visible = false;
            lblError.Text = string.Empty;
        }
        else
        {
            lblError.Visible = false;
        }
    }

    private void MsgErr(string messageText)
    {
        lblError.Visible = true;
        lblError.Text = messageText + @"<br/>";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        fckBriefDescription.Language = fckDescription.Language = CultureInfo.CurrentCulture.ToString();
        if (!string.IsNullOrEmpty(Request["categoryid"]))
        {
            if (Request["categoryid"] == "Add")
            {
                _mode = eCategoryMode.Create;
            }
            else
            {
                Int32.TryParse(Request["categoryid"], out _categoryId);
                if (_categoryId != -1) _mode = eCategoryMode.Edit;
            }
            if (_categoryId == -1) _mode = eCategoryMode.Err;
        }
        else
        {
            _mode = eCategoryMode.Err;
        }

        Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_m_Category_AddCategory);

        lblImageInfo.Text = string.Format("* {0} {1}x{2}px", Resource.Admin_m_Category_ResultImageSize,
                                          SettingsPictureSize.SmallCategoryImageWidth,
                                          SettingsPictureSize.SmallCategoryImageHeight);

        if (Request["mode"] != null)
        {
            if (Request["mode"] == "edit")
            {
                _mode = eCategoryMode.Edit;
            }
            else if (Request["mode"] == "create")
            {
                _mode = eCategoryMode.Create;
            }
            else
            {
                _mode = eCategoryMode.Err;
            }
        }
        else
        {
            _mode = eCategoryMode.Err;
        }

        //----------------------------------------

        if (!CategoryService.IsExistCategory(_categoryId))
        {
            _mode = eCategoryMode.Err;
        }


        if (_categoryId == 0 && _mode == eCategoryMode.Edit)
        {
            lParent.Text = Resource.Admin_m_Category_No;
        }
        else if (!string.IsNullOrEmpty(tree.SelectedValue))
        {
            var tt = CategoryService.GetCategory(tree.SelectedValue.TryParseInt());
            if (tt != null)
                lParent.Text = tt.Name;
        }


        //----------------------------------------

        if (!IsPostBack)
        {
            var node = new TreeNode { Text = Resource.Admin_m_Category_Root, Value = @"0", Selected = true };
            tree.Nodes.Add(node);

            LoadChildCategories(tree.Nodes[0]);

            IList<Category> parentCategories = CategoryService.GetParentCategories(_categoryId);

            if (parentCategories != null)
            {
                TreeNodeCollection nodes = tree.Nodes[0].ChildNodes;
                for (int i = parentCategories.Count - 1; i >= 0; i--)
                {
                    int ii = i;
                    TreeNode tn =
                        (from TreeNode n in nodes where n.Value == parentCategories[ii].CategoryId.ToString() select n).SingleOrDefault();
                    if (tn != null)
                    {
                        tn.Select();
                        tn.Expand();
                    }
                    else
                    {
                        break;
                    }
                    nodes = tn.ChildNodes;
                }
            }

            if (_mode == eCategoryMode.Edit)
            {
                btnAdd.Text = Resource.Admin_m_Category_Save;
                lblSubHead.Text = Resource.Admin_m_Category_EditCategory;
                LoadCategory(_categoryId);
            }
            else if (_mode == eCategoryMode.Create)
            {
                pnlImage.Visible = false;
                pnlMiniImage.Visible = false;

                txtName.Text = string.Empty;
                txtName.Focus();
                fckDescription.Text = string.Empty;
                txtSortIndex.Text = @"0";

                txtTitle.Text = string.Empty;
                txtMetaKeywords.Text = string.Empty;
                txtMetaDescription.Text = string.Empty;

                btnAdd.Text = Resource.Admin_m_Category_Add;
                lblSubHead.Text = Resource.Admin_m_Category_AddCategory;
            }

            if (_categoryId == 0 && _mode == eCategoryMode.Edit)
            {
                lParent.Text = Resource.Admin_m_Category_No;
                lbParentChange.Visible = false;
            }
            else
            {
                lParent.Text = CategoryService.GetCategory(tree.SelectedValue.TryParseInt()).Name;
            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        MsgErr(true);
        if (_mode == eCategoryMode.Edit)
        {
            if (lblError.Visible == false)
            {
                SaveCategory();

                if (lblError.Visible == false)
                {
                    CommonHelper.RegCloseScript(this, string.Empty);
                }
            }
        }
        else if (_mode == eCategoryMode.Create)
        {
            int catId = CreateCategory();
            if (catId != 0 && lblError.Visible == false)
            {
                CommonHelper.RegCloseScript(this, string.Empty);
            }
        }
    }

    protected void LoadCategory(int catID)
    {
        try
        {
            Category category = CategoryService.GetCategory(catID);
            txtName.Text = category.Name;
            if (category.Picture != null && File.Exists(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, category.Picture.PhotoName)))
            {
                Label10.Text = category.Picture.PhotoName;
                pnlImage.Visible = true;
                Image1.ImageUrl = FoldersHelper.GetImageCategoryPath(CategoryImageType.Big, category.Picture.PhotoName, true);
                Image1.ToolTip = category.Picture.PhotoName;
            }
            else
            {
                Label10.Text = @"No picture";
                pnlImage.Visible = false;
            }

            if (category.MiniPicture != null && File.Exists(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, category.MiniPicture.PhotoName)))
            {
                lblMiniPictureFileName.Text = category.MiniPicture.PhotoName;
                pnlMiniImage.Visible = true;

                imgMiniPicture.ImageUrl = FoldersHelper.GetImageCategoryPath(CategoryImageType.Small, category.MiniPicture.PhotoName, true);
                imgMiniPicture.ToolTip = category.MiniPicture.PhotoName;
            }
            else
            {
                lblMiniPictureFileName.Text = @"No picture";
                pnlMiniImage.Visible = false;
            }

            SubCategoryDisplayStyle.SelectedValue = category.DisplayStyle;
            ChkDisplayChildProducts.Checked = category.DisplayChildProducts;
            ChkDisplayBrands.Checked = category.DisplayBrandsInMenu;
            ChkDisplaySubCategories.Checked = category.DisplaySubCategoriesInMenu;
            fckDescription.Text = category.Description;
            fckBriefDescription.Text = category.BriefDescription;
            txtSortIndex.Text = category.SortOrder.ToString();
            ChkEnableCategory.Checked = category.Enabled;

            txtSynonym.Text = category.UrlPath;

            var meta = MetaInfoService.GetMetaInfo(catID, MetaType.Category);
            if (meta == null)
            {
                category.Meta = new MetaInfo(0, 0, MetaType.Product, string.Empty, string.Empty, string.Empty);
                chbDefaultMeta.Checked = true;
            }
            else
            {
                chbDefaultMeta.Checked = false;
                category.Meta = meta;
                txtTitle.Text = category.Meta.Title;
                txtMetaKeywords.Text = category.Meta.MetaKeywords;
                txtMetaDescription.Text = category.Meta.MetaDescription;
            }
        }
        catch (Exception ex)
        {
            MsgErr(ex.Message + " at LoadCategory");
            Debug.LogError(ex, "at LoadCategory");
        }
    }

    protected void SaveCategory()
    {
        if (_mode == eCategoryMode.Err)
        {
            return;
        }

        int categoryID = _categoryId;

        lblError.Text = string.Empty;
        string synonym = txtSynonym.Text.Trim();

        if (String.IsNullOrEmpty(synonym))
        {
            MsgErr(Resource.Admin_m_Category_NoSynonym);
            return;
        }
        string oldSynonym = UrlService.GetObjUrlFromDb(ParamType.Category, categoryID);

        if (oldSynonym != synonym)
        {
            //Changed by Evgeni to Add synonims in Russian (changed also in aspx)
            var reg = new Regex("^[а-яА-Яa-zA-Z0-9_-]*$");
            if (!reg.IsMatch(synonym))
            {
                MsgErr(Resource.Admin_m_Category_SynonymInfo);
                return;
            }
            if (!UrlService.IsAvalibleUrl(categoryID, ParamType.Category, synonym))
            {
                MsgErr(Resource.Admin_SynonymExist);
                return;
            }
        }

        var c = new Category
                    {
                        CategoryId = categoryID,
                        Name = txtName.Text,
                        ParentCategoryId = tree.SelectedValue.TryParseInt(),
                        Description = fckDescription.Text == "<br />" || fckDescription.Text == "&nbsp;" || fckDescription.Text == "\r\n" ? string.Empty : fckDescription.Text,
                        BriefDescription = fckBriefDescription.Text == "<br />" || fckBriefDescription.Text == "&nbsp;" || fckBriefDescription.Text == "\r\n" ? string.Empty : fckBriefDescription.Text,
                        Enabled = ChkEnableCategory.Checked,
                        DisplayStyle = SubCategoryDisplayStyle.SelectedValue,
                        DisplayChildProducts = ChkDisplayChildProducts.Checked,
                        DisplayBrandsInMenu = ChkDisplayBrands.Checked,
                        DisplaySubCategoriesInMenu = ChkDisplaySubCategories.Checked,
                        UrlPath = synonym,
                        SortOrder = txtSortIndex.Text.TryParseInt()
                    };

        FileHelpers.UpdateDirectories();
        if (PictureFileUpload.HasFile)
        {
            PhotoService.DeletePhotos(_categoryId, PhotoType.CategoryBig);
            if (!FileHelpers.CheckImageExtension(PictureFileUpload.FileName))
            {
                MsgErr(Resource.Admin_ErrorMessage_WrongImageExtension);
                return;
            }

            var tempName = PhotoService.AddPhoto(new Photo(0, categoryID, PhotoType.CategoryBig) { OriginName = PictureFileUpload.FileName });
            if (!string.IsNullOrWhiteSpace(tempName))
            {
                using (Image image = Image.FromStream(PictureFileUpload.FileContent))
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, tempName), SettingsPictureSize.BigCategoryImageWidth, SettingsPictureSize.BigCategoryImageHeight, image);
            }
        }


        if (MiniPictureFileUpload.HasFile)
        {
            PhotoService.DeletePhotos(_categoryId, PhotoType.CategorySmall);

            if (!FileHelpers.CheckImageExtension(MiniPictureFileUpload.FileName))
            {
                MsgErr(Resource.Admin_ErrorMessage_WrongImageExtension);
                return;
            }

            var tempName = PhotoService.AddPhoto(new Photo(0, categoryID, PhotoType.CategorySmall) { OriginName = MiniPictureFileUpload.FileName });
            if (!string.IsNullOrWhiteSpace(tempName))
            {
                using (Image image = Image.FromStream(MiniPictureFileUpload.FileContent))
                    FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, tempName), SettingsPictureSize.SmallCategoryImageWidth, SettingsPictureSize.SmallCategoryImageHeight, image);
            }
        }

        c.Meta = new MetaInfo(0, c.CategoryId, MetaType.Category, txtTitle.Text, txtMetaKeywords.Text, txtMetaDescription.Text);

        if (!CategoryService.UpdateCategory(c, true))
            MsgErr("Failed to save category");
    }

    private int CreateCategory()
    {
        // Validation
        MsgErr(true);

        if (string.IsNullOrEmpty(txtName.Text))
        {
            MsgErr(Resource.Admin_m_Category_NoName);
            return 0;
        }
        if (string.IsNullOrEmpty(txtSynonym.Text))
        {
            MsgErr(Resource.Admin_m_Category_NoSynonym);
            return 0;
        }
        var reg = new Regex("^[a-zA-Z0-9_-]*$");
        if (!reg.IsMatch(txtSynonym.Text))
        {
            MsgErr(Resource.Admin_m_Category_SynonymInfo);
            return 0;
        }

        if (!UrlService.IsAvalibleUrl(ParamType.Category, txtSynonym.Text))
        {
            MsgErr(Resource.Admin_SynonymExist);
            return 0;
        }

        var myCat = new Category
                        {
                            Name = txtName.Text,
                            ParentCategoryId = tree.SelectedValue.TryParseInt(),
                            Description = fckDescription.Text,
                            BriefDescription = fckBriefDescription.Text,
                            SortOrder = txtSortIndex.Text.TryParseInt(),
                            Enabled = ChkEnableCategory.Checked,
                            DisplayChildProducts = ChkDisplayChildProducts.Checked,
                            DisplayStyle = SubCategoryDisplayStyle.SelectedValue,
                            UrlPath = txtSynonym.Text,
                            Meta = new MetaInfo(0, 0, MetaType.Category, txtTitle.Text, txtMetaKeywords.Text, txtMetaDescription.Text)
                        };
        try
        {
            myCat.CategoryId = CategoryService.AddCategory(myCat, true);
            if (myCat.CategoryId == 0)
                return 0;

            if (PictureFileUpload.HasFile)
            {
                var tempName = PhotoService.AddPhoto(new Photo(0, myCat.CategoryId, PhotoType.CategoryBig ) { OriginName = PictureFileUpload.FileName });
                if (!string.IsNullOrWhiteSpace(tempName))
                {
                    using (Image image = Image.FromStream(PictureFileUpload.FileContent))
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Big, tempName), SettingsPictureSize.BigCategoryImageWidth, SettingsPictureSize.BigCategoryImageHeight, image);
                }
            }


            if (MiniPictureFileUpload.HasFile)
            {
                var tempName = PhotoService.AddPhoto(new Photo(0, myCat.CategoryId, PhotoType.CategorySmall) { OriginName = MiniPictureFileUpload.FileName });
                if (!string.IsNullOrWhiteSpace(tempName))
                {
                    using (Image image = Image.FromStream(MiniPictureFileUpload.FileContent))
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetImageCategoryPathAbsolut(CategoryImageType.Small, tempName), SettingsPictureSize.SmallCategoryImageWidth, SettingsPictureSize.SmallCategoryImageHeight, image);
                }
            }

            return myCat.CategoryId;
        }
        catch (Exception ex)
        {
            MsgErr(ex.Message + "at CreateCategory");
            Debug.LogError(ex);
        }
        return 0;
    }

    protected void btnDeleteImage_Click(object sender, EventArgs e)
    {
        if (_mode == eCategoryMode.Edit)
        {
            PhotoService.DeletePhotos(_categoryId, PhotoType.CategoryBig);
            pnlImage.Visible = false;
        }
    }

    protected void btnDeleteMiniImage_Click(object sender, EventArgs e)
    {
        if (_mode == eCategoryMode.Edit)
        {
            PhotoService.DeletePhotos(_categoryId, PhotoType.CategorySmall);
            pnlMiniImage.Visible = false;
        }
    }

    public void PopulateNode(object sender, TreeNodeEventArgs e)
    {
        LoadChildCategories(e.Node);
    }

    private void LoadChildCategories(TreeNode node)
    {
        foreach (Category c in CategoryService.GetChildCategoriesByCategoryId(node.Value.TryParseInt(), false))
        {
            if (c.CategoryId != _categoryId || _mode == eCategoryMode.Create)
            {
                var newNode = new TreeNode
                                  {
                                      Text = string.Format("{0} ({1})", c.Name, c.ProductsCount),
                                      Value = c.CategoryId.ToString()

                                  };
                if (c.HasChild)
                {
                    newNode.Expanded = false;
                    newNode.PopulateOnDemand = true;
                }
                else
                {
                    newNode.Expanded = true;
                    newNode.PopulateOnDemand = false;
                }
                node.ChildNodes.Add(newNode);
            }
        }
    }

    protected void lbParentChange_Click(object sender, EventArgs e)
    {
        mpeTree.Show();
    }

    protected void Select_change(object sender, EventArgs e)
    {
        mpeTree.Show();
        lParent.Text = CategoryService.GetCategory(tree.SelectedValue.TryParseInt()).Name;
    }

    protected void btnUpdateParent_Click(object sender, EventArgs e)
    {
        mpeTree.Hide();
        lParent.Text = CategoryService.GetCategory(tree.SelectedValue.TryParseInt()).Name;
    }
}