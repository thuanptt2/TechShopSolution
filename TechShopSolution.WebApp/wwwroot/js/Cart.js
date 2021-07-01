var CartController = function () {
    this.initialize = function () {
        loadData();

        registerEvents();
    }

    function registerEvents() {
        $('body').on('click', '.btn-plus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const instock = $('#txt_quantity_' + id).data('instock');
            const quantity = parseInt($('#txt_quantity_' + id).val()) + 1;
            if (instock != null) {
                if (quantity > instock) {
                    alert("Sản phẩm này chỉ còn " + instock + " cái, quý khách chỉ được mua tối đa " + instock + " sản phẩm.")
                    $(this).val(instock);
                    updateCart(id, instock);
                }
                else if (quantity > 5) {
                    alert("Bạn chỉ được mua tối đa 5 sản phẩm");
                } else {
                    updateCart(id, quantity);
                }
            } else {
                if (quantity > 5) {
                    alert("Bạn chỉ được mua tối đa 5 sản phẩm");
                }
                else updateCart(id, quantity);
            }
        });

        $('body').on('click', '.btn-minus', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('#txt_quantity_' + id).val()) - 1;
            if (quantity == 0) {
                if (confirm("Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?"))
                    updateCart(id, quantity);
            }
            else {
                updateCart(id, quantity);
            }
        });

        $('body').on('click', '.btn-remove', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            if (confirm("Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?"))
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
            } else if (isNaN(quantity)) {
                alert("Số lượng phải là chữ số, mời quý khách nhập lại");
                $(this).val($(this).data('count'));
            } else if (instock != null) {
                if (quantity > 5 && quantity < instock) {
                    alert("Bạn chỉ được mua tối đa 5 sản phẩm");
                    $(idInput).val(5);
                    updateCart(id, 5);
                } else if (quantity > instock) {
                    alert("Sản phẩm này chỉ còn " + instock + " cái, quý khách chỉ được mua tối đa " + instock + " sản phẩm.")
                    $(this).val(instock);
                    updateCart(id, instock);
                }
                else updateCart(id, quantity);


            }
            else if (quantity > 5) {
                alert("Bạn chỉ được mua tối đa 5 sản phẩm");
                $(idInput).val(5);
                updateCart(id, 5);
            }
            else updateCart(id, quantity);
        });

        $('body').on('click', '#btnApplyCoupon', function () {
            const code = $("#codeCoupon").val();
            if (code != "")
                useCoupon(code);
        });

    }
    function useCoupon(code) {
        $.ajax({
            type: "POST",
            url: '/Cart/UseCoupon',
            data: {
                code: code
            },
            success: function (res) {
                var total = 0;
                var couponPrice = 0;

                $.each(res.items, function (i, item) {
                    if (item.promotionPrice > 0) {
                        var amount = item.promotionPrice * item.quantity;
                        var promotion = (item.price - item.promotionPrice) * item.quantity;
                    }
                    else {
                        var amount = item.price * item.quantity;
                        var promotion = 0;
                    }
                    total += amount;
                });
                if (res.coupon.type == "Phần trăm")
                    couponPrice = total * (res.coupon.value / 100);
                else if (res.coupon.type == "Số tiền")
                    couponPrice = total - res.coupon.value;

                $('#lbl_couponprice').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total - couponPrice));

                $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(couponPrice));


                var x = document.getElementById("snackbar");
                $('.ReultMessage').text("Sử dụng mã giảm giá thành công");

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
    function updateCart(id, quantity) {
        $.ajax({
            type: "POST",
            url: '/Cart/UpdateCart',
            data: {
                id: id,
                quantity: quantity
            },
            success: function (res) {
                $('#lbl_number_items_header').text(res.items.length);
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
                if (res.items.length === 0) {
                    $('.table-cart').hide();
                }
                var html = '';
                var total = 0;

                $.each(res.items, function (i, item) {
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
                if (res.coupon != null) {
                    if (res.coupon.type == "Phần trăm")
                        couponPrice = total * (res.coupon.value / 100);
                    else if (res.coupon.type == "Số tiền")
                        couponPrice = total - res.coupon.value;

                    $('#lbl_couponprice').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total - couponPrice));
                    $('#lbl_total').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total));
                    $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(couponPrice));
                    $('#cart_body').html(html);
                    $('#lbl_number_of_items').text(res.items.length);
                    $('#codeCoupon').val(res.coupon.code);

                }
                else {
                    $('#cart_body').html(html);
                    $('#lbl_number_of_items').text(res.items.length);
                    $('#lbl_total').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total));
                    $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total));
                }
            },
        })
    }
}