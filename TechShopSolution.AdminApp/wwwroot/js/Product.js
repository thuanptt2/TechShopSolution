
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
            var flag = false;
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
            if (flag == false) {
                var fileName = $(this).parent().children()[0].getAttribute("data-name")
                $.ajax(
                    {
                        url: "/Product/DeleteImage",
                        data: fileName,
                        processData: false,
                        contentType: false,
                        type: "POST",
                        success: function (data) {
                            $(this).parent().remove();
                            if (!$("#ProductImages").children(".col-lg-3").length) {
                                $("#NonImageProduct").show();
                            }
                        }
                    }
                );
            }
        });

        $("#btnSave").click(function () {
            var form = $("#CreateForm");
            $.validator.unobtrusive.parse(form);
            if (form.valid()) {
                listImages = new FormData();
                for (var key of formData.keys()) {
                    listImages.append("files", formData.get(key));
                }
                $.ajax(
                    {
                        url: "/Product/sendListMoreImage",
                        data: listImages,
                        processData: false,
                        contentType: false,
                        type: "POST",
                    }
                );
            }
        })
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


    function to_slug(str) {
        // Chuyển hết sang chữ thường
        str = str.toLowerCase();

        // xóa dấu
        str = str.replace(/(à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ)/g, 'a');
        str = str.replace(/(è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ)/g, 'e');
        str = str.replace(/(ì|í|ị|ỉ|ĩ)/g, 'i');
        str = str.replace(/(ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ)/g, 'o');
        str = str.replace(/(ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ)/g, 'u');
        str = str.replace(/(ỳ|ý|ỵ|ỷ|ỹ)/g, 'y');
        str = str.replace(/(đ)/g, 'd');

        // Xóa ký tự đặc biệt
        str = str.replace(/([^0-9a-z-\s])/g, '');

        // Xóa khoảng trắng thay bằng ký tự -
        str = str.replace(/(\s+)/g, '-');

        // xóa phần dự - ở đầu
        str = str.replace(/^-+/g, '');

        // xóa phần dư - ở cuối
        str = str.replace(/-+$/g, '');

        // return
        return str;
    }
    $('#txtName').on("keyup change", function () {
        var content = to_slug($('#txtName').val());
        $('#txtSlug').val(content);
    })
})

//$("#ProductMoreImagesInput").change(function () {
//    var input = document.getElementById("ProductMoreImagesInput");
//    var files = input.files;

//    $("#NonImageProduct").hide();
//    for (var i = 0; i != files.length; i++) {
//        var x = (window.URL || window.webkitURL).createObjectURL(files[i]);
//        $("#ProductImages").append(' <div class="col-lg-3 col-3 col-sm-3 mb-3"><img src="' + x + '" class="ProductMoreImage"/><div class="middle"><i class="fas fa-trash fa-2x" id="btnRemoveImage"></i></div></div>');
//        formData.append(x, files[i]);
//    }
//    $(".middle").click(function (e) {
//        e.preventDefault();
//        for (var key of formData.keys()) {
//            if ($(this).parent().children()[0].getAttribute("src") == key) {
//                formData.delete(key);
//                break;
//            }
//        }
//        $(this).parent().remove();
//        if (!$("#ProductImages").children(".col-lg-3").length) {
//            $("#NonImageProduct").show();
//        }

//        listImages = new FormData();
//        for (var key of formData.keys()) {
//            listImages.append("files", formData.get(key));
//        }
//        $.ajax(
//            {
//                url: "/Product/sendListMoreImage",
//                data: listImages,
//                processData: false,
//                contentType: false,
//                type: "POST",
//                success: function (data) {
//                    $("#MoreImages").val(data);
//                }
//            }
//        );
//    });

//    listImages = new FormData();
//    for (var key of formData.keys()) {
//        listImages.append("files", formData.get(key));
//    }
//    $.ajax(
//        {
//            url: "/Product/sendListMoreImage",
//            data: listImages,
//            processData: false,
//            contentType: false,
//            type: "POST",
//            success: function (data) {
//                $("#MoreImages").val(data);
//            }
//        }
//    );
//});
