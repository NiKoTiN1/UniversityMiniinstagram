$('.dropdownDel').click(function () {
    var label = $(this).attr("id").replace('Delete', '');
    var comment = $('#' + label + 'content');
    $.ajax({
        type: 'DELETE',
        url: '/news/removedComment',
        data: { 'commentId': label },
        success: function (data) {
            var countComment = $('#' + data + 'count');
            countComment.text(parseInt(countComment.text()) - 1);
            comment.remove();
        }
    });
});