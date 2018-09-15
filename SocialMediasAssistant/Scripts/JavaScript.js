$(document).ready(function () {
    let link = window.location.pathname;
    $('.nav-item').removeClass('active');
    $('[href="' + link + '"]').parents('.nav-item').addClass('active');

});