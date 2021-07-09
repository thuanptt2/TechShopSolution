window.onload = function () {
    if ($('#txtTotal').val() >= 2000000) {
        document.getElementById("txtTransportFee").value = 0;
        $('#transport_fee').text("Miễn phí");
        var newTotal = $('#txtTotal').val() - $('#txtDiscount').val();
        $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(newTotal));
    }
    else {
        var address = document.getElementById("txtAddressReceiver").value;
        if (address != "") {
            var res = address.split(",");
            if (res[res.length - 1] === " TP Hồ Chí Minh") {
                document.getElementById("txtTransportFee").value = 0;
                $('#transport_fee').text("Miễn phí");
                var newTotal = $('#txtTotal').val() - $('#txtDiscount').val();
                $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(newTotal));
            }
            else {
                document.getElementById("txtTransportFee").value = 50000;
                var newTotal = $('#txtTotal').val() - $('#txtDiscount').val() + 50000;
                $('#transport_fee').text("50.000");
                $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(newTotal));
            }
        };
    }
};
$("#check2").click(function () {
    if (this.checked) {
        $("#address").hide();
    } else
        $("#address").show();
})
$("#check1").click(function () {
    $("#address").show();
    $('#ddlDistrict option:first').prop('selected', true);
    $('#ddlCity option:first').prop('selected', true);
    $('#ddlWard option:first').prop('selected', true);
    if ($('#txtTotal').val() >= 2000000) {
        document.getElementById("txtTransportFee").value = 0;
        $('#transport_fee').text("Miễn phí");
        var newTotal = $('#txtTotal').val() - $('#txtDiscount').val();
        $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(newTotal));
    }
    else {
        var address = document.getElementById("txtAddressReceiver").value;
        if (address != "") {
            var res = address.split(",");
            if (res[res.length - 1] === " TP Hồ Chí Minh") {
                document.getElementById("txtTransportFee").value = 0;
                $('#transport_fee').text("Miễn phí");
                var newTotal = $('#txtTotal').val() - $('#txtDiscount').val();
                $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(newTotal));
            }
            else {
                document.getElementById("txtTransportFee").value = 50000;
                var newTotal = $('#txtTotal').val() - $('#txtDiscount').val() + 50000;
                $('#transport_fee').text("50.000");
                $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(newTotal));
            }
        };
    }
});
$(document).on('change', '#ddlCity', function () {
    if ($('#txtTotal').val() < 2000000) {
        var city = $("#ddlCity option:selected").text();
        if (city === "TP Hồ Chí Minh") {
            document.getElementById("txtTransportFee").value = 0;
            $('#transport_fee').text("Miễn phí");
            var newTotal = $('#txtTotal').val() - $('#txtDiscount').val();
            $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(newTotal));
        }
        else {
            document.getElementById("txtTransportFee").value = 50000;
            var newTotal = $('#txtTotal').val() - $('#txtDiscount').val() + 50000;
            $('#transport_fee').text("50.000");
            $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(newTotal));
        }
    } else {
        document.getElementById("txtTransportFee").value = 0;
        $('#transport_fee').text("Miễn phí");
        var newTotal = $('#txtTotal').val() - $('#txtDiscount').val();
        $('#lbl_maintotal').text(new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(newTotal));
    }
});
$("#btnSubmit").click(function () {
    var radio = document.getElementById('check2');
    if (radio.checked) {
        if ($("#txtAddressChoose")) {
            if ($("#ddlCity").val() != "" && $("#ddlDistrict").val() != "" && $("#ddlWard").val() != "" && $("#txtHomeAddress").val() != "") {
                $("#loaderbody").removeClass('hide');
                var city = $("#ddlCity option:selected").text();
                var district = $("#ddlDistrict option:selected").text();
                var ward = $("#ddlWard option:selected").text();
                var address = $("#txtHomeAddress").val() + ", " + ward + ", " + district + ", " + city;
                document.getElementById("txtAddressReceiver").value = address;
                document.getElementById("checkoutForm").submit();
            }
            else {
                $("#messageCheckout").text("Vui lòng chọn đầy đủ thông tin địa chỉ");
                alert("Vui lòng chọn đầy đủ thông tin địa chỉ");
            }
        }
    }
    else {
        $("#loaderbody").removeClass('hide');
        document.getElementById("txtAddressReceiver").value = $("#txtAddressDefault").val();
        $("#loaderbody").removeClass('hide');
        document.getElementById("checkoutForm").submit();
    }
});
$(document).on('change', '.radio-button', function () {

    const radio = $(this);

    if (radio.is(':checked')) {
        document.getElementById("PaymentDecription").innerHTML = document.getElementById("decription-" + radio.val()).innerHTML;
    }
});
