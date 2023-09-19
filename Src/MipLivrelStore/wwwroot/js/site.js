$(document).ready(function () {
    
    // initialization of unfold component
    $.HSCore.components.HSUnfold.init($('[data-unfold-target]'));

    // initialization of malihu scrollbar
    $.HSCore.components.HSMalihuScrollBar.init($('.js-scrollbar'));

    // initialization of slick carousel
    $.HSCore.components.HSSlickCarousel.init('.js-slick-carousel');

    // initialization of show animations
    $.HSCore.components.HSShowAnimation.init('.js-animation-link');

    // init zeynepjs
    var zeynep = $('.zeynep').zeynep({
        onClosed: function () {
            // enable main wrapper element clicks on any its children element
            $("body main").attr("style", "");

            console.log('the side menu is closed.');
        },
        onOpened: function () {
            // disable main wrapper element clicks on any its children element
            $("body main").attr("style", "pointer-events: none;");

            console.log('the side menu is opened.');
        }
    });
   
    // handle zeynep overlay click
    $(".zeynep-overlay").click(function () {
        zeynep.close();
    });

    // open side menu if the button is clicked
    $(".cat-menu").click(function () {
        if ($("html").hasClass("zeynep-opened")) {
            zeynep.close();
        } else {
            zeynep.open();
        }
    });

    $('.selectpicker-language ').selectpicker();

    //// Catch all ajax queries ////

    $.ajaxSetup({
        beforeSend: function () {
            $('.loader').addClass('show');
        },
        complete: function () {
            setTimeout(() => { $('.loader').removeClass('show'); }, 2000);
        }
    });

    $("#searchTextId").on("input", function () {
        ////debugger
        if (document.getElementById("searchTextId").value.length == 0) {
            $('#MainSearchForm button[type="submit"]').prop('disabled', true);
        }
        else {
            $('#MainSearchForm button[type="submit"]').prop('disabled', false);
        }
    });

    //Search Engine
    $('#searchTextId').keyup(function () {
        if ($(this).val().length == 0) {
            $('#MainSearchForm button[type="submit"]').prop('disabled', true);
        }
        else {
            $('#MainSearchForm button[type="submit"]').prop('disabled', false);
        }
        if ($("#MainSearchForm select[name='SearchType']").is(":hidden")) {
            $('#SearchTypeMobile').show();
        }
        else {
            $('#SearchTypeMobile').hide();
        }
    });

    var _SearchType = 1;

    $('#SearchTypeMobile div ul li a').click(function () {
        _SearchType = $(this).attr("data-value");
        $(this).attr('isClicked', true);
        $(this).removeClass("bg-light");
        $(this).css('background-color', "#471069");
        $(this).css('color', "white");
        //pour les autre buttons ont les désactive
        $('#SearchTypeMobile div ul li').each(function () {
            if (_SearchType != $(this).children("a").attr("data-value")) {
                $(this).children("a").attr('isClicked', false);
                $(this).children("a").css('background-color', "");
                $(this).children("a").css('color', "#471069");
                $(this).children("a").addClass("bg-light");
            }
        });
    });

    $('#MainSearchForm').submit(function (e) {
        e.preventDefault();
        var form = $(this);
        let isMobile = window.matchMedia("only screen and (max-width: 768px)").matches;
        if (!isMobile)
            _SearchType = form.find("select[name='SearchType']").val();

        $.ajax({
            url: form.attr('action'),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                "SearchType": _SearchType,
                "KeyWord": form.find("input[name='KeyWord']").val(),
                "currentPageIndex": 0 /*form.find("input[name='currentPageIndex']").val()*/
            }),
            success: function (response) {
                //$('#MainContent').hide();
                //$('#MainContent').html(response).css('padding-top', '150px');
                $("#MainContent").empty();
                $('#MainContent').append(response);
                //$("body").append(response);
                //$("#MainContent").append(response);
            },
            error: function (jqXhr, textStatus, errorThrown) { console.log(errorThrown); }
        });
    });
    $(".Checkout_Post").click(function (e) {
        alert("click")
        $.ajax({
            method: "Get",
            url: "/MyAccount/MPGSCreateSession",
            success: function (response) {
                //alert(response)
                //alert(response.data)
                //alert(JSON.parse(response))
                //alert(JSON.parse(response)["merchant"])
                //var resp = JSON.parse(response);
                var responseobj = JSON.parse(response["result"]);
                Checkout.configure({
                    session: {
                        id: responseobj["session"]["id"]
                    }
                })
                Checkout.showPaymentPage();
            },
            error: function (jqXhr, textStatus, errorThrown) {
                $(".md-toast-error").fadeTo(2000, 500).slideUp(500, function () {
                    $(".md-toast-error").slideUp(500);
                });
            }
        });
    });
    $('#passwordToggleLogin').keypress(function (e) {
        if (e.which == 13) {
            $('#loginBtn').click();
            return false;
        }
    });

    $('#showPassword').click(function (e) {
        var x = document.getElementsByClassName('passwordToggle')[0];

        if (x.type === "password") {
            x.type = "text";
        } else {
            x.type = "password";
        }
    });

    $('#showPassword2').click(function (e) {
        $('.passwordToggle').map(function () {
            var x = $(this)[0];
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        });
    });

    $('#showPasswordMobile').click(function (e) {
        $('.passwordToggleMobile').map(function () {
            var x = $(this)[0];
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        });
    });

    $('#showPassword3').click(function (e) {
        $('.passwordToggle3').map(function () {
            var x = $(this)[0];
            if (x.type === "password") {
                x.type = "text";
            } else {
                x.type = "password";
            }
        });
    });

    $('#loginBtn').click(function () {
        var $summaryUl = $("#validationSummaryLogin").find("ul");
        $summaryUl.empty();
        $("#emptyFieldsMsg").empty();
        $("#emptyFieldsMsg").hide();
        var inputs = $("#formLogin").find('.form-control');
        var NbrEmptyFields = 0;
        for (let field of inputs) {
            let elt = $(field)[0];
            $(elt).css('border-color', '');
            if ($(elt).val() == '') {
                $(elt).css('border-color', 'red');
                NbrEmptyFields++;
            }
        }
        ////debugger
        if (NbrEmptyFields > 0) {

            $summaryUl.append("<li> " + $('#EmptyFields').val() + "</li>");
            $summaryUl.show();
        }
        else {

            $summaryUl.empty();
            $('#formLogin').submit();
        }
    });

    $('#loginBtnMobile').click(function () {

        var $summaryUl = $("#validationSummaryLoginMobile").find("ul");
        $summaryUl.empty();
        $("#emptyFieldsMsg").empty();
        $("#emptyFieldsMsg").hide();
        var inputs = $("#formLoginMobile").find('.form-control');
        var NbrEmptyFields = 0;
        for (let field of inputs) {
            let elt = $(field)[0];
            $(elt).css('border-color', '');
            if ($(elt).val() == '') {
                $(elt).css('border-color', 'red');
                NbrEmptyFields++;
            }
        }
        if (NbrEmptyFields > 0) {

            $summaryUl.append("<li> " + $('#EmptyFields').val() + "</li>");
            $summaryUl.show();
        }
        else {

            $summaryUl.empty();
            $('#formLoginMobile').submit();
        }
    });

    $(document).on("change", ".GeneralCondition_CB", function () {
        ////debugger
        var isChecked = $(this).is(":checked");
        if (isChecked) {
            $('.registerBtn').prop("disabled", false)
        }
        else {
            $('.registerBtn').prop("disabled", true)
        }
    });

    $('#openTermsAndConditionModal').click(function () {
        $('#Terms_ConditionsModal').css('direction', 'ltr');
        $('#Terms_ConditionsModal').modal('show');
    });
    $('#openTermsAndConditionModalMobile').click(function () {
        $('#Terms_ConditionsModal').css('direction', 'ltr');
        $('#Terms_ConditionsModal').modal('show');
    });
    //$('.accept_Terms_Conditions').click(function () {
    //    $('.GeneralCondition_CB').prop('checked', true);
    //    //$('.GeneralCondition_CB').prop("disabled", false);
    //    $('.registerBtn').prop("disabled", false);
    //    $('#Terms_ConditionsModal').modal('toggle'); 
    //});

    $('#Terms_ConditionsModal').on('hidden.bs.modal', function () {
        setTimeout(() => {
            if (!window.matchMedia("only screen and (max-width: 768px)").matches) {
                $('#sidebarNavTogglerLogin').trigger('click.HSUnfold');
            }
            else {
                $('#signupMobile').css('opacity', '1');
                $('#signupMobile').css('display', 'block');
            }
        }, "300")
    });

    $('#registerBtn').click(function () {

        var $summaryUl = $("#validationSummaryRegister").find("ul");
        $summaryUl.empty();
        $("#emptyFieldsMsg").empty();
        $("#emptyFieldsMsg").hide();
        var inputs = $("#formRegister").find('.form-control');


        var NbrEmptyFields = 0;
        for (let field of inputs) {
            let elt = $(field)[0];
            $(elt).css('border-color', '');
            if ($(elt).val() == '') {
                $(elt).css('border-color', 'red');
                NbrEmptyFields++;
            }
        }

        if (NbrEmptyFields > 0) {

            $summaryUl.append("<li> " + $('#EmptyFields').val() + "</li>");
            $summaryUl.show();
        }
        else {

            $summaryUl.empty();
            $('#formRegister').submit();
        }
    });

    $('#registerBtnMobile').click(function () {

        var $summaryUl = $("#validationSummaryRegisterMobile").find("ul");
        $summaryUl.empty();
        $("#emptyFieldsMsg").empty();
        $("#emptyFieldsMsg").hide();
        $("#genderOptionsMobile").css('border-color', '#dfdcd7');
        var inputs = $("#formRegisterMobile").find('.form-control');


        var NbrEmptyFields = 0;
        for (let field of inputs) {
            let elt = $(field)[0];
            $(elt).css('border-color', '');
            if ($(elt).val() == '') {
                $(elt).css('border-color', 'red');
                NbrEmptyFields++;
            }
        }

        if (NbrEmptyFields > 0) {

            $summaryUl.append("<li> " + $('#EmptyFields').val() + "</li>");
            $summaryUl.show();
        }
        else {

            $summaryUl.empty();
            $('#formRegisterMobile').submit();
        }
    });

    $('#formLogin').submit(function (e) {
        e.preventDefault();
        var form = $(this);
        ////debugger
        $.ajax({
            url: form.attr('action'),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                "Email": form.find("input[name='email']").val(),
                "Password": form.find("input[name='password']").val(),
                "RememberMe": $('#RememberMeCB').is(":checked")
            }),
            success: function (response) {
                var $summaryUl = $("#validationSummaryLogin").find("ul");
                $summaryUl.empty();
                location.reload();
            },
            error: function (jqXhr, textStatus, errorThrown) {

                var listErroroMessages = new Array();
                var $summaryUl = $("#validationSummaryLogin").find("ul");
                $summaryUl.empty();
                if (textStatus === 'error') {
                    $.each(jqXhr.responseJSON.errors,
                        function (a, b) {
                            listErroroMessages.push(b);
                        });
                    translateErrorMessages(listErroroMessages, $summaryUl);
                }
            }
        });
    });

    $('#formLoginMobile').submit(function (e) {
        e.preventDefault();
        var form = $(this);

        $.ajax({
            url: form.attr('action'),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                "Email": form.find("input[name='email']").val(),
                "Password": form.find("input[name='password']").val(),
                "RememberMe": $('#RememberMeCB_Mobile').is(":checked")
            }),
            success: function (response) {
                location.reload();
                var $summaryUl = $("#validationSummaryLoginMobile").find("ul");
                $summaryUl.empty();
               
            },
            error: function (jqXhr, textStatus, errorThrown) {

                var listErroroMessages = new Array();
                var $summaryUl = $("#validationSummaryLoginMobile").find("ul");
                $summaryUl.empty();
                if (textStatus === 'error') {
                    $.each(jqXhr.responseJSON.errors,
                        function (a, b) {
                            listErroroMessages.push(b);
                        });
                    translateErrorMessages(listErroroMessages, $summaryUl);
                }
            }
        });
    });

    $('#formRegister').submit(function (e) {
        e.preventDefault();
        var form = $(this);
        $.ajax({
            url: form.attr('action'),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                "FirstName": form.find("input[name='firstname']").val(),
                "LastName": form.find("input[name='lastname']").val(),
                "Email": form.find("input[name='email']").val(),
                "Password": form.find("input[name='password']").val(),
                "ConfirmPassword": form.find("input[name='confirmPassword']").val(),
            }),
            success: function (resp) {
                //    resp = JSON.parse(resp);
                //    window.location.href = resp;
                window.location.pathname = '/MyAccount/RegisterConfirmation';
                fbq('track', 'CompleteRegistration',
                    // begin parameter object data
                    {
                        "data": [
                            {
                                "event_name": "CompleteRegistration",
                                "event_time": 1678180579,
                                "action_source": "website",
                                "event_id": null,
                                "user_data": {
                                    "em": [
                                        "7b17fb0bd173f625b58636fb796407c22b3d16fc78302d79f0fd30c2fc2fc068"
                                    ],
                                    "ph": [
                                        null
                                    ],
                                    "fb_login_id": null,
                                    "anon_id": null,
                                    "fn": [
                                        null
                                    ],
                                    "ln": [
                                        null
                                    ]
                                },
                                "custom_data": {
                                    "currency": "TND",
                                    "value": null
                                }
                            }
                        ]
                    }
                    // end parameter object data
                );

            },
            error: function (jqXhr, textStatus, errorThrown) {

                var listErroroMessages = new Array();
                var $summaryUl = $("#validationSummaryRegister").find("ul");
                $summaryUl.empty();
                $summaryUl.text("");
                if (textStatus === 'error') {
                    $.each(jqXhr.responseJSON.errors,
                        function (a, b) {
                            listErroroMessages.push(b);
                        });
                    translateErrorMessages(listErroroMessages, $summaryUl);
                }
            }
        });
    });

    $('#formRegisterMobile').submit(function (e) {
        e.preventDefault();
        var form = $(this);
        $.ajax({
            url: form.attr('action'),
            type: 'post',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({
                "FirstName": form.find("input[name='firstname']").val(),
                "LastName": form.find("input[name='lastname']").val(),
                "Email": form.find("input[name='email']").val(),
                "Password": form.find("input[name='password']").val(),
                "ConfirmPassword": form.find("input[name='confirmPassword']").val(),
            }),
            success: function (resp) {
                //    resp = JSON.parse(resp);
                //    window.location.href = resp;
                window.location.pathname = '/MyAccount/RegisterConfirmation';
                fbq('track', 'CompleteRegistration',
                    // begin parameter object data
                    {
                        "data": [
                            {
                                "event_name": "CompleteRegistration",
                                "event_time": 1678180579,
                                "action_source": "website",
                                "event_id": null,
                                "user_data": {
                                    "em": [
                                        "7b17fb0bd173f625b58636fb796407c22b3d16fc78302d79f0fd30c2fc2fc068"
                                    ],
                                    "ph": [
                                        null
                                    ],
                                    "fb_login_id": null,
                                    "anon_id": null,
                                    "fn": [
                                        null
                                    ],
                                    "ln": [
                                        null
                                    ]
                                },
                                "custom_data": {
                                    "currency": "TND",
                                    "value": null
                                }
                            }
                        ]
                    }
                    // end parameter object data
                );
            },
            error: function (jqXhr, textStatus, errorThrown) {

                var listErroroMessages = new Array();
                var $summaryUl = $("#validationSummaryRegisterMobile").find("ul");
                $summaryUl.empty();
                $summaryUl.text("");
                if (textStatus === 'error') {
                    $.each(jqXhr.responseJSON.errors,
                        function (a, b) {
                            listErroroMessages.push(b);
                        });
                    translateErrorMessages(listErroroMessages, $summaryUl);
                }
            }
        });
    });


    // MyAccount Page Sidebar menu
    $("ul.my__account-nav li a").click(function (e) {
        e.preventDefault();
        var link = $(this).attr('href');
        ////debugger
        $("#pills-myaccount-body").load($(this).attr('href'), function () {
            if ($(".dashboard-content").length > 0) {
                $(".dashboard-content a").click(function (e) {
                    e.preventDefault();
                    $("#pills-myaccount-body").load($(this).attr('href'));
                });
            }
        });

    });

    $(".dashboard-content a").click(function (e) {
        e.preventDefault();
        var link = $(this).attr('href');
        ////debugger;
        $("#pills-myaccount-body").load($(this).attr('href'));
    });

    $('#pdfviewer_viewerContainer').click(function () {
        if (!IsFullScreen()) {
            goFullScreen();
        }
        else {
            exitFullScreen();
        }
    }
    );

    getBookCartNumber();
    getUserWalletBalance();

});

function translateErrorMessages(listErroroMessages, $summaryUl) {
    //var $summaryUl = $("#validationSummaryLogin").find("ul");
    var i;
    for (i = 0; i < listErroroMessages.length; ++i) {
        var value = listErroroMessages[i];
        switch (value) {
            case "UserNotFound": {
                $summaryUl.append("<li> " + $('#UserNotFound').val() + "</li>");
                break;
            }
            case "EmailNotConfirmed": {
                $summaryUl.append("<li> " + $('#EmailNotConfirmed').val() + "</li>");
                break;
            }
            case "PasswordRequiresDigit": {
                $summaryUl.append("<li> " + $('#PasswordRequiresDigit').val() + "</li>");
                break;
            }
            case "PasswordRequiresLower": {
                $summaryUl.append("<li> " + $('#PasswordRequiresLower').val() + "</li>");
                break;
            }
            case "PasswordRequiresUpper": {
                $summaryUl.append("<li> " + $('#PasswordRequiresUpper').val() + "</li>");
                break;
            }
            case "PasswordRequiresNonAlphanumeric": {
                $summaryUl.append("<li> " + $('#PasswordRequiresNonAlphanumeric').val() + "</li>");
                break;
            }
            case "DuplicateUserName": {
                $summaryUl.append("<li> " + $('#DuplicateUserName').val() + "</li>");
                break;
            }
            case "Error_Email": {
                $summaryUl.append("<li> " + $('#WrongEmailFormat').val() + "</li>");
                break;
            }
            case "Error_ConfirmPassword": {
                $summaryUl.append("<li> " + $('#Error_ConfirmPassword').val() + "</li>");
                break;
            }
            case "Error_Password": {
                $summaryUl.append("<li> " + $('#Error_Password').val() + "</li>");
                break;
            }
            case "PasswordTooShort": {
                $summaryUl.append("<li> " + $('#Error_Password').val() + "</li>");
                break;
            }
            case "WrongPassword": {
                $summaryUl.append("<li> " + $('#WrongPassword').val() + "</li>");
                break;
            }
            case "AdminNotAuthorized": {
                $summaryUl.append("<li> " + $('#AdminNotAuthorized').val() + "</li>");
                break;
            }
            default: {
                $summaryUl.append("<li> " + "Unknown error" + "</li>");
                break;
            }
        }
    }
}

function getBookPDFContent(clickedElement) {
    var _bookId = $(clickedElement).attr("data-id");
    var _bookTitle = $(clickedElement).attr("data-Title");
    if ($(clickedElement).attr("data-library-id")) {
        $('#currentLibraryIdOfViewer').val($(clickedElement).attr("data-library-id"));
    }
    $('#currentPageOfViewer').val($(clickedElement).attr("data-current-page"));
    ////debugger
    var link = '/BookViewer/LoadBookPDFContent'
    //alert("ok");
    $.ajax({
        url: link,
        data: {
            '_BookId': _bookId
        },
        success: function (data) {
            $('#currentBookIdOfViewer').val(_bookId);
            $("#detailsModal .modal-title").empty();
            $("#detailsModal .modal-title").append(_bookTitle);

            $("#pdf-content").empty();
            
            $("#pdf-content").append(data);
            $('#detailsModal').modal('show');
        },
        error: function (xhr, status, error) {
            //debugger
            alert("error getBookPDFContent");
        }
    });
}

// Validate Confirm Password
$('#conpasscheck_ExtLog').hide();
//$('#newpassword-field_ExtLog').keyup(function () {
//    ////debugger
//    let passwordValue = $('#newpassword-field_ExtLog').val();
//    let confirmPasswordValue = $('#confirmpassword-field_ExtLog').val();
//    if (passwordValue.length >= 1 && confirmPasswordValue != "") {
//        if (passwordValue == confirmPasswordValue) {
//            $('#conpasscheck_ExtLog').hide();
//            $('#myBtn_ExtLog').removeAttr('disabled');
//            return true;
//        }
//        else {
//            $('#conpasscheck_ExtLog').show();
//            $('#conpasscheck_ExtLog').html($('#Error_ConfirmPassword').val());
//            $('#myBtn_ExtLog').attr('disabled', 'disabled');
//            return false;
//        }
//    }
//});
$('#confirmpassword-field_ExtLog').keyup(function () {
    let passwordValue = $('#newpassword-field_ExtLog').val();
    let confirmPasswordValue = $('#confirmpassword-field_ExtLog').val();
    ////debugger
    if (passwordValue == confirmPasswordValue && passwordValue != "") {
        $('#conpasscheck_ExtLog').hide();
        $('#myBtn_ExtLog').removeAttr('disabled');
    }
    else {
        $('#conpasscheck_ExtLog').show();
        $('#conpasscheck_ExtLog').html($('#Error_ConfirmPassword').val());
        $('#myBtn_ExtLog').attr('disabled', 'disabled');
    }
});

$('#ExternalLoginForm').submit(function (e) {
    e.preventDefault();
    var form = $(this);
    var formData = new FormData($(this)[0]);

    $.ajax({
        url: form.attr('action'),
        type: 'post',
        datatype: 'json',
        contentType: false,
        data: formData,
        async: true,
        cache: false,
        processData: false,
        success: function (data) {
            //var $summaryUl = $("#validationSummaryUpdate_ExtLog").find("ul");
            //$summaryUl.empty();
            //go to home page
            alert("resp1" + data)

            window.location.pathname = '/';
            //debugger
        },
        error: function (jqXhr, textStatus, errorThrown) {
            $("html, body").animate({ scrollTop: 0 }, 1000);
            var $summaryUl = $("#validationSummaryUpdate_ExtLog").find("ul");
            $summaryUl.empty();
            if (jqXhr.responseJSON.errors.length > 0) {
                var listErroroMessages = new Array();
                $.each(jqXhr.responseJSON.errors,
                    function (a, b) {
                        listErroroMessages.push(b);
                    });
                //debugger
                translateErrorMessages(listErroroMessages, $summaryUl);
            }
        }
    });
});


ej.base.L10n.load({
    'ar-AE': {
        'PdfViewer': {
            'PdfViewer': 'قوات الدفاع الشعبي المشاهد',
            'Cancel': 'إلغاء',
            'Download file': 'تحميل الملف',
            'Download': 'تحميل',
            'Enter Password': 'هذا المستند محمي بكلمة مرور. يرجى إدخال كلمة مرور.',
            'File Corrupted': 'ملف تالف',
            'File Corrupted Content': 'الملف تالف ولا يمكن فتحه.',
            'Fit Page': 'لائق بدنيا الصفحة',
            'Fit Width': 'لائق بدنيا عرض',
            'Automatic': 'تلقائي',
            'Go To First Page': 'عرض الصفحة الأولى',
            'Invalid Password': 'كلمة سر خاطئة. حاول مرة اخرى.',
            'Next Page': 'عرض الصفحة التالية',
            'OK': 'حسنا',
            'Open': 'فتح الملف',
            'Page Number': 'رقم الصفحة الحالية',
            'Previous Page': 'عرض الصفحة السابقة',
            'Go To Last Page': 'عرض الصفحة الأخيرة',
            'Zoom': 'تكبير',
            'Zoom In': 'تكبير في',
            'Zoom Out': 'تكبير خارج',
            'Page Thumbnails': 'مصغرات الصفحة',
            'Bookmarks': 'المرجعية',
            'Print': 'اطبع الملف',
            'Password Protected': 'كلمة المرور مطلوبة',
            'Copy': 'نسخ',
            'Text Selection': 'أداة اختيار النص',
            'Panning': 'وضع عموم',
            'Text Search': 'بحث عن نص',
            'Find in document': 'ابحث في المستند',
            'Match case': 'حالة مباراة',
            'Apply': 'تطبيق',
            'GoToPage': 'انتقل إلى صفحة',
            // tslint:disable-next-line:max-line-length
            'No matches': 'انتهى العارض من البحث في المستند. لم يتم العثور على مزيد من التطابقات',
            'No Text Found': 'لم يتم العثور على نص',
            'Undo': 'فك',
            'Redo': 'فعل ثانية',
            'Annotation': 'إضافة أو تعديل التعليقات التوضيحية',
            'Highlight': 'تسليط الضوء على النص',
            'Underline': 'تسطير النص',
            'Strikethrough': 'نص يتوسطه خط',
            'Delete': 'حذف التعليق التوضيحي',
            'Opacity': 'غموض',
            'Color edit': 'غير اللون',
            'Opacity edit': 'تغيير التعتيم',
            'Highlight context': 'تسليط الضوء',
            'Underline context': 'أكد',
            'Strikethrough context': 'يتوسطه',
            'Server error': 'خدمة الانترنت لا يستمع. يعتمد قوات الدفاع الشعبي المشاهد على خدمة الويب لجميع ميزاته. يرجى بدء خدمة الويب للمتابعة.',
            'Open text': 'افتح',
            'First text': 'الصفحة الأولى',
            'Previous text': 'الصفحة السابقة',
            'Next text': 'الصفحة التالية',
            'Last text': 'آخر صفحة',
            'Zoom in text': 'تكبير',
            'Zoom out text': 'تصغير',
            'Selection text': 'اختيار',
            'Pan text': 'مقلاة',
            'Print text': 'طباعة',
            'Search text': 'بحث',
            'Annotation Edit text': 'تحرير التعليق التوضيحي'

        }
    }

    //$("#pills-myaccount-body").load($(this).attr('href'));
});


$('.formJoinRequest button[type="submit"]').on('click', function () {


    var firstname = $(this).parents('form').find('input[name=Firstname]').val();
    var lastname = $(this).parents('form').find('input[name=Lastname]').val();
    var email = $(this).parents('form').find('input[name=Email]').val();
    var description = $(this).parents('form').find('input[name=Description]').val();
    if (firstname == '' || lastname == '' || email == '' /*|| description == ''*/) {
        $(this).parents('form').valid();

        $(".md-toast-error").fadeTo(2000, 500).slideUp(500, function () {
            $(".md-toast-error").slideUp(500);
        });
        return false;
    }


});

$('.formJoinRequest').submit(function (e) {
    e.preventDefault();

    var form = $(this);
    var firstname = form.find('input[name=Firstname]').val();
    var lastname = form.parents('form').find('input[name=Lastname]').val();
    var email = form.parents('form').find('input[name=Email]').val();
    var description = form.parents('form').find('input[name=Description]').val();
    if (firstname == '' || lastname == '' || email == '' || description == '') {
        form.valid();

        return false;
    }


    var formData = new FormData($(this)[0]);

    $.ajax({
        url: form.attr('action'),
        type: 'post',
        datatype: 'json',
        contentType: false,
        data: formData,
        async: true,
        cache: false,
        processData: false,

        success: function (data, textStatus, jQxhr) {

            $('.formJoinRequest input').not(':input[type=hidden]').val('');
            $('.formJoinRequest textarea').val('');
            $(".toast-success").fadeTo(2000, 500).slideUp(500, function () {
                $(".toast-success").slideUp(500);
            });
        },

        error: function (data, textStatus, jQxhr) {
            if (data.responseJSON.errors[0] == "This Email is Already Used")
                $("#error-toast-message").text($('#DuplicateUserName').val());
            else $("#error-toast-message").text(data.responseJSON.errors[0]);

            $("#md-toast-error-Id").fadeTo(2000, 500).slideUp(2000, function () {
                $(".md-toast-error").slideUp(500);
            });
            //debugger
        }
    });

});

$('#ComboboxContact').on('change', function () {
    if (this.value == '1')
    //.....................^.......
    {
        $("#formcontact").show();
    }
    else {
        $("#formcontact").hide();
    }

    if (this.value == '2')
    //.....................^.......
    {
        $("#FormAuteur").show();
    }
    else {
        $("#FormAuteur").hide();
    }

    if (this.value == '3')
    //.....................^.......
    {
        $("#formEditeur").show();
    }
    else {
        $("#formEditeur").hide();
    }
    if (this.value == '4')
    //.....................^.......
    {
        $("#formCompetition").show();
    }
    else {
        $("#formCompetition").hide();
    }
    if (this.value == '5')
    //.....................^.......
    {
        $("#formMothersDay").show();
    }
    else {
        $("#formMothersDay").hide();
    }
    if (this.value == '6')
    //.....................^.......
    {
        $("#formLaureat").show();
    }
    else {
        $("#formLaureat").hide();
    }
});

$(document).on("click", "a.addToFavoriteLink", function () {
    var thisLink = $(this);
    var linkText = thisLink.children("span");
    var linkIcon = thisLink.children("i");
    var bookId = $(this).attr("value");
    var isFavorite = $(this).attr("data-favorite");
    var currentDiv = $(this).closest(".product").parent();
    var inWishList = currentDiv.hasClass("FavoriteListItem");

    //debugger
    if (isFavorite == "True") {
        xxxcount++;
        //alert(xxxcount);
        $.ajax({
            method: "Delete",
            url: "/Book/DeleteBookFromFavorite",
            data: "BookId=" + JSON.stringify(bookId),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            success: function () {
               
                $(thisLink).attr("data-favorite", "False");
                if (linkText.text() != "") {
                    $(linkIcon).css({ 'color': 'black' });
                    $(linkText).text('');
                    $(linkText).append($('#AddToWishlist').val());
                }
                else {
                    $(thisLink).css('background-color', '');
                }
                $(".bookListView").load(location.href + " .bookListView");
                if (inWishList)
                    currentDiv.remove();
            },
            error: function (response, textStatus) {
               
                alert("error DeleteBookFromFavorite");
            }
        });
    }
    else {
        //debugger
        $.ajax({
            method: "POST",
            url: "/Book/AddBookToFavorite",
            data: "BookId=" + JSON.stringify(bookId),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            success: function () {
                var cultureInfo = '@System.Globalization.CultureInfo.CurrentCulture.Name';
                //debugger
                $(thisLink).attr("data-favorite", "True");
                if (linkText.text() != "") {
                    $(linkIcon).css({ 'color': '#f75454' });/*, 'font-weight': ''*/
                    $(linkText).text('');
                    $(linkText).append($('#RemoveFromWishlist').val());
                    //alert($(linkIcon).attr("data-favorite"));
                }
                else {
                    //alert($(linkIcon).attr("data-favorite"));
                    $(thisLink).css('background-color', '#f75454');
                }
                $(".bookListView").load(location.href + " .bookListView");
            },
            error: function (response, textStatus) {
               
                alert("error AddBookToFavorite");
                if (response.responseJSON.message == "Book_Exists_already")
                    $("#error-toast-message").text($('#AlreadyExists').val());
                else $("#error-toast-message").text(response.responseJSON.message);

                $("#md-toast-error-Id").fadeTo(2000, 500).slideUp(2000, function () {
                    $(".md-toast-error").slideUp(500);
                });
            }
        });
    }
    //$(this).blur();
});
var xxxcount = 1;
$(document).on("click", "a.addToFavoriteLink_LV", function () {
    var thisLink = $(this);
    var linkText = thisLink.children("span");
    var linkIcon = thisLink.children("i");
    var bookId = $(this).attr("value");
    var isFavorite = $(this).attr("data-favorite");
    if (isFavorite == "True") {
        xxxcount++;
        //alert(xxxcount);
        $.ajax({
            method: "Delete",
            url: "/Book/DeleteBookFromFavorite",
            data: "BookId=" + JSON.stringify(bookId),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            success: function () {
                $(thisLink).attr("data-favorite", "False");
                if (linkText.text() != "") {
                    $(linkIcon).css({ 'color': 'black' });
                    $(linkText).text('');
                    $(linkText).append($('#AddToWishlist').val());
                }
                else {
                    $(thisLink).css('background-color', '');
                }
                $("#grid-view-zone").load(location.href + " #grid-view-zone");
            },
            error: function (response, textStatus) {
                alert(textStatus);
            }
        });
    }
    else {
        $.ajax({
            method: "POST",
            url: "/Book/AddBookToFavorite",
            data: "BookId=" + JSON.stringify(bookId),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            success: function () {
                var cultureInfo = '@System.Globalization.CultureInfo.CurrentCulture.Name';
                $(thisLink).attr("data-favorite", "True");
                if (linkText.text() != "") {
                    $(linkIcon).css({ 'color': '#f75454' });/*, 'font-weight': ''*/
                    $(linkText).text('');
                    $(linkText).append($('#RemoveFromWishlist').val());
                    //alert($(linkIcon).attr("data-favorite"));
                }
                else {
                    //alert($(linkIcon).attr("data-favorite"));
                    $(thisLink).css('background-color', '#f75454');
                }
                $("#grid-view-zone").load(location.href + " #grid-view-zone");

            },
            error: function (response, textStatus) {
                //alert(textStatus);
                if (response.responseJSON.message == "Book_Exists_already")
                    $("#error-toast-message").text($('#AlreadyExists').val());
                else $("#error-toast-message").text(response.responseJSON.message);

                $("#md-toast-error-Id").fadeTo(2000, 500).slideUp(2000, function () {
                    $(".md-toast-error").slideUp(500);
                });
            }
        });
    }
    //$(this).blur();
});

$(document).on("click", "a.addToCartLink", function () {
    var thisLink = $(this);
    var linkSpan = thisLink.children("span.product__add-to-cart");
    var linkSpan2 = thisLink.children("span.product__add-to-cart-icon");
    var linkSpan3 = thisLink.children("span.woocommerce-Price-amount");
    var linkIcon2 = linkSpan2.children("i");
    var linkIcon3 = thisLink.children("i");
    var bookId = $(this).attr("value");
    var inCart = $(this).attr("data-cart");
    if (inCart == "True") {
        $.ajax({
            method: "DELETE",
            url: "/Book/DeleteBookFromCart",
            data: "BookId=" + JSON.stringify(bookId),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $(thisLink).attr("data-cart", "False");
                //$(thisLink).css('background-color', '');
                $(linkSpan2).css({ 'background-color': '' });
                $(linkSpan2).css({ 'color': 'black' });
                $(linkIcon3).css({ 'background-color': '' });
                $(linkIcon3).css({ 'color': 'black' });
                $(linkSpan).css({ 'color': 'black' });
                $(linkSpan2).css({ 'color': 'black' });
                //$(linkSpan3).css({ 'color': 'black' });
                $(linkSpan).text('');
                $(linkSpan).append($('#AddToCart').val());
                $(linkSpan3).text('');
                $(linkSpan3).append($('#AddToCart').val());
                $("#totalCount").text(response.value.count);
                $("#totalCountMobile").text(response.value.count);
                $(".bookListView").load(location.href + " .bookListView");
            },
            error: function (response, textStatus) {
                //alert(textStatus);
                //var x = response.responseJSON.message;
                if (response.responseJSON.message == "Book_Exists_already")
                    $("#error-toast-message").text($('#AlreadyInCart').val());
                else $("#error-toast-message").text(response.responseJSON.message);

                $("#md-toast-error-Id").fadeTo(2000, 500).slideUp(2000, function () {
                    $(".md-toast-error").slideUp(500);
                });
            }
        });
    }
    else {
        $.ajax({
            method: "POST",
            url: "/Book/AddBookToCart",
            data: "BookId=" + JSON.stringify(bookId),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $(thisLink).attr("data-cart", "True");
                //$(thisLink).css('background-color', '#f75454');
                $(linkSpan2).css({ 'background-color': '#f75454' });
                $(linkSpan2).css({ 'color': '#f75454' });
                $(linkIcon3).css({ 'background-color': '' });//'#f75454'
                $(linkIcon3).css({ 'color': '#f75454' });
                $(linkSpan).css({ 'color': '#f75454' });
                $(linkSpan2).css({ 'color': '#f75454' });
                //$(linkSpan3).css({ 'color': '#f75454' });
                $(linkSpan).text('');
                $(linkSpan).append($('#RemoveFromCart').val());
                $(linkSpan3).text('');
                $(linkSpan3).append($('#RemoveFromCart').val());
                $(".bookListView").load(location.href + " .bookListView");
                $("#totalCount").text(response.value.count);
                $("#totalCountMobile").text(response.value.count);

            },
            error: function (response, textStatus) {
                var x = response.responseJSON.message;
                //alert(textStatus);
                if (response.responseJSON.message == "Book_Exists_already")
                    $("#error-toast-message").text($('#AlreadyInCart').val());
                else $("#error-toast-message").text(response.responseJSON.message);

                $("#md-toast-error-Id").fadeTo(2000, 500).slideUp(2000, function () {
                    $(".md-toast-error").slideUp(500);
                });
            }
        });
    }
});

$(document).on("click", "a.addToCartLink_LV", function () {
    var thisLink = $(this);
    var linkText = thisLink.children("span");
    var linkIcon = thisLink.children("i");
    var bookId = $(this).attr("value");
    var inCart = $(this).attr("data-cart");
    if (inCart == "True") {
        $.ajax({
            method: "DELETE",
            url: "/Book/DeleteBookFromCart",
            data: "BookId=" + JSON.stringify(bookId),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $(thisLink).attr("data-cart", "False");
                if (linkText.text() != "") {
                    $(linkIcon).css({ 'color': 'black' });
                }
                else {
                    $(thisLink).css('background-color', '');
                }
                $("#totalCount").text(response.value.count);
                $("#totalCountMobile").text(response.value.count);
                $("#grid-view-zone").load(location.href + " #grid-view-zone");
            },
            error: function (response, textStatus) {
                //alert(textStatus);
                //var x = response.responseJSON.message;
                if (response.responseJSON.message == "Book_Exists_already")
                    $("#error-toast-message").text($('#AlreadyInCart').val());
                else $("#error-toast-message").text(response.responseJSON.message);

                $("#md-toast-error-Id").fadeTo(2000, 500).slideUp(2000, function () {
                    $(".md-toast-error").slideUp(500);
                });
            }
        });
    }
    else {
        $.ajax({
            method: "POST",
            url: "/Book/AddBookToCart",
            data: "BookId=" + JSON.stringify(bookId),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            success: function (response) {
                $(thisLink).attr("data-cart", "True");
                if (linkText.text() != "") {
                    $(linkIcon).css({ 'color': '#f75454' });
                }
                else {
                    $(thisLink).css('background-color', '#f75454');
                }
                $("#totalCount").text(response.value.count);
                $("#totalCountMobile").text(response.value.count);
                $("#grid-view-zone").load(location.href + " #grid-view-zone");
            },
            error: function (response, textStatus) {
                if (response.responseJSON.message == "Book_Exists_already")
                    $("#error-toast-message").text($('#AlreadyInCart').val());
                else $("#error-toast-message").text(response.responseJSON.message);

                $("#md-toast-error-Id").fadeTo(2000, 500).slideUp(2000, function () {
                    $(".md-toast-error").slideUp(500);
                });
            }
        });
    }
});

$(document).on("click", "a.addToLibraryLink", function (e) {
    var thisLink = $(this);
    var linkSpan2 = thisLink.children("span.product__add-to-cart");
    var linkSpan3 = thisLink.children("span.woocommerce-Price-amount");
    var linkIcon3 = thisLink.children("i");
    var bookId = $(this).attr("value");
    var inLibrary = $(this).attr("data-library");
    var promotionId = $(this).attr("data-promotion");
    if (inLibrary == "True") {
        $.ajax({
            method: "DELETE",
            url: "/Book/DeleteBookFromLibrary",
            data: "BookId=" + JSON.stringify(bookId),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            success: function () {
                $(thisLink).attr("data-library", "False");
                //$(linkSpan2).css({ 'background-color': '' });
                $(linkSpan2).css({ 'color': 'black' });
                $(linkIcon3).css({ 'background-color': '' });
                $(linkIcon3).css({ 'color': 'black' });
                //$(linkSpan).css({ 'color': 'black' });
                //$(linkSpan).text('');
                //$(linkSpan).append($('#AddToLibrary').val());
                $(linkSpan3).text('');
                $(linkSpan2).text('');
                //$(linkSpan3).css({ 'color': 'black' });
                $(linkSpan3).append($('#AddToLibrary').val());
                $(linkSpan2).append($('#AddToLibrary').val());
                $(".bookListView").load(location.href + " .bookListView");
            },
            error: function (response, textStatus) {
                //alert(textStatus);
                if (response.responseJSON.errors[0] == "This Email is Already Used")
                    $("#error-toast-message").text($('#DuplicateUserName').val());
                else $("#error-toast-message").text(data.responseJSON.errors[0]);

                $("#md-toast-error-Id").fadeTo(2000, 500).slideUp(2000, function () {
                    $(".md-toast-error").slideUp(500);
                });
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            url: "/Book/AddBookToLibrary",
            //data: { 'BookId': bookId, 'PromotionId': promotionId },
            //data: JSON.stringify({ 'BookId': bookId, 'PromotionId': promotionId }),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            data: { BookId: bookId, PromotionId: promotionId },
            dataType: "json",
            success: function (response) {
                if (response.value.maxReached) {
                    $("#maxLib").fadeIn(2000);
                    $("#maxLib").delay(2000).fadeOut('slow');
                    return;
                }
                //alert("link must be disabled");
                $(thisLink).attr("data-library", "True");
                $(linkSpan2).css({ 'color': '#f75454' });
                $(linkSpan3).css({ 'color': '#f75454' });
                $(linkIcon3).css({ 'background-color': '' });
                $(linkIcon3).css({ 'color': '#f75454' });
                //$(linkSpan).css({ 'color': '#f75454' });
                //$(linkSpan).text('');
                //$(linkSpan).append($('#RemoveFromLibrary').val());
                $(linkSpan2).text('');
                $(linkSpan3).text('');
                //$(linkSpan3).css({ 'color': '#f75454' });
                $(linkSpan2).append($('#InLibrary').val());
                $(linkSpan3).append($('#InLibrary').val());
                $("#grid-view-zone").load(location.href + " #grid-view-zone");
                $(".bookListView").load(location.href + " .bookListView");
                $(thisLink).css('cursor', 'auto');
                $(thisLink).prop("disabled", true);

            },
            error: function (response, textStatus) {
                //alert(textStatus);
                if (response.responseJSON.message == "Book_Exists_already")
                    $("#error-toast-message").text($('#AlreadyInCart').val());
                else $("#error-toast-message").text(response.responseJSON.message);

                $("#md-toast-error-Id").fadeTo(2000, 500).slideUp(2000, function () {
                    $(".md-toast-error").slideUp(500);
                });
            }
        });
    }
});

$(document).on("click", "a.addToLibraryLink_LV", function () {
    var thisLink = $(this);
    var linkText = thisLink.children("span");
    var linkIcon = thisLink.children("i");
    var bookId = $(this).attr("value");
    var inLibrary = $(this).attr("data-library");
    var bookId = $(this).attr("value");
    var promotionId = $(this).attr("data-promotion");
    if (inLibrary == "True") {
        $.ajax({
            method: "Delete",
            url: "/Book/DeleteBookFromLibrary",
            data: "BookId=" + JSON.stringify(bookId),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            success: function () {
                $(thisLink).attr("data-library", "False");
                if (linkText.text() != "") {
                    $(linkIcon).css({ 'color': 'black' });
                }
                else {
                    $(thisLink).css('background-color', '');
                }
                $("#grid-view-zone").load(location.href + " #grid-view-zone");
            },
            error: function (response, textStatus) {
                alert(textStatus);
            }
        });
    }
    else {
        $.ajax({
            method: "POST",
            url: "/Book/AddBookToLibrary",
            data: { BookId: bookId, PromotionId: promotionId },
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            dataType: "json",
            success: function () {
                var cultureInfo = '@System.Globalization.CultureInfo.CurrentCulture.Name';
                $(thisLink).attr("data-library", "True");
                if (linkText.text() != "") {
                    $(linkIcon).css({ 'color': '#f75454' });
                }
                else {
                    $(thisLink).css('background-color', '#f75454');
                }
                $("#grid-view-zone").load(location.href + " #grid-view-zone");
                $(thisLink).css('cursor', 'auto');
                $(thisLink).prop("disabled", true);
            },
            error: function (response, textStatus) {
                //alert(textStatus);
                if (response.responseJSON.message == "Book_Exists_already")
                    $("#error-toast-message").text($('#AlreadyExists').val());
                else $("#error-toast-message").text(response.responseJSON.message);

                $("#md-toast-error-Id").fadeTo(2000, 500).slideUp(2000, function () {
                    $(".md-toast-error").slideUp(500);
                });
            }
        });
    }
    //$(this).blur();
});

var _currentFavoriteLink = "/MyAccount/GetWishlistBooks";
$("#sidebarNavToggler").click(function (e) {
    e.preventDefault();
    //_currentFavoriteLink = $(this).attr('href');
    $.ajax({
        type: "GET",
        url: "/MyAccount/Index",
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        //async: false,
        success: function (data) {
            document.open();
            document.write(data);
            document.close();
            //afterSuccess(); 
        },
        error: function (response) {
            alert(response.responseText);
        }
    }).always(function () {
        $("#pills-myaccount-body").load(_currentFavoriteLink);
    });
});

var afterSuccess = function () {
    var tt = $("#pills-myaccount-body");
    $("#pills-myaccount-body").load(_currentFavoriteLink);
};

$(document).on("click", ".MyAccountGoBack", function () {
    //$(".MyAccountGoBack").click(function (e) {
    $("#pills-myaccount-body").load($(this).attr('href')); 
});

$("#sidebarNavTogglerMobile").click(function (e) {
    e.preventDefault();
    //_currentFavoriteLink = $(this).attr('href');
    $.ajax({
        type: "GET",
        url: "/MyAccount/Index",
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        //async: false,
        success: function (data) {
            document.open();
            document.write(data);
            document.close();
        },
        error: function (response) {
            alert(response.responseText);
        }
    }).always(function () {
        $("#pills-myaccount-body").load(_currentFavoriteLink);
    });
});


//window.location.href = '/MyAccount/Index';
//    $("#pills-myaccount-body").load("/MyAccount/Index", function () {
//        if ($(".dashboard-content").length > 0) {
//            $(".dashboard-content a").click(function (e) {
//                e.preventDefault();
//                $("#pills-myaccount-body").load("/MyAccount/GetWishlistBooks");
//            });
//        }
//    });
//});

$('#sidebarNavToggler1').click(function () {
    $.ajax({
        type: "GET",
        url: "/Book/GetCartList",
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            $('#CartListDiv').html(response);
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            $('#CartListDiv').html(response);
        }
    });
});
var r = 'e';
$('#sidebarNavToggler1Mobile').click(function () {
    $.ajax({
        type: "GET",
        url: "/Book/GetCartList",
        //data: '{customerId: "' + customerId + '" }',
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (response) {
            $('#CartListDiv').html(response);
            //$('#dialog').dialog('open');
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            $('#CartListDiv').html(response);
            //alert(response.responseText);
        }
    });
});

$(document).on("change", ".editionNameCB_Cat", function () {
    var prized = $(this).attr('value');
    var isChecked = $(this).is(":checked");
    //_previousCategoryCB.prop("checked", false); // Unchecks it
    if (isChecked) {
        $.ajax({
            method: "GET",
            url: "/Prized/Details",
            data: "Edition=" + JSON.stringify(prized),
            contentType: "application/x-www-form-urlencoded; charset=utf-8",
            // dataType: "json",
            success: function (response) {
                $("body").html(response);

            },
            error: function (response, textStatus) {
                alert("error editionNameCB_Cat");
            }
        });
    }
});


$(document).on("keyup", "#searchTheKey_Cat", function () {
    var value = $(this).val().toLowerCase();
    $("#authorsListId_Cat_web li").each(function () {
        if ($(this).text().toLowerCase().search(value) > -1) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
});

$(document).on("search", "#searchTheKey_Cat", function () {
    $("#authorsListId_Cat_web li").each(function () {
        $(this).show();
    });
});

$(document).on("keyup", "#searchTheKey_Cat_mobile", function () {
    var value = $(this).val().toLowerCase();
    //debugger
    $("authorsListId_Cat li").each(function () {
        if ($(this).text().toLowerCase().search(value) > -1) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
});

$(document).on("search", "#searchTheKey_Cat_mobile", function () {
    $("authorsListId_Cat li").each(function () {
        $(this).show();
    });
});

$(document).on("keyup", "#searchTheKey_Promo", function () {
    var value = $(this).val().toLowerCase();
    //debugger
    $(".authorsListId_Promo_web li").each(function () {
        if ($(this).text().toLowerCase().search(value) > -1) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
});

$(document).on("search", "#searchTheKey_Promo", function () {
    $(".authorsListId_Promo_web li").each(function () {
        $(this).show();
    });
});

$(document).on("keyup", "#searchTheKey_Promo_mobile", function () {
    var value = $(this).val().toLowerCase();
    ////debugger
    $(".authorsListId_Promo_mobile li").each(function () {
        if ($(this).text().toLowerCase().search(value) > -1) {
            $(this).show();
        }
        else {
            $(this).hide();
        }
    });
});

$(document).on("search", "#searchTheKey_Promo_mobile", function () {
    $(".authorsListId_Promo_mobile li").each(function () {
        $(this).show();
    });
});




var lastScrollTop_Cat = 0;
var call_time_Cat = 1;
var isloading_Cat = false;
var isEmpty_Cat = false;

$(window).unbind("scroll");






//Promotions Books section
var authors_filter_list_Promo = [];
var languages_filter_list_Promo = [];
var categories_filter_list_Promo = [];


$(document).on("change", ".PromotionTypeCB_Promo", function () {
    //debugger
    if ($(this).is(":checked"))
        window.location.href = $(this).attr('value');
});



// promo region
var lastScrollTop_Promo = 0;
var call_time_Promo = 1;
var isloading_Promo = false;
var isEmpty_Promo = false;

$(window).unbind("scroll");

function fillPromoBookFilter() {
    var categories = new Array();
    var authors = new Array();
    var languages = new Array();

    $(".categoryNameCB_Promo:checkbox:checked").each(function () {
        categories.push($(this).val());
    });
    $(".authorNameCB_Promo:checkbox:checked").each(function () {
        authors.push($(this).val());
    });
    $(".languageNameCB_Promo:checkbox:checked").each(function () {
        languages.push(Number($(this).val()));
    });
    var pagedBooks = new Object();
    pagedBooks.Categories = categories;
    pagedBooks.Authors = authors;
    pagedBooks.Languages = languages;
    pagedBooks.PageIndex = call_time_Promo;
    pagedBooks.PageSize = 12;
    pagedBooks.EditorId = "";
    pagedBooks.IsPromotedBook = true;
    pagedBooks.PromotionType = 0; //free
    return pagedBooks;
    l
}
$(document).on("change", ".filter_PROMO", function () {
    if (isEmpty_NB) { return; }
    isloading_NB = true;
    call_time_Promo = 0;
    var filter = fillPromoBookFilter();
    $.ajax({
        url: 'LoadPromoBookList',
        type: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(filter),
        success: function (response) {
            if (response.status == 0) {
                $("#Div_loadMore_Promo").css("display", "none");
                $(".Full_List_Panel").fadeIn(2000);
                $('.Full_List_Panel').delay(2000).fadeOut('slow');
                $('#grid-view-zone ul.bookGridView').empty();
                $('#list-view-zone ul.bookListView').empty();
                return;
            }
            $("#Div_loadMore_Promo").css("display", "block");

            if (response.booksCount < 12) {
                $("#loadMore_Promo").prop('disabled', true);
            } else{

                $("#loadMore_Promo").prop('disabled', false)
            }
            call_time_Promo = call_time_Promo + 1;
            $('#grid-view-zone ul.bookGridView').empty();
            $('#list-view-zone ul.bookListView').empty();
            $('#grid-view-zone ul.bookGridView').append(response.booksGridView);
            $('#list-view-zone ul.bookListView').append(response.booksListView);
        },
        error: function (xhr) {

            //var err = JSON.parse(xhr.responseText);
            //alert(xhr.responseText); 
        }
    });
});
$("#loadMore_Promo").click(function () {

    if (isEmpty_Promo) { return; }
    isloading_Promo = true;
    var _promoType = $("#PromoTypeId").text();
    var filter = fillPromoBookFilter();
    $.ajax({
        url: 'LoadPromoBookList',
        type: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(filter),
        success: function (response) {
            ////debugger
            if (response.booksCount < 12) {
                $("#loadMore_Promo").prop('disabled', true)
            } else {
                $("#loadMore_Promo").prop('disabled', false)
            }
            call_time_Promo = call_time_Promo + 1;
            if (response.status == 0) {
                $("#Div_loadMore_Promo").css("display", "none");
                $(".Full_List_Panel").fadeIn(2000);
                $('.Full_List_Panel').delay(2000).fadeOut('slow');
                return;
            }
            $('#grid-view-zone ul.bookGridView').append(response.booksGridView);
            $('#list-view-zone ul.bookListView').append(response.booksListView);


        },
        error: function (xhr, status, error) {
            //debugger
            //var err = eval("(" + xhr.responseText + ")");
            alert(xhr.responseText);
            //alert("error");
        }
    });
});



var lastScrollTop_NB = 0;
var call_time_NB = 1;
var isloading_NB = false;
var isEmpty_NB = false;

$(window).unbind("scroll");


//New Books section

function fillNewBookFilter() {
    var categories = new Array();
    var authors = new Array();
    var languages = new Array();

    $(".categoryNameCB_NB:checkbox:checked").each(function () {
        categories.push($(this).val());
    });
    $(".authorNameCB_NB:checkbox:checked").each(function () {
        authors.push($(this).val());
    });
    $(".languageNameCB_NB:checkbox:checked").each(function () {
        languages.push(Number($(this).val()));
    });
    var pagedBooks = new Object();
    pagedBooks.Categories = categories;
    pagedBooks.Authors = authors;
    pagedBooks.Languages = languages;
    pagedBooks.PageIndex = call_time_NB;
    pagedBooks.PageSize = 12;
    pagedBooks.EditorId = "";
    return pagedBooks;
    //return {
    //    name: name,
    //    totalscore: totalScore,
    //    gamesPlayed: gamesPlayed
    //};
}
$(document).on("change", ".filter_NB", function () {
    if (isEmpty_NB) { return; }
    isloading_NB = true;
    call_time_NB = 0;
    var filter = fillNewBookFilter();
    $.ajax({
        url: 'FiltredBookList',
        type: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(filter),
        success: function (response) {
            
            if (response.booksCount < 12) {
                $("#loadMore_NB").prop('disabled', true)
            } else {
                $("#loadMore_NB").prop('disabled', false)
            }
                call_time_NB = call_time_NB + 1;
                $('#grid-view-zone ul.bookGridView').empty();
                $('#list-view-zone ul.bookListView').empty();
                $('#grid-view-zone ul.bookGridView').append(response.booksGridView);
                $('#list-view-zone ul.bookListView').append(response.booksListView);
        },
        error: function (xhr) {

            //var err = JSON.parse(xhr.responseText);
            //alert(xhr.responseText); 
        }
    });
});

$("#loadMore_NB").click(function () {
    if (isEmpty_NB) { return; }
    isloading_NB = true;
    var filter = fillNewBookFilter();
    $.ajax({
        url: 'FiltredBookList',
        type: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(filter),
        success: function (response) {
            if (response.booksCount < 12) {
                $("#loadMore_NB").prop('disabled', true)
            } else {
                $("#loadMore_NB").prop('disabled', false)
            }
            call_time_NB = call_time_NB + 1;
            $('#grid-view-zone ul.bookGridView').append(response.booksGridView);
         $('#list-view-zone ul.bookListView').append(response.booksListView);
        },
        error: function (xhr) {

            //var err = JSON.parse(xhr.responseText);
            //alert(xhr.responseText); 
        }
    });
});

function fillCAT_BookFilter() {
    var categories = new Array();
    var authors = new Array();
    var languages = new Array();

    $(".categoryNameCB_Cat:checkbox:checked").each(function () {
        categories.push($(this).val());
    });
    $(".authorNameCB_Cat:checkbox:checked").each(function () {
        authors.push($(this).val());
    });
    $(".languageNameCB_Cat:checkbox:checked").each(function () {
        languages.push(Number($(this).val()));
    });
    var pagedBooks = new Object();
    pagedBooks.Categories = categories;
    pagedBooks.Authors = authors;
    pagedBooks.Languages = languages;
    pagedBooks.PageIndex = call_time_NB;
    pagedBooks.PageSize = 12;
    pagedBooks.EditorId = "";
    return pagedBooks;
}

$(document).on("change", ".filter_CAT", function () {
    if (isEmpty_NB) { return; }
    isloading_NB = true;
    call_time_NB = 0;
    var filter = fillCAT_BookFilter();
    $.ajax({
        url: 'LoadCategoryBookList',
        type: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(filter),
        success: function (response) {
            if (response.booksCount < 12) {
                $("#loadMore_CD").prop('disabled', true)
            } else {
                $("#loadMore_CD").prop('disabled', false)
            }
            call_time_NB = call_time_NB + 1;
            $('#grid-view-zone ul.bookGridView').empty();
            $('#list-view-zone ul.bookListView').empty();
           $('#grid-view-zone ul.bookGridView').append(response.booksGridView);
            $('#list-view-zone ul.bookListView').append(response.booksListView);
        },
        error: function (xhr) {

            //var err = JSON.parse(xhr.responseText);
            //alert(xhr.responseText); 
        }
    });
});
function fillCAT_BookFilter_Mobile() {
    var categories = new Array();
    var authors = new Array();
    var languages = new Array();

    $(".categoryNameCB_Cat_Mobile:checkbox:checked").each(function () {
        categories.push($(this).val());
    });
    $(".authorNameCB_Cat_Mobile:checkbox:checked").each(function () {
        authors.push($(this).val());
    });
    $(".languageNameCB_Cat_Mobile:checkbox:checked").each(function () {
        languages.push(Number($(this).val()));
    });
    var pagedBooks = new Object();
    pagedBooks.Categories = categories;
    pagedBooks.Authors = authors;
    pagedBooks.Languages = languages;
    pagedBooks.PageIndex = call_time_NB;
    pagedBooks.PageSize = 12;
    pagedBooks.EditorId = "";
    return pagedBooks;
}

$(document).on("change", ".filter_CAT_Mobile", function () {
    if (isEmpty_NB) { return; }
    isloading_NB = true;
    call_time_NB = 0;
    var filter = fillCAT_BookFilter_Mobile();
    $.ajax({
        url: 'LoadCategoryBookList',
        type: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(filter),
        success: function (response) {
            if (response.booksCount < 12) {
                $("#loadMore_CD").prop('disabled', true)
            } else {
                $("#loadMore_CD").prop('disabled', false)
            }
            call_time_NB = call_time_NB + 1;
            $('#grid-view-zone ul.bookGridView').empty();
            $('#list-view-zone ul.bookListView').empty();
           $('#grid-view-zone ul.bookGridView').append(response.booksGridView);
            $('#list-view-zone ul.bookListView').append(response.booksListView);
        },
        error: function (xhr) {

            //var err = JSON.parse(xhr.responseText);
            //alert(xhr.responseText); 
        }
    });
});
$("#loadMore_CD").click(function () {
    if (isEmpty_NB) { return; }
    isloading_NB = true;
    var categories = new Array();

    var filter = fillCAT_BookFilter();


    $.ajax({
        url: 'LoadCategoryBookList',
        type: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(filter),
        success: function (response) {
            call_time_NB = call_time_NB + 1;
            if (response.booksCount < 12) {
                $("#loadMore_CD").prop('disabled', true)
            } else {
                $("#loadMore_CD").prop('disabled', false)
            }
           $('#grid-view-zone ul.bookGridView').append(response.booksGridView);
            $('#list-view-zone ul.bookListView').append(response.booksListView);
        },
        error: function (xhr) {

            //var err = JSON.parse(xhr.responseText);
            //alert(xhr.responseText); 
        }
    });
});
$(document).on("keyup", ".searchTheKey_EL", function () {
    var value = $(this).val().toLowerCase();
    //if (value.length < 3) return;
    $("#editorsListId_EL li").removeClass('flagvisibility');
    $("#editorsListId_EL li").each(function () {

        var raisonSocial = $(this).find('.woocommerce-loop-product__title a').text();
        if (raisonSocial.toLowerCase().search(value) < 0) {
            $(this).addClass('flagvisibility');
        }

    });
});

$(document).on("search", ".searchTheKey_EL", function () {
    $("#editorsListId_EL li").each(function () {
        $(this).show();
    });
});

$('.RecoverPasswordBtn').click(function () {
    //debugger
    var email = $.trim($("#signinEmail3").val());
    $.ajax({
        type: "Post",
        url: "/MyAccount/ChangePasswordMail",
        data: "Email=" + JSON.stringify(email),
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        dataType: "html",
        success: function (response) {
            //debugger
            //show toast
            $("#IdRecoverPasswordMailText").css('display', 'block');
        },
        error: function (response) {
            //debugger
            $("#md-toast-error-Id").fadeTo(2000, 500).slideUp(500, function () {
                $("#md-toast-error-Id").slideUp(500);
            });
        }
    });
});

var lastScrollTop_ED = 0;
var call_time_ED = 1;
var isloading_ED = false;
var isEmpty_ED = false;
var oldHeight = 1;
$(window).unbind("scroll");

$("#loadMore_ED").click(function () {
    var ModelEditorId = $("#ModelEditorId").attr("value");
    ////debugger
    $.ajax({
        url: 'LoadEditorBookList',
        type: 'post',
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        data: { currentPageIndex: JSON.stringify(call_time_ED), EditorId: ModelEditorId },
        success: function (response) {
            ////debugger
            call_time_ED = call_time_ED + 1;
            if (response.booksCount < 12) {
                //debugger
                isEmpty_ED = true;
                $("#Div_loadMore_ED").css("display", "none");
            }
            if (response.status == 0) {
                $("#Div_loadMore_ED").css("display", "none");
                $(".Full_List_Panel").fadeIn(2000);
                $('.Full_List_Panel').delay(2000).fadeOut('slow');
                return;
            }
            $('#grid-view-zone_ED ul.bookGridView').append(response.booksGridView);

        },
        error: function (xhr, status, error) {
            //debugger
            //var err = eval("(" + xhr.responseText + ")");
            alert(xhr.responseText);
            //alert("error");
        }
    });
});

$("#searchTheKey_Cat_web").on('keyup', function () {
    var value = $(this).val().toLowerCase();
   
    if (value != "") {
        $(".authorsListId_Cat_web li").each(function () {
            if ($(this).text().toLowerCase().search(value) > -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    }
    else {
        $(".authorsListId_Cat_web li").each(function () {
            $(this).show();
        });
    }
});

$("#searchTheKey_Cat_mobile").on('keyup', function () {
    var value = $(this).val().toLowerCase();
    //debugger
    if (value != "") {
        $(".authorsListId_Cat_mobile li").each(function () {
            if ($(this).text().toLowerCase().search(value) > -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    }
    else {
        $("#authorsListId_Cat_mobile li").each(function () {
            $(this).show();
        });
    }
});

$("#searchTheKey_NB_web").on('keyup', function () {
    var value = $(this).val().toLowerCase();
   
    if (value != "") {
        $(".authorsListId_NB_web li").each(function () {
            if ($(this).text().toLowerCase().search(value) > -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    }
    else {
        $(".authorsListId_NB_web li").each(function () {
            $(this).show();
        });
    }
});

$("#searchTheKey_NB_mobile").on('keyup', function () {
    var value = $(this).val().toLowerCase();
    //debugger
    if (value != "") {
        $(".authorsListId_NB_mobile li").each(function () {
            if ($(this).text().toLowerCase().search(value) > -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    }
    else {
        $("#authorsListId_NB_mobile li").each(function () {
            $(this).show();
        });
    }
});

$("#searchTheKey_Promo_web").on('keyup', function () {
    var value = $(this).val().toLowerCase();
   
    if (value != "") {
        $(".authorsListId_Promo_web li").each(function () {
            if ($(this).text().toLowerCase().search(value) > -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    }
    else {
        $(".authorsListId_Promo_web li").each(function () {
            $(this).show();
        });
    }
});

$("#searchTheKey_Promo_mobile").on('keyup', function () {
    var value = $(this).val().toLowerCase();
    //debugger
    if (value != "") {
        $(".authorsListId_Promo_mobile li").each(function () {
            if ($(this).text().toLowerCase().search(value) > -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    }
    else {
        $("#authorsListId_Promo_mobile li").each(function () {
            $(this).show();
        });
    }
});


$("#searchTheKey_AD_web").on('keyup', function () {
    var value = $(this).val().toLowerCase();
   
    if (value != "") {
        $(".authorsListId_AD_web li").each(function () {
            if ($(this).text().toLowerCase().search(value) > -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    }
    else {
        $(".authorsListId_AD_web li").each(function () {
            $(this).show();
        });
    }
});

$("#searchTheKey_AD_mobile").on('keyup', function () {
    var value = $(this).val().toLowerCase();
    //debugger
    if (value != "") {
        $(".authorsListId_AD_mobile li").each(function () {
            if ($(this).text().toLowerCase().search(value) > -1) {
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    }
    else {
        $("#authorsListId_AD_mobile li").each(function () {
            $(this).show();
        });
    }
});

function getBookCartNumber() {
    //debugger
    $.ajax({
        method: "get",
        url: "/Book/GetBookCartNumber",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            //debugger
            var x = response.count;
            $("#totalCount").text(response.count);
            $("#totalCountMobile").text(response.count);
        }
    });
}

function getUserWalletBalance() {
    $.ajax({
        method: "get",
        url: "/WalletTransaction/GetCurrentUserBalance",
        contentType: "application/x-www-form-urlencoded; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (response) {
            $(".walletBalance").html(response.balance + 'DT');
            //$("#userWalletBalanceMobile").text(response.balance + 'DT');
            if (response.balance > 0) {
                $(".wallet").removeClass('flaticon-wallet-empty').addClass('flaticon-wallet-filled');
                $(".wallet-bigsize").removeClass('flaticon-wallet-empty').addClass('flaticon-wallet-filled');
                $(".flaticon-wallet-img").removeClass('flaticon-wallet-empty-img').addClass('flaticon-wallet-filled-img');
            } else {
                $(".wallet").removeClass('flaticon-wallet-filled').addClass('flaticon-wallet-empty');
                $(".wallet-bigsize").removeClass('flaticon-wallet-filled').addClass('flaticon-wallet-empty');
                $(".flaticon-wallet-img").removeClass('flaticon-wallet-filled-img').addClass('flaticon-wallet-empty-img');
            }
        }
    });
}

$('#SelectionSection2').slick({
    dots: false,
    infinite: false,
    speed: 300,
    slidesToShow: 5,
    slidesToScroll: 5,
    responsive: [
        {
            breakpoint: 1500,
            settings: {
                slidesToShow: 4,
                slidesToScroll: 4,
                infinite: false,
                dots: false
            }
        },
        {
            breakpoint: 992,
            settings: {
                slidesToShow: 3,
                slidesToScroll: 3
            }
        },
        {
            breakpoint: 554,
            settings: {
                slidesToShow: 2,
                slidesToScroll: 2
            }
        }
        // You can unslick at a given breakpoint now by adding:
        // settings: "unslick"
        // instead of a settings object
    ]
});
