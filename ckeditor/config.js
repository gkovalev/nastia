/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';
	config.filebrowserImageBrowseUrl = CKEDITOR.basePath + "connectors/imagebrowser.aspx";
    config.filebrowserImageWindowWidth = 770;
    config.filebrowserImageWindowHeight = 741;
    config.filebrowserBrowseUrl = CKEDITOR.basePath + "connectors/LinkBrowser.aspx";
    config.filebrowserWindowWidth = 100;
    config.filebrowserWindowHeight = 615;

    config.toolbar = 'AdVantShopToolBar';

    config.toolbar_AdVantShopToolBar =
    [
	{ name: 'document', items: ['Source', '-', 'NewPage', 'DocProps', 'Preview', 'Print', '-', 'Templates'] },
	{ name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
	{ name: 'forms', items: ['Form', 'Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button', 'ImageButton',
        'HiddenField', 'CreateDiv']
	},
	'/',
	{ name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
	{ name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock', '-', 'BidiLtr', 'BidiRtl']
	},
	{ name: 'links', items: ['Link', 'Unlink'] },
	{ name: 'insert', items: ['Image', 'Flash', 'Table', 'HorizontalRule', 'SpecialChar', 'Iframe'] },
	'/',
	{ name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
	{ name: 'colors', items: ['TextColor', 'BGColor'] },
	{ name: 'tools', items: ['Maximize', 'ShowBlocks', '-', 'About'] }
];

};
