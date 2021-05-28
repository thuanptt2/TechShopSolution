var customer = {
    init: function () {

        customer.loadProvince();
        customer.registerEvent();
    },
    registerEvent: function () {
        $('#ddlCity').off('change').on('change', function () {
            var name = $(this).val();
            if (name != '') {
                customer.loadDistrict(name);
            }
            else {
                $('#ddlDistrict').html('');
            }
        });
        $('#ddlDistrict').off('change').on('change', function () {
            var name = $(this).val();
            if (name != '') {
                customer.loadWard(name);
            }
            else {
                $('#ddlWard').html('');
            }
        });
    },
    loadProvince: function () {

        $.ajax({
            url: '/customer/LoadProvince',
            type: "POST",
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn tỉnh thành--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.name + '">' + item.name + '</option>'
                    });
                    $('#ddlCity').html(html);
                }
            }
        })
    },
    loadDistrict: function (Name) {
        $.ajax({
            url: '/customer/LoadDistrict',
            type: "POST",
            data: { provinceName: Name },
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn quận huyện--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.name + '">' + item.name + '</option>'
                    });
                    $('#ddlDistrict').html(html);
                }
            }
        })
    },
    loadWard: function (Name) {
        $.ajax({
            url: '/customer/LoadWard',
            type: "POST",
            data: { districtName: Name },
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">--Chọn phường xã--</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.name + '">' + item.name + '</option>'
                    });
                    $('#ddlWard').html(html);
                }
            }
        })
    }
}
customer.init();