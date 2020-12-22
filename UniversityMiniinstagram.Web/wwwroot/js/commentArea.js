$('.Comment').click(function () {
    var label = $(this).parent();
    var postId = $(this).attr("value");
    var ComId = '#' + postId + 'commAreaa';
    var isCommntOpen = label.children().eq(2).attr("value");
    if (isCommntOpen == "1") {
        $(ComId).hide();
        label.children().eq(2).attr("value", "0");
    }
    else {
        $(ComId).show();
        label.children().eq(2).attr("value", "1");
    }

});