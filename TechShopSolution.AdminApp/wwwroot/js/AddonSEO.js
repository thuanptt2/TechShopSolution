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
    $('.collection-seo--preview-url').text("http://techshopvn.xyz/san-pham/" + content);
})

$('#txtMetaTitle').on("keyup change", function () {
    var content = $('#txtMetaTitle').val();
    var contentDesc = $('#txtMetaDesc').val();
    if (!content || !content.trim()) {
        $(".collection-seo--preview").hide();
        $("#text-seo-sug").removeClass("hide");
    }
    else {
        if (contentDesc && contentDesc.trim()) {
            $(".collection-seo--preview").show();
            $("#text-seo-sug").addClass("hide");
            $('.collection-seo--preview-title').text(content);
            $('.collection-seo--preview-mota').text($("txtMetaDesc").val());
        }
    }
});

$('#txtMetaDesc').on("keyup change", function () {
    var content = $('#txtMetaDesc').val();
    var contentTitle = $('#txtMetaDesc').val();
    if (!content || !content.trim()) {
        $(".collection-seo--preview").hide();
        $("#text-seo-sug").removeClass("hide");
    }
    else {
        if (contentTitle && contentTitle.trim()) {
            $(".collection-seo--preview").show();
            $("#text-seo-sug").addClass("hide");
            $('.collection-seo--preview-mota').text(content);
            $('.collection-seo--preview-title').text($("#txtMetaTitle").val());
        }
    }
});

$('#txtSlug').on("keyup change", function () {
    var content = $('#txtSlug').val();
    $('.collection-seo--preview-url').text("http://techshopvn.xyz/san-pham/" + content);
});
