$(document).ready(function () {
    var offset = new Date().getTimezoneOffset() / -60;
    document.cookie = "offset=" + offset + "; max-age=3600";
});