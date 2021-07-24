
$(".unfavorite").click(function () {
    if (confirm('Quý khách có chắc muốn hủy yêu thích sản phẩm này?')) {
        const cus_id = $(this).data('cusid');
        const product_id = $(this).data('productid');
        $.ajax({
            type: "POST",
            url: '/Product/UnFavoriteProduct/',
            data: {
                cus_id: parseInt(cus_id), product_id: parseInt(product_id),
            },
            success: function (res) {
                var product = document.getElementById(product_id);

                product.remove();

                var x = document.getElementById("snackbar");

                $('.ReultMessage').text("Hủy yêu thích thành công");

                // Add the "show" class to DIV
                x.className = "show";

                // After 3 seconds, remove the show class from DIV
                setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var x = document.getElementById("snackbarDanger");
                $('#ErrorMessage').text(jqXHR.responseText);

                // Add the "show" class to DIV
                x.className = "show";

                // After 3 seconds, remove the show class from DIV
                setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
            }
        });
    }
    return false;
})