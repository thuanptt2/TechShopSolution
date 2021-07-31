$(document).ready(function () {
    var today = new Date();
    $('.datepicker').datepicker({
        autoclose: true,
        endDate: today,
        dateFormat: 'dd/mm/yy',
        minDate: 0
    }).on('changeDate', function (ev) {
        $(this).datepicker('hide');
    });

    $('.datepicker').keyup(function () {
        if (this.value.match(/[^0-9]/g)) {
            this.value = this.value.replace(/[^0-9^-]/g, '');
        }
    });
    $('#checkMinValue').click(function () {
        if (!$(this).is(':checked')) {
            $("#txtMinValue").val(null);
        }
    });
    $('#checkQuantity').click(function () {
        if (!$(this).is(':checked')) {
            $("#txtQuantity").val(null);
        }
    });
    $('#checkMaxValue').click(function () {
        if (!$(this).is(':checked')) {
            $("#txtMaxValue").val(null);
        }
    });

    new AutoNumeric('#txtMinValue', {
        allowDecimalPadding: false,
        currencySymbol: "₫",
        currencySymbolPlacement: "s",
        digitGroupSeparator: " ",
        maximumValue: "9999999999",
        minimumValue: "0"
    });
    new AutoNumeric('#txtMaxValue', {
        allowDecimalPadding: false,
        currencySymbol: "₫",
        currencySymbolPlacement: "s",
        digitGroupSeparator: " ",
        maximumValue: "9999999999",
        minimumValue: "0"
    });

    $("#btn-save").click(function () {

        var form = $("#CreateForm");

        var min_order_value = $("#txtMinValue").val();
        var max_value = $("#txtMaxValue").val();

        if (min_order_value.length != 0 && min_order_value != "") {
            min_order_value = min_order_value.replace(/(₫)/gm, "");
            min_order_value = min_order_value.replace(/\s/g, "");
            document.getElementById("txtMinValue").value = min_order_value;
        }
        if (max_value.length != 0 && max_value != "") {
            max_value = max_value.replace(/(₫)/gm, "");
            max_value = max_value.replace(/\s/g, "");
            document.getElementById("txtMaxValue").value = max_value;
        }

        form.submit();
    });
    $("#typeCoupon").change(function () {
        if ($('#typeCoupon option:selected').text() == "Giảm theo số tiền") {
            $("#Max-value-area").hide();
        }
        else $("#Max-value-area").show();
    });

});
