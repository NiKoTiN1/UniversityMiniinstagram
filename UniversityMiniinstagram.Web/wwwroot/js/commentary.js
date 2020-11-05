
var connection = new signalR.HubConnectionBuilder().withUrl("/comment").build();


connection.on("SendCommentHub", function (message) {
    console.log(message);
    //document.getElementById("messagesList").appendChild(li);
});

$('.SendComment').click(function () {
    var div = $(this).parent().parent().children(1);
    var postId = $(this).attr("value");
    var textId = '#' + postId + 'text';
    var text = $(textId).val();
    var countComment = $('#' + postId + 'count');
    connection.invoke("SendMessage", text).catch(function (err) {
        return console.error(err.toString());
    });

    //$.ajax({
    //    type: 'POST',
    //    url: 'addComment',
    //    data: { 'postId': postId, 'text': text },
    //    success: function (data) {
    //        var commAreaId = '#' + postId + 'content';
    //        $(commAreaId).append(data);
    //        $(textId).val("");
    //        countComment.text(parseInt(countComment.text()) + 1);
    //    }
    //});
});

connection.start().then(function () {
    console.log("bbb");
    //document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

//document.getElementById("sendButton").addEventListener("click", function (event) {
//    var user = document.getElementById("userInput").value;
//    var message = document.getElementById("messageInput").value;
//    connection.invoke("SendMessage", user, message).catch(function (err) {
//        return console.error(err.toString());
//    });
//    event.preventDefault();
//});