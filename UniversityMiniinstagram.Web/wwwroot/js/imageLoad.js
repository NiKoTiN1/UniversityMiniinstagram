$('#File').change(function () {
    var input = this;
    var url = $(this).val();
    var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
    $('#Image').attr('src', url);

    if (input.files && input.files[0] && (ext == "gif" || ext == "png" || ext == "jpeg" || ext == "jpg")) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#Image').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
});