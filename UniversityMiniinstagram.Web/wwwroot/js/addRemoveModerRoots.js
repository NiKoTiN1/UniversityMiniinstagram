$('.Moder').click(function () {
    var userId = $(this).attr("id");
    userId = userId.replace('Moder', '');
    var unModer = '#UnModer' + userId;
    $.ajax({
        type: 'POST',
        url: '@Url.Content("~/admin/moderroots")',
        data: { 'userId': userId },
        success: function (data) {
            $('#Moder' + userId).hide();
            $(unModer).show();
        }
    });
});

$('.UnModer').click(function () {
    var userId = $(this).attr("id");
    userId = userId.replace('UnModer', '');
    var Moder = '#Moder' + userId;
    $.ajax({
        type: 'POST',
        url: '@Url.Content("~/admin/userroots")',
        data: { 'userId': userId },
        success: function (data) {
            $('#UnModer' + userId).hide();
            $(Moder).show();
        }
    });
});