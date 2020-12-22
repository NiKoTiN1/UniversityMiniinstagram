var connection = new signalR.HubConnectionBuilder().withUrl("/comment").build();

connection.on("SendCommentHub", function (data, postId, commentId) {
    var textId = '#' + postId + 'text';
    var countComment = $('#' + postId + 'count');
    var commAreaId = '#' + postId + 'content';
    $(commAreaId).append(data);
    countComment.text(parseInt(countComment.text()) + 1);
    $(textId).val("");
    var offset = new Date().getTimezoneOffset() / -60;
    var dateElem = $('#Date' + commentId);
    var date = new Date(dateElem.text());
    date.setTime(date.getTime() + (offset * 60 * 60 * 1000));
    var minutes = date.getMinutes();
    if (minutes < 10) {
        minutes = "0" + minutes;
    }
    dateElem.text(date.getDate() + "." + date.getMonth() + "." + date.getFullYear() + " " + date.getHours() + ":" + minutes);
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