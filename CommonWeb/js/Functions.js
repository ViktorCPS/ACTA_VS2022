function calendarPicker(strField, doPostBack) {
    try {
        var wOptions;

        //    wOptions = 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=no';
        //    // window should be in maximized mode
        wOptions = wOptions + ',width=250';
        wOptions = wOptions + ',height=195';
        wOptions = wOptions + ',left=650,top=390';
       	window.open('/ACTAWeb/CommonWeb/Pickers/DatePicker.aspx?field=' + strField,'calendarPopup',wOptions);
//        wOptions = 'dialogWidth: 250px';
//        wOptions = wOptions + '; dialogHeight: 195px';
//        wOptions = wOptions + '; dialogLeft: 650px' + '; dialogTop: 390px; edge: Raised; center: Yes; resizable: Yes; status: Yes;';
//        window.open('/ACTAWeb/CommonWeb/Pickers/DatePicker.aspx?field=' + strField + '&doPostBack=' + doPostBack, window, wOptions);
        return false;
    }
    catch (e) { alert(e); }
}

function ADAMTest() {
    try {
        var wOptions;
        var width = 700;
        var height = 300;
        var left = (screen.availWidth - width) / 2;
        var top = (screen.availHeight - height) / 2;
        if (left < 0) {
            left = 0;
        }
        if (top < 0) {
            top = 0;
        }
        //    wOptions = 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=yes';
        //    // window should be in maximized mode
        wOptions = wOptions + ',width=' + width;
        wOptions = wOptions + ',height=' + height;
        wOptions = wOptions + ',left=' + left + ',top=' + top;
        window.open('/ACTAWeb/CommonWeb/Pickers/WUnitsPicker.aspx?postBackCtrl=' + btnTree + '&isTmp=' + isTmp,'wUnitsPopup',wOptions);
//        wOptions = 'dialogWidth: ' + width + 'px';
//        wOptions = wOptions + '; dialogHeight: ' + height + 'px';
//        wOptions = wOptions + '; dialogLeft: ' + left + 'px' + '; dialogTop: ' + top + 'px; edge: Raised; center: Yes; resizable: Yes; status: Yes;';
//        window.open('/ACTAWeb/ACTAWebUI/ADAMLogin.aspx', window, wOptions);
        //window.location.reload(true);
        return false;
    }
    catch (e) { alert(e); }
}

function mouseDown(e) {
    try { if (event.button == 2 || event.button == 3) { return false; } }
    catch (e) { if (e.which == 3) { return false; } }
}
document.oncontextmenu = function() { return false; }
document.onmousedown = mouseDown;

function CheckPDF() {
    var isInstalled = false;
    var version = null;
    if (window.ActiveXObject) {
        var control = null;
        try {
            // version 7
            control = new ActiveXObject('AcroPDF.PDF');
        }
        catch (e) {
            // Do nothing

        }
        if (!control) {
            try {
                //version 6
                control = new ActiveXObject('PDF.PdfCtrl');
            }
            catch (e) {

            }
        }
        if (!control) {
            try {
                //version 6
                control = new ActiveXObject('AcroExch.Document');
            }
            catch (e) {

            }
        }
        if (!control) {
            try {
                //version 6
                control = new ActiveXObject('FoxitReader.FoxitReaderCtl.1');
            }
            catch (e) {
            }
        }
        if (!control) {
            try {
                //version 6
                control = new ActiveXObject('FOXITREADEROCX.FoxitReaderOCXCtrl.1');
            }
            catch (e) {

            }
        }
        if (control) {

            document.getElementById('test').value = '';


        }
        else {

            document.getElementById('test').value = 'no';
        }
    }
}

function wUnitsPicker(btnTree, isTmp) {
    try {
        var wOptions;
        var width = 550;
        var height = 500;
        var left = (screen.availWidth - width) / 2;
        var top = (screen.availHeight - height) / 2;
        if (left < 0) {
            left = 0;
        }
        if (top < 0) {
            top = 0;
        }
            wOptions = 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=yes';
            // window should be in maximized mode
            wOptions = wOptions + ',width=' + width;
            wOptions = wOptions + ',height=' + height;
            wOptions = wOptions + ',left=' + left + ',top=' + top;
        	window.open('/ACTAWeb/CommonWeb/Pickers/WUnitsPicker.aspx?postBackCtrl=' + btnTree + '&isTmp=' + isTmp,'wUnitsPopup',wOptions);
////        wOptions = 'dialogWidth: ' + width + 'px';
////        wOptions = wOptions + '; dialogHeight: ' + height + 'px';
////        wOptions = wOptions + '; dialogLeft: ' + left + 'px' + '; dialogTop: ' + top + 'px; edge: Raised; center: Yes; resizable: Yes; status: Yes;';
////        window.open('/ACTAWeb/CommonWeb/Pickers/WUnitsPicker.aspx?postBackCtrl=' + btnTree + '&isTmp=' + isTmp, window, wOptions);
//        window.location.reload(true);
        return false;
    }
    catch (e) { alert(e); }
}

function oUnitsPicker(btnTree, isTmp) {
    try {
        var wOptions;
        var width = 550;
        var height = 500;
        var left = (screen.availWidth - width) / 2;
        var top = (screen.availHeight - height) / 2;
        if (left < 0) {
            left = 0;
        }
        if (top < 0) {
            top = 0;
        }
        //    wOptions = 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=yes';
        //    // window should be in maximized mode
        wOptions = wOptions + ',width=' + width;
        wOptions = wOptions + ',height=' + height;
        wOptions = wOptions + ',left=' + left + ',top=' + top;
        window.open('/ACTAWeb/CommonWeb/Pickers/OrgUnitsPicker.aspx?postBackCtrl=' + btnTree + '&isTmp=' + isTmp,'oUnitsPopup',wOptions);
//        wOptions = 'dialogWidth: ' + width + 'px';
//        wOptions = wOptions + '; dialogHeight: ' + height + 'px';
//        wOptions = wOptions + '; dialogLeft: ' + left + 'px' + '; dialogTop: ' + top + 'px; edge: Raised; center: Yes; resizable: Yes; status: Yes;';
//        window.open('/ACTAWeb/CommonWeb/Pickers/OrgUnitsPicker.aspx?postBackCtrl=' + btnTree + '&isTmp=' + isTmp, window, wOptions);
        //window.location.reload(true);
        return false;
        
    }
    catch (e) { alert(e); }
}

function pairBarChange(date, emplTypeID, company, emplID, ctrl, confirm, confirmType, verify, verifyType, pairRecID) {
    try {
        var wOptions;
        var width = 850;
        var height = 620;
        var left = (screen.availWidth - width) / 2;
        var top = (screen.availHeight - height) / 2;
        if (left < 0) {
            left = 0;
        }
        if (top < 0) {
            top = 0;
        }
        // wOptions = 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=yes';
        // window should be in maximized mode
//        wOptions = 'dialogWidth: ' + width + 'px';
//        wOptions = wOptions + '; dialogHeight: ' + height + 'px';
//        wOptions = wOptions + '; dialogLeft: ' + left + 'px' + '; dialogTop: ' + top + 'px; edge: Raised; center: Yes; resizable: Yes; status: Yes;';
        ////window.open('/ACTAWeb/ACTAWebUI/IOPairBarChange.aspx?date=' + date + '&emplTypeID=' + emplTypeID + '&company=' + company + '&emplID='
            ////+ emplID + '&postBackID=' + ctrl + '&confirm=' + confirm + '&confirmType=' + confirmType + '&verify=' + verify + '&verifyType=' + verifyType + '&pairRecID=' + pairRecID, window, wOptions);
        
        wOptions = wOptions + ',width=' + width;
        wOptions = wOptions + ',height=' + height;
        wOptions = wOptions + ',left=' + left + ',top=' + top;
        window.open('/ACTAWeb/ACTAWebUI/IOPairBarChange.aspx?date=' + date + '&emplTypeID=' + emplTypeID + '&company=' + company + '&emplID='
            + emplID + '&postBackID=' + ctrl + '&confirm=' + confirm + '&confirmType=' + confirmType + '&verify=' + verify + '&verifyType=' + verifyType + '&pairRecID=' + pairRecID,window,wOptions);
        //window.location.reload(true);
        return false;
    }
    catch (e) { alert(e); }
}

function pairBarPreview(date, emplTypeID, company, emplID) {
    try {
        var wOptions;
        var width = 850;
        var height = 620;
        var left = (screen.availWidth - width) / 2;
        var top = (screen.availHeight - height) / 2;
        if (left < 0) {
            left = 0;
        }
        if (top < 0) {
            top = 0;
        }
        //    wOptions = 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=yes';
        //    // window should be in maximized mode
        wOptions = wOptions + ',width=' + width;
        wOptions = wOptions + ',height=' + height;
        wOptions = wOptions + ',left=' + left + ',top=' + top;
        window.open('/ACTAWeb/ACTAWebUI/IOPairBarPreview.aspx?date=' + date + '&emplTypeID=' + emplTypeID + '&company=' + company + '&emplID=' + emplID,'pairPreviewPopup',wOptions);
//        wOptions = 'dialogWidth: ' + width + 'px';
//        wOptions = wOptions + '; dialogHeight: ' + height + 'px';
//        wOptions = wOptions + '; dialogLeft: ' + left + 'px' + '; dialogTop: ' + top + 'px; edge: Raised; center: Yes; resizable: Yes; status: Yes;';
//        window.open('/ACTAWeb/ACTAWebUI/IOPairBarPreview.aspx?date=' + date + '&emplTypeID=' + emplTypeID + '&company=' + company + '&emplID=' + emplID, window, wOptions);
        return false;
    }
    catch (e) { alert(e); }
}

function visitHistPreview(visitID) {
    try {
        var wOptions;
        var width = 1050;
        var height = 650;
        var left = (screen.availWidth - width) / 2;
        var top = (screen.availHeight - height) / 2;
        if (left < 0) {
            left = 0;
        }
        if (top < 0) {
            top = 0;
        }
        // wOptions = 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=yes';
        // window should be in maximized mode
//        wOptions = 'dialogWidth: ' + width + 'px';
//        wOptions = wOptions + '; dialogHeight: ' + height + 'px';
//        wOptions = wOptions + '; dialogLeft: ' + left + 'px' + '; dialogTop: ' + top + 'px; edge: Raised; center: Yes; resizable: Yes; status: Yes;';
//        window.open('/ACTAWeb/ACTAWebUI/MCVisitHistoryPage.aspx?visitID=' + visitID, window, wOptions);

        wOptions = wOptions + ',width=' + width;
        wOptions = wOptions + ',height=' + height;
        wOptions = wOptions + ',left=' + left + ',top=' + top;
        window.open('/ACTAWeb/ACTAWebUI/MCVisitHistoryPage.aspx?visitID=' + visitID, window, wOptions);
        return false;
    }
    catch (e) { alert(e); }
}

function emplSchedulesPreview(emplIDs) {
    try {
        var wOptions;
        var width = 1050;
        var height = 650;
        var left = (screen.availWidth - width) / 2;
        var top = (screen.availHeight - height) / 2;
        if (left < 0) {
            left = 0;
        }
        if (top < 0) {
            top = 0;
        }
        // wOptions = 'toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,copyhistory=no,resizable=yes';
        // window should be in maximized mode
//        wOptions = 'dialogWidth: ' + width + 'px';
//        wOptions = wOptions + '; dialogHeight: ' + height + 'px';
//        wOptions = wOptions + '; dialogLeft: ' + left + 'px' + '; dialogTop: ' + top + 'px; edge: Raised; center: Yes; resizable: Yes; status: Yes;';
//        window.open('/ACTAWeb/ACTAWebUI/EmployeeSchedulesPage.aspx?emplID=' + emplIDs, window, wOptions);


        wOptions = wOptions + ',width=' + width;
        wOptions = wOptions + ',height=' + height;
        wOptions = wOptions + ',left=' + left + ',top=' + top;
        window.open('/ACTAWeb/ACTAWebUI/EmployeeSchedulesPage.aspx?emplID=' + emplIDs, window, wOptions);
        return false;
    }
    catch (e) { alert(e); }
}

function legendWindow(company) {
    try {
        var wOptions;
        var width = 850;
        var height = 600;
        var left = (screen.availWidth - width) / 2;
        var top = (screen.availHeight - height) / 2;
        if (left < 0) {
            left = 0;
        }
        if (top < 0) {
            top = 0;
        }
//        wOptions = 'dialogWidth: ' + width + 'px';
//        wOptions = wOptions + '; dialogHeight: ' + height + 'px';
//        wOptions = wOptions + '; dialogLeft: ' + left + 'px' + '; dialogTop: ' + top + 'px; edge: Raised; center: Yes; resizable: Yes; status: Yes;';
//        window.showModalDialog('/ACTAWeb/CommonWeb/Pickers/Legend.aspx?company=' + company, window, wOptions);


        wOptions = wOptions + ',width=' + width;
        wOptions = wOptions + ',height=' + height;
        wOptions = wOptions + ',left=' + left + ',top=' + top;
        window.open('/ACTAWeb/CommonWeb/Pickers/Legend.aspx?company=' + company, window, wOptions);
        return false;
    }
    catch (e) { alert(e); }
}

function closeWindow() {
    try {
        window.close();
        return false;
    }
    catch (e) { alert(e); }
}

function getSelectedKeys(selBox, message, frameID) {
    try {
        //alert('getting selection');
        var myIframe = document.getElementById(frameID); // get iframe
        if (myIframe != null) {
            var iframeDocument = myIframe.contentWindow ? myIframe.contentWindow.document : myIframe.contentDocument; // get iframe page document

            if (iframeDocument != null) {
                var selectedKeysElement = iframeDocument.getElementById('selectedKeys');
                if (selectedKeysElement != null) {
                    var selValue = selectedKeysElement.value; // get added values
                    var selBoxElement = document.getElementById(selBox);
                    if (selBoxElement != null) {
                        if (selValue == null || selValue == '') {
                            alert(message);
                            // put -1 in selected values as sign that something was tryed with selected values but there was no selected values so, same page should be reloaded                        
                            selBoxElement.value = -1;
                        }
                        else {
                            selBoxElement.value = selValue;
                        }
                    }
                }
            }
        }
    }
    catch (e) { alert(e); }
}

function getDelSelectedKeys(selBox, message, frameID) {
    try {
        if (confirm('Are you sure you want to delete?')) {
            getSelectedKeys(selBox, message, frameID);
        }
    }
    catch (e) { alert(e); }
}

// if is in use, check foreach variable if it is null
function getAllKeys(selBox, message, frameID) {
    try {
        var myIframe = document.getElementById(frameID); // get iframe
        var iframeDocument = myIframe.contentWindow ? myIframe.contentWindow.document : myIframe.contentDocument; // get iframe page document
        var resGrid = iframeDocument.getElementById('resultGrid'); // get result data grid

        if (resGrid == null || resGrid.rows.length == 0) {
            alert(message);
            // put -1 in selected values as sign that something was tryed with data grid records but there was no data grid records so, same page should be reloaded
            document.getElementById(selBox).value = -1;
        }
        else {
            // get all check boxes and their values
            var inputItemsArray = resGrid.getElementsByTagName('input');

            for (var i = 0; i < inputItemsArray.length; i++) {
                var inputItem = inputItemsArray[i];

                if (inputItem.type == 'checkbox') {
                    document.getElementById(selBox).value += inputItem.value + '|';
                }
            }

            //alert("for report: " + document.getElementById(selBox).value);
        }
    }
    catch (e) { alert(e); }
}

// if is in use, check foreach variable if it is null
function getAllGridValues(selBox, message, frameID) {
    try {
        var myIframe = document.getElementById(frameID); // get iframe
        var iframeDocument = myIframe.contentWindow ? myIframe.contentWindow.document : myIframe.contentDocument; // get iframe page document
        var resGrid = iframeDocument.getElementById('resultGrid'); // get result data grid

        if (resGrid == null || resGrid.rows.length == 0) {
            alert(message);
            // put -1 in selected values as sign that something was tryed with data grid records but there was no data grid records so, same page should be reloaded
            document.getElementById(selBox).value = -1;
        }
        else {
            // get all cell values for each data grid record
            for (var i = 0; i < resGrid.rows.length; i++) {
                // skip first cell, it is check box
                for (var j = 1; j < resGrid.rows[i].cells.length; j++) {
                    document.getElementById(selBox).value += resGrid.rows[i].cells[j].innerText + '|';
                }
                // put delimiter for row end
                document.getElementById(selBox).value += '~';
            }

            //alert("for report: " + document.getElementById(selBox).value);
        }
    }
    catch (e) { alert(e); }
}

function selectListItems(chbID, listID) {
    try {
        var chk = document.getElementById(chbID);
        if (chk != null) {
            var xState = chk.checked;

            var list = document.getElementById(listID);
            if (list != null) {
                for (var i = 0; i < list.options.length; i++) {
                    list.options[i].selected = xState;
                }
            }
        }
    }
    catch (e) { alert(e); }
}

function selectItem(tbID, listID) {
    try {
        var tb = document.getElementById(tbID);
        if (tb != null) {
            if (tb.value != null) {
                var list = document.getElementById(listID);
                if (list != null) {
                    var found = false;
                    var isValue = false;
                    if (!isNaN(tb.value.substring(0, 1))) {
                        isValue = true;
                    }
                    for (var i = 0; i < list.options.length; i++) {
                        if (!isValue) {
                            if (!found && tb.value != '' && list.options[i].text.toUpperCase().indexOf(tb.value.toUpperCase()) == 0) {
                                list.options[i].selected = true;
                                found = true;
                            }
                            else {
                                list.options[i].selected = false;
                            }
                        }
                        else {
                            if (!found && tb.value != '' && list.options[i].value.toUpperCase().indexOf(tb.value.toUpperCase()) == 0) {
                                list.options[i].selected = true;
                                found = true;
                            }
                            else {
                                list.options[i].selected = false;
                            }
                        }
                    }
                }
            }
        }
    }
    catch (e) { alert(e); }
0}

function check(rbChkID, rbID) {
    try {
        var rb = document.getElementById(rbID);
        var rbChk = document.getElementById(rbChkID);

        if (rb != null && rbChk != null) {
            rb.checked = !rbChk.checked;
        }
    }
    catch (e) { alert(e); }
}

function checkRB(rbChkID, rb1ID, rb2ID) {
    try {
        var rb1 = document.getElementById(rb1ID);
        var rb2 = document.getElementById(rb2ID);
        var rbChk = document.getElementById(rbChkID);

        if (rb1 != null && rb2 != null && rbChk != null) {
            rb1.checked = rb2.checked = !rbChk.checked;
        }
    }
    catch (e) { alert(e); }
}
function test() {
    try {

        ajaxRequest = new XMLHttpRequest();

    } catch (e) {

        try {

            ajaxRequest = new ActiveXObject("Msxml2.XMLHTTP");
        } catch (e) {
            try {
                ajaxRequest = new ActiveXObject("Microsoft.XMLHTTP");
            } catch (e) {
                alert(brokeBrowser);
                return false;
            }
        }
    }


    ajaxRequest.onreadystatechange = function() {

    if (ajaxRequest.readyState == 4 && ajaxRequest.status == 200) {
            var lang = document.getElementById('h1').value;
            alert(lang);

            window.location = "TLDetailedDataPage.aspx";
        }
    }
 
    ajaxRequest.open("POST", "TestReport.aspx", true);
    ajaxRequest.send(null);
}


