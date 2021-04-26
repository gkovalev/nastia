/****************************************
*
Bosch JS: Divisionstartpage
*
*****************************************/
// START Division Startpage functions
initDivisionSlide = function (slideArray) {
	if ($('#divisionContainerBG').length > 0) {
		
		var divisionSlideImages = slideArray;
		
		var divSlideShowCount = 1;
		
		$.fn.slideBgImg = function() {
			// check if background images >= 1;
			if (slideArray.length <= 1) {return};
			if (divSlideShowCount > ($('.dcBox').length-1)) {
				divSlideShowCount = 0;
			}
			divSlideShowCount ++;
			var image = divisionSlideImages[divSlideShowCount-1];
			return this.each(function() {
				var $obj = $(this);
				$obj.fadeOut(0,function() {
					//$obj.css('background', 'url(' + image + ')').fadeIn(400);
					$obj.css('background-image', 'url(' + image + ')').fadeIn(150);
				});
			});	
		};		
		setInterval('$("#divisionContainerBG").slideBgImg()', 5000);	
		var divisionContainerHeight = $('#divisionContainerBG').height();
		var dcBoxHeight = 0;
		$('.dcBox').each (function () {
			if (dcBoxHeight < $(this).height()) {
				dcBoxHeight	= $(this).height();
			}		
		});
		
		$('.dcBox').height(dcBoxHeight);
		var dcHolderMargin = divisionContainerHeight - dcBoxHeight - 10;	
		$('#dcHolder').css ('margin-top', dcHolderMargin+'px');	
		$('#dcHolder').addClass('show');
	}		
}	
// END Division Startpage functions