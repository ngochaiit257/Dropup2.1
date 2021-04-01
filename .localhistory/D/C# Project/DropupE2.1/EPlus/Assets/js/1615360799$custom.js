function PageFunction() {
    var self = this;
    var ww = wh = 0;

    this.init = function () {
        self.jsMenu();
        self.fixFloating();

        if ($('.wrap-other').length > 0) {
            var swiper_other = new Swiper('.wrap-other .swiper-container', {
                slidesPerView: 3,
                spaceBetween: 30,
                navigation: {
                    nextEl: '.wrap-other .swiper-button-next',
                    prevEl: '.wrap-other .swiper-button-prev'
                },

                breakpoints: {
                    768: {
                        slidesPerView: 2,
                        spaceBetween: 10
                    }
                }
            });
        }

        if ($('.js-scroll-bar').length > 0) {
            $(".js-scroll-bar").mCustomScrollbar();
        }

        if ($('.wrap-banner').length > 0) {
            var swiper_banner = new Swiper('.wrap-banner .swiper-container', {
                loop: true,
                autoplay: {
                    delay: 5000
                },
                pagination: {
                    el: '.wrap-banner .swiper-pagination',
                    clickable: true
                },
                navigation: {
                    nextEl: '.wrap-banner .swiper-button-next',
                    prevEl: '.wrap-banner .swiper-button-prev'
                },
                on: {
                    slideChangeTransitionEnd: slideChangeTransitionEnd
                }
            });

            $(document).on('click', '.wrap-banner .btn-play', function () {
                var that = $(this);
                var id = that.attr('data-id');
                var ele_box = that.parents('.wrap-banner__main--video').find('.box-iframe');
                ele_box.append('<iframe width="100%" height="100%" src="https://www.youtube.com/embed/' + id + '" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>');
                ele_box.addClass('show');
                return false;
            });

            $(document).on('click', '.wrap-banner .close', function () {
                slideChangeTransitionEnd();
                return false;
            });

            function slideChangeTransitionEnd() {
                var ele = $('.wrap-banner__main--video .box-iframe');
                ele.removeClass('show');
                ele.find('iframe').remove();
            }
        }

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

        $(document).on('click', '.floating-menu .wrap-fb a', function () {
            var that = $(this);
            var next = that.next();
            if (next.is(":hidden")) {
                next.fadeIn();
            } else {
                next.fadeOut();
            }
            return false;
        });

        $(document).on('click', '.menu-mb li a', function () {
            var that = $(this);
            var next = that.next('.sub');
            if (next.is(":hidden")) {
                next.slideDown();
                return false;
            }
            return true;
        });

        $(document).on('click', '.wrap-study--detail .btn--red', function () {
            $('.box-register .title').trigger('click');
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
            //} else {
            //    ele_register.removeClass('fix-screen');
            //}
        }

        fix($(window));

        $(window).scroll(function () {
            fix($(this));
        });
    }

    this.jsMenu = function () {
        var ele_header = $('header');
        var ele_menu_mb = $('.menu-mb');
        var ele_header_left = $('.header__left');
        var ele_container = $('#container-main');
        var ele_top = $('.btn-top');

        ele_menu_mb.prepend(ele_header_left.html());

        $(document).on('click', '#nav-icon', function () {
            var that = $(this);
            if (!that.hasClass('open')) {
                that.addClass('open');
                ele_header.addClass('open');
                ele_container.addClass('open');
            } else {
                that.removeClass('open');
                ele_header.removeClass('open');
                ele_container.removeClass('open');
            }
            return false;
        });

        $(document).on('click', '.btn-top', function () {
            self.scrollPage(ele_container);
            return false;
        });

        $(window).scroll(function () {
            var sTop = $(this).scrollTop();
            if (sTop > 30) {
                ele_header.addClass('header--small');
                ele_top.addClass('show');
            } else {
                ele_header.removeClass('header--small');
                ele_top.removeClass('show');
            }

            clearTimeout(time_s);
            time_s = setTimeout(function () {
                hasScrolled();
            }, 5);
        });
    }

    this.antHome = function () {

        var ele_video = $('#modal-video');
        if (ele_video.length > 0) {
            var sub = ele_video.find('.embed-responsive');
            ele_video.on('hide.bs.modal', function (e) {
                sub.html("");
            });

            ele_video.on('shown.bs.modal', function (e) {
                var type = $("#type_popup").val();
                var value = $("#value_popup").val();
                if (type == 1) {
                    sub.html('<iframe width="560" height="315" src="https://www.youtube.com/embed/' + value + '" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>');
                } else {
                    var link = root + "static/uploads/banner/" + value;
                    sub.html('<a href="#" title="" style="background-image: url(' + link + ')"></a>');
                }

            });

            ele_video.modal("show");
        }

        var obj_1 = { y: '100px', opacity: 0 };
        var obj_2 = { marginTop: '100px', opacity: 0 };
        var obj_3 = { opacity: 0 };
        var speed_1 = 0.2;

        var controller = new ScrollMagic.Controller();
        var tl = new TimelineMax()
            .from('.wrap-banner .box-text', speed_1, obj_2)
            .from('.wrap-banner .img', speed_1, obj_3);
        new ScrollMagic.Scene({
            triggerElement: '.wrap-banner',
            offset: -100
        })
            .setTween(tl)
            .addTo(controller)
            .reverse(false);

        var tl1 = new TimelineMax()
            .from('.wrap-only-apax .tt-2', speed_1, obj_1)
            .staggerFrom('.wrap-only-apax .tab-control a', speed_1, obj_1, 0.2)
            .from('.wrap-only-apax .wrap-slider', speed_1, obj_1);
        new ScrollMagic.Scene({
            triggerElement: '.wrap-only-apax',
            offset: -100
        })
            .setTween(tl1)
            .addTo(controller)
            .reverse(false);

        var tl2 = new TimelineMax()
            .from('.wrap-only-apax__proud .col-9', speed_1, obj_1)
            .staggerFrom('.wrap-only-apax__proud .col-9 div', speed_1, obj_1, 0.2);
        new ScrollMagic.Scene({
            triggerElement: '.wrap-only-apax__proud',
            offset: -100
        })
            .setTween(tl2)
            .addTo(controller)
            .reverse(false);

        var tl3 = new TimelineMax()
            .from('.wrap-courses .tt-2', speed_1, obj_1)
            .from('.wrap-courses .tab-main', speed_1, obj_1)
            .from('.wrap-courses .tab-control', speed_1, obj_1)
            .from('.wrap-courses .box-content', speed_1, obj_1);
        new ScrollMagic.Scene({
            triggerElement: '.wrap-courses',
            offset: -100
        })
            .setTween(tl3)
            .addTo(controller)
            .reverse(false);

        var tl4 = new TimelineMax()
            .from('.wrap-meeting .tt-2', speed_1, obj_1)
            .staggerFrom('.wrap-meeting .item-meeting', speed_1, obj_1, 0.2)
            .from('.wrap-meeting .box-text', speed_1, obj_1);
        new ScrollMagic.Scene({
            triggerElement: '.wrap-meeting',
            offset: -100
        })
            .setTween(tl4)
            .addTo(controller)
            .reverse(false);

        var tl5 = new TimelineMax()
            .from('.wrap-parents .tt-2', speed_1, obj_1)
            .from('.wrap-parents .wrap-slider', speed_1, obj_1);
        new ScrollMagic.Scene({
            triggerElement: '.wrap-parents',
            offset: -100
        })
            .setTween(tl5)
            .addTo(controller)
            .reverse(false);

        var tl6 = new TimelineMax()
            .from('.wrap-angle .tt-2', speed_1, obj_1)
            .staggerFrom('.wrap-angle .tab-control a', speed_1, obj_1, 0.2)
            .from('.wrap-angle .wrap-slider', speed_1, obj_1);
        new ScrollMagic.Scene({
            triggerElement: '.wrap-angle',
            offset: -100
        })
            .setTween(tl6)
            .addTo(controller)
            .reverse(false);

        var tl7 = new TimelineMax()
            .from('.wrap-news .tt-2', speed_1, obj_1)
            .staggerFrom('.wrap-news .item-news', speed_1, obj_1, 0.2)
            .from('.wrap-news .btn', speed_1, obj_1);
        new ScrollMagic.Scene({
            triggerElement: '.wrap-news',
            offset: -100
        })
            .setTween(tl7)
            .addTo(controller)
            .reverse(false);

    }

    this.onlyApax = function () {
        var tab_only = $('.wrap-only-apax .tab-control a');

        var swiper_only_apax_tab = new Swiper('.wrap-only-apax .tab-control .swiper-container', {
            slidesPerView: tab_only.length,
            spaceBetween: 10,
            navigation: {
                nextEl: '.wrap-only-apax .tab-control .swiper-button-next',
                prevEl: '.wrap-only-apax .tab-control .swiper-button-prev'
            },
            breakpoints: {
                768: {
                    slidesPerView: 1,
                    spaceBetween: 10
                },
                990: {
                    slidesPerView: 3,
                    spaceBetween: 10
                },
                1200: {
                    slidesPerView: 4,
                    spaceBetween: 10
                }
            },
            on: {
                slideChangeTransitionEnd: slideEndOnlyApax1
            }
        });

        var swiper_only_apax = new Swiper('.wrap-only-apax .wrap-slider .swiper-container', {
            slidesPerView: 1,
            navigation: {
                nextEl: '.wrap-only-apax .wrap-slider .swiper-button-next',
                prevEl: '.wrap-only-apax .wrap-slider .swiper-button-prev'
            },
            on: {
                slideChangeTransitionEnd: slideEndOnlyApax
            }
        });

        $(document).on('click', '.wrap-only-apax .tab-control a', function () {
            var that = $(this);
            var index = $('.wrap-only-apax .tab-control a').index(this);
            swiper_only_apax_tab.slideTo(index, 500, true);
            swiper_only_apax.slideTo(index, 500, true);
            return false;
        });


        function slideEndOnlyApax(sw) {
            var index = swiper_only_apax.activeIndex;
            $('.wrap-only-apax .tab-control a').eq(index).trigger('click');
            tab_only.removeClass('active').eq(index).addClass('active');
        }

        function slideEndOnlyApax1(sw) {
            var index = swiper_only_apax_tab.activeIndex;
            $('.wrap-only-apax .tab-control a').eq(index).trigger('click');
            tab_only.removeClass('active').eq(index).addClass('active');
        }

        setTimeout(function () {
            console.log(swiper_only_apax.activeIndex);
        }, 10000)
    }

    this.jsAngle = function () {
        var tab_angle = $('.wrap-angle .tab-control a');

        var swiper_angle_tab = new Swiper('.wrap-angle .tab-control .swiper-container', {
            slidesPerView: tab_angle.length,
            spaceBetween: 10,
            navigation: {
                nextEl: '.wrap-angle .tab-control .swiper-button-next',
                prevEl: '.wrap-angle .tab-control .swiper-button-prev'
            },
            breakpoints: {
                768: {
                    slidesPerView: 1,
                    spaceBetween: 10,

                },
                990: {
                    slidesPerView: 3,
                    spaceBetween: 10
                },
                1200: {
                    slidesPerView: 4,
                    spaceBetween: 10
                }
            },
            on: {
                slideChangeTransitionEnd: slideEndAngle1
            }
        });

        var swiper_angle = new Swiper('.wrap-angle .wrap-slider .swiper-container', {
            navigation: {
                nextEl: '.wrap-angle .wrap-slider .swiper-button-next',
                prevEl: '.wrap-angle .wrap-slider .swiper-button-prev'
            },
            on: {
                slideChangeTransitionEnd: slideEndAngle
            }
        });

        $(document).on('click', '.wrap-angle .tab-control a', function () {
            var that = $(this);
            var index = $('.wrap-angle .tab-control a').index(that);
            swiper_angle_tab.slideTo(index, 500, true);
            swiper_angle.slideTo(index, 500, true);
            return false;
        });

        function slideEndAngle() {
            var index = swiper_angle.activeIndex;
            $('.wrap-angle .tab-control a').eq(index).trigger('click');
            tab_angle.removeClass('active').eq(index).addClass('active');
        }

        function slideEndAngle1() {
            var index = swiper_angle_tab.activeIndex;
            $('.wrap-angle .tab-control a').eq(index).trigger('click');
            tab_angle.removeClass('active').eq(index).addClass('active');
        }

        $(document).on('click', '.item-angle .item-angle__list a', function () {
            var that = $(this);
            var id = that.attr('data-id');
            var ele_box = that.parents('.item-angle').find('.item-angle__video');
            if (!that.hasClass('active')) {
                that.siblings().removeClass('active');
                that.addClass('active');
                ele_box.css('background-image', 'url(' + id + ')');
            }
            return false;
        });


    }

    this.onlyCourses = function () {
        var ele_group = $('.wrap-courses .group-content');

        var swiper_1 = new Swiper('#group-courses-1 .tab-control .swiper-container', {
            slidesPerView: 4,
            spaceBetween: 10,
            navigation: {
                nextEl: '#group-courses-1 .tab-control .swiper-button-next',
                prevEl: '#group-courses-1 .tab-control .swiper-button-prev'
            },
            breakpoints: {
                768: {
                    slidesPerView: 1,
                    spaceBetween: 10,
                }
            },
            on: {
                slideChangeTransitionEnd: slideEnd1
            }
        });


        var swiper_1_1 = new Swiper('#group-courses-1 .box-content .swiper-container', {
            slidesPerView: 1,
            on: {
                slideChangeTransitionEnd: slideEnd1_1
            }
        });


        var swiper_2 = new Swiper('#group-courses-2 .tab-control .swiper-container', {
            slidesPerView: 4,
            spaceBetween: 10,
            navigation: {
                nextEl: '#group-courses-2 .swiper-button-next',
                prevEl: '#group-courses-2 .swiper-button-prev'
            },
            breakpoints: {
                768: {
                    slidesPerView: 1,
                    spaceBetween: 10
                }
            },
            on: {
                slideChangeTransitionEnd: slideEnd2
            }
        });

        var swiper_2_1 = new Swiper('#group-courses-2 .box-content .swiper-container', {
            slidesPerView: 1,
            on: {
                slideChangeTransitionEnd: slideEnd2_1
            }
        });

        function slideEnd1() {
            var index = swiper_1.activeIndex;
            $('#group-courses-1 .tab-control a').eq(index).trigger('click');
        }

        function slideEnd1_1() {
            var index = swiper_1_1.activeIndex;
            $('#group-courses-1 .tab-control a').eq(index).trigger('click');
        }

        function slideEnd2() {
            var index = swiper_2.activeIndex;
            $('#group-courses-2 .tab-control a').eq(index).trigger('click');
        }

        function slideEnd2_1() {
            var index = swiper_2_1.activeIndex;
            $('#group-courses-2 .tab-control a').eq(index).trigger('click');
        }

        $(document).on('click', '.wrap-courses .tab-main a', function () {
            var that = $(this);
            var index = $('.wrap-courses .tab-main a').index(this);

            if (!that.hasClass('active')) {
                $('.wrap-courses .tab-main a').removeClass('active');
                that.addClass('active');
                ele_group.removeClass('show').eq(index).addClass('show');
                setTimeout(function () {
                    if (index == 0) {
                        swiper_1.update();
                        swiper_1_1.update();
                    } else {
                        swiper_2.update();
                        swiper_2_1.update();
                    }
                }, 500);
            }

            return false;
        });

        $(document).on('click', '#group-courses-1 .tab-control a', function () {
            var that = $(this);
            var index = $('#group-courses-1 .tab-control a').index(that);
            var parent = that.parents('.group-content');
            var ele_item = parent.find('.item-courses');

            if (!that.hasClass('active')) {
                $('#group-courses-1 .tab-control a').removeClass('active');
                that.addClass('active');
                ele_item.removeClass('show').eq(index).addClass('show');
                swiper_1.slideTo(index, 500, true);
                swiper_1_1.slideTo(index, 500, true);
            }
            return false;
        });

        $(document).on('click', '#group-courses-2 .tab-control a', function () {
            var that = $(this);
            var index = $('#group-courses-2 .tab-control a').index(that);
            var parent = that.parents('.group-content');
            var ele_item = parent.find('.item-courses');
            if (!that.hasClass('active')) {
                $('#group-courses-2 .tab-control a').removeClass('active');
                that.addClass('active');
                ele_item.removeClass('show').eq(index).addClass('show');
                swiper_2.slideTo(index, 500, true);
                swiper_2_1.slideTo(index, 500, true);
            }
            return false;
        });
    }

    this.onlyParent = function () {
        var swiper_parents = new Swiper('.wrap-parents .swiper-container', {
            slidesPerView: 4,
            slidesPerColumn: 2,
            spaceBetween: 0,
            navigation: {
                nextEl: '.wrap-parents .swiper-button-next',
                prevEl: '.wrap-parents .swiper-button-prev'
            },
            breakpoints: {
                768: {
                    slidesPerView: 2,
                    slidesPerColumn: 4
                }
            }
        });

        ww = $(window).width();
        resizeTooltip();
        $(window).resize(function () {
            ww = $(this).width();
            resizeTooltip();
        });

        function resizeTooltip() {
            $('.wrap-parents .swiper-slide a').tooltip('dispose');
            if (ww > 768) {
                $('.wrap-parents .swiper-slide a').tooltip({
                    container: '.wrap-parents .swiper-container',
                    placement: 'left',
                    html: true
                });

                $('.wrap-parents .swiper-slide a').on('shown.bs.tooltip', function () {

                    var ele = $('.wrap-parents .swiper-container');
                    var ele_tooltip = $('.wrap-parents .swiper-container .tooltip.show');
                    var y = getValues(ele_tooltip)[1];

                    if (y < 0) {
                        y = (-1) * y;
                    } else {
                        var flag = (y + ele_tooltip.outerHeight()) - ele.outerHeight();
                        if (flag > 0) {
                            y = (-1) * (flag);
                        } else {
                            y = 0;
                        }
                    }
                    ele_tooltip.find('.tooltip-inner').css('top', y);
                });
            } else {
                $('.wrap-parents .swiper-slide a').tooltip({
                    container: '.wrap-parents .swiper-container',
                    placement: 'top',
                    html: true
                });

                $('.wrap-parents .swiper-slide a').on('shown.bs.tooltip', function () {

                    var ele = $('.wrap-parents .swiper-container');
                    var ele_tooltip = $('.wrap-parents .swiper-container .tooltip.show');
                    var x = getValues(ele_tooltip)[0];

                    if (x < 0) {
                        x = (-1) * x;
                    } else {
                        var flag = (x + ele_tooltip.outerWidth()) - ele.outerWidth();
                        if (flag > 0) {
                            x = (-1) * (flag);
                        } else {
                            x = 0;
                        }
                    }
                    ele_tooltip.find('.tooltip-inner').css('left', x);
                });
            }

            $('.wrap-parents .swiper-slide a').on('inserted.bs.tooltip', function () {
                var items = ["orange", "red", "blue", "green"];
                var item = items[Math.floor(Math.random() * items.length)];
                $('.wrap-parents .tooltip .tooltip-inner, .wrap-parents .tooltip .arrow').addClass(item);
            });

            $('.wrap-parents .swiper-slide a').on('hide.bs.tooltip', function () {
                $('.wrap-parents .tooltip .tooltip-inner, .wrap-parents .tooltip .arrow').removeClass("orange red blue green");
            });
        }
    }

    this.homePage = function () {
        self.antHome();

        self.onlyApax();

        self.jsAngle();

        self.onlyCourses();

        self.onlyParent();

        $(document).on('click', '.group-content .btn--red', function () {
            $('.box-register .title').trigger('click');
            return false;
        });

        var swiper_meeting = new Swiper('.wrap-meeting .swiper-container', {
            slidesPerView: 5,
            spaceBetween: 30,
            navigation: {
                nextEl: '.wrap-meeting .swiper-button-next',
                prevEl: '.wrap-meeting .swiper-button-prev'
            },
            breakpoints: {
                460: {
                    slidesPerView: 2,
                    spaceBetween: 10
                },
                640: {
                    slidesPerView: 3,
                    spaceBetween: 10
                },
                990: {
                    slidesPerView: 4,
                    spaceBetween: 10
                }
            }
        });

        var ele_dsc = $('.wrap-meeting .box-text');
        $(document).on('click', '.wrap-meeting .item-meeting', function () {
            var that = $(this);
            var html_dsc = that.find('.dsc').html();
            if (!that.hasClass('active')) {
                $('.wrap-meeting .item-meeting').removeClass('active');
                that.addClass('active');
                ele_dsc.html(html_dsc);
            }
            return false;
        });

        var ele_play = $('.wrap-angle .btn-play');
        var ele_video = $('.wrap-angle .item-demo__video');
        var ele_content = $('.wrap-angle .item-demo__video span');
        var ele_iframe = $('.wrap-angle .item-demo__video .box-iframe');

        $(document).on('click', '.wrap-angle .btn-play', function () {
            var that = $(this);
            var id = that.attr('data-id');
            if (id != null) {
                ele_iframe.append('<iframe width="100%" height="100%" src="https://www.youtube.com/embed/' + id + '" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>').addClass('show');
            }
            return false;
        });

        $(document).on('click', '.wrap-angle .item-demo__list a', function () {
            var that = $(this);
            var id = that.attr('data-id');
            var src = that.find('img').attr('src');
            var dsc = that.find('span').html();
            if (!that.hasClass('active')) {
                that.siblings().removeClass('active');
                that.addClass('active');
                ele_iframe.removeClass('show').html('');
                ele_content.html(dsc);
                ele_play.attr('data-id', id);
                ele_video.css('background-image', 'url(' + src + ')');
            }
            return false;
        });

    }

    this.aboutPage = function () {
        self.onlyApax();
    }

    this.diffPage = function () {
        self.onlyApax();

        self.onlyParent();

        self.onlyCourses();

        var ele_play = $('.wrap-demo .btn-play');
        var ele_video = $('.wrap-demo .item-demo__video');
        var ele_content = $('.wrap-demo .item-demo__video span');
        var ele_iframe = $('.wrap-demo .item-demo__video .box-iframe');

        $(document).on('click', '.wrap-demo .btn-play', function () {
            var that = $(this);
            var id = that.attr('data-id');
            if (id != null) {
                ele_iframe.append('<iframe width="100%" height="100%" src="https://www.youtube.com/embed/' + id + '" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>').addClass('show');
            }
            return false;
        });

        $(document).on('click', '.wrap-demo .item-demo__list a', function () {
            var that = $(this);
            var id = that.attr('data-id');
            var src = that.find('img').attr('src');
            var dsc = that.find('span').html();
            if (!that.hasClass('active')) {
                that.siblings().removeClass('active');
                that.addClass('active');
                ele_iframe.removeClass('show').html('');
                ele_content.html(dsc);
                ele_play.attr('data-id', id);
                ele_video.css('background-image', 'url(' + src + ')');
            }
            return false;
        });
    }

    this.studyPage = function () {
        self.onlyApax();

        var swiper_study = new Swiper('.wrap-study .swiper-container', {
            navigation: {
                nextEl: '.wrap-study .swiper-button-next',
                prevEl: '.wrap-study .swiper-button-prev'
            }
        });
    }

    this.routePage = function () {
        $(document).on('click', '.wrap-bottom .btn--red', function () {
            $('.box-register .title').trigger('click');
            return false;
        });
        function slideEndOnlyApax() {
            var index = swiper_only_apax.activeIndex;
            tab_only.removeClass('active').eq(index).addClass('active');
        }
        var swiper_study = new Swiper('.wrap-study .swiper-container', {
            navigation: {
                nextEl: '.wrap-study .swiper-button-next',
                prevEl: '.wrap-study .swiper-button-prev'
            },
            on: {
                slideChangeTransitionEnd: slideEndOnlyApax
            }
        });
    }

    this.gocApaxPage = function () {

        self.jsAngle();

        var ele_play = $('.wrap-angle .btn-play');
        var ele_video = $('.wrap-angle .item-demo__video');
        var ele_content = $('.wrap-angle .item-demo__video span');
        var ele_iframe = $('.wrap-angle .item-demo__video .box-iframe');

        $(document).on('click', '.wrap-angle .btn-play', function () {
            var that = $(this);
            var id = that.attr('data-id');
            if (id != null) {
                ele_iframe.append('<iframe width="100%" height="100%" src="https://www.youtube.com/embed/' + id + '" frameborder="0" allow="autoplay; encrypted-media" allowfullscreen></iframe>').addClass('show');
            }
            return false;
        });

        $(document).on('click', '.wrap-angle .item-demo__list a', function () {
            var that = $(this);
            var id = that.attr('data-id');
            var src = that.find('img').attr('src');
            var dsc = that.find('span').html();
            if (!that.hasClass('active')) {
                that.siblings().removeClass('active');
                that.addClass('active');
                ele_iframe.removeClass('show').html('');
                ele_content.html(dsc);
                ele_play.attr('data-id', id);
                ele_video.css('background-image', 'url(' + src + ')');
            }
            return false;
        });

        var swiper_parents = new Swiper('.wrap-parents .swiper-container', {
            slidesPerView: 4,
            slidesPerColumn: 3,
            spaceBetween: 0,
            navigation: {
                nextEl: '.wrap-parents .swiper-button-next',
                prevEl: '.wrap-parents .swiper-button-prev'
            },
            breakpoints: {
                768: {
                    slidesPerView: 3,
                    slidesPerColumn: 4
                }
            }
        });

        ww = $(window).width();
        resizeTooltip();
        $(window).resize(function () {
            ww = $(this).width();
            resizeTooltip();
        });

        function resizeTooltip() {
            $('.wrap-parents .swiper-slide a').tooltip('dispose');
            if (ww > 768) {

                $('.wrap-parents .swiper-slide a').tooltip({
                    container: '.wrap-parents .swiper-container',
                    placement: 'left',
                    html: true
                });

                $('.wrap-parents .swiper-slide a').on('shown.bs.tooltip', function () {

                    var ele = $('.wrap-parents .swiper-container');
                    var ele_tooltip = $('.wrap-parents .swiper-container .tooltip.show');
                    var y = getValues(ele_tooltip)[1];

                    if (y < 0) {
                        y = (-1) * y;
                    } else {
                        var flag = (y + ele_tooltip.outerHeight()) - ele.outerHeight();
                        if (flag > 0) {
                            y = (-1) * (flag);
                        } else {
                            y = 0;
                        }
                    }
                    ele_tooltip.find('.tooltip-inner').css('top', y);
                });
            } else {
                $('.wrap-parents .swiper-slide a').tooltip({
                    container: '.wrap-parents .swiper-container',
                    placement: 'top',
                    html: true
                });

                $('.wrap-parents .swiper-slide a').on('shown.bs.tooltip', function () {

                    var ele = $('.wrap-parents .swiper-container');
                    var ele_tooltip = $('.wrap-parents .swiper-container .tooltip.show');
                    var x = getValues(ele_tooltip)[0];

                    if (x < 0) {
                        x = (-1) * x;
                    } else {
                        var flag = (x + ele_tooltip.outerWidth()) - ele.outerWidth();
                        if (flag > 0) {
                            x = (-1) * (flag);
                        } else {
                            x = 0;
                        }
                    }
                    ele_tooltip.find('.tooltip-inner').css('left', x);
                });
            }

            $('.wrap-parents .swiper-slide a').on('inserted.bs.tooltip', function () {
                var items = ["orange", "red", "blue", "green"];
                var item = items[Math.floor(Math.random() * items.length)];
                $('.wrap-parents .tooltip .tooltip-inner, .wrap-parents .tooltip .arrow').addClass(item);
            });


            $('.wrap-parents .swiper-slide a').on('hide.bs.tooltip', function () {
                $('.wrap-parents .tooltip .tooltip-inner, .wrap-parents .tooltip .arrow').removeClass("orange red blue green");
            });
        }


        var swiper_meeting = new Swiper('.wrap-meeting .swiper-container', {
            slidesPerView: 5,
            spaceBetween: 30,
            slidesPerColumn: 2,
            navigation: {
                nextEl: '.wrap-meeting .swiper-button-next',
                prevEl: '.wrap-meeting .swiper-button-prev'
            },
            breakpoints: {
                768: {
                    slidesPerView: 2,
                    slidesPerColumn: 5,
                    spaceBetween: 10
                }
            }
        });


        $('.wrap-meeting .swiper-slide[data-swiper-row=1] .item-meeting').tooltip({
            container: '.wrap-meeting .swiper-container',
            placement: 'bottom',
            html: true
        });

        $('.wrap-meeting .swiper-slide[data-swiper-row=0] .item-meeting').tooltip({
            container: '.wrap-meeting .swiper-container',
            placement: 'top',
            html: true
        });

        $('.wrap-meeting .item-meeting').on('shown.bs.tooltip', function () {

            var ele = $('.wrap-meeting .swiper-container');
            var ele_tooltip = $('.wrap-meeting .swiper-container .tooltip.show');
            var x = getValues(ele_tooltip)[0];

            if (x < 0) {
                x = (-1) * x;
            } else {
                var flag = (x + ele_tooltip.outerWidth()) - ele.outerWidth();
                if (flag > 0) {
                    x = (-1) * (flag);
                } else {
                    x = 0;
                }
            }
            ele_tooltip.find('.tooltip-inner').css('left', x);
        });

        $('.wrap-meeting .item-meeting').on('inserted.bs.tooltip', function () {
            var items = ["orange", "red", "blue", "green"];
            var item = items[Math.floor(Math.random() * items.length)];
            $('.wrap-meeting .tooltip .tooltip-inner, .wrap-meeting .tooltip .arrow').addClass(item);
        });


        $('.wrap-meeting .item-meeting').on('hide.bs.tooltip', function () {
            $('.wrap-meeting .tooltip .tooltip-inner, .wrap-meeting .tooltip .arrow').removeClass("orange red blue green");
        });

    }
}

var PageFunction = new PageFunction();

$(document).ready(function () {
    $(window).on('load', function () {
        PageFunction.init();
    });
});

function getValues(el) {
    var transform = el.css('transform');
    if (typeof transform != 'undefined') {
        var matrix = transform.replace(/[^0-9\-.,]/g, '').split(',');
        var x = matrix[12] || matrix[4];
        var y = matrix[13] || matrix[5];
        return [parseInt(x), parseInt(y)];
    }
    return 0;
};

var didScroll;
var lastScrollTop = 0;
var delta = 50;
var time_s;

function hasScrolled() {
    var navbarHeight = $(".header").height();
    var st = $(window).scrollTop();
    if (Math.abs(lastScrollTop - st) <= delta)
        return;
    if (st > lastScrollTop && st > navbarHeight) {
        $(".header").addClass("hide-nav-bar");
    } else {
        if (st + $(window).height() < $(document).height()) {
            $(".header").removeClass("hide-nav-bar");
        }
    }

    lastScrollTop = st;
}


