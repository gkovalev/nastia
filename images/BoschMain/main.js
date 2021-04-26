/****************************************
*
Bosch Main JS: Divisions OCS/CMS/ASA
*

e0: 	log message
e1: 	set/reset/validate default input text from textfields with class .cdVal
e2: 	mouseover for #leftNav
e3: 	iterate each #leftNav li.active to set active state
e4: 	!EXPIRED: fadeToggle stageContainer. Look at e16
e5: 	tab navigation (toggleTabContainer)
e6: 	accessory toggle up/down
e7: 	lightbox functions
e8: 	submit button: hover
e9: 	custom Bosch lightbox/popup with URL param processing and header include
e10: 	location change 
e11: 	flyout navigation (2-rows)
e12: 	autoresize height of current class elements
e13: 	slideToggle for FAQ Elements
e14: 	open Nyromodal with a function: openNyroManual (wUrl, WWidth, WHeight, WHeadline)
e15: 	search term validation: requires stvURL, stvWidth, stvHeight, stvHeadline
e16: 	Stage slideshow: 
		$('div#site').slideshow({duration:8000, delay:3000, autoPlay:true, ssButton: '.stagePg', ssContainer: '.stageContainer'});
e17: 	lightbox manual
e18: 	popup manual
e19: 	resize height of '.bobPreWrapper' 
e25:  	Repositioning of subnavi if it doesn't fit
e26: 	FAQ Search
e27: 	JW player icon repositioning
e28: 	Related system accessory text repositioning
e29: 	Bookmark script (needs jquery-ui dialog)
e30: 	Manual Popup: openBWindow
e31: 	OCS Stage, fixed first column and scroll for other columns (table .techDetails inside .tableFixed)
e32: 	Universal vertical/horizontal alignment for elements inside container ".valignMidCen"
*****************************************/

// START e0 
bLog = function( text ){
    if( (window['console'] !== undefined) ){
        console.log( text );
    }
};
// END e0

$(document).ready (function () {
	
	// START e1 
		// bind: focus
		$('.cdVal').bind ('focus', function () {
			if ($(this).val() == $(this).attr('title')) {
				$(this).removeClass('dtActive');
				$(this).val('');
			};
		});
		// bind: blur
		$('.cdVal').bind ('blur', function () {
			if ($(this).val() == '') {
				$(this).addClass('dtActive');
				$(this).val($(this).attr('title'));
			};
		});		
		// iterate elms to set default text from title
		$('.cdVal').each (function () {
			$(this).addClass('dtActive');
			$(this).val($(this).attr('title'));							  
		});
		
	// END e1 
	
	// START e2 
		// bind: mouseover
		$('#leftNav li a').bind ('mouseover', function () {
			$('#leftNav li *').removeClass ('lnH');
			$(this).closest('li').children('.l1, .l2, .l3, .l4, span.navE').addClass('lnH');
		});
		
		// bind: mouseout
		$('#leftNav li a').bind ('mouseout', function () {
			$('#leftNav li *').removeClass ('lnH');
		});
	// END e2 
		
	// START e3
		$('#leftNav li.active').each (function () {
			$(this).children('.l1, .l2, .l3, .l4, span.navE').addClass('lnA');
		});
	// END e3 
		
	// START e4
	/*
	function toggleStageImg (wEvt, initClass, targetClass, targetIdPrefix) {
		$(initClass).each (function () {
			if (wEvt == 'clickEvt') {
				$(this).bind ('click', function () {
					currentElmNo = $(this).attr('id').split('_')[1];
					$(targetClass).css('display', 'none');
					$(targetIdPrefix+currentElmNo).fadeIn('slow');
				});				
			}
		});
	};	
	toggleStageImg ('clickEvt', '.stagePg', '.stageContainer', '#con_');
	*/
	
	/*
	window.setTimeout(function () {
		toggleStageImg ('.stagePg', '.stageContainer', '#con_');
	}, 2000);
	*/	
	// END e4
	
	// START e5
		function toggleTabContainer (id) {
			$('.contentTab').css('display', 'none');
			$('#ct_'+id).css('display', 'block');
		}; 
		$('.ctn').bind ('click', function () {
			currentId = $(this).attr('id').split('_')[1];
			toggleTabContainer (currentId);
			$('.ctn').removeClass('active');
			$(this).addClass('active');
		});	
	// END e5
	
	// START e6
		function catLoadAni (liElm) {
			liElm.addClass('showLoad');
		};
		function removeCatLoadAni (liElm) {
			liElm.removeClass('showLoad');
		};
		function catSlideAfterLoad (liElm) {
			if (liElm.find('ul').hasClass('sub')) {
				liElm.toggleClass('down');
				liElm.find('ul.sub').slideToggle('slow');
			};
		};
	// END e6
	
	
	// START e7		
		
		$('a.nyroModal.w400').nyroModal ({
				closeButton: '', // Adding automaticly as the first child of #nyroModalWrapper 
				minWidth: 400,
				minHeight: 300
		});	
		
		$('a.nyroModal.w600').nyroModal ({
				closeButton: '', // Adding automaticly as the first child of #nyroModalWrapper 
				minWidth: 600,
				minHeight: 400
		});
		
		$('a.nyroModal.w735').nyroModal ({
				closeButton: '', // Adding automaticly as the first child of #nyroModalWrapper 
				minWidth: 735,
				minHeight: 600
		});
		$('a.nyroModal.w858').nyroModal ({
				closeButton: '', // Adding automaticly as the first child of #nyroModalWrapper 
				minWidth: 858,
				minHeight: 400
		});
		
		$('a.nyroModal.w800').nyroModal ({
				closeButton: '', // Adding automaticly as the first child of #nyroModalWrapper 
				minWidth: 800,
				minHeight: 600
		});
		
		$('a.nyroModal.w900').nyroModal ({
				closeButton: '', // Adding automaticly as the first child of #nyroModalWrapper 
				minWidth: 900,
				minHeight: 700
		});
		
		$('a.nyroModal.w960').nyroModal ({
				closeButton: '', // Adding automaticly as the first child of #nyroModalWrapper 
				minWidth: 960,
				minHeight: 800
		});
	// END e7
	
	// START e8
		$('.button').bind ('mouseover', function () {
			$(this).find ('input').addClass ('hover');
			$(this).find ('span').addClass ('hover');
			$(this).find ('b').addClass ('hover');
			$(this).find ('img').addClass ('hover');
		});
		$('.button').bind ('mouseout', function () {
			$(this).find ('input').removeClass ('hover');
			$(this).find ('span').removeClass ('hover');
			$(this).find ('b').removeClass ('hover');
			$(this).find ('img').removeClass ('hover');
		});
	// END e8
	
	// START e9 
		/* START custom Bosch lightbox URL param processing and header include */
				
		/* function to resize iframe within a lightbox. Notice: This will effect ALL iframes */
		initBlb(); // --> e17
		/* END custom Bosch lightbox URL param processing and header include */	
		
		/* START custom Bosch popup URL param processing and header include */
		initBPopup(); // --> e18
		/* END custom Bosch popup URL param processing and header include */
		
	// END e9
	
	// START e11 
		$('.foSub').bind('mouseenter', function () {
			$(this).find('.foRow').addClass('hover');
		});
		$('.foSub').bind('mouseleave', function () {
			$('.foRow').removeClass('hover');
		});
	// END e11		
	
// START e13
	$('.tabAcc.faq li').bind('click', function () {
		if ($(this).find('ul').hasClass('sub')) {
			$(this).toggleClass('down');
			$(this).find('ul.sub').slideToggle(200);
		};
	});
// END e13




// START e15
	$('.cdVal').closest('form').bind('submit', function () {
		if (typeof(initSTV.stvURL) == 'undefined') {return};
		if (typeof(initSTV.stvWidth) == 'undefined') {initSTV.stvWidth = 450};
		if (typeof(initSTV.stvHeight) == 'undefined') {initSTV.stvHeight = 200};
		if (typeof(initSTV.stvHeadline) == 'undefined') {initSTV.stvHeadline = ''};				
		if ( 
			($(this).find('.cdVal').val() == $('.cdVal').attr('title')) ||
			($(this).find('.cdVal').val() == '') ||
			($(this).find('.cdVal').val().length < 2) && ($('.cdVal').val() != '')
		) {
			openNyroManual (initSTV.stvURL, initSTV.stvWidth, initSTV.stvHeight, initSTV.stvHeadline);
			return false;
		};			
		return true;
	});
	
	$('.cdValContent').closest('form').bind('submit', function () {
		if (typeof(initSTV.stvURL) == 'undefined') {return};
		if (typeof(initSTV.stvWidth) == 'undefined') {initSTV.stvWidth = 450};
		if (typeof(initSTV.stvHeight) == 'undefined') {initSTV.stvHeight = 200};
		if (typeof(initSTV.stvHeadline) == 'undefined') {initSTV.stvHeadline = ''};				
		if ( 
			($(this).find('.cdValContent').val() == $('.cdValContent').attr('title')) ||
			($(this).find('.cdValContent').val() == '') ||
			($(this).find('.cdValContent').val().length < 2) && ($('.cdValContent').val() != '')
		) {
			openNyroManual (initSTV.stvURL, initSTV.stvWidth, initSTV.stvHeight, initSTV.stvHeadline);
			return false;
		};			
		return true;
	});
// END e15



// START e26
	// If FAQ exists
	if ($('.searchBoxResult.faq ul').length) {
		$('.searchBoxResult.faq ul li a').bind ('click', function (event) {
			// check for next node
			if ($(this).next().length) {
				var nextNode = $(this).next();
				// check if next node is UL
				if ( (nextNode.get(0).tagName) == 'UL' ) {
					// if next node is UL prevent href firing
					event.preventDefault();
					// class show to display arrow down
					$(this).toggleClass('show');
					// toggle up/down
					nextNode.slideToggle ('slow', function () {
						// slide success
					});
				} else {
					// case li
				};
			};
		});
	};
// END e26

});



// START e14 
function openNyroManual (wUrl, WWidth, WHeight, WHeadline) {
	$.nmManual(
		wUrl, {
			closeButton: '',
			sizes: {	// Size information
			minW: WWidth,	// minimum width
			minH: WHeight	// minimum height
			},
			header: '<div class="blbHeader" style="width:'+WWidth+'px"><h3>'+WHeadline+'</h3><a href="javascript:void(0);" class="nyroModalClose"><img src="http://www.bosch-professional.com/media/images/icon_close.png" alt="" /></a><div style="clear:both;"></div></div>',	// header include in every modal
			title: '',
			callbacks: {                    
				beforeShowBg: function(){
					blbResizeIframe(WWidth, WHeight);
				}
			}
		}
	);
};
// END e14

// START e10
var wURL = false;
var wTarget = false;
function jsLink (wURL, wTarget) {			
	if (wURL) {
		if ((wURL.indexOf("http") == -1) && ($('base').length > 0) && wURL.substring(0,1) != '/') {
			wURL = $('base').eq(0).attr('href')+wURL;
		}

		if (wTarget && wTarget == '_blank') {
			window.open(wURL);
		} 
		else {
			window.location.href = wURL;
		};
	} 
	else {
		alert('URL undefined or broken');
	};
};   
// END e10


// START e12
	function classAutoHeight (whichClass) {
		if ($(whichClass).length > 0) {
			var classHeight = 0;
			$(whichClass).each(function () {
				var currentClassHeight = $(this).height();
				if (currentClassHeight > classHeight) {
					classHeight = currentClassHeight;
				};
			});
			$(whichClass).height(classHeight);
		};
	};
	$(document).ready(function () {
		classAutoHeight ('.floatTAutoHeight'); // resize height of all elements			
	});
// END e12



// START e16
	( function( $ ) {
	
	var Slideshow = function( e, o )
	{
		
		var __o = this;
		var __e = $( e );
		var __s = $.extend({ duration:2000, delay:1000, autoPlay:true, selectedIndex:0, ssButton: '', ssContainer: '' }, o || {} );
		var __containers = [];
		
		// delay bis autoPlay anfängt
		var __delay = {};
		
		
		// delay zwischen bildern
		var __duration = {};
		var __loop = false;
		
		this.swap = function ( index )
		{
			__o.stopAutoPlay();
			if ( index == __s.selectedIndex)
			{
				return;	
			}
			if ( __s.selectedIndex > -1 )
			{
				$( __containers[__s.selectedIndex] ).hide(  );
			}
			__s.selectedIndex = index;
			$( __containers[__s.selectedIndex] ).fadeIn( 'slow' );
			if ( __loop )
			{
				__o.startAutoPlay();
			}
		};
		
		this.startAutoPlay = function ()
		{
			__loop = true;
			__duration = window.setTimeout( function () { __o.nextImage() }, __s.duration );
		};
		
		this.stopAutoPlay = function ()
		{
			window.clearInterval( __duration );
		};
		
		this.startDelay = function ()
		{
			__o.stopAutoPlay();
			__o.stopDelay();
			__delay = window.setTimeout( function () { __o.delay() }, __s.delay );	
		};
		this.stopDelay = function ()
		{
			__o.stopAutoPlay();
			window.clearTimeout( __delay );
		};
			this.nextImage = function ()
		{			
			var nextImage = __s.selectedIndex+1;
			if ( nextImage > __containers.length-1 )
			{
				nextImage = 0;
			}
			__o.swap( nextImage );		
			
			$(__s.ssButton).removeClass('active');
			$(__s.ssButton).eq(__s.selectedIndex).addClass('active');
		};
		
		this.delay = function () 
		{
			__o.startAutoPlay();	
		};
		
			var __i = function () 
		{
			var btns = __e.find( __s.ssButton );
			__containers = __e.find( __s.ssContainer );
			btns.each( function ( i ) {
				$( this ).data( 'containerIndex', i );								 
			});
			btns.bind( {
				'mouseenter': function ( event )
				{
					__o.stopDelay();
					__loop = false;
				},
				'mouseleave': function ( event )
				{
					__o.startDelay();
				},
				'click': function ( event )
				{
					__o.stopDelay();
					__o.swap( $( this ).data( 'containerIndex' ), false );
					$(__s.ssButton).removeClass('active');
					$(__s.ssButton).eq(__s.selectedIndex).addClass('active');
				}
			});
			if ( __s.autoPlay )
			{
				__o.startAutoPlay();
			}
		};
		//i
		__i();
	};

	
	// inject all instances
	$.fn.slideshow = function ( o )
	{
		return this.each( function()
		{
			var e = $( this );
			if ( e.data( 'ss' ) ) return;
			var ss = new Slideshow( this, o );
			e.data( 'ss', ss );
		});
	};
	
})( jQuery );

// END e16


// START  e17
function blbResizeIframe (whichWidth, whichHeight) {
	if ($('.nyroModalCont iframe').length > 0) {
		//dsp: 20120814 howHigh = whichHeight - $('.blbHeader').height();
		howHigh = (whichHeight-5) - $('.blbHeader').height();
		
		$('.nyroModalCont iframe').css( {
			'width': whichWidth+'px',
			'height': howHigh+'px',
			'overflow-x': 'hidden'
		});
	};
};

function initBlb() {
	$('.bLightbox').each (function () {
		var nyroUrl = $(this).attr('href');
		
		var currentblbHeight = 0;	
		var currentblbWidth = 0;
		
		var blbTitle = $(this).attr('title');
		if (blbTitle == '' || blbTitle == 'undefined') {
			blbTitle = '';
		}
		
		
		var inlHead = $(this).attr('inlHead');
		if (inlHead == '' || inlHead == 'undefined') {
			inlHead = '';
		} else {
			blbTitle = inlHead;
		}
		
		var inlWidth = $(this).attr('inlWidth');
		if (inlWidth == '' || inlWidth == 'undefined') {
			inlWidth = '';
		} else {
			currentblbWidth = inlWidth;
		}
		
		var inlHeight = $(this).attr('inlHeight');
		if (inlHeight == '' || inlHeight == 'undefined') {
			inlHeight = '';
		} else {
			currentblbHeight = inlHeight;
		}
		
		
		$(this).nyroModal ({
			_loading: true,			
			closeButton: '', // Adding automaticly as the first child of #nyroModalWrapper 
			sizes: {	// Size information
				minW: currentblbWidth,	// minimum width
				minH: currentblbHeight	// minimum height
	
			},
			header: '<div class="blbHeader" style="width:'+currentblbWidth+'px"><h3>'+blbTitle+'</h3><a href="javascript:void(0);" class="nyroModalClose"><img src="http://www.bosch-professional.com/media/images/icon_close.png" alt="" /></a><div style="clear:both;"></div></div>',	// header include in every modal
			title: '',
			callbacks: {                    
				beforeShowBg: function(){
					blbResizeIframe(currentblbWidth, currentblbHeight);
					//resizeNyroBox(currentblbWidth, currentblbHeight);
				},                
				afterShowCont: function(){
					//resizePopupContent(currentblbWidth, currentblbHeight);
				}
			}			
			
		});
	});	
};

// END e17


// START e18
function initBPopup() { 
	$('.bPopup').each (function () {
		var nyroUrl = $(this).attr('href');
		
		var currentblbHeight = 0;	
		var currentblbWidth = 0;	
		
		var blbTitle = $(this).attr('title');
		if (blbTitle == '' || blbTitle == 'undefined') {
			blbTitle = '';
		}		
		
		var inlHead = $(this).attr('inlHead');
		if (inlHead == '' || inlHead == 'undefined') {
			inlHead = '';
		} else {
			blbTitle = inlHead;
		}
		
		var inlWidth = $(this).attr('inlWidth');
		if (inlWidth == '' || inlWidth == 'undefined') {
			inlWidth = '';
		} else {
			currentblbWidth = inlWidth;
		}
		
		var inlHeight = $(this).attr('inlHeight');
		if (inlHeight == '' || inlHeight == 'undefined') {
			inlHeight = '';
		} else {
			currentblbHeight = inlHeight;
		}
		
		$(this).bind ('click', function () {
			window.open(nyroUrl, blbTitle, "width="+currentblbWidth+",height="+currentblbHeight+"");
			return false;
		});
			
	});
};
// END e18

// START e19
reHeightBPW = function () {
	if ( 
		($('.bobPreWrapper').length > 0) && 
		($('.teaserContent .floatBox').length > 0) && 
		($('.bobPreview .bobPreRight h3').length > 0) && 
		($('.bobPreview .bobPreRight').length > 0) 
	) {
		var bobPreWrapper = $('.bobPreWrapper');
		var curFloatBox = $('.teaserContent .floatBox').height();
		var curBobH3Height = $('.bobPreview .bobPreRight h3').height();
		var curBobPreRightPT = eval($('.bobPreview .bobPreRight').css('padding-top').split('px')[0]);
		var curBobPreRightPT =  eval($('.bobPreview .bobPreRight').css('padding-bottom').split('px')[0]);	
		var bobPreElmHeight = curFloatBox - (curBobH3Height + curBobPreRightPT + curBobPreRightPT);
		bobPreWrapper.height(bobPreElmHeight);	
	};
};
$(window).load	(function () {
	classAutoHeight ('.tbAutoHeight');
	reHeightBPW();
});
// END e19



// START e25
$(document).ready(function () {
	if ($('.foSub').length > 0) { // if subnavi exists
		var subNaviWidth = 724; // navi visible width (maybe smaller than $(this).find('.foRow').width())
		var siteMargin = ( $(document).width() - $('#site').width() ) / 2;  // left and right margin
		var siteWidth = $('#site').width();  // #site width
		
		$('.foSub').each (function (i) { // loop through navi elements
			var curNavLeft = $(this).offset().left - siteMargin; // left position of current navi element
			var curTotalWidth = curNavLeft + subNaviWidth;  // total width of left position plus subnavi width
			
			//console.log ('siteMargin: ' + siteMargin);	
			//console.log ('siteWidth: ' + siteWidth);	
			//console.log ('curNavLeft: ' + curNavLeft);	
			//console.log ('subNaviWidth: ' + subNaviWidth);	
			//console.log ('curTotalWidth: ' + curTotalWidth);
			
			if (curTotalWidth > siteWidth) {   // if subnavi doesnt fit 
				$(this).find ('.foRow').css('margin-left', '-' + (curTotalWidth - siteWidth) + 'px'); // repos subnavi width neg margin-left
			};
		});
	};
});
// END e25

// START e27
$(window).load(function () {
	// if JW-Play icon exists
	if ($('.jwPlayIcon').length) {
		$('.jwPlayIcon').each (function () {
			var ico = $(this); 
			var med = $(this).closest('.media');
			var iconW = ico.width();
			var iconH = ico.height();
			var mediaW = med.width();
			var mediaH = med.height();							
			ico.css ({'margin-top': (mediaH-iconH)/2 + 'px','margin-left': (mediaW-iconW)/2 + 'px'});
		});
	};
});
// END e27

// START e28
$(window).load(function () {
	// if related system accessory exists
	if ($('.sysAccCon').length) {
		$('.sysAccCon').each (function () {
			var con = $(this); 
			var txt = $(this).find('.sacRight');
			var conH = con.height();
			var txtH = txt.height();							
			if (conH > txtH) {
				txt.css ({'padding-top': (conH-txtH)/2 + 'px'});
			} else {
				txt.css ({'padding-top': '0'});			
			};
		});
	};
});
// END e28

// START e29
function bookmarkPage(url,title) {
	if (!url) {url = window.location}
	if (!title) {title = document.title}
	var browser=navigator.userAgent.toLowerCase();
	if (window.sidebar) { // Mozilla, Firefox, Netscape
		window.sidebar.addPanel(title, url,"");
	} else if( window.external) { // IE or chrome
		if (browser.indexOf('chrome')==-1){ // ie
			window.external.AddFavorite( url, title); 
		} else { // chrome
			$( "#bookmark_chrome").dialog ({modal: true, buttons: {Ok: function() {$( this ).dialog( "close" );}}});
		}
	}
	else if(window.opera && window.print) { // Opera - automatically adds to sidebar if rel=sidebar in the tag
		return true;
	}
	else if (browser.indexOf('konqueror')!=-1) { // Konqueror
		$( "#bookmark_webkit").dialog ({modal: true, buttons: {Ok: function() {$( this ).dialog( "close" );}}});
	}
	else if (browser.indexOf('webkit')!=-1){ // safari
		$( "#bookmark_webkit").dialog ({modal: true, buttons: {Ok: function() {$( this ).dialog( "close" );}}});
	} else {
		$( "#bookmark_nosupport").dialog ({modal: true, buttons: {Ok: function() {$( this ).dialog( "close" );}}});
	}
};
// END e29



// START e30
function openBWindow(obwURL, obwTitle, obwWidth, obwHeight) {
	window.open(obwURL, obwTitle, "width="+obwWidth+",height="+obwHeight+"");
	return false;
};
// END e30



// START e31 
$(document).ready(function () {
	
	initTableFixed = function () {
		getWidestFirstCol = function () {
			var widestCol = 0;
			var curWidth = 0;
			$('.tableFixed.original .techDetails .colfixed').each (function () {
				curWidth = $(this).outerWidth(true);
				if (widestCol < curWidth) {
					widestCol = curWidth;		
				}
			});
			return widestCol;
		}	
		// check existance of .tableFixed 
		if ($('.tableFixed').length > 0) {
			// if IE lt 8
			if ( ($.browser.msie) &&  (parseInt($.browser.version, 10) < 8)) {
				$('.tableFixed.clone').css('display', 'none');
			} 
			// all other browsers
			else {
				$('.tableFixed.clone').css('width', getWidestFirstCol() + 'px');	
				$('.tableFixed.clone').css('display', 'block');	
			}
		};	
	};
	
	initTableFixed();
	
});
// END e31

// START e32 
$(window).load(function () {
	// check if element exists
	if ($('.valignMidCen').length > 0) {
		$('.valignMidCen').each (function () {
			// container
			var co = $(this);
			// child
			var ch = $(this).children();
			// case: only one child inside container
			if (ch.length == 1) {
				// child must be a block element
				ch.css('display', 'block');
				co.css('text-align', 'center');
				ch.css('text-align', 'center');
				//check if container height larger than child height
				if (co.height() > ch.height()) {
					var mT = (co.height()-ch.height()) / 2;
					ch.css('margin-top', mT+'px');
				};
				//check if container width larger than child width
				if (co.width() > ch.width()) {
					var mL = (co.width()-ch.width()) / 2;
					ch.css('margin-left', mL+'px');
				};		
			}
		});
	};	
});
// END e32