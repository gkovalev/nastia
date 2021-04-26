<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CatalogDataTreeViewForTaxes.ascx.cs"
    Inherits="UserControls_CatalogDataTreeViewForTaxes" %>
<div id="dataTableBlock" style="width: 100%">
    <div class="controlLinks">
        <span style="float: left"><a class="Link" href="javascript: SelectAll();">
            <asp:Literal ID="Literal1" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_TreeControlSelectAll %>' /></a>
            | <a class="Link" href="javascript: DeselectAll();">
                <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_TreeControlClearAll %>' /></a>
            <!--|
            <span id="TotalProductSelectedLiteral" style="font-weight:bold" runat="Server" /> <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_TreeControlTotalSelected %>' /></span>-->
        </span><span><span>
            <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_TreeControlTotalFounded %>' />
            <span id="TotalRecordsLiteral" style="font-weight: bold" runat="Server" />
            <asp:Literal runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_TreeControlRecords %>' /></span></span>
    </div>
    <asp:UpdatePanel ID="DataTableUpdatePanel" runat="server">
        <ContentTemplate>
            <div class="dataTableBlock">
                <asp:HiddenField ID="categoryToExpand" Value="" runat="server" />
                <asp:Table ID="DataTable" class="dataTable" CellSpacing="0" runat="server">
                    <asp:TableHeaderRow class="headerRow">
                        <asp:TableHeaderCell class="headerCell" Style="width: auto;">
                            <div class="headerRowFiller">
                                <asp:Literal ID="Literal2" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_TreeControlProduct %>' /></div>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell class="headerCell" Style="width: 200px">
                            <asp:Literal ID="Literal3" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_TreeControlPrice %>' /></asp:TableHeaderCell>
                        <asp:TableHeaderCell class="headerCell" Style="width: 100px">
                            <asp:Literal ID="Literal4" runat="Server" Text='<%$ Resources:Resource, Admin_ExportFeed_TreeControlUnit %>' /></asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <img style="display: none" src="images/exportfeed_loader.gif" />
</div>
<script type="text/javascript">
    var TREEVIEW_ID = "<%# ClientID %>";
    var SELECTED_ID = "<%# TotalProductSelectedLiteral.ClientID %>";


    function UpdateTable(categoryId, catRow, depth, parentCategoryId) {
        catRow.getElementsByTagName('img')[0].src = "images/collapse.jpg";
        $("#" + catRow.id + " .categoryLink").after("<img id='loadImage' style='margin-left:10px' src ='images/exportfeed_loader.gif' />");
        $.ajax({
            url: '../HttpHandlers/TaxGetCategoryTable.ashx',
            cache:false,
            dataType: "html",
            data: { catId: categoryId, depth: depth, parentCategoryId: parentCategoryId, TaxID: <%= Request["TaxID"] %> },
            success: function (data) {
                var catId = "#" + catRow.id;
                $(catId).after(data);
                $(catId + " #loadImage").remove();
                var rows = $("#<%= DataTable.ClientID %> tr").filter(function () {
                    return this.id.match(categoryId + "(_[a-zA-Z0-9]+)+_TableRow");
                });

                var selected = false;
                var unSelected = false;

                if ($(catId + " input:checkbox:checked").length != 0 & $(catId + " input:hidden").val() == "full") {
                    selected = true;
                } else if ($(catId + " input:checkbox:checked").length == 0) {
                    unSelected = true;
                }

                for (var i = 0; i < rows.length; ++i) {
                    var row = rows[i];
                    var IdRegExp = /_([a-zA-Z0-9]*)_TableRow/;
                    var IdMatch = IdRegExp.exec(row.id);
                    var Id = IdMatch[1];
                    var anc = $("#" + row.id + " .categoryLiteral span");
                    if (anc.html() != "(0)") {
                        AddUpdateTableHandler('#' + row.id, Id, row, depth + 1, categoryId);
                    } else {
                        $("#" + row.id).css("cursor", "auto");
                    }
                    if (selected) {
                        $("#" + row.id + " input:checkbox").attr("checked", "true");
                        $("#" + row.id + " input:hidden").val("full");
                    }

                    if (unSelected) {
                        $("#" + row.id + " input:checkbox").attr("checked", "");
                        $("#" + row.id + " input:hidden").val("");
                    }
                }



                rows.find("input[type='checkbox']").bind("click", function (eventObject) {
                    eventObject.stopPropagation();
                    ToggleCheckBox(eventObject.target);
                });
            }
        });

        var catId = "#" + catRow.id;
        $(catId).attr("isShow", "true");
        $(catId).unbind('click');
        $(catId).bind('click', function (eventObject) {
            HideTableRow(catRow.getElementsByTagName("input")[1].id);
        });
    }

    function AddUpdateTableHandler(selector, id, row, depth, parentCategoryId) {
        $(selector).one('click', function(event) {
            UpdateTable(id, row, depth, parentCategoryId);
        });
    }


    //checkbox click event handler
    function checkBox_Click(eventElement) {
        eventElement.stopPropagation();
        ToggleCheckBox(eventElement.target);
    }

    function ProductRow_ClickHandler(row) {
        var checkbox = row.getElementsByTagName("input")[0];
        checkbox.checked = !checkbox.checked;
        ToggleCheckBox(checkbox);
    }    

    function SelectAll() {
        var cb = $("#dataTableBlock input[type='checkbox']").filter(function() {
            return this.id.match("__[a-zA-Z0-9]+_Category");
        });
        for (var i = 0; i < cb.length; ++i) {
            var idRegExp = /_(_[a-zA-Z0-9])+_Category/;
            var m = idRegExp.exec(cb[i].id);
            ToggleChildCheckbox(m[1] + "_Category", true);
        }
    }

    function DeselectAll() {
        var cb = $("#dataTableBlock input[type='checkbox']").filter(function() {
            return this.id.match("__[a-zA-Z0-9]+_Category");
        });
        for (var i = 0; i < cb.length; ++i) {
            var idRegExp = /_(_[a-zA-Z0-9])+_Category/;
            var m = idRegExp.exec(cb[i].id);
            ToggleChildCheckbox(m[1] + "_Category", false);
        }
    }

    function ToggleCheckBox(element) {
        var state = element.checked;
        if (state) {
            $("#dataTableBlock #" + element.id + "_State").val("full");            
        } else {
            $("#dataTableBlock #" + element.id + "_State").val("");
        }

        var checkboxIdRegExp = /(_[a-zA-Z0-9]+)+_(Category|Product)/;
        var checkboxIdMatch = checkboxIdRegExp.exec(element.id);
        var checkboxId = checkboxIdMatch[0];

        ToggleChildCheckbox(checkboxId, state);
        ToggleParentCheckbox(checkboxId, state);       
        

        var checkbox = document.getElementById(element.id);
        var checkboxIdRegExp = /(.*)_Product/;
        var checkboxIdMatch = checkboxIdRegExp.exec(element.id);
    }

    function ToggleChildCheckbox(checkboxId, state) {
        var checkbox = $("input[type='checkbox']").filter(function() {
            return this.id.match(checkboxId);
        })[0];
        var checkboxIdRegExp = /(.*)_Category/;
        var checkboxIdMatch = checkboxIdRegExp.exec(checkboxId);
        if (checkboxIdMatch == null) {
            var selected = document.getElementById(SELECTED_ID);
            checkbox.checked = state;
            return;
        }

        var childCheckBoxes = $("#dataTableBlock input[type='checkbox']").filter(function() {
            return this.id.match(checkboxIdMatch[1] + "(_[a-zA-Z0-9]*_){1}(Category|Product)$");
        })
        var totalSubelements = childCheckBoxes.length;

        var selectedSubElements = childCheckBoxes.filter(function() { return this.checked; }).length;

        if (totalSubelements == 0) {
            if (state) {
                $("#dataTableBlock #" + checkbox.id + "_State").val("full");
            } else {
                $("#dataTableBlock #" + checkbox.id + "_State").val("");
            }
        }

        checkbox.checked = state;

        checkboxId = checkboxIdMatch[1];

        var cb = $("#dataTableBlock input[type='checkbox']").filter(function() {
            return this.id.match(checkboxId + "(_[a-zA-Z0-9]*_){1}(Category|Product)");
        });
        for (var i = 0; i < cb.length; ++i) {
            if (state) {
                $("#dataTableBlock #" + cb[i].id + "_State").val("partial");
            } else {
                $("#dataTableBlock #" + cb[i].id + "_State").val("");
            }
            ToggleChildCheckbox(cb[i].id, state);
        }
    }


    function ToggleParentCheckbox(checkboxId, state) {
        var checkbox = $("input[type='checkbox']").filter(function() {
            return this.id.match(checkboxId);
        })[0];

        var checkboxIdRegExp = /(.*)_Category/;
        var checkboxIdMatch = checkboxIdRegExp.exec(checkboxId);
        if (checkboxIdMatch != null) {
            var subElementsRegExp = new RegExp(checkboxIdMatch[1] + "(_[a-zA-Z0-9]*_){1}(Category|Product)$");
            var checkboxes = document.getElementById("dataTableBlock").getElementsByTagName("input");

            var totalSubelements = 0;
            var selectedSubElements = 0;

            var childCheckBoxes = $("#dataTableBlock input[type='checkbox']").filter(function() {
                return this.id.match(checkboxIdMatch[1] + "(_[a-zA-Z0-9]*_){1}(Category|Product)$");
            })
            totalSubelements = childCheckBoxes.length;

            selectedSubElements = childCheckBoxes.filter(function() { return this.checked; }).length;
            
            
            if (selectedSubElements != 0) {
                checkbox.checked = true;
                $("#dataTableBlock input[type='hidden']").filter(function() {
                    return this.id.match(checkboxId + "_State");
                }).val("partial"); 
            } else {
                checkbox.checked = false;
                $("#dataTableBlock input[type='hidden']").filter(function() {
                    return this.id.match(checkboxId + "_State");
                }).val("");
            }

            if (totalSubelements == 0) {
                checkbox.checked = state;
                if (state) {
                    $("#dataTableBlock input[type='hidden']").filter(function() {
                        return this.id.match(checkboxId + "_State");
                    }).val("full");
                } else {
                    $("#dataTableBlock input[type='hidden']").filter(function() {
                        return this.id.match(checkboxId + "_State");
                    }).val("");
                }
            }

            if (totalSubelements == selectedSubElements && totalSubelements != 0) {
                if (state) {
                    $("#dataTableBlock input[type='hidden']").filter(function() {
                        return this.id.match(checkboxId + "_State");
                    }).val("partial");
                } else {
                    $("#dataTableBlock input[type='hidden']").filter(function() {
                        return this.id.match(checkboxId + "_State");
                    }).val("");
                }
            }
        }

        var parentIdRegExp = /([\w#]*)(_[a-zA-Z0-9]*)(_[a-zA-Z0-9]*)_(Category|Product)/;
        var parentIdMatch = parentIdRegExp.exec(checkboxId);
        if (parentIdMatch == null) {
            return;
        }
        var parentId = parentIdMatch[1] + parentIdMatch[2] + "_Category";
        
        ToggleParentCheckbox(parentId, state);
    }

    

    function ShowTableRow(checkboxId) {
        var rowId = "#" + checkboxId.replace("_Category", "_TableRow")

        var checkboxIdRegExp = /_([a-zA-Z0-9]*)_(Category|Product)/;
        var checkboxIdMatch = checkboxIdRegExp.exec(checkboxId);
        var oldId = checkboxId;
        checkboxId = checkboxIdMatch[1];

        $("#<%= DataTable.ClientID %> tr").filter(function () {
            return this.id.match("_" + checkboxId + "(_[a-zA-Z0-9]+)_TableRow");
        }).each(function (i) {
            $(this).css("display", "");
            $(this).children("td").children("div").children(".categoryImage").attr("src", "images/expand_blue.jpg");
            if (($(this).attr("class") == "categoryRow") && ($(this).attr("isShow") == "true")) {
                var localId = $(this).attr("id").replace("_TableRow", "_Category");
                $(this).unbind('click').bind('click', function (event) {
                    ShowTableRow(localId);
                });
            }
        });

        $(rowId).unbind('click').bind('click', function (event) {
            HideTableRow(oldId);
        });

        $("#<%= DataTable.ClientID %> tr").filter(function () {
            return this.id.match(checkboxId + "_TableRow");
        })[0].getElementsByTagName('img')[0].src = "images/collapse.jpg";
    }


    function HideTableRow(checkboxId) {
        var rowId = "#" + checkboxId.replace("_Category", "_TableRow")

        var checkboxIdRegExp = /_([a-zA-Z0-9]+)_(Category|Product)/;
        var checkboxIdMatch = checkboxIdRegExp.exec(checkboxId);
        var oldId = checkboxId;
        checkboxId = checkboxIdMatch[1];
        $("#<%= DataTable.ClientID %> tr").filter(function () {
            return this.id.match("_" + checkboxId + "(_[a-zA-Z0-9]+)+_TableRow");
        }).each(function (i) {
            $(this).css("display", "none");
        });

        $(rowId).unbind('click').bind('click', function (event) {
            ShowTableRow(oldId);
        });

        $("#<%= DataTable.ClientID %> tr").filter(function () {
            return this.id.match(checkboxId + "_TableRow");
        })[0].getElementsByTagName('img')[0].src = "images/expand_blue.jpg";
    }

    function onDataRowMouseOver(item) {
        item.style.backgroundColor = "#cccccc";
    }

    function onDataRowMouseOut(item) {
        item.style.backgroundColor = "";
    }

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function () {
            $("#<%= DataTable.ClientID %> tr").one('click', function (eventObject) {
                UpdateTable('0', $('#'+'<%= this.ClientID %>' +'__0_TableRow')[0], 0, '');
            });
            $("#dataTableBlock input[type='checkbox']").bind('click', function (eventObject) {
                eventObject.stopPropagation();
                ToggleCheckBox(eventObject.target);
            });

            var a = $("#dataTableBlock input[type='checkbox']");
        });
</script>
