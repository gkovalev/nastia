using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Catalog;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Resources;

public partial class Admin_UserControls_ProductProperties : UserControl
{
    private List<PropertyValue> _properties;
    protected List<PropertyValue> Properties
    {
        get { return _properties ?? (_properties = PropertyService.GetPropertyValuesByProductId(ProductId)); }
    }

    public int ProductId { set; get; }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillcboPropertyName();
            txtSortIndex0.Text = PropertyService.GetNewPropertyValueSortOrder(ProductId).ToString(CultureInfo.InvariantCulture);
            LoadDataGrid();
        }

        cboPropertyName.CssClass = RadioButtonExistProperty.Checked ? string.Empty : "disabled";
        txtNewPropertyName.CssClass = RadioButtonExistProperty.Checked ? "disabled" : string.Empty;
        RadioButtonNewPropertyValue.Checked = RadioButtonAddNewProperty.Checked || RadioButtonNewPropertyValue.Checked;
        cboPropertyValue.Enabled = !RadioButtonAddNewProperty.Checked;
        RadioButtonExistPropertyValue.Enabled = !RadioButtonAddNewProperty.Checked;
        RadioButtonExistPropertyValue.Checked = RadioButtonExistPropertyValue.Checked && !RadioButtonAddNewProperty.Checked;

        cboPropertyValue.CssClass = RadioButtonExistPropertyValue.Checked ? string.Empty : "disabled";
        txtNewPropertyValue.CssClass = RadioButtonExistPropertyValue.Checked ? "disabled" : string.Empty;

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {

    }
    private void LoadDataGrid()
    {
        FillcboPropertyValue();
        _properties = PropertyService.GetPropertyValuesByProductId(ProductId);
        gvProperties.DataSource = Properties;
        gvProperties.DataBind();
    }

    /// <summary>
    /// Ќажали на кнопку добавить, провер€ем добавить новое свойство или выбрать из существующих
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    protected void btnAddNewExistProperty_Click(object sender, EventArgs e)
    {
        txtNewPropertyName.Text = txtNewPropertyName.Text.Trim();
        txtNewPropertyValue.Text = txtNewPropertyValue.Text.Trim();

        try
        {
            int sortOrder = 0;
            if (!string.IsNullOrEmpty(txtSortIndex0.Text) && int.TryParse(txtSortIndex0.Text, out sortOrder))
            {
                if (RadioButtonExistProperty.Checked)
                {
                    if (RadioButtonExistPropertyValue.Checked)
                    {
                        if (!string.IsNullOrEmpty(cboPropertyValue.SelectedValue))
                            AddExistPropertyValue(Convert.ToInt32(cboPropertyValue.SelectedValue), sortOrder);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(txtNewPropertyValue.Text))
                        {
                            AddNewPropertyValue(Convert.ToInt32(cboPropertyName.SelectedValue), txtNewPropertyValue.Text, sortOrder);
                            //FillcboPropertyValue();
                        }
                        else
                        {
                            lblValueRequired.Visible = true;
                            lblValueRequired.Text = Resource.Admin_m_Product_RequiredField;
                        }
                    }
                }
                else if (RadioButtonAddNewProperty.Checked)
                {
                    if (!string.IsNullOrEmpty(txtNewPropertyName.Text) && !string.IsNullOrEmpty(txtNewPropertyValue.Text))
                    {
                        AddNewPropertyWithValue(txtNewPropertyName.Text, txtNewPropertyValue.Text, sortOrder);
                        FillcboPropertyName();
                    }
                }
            }
            else
            {
                lblValueRequired.Visible = true;
                lblValueRequired.Text = Resource.Admin_m_Product_Validation_Int;
            }

        }
        catch (Exception ex)
        {
            Msg(ex.Message);
        }


        //FillcboPropertyValue();
        LoadDataGrid();

        txtNewPropertyValue.Text = string.Empty;
        txtNewPropertyName.Text = string.Empty;
        txtSortIndex0.Text = PropertyService.GetNewPropertyValueSortOrder(ProductId).ToString(CultureInfo.InvariantCulture);
        //txtSortIndexAdd.Text = txtSortIndex0.Text
    }

    /// <summary>
    ///  ƒобавл€ет продукту свойство с существующим значением
    /// </summary>
    /// <param name="propValId"></param>
    /// <param name="sortOrder"></param>
    /// <remarks></remarks>
    protected void AddExistPropertyValue(int propValId, int sortOrder)
    {
        PropertyService.AddProductProperyValue(propValId, ProductId, sortOrder);
    }

    /// <summary>
    ///  ƒобавл€ет продукту новое значение свойства
    /// </summary>
    /// <param name="propVal"></param>
    /// <param name="sortOrder"></param>
    /// <param name="propId"></param>
    /// <remarks></remarks>
    protected void AddNewPropertyValue(int propId, string propVal, int sortOrder)
    {
        PropertyService.AddProductProperyValue(
            PropertyService.AddPropertyValue(new PropertyValue { PropertyId = propId, Value = propVal }),
            ProductId, sortOrder);
    }

    protected void Msg(string message)
    {
        lblErrorAddProp.Visible = true;
        lblErrorAddProp.Text = message;
    }

    /// <summary>
    /// ƒобавл€ет  продукту новое  несуществующее свойство
    /// </summary>
    /// <remarks></remarks>
    protected void AddNewPropertyWithValue(string propName, string propVal, int sortOrder)
    {

        if (Page.Request["productid"] != null)
        {
            try
            {
                PropertyService.AddProductProperyValue(
                    PropertyService.AddPropertyValue(new PropertyValue
                                                         {
                                                             PropertyId =
                                                                 PropertyService.AddProperty(new Property
                                                                                                 {
                                                                                                     Name = propName,
                                                                                                     UseInFilter =
                                                                                                         true
                                                                                                 }),
                                                             Value = propVal,
                                                             SortOrder = sortOrder
                                                         }), ProductId, sortOrder);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                Msg("Unable to add property with value");
            }
            cboPropertyName.DataBind();
        }

    }

    protected void gvProperties_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PropertyService.DeleteProductPropertyValue(ProductId,
                                                   Convert.ToInt32(
                                                       ((HiddenField)
                                                        gvProperties.Rows[e.RowIndex].FindControl("hfPropValId")).
                                                           Value));
        LoadDataGrid();
    }

    protected void gvProperties_Edit(object sender, EventArgs e)
    {
        LoadDataGrid();
        gvProperties.EditIndex = gvProperties.SelectedIndex;
        gvProperties.DataBind();
    }

    protected void cboPropertyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillcboPropertyValue();
    }


    protected void gvProperties_DataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowState == (DataControlRowState.Selected | DataControlRowState.Edit)) | (e.Row.RowState == (DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit)))
        {
            // var productProps = PropertyService.GetPropertyValuesByProductId(ProductId);
            var ddl = (DropDownList)e.Row.FindControl("ddlValue");
            ddl.Items.Clear();
            var pVal = ((PropertyValue)e.Row.DataItem);
            ddl.Items.Add(new ListItem(pVal.Value, pVal.PropertyValueId.ToString(CultureInfo.InvariantCulture)));
            foreach (
                PropertyValue propVal in
                    PropertyService.GetValuesByPropertyId(pVal.PropertyId).Except(Properties, (prop, prodProp) => prop.PropertyValueId == prodProp.PropertyValueId))
            {
                ddl.Items.Add(new ListItem(propVal.Value, propVal.PropertyValueId.ToString(CultureInfo.InvariantCulture)));
            }
            ddl.SelectedValue = ((PropertyValue)e.Row.DataItem).PropertyValueId.ToString(CultureInfo.InvariantCulture);

        }
    }

    protected void gvProperties_Cancel(object sender, GridViewCancelEditEventArgs e)
    {
        LoadDataGrid();
        gvProperties.EditIndex = -1;
        gvProperties.DataBind();
    }

    protected void gvProperties_Update(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = gvProperties.Rows[e.RowIndex];
        if (((RadioButton)row.FindControl("rbExistValue")).Checked)
        {
            var ddlValue = (DropDownList)row.FindControl("ddlValue");
            var txtSortOrder = (TextBox)row.FindControl("txtSortOrder");
            if (ValidationHelper.IsValidPositiveIntNumber(ddlValue.SelectedValue))
            {
                if (ValidationHelper.IsValidPositiveIntNumber(txtSortOrder.Text))
                    PropertyService.UpdateProductPropertyValueWithSort(ProductId, Convert.ToInt32(((HiddenField)row.FindControl("hfPropValId")).Value),
                        Convert.ToInt32(ddlValue.SelectedValue),
                        Convert.ToInt32(txtSortOrder.Text));
            }
            else
                throw new Exception("gvProperties not bound");
        }
        else
        {
            var hfPropValId = (HiddenField) row.FindControl("hfPropValId");
            var txtValue = (TextBox) row.FindControl("txtValue");
            var txtSortOrder = (TextBox) row.FindControl("txtSortOrder");

            int sort = 0;
            if (ValidationHelper.IsValidPositiveIntNumber(txtSortOrder.Text))
                sort = Convert.ToInt32(txtSortOrder.Text);

            if (ValidationHelper.IsValidPositiveIntNumber(hfPropValId.Value) && !string.IsNullOrEmpty(txtValue.Text))
                PropertyService.UpdateProductPropertyValueWithSort(ProductId, Convert.ToInt32(hfPropValId.Value), txtValue.Text, sort);
        }
        gvProperties.EditIndex = -1;
        //gvProperties.DataBind();
        LoadDataGrid();
    }

    private void FillcboPropertyName()
    {
        cboPropertyName.Items.Clear();
        foreach (Property prop in PropertyService.GetAllProperties())
        {
            cboPropertyName.Items.Add(new ListItem(prop.Name, prop.PropertyId.ToString(CultureInfo.InvariantCulture)));
        }
        cboPropertyName.DataBind();
        FillcboPropertyValue();
    }
    private void FillcboPropertyValue()
    {
        cboPropertyValue.Items.Clear();
        if (string.IsNullOrEmpty(cboPropertyName.SelectedValue)) return;

        List<PropertyValue> propList =
            PropertyService.GetValuesByPropertyId(Convert.ToInt32(cboPropertyName.SelectedValue));

        foreach (PropertyValue propVal in propList.Except(Properties, (prop, prodProp) => prop.PropertyValueId == prodProp.PropertyValueId).OrderBy(p => p.Value))
        {
            cboPropertyValue.Items.Add(new ListItem(propVal.Value, propVal.PropertyValueId.ToString(CultureInfo.InvariantCulture)));
        }
        cboPropertyValue.DataBind();
    }
}