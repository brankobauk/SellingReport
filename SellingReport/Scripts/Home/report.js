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
                    var height = 165 - (165 * percentage / 100);
                    var el = '<div class="overlay" style="height:' + height + 'px;border-bottom:1px solid red;"></div>';
                    $(this).parent().next().find('td').eq($(this).index()).append(el);
                }
            }
        }
    });
    return false;

});