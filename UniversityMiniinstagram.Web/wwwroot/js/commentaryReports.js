$('.form-check-input').change(function () {
    var accBtn = $(this).attr("id");
    accBtn = accBtn.replace('Ban', '');
    accBtn = accBtn.replace('DeleteBBtn', '');
    accBtn = accBtn.replace('HideCommnet', '');
    accBtn = accBtn.replace('Hide', '');
    accBtn = accBtn.replace('DeleteComment', '');
    var banId = '#Ban' + accBtn;
    var delId = '#DeleteBBtn' + accBtn;
    var hideId = '#Hide' + accBtn;
    var hideCommId = '#HideCommnet' + accBtn;
    var delCommId = '#DeleteComment' + accBtn;

    accBtn = '#Accept' + accBtn;

    if ($(hideId).is(':checked') || $(banId).is(':checked') || $(delId).is(':checked') || $(hideCommId).is(':checked') || $(delCommId).is(':checked')) {
        $(accBtn).prop('disabled', false);
    } else {
        $(accBtn).prop('disabled', true);
    }
});

$('.AcceptBtn').click(function () {
    var accBtn = $(this).attr("id");
    accBtn = accBtn.replace('Accept', '');
    var mainId = '#Main' + accBtn;
    var banId = '#Ban' + accBtn;
    var delId = '#DeleteBBtn' + accBtn;
    var hideId = '#Hide' + accBtn;
    var hideCommId = '#HideCommnet' + accBtn;
    var delCommId = '#DeleteComment' + accBtn;
    $.ajax({
        type: 'POST',
        url: '/admin/comment/decision',
        data: {
            'ReportId': accBtn,
            'IsBanUser': $(banId).is(':checked'),
            'IsDeletePost': $(delId).is(':checked'),
            'IsHidePost': $(hideId).is(':checked'),
            'IsHideComment': $(hideCommId).is(':checked'),
            'IsDeleteComment': $(delCommId).is(':checked')
        },
        success: function (data) {
            $(mainId).remove();
        }
    });
});