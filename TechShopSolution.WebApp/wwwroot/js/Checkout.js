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
});
$("#btnSubmit").click(function () {
    if ($("#check2").attr("checked") == "checked") {
        if ($("#txtAddressChoose")) {
            if ($("#ddlCity").val() != "" && $("#ddlDistrict").val() != "" && $("#ddlWard").val() != "" && $("#txtHomeAddress").val() != "") {
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
        document.getElementById("txtAddressReceiver").value = $("#txtAddressDefault").val();
        document.getElementById("checkoutForm").submit();
    }
});
$(document).on('change', '.radio-button', function () {

    const radio = $(this);

    if (radio.is(':checked')) {
        document.getElementById("PaymentDecription").innerHTML = document.getElementById("decription-" + radio.val()).innerHTML;
    }

});