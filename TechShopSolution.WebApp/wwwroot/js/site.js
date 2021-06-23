$(function () {
    $("#loaderbody").addClass('hide');

    $(document).bind('ajaxStart', function () {
        $("#loaderbody").removeClass('hide');
    }).bind('ajaxStop', function () {
        $("#loaderbody").addClass('hide');
    });
});

$('body').on('click', '.btn-add-cart', function (e) {
    e.preventDefault();
    const idProduct = $(this).data('id');
    $.ajax({
        type: "POST",
        url: '/Cart/AddToCart/' + idProduct,
        success: function (res) {
            console.log(res);
        },
    })
})