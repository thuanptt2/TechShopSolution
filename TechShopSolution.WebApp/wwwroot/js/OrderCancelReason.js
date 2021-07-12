$().ready(function () {
    function SubmitForm(form) {
        $.ajax({
            type: "PUT",
            url: form.action,
            data: $(form).serialize(),
            success: function (data) {
                if (data.success) {
                    $('#form-modal').modal('hide');
                } else {

                }
            },
        });
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
    };
})