$(function () {

    $(document).on('click.bs.dropdown.data-api', '.dropdown.dropdown-filter', function (e) {
        e.stopPropagation();
    });
    //initSearch();
   
    //$('.datepicker').datetimepicker({
    //    format: 'L'
    //});

    //$('li.dropdown.dropdown-filter .dropdown-item').on('click', function (event) {

    //    $(this).toggleClass('check');
    //    var attr = $(this).attr('data-type');
    //    var option_key = $(this).data("option");
    //    // For some browsers, `attr` is undefined; for others,
    //    // `attr` is false.  Check for both.
    //    if (typeof attr !== 'undefined' && attr !== false) {
    //        var OptionSearchDatime = $('.form-group-seach-' + option_key + '');
    //        $(OptionSearchDatime).toggleClass('hidden');
    //    }
    //    else {
    //        // ...
    //        var OptionSearch = $('.OptionSearch');

    //        let title = this;
    //        //$(title).find('i').remove();

    //        title = title.innerText;
    //        if ($(this).hasClass("check")) {

    //            var strButton = getButton(title, option_key, 'normal', '', '');
    //            $(OptionSearch).append(strButton);
    //        }
    //        else {
    //            $(OptionSearch).append(strButton);
    //            $(OptionSearch).find('.btn-group[data-option="' + option_key + '"]').remove();
    //        }
    //    }

    //});
    //$('.OptionSearch').on('click', '.CloseOptionSearch', function () {
    //    //$('button').on('click', function (event) {
    //    var divgroup = $(this).parent();
    //    var option_key = $(divgroup).data("option");
    //    var OptionSearch = $('.OptionSearch');
    //    $(OptionSearch).find('.btn-group[data-option="' + option_key + '"]').remove();
    //    var dropdownitem = $('li.dropdown.dropdown-filter a[data-option="' + option_key + '"]');
    //    $(dropdownitem).toggleClass('check');

    //    var OptionSearchDatime = $('.form-group-seach-' + option_key + '');
    //    $(OptionSearchDatime).toggleClass('hidden');
    //});
    //$('.datetime-content').on('click', '.Applay-seacrh', function () {

    //    var OptionSearch = $('.OptionSearch');
    //    var option_title = $(this).data("title");
    //    var option_key = $(this).data("option");
    //    var value1 = document.getElementById('input' + option_key + '1').value;
    //    var value2 = document.getElementById('input' + option_key + '2').value;
    //    if (value1 != "" && value2 != "") {
    //        option_title = option_title + " từ " + value1 + " đến " + value2;
    //        var strButton = getButton(option_title, option_key, 'datetime', value1, value2);
    //        $(OptionSearch).append(strButton);


    //    }
    //    alert(option_key);
    //});
});
function initSearch()
{
    $(document).on('click.bs.dropdown.data-api', '.dropdown.dropdown-filter', function (e) {
        e.stopPropagation();
    });
    
    //var OptionSearch_Save = $('div.OptionSearchSave.OptionSearch');
    //var OptionSearch = $('div.OptionSearchDisplay.OptionSearch');
    //$(OptionSearch).append(OptionSearch_Save.html());

    //$('.datepicker').datetimepicker({
    //    format: 'L'
    //});
   
    $('li.dropdown.dropdown-filter .dropdown-item').on('click', function (event) {

        //alert(this);
        //$(this).toggleClass('check');
        if ($(this).hasClass("check"))
        {
            $(this).removeClass('check');
        }
        else {
            $(this).addClass('check');
        }
  
        var attrtype = $(this).attr('data-type');
        var option_key = $(this).data("option");
        var option_value = $(this).data("value");
        // For some browsers, `attr` is undefined; for others,
        // `attr` is false.  Check for both.
        if (attrtype == 'datetime') {
            //alert(option_key);
            var OptionSearchDatime = $('.form-group-seach[data-key="' + option_key + '"]');
            $(OptionSearchDatime).toggleClass('hidden');
        }
        else {
            // ...
            var OptionSearch = $('.OptionSearch');
            let title = this;
            //$(title).find('i').remove();

            title = title.innerText;
            if ($(this).hasClass("check")) {
               
                if ( attrtype == 'normal')
                {
                    $('.textSearchContent_HiddenField').val($('.textSearchContent_HiddenField').val() + "|" + "" + option_key + "," + attrtype + ",,");
                    var strButton = getButton(title, option_key, attrtype, '', '');
                    $(OptionSearch).append(strButton);
                }
                else if (attrtype == 'equal')
                {
                    $('.textSearchContent_HiddenField').val($('.textSearchContent_HiddenField').val() + "|" + "" + option_key + "," + attrtype + "," + option_value + ",");
                    var strButton = getButton(title, option_key, attrtype, option_value, '');
                    $(OptionSearch).append(strButton);
                }
               
            }
            else {
                $(OptionSearch).append(strButton);
                // Start Command
                //Trường hợp bỏ check chọn tiêu chí equal có cùng thuộc tính -> phân biệt bổ sung value 
                //(Tránh trường hợp remove hết các tiêu chí tìm kiếm cùng thuộc tính trên textbox tìm kiếm)
                if (attrtype == 'equal') {
                    
                    $(OptionSearch).find('.btn-group[data-option="' + option_key + '"][data-value1="' + option_value + '"]').remove();
                }
                else {
                    $(OptionSearch).find('.btn-group[data-option="' + option_key + '"]').remove();
                }
                // End
                
                DeleteKeySeach(option_key, attrtype, option_value);
            }
        }

    });
    $('.OptionSearch').on('click', '.CloseOptionSearch', function () {
      
        var divgroup = $(this).parent();

        var option_key = $(divgroup).data("option");
        var type = $(divgroup).data('type');
        var option_value1 = $(divgroup).data("value1");
        var option_value2 = $(divgroup).data("value2");

        
        DeleteKeySeach(option_key, type, option_value1, option_value2);

        var OptionSearch = $('.OptionSearch');
        // Start Command
        //Trường hợp bỏ check chọn tiêu chí equal có cùng thuộc tính -> phân biệt bổ sung value 
        //(Tránh trường hợp remove hết các tiêu chí tìm kiếm cùng thuộc tính trên textbox tìm kiếm)
        if (type == 'equal') {

            $(OptionSearch).find('.btn-group[data-option="' + option_key + '"][data-value1="' + option_value1 + '"]').remove();
            var dropdownitem = $('li.dropdown.dropdown-filter a[data-option="' + option_key + '"][data-value="' + option_value1 +'"]');
            $(dropdownitem).toggleClass('check');
        }
        else {
            $(OptionSearch).find('.btn-group[data-option="' + option_key + '"]').remove();
            var dropdownitem = $('li.dropdown.dropdown-filter a[data-option="' + option_key + '"]');
            $(dropdownitem).toggleClass('check');
        }
        var OptionSearchDatime = $('.form-group-seach-' + option_key + '');
        $(OptionSearchDatime).toggleClass('hidden');
    });
    $('.datetime-content').on('click', '.Applay-seacrh', function () {

        var OptionSearch = $('.OptionSearch');
        var option_title = $(this).data("title");
        var option_key = $(this).data("option");
        var value1 = document.getElementById('input' + option_key.replace(".", "_") + '1').value;
        var value2 = document.getElementById('input' + option_key.replace(".", "_") + '2').value;
        // Xóa option datetime đã chọn 
        DeleteKeySeach(option_key, 'datetime', value1, value2);
        var OptionSearch = $('.OptionSearch');
        $(OptionSearch).find('.btn-group[data-option="' + option_key + '"]').remove();

        if (value1 != "" && value2 != "") {
            option_title = option_title + " từ " + value1 + " đến " + value2;
            var strButton = getButton(option_title, option_key, 'datetime', value1, value2);
            $(OptionSearch).append(strButton);
            $('.textSearchContent_HiddenField').val($('.textSearchContent_HiddenField').val() + "|" + "" + option_key + ",datetime," + value1+','+ value2);
        }
    });

    var itemOptiondatetimes = $('li.dropdown.dropdown-filter .dropdown-item[data-type="datetime"]');
    if (itemOptiondatetimes.length > 0)
    {
        for (var i = 0; i < itemOptiondatetimes.length; i++) {

            itemOption = itemOptiondatetimes[i];
            var option_key = $(itemOption).data("option"); 
            title = itemOption.innerText;
            
            var Optiondatetime_HTML = getOptiondatetime_HTML(title, option_key, i);
            $('.datetime-content[data-option="' + option_key+'"]').append(Optiondatetime_HTML)
            //$(itemOption).append(Optiondatetime_HTML);

        }
    }
    rebidingdata();
}
function getOptiondatetime_HTML(title, option_key,numitem) {
    //option_key = numitem;
    var day = new Date();
    var lasmonth = new Date();
    lasmonth.setMonth(lasmonth.getMonth() - 1);
    //var lasmonth = today.getMonth() - 1;
    console.log(day.toLocaleDateString("vi-VN")); // 9/17/2016
    //alert(lasmonth.toLocaleDateString("vi-VN"));
    var strButton = "";
    strButton = strButton + "<div class='form-group hidden form-group-seach' data-key=" + option_key+">";
    strButton = strButton + "<select class='form-control'>";
    strButton = strButton + " <option value='between'>Trong khoảng</option>";
    //strButton = strButton + "<option>option 2</option>";
    //strButton = strButton + "<option>option 3</option>";
    //strButton = strButton + "<option>option 4</option>";
    //strButton = strButton + "<option>option 5</option>";
    strButton = strButton + " </select>";
    strButton = strButton + "<div class='input-group date datepicker' id='date" + option_key.replace(".", "_") + "1' data-target-input='nearest'>";
    strButton = strButton + "<input type='text' class='form-control datetimepicker-input' value='" + lasmonth.toLocaleDateString("vi-VN") + "' data-target='#date" + option_key.replace(".", "_") + "1' id='input" + option_key.replace(".","_") + "1' />";
    strButton = strButton + "<div class='input-group-append' data-target='#date" + option_key.replace(".", "_") +"1' data-toggle='datetimepicker'>";
    strButton = strButton + "<div class='input-group-text'><i class='icofont-ui-calendar  mb - 0'></i></div>";
    strButton = strButton + "</div></div>";
    strButton = strButton + "<div class='input-group date datepicker' id='date" + option_key.replace(".", "_") + "2' data-target-input='nearest'>";
    strButton = strButton + "<input type='text' class='form-control datetimepicker-input' value='" + day.toLocaleDateString("vi-VN") + "' data-target='#date" + option_key.replace(".", "_") + "2' id='input" + option_key.replace(".", "_") +"2' />";
    strButton = strButton + "<div class='input-group-append' data-target='#date" + option_key.replace(".", "_") + "2' data-toggle='datetimepicker'>";
    strButton = strButton + "<div class='input-group-text'><i class='icofont-ui-calendar  mb - 0'></i></div>";
    strButton = strButton + "</div></div>";
    strButton = strButton + "<div class='col-sm-6 offset-md-6' ><button type='button' class='btn btn-block bg-gradient-primary btn-xs Applay-seacrh' data-option='" + option_key + "' data-title='" + title +"'  >Áp dụng</button></div>"
    strButton = strButton + "</br></div>";
    
    return strButton;
}

function getButton(title, option_key, type , value1, value2)
{

    var strButton = "";
    strButton = strButton + "<div class='btn-group btn-group-search-option' data-option='" + option_key + "' data-type='" + type + "' data-value1='" + value1 + "'  data-value2='" + value2 + "' >";
    if (type == 'datetime')
    {
        strButton = strButton + "<button type = 'button' class='btn btn-xs ' ><i class='icofont-calendar text-primary'></i></button >";
    }
    else
    {
        strButton = strButton + "<button type = 'button' class='btn btn-xs ' ><i class='icofont-filter text-primary'></i></button >";

    }
   
    strButton = strButton + "<button type='button' class='btn btn-xs '>" + title+"</button>";
    strButton = strButton + "<button type='button' class='btn btn-xs CloseOptionSearch' ><i class='icofont-close'></i></button>";//onclick='CloseOptionSearch()'
    strButton = strButton +  "</div >";
    return strButton;
}
function CloseOptionSearch()
{
    var divgroup = $(this).parent();
    var option_key = $(divgroup).data("option"); 
    var OptionSearch = $('.OptionSearch');
    $(OptionSearch).find('.btn-group[data-option="' + option_key + '"]').remove();

   // alert("aa");
}
function DeleteKeySeach(key, option, value1, value2 ) {
    var textSearchContent_HiddenField = $('.textSearchContent_HiddenField').val();
    var keyindexOf_start = 0;
    var keyindexOfseparator = 0;
    if (option == 'normal' || option == 'datetime')
    {
        var keyindexOf_start = textSearchContent_HiddenField.indexOf(key + "," + option);
        if (keyindexOf_start > 0) {
            var keyindexOfseparator = textSearchContent_HiddenField.indexOf("|", keyindexOf_start + 1);

            if (keyindexOfseparator < 0)
            {
                keyindexOfseparator = textSearchContent_HiddenField.length;
            }
            
        }
    }
    else
    {
        var keyindexOf_start = textSearchContent_HiddenField.indexOf(key + "," + option + "," + value1);
        if (keyindexOf_start > 0) {

            var keyindexOfseparator = textSearchContent_HiddenField.indexOf("|", keyindexOf_start);
            if (keyindexOfseparator < 0) {
                keyindexOfseparator = textSearchContent_HiddenField.length ;
            }

        }
    }
    var strreplace = textSearchContent_HiddenField.substring(keyindexOf_start, (keyindexOfseparator));

   
    var textSearchContent_HiddenField = textSearchContent_HiddenField.replace("|" + strreplace, "");
    $('.textSearchContent_HiddenField').val(textSearchContent_HiddenField);
}
function rebidingdata()
{
    var OptionSearch_Save = $('div.OptionSearchSave.OptionSearch');
    var OptionSearch = $('div.OptionSearchDisplay.OptionSearch');
    $(OptionSearch).append(OptionSearch_Save.html());
    var buttongroups = $('div.OptionSearchDisplay.OptionSearch').find(".btn-group");
    for (var i = 0; i < buttongroups.length; i++) {

        var option_key = $(buttongroups[i]).data("option");
        //Start command
        // Trường hợp rebiding cùng option -> tránh trường hợp chọn tất các tiêu chí trên dropdown
        if ($(buttongroups[i]).data("type") == "equal") {
            var option_value1 = $(buttongroups[i]).data("value1"); // Dùng giá trị value1 để phân biệt
            $('li.dropdown.dropdown-filter .dropdown-item[data-option="' + option_key + '"][data-value="' + option_value1+'"]').addClass("check");
        }
        else {
            $('li.dropdown.dropdown-filter .dropdown-item[data-option="' + option_key + '"]').addClass("check");
        }
        //End
        $('.form-group-seach[data-key="' + option_key + '"]').removeClass("hidden") ;
    }
}

//$('.CloseOptionSearch').on('click', function (event) {
//        alert("aa");
//        var divgroup = $(this).parent();
//        var option_key = $(divgroup).data("option"); 
//        var OptionSearch = $('.OptionSearch');
//        $(OptionSearch).find('.btn-group[data-option="' + option_key + '"]').remove();

//    });