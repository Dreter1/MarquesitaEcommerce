function getProductPanelList() {
    var url = "/Product/GetProductList";
    $.get(url, function (e) {
        $("#ProducList").html(e);
    });
}