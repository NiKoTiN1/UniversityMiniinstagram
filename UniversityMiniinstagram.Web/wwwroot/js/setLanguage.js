$('#Language').change(function () {
    var cultureVal = $(this);
    var url = $(location).attr("href");
    var culture = "";
    switch (cultureVal.val()) {
        case "Русский":
            {
                culture = "ru";
                break;
            }
        case "English":
            {
                culture = "en";
                break;
            }
        case "Deutsche":
            {
                culture = "de";
                break;
            }
    }
    var a = "";
    $.ajax({
        type: 'POST',
        url: '@Url.Content("~/settings/language")',
        data: { 'culture': culture, 'returnUrl': url },
        success: function (data) {
            location.reload();
        }
    });
});