$(document).ready(function () {
    $("#ProductSellingReport_Date").addClass("date");
    $('#ProductSellingReport_Date').prop('readOnly', true);
    if ($('#ProductSellingReport_Date').val == "") {
        $('#ProductSellingReport_Date').val(Date.now());
    }
    $("#ProductSellingReport_SoldValue").removeAttr("data-val");
    $(".date").datepicker({
        dateFormat: 'dd.mm.yy',
        minDate: '1.1.2010',
        beforeShowDay: $.datepicker.noWeekends
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