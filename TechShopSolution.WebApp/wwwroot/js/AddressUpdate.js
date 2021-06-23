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
                    $.notify("Cập nhật thất bại", {
                        globalPosition: "top center",
                        className: "error"
                    })
                }
            },
        });
    }
    return false;
}
showInPopup = (url) => {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $('#formAddress').html(res);
        }
    })
}
$('#btnEditInfo').on('click', function () {
    if ($('#formAddress').hasClass('hide'))
        $('#formAddress').removeClass('hide');
    else $('#formAddress').addClass('hide')
})