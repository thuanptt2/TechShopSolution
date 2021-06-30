
var SiteController = function () {
    this.initialize = function () {
        regsiterEvents();
        loadCart();
    }
    function loadCart() {
        $.ajax({
            type: "GET",
            url: "/Cart/GetListItems",
            success: function (res) {
                $('#lbl_number_items_header').text(res.items.length);
            }
        });
    }
    function regsiterEvents() {
        // Write your JavaScript code.
        $('body').on('click', '.btn-add-cart', function (e) {
            e.preventDefault();
            const idProduct = $(this).data('id');
            $.ajax({
                type: "POST",
                url: '/Cart/AddToCart/' + idProduct,
                success: function (res) {
                    $('#lbl_number_items_header').text(res.items.length);
                    var x = document.getElementById("snackbar");
                    $('.ReultMessage').text("Thêm vào giỏ hàng thành công");

                    // Add the "show" class to DIV
                    x.className = "show";

                    // After 3 seconds, remove the show class from DIV
                    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    $("#snackbar").addClass("snackbarDanger");
                    var x = document.getElementById("snackbar");
                    $('.ReultMessage').text(jqXHR.responseText);

                    // Add the "show" class to DIV
                    x.className = "show";

                    // After 3 seconds, remove the show class from DIV
                    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
                }
            })
        });
        $('body').on('click', '.btn-buy-now', function (e) {
            e.preventDefault();
            const idProduct = $(this).data('id');
            $.ajax({
                type: "POST",
                url: '/Cart/AddToCart/' + idProduct,
                success: function (res) {
                    $('#lbl_number_items_header').text(res.items.length);
                    window.location = "/gio-hang";
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    window.location = "/gio-hang";

                    $("#snackbar").addClass("snackbarDanger");

                    var x = document.getElementById("snackbar");
                    $('.ReultMessage').text(jqXHR.responseText);

                    // Add the "show" class to DIV
                    x.className = "show";

                    // After 3 seconds, remove the show class from DIV
                    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
                }
            })
        });
    }
    
}
