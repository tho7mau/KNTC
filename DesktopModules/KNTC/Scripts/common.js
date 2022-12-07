
function ConfirmDelete() {
    if (confirm("Bạn có muốn xóa không?")) {
        return true;
    }
    else {
        return false;
    }
}

function ConfirmRestore() {
    if (confirm("Bạn có muốn khôi phục?")) {
        return true;
    }
    else {
        return false;
    }
}

function ConfirmAction() {
    if (confirm("Bạn có chắc muốn thực hiện thao tác này không?") == true) {
        return true;
    }
    else {
        return false;
    }
}
function CannotDelete() {
    alert("Dữ liệu đã được sử dụng không thể xóa?")
    return;
}

function DaKhoaSo() {
    alert("Dữ liệu đã khóa")
    return;
}
function THONGBAOKHONGTONTAI(obj) {
    alert(obj)
    return;
}

function OpenWindow(url) {
    var x = window.open(url, 'FS', 'width=425, height=200,resizeable=yes,menubar=0,toolbar=0')
    x.focus;
}

////////////////////////////////////////////////////////////////////////
// File name    :   Common.js
// Purpose      :   Chua cac ham javascript cho du an
// Create date  :   10/10/2007
// Author       :   DVTIN
// Version      :   v1.0
// Copyright    :   CUSC
///////////////////////////////////////////////////////////////////////
function ConfirmDelete() {
    if (confirm("Bạn có muốn xóa không?") == true) {
        return true;
    }
    else {
        return false;
    }
}
function ConfirmDelete(id) {
    if (confirm("Bạn có muốn xóa không?") == true) {
        var ctl = document.getElementById(id);
        if (ctl) ctl.checked = true;
        return true;
    }
    else {
        return false;
    }
}
//////////////////////////////////////////////////////////////////
function validateLength(oSrc, args) {
    args.IsValid = (args.Value.Length > 1)
}

////////////////////////////////////////////////////////////////////////////
// Loc bo dau tru
function Check_SoAm(obj) {
    var so;
    so = (obj.value);
    while (so.indexOf('-') != -1) {
        so = so.replace('-', '');
    }
    obj.value = so;
    return true;
}

///////////////////////////////////////////////////////////////////////////
// Ham chi cho nhap so
function CheckNumber(e, Id) {
    if (e.keyCode < 48 || e.keyCode > 57) {
        alert("Chỉ nhập số");
        return false;
    }
    else {
        return true;
    }
}
///////////////////////////////////////////////////////////////////////////
// Kiem tra so
function Check_num(obj) {
    var so;
    so = (obj.value);
    if (so == '') {
        obj.value = '0';
        return false;
    }
    while (so.indexOf(' ') != -1) {
        so = so.replace(' ', '');
        obj.value = '0';
        return false;
    }
    while (so.indexOf('.') != -1) {
        so = so.replace('.', '');
    }

    // Loc bo dau "tru" khi nhap sp am
    while (so.indexOf('-') != -1) {
        so = so.replace('-', '');
    }

    so = (so).replace(',', '.');
    if (so * 1 > 9999999999) {
        alert('Số nhập vào không được lớn hơn số 9.999.999.999 !');
        obj.value = '0';
        obj.focus();
        return false;
    }
    if (isNaN(so)) {
        alert('Dữ liệu nhập phải là kiểu số.');
        obj.value = '0';
        obj.focus();
        return false;
    }
    else {
        obj.value = convertSoVN(so);
        return true;
    }
}

var focusInterval = "";
var focusId = "";
function SetFocus(id) {
    var ctl = document.getElementById(id);
    if (ctl) {
        focusId = id;
        ctl.lang = "on";
        focusInterval = setInterval("LoopFocus()", 200);
    }
}

function LoopFocus() {
    if ((focusInterval != "") && (focusId != "")) {
        try {
            var ctl = document.getElementById(focusId);
            if (ctl) {
                ctl.focus();
                if (ctl.lang != "on") {
                    if (navigator.appName == 'Microsoft Internet Explorer') {
                        if (document.activeElement.id != focusId) return;
                    }
                    else {
                        if (document.getElementById('newdiv')) return;
                    }
                    clearInterval(focusInterval);
                    focusInterval = focusId = "";
                }
            }
        }
        catch (err) {
            clearInterval(focusInterval);
            focusInterval = focusId = "";
        }
    }
}

var borderInterval = "";
var borderId = "";
function SetBorder(id) {
    var ctl = document.getElementById(id);
    if (ctl) ctl.lang = "on";
    borderId = id;
    borderInterval = setInterval("LoopBorder()", 10);
}

function LoopBorder() {
    if ((borderInterval != "") && (borderId != "")) {
        try {
            var ctl = document.getElementById(borderId);
            var tb = document.getElementById("tbLanhDao");
            if (ctl) {
                var c = ctl.getElementsByTagName('td');
                for (var i = 0; i < c.length; i++) c[i].style.borderWidth = '0px';

                if (tb) {
                    var td = tb.getElementsByTagName('td');
                    for (var i = 0; i < td.length; i++) td[i].style.borderWidth = '0px';
                }

                if (ctl.lang != "on") {
                    clearInterval(borderInterval);
                    borderInterval = borderId = "";
                }
            }
        }
        catch (err) {
            clearInterval(borderInterval);
            borderInterval = borderId = "";
        }
    }
}


var borderIntervalB = "";
var borderIdB1 = "";
var borderIdB2 = "";
function SetBorderB(id1, id2) {
    var ctl1 = document.getElementById(id1);
    var ctl2 = document.getElementById(id2);
    if (ctl1 && ctl2) {
        ctl1.lang = ctl2.lang = "on";
        borderIdB1 = id1;
        borderIdB2 = id2;
        borderIntervalB = setInterval("LoopBorderB()", 10);
    }
}

function LoopBorderB() {
    if ((borderIntervalB != "") && (borderIdB1 != "") && (borderIdB2 != "")) {
        try {
            var ctl1 = document.getElementById(borderIdB1);
            var ctl2 = document.getElementById(borderIdB2);
            if (ctl1 && ctl2) {
                var c1 = ctl1.getElementsByTagName('td');
                var c2 = ctl2.getElementsByTagName('td');
                for (var i = 0; i < c1.length; i++) c1[i].style.borderWidth = '0px';
                for (var i = 0; i < c2.length; i++) c2[i].style.borderWidth = '0px';

                if ((ctl1.lang != "on") && (ctl2.lang != "on")) {
                    clearInterval(borderIntervalB);
                    borderIntervalB = borderIdB1 = borderIdB2 = "";
                }
            }
            else {
                clearInterval(borderIntervalB);
                borderIntervalB = borderIdB1 = borderIdB2 = "";
            }
        }
        catch (err) {
            clearInterval(borderIntervalB);
            borderIntervalB = borderIdB1 = borderIdB2 = "";
        }
    }
}

//only use for onkeypress
function AcceptNumber(obj, e) {
    try {
        var key;
        var keychar;
        if (window.event) key = window.event.keyCode;
        else if (e) key = e.which;
        else return true;
        keychar = String.fromCharCode(key);
        if (key == 13) {
            RightMessage(obj, "Chỉ được nhập số"); return false;
        }
        if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 27)) return true;
        else if ((("0123456789").indexOf(keychar) > -1)) return true;
        else {
            RightMessage(obj, "Chỉ được nhập số");
            return false;
        }
    }
    catch (err) { }
}

function CheckZero(obj) {
    if (parseFloat(obj.value) <= 0) {
        obj.value = "";
        RightMessage(obj, "Chỉ được nhập số lớn hơn 0");
    }
}

//firefox
function removeDropNumber(obj) {
    try {
        if (navigator.appName != 'Microsoft Internet Explorer') {
            var text = obj.value;
            var num = "0123456789";
            for (var i = 0; i < text.length; i++) {
                var ch = text.charAt(i);
                if (num.indexOf(ch) == -1) {
                    obj.value = "";
                    break;
                }
            }
            if ((obj.lang != obj.value) && (obj.attributes.getNamedItem("lang") != null)) obj.value = obj.lang;
        }
    }
    catch (err) { }
}

//firefox
function ClearValue(obj) {
    try {
        if (navigator.appName != 'Microsoft Internet Explorer') {
            obj.removeAttribute("lang");
        }
    }
    catch (err) { }
}
function rememberValue(obj) {
    try {
        if (navigator.appName != 'Microsoft Internet Explorer') {
            obj.lang = obj.value;
        }
    }
    catch (err) { }
}

//only use for onkeypress, onkeydown
function NotEditTextInput(e) {
    try {
        if ((e.keyCode == 9) || (e.keyCode == 116)) return true;
        else return false;
    }
    catch (err) { }
}

var textChange = null;
function OnKeyDown(obj) {
    try {
        if (navigator.appName != 'Microsoft Internet Explorer') {
            textChange = obj.options[obj.selectedIndex].text;
        }
    }
    catch (err) { }
}

function OnKeyUp(obj) {
    try {
        if (navigator.appName != 'Microsoft Internet Explorer') {
            if (textChange != null) {
                try {
                    if (textChange != obj.options[obj.selectedIndex].text)
                        obj.onchange();
                    else return false;
                }
                catch (err) { }
            }
        }
    }
    catch (err) { }
}

function PhoneKeyPress(obj, e) {
    try {
        var key;
        var keychar;
        if (window.event) key = window.event.keyCode;
        else if (e) key = e.which;
        else return true;
        keychar = String.fromCharCode(key);
        if (key == 13) {
            RightMessage(obj, "Chỉ được nhập số và một dấu chấm phân cách"); return false;
        }
        if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 27)) return true;
        else if (((".0123456789").indexOf(keychar) > -1)) return true;
        else {
            RightMessage(obj, "Chỉ được nhập số và một dấu chấm phân cách"); return false;
        }
    }
    catch (err) { }
}

function PhoneKeyUp(obj) {
    try {
        while (obj.value.indexOf('.') > -1) {
            if (((obj.value.indexOf('.') == 3) || (obj.value.indexOf('.') == 4)) && (obj.value.indexOf('.') == obj.value.lastIndexOf('.'))) {
                break;
            }
            else if (obj.value.indexOf('.') < 3) {
                obj.value = obj.value.replace('.', '');
            }
            else if ((obj.value.indexOf('.') == 3) || (obj.value.indexOf('.') == 4)) {
                var s = obj.value;
                obj.value = s.substring(0, (s.indexOf('.') + 1)) + s.substring((s.indexOf('.') + 1), s.length).replace('.', '');
            }
            else {
                obj.value = obj.value.replace('.', '');
            }
        }
    }
    catch (err) { }
}

function ConfirmRecover() {
    if (confirm("Bạn có muốn thu hồi không?") == true) {
        return true;
    }
    else {
        return false;
    }
}

function ConfirmPublish() {
    if (confirm("Bạn có chắc muốn thực hiện thao tác này không?") == true) {
        return true;
    }
    else {
        return false;
    }
}

function checkFileSize() {

    //check if file size is > 10 MB
    //alert(eval(eval(ctrl)+".value"));
    //	    if(GetSize(eval(eval(ctrl)+".value")) > 10485760)//10 MB=10485760 B
    //	    {
    //	        alert(document.getElementById("txtAnh").value);
    //	        //ctrl.form.reset();//file grater than 10MB then reset form
    //	        return false;  
    //	    }
    //	    else
    //	    {
    //	        return true;
    //	    }
}

//get file size using ActiveX 
//wrong turn
function GetSize(file) {
    var fso = new ActiveXObject("Scripting.FileSystemObject");
    var f = fso.getFile(file);
    return fso.getFile(file).size;
}

/*
   Hien thi tooltip cho cac tin khac
*/
if (typeof document.attachEvent != 'undefined') {
    window.attachEvent('onload', init);
    document.attachEvent('onmousemove', moveMouse);
    document.attachEvent('onclick', checkMove);
}
else {
    window.addEventListener('load', init, false);
    document.addEventListener('mousemove', moveMouse, false);
    document.addEventListener('click', checkMove, false);
}

var oDv = document.createElement("div");
var dvHdr = document.createElement("div");
var dvBdy = document.createElement("div");
var windowlock, boxMove, fixposx, fixposy, lockX, lockY, fixx, fixy, ox, oy, boxLeft, boxRight, boxTop, boxBottom, evt, mouseX, mouseY, boxOpen, totalScrollTop, totalScrollLeft;
boxOpen = false;
ox = 10;
oy = 10;
lockX = 0;
lockY = 0;

function init() {
    oDv.appendChild(dvHdr);
    oDv.appendChild(dvBdy);
    oDv.style.position = "absolute";
    oDv.style.visibility = 'hidden';
    document.body.appendChild(oDv);
}

function defHdrStyle() {
    dvHdr.innerHTML = '<img  style="vertical-align:middle"  src="info.gif">&nbsp;&nbsp;' + dvHdr.innerHTML;
    dvHdr.style.fontWeight = 'bold';
    dvHdr.style.width = '150px';
    dvHdr.style.fontFamily = 'arial';
    dvHdr.style.border = '1px solid #A5CFE9';
    dvHdr.style.padding = '3';
    dvHdr.style.fontSize = '11';
    dvHdr.style.color = '#4B7A98';
    dvHdr.style.background = '#D5EBF9';
    dvHdr.style.filter = 'alpha(opacity=85)'; // IE
    dvHdr.style.opacity = '0.85'; // FF
}

function defBdyStyle() {
    dvBdy.style.borderBottom = '1px solid #A5CFE9';
    dvBdy.style.borderLeft = '1px solid #A5CFE9';
    dvBdy.style.borderRight = '1px solid #A5CFE9';
    dvBdy.style.width = '150px';
    dvBdy.style.fontFamily = 'arial';
    dvBdy.style.fontSize = '11';
    dvBdy.style.padding = '3';
    dvBdy.style.color = '#1B4966';
    dvBdy.style.background = '#FFFFFF';
    dvBdy.style.filter = 'alpha(opacity=85)'; // IE
    dvBdy.style.opacity = '0.85'; // FF
}

function checkElemBO(txt) {
    if (!txt || typeof (txt) != 'string') return false;
    if ((txt.indexOf('header') > -1) && (txt.indexOf('body') > -1) && (txt.indexOf('[') > -1) && (txt.indexOf('[') > -1))
        return true;
    else
        return false;
}

function scanBO(curNode) {
    if (checkElemBO(curNode.title)) {
        curNode.boHDR = getParam('header', curNode.title);
        curNode.boBDY = getParam('body', curNode.title);
        curNode.boCSSBDY = getParam('cssbody', curNode.title);
        curNode.boCSSHDR = getParam('cssheader', curNode.title);
        curNode.IEbugfix = (getParam('hideselects', curNode.title) == 'on') ? true : false;
        curNode.fixX = parseInt(getParam('fixedrelx', curNode.title));
        curNode.fixY = parseInt(getParam('fixedrely', curNode.title));
        curNode.absX = parseInt(getParam('fixedabsx', curNode.title));
        curNode.absY = parseInt(getParam('fixedabsy', curNode.title));
        curNode.offY = (getParam('offsety', curNode.title) != '') ? parseInt(getParam('offsety', curNode.title)) : 10;
        curNode.offX = (getParam('offsetx', curNode.title) != '') ? parseInt(getParam('offsetx', curNode.title)) : 10;
        curNode.fade = (getParam('fade', curNode.title) == 'on') ? true : false;
        curNode.fadespeed = (getParam('fadespeed', curNode.title) != '') ? getParam('fadespeed', curNode.title) : 0.04;
        curNode.delay = (getParam('delay', curNode.title) != '') ? parseInt(getParam('delay', curNode.title)) : 0;
        if (getParam('requireclick', curNode.title) == 'on') {
            curNode.requireclick = true;
            document.all ? curNode.attachEvent('onclick', showHideBox) : curNode.addEventListener('click', showHideBox, false);
            document.all ? curNode.attachEvent('onmouseover', hideBox) : curNode.addEventListener('mouseover', hideBox, false);
        }
        else {// Note : if requireclick is on the stop clicks are ignored   			
            if (getParam('doubleclickstop', curNode.title) != 'off') {
                document.all ? curNode.attachEvent('ondblclick', pauseBox) : curNode.addEventListener('dblclick', pauseBox, false);
            }
            if (getParam('singleclickstop', curNode.title) == 'on') {
                document.all ? curNode.attachEvent('onclick', pauseBox) : curNode.addEventListener('click', pauseBox, false);
            }
        }
        curNode.windowLock = getParam('windowlock', curNode.title).toLowerCase() == 'off' ? false : true;
        curNode.title = '';
        curNode.hasbox = 1;
    }
    else
        curNode.hasbox = 2;
}


function getParam(param, list) {
    var reg = new RegExp('([^a-zA-Z]' + param + '|^' + param + ')\\s*=\\s*\\[\\s*(((\\[\\[)|(\\]\\])|([^\\]\\[]))*)\\s*\\]');
    var res = reg.exec(list);
    var returnvar;
    if (res)
        return res[2].replace('[[', '[').replace(']]', ']');
    else
        return '';
}

function Left(elem) {
    var x = 0;
    if (elem.calcLeft)
        return elem.calcLeft;
    var oElem = elem;
    while (elem) {
        if ((elem.currentStyle) && (!isNaN(parseInt(elem.currentStyle.borderLeftWidth))) && (x != 0))
            x += parseInt(elem.currentStyle.borderLeftWidth);
        x += elem.offsetLeft;
        elem = elem.offsetParent;
    }
    oElem.calcLeft = x;
    return x;
}

function Top(elem) {
    var x = 0;
    if (elem.calcTop)
        return elem.calcTop;
    var oElem = elem;
    while (elem) {
        if ((elem.currentStyle) && (!isNaN(parseInt(elem.currentStyle.borderTopWidth))) && (x != 0))
            x += parseInt(elem.currentStyle.borderTopWidth);
        x += elem.offsetTop;
        elem = elem.offsetParent;
    }
    oElem.calcTop = x;
    return x;

}

var ah, ab;
function applyStyles() {
    if (ab)
        oDv.removeChild(dvBdy);
    if (ah)
        oDv.removeChild(dvHdr);
    dvHdr = document.createElement("div");
    dvBdy = document.createElement("div");
    CBE.boCSSBDY ? dvBdy.className = CBE.boCSSBDY : defBdyStyle();
    CBE.boCSSHDR ? dvHdr.className = CBE.boCSSHDR : defHdrStyle();
    dvHdr.innerHTML = CBE.boHDR;
    dvBdy.innerHTML = CBE.boBDY;
    ah = false;
    ab = false;
    if (CBE.boHDR != '') {
        oDv.appendChild(dvHdr);
        ah = true;
    }
    if (CBE.boBDY != '') {
        oDv.appendChild(dvBdy);
        ab = true;
    }
}

var CSE, iterElem, LSE, CBE, LBE, totalScrollLeft, totalScrollTop, width, height;
var ini = false;

// Customised function for inner window dimension
function SHW() {
    if (document.body && (document.body.clientWidth != 0)) {
        width = document.body.clientWidth;
        height = document.body.clientHeight;
    }
    if (document.documentElement && (document.documentElement.clientWidth != 0) && (document.body.clientWidth + 20 >= document.documentElement.clientWidth)) {
        width = document.documentElement.clientWidth;
        height = document.documentElement.clientHeight;
    }
    return [width, height];
}


var ID = null;
function moveMouse(e) {
    //boxMove=true;
    e ? evt = e : evt = event;

    CSE = evt.target ? evt.target : evt.srcElement;

    if (!CSE.hasbox) {
        // Note we need to scan up DOM here, some elements like TR don't get triggered as srcElement
        iElem = CSE;
        while ((iElem.parentNode) && (!iElem.hasbox)) {
            scanBO(iElem);
            iElem = iElem.parentNode;
        }
    }

    if ((CSE != LSE) && (!isChild(CSE, dvHdr)) && (!isChild(CSE, dvBdy))) {
        if (!CSE.boxItem) {
            iterElem = CSE;
            while ((iterElem.hasbox == 2) && (iterElem.parentNode))
                iterElem = iterElem.parentNode;
            CSE.boxItem = iterElem;
        }
        iterElem = CSE.boxItem;
        if (CSE.boxItem && (CSE.boxItem.hasbox == 1)) {
            LBE = CBE;
            CBE = iterElem;
            if (CBE != LBE) {
                applyStyles();
                if (!CBE.requireclick)
                    if (CBE.fade) {
                        if (ID != null)
                            clearTimeout(ID);
                        ID = setTimeout("fadeIn(" + CBE.fadespeed + ")", CBE.delay);
                    }
                    else {
                        if (ID != null)
                            clearTimeout(ID);
                        COL = 1;
                        ID = setTimeout("oDv.style.visibility='visible';ID=null;", CBE.delay);
                    }
                if (CBE.IEbugfix) { hideSelects(); }
                fixposx = !isNaN(CBE.fixX) ? Left(CBE) + CBE.fixX : CBE.absX;
                fixposy = !isNaN(CBE.fixY) ? Top(CBE) + CBE.fixY : CBE.absY;
                lockX = 0;
                lockY = 0;
                boxMove = true;
                ox = CBE.offX ? CBE.offX : 10;
                oy = CBE.offY ? CBE.offY : 10;
            }
        }
        else if (!isChild(CSE, dvHdr) && !isChild(CSE, dvBdy) && (boxMove)) {
            // The conditional here fixes flickering between tables cells.
            if ((!isChild(CBE, CSE)) || (CSE.tagName != 'TABLE')) {
                CBE = null;
                if (ID != null)
                    clearTimeout(ID);
                fadeOut();
                showSelects();
            }
        }
        LSE = CSE;
    }
    else if (((isChild(CSE, dvHdr) || isChild(CSE, dvBdy)) && (boxMove))) {
        totalScrollLeft = 0;
        totalScrollTop = 0;

        iterElem = CSE;
        while (iterElem) {
            if (!isNaN(parseInt(iterElem.scrollTop)))
                totalScrollTop += parseInt(iterElem.scrollTop);
            if (!isNaN(parseInt(iterElem.scrollLeft)))
                totalScrollLeft += parseInt(iterElem.scrollLeft);
            iterElem = iterElem.parentNode;
        }
        if (CBE != null) {
            boxLeft = Left(CBE) - totalScrollLeft;
            boxRight = parseInt(Left(CBE) + CBE.offsetWidth) - totalScrollLeft;
            boxTop = Top(CBE) - totalScrollTop;
            boxBottom = parseInt(Top(CBE) + CBE.offsetHeight) - totalScrollTop;
            doCheck();
        }
    }

    if (boxMove && CBE) {
        // This added to alleviate bug in IE6 w.r.t DOCTYPE
        bodyScrollTop = document.documentElement && document.documentElement.scrollTop ? document.documentElement.scrollTop : document.body.scrollTop;
        bodyScrollLet = document.documentElement && document.documentElement.scrollLeft ? document.documentElement.scrollLeft : document.body.scrollLeft;
        mouseX = evt.pageX ? evt.pageX - bodyScrollLet : evt.clientX - document.body.clientLeft;
        mouseY = evt.pageY ? evt.pageY - bodyScrollTop : evt.clientY - document.body.clientTop;
        if ((CBE) && (CBE.windowLock)) {
            mouseY < -oy ? lockY = -mouseY - oy : lockY = 0;
            mouseX < -ox ? lockX = -mouseX - ox : lockX = 0;
            mouseY > (SHW()[1] - oDv.offsetHeight - oy) ? lockY = -mouseY + SHW()[1] - oDv.offsetHeight - oy : lockY = lockY;
            mouseX > (SHW()[0] - dvBdy.offsetWidth - ox) ? lockX = -mouseX - ox + SHW()[0] - dvBdy.offsetWidth : lockX = lockX;
        }
        oDv.style.left = ((fixposx) || (fixposx == 0)) ? fixposx : bodyScrollLet + mouseX + ox + lockX + "px";
        oDv.style.top = ((fixposy) || (fixposy == 0)) ? fixposy : bodyScrollTop + mouseY + oy + lockY + "px";

    }
}

function doCheck() {
    if ((mouseX < boxLeft) || (mouseX > boxRight) || (mouseY < boxTop) || (mouseY > boxBottom)) {
        if (!CBE.requireclick)
            fadeOut();
        if (CBE.IEbugfix) { showSelects(); }
        CBE = null;
    }
}

function pauseBox(e) {
    e ? evt = e : evt = event;
    boxMove = false;
    evt.cancelBubble = true;
}

function showHideBox(e) {
    oDv.style.visibility = (oDv.style.visibility != 'visible') ? 'visible' : 'hidden';
}

function hideBox(e) {
    oDv.style.visibility = 'hidden';
}

var COL = 0;
var stopfade = false;
function fadeIn(fs) {
    ID = null;
    COL = 0;
    oDv.style.visibility = 'visible';
    fadeIn2(fs);
}

function fadeIn2(fs) {
    COL = COL + fs;
    COL = (COL > 1) ? 1 : COL;
    oDv.style.filter = 'alpha(opacity=' + parseInt(100 * COL) + ')';
    oDv.style.opacity = COL;
    if (COL < 1)
        setTimeout("fadeIn2(" + fs + ")", 20);
}


function fadeOut() {
    oDv.style.visibility = 'hidden';

}

function isChild(s, d) {
    while (s) {
        if (s == d)
            return true;
        s = s.parentNode;
    }
    return false;
}

var cSrc;
function checkMove(e) {
    e ? evt = e : evt = event;
    cSrc = evt.target ? evt.target : evt.srcElement;
    if ((!boxMove) && (!isChild(cSrc, oDv))) {
        fadeOut();
        if (CBE && CBE.IEbugfix) { showSelects(); }
        boxMove = true;
        CBE = null;
    }
}

function showSelects() {
    var elements = document.getElementsByTagName("select");
    for (i = 0; i < elements.length; i++) {
        elements[i].style.visibility = 'visible';
    }
}

function hideSelects() {
    var elements = document.getElementsByTagName("select");
    for (i = 0; i < elements.length; i++) {
        elements[i].style.visibility = 'hidden';
    }
}

function FuncAcceptNumber_V(obj, e) {
    try {
        var key;
        var keychar;
        if (window.event) key = window.event.keyCode;
        else if (e) key = e.which;
        else return true;
        keychar = String.fromCharCode(key);
        if (key == 13) {
            return false;
        }
        if ((key == null) || (key == 0) || (key == 8) || (key == 9) || (key == 27)) return true;
        else if ((("0123456789").indexOf(keychar) > -1) && (obj.value.length > 0)) return true;
        else if ((keychar == '0') && (obj.value.length == 0)) return false;
    }
    catch (err) { }
}

function VNCurrencyFormat(pString, pAcceptOddNumber) {

    number = '';
    number_only = pString.replace(/[^0-9,]/g, '').match(/[0-9]+[,]?[0-9]*/);


    if (number_only != null) {

        isIncludeComma = number_only[0].indexOf(',') != -1 ? true : false;
        groups = number_only[0].split(',');

        if (groups != null) {

            for (i = groups[0].length - 1, j = 1; i >= 0; i--, j++) {
                number = (j % 3 == 0 && j < groups[0].length ? '.' : '') + groups[0].charAt(i) + number;
            }

            number = number + (pAcceptOddNumber && isIncludeComma ? ',' + (groups[1] != null ? groups[1] : '') : '');
        }
    }

    final_format = number.match(/[0-9.]+[,]?[0-9]{4}/);

    if (final_format != null) {

        number = final_format[0];
    }

    return number;
}
/*
    Kết thúc hiển thị tooltip
*/

function trim(text) {
    while ((text.indexOf(' ') != -1) || (text.indexOf('\r\n') != -1)) {
        if (text.indexOf(' ') != -1) {
            text = text.replace(' ', '')
        }
        else {
            text = text.replace('\r\n', '')
        }
    }

    return text
}

var boxRight = "<table style='position:absolute; left: -1;' border='0' cellpadding='0' cellspacing='0' width='200'><tbody><tr><td align='right' valign='top' style='border-width:0px;padding:8px 0px 0px 0px;' width='18'><div style='border-top: 1px solid black; font-size: 1px; position: relative; left: 1px; width: 18px;'><div style='border-left: 1px solid black; overflow: hidden; width: 17px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 16px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 15px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 14px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 13px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 12px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 11px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 10px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 9px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 8px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 7px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 6px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 5px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 4px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 3px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 2px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 1px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black; overflow: hidden; width: 0px; height: 1px; background-color: LemonChiffon;'></div></div></td><td style='border: 1px solid black; padding: 5px; background-color: LemonChiffon;'>{0}</td></tr></tbody></table>";
var boxLeft = "<table style='position:absolute; left: -199px;' border='0' cellpadding='0' cellspacing='0' width='200'><tbody><tr><td style='border: 1px solid black; padding: 5px; background-color: LemonChiffon;'>{0}</td><td style='border-width: 0px;padding: 8px 0px 0px 0px;' valign='top' align='left' width='18'><div style='border-top: 1px solid black; font-size: 1px; position: relative; left: -1px; width: 18px;'><div style='border-right: 1px solid black; overflow: hidden; width: 17px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 16px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 15px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 14px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 13px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 12px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 11px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 10px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 9px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 8px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 7px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 6px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 5px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 4px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 3px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 2px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 1px; height: 1px; background-color: LemonChiffon;'></div><div style='border-right: 1px solid black; overflow: hidden; width: 0px; height: 1px; background-color: LemonChiffon;'></div></div></td></tr></tbody></table>";
var boxBottom = "<table style='position:absolute; top: -1px;' border='0' cellpadding='0' cellspacing='0' width='200'><tbody><tr><td valign='bottom' style='border-width:0px; height:18px;padding:0px 0px 0px 8px;'><div style='font-size: 1px; width: 1px;height:18px; position: relative;top:1 px;'><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 0px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 1px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 2px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 3px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 4px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 5px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 6px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 7px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 8px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 9px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 10px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 11px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 12px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 13px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 14px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 15px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 16px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 17px; height: 1px; background-color: LemonChiffon;'></div><div style='border-left: 1px solid black;border-right: 1px solid black; overflow: hidden; width: 18px; height: 1px; background-color: LemonChiffon;'></div></div></td></tr><tr><td style='border: 1px solid black;  padding: 5px; background-color: LemonChiffon;' >{0}</td></tr></tbody></table>";

function RightMessage(obj, message) {
    try {
        if (obj.parentNode.lastChild.id != 'rdiv') {
            var rdiv = document.createElement('div');
            rdiv.style.position = "absolute";
            rdiv.style.zIndex = "5000";
            rdiv.style.display = "inline";
            rdiv.id = 'rdiv';
            rdiv.innerHTML = boxRight.replace("{0}", message);
            obj.parentNode.appendChild(rdiv);
            obj.onblur = obj.onmouseout = rdiv.onmousemove = function () { if (this.parentNode.lastChild.id == 'rdiv') { this.parentNode.lastChild.style.visibility = 'hidden'; }; }
        }
        else {
            obj.parentNode.lastChild.innerHTML = boxRight.replace("{0}", message);
            obj.parentNode.lastChild.style.visibility = "visible";
        }
    }
    catch (err) { }
}

function LeftMessage(obj, message, width) {
    try {
        if (obj.parentNode.firstChild.id != 'ldiv') {
            var ldiv = obj.parentNode.insertBefore(document.createElement('div'), obj.parentNode.firstChild);
            ldiv.style.position = "absolute";
            ldiv.style.zIndex = "5000";
            ldiv.style.display = "inline";
            ldiv.id = 'ldiv';
            ldiv.innerHTML = boxLeft.replace("{0}", message);
            obj.onblur = obj.onmouseout = ldiv.onmousemove = function () { if (this.parentNode.firstChild.id == 'ldiv') { this.parentNode.firstChild.style.visibility = 'hidden'; }; }
        }
        else {
            obj.parentNode.firstChild.innerHTML = boxLeft.replace("{0}", message);
            obj.parentNode.firstChild.style.visibility = "visible";
        }
    }
    catch (err) { }
}

function BottomMessage(obj, message) {
    try {
        if (obj.parentNode.lastChild.id != 'bdiv') {
            var bdiv = document.createElement('div');
            bdiv.style.position = "relative";
            bdiv.style.zIndex = "5000";
            bdiv.id = 'bdiv';
            bdiv.innerHTML = boxBottom.replace("{0}", message);
            obj.parentNode.appendChild(bdiv);
            obj.onblur = obj.onmouseout = bdiv.onmousemove = function () { if (this.parentNode.lastChild.id == 'bdiv') { this.parentNode.lastChild.style.visibility = 'hidden'; }; }
        }
        else {
            obj.parentNode.lastChild.innerHTML = boxBottom.replace("{0}", message);
            obj.parentNode.lastChild.style.visibility = "visible";
        }
    }
    catch (err) { }
}

function MessageUpload(objThem, objVanBan, objTapTin, messageVanBan, messageTapTin) {
    try {
        if ((objVanBan.lastChild.id != 'div') && (objTapTin.lastChild.id != 'div')) {
            var divVanBan = document.createElement('div');
            var divTapTin = document.createElement('div');
            divVanBan.style.position = divTapTin.style.position = "absolute";
            divVanBan.style.zIndex = divTapTin.style.zIndex = "5000";
            divVanBan.style.display = divTapTin.style.display = "inline";
            divVanBan.id = divTapTin.id = 'div';
            divVanBan.style.visibility = divTapTin.style.visibility = "hidden";
            divVanBan.innerHTML = boxRight.replace("{0}", messageVanBan);
            divTapTin.innerHTML = boxRight.replace("{0}", messageTapTin);
            objVanBan.appendChild(divVanBan);
            objTapTin.appendChild(divTapTin);
            objThem.onblur = objThem.onmouseout = function () {
                if (document.getElementById(objVanBan.id)) {
                    if (document.getElementById(objVanBan.id).lastChild.id == 'div') {
                        document.getElementById(objVanBan.id).lastChild.style.visibility = "hidden";
                    }
                };
                if (document.getElementById(objTapTin.id)) {
                    if (document.getElementById(objTapTin.id).lastChild.id == 'div') {
                        document.getElementById(objTapTin.id).lastChild.style.visibility = "hidden";
                    }
                };
            }
            divVanBan.onmousemove = divTapTin.onmousemove = function () { if (this.parentNode.lastChild.id == 'div') { this.parentNode.lastChild.style.visibility = "hidden"; }; }
        }

        if (messageVanBan != "") {
            objVanBan.lastChild.innerHTML = boxRight.replace("{0}", messageVanBan);
            objVanBan.lastChild.style.visibility = "visible";
        }
        if (messageTapTin != "") {
            objTapTin.lastChild.innerHTML = boxRight.replace("{0}", messageTapTin);
            objTapTin.lastChild.style.visibility = "visible";
        }
    }
    catch (err) { }
}

function ReplaceSymbol(text) {
    if (!text)
        return '';

    text = text.replace(/&/g, '&amp;');
    text = text.replace(/</g, '&lt;');
    text = text.replace(/>/g, '&gt;');

    return text;
}

function OpenWindow(url) {
    var win = window.open(url, 'new', 'toolbar=0,scrollbars=1,location=0,statusbar=0,menubar=1,resizable=0,top=0,left=0,width=' + screen.width + ',height=' + screen.height); return false;
    win.focus();
    return false;
}
function LoadToast() {
    toastr.options = {
        "closeButton": true,
        "debug": true,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "10000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
}
function Toastr_Validation() {

    LoadToast();
    var v_Validations = $("html").find(".js_toastr_validation");
    for (var v = 0; v < v_Validations.length; v++) {
        var v_message_all = $(v_Validations[v]).find("ul li");
        if (v_message_all.length > 0) {
            toastr.clear();
        }
        for (var i = v_message_all.length - 1; i >= 0 ; i--) {
            var lstLoi = '';
            lstLoi += ($(v_message_all[i]).html());
            toastr["error"](lstLoi);
        }
    }
}
$(document).ready(function () {
    $(".paping").each(function () {
        var $this = $(this);
        $this.html($this.html().replace(/&nbsp;/g, ''));
    });
});

/**
 * Giữ focus của control sau khi auto postback
 */
(function () {
    var focusElement;
    function restoreFocus() {
        if (focusElement) {
            if (focusElement.id) {
                $('#' + focusElement.id).focus();
            } else {
                $(focusElement).focus();
            }
        }
    }

    $(document).ready(function () {
        $(document).on('focusin', function (objectData) {
            focusElement = objectData.currentTarget.activeElement;
        });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(restoreFocus);
    });
})();

/*Xử lý checkRow và checkAll*/
var count = 0;
function confirm_delete_rows(id) {
    if (count > 0) {
        if (confirm("Bạn có muốn xóa không?")) {
            count = 0;
            document.getElementById(id).click();
            document.getElementById('divShowBtnXoa').style.display = "none";
        }
    }

}
function confirm_delete_rows_update(id) {
    document.getElementById(id).click();
    document.getElementById('divShowBtnXoa').style.display = "none";
}

function handle_checked_delete_row(obj, id) {
    count = 0;
    //Get the Row based on checkbox
    var row = obj.parentNode.parentNode;

    //Get the reference of GridView
    var GridView = row.parentNode;
    //Get all input elements in Gridview
    var inputList = GridView.getElementsByTagName("input");
    var checked = true;
    for (var i = 0; i < inputList.length; i++) {
        //The First element is the Header Checkbox
        var headerCheckBox = inputList[0];
        //Based on all or none checkboxes are checked check/uncheck Header Checkbox

        if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
            if (!inputList[i].checked) {
                checked = false;
            }
            else {
                count++;
            }
        }
    }

    headerCheckBox.checked = checked;
    if (count == 0) {
        document.getElementById("divShowBtnXoa").style.display = "none";
        //$("#divShowBtnXoa")[0].style.display = "none";
    }
    else {
        //$("#divShowBtnXoa")[0].style.display = "inline-block";
        document.getElementById("divShowBtnXoa").style.display = "inline-block";
    }
}

function handle_checked_delete_all_rows(obj, id) {
    count = 0;
    var GridView = obj.parentNode.parentNode.parentNode;
    var inputList = GridView.getElementsByTagName("input");

    for (var i = 0; i < inputList.length; i++) {
        //Get the Cell To find out ColumnIndex
        var row = inputList[i].parentNode.parentNode;
        if (inputList[i].type == "checkbox" && obj != inputList[i]) {
            if (obj.checked) {
                //If the header checkbox is checked check all checkboxes
                //and highlight all rows
                count++;
                inputList[i].checked = true;
            }
            else {
                inputList[i].checked = false;
            }
        }
    }
    if (count == 0) {
        document.getElementById("divShowBtnXoa").style.display = "none";
    }
    else {
        document.getElementById("divShowBtnXoa").style.display = "inline-block";
    }
}


function handle_checked_one_row(obj, id) {
    count = 0;
    //Get the Row based on checkbox
    var row = obj.parentNode.parentNode;

    //Get the reference of GridView
    var GridView = row.parentNode;
    //Get all input elements in Gridview
    var inputList = GridView.getElementsByTagName("input");
    var checked = obj.checked;
    for (var i = 0; i < inputList.length; i++) {
        //The First element is the Header Checkbox
        var headerCheckBox = inputList[0];
        //Based on all or none checkboxes are checked check/uncheck Header Checkbox

        if (inputList[i].type == "checkbox" ) {
            //if (!inputList[i].checked) {
            //    checked = false;
            //}
            //else {
            //    count++;
            //}
            inputList[i].checked = false;
        }
    }

    if (checked) {
        obj.checked = true;
    }
    else {
        obj.checked = false;
        
    }
    
    if (!obj.checked) {
        document.getElementById("divShowBtnXoa").style.display = "none";
        //$("#divShowBtnXoa")[0].style.display = "none";
    }
    else {
        //$("#divShowBtnXoa")[0].style.display = "inline-block";
        document.getElementById("divShowBtnXoa").style.display = "inline-block";
    }
}
/*Kết thúc checkRow và checkAll*/