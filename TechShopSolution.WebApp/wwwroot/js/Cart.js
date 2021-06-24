var CartController = function () {
    this.initialize = function () {
        loadData();

        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-plus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) + 1;
            if (quantity > 100) {
                alert("Bạn muốn mua số lượng lớn đấy, hãy liên hệ với chúng tôi để nhận được ưu đãi nhé!");
            } else {
                updateCart(id, quantity);
            }
        });

        $('body').on('click', '.btn-minus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) - 1;
            updateCart(id, quantity);
        });

        $('body').on('click', '.btn-remove', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            updateCart(id, 0);
        });

        $('body').on('change', '.txtQuantity', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const instock = $(this).data('instock');
            var idInput = '#txt_quantity_' + id;
            const quantity = parseInt($(this).val());
            if (quantity < 0) {
                alert("Số lượng sản phẩm phải lớn hơn 0, mời quý khách nhập lại");
                $(this).val($(this).data('count'));
            } else if (isNaN(quantity))
            {
                alert("Số lượng phải là chữ số, mời quý khách nhập lại");
                $(this).val($(this).data('count'));
            } else if (instock != null)
            {
                if (quantity > instock) {
                    alert("Sản phẩm này chỉ còn " + instock + " sản phẩm, quý khách chỉ được mua tối đa " + instock + " sản phẩm.")
                    $(this).val(instock);
                    updateCart(id, instock);
                }
            }
            else if (quantity > 100) {
                alert("Bạn muốn mua số lượng lớn đấy, hãy liên hệ với chúng tôi để nhận được ưu đãi nhé!");
                $(idInput).val(100);
                updateCart(id, 100);
            }
            else updateCart(id, quantity);
        });

    }

    function updateCart(id, quantity) {
        $.ajax({
            type: "POST",
            url: '/Cart/UpdateCart',
            data: {
                id: id,
                quantity: quantity
            },
            success: function (res) {
                $('#lbl_number_items_header').text(res.length);
                loadData();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }

    function loadData() {
         $.ajax({
            type: "GET",
             url: '/Cart/GetListItems/',
             success: function (res) {
                 if (res.length === 0) {
                     $('.table-cart').hide();
                 }
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
                        + "<td><div class=\"input-append\"><input class=\"span1 txtQuantity\" style=\"max-width: 34px\" data-id=\"" + item.id + "\" placeholder=\"1\" id=\"txt_quantity_" + item.id + "\" data-count=\"" + item.quantity + "\" value=\"" + item.quantity + "\"  data-instock=\"" + item.instock + "\" size=\"16\" type=\"text\">"
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