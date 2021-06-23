var customer = {
    init: function () {

        customer.loadProvince();
        customer.registerEvent();
    },
    registerEvent: function () {
        $('#ddlCity').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                customer.loadDistrict(parseInt(id));
                $('#ddlWard').html('');
                $('#txtCity').val($("#ddlCity option:selected").text());
            }
            else {
                $('#ddlDistrict').html('');
                $('#ddlWard').html('');
            }
        });
        $('#ddlDistrict').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                customer.loadWard(parseInt(id));
                $('#txtDistrict').val($("#ddlDistrict option:selected").text());
            }
            else {
                $('#ddlWard').html('');
            }
        });
        $('#ddlWard').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                $('#txtWard').val($("#ddlWard option:selected").text());
            }
        });
    },
    loadProvince: function () {

        $.ajax({
            url: '/Account/LoadProvince',
            type: "POST",
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn tỉnh thành--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.id + '">' + item.name + '</option>'
                    });
                    $('#ddlCity').html(html);
                }
            }
        })
    },
    loadDistrict: function (id) {
        $.ajax({
            url: '/Account/LoadDistrict',
            type: "POST",
            data: { provinceID: id },
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn quận huyện--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.id + '">' + item.name + '</option>'
                    });
                    $('#ddlDistrict').html(html);
                }
            }
        })
    },
    loadWard: function (id) {
        $.ajax({
            url: '/Account/LoadWard',
            type: "POST",
            data: { districtID: id },
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn phường xã--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.id + '">' + item.name + '</option>'
                    });
                    $('#ddlWard').html(html);
                }
            }
        })
    },
}
customer.init();
