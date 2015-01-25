$(document).ready(function () {

    $("#main-table table").each(function () {
        var $this = $(this);
        var newrows = [];
        $this.find("tr").each(function () {
            var i = 0;
            $(this).find("td").each(function () {
                i++;
                if (newrows[i] === undefined) { newrows[i] = $("<tr></tr>"); }
                newrows[i].append($(this));
            });
        });
        $this.find("tr").remove();
        $.each(newrows, function () {
            $this.append(this);
        });
    });

    $("#main-table table tr td").each(function () {
        if ($(this).hasClass("percentage")) {
            if (Math.floor($(this).text()) == $(this).text() && $.isNumeric($(this).text())) {
                var percentage = $(this).text();
                if (percentage < 100) {
                    var height = 55 - (55 * percentage / 100);
                    var el = '<div class="overlay" style="height:' + height + 'px;border-bottom:1px solid red;"></div>';
                    $(this).parent().next().find('td').eq($(this).index()).append(el);
                }
            }
        }
    });

    $("#yearly-reports .item").each(function () {

        var container = $(this);

        var yearlyPlanValueDiv = container.find(".yearly-planned-ammount-value");
        var monthlyPlanValueDiv = container.find(".monthly-planned-ammount-value");
        var monthlySoldValueDiv = container.find(".monthly-achieved-ammount-value");

        var yearlyPlanValue = parseInt(yearlyPlanValueDiv.text());
        var monthlySoldValue = parseInt(monthlySoldValueDiv.text());
        var topValue = yearlyPlanValue;
        var onPlan = false;
        if (yearlyPlanValue < monthlySoldValue) {
            topValue = monthlySoldValue;
            onPlan = true;
        }

        container.find(".line").each(function () {
            var val = parseInt($(this).find(".planned-pieces").attr("data-val"));
            var h = 210 / (topValue / (val * 100)) / 100;
            $(this).height(h);
        });

        if (onPlan == false) {
            container.find(".margin").height(0);
        } else {
            container.find(".margin").height(210 * (1 - (yearlyPlanValue / monthlySoldValue)));
        }

        var chartLineContainer = container.find(".chart-line.arrow");
        var chartLineContainerHeight = chartLineContainer.height();
        var chartLineContainerYPos = chartLineContainer.position().top;
        var yearlyPlanDiv = container.find(".yearly-planned-ammount");
        

        var monthlyPlanContainer = container.find(".chart-line.chart-line-planned.arrow");
        var monthlyPlanContainerHeight = chartLineContainer.height();
        var monthlyPlanContainerYPos = monthlyPlanContainer.position().top;
        var monthlyPlanDiv = container.find(".monthly-planned-ammount");
        

        var monthlySoldContainer = container.find(".achieved-line.arrow");
        var monthlySoldContainerHeight = chartLineContainer.height();
        var monthlySoldContainerYPos = monthlySoldContainer.position().top;
        var monthlySoldDiv = container.find(".monthly-achieved-ammount");
        

        
        if (onPlan) {
            monthlySoldContainerYPos -= 210 * (1 - (yearlyPlanValue / monthlySoldValue)) - 18;
            chartLineContainerYPos = container.find(".margin").position().top + container.find(".margin").height();
            monthlyPlanContainerYPos = container.find(".margin").position().top + container.find(".margin").height();
        }
        

        var b = container.find(".clear").position().top;

        yearlyPlanValueDiv.css("top", chartLineContainerYPos - 18);
        yearlyPlanDiv.css("top", chartLineContainerYPos);
        yearlyPlanDiv.height(b - chartLineContainerYPos);

        alert(monthlyPlanContainerYPos);
        monthlyPlanValueDiv.css("top", monthlyPlanContainerYPos - 18);
        monthlyPlanDiv.css("top", monthlyPlanContainerYPos);
        monthlyPlanDiv.height(b - monthlyPlanContainerYPos);

        alert(monthlySoldContainerYPos);
        monthlySoldValueDiv.css("top", monthlySoldContainerYPos - 18);
        monthlySoldDiv.css("top", monthlySoldContainerYPos);
        monthlySoldDiv.height(b - monthlySoldContainerYPos);
    });
    return false;

});