$('.dropdownRep').click(function () {
    var elem = $(this);
    var label = elem.attr("id");
    label = label.replace('Report', '');
    var div = $(this).parent();
    var commentBtnId = '#' + label + 'Button';
    $.ajax({
        type: 'POST',
        url: '/admin/report/send',
        data: { 'CommentId': label },
        success: function (data) {
            elem.remove();
            if (div.children().length == 0) {
                $(commentBtnId).remove();
            }
        }
    });
});