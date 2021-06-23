var inputBoxLow = document.getElementById("inputSortPriceLow");
var inputBoxHigh = document.getElementById("inputSortPriceHigh");

var invalidChars = [
    "-",
    "+",
    "e",
];

inputBoxLow.addEventListener("keydown", function (e) {
    if (invalidChars.includes(e.key)) {
        e.preventDefault();
    }
});
inputBoxHigh.addEventListener("keydown", function (e) {
    if (invalidChars.includes(e.key)) {
        e.preventDefault();
    }
});
$("#inputSortPriceLow").change(function () {
    if ($(this).val() == "")
        $("#btnSortPrice").css("visibility", "hidden");
    else $("#btnSortPrice").css("visibility", "visible");
});
$("#inputSortPriceHigh").change(function () {
    if ($(this).val() == "")
        $("#btnSortPrice").css("visibility", "hidden");
    else $("#btnSortPrice").css("visibility", "visible");
});