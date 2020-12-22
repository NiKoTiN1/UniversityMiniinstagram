$(document).ready(function () {
    var offset = new Date().getTimezoneOffset() / -60;
    $('.DateElem').each(function () {
        var dateElem = $(this);
        var date = new Date(dateElem.text());
        date.setTime(date.getTime() + (offset * 60 * 60 * 1000));
        var minutes = date.getMinutes();
        if (minutes < 10) {
            minutes = "0" + minutes;
        }
        dateElem.text(date.getDate() + "." + date.getMonth() + "." + date.getFullYear() + " " + date.getHours() + ":" + minutes);
    });
});