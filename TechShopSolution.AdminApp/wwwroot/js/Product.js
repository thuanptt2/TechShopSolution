
$().ready(function () {
    var formData = new FormData();

    $().ready(function () {
        $("#ProductImageInput").change(function (event) {
            var x = URL.createObjectURL(event.target.files[0]);
            $("#ProductImage").attr("src", x);
            console.log(event);
        })
    });

    $("#ProductMoreImagesInput").change(function () {
        var input = document.getElementById("ProductMoreImagesInput");
        var files = input.files;

        $("#NonImageProduct").hide();
        for (var i = 0; i != files.length; i++) {
            var x = (window.URL || window.webkitURL).createObjectURL(files[i]);
            $("#ProductImages").append(' <div class="col-lg-3 col-3 col-sm-3 mb-3"><img src="' + x + '" class="ProductMoreImage"/><div class="middle"><i class="fas fa-trash fa-2x" id="btnRemoveImage"></i></div></div>');
            formData.append(x, files[i]);
        }
        $(".middle").click(function (e) {
            e.preventDefault();
            for (var key of formData.keys()) {
                if ($(this).parent().children()[0].getAttribute("src") == key) {
                    formData.delete(key);
                    flag = true;
                    $(this).parent().remove();
                    if (!$("#ProductImages").children(".col-lg-3").length) {
                        $("#NonImageProduct").show();
                    }
                    break;
                }
            }
        });

    });
    $("#btnSave").click(function () {
        var form = $("#CreateForm");
        $.validator.unobtrusive.parse(form);
        if (form.valid()) {
            listImages = new FormData();
            for (var key of formData.keys()) {
                listImages.append("files", formData.get(key));
            }
            if (document.getElementById('checkInstock').checked == false) {
                document.getElementById("txtInstock").value = null;
            }
            $.ajax(
                {
                    url: "/Product/sendListMoreImage",
                    data: listImages,
                    processData: false,
                    contentType: false,
                    type: "POST",
                    success: function (data) {
                        form.submit();
                    }
                }
            );
        }
        else {
            alert("Vui lòng điền đầy đủ thông tin");
        }
    })
    $(".DeleteImageButton").click(function (e) {
        if (confirm("Bạn có chắc muốn xóa hình ảnh này, hành động này không thể hoàn tác ?")) {
            e.preventDefault();
            var image = $(this).parent();
            var fileName = $(this).parent().children()[0].getAttribute("data-name");
            var id = $(this).parent().children()[0].getAttribute("data-id");
            $.ajax(
                {
                    url: "/Product/DeleteImage",
                    data: { 'id': id, 'fileName': fileName },
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        if (data.success) {
                            image.remove();
                            if (!$("#ProductImages").children(".col-lg-3").length) {
                                $("#NonImageProduct").show();
                            }
                        }
                        else {
                            $("#snackbar").css("background-color", "Red");
                            $("#snackbar").css("color", "#fff");
                        }
                        $("#messageNotification").html(data.message);
                        var x = document.getElementById("snackbar");

                        // Add the "show" class to DIV
                        x.className = "show";

                        // After 3 seconds, remove the show class from DIV
                        setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
                    }
                }
            );
        }
    });


    ClassicEditor
        .create(document.querySelector('#txtSpecs'), {
            // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
        })
        .then(editor => {
            window.editor = editor;
        })
        .catch(err => {
            console.error(err.stack);
        });
    ClassicEditor
        .create(document.querySelector('#txtDesc'), {
            // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
        })
        .then(editor => {
            window.editor = editor;
        })
        .catch(err => {
            console.error(err.stack);
        });
    ClassicEditor
        .create(document.querySelector('#txtShortDesc'), {
            // toolbar: [ 'heading', '|', 'bold', 'italic', 'link' ]
        })
        .then(editor => {
            window.editor = editor;
        })
        .catch(err => {
            console.error(err.stack);
        });

    
    $('#cboCategory').change(function () {
        var CateID = $('#txtCateID').val();
        var value = $(this).val();
        var cateName = $("#cboCategory option:selected").text();
        var cateID = $("#cboCategory option:selected").val();
        if (CateID.length == 0) {
            $(".CateTag").append('<span class="badge badge-pill badge-info badge-cate-tag"  data-id="' + cateID + '">' + cateName + '<span class="deleteCateTag">x</span></span>');
            $('#txtCateID').val(value + "," + CateID);
        }
        else {
            if (cateID != "") {
                var flag = true;
                var arr = CateID.split(",");
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i] != "")
                        if (arr[i] == value)
                            flag = false;
                }
                if (flag == true) {
                    $(".CateTag").append('<span class="badge badge-pill badge-info badge-cate-tag" data-id="' + cateID + '">' + cateName + '<span class="deleteCateTag">x</span></span>');
                    $('#txtCateID').val(value + "," + CateID);
                }
            }
        }
        $('.badge-cate-tag').click(function () {
            var flag = false;
            var id = $(this).attr("data-id");
            var CateID = $('#txtCateID').val();
            var arr = CateID.split(",");
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] != "")
                    if (arr[i] == id) {
                        arr.splice(i, 1);
                        flag = true;
                        break;
                    }
            }
            if (flag == true) {
                var string = ""
                for (var j = 0; j < arr.length; j++) {
                    if (arr[j] != "")
                        string += arr[j] + ",";
                }
                $('#txtCateID').val(string);
                $(this).remove();
            }
        })

    });
    $(".OldCate").click(function () {
        var flag = false;
        var id = $(this).attr("data-id");
        var CateID = $('#txtCateID').val();
        var arr = CateID.split(",");
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] != "")
                if (arr[i] == id) {
                    arr.splice(i, 1);
                    flag = true;
                    break;
                }
        }
        if (flag == true) {
            var string = ""
            for (var j = 0; j < arr.length; j++) {
                if (arr[j] != "")
                    string += arr[j] + ",";
            }
            $('#txtCateID').val(string);
            $(this).remove();
        }
    })

    $('#cboBrand').change(function () {
        var value = $(this).val();
        if (!isNaN(value)) {
            $('#txtBrandID').val(value);
        }
    })
})
