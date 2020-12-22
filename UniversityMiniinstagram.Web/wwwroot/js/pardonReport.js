$('.PardonBtn').click(function () {
    var btnId = $(this).attr("id");
    btnId = btnId.replace('Pardon', '');
    var mainId = '#Main' + btnId;
    $.ajax({
        type: 'POST',
        url: '/admin/pardon',
        data: { 'reportId': btnId },
        success: function (data) {
            $(mainId).remove();
        }
    });
});