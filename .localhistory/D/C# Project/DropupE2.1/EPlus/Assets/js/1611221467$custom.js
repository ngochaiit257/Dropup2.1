function PageFunction() {
    var self = this;
    var ww = wh = 0;

    this.init = function () {
        self.jsMenu();
        self.fixFloating();

        var ele_box_register = $('.wrap-box-register');
        $(document).on('click', '.box-register .title', function () {
            var that = $(this);
            if (ele_box_register.hasClass('show')) {
                ele_box_register.removeClass('show');
            } else {
                ele_box_register.addClass('show');
            }
            return false;
        });

    };

    this.scrollPage = function (ele) {
        $("html, body").stop().animate({ scrollTop: ele.offset().top }, "slow");
    };

    this.fixFloating = function () {
        var floating = $('.floating-menu');
        var ft = $('.footer');
        var ele_register = $('.wrap-box-register');

        function fix(that) {
            var sTop = that.scrollTop();
            var h = floating.height();
            var offset_top = ft.offset().top - 10;
            wh = that.height();

            var flag = sTop + ((wh + h) / 2);

            var flag1 = sTop + wh;

            if (flag < offset_top) {
                floating.addClass('fix-screen');

            } else {
                floating.removeClass('fix-screen');
            }

            if (flag1 < offset_top) {
                ele_register.addClass('fix-screen');
            } else {
                ele_register.removeClass('fix-screen');
            }
        }

        fix($(window));

        $(window).scroll(function () {
            fix($(this));
        });
    }

}

var PageFunction = new PageFunction();

$(document).ready(function () {
    $(window).on('load', function () {
        PageFunction.init();
    });
});

