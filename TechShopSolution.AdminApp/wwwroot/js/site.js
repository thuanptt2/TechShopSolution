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
    if ($(form).valid()) {
        $.ajax({
            type: "PUT",
            url: form.action,
            data: $(form).serialize(),
            success: function (data) {
                if (data.success) {
                    $('#form-modal').modal('hide');
                    location.reload();

                } else {
                    alert(data.message);
                }
            },
        });
    }
    return false;
}
