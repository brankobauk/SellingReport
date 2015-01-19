$(document).ready(function () {
    
    function noWeekendsOrHolidays(date) {
        var selectedCountryId = $("#ProductSellingMonthlyReport_CountryId option:selected").val();
        var holidays = $("#country-" + selectedCountryId).text();
        var arrHolidays = holidays.split(",");
        var i;
        for (i = 0; i < arrHolidays.length; i++) {
            if (arrHolidays[i].length == 6) {
                arrHolidays[i] = arrHolidays[i] + jQuery.datepicker.formatDate('yy', date);
            }
        }
        var noWeekend = $.datepicker.noWeekends(date);
        if (noWeekend[0]) {
            var datestring = jQuery.datepicker.formatDate('dd.mm.yy', date);
            return [arrHolidays.indexOf(datestring) == -1];
        } else {
            return noWeekend;
        }
    }
    $("#ProductSellingMonthlyReport_Date").addClass("date");
    $('#ProductSellingMonthlyReport_Date').prop('readOnly', true);
    $("#ProductSellingMonthlyReport_SoldValue").removeAttr("data-val");
    $(".date").datepicker({
        dateFormat: 'dd.mm.yy',
        minDate: '1.1.2010',
        beforeShowDay: noWeekendsOrHolidays
    });
    jQuery.validator.addMethod(
            'date',
            function (value, element, params) {
                if (this.optional(element)) {
                    return true;
                };
                var result = false;
                try {
                    $.datepicker.parseDate('dd.mm.yy', value);
                    result = true;
                } catch (err) {
                    result = false;
                }
                return result;
            },
            ''
        );
});