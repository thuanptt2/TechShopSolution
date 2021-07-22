var rating = $('.rating-histogram');
var totalVotes = 0;
var secondaryVotes = 0;
var totalVotesInc = 0;
var averageScore = 0;
var array = [];
var highest = 0;

$('.rating-bar-container').each(function () {
    $(this).attr('valuemax', Number($(this).children('.bar-label').html()) * Number($(this).attr('valuenow')));
    totalVotes += Number($(this).attr('valuenow'));
    array.push(Number($(this).attr('valuenow')));
    highest = Math.max.apply(Math, array);
    totalVotesInc += Number($(this).children('.bar-label').html()) * Number($(this).attr('valuenow'))
    if ($(this).attr('valuenow') == highest) {
        $(this).children('.bar').width('100%');
    } else {
        secondaryVotes += Number($(this).attr('valuenow'));
    }
    $(this).children('.bar-number').html(Number($(this).attr('valuenow')).toLocaleString('ru'));
});
averageScore = (totalVotesInc / totalVotes).toFixed(1).toString().replace(".", ",");
rating.attr({
    'valuemax': totalVotes,
    'secvaluemax': secondaryVotes
});
$('.rating-bar-container').each(function () {
    if ($(this).attr('valuenow') == highest) {
        $(this).children('.bar').width('100%');
    } else {
        // console.log(1);
        console.log($(this).attr('valuenow'));
        $(this).children('.bar').width($(this).attr('valuenow') * 200 / rating.attr('valuemax') + '%');
    }
});
if (isNaN(parseFloat(averageScore))) {
    averageScore = 0;
}
$('.reviews-num').html(' ' + Number(rating.attr('valuemax')).toLocaleString('ru'));
$('.score-container .score').html(averageScore);
$('#rating-score-number').html(averageScore + "/5,0");
$('#rating-vote-times').html(" (" + totalVotes + " lượt)");
const starScore = document.getElementById("rating-score-star");
if (averageScore == 0) {
    starScore.style.setProperty("--rating", 0);
} else {
    starScore.style.setProperty("--rating", parseFloat(averageScore.replace(",", ".")));
}

$("#btnFavoriteProduct").click(function () {
    const cus_id = $(this).data('cusid');
    const product_id = $(this).data('productid');
    $.ajax({
        type: "POST",
        url: '/Product/FavoriteProduct/',
        data: {
            cus_id: parseInt(cus_id), product_id: parseInt(product_id),
        },
        success: function (res) {

            var favorite_total = $("#favorite-total").text();

            $("#favorite-total").text(parseInt(favorite_total) + 1);

            document.getElementById('favorite-button-area').style.display = "none";
            document.getElementById('unfavorite-button-area').style.display = "block";

            var x = document.getElementById("snackbar");
            $('.ReultMessage').text("Yêu thích sản phẩm thành công");

            // Add the "show" class to DIV
            x.className = "show";

            // After 3 seconds, remove the show class from DIV
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);

        },
        error: function (jqXHR, textStatus, errorThrown) {
            var x = document.getElementById("snackbarDanger");
            $('#ErrorMessage').text(jqXHR.responseText);

            // Add the "show" class to DIV
            x.className = "show";

            // After 3 seconds, remove the show class from DIV
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
        }
    });
});

$("#btnUnFavoriteProduct").click(function () {
    const cus_id = $(this).data('cusid');
    const product_id = $(this).data('productid');
    $.ajax({
        type: "POST",
        url: '/Product/UnFavoriteProduct/',
        data: {
            cus_id: parseInt(cus_id), product_id: parseInt(product_id),
        },
        success: function (res) {

            var favorite_total = $("#favorite-total").text();
            $("#favorite-total").text(parseInt(favorite_total) - 1);

            document.getElementById('favorite-button-area').style.display = "block";
            document.getElementById('unfavorite-button-area').style.display = "none";

            var x = document.getElementById("snackbar");
            $('.ReultMessage').text("Hủy yêu thích thành công");

            // Add the "show" class to DIV
            x.className = "show";

            // After 3 seconds, remove the show class from DIV
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            var x = document.getElementById("snackbarDanger");
            $('#ErrorMessage').text(jqXHR.responseText);

            // Add the "show" class to DIV
            x.className = "show";

            // After 3 seconds, remove the show class from DIV
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
        }
    });
})
