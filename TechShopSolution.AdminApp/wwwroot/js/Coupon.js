$(document).ready(function () {
    var today = new Date();
    $('.datepicker').datepicker({
        autoclose: true,
        endDate: today,
        minDate: 0
    }).on('changeDate', function (ev) {
        $(this).datepicker('hide');
    });
    $.datepicker.setDefaults({
        dateFormat: 'dd-mm-yy'
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
});
