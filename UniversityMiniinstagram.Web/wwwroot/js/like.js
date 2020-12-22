$('.Like').click(function () {
    var label = $(this).parent();
    var postId = $(this).attr("value");
    var svg = label.children().eq(0);
    var svgL = label.children().eq(1);
    var likesCount = label.children().eq(3);
    var isLiked = label.children().eq(2).attr("value");
    if (isLiked == "1") {
        svgL.hide();
        svg.show();
        label.children().eq(2).attr("value", "0");
        likesCount.text(parseInt(likesCount.text()) - 1);
        $.ajax({
            type: 'DELETE',
            async: false,
            url: '/news/removeLike',
            data: { 'postId': postId },
            error: function (data) {
                svgL.show();
                svg.hide();
                label.children().eq(2).attr("value", "1");
                likesCount.text(parseInt(likesCount.text()) + 1);
            },
        });
    }
    else {
        svg.hide();
        svgL.show();
        label.children().eq(2).attr("value", "1");
        likesCount.text(parseInt(likesCount.text()) + 1);
        $.ajax({
            type: 'POST',
            async: false,
            url: '/news/addLike',
            data: { 'postId': postId },
            error: function (data) {
                svgL.hide();
                svg.show();
                label.children().eq(2).attr("value", "0");
                likesCount.text(parseInt(likesCount.text()) - 1);
            }
        });

    }
});