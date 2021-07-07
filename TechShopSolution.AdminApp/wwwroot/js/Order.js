$().ready(function () {
    $(function () {
        $("#loaderbody").addClass('hide');

        $(document).bind('ajaxStart', function () {
            $("#loaderbody").removeClass('hide');
        }).bind('ajaxStop', function () {
            $("#loaderbody").addClass('hide');
        });
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
})
