$('.AddBan').click(function () {
    var banId = $(this).attr("id");
    var userId = banId.replace('AddBan', '');
    var unBan = '#UnBan' + userId;
    var userRoleId = '#UnModer' + userId;
    var moderatorRole = '#Moder' + userId;
    $.ajax({
        type: 'POST',
        url: '@Url.Content("~/account/ban")',
        data: { 'userId': userId },
        success: function (data) {
            $('#' + banId).hide();
            $(unBan).show();
            $(userRoleId).hide();
            $(moderatorRole).hide();
        }
    });
});

$('.UnBan').click(function () {
    var unBan = $(this).attr("id");
    var userId = unBan.replace('UnBan', '');
    var ban = '#AddBan' + userId;
    var moderatorRole = '#Moder' + userId;
    $.ajax({
        type: 'POST',
        url: '@Url.Content("~/account/unban")',
        data: { 'userId': userId },
        success: function (data) {
            $(ban).show();
            $(moderatorRole).show();
            $('#' + unBan).hide();
        }
    });
});