$(document).ready(function () {
    $("#tabs").tabs().tabs('rotate', 10000);;
    $("#tabs li a").click(function() {
        var url = '/Home/Report?countryId=' + $(this).attr("id");
        var targetDiv = $(this).attr("href");
        $.get(url, null, function (result) {
            $(targetDiv).html(result);
        });
    });
    $("#tabs").find('> ul a').trigger("click");
    $('#tabs').tabs('select', 0);
});