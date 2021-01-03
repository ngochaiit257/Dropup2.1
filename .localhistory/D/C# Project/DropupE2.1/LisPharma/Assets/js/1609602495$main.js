(function ($) {
    "use strict"; jQuery(document).on('ready', function () {
        $(window).on('scroll', function () {
            if ($(this).scrollTop() > 120) { $('.navbar-area').addClass("is-sticky"); }
            else { $('.navbar-area').removeClass("is-sticky"); }
        }); jQuery('.mean-menu').meanmenu({ meanScreenWidth: "991" }); $('.others-option .close-btn').on('click', function () { $('.search-overlay').fadeOut(); $('.search-btn').show(); $('.close-btn').removeClass('active'); }); $('.others-option .search-btn').on('click', function () { $(this).hide(); $('.search-overlay').fadeIn(); $('.close-btn').addClass('active'); }); $(".burger-menu").on('click', function () { $('.sidebar-modal').toggleClass('active'); }); $(".sidebar-modal-close-btn").on('click', function () { $('.sidebar-modal').removeClass('active'); }); $('.home-slides').owlCarousel({ loop: true, nav: true, dots: false, autoplayHoverPause: true, autoplay: false, items: 1, animateOut: 'fadeOut', navText: ["<i class='flaticon-back'></i>", "<i class='flaticon-next-1'></i>"] }); $(".home-slides").on("translate.owl.carousel", function () { $(".main-banner-content .sub-title").removeClass("animated fadeInLeft").css("opacity", "0"); $(".main-banner-content h1").removeClass("animated fadeInLeft").css("opacity", "0"); $(".main-banner-content p").removeClass("animated fadeInUp").css("opacity", "0"); $(".main-banner-content .btn-box .btn").removeClass("animated fadeIn").css("opacity", "0"); $(".main-banner-content .btn-box .video-btn").removeClass("animated fadeIn").css("opacity", "0"); }); $(".home-slides").on("translated.owl.carousel", function () { $(".main-banner-content .sub-title").addClass("animated fadeInLeft").css("opacity", "1"); $(".main-banner-content h1").addClass("animated fadeInLeft").css("opacity", "1"); $(".main-banner-content p").addClass("animated fadeInUp").css("opacity", "1"); $(".main-banner-content .btn-box .btn").addClass("animated fadeIn").css("opacity", "1"); $(".main-banner-content .btn-box .video-btn").addClass("animated fadeIn").css("opacity", "1"); }); $('.why-choose-us-slides').owlCarousel({ loop: true, nav: true, dots: true, autoplayHoverPause: true, autoplay: false, items: 1, animateOut: 'fadeOut', navText: ["<i class='flaticon-back'></i>", "<i class='flaticon-next-1'></i>"] }); $('.popup-youtube').magnificPopup({ disableOn: 320, type: 'iframe', mainClass: 'mfp-fade', removalDelay: 160, preloader: false, fixedContentPos: false }); $('.odometer').appear(function (e) { var odo = $(".odometer"); odo.each(function () { var countNumber = $(this).attr("data-count"); $(this).html(countNumber); }); }); $(function () { $('.accordion').find('.accordion-title').on('click', function () { $(this).toggleClass('active'); $(this).next().slideToggle('fast'); $('.accordion-content').not($(this).next()).slideUp('fast'); $('.accordion-title').not($(this)).removeClass('active'); }); }); $('.doctor-slides').owlCarousel({ loop: true, nav: false, dots: true, autoplayHoverPause: true, autoplay: false, margin: 30, navText: ["<i class='flaticon-back'></i>", "<i class='flaticon-next-1'></i>"], responsive: { 0: { items: 1, }, 576: { items: 2, }, 768: { items: 3, }, 1200: { items: 4, } } }); $('.feedback-slides').owlCarousel({ loop: true, nav: false, dots: true, autoplayHoverPause: true, autoplay: false, items: 1, navText: ["<i class='flaticon-back'></i>", "<i class='flaticon-next-1'></i>"] }); $('select').niceSelect(); $('.partner-slides').owlCarousel({ loop: true, nav: false, dots: false, autoplayHoverPause: true, autoplay: false, margin: 30, navText: ["<i class='flaticon-back'></i>", "<i class='flaticon-next-1'></i>"], responsive: { 0: { items: 2, }, 576: { items: 3, }, 768: { items: 3, }, 1200: { items: 5, } } }); $('.services-details-image-slides').owlCarousel({ loop: true, nav: true, dots: true, autoplayHoverPause: true, autoplay: false, items: 1, animateOut: 'fadeOut', navText: ["<i class='flaticon-back'></i>", "<i class='flaticon-next-1'></i>"] }); $('.research-details-image-slides').owlCarousel({ loop: true, nav: true, dots: true, autoplayHoverPause: true, autoplay: false, items: 1, animateOut: 'fadeOut', navText: ["<i class='flaticon-back'></i>", "<i class='flaticon-next-1'></i>"] }); var $imagesSlider = $(".testimonial-slides .client-feedback>div"), $thumbnailsSlider = $(".client-thumbnails>div"); $imagesSlider.slick({ speed: 300, slidesToShow: 1, slidesToScroll: 1, cssEase: 'linear', fade: true, autoplay: false, draggable: true, asNavFor: ".client-thumbnails>div", prevArrow: '.client-feedback .prev-arrow', nextArrow: '.client-feedback .next-arrow' }); $thumbnailsSlider.slick({ speed: 300, slidesToShow: 5, slidesToScroll: 1, cssEase: 'linear', autoplay: false, centerMode: true, draggable: false, focusOnSelect: true, asNavFor: ".testimonial-slides .client-feedback>div", prevArrow: '.client-thumbnails .prev-arrow', nextArrow: '.client-thumbnails .next-arrow', }); $(".newsletter-form").validator().on("submit", function (event) { if (event.isDefaultPrevented()) { formErrorSub(); submitMSGSub(false, "Please enter your email correctly."); } else { event.preventDefault(); } }); function callbackFunction(resp) {
            if (resp.result === "success") { formSuccessSub(); }
            else { formErrorSub(); }
        }
        function formSuccessSub() { $(".newsletter-form")[0].reset(); submitMSGSub(true, "Thank you for subscribing!"); setTimeout(function () { $("#validator-newsletter").addClass('hide'); }, 4000) }
        function formErrorSub() { $(".newsletter-form").addClass("animated shake"); setTimeout(function () { $(".newsletter-form").removeClass("animated shake"); }, 1000) }
        function submitMSGSub(valid, msg) {
            if (valid) { var msgClasses = "validation-success"; } else { var msgClasses = "validation-danger"; }
            $("#validator-newsletter").removeClass().addClass(msgClasses).text(msg);
        }
        $(".newsletter-form").ajaxChimp({ url: "https://envytheme.us20.list-manage.com/subscribe/post?u=60e1ffe2e8a68ce1204cd39a5&amp;id=42d6d188d9", callback: callbackFunction }); $(function () { $(window).on('scroll', function () { var scrolled = $(window).scrollTop(); if (scrolled > 600) $('.go-top').addClass('active'); if (scrolled < 600) $('.go-top').removeClass('active'); }); $('.go-top').on('click', function () { $("html, body").animate({ scrollTop: "0" }, 500); }); }); $(function () { $(".twentytwenty-container[data-orientation!='vertical']").twentytwenty({ default_offset_pct: 0.5 }); });
    }); $(window).on('load', function () { if ($(".wow").length) { var wow = new WOW({ boxClass: 'wow', animateClass: 'animated', offset: 20, mobile: true, live: true, }); wow.init(); } }); jQuery(window).on('load', function () { $('.preloader').fadeOut(); });
}(jQuery));


$('.doctor-slides').owlCarousel({ loop: true, nav: false, dots: true, autoplayHoverPause: true, autoplay: false, margin: 30, navText: ["<i class='flaticon-back'></i>", "<i class='flaticon-next-1'></i>"], responsive: { 0: { items: 1, }, 576: { items: 2, }, 768: { items: 3, }, 1200: { items: 4, } } });

// SLIDER
var mainslider = new Swiper('.gallery-top', {
    spaceBetween: 0,
    autoplay: {
        delay: 9500,
        disableOnInteraction: false,
    },
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
    pagination: {
        el: '.swiper-pagination',
        type: 'progressbar',
    },
    loop: true,
    loopedSlides: 3,
    thumbs: {
        swiper: sliderthumbs
    }
});



// SLIDER THUMBS
var sliderthumbs = new Swiper('.gallery-thumbs', {
    spaceBetween: 10,
    centeredSlides: true,
    slidesPerView: 3,
    touchRatio: 0.2,
    slideToClickedSlide: true,
    loop: true,
    loopedSlides: 3,
    breakpoints: {
        1024: {
            slidesPerView: 3
        },
        768: {
            slidesPerView: 1
        },
        640: {
            slidesPerView: 1
        },
        320: {
            slidesPerView: 1
        }
    }
});

if ($(".gallery-top")[0]) {
    mainslider.controller.control = sliderthumbs;
    sliderthumbs.controller.control = mainslider;
}
else {

}




// OFFICE SLIDER
new Swiper('.office-slider', {
    slidesPerView: '1',
    spaceBetween: 0,
    centeredSlides: true,
    loop: true,
    pagination: {
        el: '.swiper-pagination',
        clickable: true,
    },
});




// SLIDER
var carouselslider = new Swiper('.carousel-slider', {
    spaceBetween: 0,
    slidesPerView: 3,
    centeredSlides: true,
    autoplay: {
        delay: 9500,
        disableOnInteraction: false,
    },
    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev',
    },
    pagination: {
        el: '.swiper-pagination',
        type: 'progressbar',
    },
    loop: true,
    breakpoints: {
        1024: {
            slidesPerView: 3
        },
        768: {
            slidesPerView: 2
        },
        640: {
            slidesPerView: 1
        },
        320: {
            slidesPerView: 1
        }
    }
});