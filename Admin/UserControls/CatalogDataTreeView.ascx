<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CatalogDataTreeView.ascx.cs"
    Inherits="UserControls_CatalogDataTreeView" %>
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
    <%--<asp:UpdatePanel ID="DataTableUpdatePanel" runat="server">
        <ContentTemplate>--%>
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
                    <asp:TableRow ID="mainTr" CssClass="unvisible">
                    </asp:TableRow>
                </asp:Table>
            </div>
        <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</div>
<script type="text/javascript">
    var TREEVIEW_ID = "#<%= ClientID %>";
    var SELECTED_ID = "#<%= TotalProductSelectedLiteral.ClientID %>";

    function insertElements(parentElementId, itemsArray, depth) {
        var frag = document.createDocumentFragment();

        for (var i = 0; i < itemsArray.length; i++) {
            var element = getElement(parentElementId, itemsArray[i], depth);
            if (element != null) {
                AddMargin(element, depth);
                frag.appendChild(element);
            }
        }

        $("#" + parentElementId).after(frag);
        $("." + parentElementId).remove();

        /*
        var depthCategory = 5 + 20 * depth
        $(".depthCategory_" + depth).css("margin", "0px 0px 0px " + depthCategory + "px;");

        var depthProduct = 30 + 20 * depth
        $(".depthProduct_" + depth).css("margin", "0px 0px 0px " + depthProduct + "px;");
        */
    }

    function getElement(parentElementId, item, depth) {
        var parentId = parentElementId;

        if (parentId == "<%= mainTr.ClientID %>") {
            parentId = "";
        }
        parentId = parentId.replace("_TableRow", "");

        var elementId = parentId + "_" + item.Id;

        switch (item.Type) {
            case 0: return getCategoryElement(elementId, item, depth); break;
            case 1: return getProductElement(elementId, item, depth); break;
        }

        return null;
    }

    function getCategoryElement(elementId, item, depth) {
        var tr = document.createElement("tr");
        tr.id = elementId + "_TableRow";
        tr.setAttribute("onmouseout", "onDataRowMouseOut(this);");
        tr.setAttribute("onmouseover", "onDataRowMouseOver(this);");
        tr.className = "categoryRow";

        var td = document.createElement("td");
        tr.appendChild(td);
        var td2 = document.createElement("td");
        tr.appendChild(td2);
        var td3 = document.createElement("td");
        tr.appendChild(td3);

        var div = document.createElement("div");
        div.id = "divCat" + elementId;
        $(div).addClass("categoryIndentFiller depthCategory_" + depth);
        td.appendChild(div);

        var img = document.createElement("img");
        img.src = "../Admin/images/expand_blue.jpg";
        img.alt = "<%= Resources.Resource.Admin_ExportFeed_Expandcategory %>";
        img.className = "categoryImage";
        div.appendChild(img);

        var hiddenField = document.createElement("input");
        hiddenField.id = elementId + "_Category_State";
        hiddenField.name = elementId + "_Category_State";
        hiddenField.value = (item.Checked == "True") ? "partical" : "";
        hiddenField.type = "hidden";
        div.appendChild(hiddenField);

        var checkBox = document.createElement("input");
        checkBox.id = elementId + "_Category";
        checkBox.name = elementId + "_Category";
        checkBox.type = "checkbox";
        checkBox.defaultChecked = item.Checked == "True";
        div.appendChild(checkBox);

        var a = document.createElement("a");
        a.className = "categoryLink";
        div.appendChild(a);

        var span = document.createElement("span");
        span.className = "categoryLiteral";
        a.appendChild(span);

        var text = document.createTextNode(item.Name + " (" + item.Value + ")");
        span.appendChild(text);

        return tr;
    }

    function getProductElement(elementId, item, depth) {
        var tr = document.createElement("tr");
        tr.id = elementId + "_TableRow";
        tr.setAttribute("onmouseout", "onDataRowMouseOut(this);");
        tr.setAttribute("onmouseover", "onDataRowMouseOver(this);");
        tr.className = "productRow";

        var firstTd = document.createElement("td");
        tr.appendChild(firstTd);

        var div = document.createElement("div");
        div.id = "divPro" + elementId;
        $(div).addClass = ("categoryIndentFiller depthProduct_" + depth);
        firstTd.appendChild(div);

        var hiddenField = document.createElement("input");
        hiddenField.id = elementId + "_Product_State";
        hiddenField.name = elementId + "_Product_State";
        hiddenField.value = (item.Checked == "True") ? "full" : "";
        hiddenField.type = "hidden";
        div.appendChild(hiddenField);

        var checkBox = document.createElement("input");
        checkBox.id = elementId + "_Product";
        checkBox.name = elementId + "_Product";
        checkBox.type = "checkbox";
        checkBox.defaultChecked = item.Checked == "True";
        div.appendChild(checkBox);

        var nameSpan = document.createElement("span");
        nameSpan.className = "productLiteral";
        div.appendChild(nameSpan);

        var text = document.createTextNode(item.Name);
        nameSpan.appendChild(text);

        var secondTd = document.createElement("td");
        tr.appendChild(secondTd);

        var price = document.createTextNode(item.Value.split('|')[0]);
        secondTd.appendChild(price);

        var thirdTd = document.createElement("td");
        tr.appendChild(thirdTd);

        var unit = document.createTextNode(item.Value.split('|')[1]);
        thirdTd.appendChild(unit);

        return tr;
    }



    function UpdateTable(categoryId, catRow, depth, recurse) {
        var _catId = "#" + catRow.id;
        $(_catId).attr("isShow", "true");
        $(_catId).off("click", "**");
        $(_catId).on("click", function (eventObject) {
            HideTableRow(catRow.getElementsByTagName("input")[1].id);
        });

        if (catRow.getElementsByTagName('img').length != 0) {
            catRow.getElementsByTagName('img')[0].src = "images/collapse.jpg";
        }

        $("#" + catRow.id + " .categoryLink").after("<span class='" + catRow.id + "'>&nbsp;loading...</span>");
        jQuery.ajax({
            url: '../HttpHandlers/ExportFeedGetCategoryTable.ashx',
            dataType: "json",
            cache: false,
            data: { catId: categoryId, depth: depth, recurse: recurse },
            success: function (data) {
                var catId = "#" + catRow.id;
                if (catRow.id == "<%= mainTr.ClientID %>") {
                    catId = "#_0_TableRow";
                }

                itemsArray = data;
                insertElements(catRow.id, data, depth);
                var rows = $("#<%= DataTable.ClientID %> tr").filter(function () {
                    return this.id.search("_TableRow") >= 0 && this.id.search("_" + categoryId + "_") >= 0; // this.id.match(categoryId + "(_[a-zA-Z0-9]+)+_TableRow");
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
                    if ((row.id != catRow.id) && (anc.html() != "(0)")) {
                        AddUpdateTableHandler('#' + row.id, Id, row, depth + 1);
                    } else {
                        $("#" + row.id).css("cursor", "auto");
                    }

                    if (selected) {
                        $("#" + row.id + " input:checkbox").attr("checked", "true");
                        $("#" + row.id + " input:hidden").val("full");
                    }                    
                }

                rows.find("input[type='checkbox']").on("click", function (eventObject) {
                    eventObject.stopPropagation();
                    ToggleCheckBox(eventObject.target);
                });

                $("span." + catRow.id).remove();
            }
        });
    }

    function AddMargin(element, depth) {
        var divDepth = 0;
        var needDiv = element.children[0].children[0];
        if (needDiv.id.indexOf("divCat") != -1) {
            divDepth = 5 + 20 * depth;
        } else if (needDiv.id.indexOf("divPro") != -1) {
            divDepth = 30 + 20 * depth;
        }

        if (divDepth != 0) {
            needDiv.style.marginLeft = divDepth + "px";
        }
    }

    function AddUpdateTableHandler(selector, id, row, depth) {
        $(selector).one("click", function (event) {
            UpdateTable(id, row, depth, true);
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
        var cb = $("#dataTableBlock input[type='checkbox']").filter(function () {
            return this.id.match("__[a-zA-Z0-9]+_Category");
        });
        for (var i = 0; i < cb.length; ++i) {
            var idRegExp = /_(_[a-zA-Z0-9])+_Category/;
            var m = idRegExp.exec(cb[i].id);
            ToggleChildCheckbox(m[1] + "_Category", true);
        }
    }

    function DeselectAll() {
        var cb = $("#dataTableBlock input[type='checkbox']").filter(function () {
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
        $("#dataTableBlock input[type='checkbox']").filter(function () {
            return this.id.search(checkboxId.replace("_Category", "")) >= 0;
        }).each(function (i) {
            document.getElementById(this.id + "_State").value = state ? "full" : "";
            document.getElementById(this.id).checked = state;
        });
    }

    function ToggleParentCheckbox(checkboxId, state) {
        var checkbox = $("input[type='checkbox']").filter(function () {
            return this.id.match(checkboxId);
        })[0];

        var checkboxIdRegExp = /(.*)_Category/;
        var checkboxIdMatch = checkboxIdRegExp.exec(checkboxId);
        if (checkboxIdMatch != null) {
            var subElementsRegExp = new RegExp(checkboxIdMatch[1] + "(_[a-zA-Z0-9]*_){1}(Category|Product)$");
            var checkboxes = document.getElementById("dataTableBlock").getElementsByTagName("input");

            var totalSubelements = 0;
            var selectedSubElements = 0;

            var childCheckBoxes = $("#dataTableBlock input[type='checkbox']").filter(function () {
                return this.id.match(checkboxIdMatch[1] + "(_[a-zA-Z0-9]*_){1}(Category|Product)$");
            })
            totalSubelements = childCheckBoxes.length;

            selectedSubElements = childCheckBoxes.filter(function () { return this.checked; }).length;


            if (selectedSubElements != 0) {
                checkbox.checked = true;
                $("#dataTableBlock input[type='hidden']").filter(function () {
                    return this.id.match(checkboxId + "_State");
                }).val("partial");
            } else {
                checkbox.checked = false;
                $("#dataTableBlock input[type='hidden']").filter(function () {
                    return this.id.match(checkboxId + "_State");
                }).val("");
            }

            if (totalSubelements == 0) {
                checkbox.checked = state;
                if (state) {
                    $("#dataTableBlock input[type='hidden']").filter(function () {
                        return this.id.match(checkboxId + "_State");
                    }).val("full");
                } else {
                    $("#dataTableBlock input[type='hidden']").filter(function () {
                        return this.id.match(checkboxId + "_State");
                    }).val("");
                }
            }

            if (totalSubelements == selectedSubElements && totalSubelements != 0) {
                if (state) {
                    $("#dataTableBlock input[type='hidden']").filter(function () {
                        return this.id.match(checkboxId + "_State");
                    }).val("partial");
                } else {
                    $("#dataTableBlock input[type='hidden']").filter(function () {
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
            $(this).show();
            $(this).children("td").children("div").children(".categoryImage").attr("src", "images/expand_blue.jpg");
            if (($(this).attr("class") == "categoryRow") && ($(this).attr("isShow") == "true")) {
                var localId = $(this).attr("id").replace("_TableRow", "_Category");
                $(this).off("click", "**").on("click", function (event) {
                    ShowTableRow(localId);
                });
            }
        });

        $(rowId).off("click", "**").on("click", function (event) {
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
            $(this).hide();
        });

        $(rowId).off("click", "**").on("click", function (event) {
            ShowTableRow(oldId);
        });

        $("#<%= DataTable.ClientID %> tr").filter(function () {
            return this.id.match(checkboxId + "_TableRow");
        })[0].getElementsByTagName('img')[0].src = "images/expand_blue.jpg";
    }

    function collapseTree() {
        var element = $("#<%= DataTable.ClientID %> tr").filter(function () {
            return this.id.match("0_TableRow");
        })[0];

        if (element.getElementsByTagName('img')[0].src.match("images/collapse.jpg") != null) {
            $("#" + element.id).click();
        }
    }

    function onDataRowMouseOver(item) {
        item.style.backgroundColor = "#cccccc";
    }

    function onDataRowMouseOut(item) {
        item.style.backgroundColor = "";
    }

    $(document).ready(function () {
        UpdateTable('0', $('#<%= mainTr.ClientID %>')[0], 0, false);
    });
</script>
