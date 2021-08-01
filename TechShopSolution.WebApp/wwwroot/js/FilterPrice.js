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

$("#btnSortPrice").click(function () {
    var form = $("#formFilter");

    var lowest_price = $("#inputSortPriceLow").val();
    var highest_price = $("#inputSortPriceHigh").val();

    if (lowest_price.length != 0 && lowest_price != "") {
        lowest_price = lowest_price.replace(/(₫)/gm, "");
        lowest_price = lowest_price.replace(/\s/g, "");
        document.getElementById("inputSortPriceLow").value = lowest_price;
    }
    if (highest_price.length != 0 && highest_price != "") {
        highest_price = highest_price.replace(/(₫)/gm, "");
        highest_price = highest_price.replace(/\s/g, "");
        document.getElementById("inputSortPriceHigh").value = highest_price;
    }

    form.submit();
});

$("#selectCategory").change(function () {
    var form = $("#formFilter");

    var lowest_price = $("#inputSortPriceLow").val();
    var highest_price = $("#inputSortPriceHigh").val();

    if (lowest_price.length != 0 && lowest_price != "") {
        lowest_price = lowest_price.replace(/(₫)/gm, "");
        lowest_price = lowest_price.replace(/\s/g, "");
        document.getElementById("inputSortPriceLow").value = lowest_price;
    }
    if (highest_price.length != 0 && highest_price != "") {
        highest_price = highest_price.replace(/(₫)/gm, "");
        highest_price = highest_price.replace(/\s/g, "");
        document.getElementById("inputSortPriceHigh").value = highest_price;
    }

    form.submit();
});

$("#selectBrand").change(function () {
    var form = $("#formFilter");

    var lowest_price = $("#inputSortPriceLow").val();
    var highest_price = $("#inputSortPriceHigh").val();

    if (lowest_price.length != 0 && lowest_price != "") {
        lowest_price = lowest_price.replace(/(₫)/gm, "");
        lowest_price = lowest_price.replace(/\s/g, "");
        document.getElementById("inputSortPriceLow").value = lowest_price;
    }
    if (highest_price.length != 0 && highest_price != "") {
        highest_price = highest_price.replace(/(₫)/gm, "");
        highest_price = highest_price.replace(/\s/g, "");
        document.getElementById("inputSortPriceHigh").value = highest_price;
    }

    form.submit();
});

$("#selectSortType").change(function () {
    var form = $("#formFilter");

    var lowest_price = $("#inputSortPriceLow").val();
    var highest_price = $("#inputSortPriceHigh").val();

    if (lowest_price.length != 0 && lowest_price != "") {
        lowest_price = lowest_price.replace(/(₫)/gm, "");
        lowest_price = lowest_price.replace(/\s/g, "");
        document.getElementById("inputSortPriceLow").value = lowest_price;
    }
    if (highest_price.length != 0 && highest_price != "") {
        highest_price = highest_price.replace(/(₫)/gm, "");
        highest_price = highest_price.replace(/\s/g, "");
        document.getElementById("inputSortPriceHigh").value = highest_price;
    }

    form.submit();
});

new AutoNumeric('#inputSortPriceLow', {
    allowDecimalPadding: false,
    currencySymbol: "₫",
    currencySymbolPlacement: "s",
    digitGroupSeparator: " ",
    maximumValue: "9999999999",
    minimumValue: "0"
})
new AutoNumeric('#inputSortPriceHigh', {
    allowDecimalPadding: false,
    currencySymbol: "₫",
    currencySymbolPlacement: "s",
    digitGroupSeparator: " ",
    maximumValue: "9999999999",
    minimumValue: "0"
})