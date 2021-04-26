<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BoschCarousel.ascx.cs" Inherits="UserControls_Default_Carousel" %>
<%@ Import Namespace="AdvantShop.Configuration" %>
<%@ Import Namespace="AdvantShop.Core" %>
<%@ Import Namespace="AdvantShop.FilePath" %>
<!--slider-->
<link rel="stylesheet" type="text/css" href="images/BoschMain/main.css" media="all" />
<link rel="stylesheet" type="text/css" href="images/BoschMain/pSlider.css" media="all" />
<script src="images/BoschMain/jquery.nyroModal.custom.js" type="text/javascript"></script>
<script src="images/BoschMain/main.js" type="text/javascript"></script>
<script src="images/BoschMain/extends.divisionstartpage.js" type="text/javascript"></script>

<script type="text/javascript">

    $(document).ready(function () {
        $('#content').slideshow({ duration: 8000, delay: 3000, autoPlay: true, ssButton: '.btSlideShow', ssContainer: '.stageContainer' });
    });
</script>
 


<div id="header">
<!-- START Bosch header -->
 

<!-- changed By Evgen i to remove carousel
 $(document).ready(function () {
        initDivisionSlide([
'https://www.instrument-opt.by/images/BoschMain/Bosch-Brest-Akcia-1.jpg'
,'https://www.instrument-opt.by/images/BoschMain/Bosch-Brest-Akcia-2.jpg'
,'https://www.instrument-opt.by/images/BoschMain/kv_main_garden.jpg'
, 'https://www.instrument-opt.by/images/BoschMain/kv_main_diy.jpg'
, 'https://www.instrument-opt.by/images/BoschMain/kv_main_garden.jpg'
, 'https://www.instrument-opt.by/images/BoschMain/kv_main_accessories.jpg'
]);
--> 
 
<div id="content">
<div class="stage">
<script type="text/javascript">
    $(document).ready(function () {
        initDivisionSlide([
'images/BoschMain/Brest-Akcia-Rotak.jpg'
]);
    });
</script>
<a title="Электроинструменты"  href="https://instrument-opt.by/news/skil6220ld_v_podarok_aqt40_rotak40" target="_self"><div id="divisionContainer">
<div id="divisionContainerBG" style="background-image:url('https://www.instrument-opt.by/images/BoschMain/Brest-Akcia-Rotak.jpg');"> 
</div> </a>
<div id="dcHolder">
<div class="dcBox first">
<h6>Инструменты для профессионального / промышленного применения</h6>
<ul>
<li>
<a title="Электроинструменты"  href="https://instrument-opt.by/categories/bosch_%D0%BF%D1%80%D0%BE%D1%84%D0%B8__101272" target="_self">Электроинструменты</a>
<li><a title="Принадлежности"  href="https://instrument-opt.by/categories/osnastka-dlia-profi_171467" target="_self">Принадлежности</a>
<li><a title="Официальный сайт"  href="https://www.bosch-professional.com/by/ru/" target="_blank">Официальный сайт </a>

</ul>
</div>
<div class="dcBox diy">
<h6>Работа в домашней мастерской и уход за садом</h6>
<ul>
<li><a title="Электроинструменты"  href="https://instrument-opt.by/categories/bosch_%D0%B4%D0%BE%D0%BC%D0%B0_95226" target="_self">Электроинструменты</a>
<li><a title="Садовые инструменты"  href="https://instrument-opt.by/categories/%D1%81%D0%B0%D0%B4_169162" target="_self">Садовые инструменты</a>
<li><a title="Принадлежности"  href="https://instrument-opt.by/categories/osnastka-dlia-mastera-131216" target="_self">Принадлежности</a>
<li><a title= "ПОМОЩЬ В ВЫБОРЕ ИНСТРУМЕНТА (БЫТОВОГО И САДОВОГО)"  href="https://instrument-opt.by/GreenBSg.aspx"  target="_blank"> ПОМОЩЬ В ВЫБОРЕ ИНСТРУМЕНТА (БЫТОВОГО И САДОВОГО) </a>

<li><a title="Официальный сайт"  href="http://www.bosch-do-it.by" target="_blank">Официальный сайт </a>

</ul>
</div>
<div class="dcBox acc">
<h6>Электроинструменты SKIL, DREMEL</h6>
<ul>
<li><a title="Электроинструмент Skil"  href="https://instrument-opt.by/categories/skil" target="_self">Электроинструмент Skil</a>
<li><a title="Электроинструмент Dremel"  href=" https://instrument-opt.by/categories/dremel" target="_self">Электроинструмент Dremel</a>
<li><a title="Официальный сайт Skil"  href="https://www.skileurope.com/ru/ru/%D0%BE-skil/%D0%BE-skil.html" target="_blank">Официальный сайт Skil</a>
<li><a title="Официальный сайт Dremel"  href="https://www.dremeleurope.com/ru/ru/%D0%B3%D0%BB%D0%B0%D0%B2%D0%BD%D0%B0%D1%8F/%D0%B3%D0%BB%D0%B0%D0%B2%D0%BD%D0%B0%D1%8F.html" target="_blank">Официальный сайт Dremel</a>

</ul>
</div>
<div class="dcBox service">
<h6>Наши преимущества</h6>
<ul>
<li><a title="ДИСКОНТНАЯ ПРОГРАММА ООО «НесТулс»"  href="https://instrument-opt.by/news/discountprogram" target="_self">ДИСКОНТНАЯ ПРОГРАММА ООО «НесТулс»</a>
<li><a title="Бесплатная доставка по Республике Беларусь"  href="https://instrument-opt.by/news/uslovia_besplatnoy_dostavki" target="_self">Бесплатная доставка *</a>
<li><a title="Сервисный центр и запасные части к электроинструментам BOSCH, SKIL, DREMEL, CST BERGER"  href="https://instrument-opt.by/news/service_and_spare_parts_bosch" target="_self">Сервисный центр и запасные части</a>
<li><a title="Рассрочка на 3 месяца!!!! Переплата 0%"  href="https://instrument-opt.by/news/rassrochka" target="_self">Рассрочка на 3 месяца!</a>
<li><a title="Наши новости..."  href="https://instrument-opt.by/news" target="_blank">Наши новости...</a>
</ul>
</div>
</div>
</div>

</div>
</div>
</div>

