

var ajaxDisableSubmitButton = function () {
    $("div#Ajax :submit").attr("disabled", "disabled");
};


var ajaxFormSuccess = function (result) {
    $('#Ajax').html(result);
    ajaxFormRegister();
};

var ajaxFormSubmit = function () {
    if ($(this).valid()) {
        var submitData = $(this).serialize();
        //submitData.ajax = true;
        $.ajax({
            url: this.action,
            type: this.method,
            data: submitData,
            success: ajaxFormSuccess
        });
    }
    return false;
};

var ajaxFormRegister = function () {
    $('div#Ajax > form[data-group="jQuery"]').submit(ajaxFormSubmit);
};

$(function () {

    jQuery.ajaxSetup({
        beforeSend: function () {
            ajaxDisableSubmitButton();
        },
        complete: function () { },
        success: function () { }
    });

    ajaxFormRegister();
});

