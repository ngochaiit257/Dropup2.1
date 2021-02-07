(function ($) {
    'use strict';

    // preloader
    $(window).on('load', function () {
        setTimeout (function(){
            $('.preloader').fadeOut(200);
        },50);
    });

    // scroll to top init
    $(window).on('scroll', function () {
        var scrollToTop = $('.scroll-to-top'),
            scroll = $(window).scrollTop();
        if (scroll >= 500) {scrollToTop.addClass('show');}
        else {scrollToTop.removeClass('show');}
    });
    $('.scroll-to-top').each(function(){
        $(this).click(function(){ 
            $('html,body').animate({ scrollTop: 0 }, '500');
            return false; 
        });
    });

    // dropdown-animation
    if ($(window).width() < 1199) {
        $('header .dropdown-toggle').on('click', function(e) {
            $(this).siblings('.dropdown-menu, .dropdown-submenu').animate({
                height: 'toggle'
            }, 300);
        });
    }

    // custom select
    $('.navbar-toggler').on('click', function(){
        $(this).children('span').toggleClass('fa-times fa-bars');
    });

    // custom select
    $('select:not(.custom-select)').niceSelect();

    // js counterUp
    $('.jsCounter').counterUp({
        delay: 10,
        time: 1000
    });

    // tutorCarousel Init
    $('.tutor-carousel').owlCarousel({
        loop: true,
        margin: 30,
        nav: false,
        dots: true,
        dotsEach: true,
        responsive : {
            0 : {
                items: 1
            },
            991 : {
                items: 2
            }
        }
    });

    // tutorsCarousel-alt Init
    $('.tutors-carousel-alt').owlCarousel({
        loop: true,
        margin: 30,
        nav: false,
        dots: true,
        dotsEach: true,
        responsive : {
            0 : {
                items: 1
            },
            767 : {
                items: 2
            },
            1200 : {
                items: 3
            }
        }
    });
    

    // courseCarousel Init
    var courseOwl = $('.course-carousel');
    courseOwl.owlCarousel({
        loop: true,
        margin: 30,
        nav: false,
        dots: false,
        responsive : {
            0 : {
                items: 1
            },
            767 : {
                items: 2
            },
            991 : {
                items: 3
            },
            1200 : {
                items: 4
            }
        }
    });
    // Custom Navigation Events
    $('.course-left').click(function(){
        courseOwl.trigger('prev.owl.carousel');
    });
    $('.course-right').click(function(){
        courseOwl.trigger('next.owl.carousel');
    });


    // galleryCarousel Init
    var galleryOwl = $('.image-gallery');
    galleryOwl.owlCarousel({
        loop: true,
        margin: 30,
        nav: false,
        dots: false,
        autoplay: true,
        items: 1
    });
    // Custom Navigation Events
    $('.gallery-left').click(function(){
        galleryOwl.trigger('prev.owl.carousel');
    });
    $('.gallery-right').click(function(){
        galleryOwl.trigger('next.owl.carousel');
    });


    // student-says-carousel Init
    var studentSays = $('.student-says-carousel');
    studentSays.owlCarousel({
        loop: true,
        margin: 30,
        nav: false,
        dots: false,
        items: 1
    });
    // Custom Navigation Events
    $('.student-says-left').click(function(){
        studentSays.trigger('prev.owl.carousel');
    });
    $('.student-says-right').click(function(){
        studentSays.trigger('next.owl.carousel');
    });


    // instructors-carousel Init
    var instructorsCarousel = $('.instructors-carousel');
    instructorsCarousel.owlCarousel({
        loop: true,
        margin: 30,
        nav: false,
        dots: false,
        responsive : {
            0 : {
                items: 1
            },
            575 : {
                items: 2
            },
            991 : {
                items: 3
            }
        }
    });
    // Custom Navigation Events
    $('.instructors-left').click(function(){
        instructorsCarousel.trigger('prev.owl.carousel');
    });
    $('.instructors-right').click(function(){
        instructorsCarousel.trigger('next.owl.carousel');
    });


    // video popup init
    if ($('.has-video-popup').length > 0) {
        $('.has-video-popup').magnificPopup({
            type: 'iframe',
            mainClass: 'mfp-fade',
            removalDelay: 160,
            preloader: false,
            fixedContentPos: false
        });
    }

})(jQuery);