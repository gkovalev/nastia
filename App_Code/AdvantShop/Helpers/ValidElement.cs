//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Summary description for ValidElement
/// </summary>
public class ValidElement
{
    public Control Control { get; set; }
    public ValidType ValidType { get; set; }
    private string _message = "";
    public string Message
    {
        get
        {
            if (string.IsNullOrEmpty(_message))
                _message = errorMessage[ValidType];
            return _message;
        }
        set { _message = value; }
    }
    public Control ErrContent { get; set; }
    public bool Valid { get; set; }

    private Dictionary<ValidType, string> errorMessage = new Dictionary<ValidType, string>();

    public ValidElement()
	{
        errorMessage.Add(ValidType.Required, "Поле обязательно для заполнения");
        errorMessage.Add(ValidType.Email, "Некорректно введен email");
        errorMessage.Add(ValidType.Number, "Некорректно введено числом");
        errorMessage.Add(ValidType.Money, "Некорректный денежный формат");
        errorMessage.Add(ValidType.Url, "Некорректно введен url");
	}  
}

public enum ValidType
{
    Required,
    Email,
    Number,
    Money,
    Url
}