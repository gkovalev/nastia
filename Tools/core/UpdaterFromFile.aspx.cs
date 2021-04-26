//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using Advantshop_Tools;

public partial class Tools_core_UpdaterFromFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (!(ckbUpdate.Checked && ckbUpdate1.Checked))
        {
            return;
        }

        //1. проверка на возможность обновления
        //var compareCodeInf = UpdaterService.CompareCodeVersions();
        //if (!string.IsNullOrEmpty(compareCodeInf))
        //{
        //    // вывести отличия
        //    return;
        //}
        //var compareBaseInf = UpdaterService.CompareBaseVersions();
        //if (!string.IsNullOrEmpty(compareBaseInf))
        //{
        //    // вывести отличия
        //    return;
        //}
        
        //2. Создание бэкапов
        UpdaterService.CreateAdvantshopBackups();
        
        //3. скачваем последнюю версию двига и обновляем
        UpdaterService.UpdateAvantshop();
    }
}