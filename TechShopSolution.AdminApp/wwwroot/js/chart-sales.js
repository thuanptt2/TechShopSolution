window.addEventListener('load', (event) => {
    Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
    Chart.defaults.global.defaultFontColor = '#292b2c';
    loadData();
    loadDatabyMonth();

    var dateToday = new Date();
    $("#Txt_Date").datepicker({
        format: 'mm/dd/yyyy',
        inline: false,
        lang: 'en',
        step: 2,
        endDate: new Date(),
        todayHighlight: true,
        multidate: 2,
        closeOnDateSelect: true,
    });
});

function loadData(from, to) {
    $.ajax({
        type: "GET",
        url: "Home/GetRevenueReport",
        data: {
            fromDate: from,
            toDate: to
        },
        dataType: "json",
        success: function (res) {
            $("#chart-revenue-line").html('<canvas id="myAreaChart" width="100" height="40"></canvas>')
            initChartLine(res);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
    });
};
function loadDatabyMonth(from, to) {
    $.ajax({
        type: "GET",
        url: "Home/GetRevenueByMonthReport",
        data: {
            fromDate: from,
            toDate: to
        },
        dataType: "json",
        success: function (res) {
            initChartBar(res);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
        }
    });
};

function initChartLine(data) {
    var arrRevenueDate = [];
    var arrRevenueValue = [];
    var maxValue = 0;
    var total = 0;
    $.each(data, function (i, item) {
        var itemDate = new Date(item.date);
        var date = itemDate.getDate() + "/" + (itemDate.getMonth() + 1) + "/" + itemDate.getFullYear();
        var value = item.revenue;
        arrRevenueDate.push(date);
        arrRevenueValue.push(value);
        total += item.revenue;
        if (value > maxValue)
            maxValue = value;
    });
    $("#totalRevenueLine").text("Tổng doanh thu: " + new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(total));
    var ctx = document.getElementById("myAreaChart");
    var myLineChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: arrRevenueDate,
            datasets: [{
                label: "Doanh thu",
                lineTension: 0.3,
                backgroundColor: "rgba(2,117,216,0.2)",
                borderColor: "rgba(2,117,216,1)",
                pointRadius: 5,
                pointBackgroundColor: "rgba(2,117,216,1)",
                pointBorderColor: "rgba(255,255,255,0.8)",
                pointHoverRadius: 5,
                pointHoverBackgroundColor: "rgba(2,117,216,1)",
                pointHitRadius: 50,
                pointBorderWidth: 2,
                data: arrRevenueValue,
            }],
        },
        options: {
            scales: {
                xAxes: [{
                    time: {
                        unit: 'date'
                    },
                    gridLines: {
                        display: false
                    },
                    ticks: {
                        maxTicksLimit: 15
                    }
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        max: maxValue,
                        maxTicksLimit: 6,
                        callback: function (value) {
                            return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value)
                        }
                    },
                    gridLines: {
                        color: "rgba(0, 0, 0, .125)",
                    }
                }],
            },
            legend: {
                display: false
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index]);
                    }
                }
            }
        }
    });
};
function initChartBar(data) {
    var arrRevenueMonth = [];
    var arrRevenueValue = [];
    var maxValue = 0;
    $.each(data, function (i, item) {
        var itemDate = new Date(item.date);
        var month = item.month;
        var value = item.revenue;
        arrRevenueMonth.push(month);
        arrRevenueValue.push(value);
        if (value > maxValue)
            maxValue = value;
    });
    var ctx = document.getElementById("myBarChart");
    var myLineChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: arrRevenueMonth,
            datasets: [{
                label: "Doanh thu",
                backgroundColor: "rgba(2,117,216,1)",
                borderColor: "rgba(2,117,216,1)",
                data: arrRevenueValue,
            }],
        },
        options: {
            scales: {
                xAxes: [{
                    time: {
                        unit: 'month'
                    },
                    barPercentage: 0.2,
                    gridLines: {
                        display: false
                    },
                    ticks: {
                        maxTicksLimit: 12
                    }
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        max: maxValue,
                        maxTicksLimit: 6,
                        callback: function (value) {
                            return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(value)
                        },
                    },
                    gridLines: {
                        display: true
                    }
                }],
            },
            legend: {
                display: false
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem, data) {
                        return new Intl.NumberFormat('vi-VN', { style: 'currency', currency: 'VND' }).format(data.datasets[tooltipItem.datasetIndex].data[tooltipItem.index]);
                    }
                }
            }
        }
    });
};

function loadRevenue() {
    var dataArr = ($("#Txt_Date").val()).split(",");

    if (dataArr.length == 2) {
       
        var date1 = new Date(dataArr[0]);
        var date2 = new Date(dataArr[1]);
        if (date1.getTime() > date2.getTime()) {
            var fromDate = (date2.getMonth() + 1) + "/" + date2.getDate() + "/" + date2.getFullYear();
            var toDate = (date1.getMonth() + 1) + "/" + date1.getDate() + "/" + date1.getFullYear();
            loadData(fromDate, toDate);
        }
        else {
            var fromDate = (date1.getMonth() + 1) + "/" + date1.getDate() + "/" + date1.getFullYear();
            var toDate = (date2.getMonth() + 1) + "/" + date2.getDate() + "/" + date2.getFullYear();
            loadData(fromDate, toDate);
        }
    }
}
function loadRevenueByMonth() {

    var fromMonth = document.getElementById('fromMonth').value;
    var toMonth = document.getElementById('toMonth').value;
   
    if (fromMonth != "" && toMonth != "") {

        var lastday = function (y, m) {
            return new Date(y, m + 1, 0).getDate();
        }

        fromMonth = new Date(fromMonth);
        toMonth = new Date(toMonth);
        if (fromMonth.getTime() > toMonth.getTime()) {
            var temp = fromMonth;
            fromMonth = toMonth;
            toMonth = temp;

            fromMonth = (fromMonth.getMonth() + 1) + "/01/" + fromMonth.getFullYear();
            toMonth = (toMonth.getMonth() + 1) + "/" + lastday(toMonth.getFullYear(), toMonth.getMonth()) + "/" + toMonth.getFullYear();
            loadDatabyMonth(fromMonth, toMonth);
        }
        else {
            fromMonth = (fromMonth.getMonth() + 1) + "/01/" + fromMonth.getFullYear();
            toMonth = (toMonth.getMonth() + 1) + "/" + lastday(toMonth.getFullYear(), toMonth.getMonth()) + "/" + toMonth.getFullYear();
            loadDatabyMonth(fromMonth, toMonth);
        }
    }
}