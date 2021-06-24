var CartController = function () {
    this.initialize = function () {

    }
    function loadData() {
         $.ajax({
            type: "POST",
            url: '/Cart/GetListItem/',
            success: function (res) {
                console.log(res);
            },
        })
    }
}