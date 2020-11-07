var connection = new signalR.HubConnectionBuilder().withUrl("/comment").build();

connection.on("SendCommentHub", function (data, postId) {
    console.log("bbb");
    var textId = '#' + postId + 'text';
    var countComment = $('#' + postId + 'count');
    var commAreaId = '#' + postId + 'content';
    $(commAreaId).append(data);
    countComment.text(parseInt(countComment.text()) + 1);
    $(textId).val("");
});

$('.SendComment').click(function () {
    var div = $(this).parent().parent().children(1);
    var postId = $(this).attr("value");
    var textId = '#' + postId + 'text';
    var text = $(textId).val();
    connection.invoke("SendMessage", postId, text);
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
});