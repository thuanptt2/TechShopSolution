$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});
$(function () {
    $('#closeButtonModal').on('click', function () {
        $('#form-modal').modal('hide');
    });
    $('#btnEditInfo').on('click', function () {
        if ($('#fieldSetUpdate').attr('disabled') == "disabled")
            $('#fieldSetUpdate').removeAttr("disabled");
        else $('#fieldSetUpdate').attr("disabled", "disabled");
        $('#fieldSetUpdate :input:first').focus();
    })
});
function SubmitForm(form) {
    $.validator.unobtrusive.parse(form);
    if ($(form).valid() == true) {
        $.ajax({
            type: "PUT",
            url: form.action,
            data: $(form).serialize(),
            success: function (data) {
                if (data.success) {
                    $('#form-modal').modal('hide');
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "success"
                    })
                    setTimeout(function () {
                        location.reload();
                    }, 1200);
                } else {
                    $.notify(data.message, {
                        globalPosition: "top center",
                        className: "error"
                    })
                }
            },
        });
    }
    return false;
}
showInPopup = (url, title) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');
        }
    })
}
