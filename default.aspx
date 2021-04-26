<%@ Page Language="C#" MasterPageFile="MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" EnableViewState="false" %>

<%@ Register Src="UserControls/StaticBlock.ascx" TagName="StaticBlock" TagPrefix="adv" %>
<%@ Register Src="UserControls/Default/MainPageProduct.ascx" TagName="MainPageProduct"
    TagPrefix="adv" %>
<%@ Register TagPrefix="adv" TagName="News" Src="~/UserControls/Default/News.ascx" %>
<%@ Register TagPrefix="adv" TagName="Carousel" Src="~/UserControls/Default/Carousel.ascx" %>
<%@ Register TagPrefix="adv" TagName="BoschCarousel" Src="~/UserControls/Default/BoschCarousel.ascx" %>
<%@ Register TagPrefix="adv" TagName="GiftCertificate" Src="~/UserControls/Default/GiftCertificate.ascx" %>
<%@ Register TagPrefix="adv" TagName="CheckOrder" Src="~/UserControls/Default/CheckOrder.ascx" %>
<%@ Register TagPrefix="adv" TagName="Voting" Src="~/UserControls/Default/VotingUC.ascx" %>
<%@ Register TagPrefix="adv" TagName="MenuCatalogAlternative" Src="~/UserControls/MasterPage/MenuCatalogAlternative.ascx" %>
<%@ MasterType VirtualPath="MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphMain" runat="Server">
    <asp:MultiView runat="server" ID="mvDefaultPage">
        <Views>
               <asp:View runat="server" ID="defaultView">
                <Adv:BoschCarousel runat="server" ID="BoschCarousel1" Mode="Default" />
                <adv:StaticBlock ID="sbTextOnMain" runat="server" SourceKey="TextOnMain" />
                <adv:MainPageProduct ID="mainPageProduct" runat="server" Mode="Default" />
                <adv:StaticBlock ID="sbTextOnMain2" runat="server" SourceKey="TextOnMain2" />
                <!--shadow_split-->
                <div class="shadow-split">
                </div>
                <!--end_shadow_split-->
                <!--blocks-->
                <div class="container-default-blocks">
                    <!--new-block-->
                    <adv:News ID="news" runat="server" />
                    <!--end_new-block-->

                    <!--check_status-->
                    <div class="block-uc-merge">
                        <adv:CheckOrder ID="checkOrder" runat="server" />
                        <adv:GiftCertificate ID="giftCertificate" runat="server" Mode="Default" />
                    </div>
                    <!--end_check_status-->
                     <div class="block-uc">
                      <Adv:StaticBlock ID="stDiscount" runat="server" SourceKey="DiscountOnMain" />
                     </div>
                    <!--social-->
                    <div class="block-uc social-big">
                    <!--ChangedBy Evgeni --> 
                       <Adv:StaticBlock ID="staticBlockBp" runat="server" SourceKey="BestPriceOnMain" />
                    </div>
                    <!--end_social-->
                </div>
                <!--end_blocks-->
                <br class="clear" />
                <div class="split-light-blue">
                </div>
            </asp:View>
  <!--
            <asp:View runat="server" ID="twoColumnsView">
                <div class="stroke">
                    <div class="col-left expander-enable">
                        <adv:MenuCatalogAlternative ID="menuCatalogAlter" runat="server" />
                        <adv:News runat="server" ID="newsTwoColumns" />
                        <adv:CheckOrder runat="server" ID="checkOrderTwoColumns" />
                        <adv:Voting runat="server" ID="votingTwoColumns" />
                        <adv:GiftCertificate ID="giftCertificateTwoColumns" runat="server" Mode="TwoColumns" />
                        <br class="clear" />
                        <div class="block-uc social-big">
                            <adv:StaticBlock ID="staticBlock4" runat="server" SourceKey="Vk" />
                        </div>
                    </div>
                    <div class="col-right">
                        <adv:Carousel ID="carouselTwoColumns" runat="server"/>
                        <adv:StaticBlock ID="StaticBlock1" runat="server" SourceKey="static-temp" />
                        <adv:StaticBlock ID="StaticBlock5" runat="server" SourceKey="TextOnMain" />
                        <adv:MainPageProduct ID="mainPageProductTwoColumns" runat="server" Mode="TwoColumns" />
                        <adv:StaticBlock ID="StaticBlock6" runat="server" SourceKey="TextOnMain2" />
                    </div>
                    <br class="clear" />
                </div>
            </asp:View>
            <asp:View runat="server" ID="threeColumnsView">
                <div class="stroke">
                    <div class="col-left">
                        <adv:MenuCatalogAlternative ID="MenuCatalogThreeColumns" runat="server" />
                        <adv:GiftCertificate ID="giftCertificateThreeColumns" runat="server" Mode="ThreeColumns" />
                        <div class="block-uc social-big">
                            <adv:StaticBlock ID="staticBlock7" runat="server" SourceKey="Vk" />
                        </div>
                    </div>
                    <div class="col-right">
                        <div class="three-constructor">
                            <div class="three-left">
                                <adv:Carousel ID="carouselThreeColumns" runat="server"/>
                                <adv:StaticBlock ID="StaticBlock8" runat="server" SourceKey="TextOnMain" />
                                <adv:MainPageProduct ID="mainPageProductThreeColumn" runat="server" Mode="ThreeColumns" />
                                <adv:StaticBlock ID="StaticBlock9" runat="server" SourceKey="TextOnMain2" />
                            </div>
                            <div class="three-right">
                                <adv:News runat="server" ID="newsThreeColumns" />
                                <adv:CheckOrder runat="server" ID="CheckOrderThreeColumns" />
                                <adv:Voting runat="server" ID="votingThreeColumns" />
                            </div>
                        </div>
                    </div>
                    <br class="clear" />
                </div>
            </asp:View>
            -->
        </Views>
    </asp:MultiView>
</asp:Content>
