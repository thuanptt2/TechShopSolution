
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
                if ($(this).parent().children()[0].getAttribute("src") == key)
                    formData.delete(key);
            }
            $(this).parent().remove();
            if (!$("#ProductImages").children(".col-lg-3").length) {
                $("#NonImageProduct").show();
            }

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
                    success: function (data) {
                        $("#MoreImages").val(data);
                    }
                }
            );
        });

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
                success: function (data) {
                    $("#MoreImages").val(data);
                }
            }
        );
    });
   
})

    