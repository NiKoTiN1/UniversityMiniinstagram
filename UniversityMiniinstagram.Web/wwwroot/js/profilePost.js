$('.Post').click(function () {
    var postId = $(this).attr("value");
    $.ajax({
        type: 'POST',
        url: '/news/getPost',
        dataType: "html",
        data: { 'postId': postId },
        success: function (data) {
            $('#ModalBody').append(data);
            $('#ModalBtn').click();

        }
    });
});

$('#ModalParrent').on('hide.bs.modal', function (event) {
    $('#ModalBody').children().remove();
})