$(document).ready(function () {
    var path = localStorage.getItem("path");
    $('#path').val(path);
});
$('#path').keyup(function () {
    localStorage.setItem("path", $(this).val());
});
$('a#dowload').attr({
    target: '_blank',
    //href: 'https://localhost:44335/Files/Sign.rar'
    href: 'http://202.78.227.89/Files/Sign.rar'
});