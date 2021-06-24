
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
