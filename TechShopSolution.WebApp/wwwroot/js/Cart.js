var CartController = function () {
    this.initialize = function () {
        loadData();

    }
   
    function loadData() {
         $.ajax({
            type: "GET",
             url: '/Cart/GetListItems/',
            success: function (res) {
                var html = '';
                var total = 0;

                $.each(res, function (i, item) {
                    if (item.promotionPrice > 0) {
                        var amount = item.promotionPrice * item.quantity;
                        var promotion = (item.price - item.promotionPrice) * item.quantity;
                    }
                    else {
                        var amount = item.price * item.quantity;
                        var promotion = 0;
                    }
                    html += "<tr>"
                        + "<td> <img width=\"60\" height=\"60\" src=\"data:image/jpeg;base64," + item.images + "\" alt=\"\" /></td>"
                        + "<td class='cart-item-name'><a href=\/san-pham\/" + item.slug + ">" + item.name + "\"</a></td>"
                        + "<td><div class=\"input-append\"><input class=\"span1\" style=\"max-width: 34px\" placeholder=\"1\" id=\"txt_quantity_" + item.id + "\" value=\"" + item.quantity + "\" size=\"16\" type=\"text\">"
                        + "<button class=\"btn btn-minus\" data-id=\"" + item.id + "\" type =\"button\"> <i class=\"icon-minus\"></i></button>"
                        + "<button class=\"btn btn-plus\" type=\"button\" data-id=\"" + item.id + "\"><i class=\"icon-plus\"></i></button>"
                        + "<button class=\"btn btn-danger btn-remove\" type=\"button\" data-id=\"" + item.id + "\"><i class=\"icon-remove icon-white\"></i></button>"
                        + "</div>"
                        + "</td>"

                        + "<td>" + new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(item.price) + "</td>"
                        + "<td>" + new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(promotion) + "</td>"
                        + "<td>" + new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(amount) + "</td>"
                        + "</tr>";
                    total += amount;
                });
                $('#cart_body').html(html);
                $('#lbl_number_of_items').text(res.length);
                $('#lbl_total').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total));
                $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total));
            },
        })
    }
}