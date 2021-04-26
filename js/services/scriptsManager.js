(function () {

    var scriptsManager = function () { };

    scriptsManager.version = "1.0";

    scriptsManager.isExistLinkJS = function (url) {
        return $("script").filter('[src$="' + url + '"]').length;
    };

    scriptsManager.isExistLinkCSS = function (url) {
        return $("link").filter('[href$="' + url + '"]').length;
    };

    scriptsManager.getScript = function (url) {

        if (!Advantshop.Utilities.File.IsExist(url)) {
            throw Error('Not found script');
        }

        if (scriptsManager.isExistLinkJS(url)) {
            return;
        }

        var scriptObj = document.createElement('script');
        scriptObj.src = url;
        scriptObj.async = true;
        $(scriptsManager.options.container)[0].appendChild(scriptObj);
    };

    scriptsManager.getCSS = function (url) {
        if (!Advantshop.Utilities.File.IsExist(url)) {
            throw Error('Not found css');
        }

        if (scriptsManager.isExistLinkCSS(url)) {
            return;
        }

        var linkObj = document.createElement('link');
        linkObj.setAttribute('href', url);
        linkObj.setAttribute('rel', 'stylesheet');
        document.getElementsByTagName('head')[0].appendChild(linkObj);
    };

    Advantshop.NamespaceRequire('Advantshop');
    Advantshop.ScriptsManager = scriptsManager;
})();
