$('.dropdownRepPostEl').click(function () {
    var thiselem = $(this);
    var postId = thiselem.attr("id");
    postId = postId.replace('ReportPostBtn', '');
    var div = $(this).parent();
    var commentBtnId = '#' + postId + 'PostBtn';

    $.ajax({
        type: 'POST',
        url: '/admin/report/send',
        data: { 'PostId': postId, },
        success: function (data) {
            thiselem.remove();
            if (div.children().length == 0) {
                $(commentBtnId).remove();
            }
        }
    });
});

$('.dropdownDelPostEl').click(function () {
    var thiselem = $(this);
    var postId = thiselem.attr("id");
    postId = postId.replace('DeletePostBtn', '');
    var postDiv = $('#' + postId);
    $.ajax({
        type: 'DELETE',
        url: '/news/removedPost',
        data: { 'PostId': postId, },
        success: function () {
            postDiv.remove();
        }
    });
});