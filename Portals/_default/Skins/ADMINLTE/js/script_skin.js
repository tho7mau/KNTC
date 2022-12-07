$(function () {
    //Make the table scroll with the header/footer stuck in place

    var mytable = jQuery("table.footable:eq(0)");
    var h = mytable.height();
    //Add the thead that datagrid omits:
    mytable.prepend(document.createElement('thead'));
    //Move the first row from tbody to thead:
    jQuery("table.footable:eq(0) thead").append(jQuery("table.footable:eq(0) tbody tr:eq(0)"));
    //mytable.Scrollable(400 < h ? 400 : h, ct.width() + 30);
    $('table.footable:eq(0)').footable();

    //resizeScreen();
    //$(window).resize(function () {
    //    resizeScreen();
    //});
    //function resizeScreen() {

    //}
	var mytable2 = jQuery("table.footable2");
    var h2 = mytable2.height();
    //Add the thead that datagrid omits:
    mytable2.prepend(document.createElement('thead'));
    //Move the first row from tbody to thead:
    jQuery("table.footable2 thead").append(jQuery("table.footable2 tbody tr:eq(0)"));
    //mytable.Scrollable(400 < h ? 400 : h, ct.width() + 30);
    $('table.footable2').footable();
});