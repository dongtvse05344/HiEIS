$(document).ready(function () {
    setTimeout(function () {
        $('.my-tooltip.center .tooltip-text').each(function (index, value) {
            var halftWidth = $(this).width() / 2;
            var parentHalfWidth = $(this).parent().width() / 2;
            var padding = parseFloat($(this).css('padding-left'));
            $(this).css({
                'transform': 'translateX(-' + (halftWidth - parentHalfWidth + padding) + 'px)'
            });
        });
    }, 1000);
});