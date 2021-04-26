//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Web.Services;
using System.Xml;
using System.Xml.Serialization;
using AdvantShop.Catalog;
using AdvantShop.Security;
using AdvantShop.SEO;

[XmlRoot("Products")]
public class Products : List<Product>
{
}

[XmlRoot("Offers")]
public class Offers : List<Offer>
{
}


// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
// <System.Web.Script.Services.ScriptService()> _
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class ProductSvc : WebService
{

    private const string MsgAddError = "Product adding failed";
    private const string MsgAddSuccess = "Product {0} added successfull";
    private const string MsgCatNotFound = "Category {0} not found";
    private const string MsgDeleteError = "Product {0} deleting failed";
    private const string MsgDeleteSuccess = "Product {0} deleted successfull";
    private const string MsgLinkDelFailed = "Removing product {0} removung from category {1} failed with Sql error {2}";
    private const string MsgLinkDeleted = "Product {0} removed from category {1}";
    private const string MsgLinkError = "Product {0} linking with category {1} failed with Sql error {2}";
    private const string MsgLinkSuccess = "Product {0} added to category {1}";
    private const string MsgNotFound = "Product {0} not found";
    private const string MsgParentNotFound = "Error determining product {0} parent category";
    private const string MsgUpdateError = "Product {0} updating failed";
    private const string MsgUpdateSuccess = "Product {0} updated successfull";

    private const string MsgAuthFailed = "Access denied, please login";
    /// <summary>
    /// LogIn as admin and write login data to cookies
    /// </summary>
    /// <param name="login">Admin login</param>
    /// <param name="password">Admin password</param>
    /// <returns>True if successfull login</returns>
    [WebMethod]
    public bool Login(string login, string password)
    {
        return AuthorizeService.LoginAdmin(login, password, false);
    }
    /// <summary>
    /// Logout and delete user cookies
    /// </summary>
    [WebMethod]
    public void Logout()
    {
        AuthorizeService.DeleteCookie();
    }

    /// <summary>
    /// Gets Product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns></returns>
    /// <remarks></remarks>
    [WebMethod]
    public Product GetProductBy(int id)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return null;
        return ProductService.GetProduct(id);
        //Return GetProduct(id, False)
    }

    /// <summary>
    /// Gets Product by ArtNo
    /// </summary>
    /// <param name="artNo">Product ArtNo</param>
    /// <returns></returns>
    /// <remarks></remarks>
    [WebMethod]
    public Product GetProductBy(string artNo)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return new Product();
        return ProductService.GetProduct(artNo);
        //Return GetProduct(ArtNo, True)
    }

    /// <summary>
    /// Gets ALL products
    /// </summary>
    /// <returns>List of products</returns>
    /// <remarks></remarks>
    [WebMethod]
    public Products GetProducts()
    {
        if (!AuthorizeService.CheckAdminCookies())
            return null;
        var products = new Products();

        products.AddRange(ProductService.GetProducts());

        return products;

    }

    /// <summary>
    /// Gets all products in cpecified category
    /// </summary>
    /// <param name="catId">Category ID</param>
    /// <returns>List of products in category</returns>
    /// <remarks></remarks>
    [WebMethod]
    public Products GetProductsByCategoryID(int catId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return null;
        var products = new Products();
        products.AddRange(CategoryService.GetProductsByCategoryId(catId));
        return products;
    }

    /// <summary>
    /// Adding new product
    /// </summary>
    /// <param name="product"></param>
    /// <returns>String message</returns>
    /// <remarks></remarks>
    [WebMethod]
    public string AddProduct(Product product)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        return ProductService.AddProduct(product, true) != 0 ? string.Format(MsgAddSuccess, product.ProductId) : MsgAddError;
    }

    /// <summary>
    /// Updates specified product info (including offers and page meta)
    /// </summary>
    /// <param name="product"></param>
    /// <returns>String message</returns>
    /// <remarks></remarks>
    [WebMethod]
    public string UpdateProduct(Product product)
    {
        if (!AuthorizeService.CheckAdminCookies())
        {
            return MsgAuthFailed;
        }

        if (ProductService.IsExists(product.ProductId))
        {
            return string.Format(MsgNotFound, product.ProductId);
        }

        if (ProductService.UpdateProduct(product, true))
        {
            return string.Format(MsgUpdateSuccess, product.ProductId);
        }

        return string.Format(MsgUpdateError, product.ProductId);
    }

    /// <summary>
    /// Updates products page meta
    /// </summary>
    /// <param name="productID">Product ID</param>
    /// <param name="meta">New MetInfo</param>
    /// <returns>StringMessage</returns>
    /// <remarks></remarks>
    [WebMethod]
    public string UpdateProductMeta(int productID, MetaInfo meta)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        Product p = ProductService.GetProduct(productID);
        if (p == null)
        {
            return string.Format(MsgNotFound, productID);
        }
        p.Meta = meta;
        MetaInfoService.SetMeta(p.Meta);
        return string.Format(MsgUpdateSuccess, productID);
    }

    /// <summary>
    /// Updates product offers
    /// </summary>
    /// <param name="productID"></param>
    /// <param name="offers"></param>
    /// <returns>String message</returns>
    /// <remarks></remarks>
    [WebMethod]
    public string UpdateProductOffers(int productID, Offers offers)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;

        if (ProductService.IsExists(productID))
        {
            return string.Format(MsgNotFound, productID);
        }

        OfferService.UpdateOffersByProductId(productID, offers);
        
            return string.Format(MsgUpdateSuccess, productID);
        

        return string.Format(MsgUpdateError, productID);
    }

    /// <summary>
    /// Set parent category of the product
    /// </summary>
    /// <param name="productID"></param>
    /// <param name="categoryID"></param>
    /// <returns>String message</returns>
    /// <remarks></remarks>
    [WebMethod]
    public string AddProductToCategory(int productID, int categoryID)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        if (ProductService.GetProduct(productID) == null)
        {
            return string.Format(MsgNotFound, productID);
        }
        if (CategoryService.GetCategory(categoryID) == null)
        {
            return string.Format(MsgCatNotFound, categoryID);
        }
        int errCode = ProductService.AddProductLink(productID, categoryID);
        if (errCode == 0)
        {
            return string.Format(MsgLinkSuccess, productID);
        }
        return string.Format(MsgLinkError, productID, categoryID, errCode);
    }

    /// <summary>
    /// Remover product from category
    /// </summary>
    /// <param name="productID"></param>
    /// <param name="categoryID"></param>
    /// <returns>String message</returns>
    /// <remarks></remarks>
    [WebMethod]
    public string RemoveProductFromCategory(int productID, int categoryID)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        if (ProductService.GetProduct(productID) == null)
        {
            return string.Format(MsgNotFound, productID);
        }
        if (CategoryService.GetCategory(categoryID) == null)
        {
            return string.Format(MsgCatNotFound, categoryID);
        }
        int errCode = ProductService.DeleteProductLink(productID, categoryID);
        if (errCode == 0)
        {
            return string.Format(MsgLinkDeleted, productID, categoryID);
        }
        return string.Format(MsgLinkDelFailed, productID, categoryID, errCode);
    }

    /// <summary>
    /// Deletes product by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>String message</returns>
    /// <remarks></remarks>
    [WebMethod]
    public string DeleteProduct(int id)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return MsgAuthFailed;
        if (ProductService.GetProduct(id) == null)
        {
            return string.Format(MsgNotFound, id);
        }
        if (ProductService.DeleteProduct(id, false))
        {
            return string.Format(MsgDeleteError, id);
        }
        return string.Format(MsgDeleteSuccess, id);
    }

    /// <summary>
    /// Gets product first parent category
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    /// <remarks></remarks>
    [WebMethod]
    public XmlDocument GetParentCategory(int productId)
    {
        if (!AuthorizeService.CheckAdminCookies())
            return ErrMsg(MsgAuthFailed);

        Product p = ProductService.GetProduct(productId);
        int catId;
        if (p.ProductCategories.Count > 0)
        {
            catId = p.CategoryID;
        }
        else
        {
            return ErrMsg(string.Format(MsgParentNotFound, productId));
        }
        return CategoryService.ConvertToXml(catId);
    }

    private XmlDocument ErrMsg(string errTxt)
    {
        var result = new XmlDocument();
        XmlElement errorXml = result.CreateElement("Error");
        errorXml.InnerText = errTxt;
        result.AppendChild(errorXml);
        return result;
    }
}