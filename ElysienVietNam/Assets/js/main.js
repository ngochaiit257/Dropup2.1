(function ($) { "use strict"; jQuery(document).ready(function ($) { $("select").niceSelect(); var iconCart = $(".mini-cart-warp"); iconCart.on("click", function () { $(".mini-cart-content").toggleClass("cart-visible") }); $(".search_list > a").on("click", function () { $(this).toggleClass("active"); $(".dropdown_search").slideToggle("medium") }); $("#mobile-menu-active").meanmenu({ meanScreenWidth: "991", meanMenuContainer: ".mobile-menu" }); $(".slider-active-3").owlCarousel({ loop: true, nav: false, dots: true, autoplay: true, autoplayTimeout: 5e3, animateOut: "fadeOut", animateIn: "fadeIn", item: 1, responsive: { 0: { items: 1 }, 768: { items: 1 }, 1e3: { items: 1 } } }); $(".slider-home-16").owlCarousel({ loop: true, nav: true, dots: false, autoplay: true, autoplayTimeout: 5e3, animateOut: "fadeOut", animateIn: "fadeIn", item: 1, responsive: { 0: { items: 1 }, 768: { items: 1 }, 1e3: { items: 1 } } }); $(".best-sell-slider").owlCarousel({ autoplay: false, loop: false, smartSpeed: 1e3, nav: true, dots: false, margin: 30, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 500: { items: 2, autoplay: true, loop: true }, 768: { items: 3 }, 992: { items: 4 }, 1200: { items: 5 } } }); $(".best-sell-slider-2").owlCarousel({ autoplay: false, loop: false, smartSpeed: 1e3, nav: true, dots: false, margin: 0, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 500: { items: 2, autoplay: true, loop: true }, 768: { items: 3 }, 992: { items: 4 }, 1200: { items: 5 } } }); $(".category-slider").owlCarousel({ autoplay: false, smartSpeed: 1e3, loop: false, nav: true, dots: false, margin: 30, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 576: { items: 1, autoplay: true, loop: true }, 767: { items: 2 }, 992: { items: 3 }, 1200: { items: 3 } } }); $(".category-slider-2").owlCarousel({ autoplay: false, loop: false, smartSpeed: 1e3, nav: true, dots: false, margin: 30, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 500: { items: 2, autoplay: true, loop: true }, 768: { items: 3 }, 992: { items: 4 }, 1200: { items: 5 } } }); $(".hot-deal").owlCarousel({ autoplay: false, smartSpeed: 1e3, nav: true, loop: false, dots: false, items: 1, margin: 0, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true } } }); $(".new-product-slider").owlCarousel({ autoplay: false, smartSpeed: 1e3, nav: true, loop: false, dots: false, items: 4, margin: 30, responsive: { 0: { items: 2, autoplay: true, loop: true }, 360: { items: 2, autoplay: true, loop: true }, 500: { items: 2, autoplay: true, loop: true }, 768: { items: 2 }, 992: { items: 2 }, 1024: { items: 2 }, 1200: { items: 3 }, 1300: { items: 4 } } }); $(".new-product-slider-2").owlCarousel({ autoplay: false, smartSpeed: 1e3, nav: true, loop: false, dots: false, items: 4, margin: 0, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 500: { items: 2, autoplay: true, loop: true }, 768: { items: 2 }, 992: { items: 2 }, 1024: { items: 2 }, 1200: { items: 3 }, 1300: { items: 4 } } }); $(".feature-slider").owlCarousel({ autoplay: false, smartSpeed: 1e3, nav: true, loop: false, dots: false, items: 4, margin: 30, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 576: { items: 1, autoplay: true, loop: true }, 768: { items: 2 }, 992: { items: 3 }, 1200: { items: 3 }, 1300: { items: 4 } } }); $(".recent-product-slider").owlCarousel({ autoplay: false, smartSpeed: 1e3, nav: true, loop: false, dots: false, items: 4, margin: 30, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 500: { items: 2, autoplay: true, loop: true }, 768: { items: 2 }, 992: { items: 4 }, 1200: { items: 5 }, 1300: { items: 6 } } }); $(".brand-slider").owlCarousel({ autoplay: false, nav: true, loop: false, smartSpeed: 1e3, dots: false, items: 5, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 576: { items: 2, autoplay: true, loop: true }, 768: { items: 3 }, 992: { items: 4 }, 1200: { items: 5 } } }); $(".testi-slider").owlCarousel({ autoplay: false, nav: false, smartSpeed: 1e3, dots: true, items: 2, margin: 30, responsive: { 0: { items: 1 }, 360: { items: 1, margin: 0 }, 576: { items: 1, margin: 0 }, 768: { items: 1, margin: 0 }, 992: { items: 2 }, 1200: { items: 2 } } }); $(".blog-slider-active").owlCarousel({ autoplay: false, nav: true, smartSpeed: 1e3, dots: false, items: 3, margin: 30, responsive: { 0: { items: 1, autoplay: true }, 360: { items: 1, autoplay: true }, 576: { items: 1, autoplay: true }, 768: { items: 2 }, 992: { items: 2 }, 1200: { items: 3 } } }); $(".feature-slider-2").owlCarousel({ autoplay: false, nav: true, smartSpeed: 1e3, dots: false, items: 2, loop: false, margin: 30, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 576: { items: 1, autoplay: true, loop: true }, 768: { items: 2 }, 992: { items: 2 }, 1e3: { items: 1 }, 1200: { items: 1 }, 1300: { items: 2 } } }); $(".feature-slider-3").owlCarousel({ autoplay: false, smartSpeed: 1e3, nav: true, loop: false, dots: false, items: 3, margin: 0, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 576: { items: 1, autoplay: true, loop: true }, 768: { items: 2 }, 992: { items: 3 }, 1200: { items: 3 }, 1300: { items: 3 } } }); $(".hot-deal-2").owlCarousel({ autoplay: false, nav: true, smartSpeed: 1e3, dots: false, items: 2, loop: false, margin: 30, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 576: { items: 1, autoplay: true, loop: true }, 768: { items: 1 }, 992: { items: 1 }, 1200: { items: 2 } } }); $(".hot-deal-3").owlCarousel({ autoplay: false, nav: true, smartSpeed: 1e3, dots: false, items: 1, loop: false, margin: 30, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 576: { items: 1, autoplay: true, loop: true }, 768: { items: 1 }, 992: { items: 1 }, 1200: { items: 1 } } }); $(".slider-nav").slick({ slidesToShow: 3, slidesToScroll: 1, asNavFor: ".slider-main", vertical: true, focusOnSelect: true, autoplay: false, arrows: true, dots: false, margin: 10 }); $(".slider-main").slick({ slidesToShow: 1, slidesToScroll: 1, arrows: false, asNavFor: ".slider-nav", autoplay: false, vertical: true, verticalSwiping: true, arrows: false, dots: false }); $(".category-product-slider").owlCarousel({ autoplay: false, smartSpeed: 1e3, nav: true, loop: false, dots: false, items: 4, margin: 30, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 576: { items: 1, autoplay: true, loop: true }, 768: { items: 2 }, 992: { items: 1 }, 1200: { items: 1 } } }); $(".single-product-slider-active").owlCarousel({ autoplay: false, smartSpeed: 1e3, nav: true, loop: true, dots: false, items: 4, margin: 30, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 576: { items: 2, autoplay: true, loop: true }, 768: { items: 3 }, 992: { items: 3 }, 1200: { items: 4 } } }); $(".category-product-2").owlCarousel({ autoplay: false, smartSpeed: 1e3, nav: true, loop: false, dots: false, items: 1, margin: 30, responsive: { 0: { items: 1, autoplay: true, loop: true }, 360: { items: 1, autoplay: true, loop: true }, 576: { items: 1, autoplay: true, loop: true }, 768: { items: 1 }, 992: { items: 1 }, 1200: { items: 1 } } }); $(".zoompro").elevateZoom({ gallery: "gallery", galleryActiveClass: "active", zoomWindowWidth: 300, zoomWindowHeight: 100, scrollZoom: false, zoomType: "inner", cursor: "crosshair" }); $(".product-dec-slider-2").slick({ infinite: true, slidesToShow: 4, arrows: false, slidesToScroll: 1, responsive: [{ breakpoint: 992, Settings: { slidesToShow: 4, slidesToScroll: 1 } }, { breakpoint: 767, Settings: { slidesToShow: 4, slidesToScroll: 1 } }, { breakpoint: 479, Settings: { slidesToShow: 2, slidesToScroll: 1 } }] }); $(".product-dec-slider-3").slick({ infinite: true, slidesToShow: 4, arrows: false, vertical: true, slidesToScroll: 1, responsive: [{ breakpoint: 992, Settings: { slidesToShow: 4, slidesToScroll: 1 } }, { breakpoint: 767, Settings: { slidesToShow: 4, slidesToScroll: 1 } }, { breakpoint: 479, Settings: { slidesToShow: 2, slidesToScroll: 1 } }] }); $(".blog-gallery").slick({ dots: false, infinite: true, arrows: true, prevArrow: '<span class="prev"><i class="ion-ios-arrow-left"></i></span>', nextArrow: '<span class="next"><i class="ion-ios-arrow-right"></i></span>', speed: 800, slidesToShow: 1, slidesToScroll: 1 }); $(".quickview-slide-active").owlCarousel({ loop: false, margin: 15, smartSpeed: 1e3, nav: true, dots: false, responsive: { 0: { items: 3, autoplay: true, smartSpeed: 300 }, 576: { items: 3 }, 768: { items: 3 }, 1e3: { items: 3 } } }); $(".quickview-slide-active a").on("click", function () { $(".quickview-slide-active a").removeClass("active") }); $.scrollUp({ scrollText: '<i class="fa fa-arrow-up"></i>', easingType: "linear", scrollSpeed: 900, animation: "fade" }); var CartPlusMinus = $(".cart-plus-minus"); CartPlusMinus.prepend('<div class="dec qtybutton">-</div>'); CartPlusMinus.append('<div class="inc qtybutton">+</div>'); $(".qtybutton").on("click", function () { var $button = $(this); var oldValue = $button.parent().find("input").val(); if ($button.text() === "+") { var newVal = parseFloat(oldValue) + 1 } else { if (oldValue > 1) { var newVal = parseFloat(oldValue) - 1 } else { newVal = 1 } } $button.parent().find("input").val(newVal) }); $(".checkout-toggle2").on("click", function () { $(".open-toggle2").slideToggle(1e3) }); $(".checkout-toggle").on("click", function () { $(".open-toggle").slideToggle(1e3) }); $(".vertical-menu-toggle").on("click", function () { $(".open-menu-toggle").slideToggle(500) }); $(".vertical-menu li.hidden").hide(); $("#more-btn").on("click", function (e) { e.preventDefault(); $(".vertical-menu li.hidden").toggle(500); var htmlAfter = '<i class="ion-ios-minus-empty" aria-hidden="true"></i> Less Categories'; var htmlBefore = '<i class="ion-ios-plus-empty" aria-hidden="true"></i> More Categories'; if ($(this).html() == htmlBefore) { $(this).html(htmlAfter) } else { $(this).html(htmlBefore) } }); $(".category-toggle").click(function () { $(".category-menu").slideToggle("slow") }); $(".menu-item-has-children-1").click(function () { $(".category-mega-menu-1").slideToggle("slow") }); $(".menu-item-has-children-2").click(function () { $(".category-mega-menu-2").slideToggle("slow") }); $(".menu-item-has-children-3").click(function () { $(".category-mega-menu-3").slideToggle("slow") }); $(".menu-item-has-children-4").click(function () { $(".category-mega-menu-4").slideToggle("slow") }); $(".menu-item-has-children-5").click(function () { $(".category-mega-menu-5").slideToggle("slow") }); $(".menu-item-has-children-6").click(function () { $(".category-mega-menu-6").slideToggle("slow") }); $("[data-countdown]").each(function () { var $this = $(this), finalDate = $(this).data("countdown"); $this.countdown(finalDate, function (event) { $this.html(event.strftime('<span class="cdown day">%-D <p>Days</p></span> <span class="cdown hour">%-H <p>Hours</p></span> <span class="cdown minutes">%M <p>Mins</p></span> <span class="cdown second">%S <p>Sec</p></span>')) }) }) }); $(window).scroll(function () { var window_top = $(window).scrollTop() + 1; if (window_top > 50) { $(".sticky-nav").addClass("menu_fixed animated fadeInDown"); $(".main-navigation ul li .a-submenu").css("color", "#1a395f"); $(".header_account_list a").css("color", "#1a395f"); $(".count-cart").css("color", "#1a395f"); $(".mean-container a.meanmenu-reveal span").css("background", "#1a395f"); $(".mean-container a.meanmenu-reveal").css("border", "1px solid #1a395f"); $(".img-logo").removeClass("brightness") } else { $(".sticky-nav").removeClass("menu_fixed animated fadeInDown"); $(".main-navigation ul li .a-submenu").css("color", "#fff"); $(".header_account_list a").css("color", "#fff"); $(".count-cart").css("color", "#fff"); $(".mean-container a.meanmenu-reveal span").css("background", "#fff"); $(".mean-container a.meanmenu-reveal").css("border", "1px solid #fff"); $(".mean-container a.meanmenu-reveal").css("border", "1px solid #fff"); $(".img-logo").addClass("brightness") } }); $(window).on("load", function (event) { $("#preloader").delay(500).fadeOut(500) }) })(jQuery);