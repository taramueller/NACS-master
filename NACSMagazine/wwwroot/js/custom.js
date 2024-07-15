 $( document ).ready(function () {    
    $("#loadMore").on('click', function (e) {
      e.preventDefault();
      $(".dept-hidden").removeClass('d-none');
        $("#loadMore").addClass('d-none');
    });

      $('.navbar-toggler.mobile').on('click', function(e) {
         $('.open').toggleClass("inactive");
         $('.close').toggleClass("active");
         e.preventDefault();
      });
  });

 $( document ).ready(function() { 
  $slider = $('.article-slider');
  if ($slider.length) {
    var currentSlide;
    var slidesCount;
    var sliderCounter = document.createElement('div');
    sliderCounter.classList.add('slider-counter');
    
    var updateSliderCounter = function(slick, currentIndex) {
      currentSlide = slick.slickCurrentSlide() + 1;
      slidesCount = slick.slideCount;
      $(sliderCounter).text(currentSlide + ' of ' +slidesCount)
    };

    $slider.on('init', function(event, slick) {
      $slider.prepend(sliderCounter);
      updateSliderCounter(slick);
    });

    $slider.on('afterChange', function(event, slick, currentSlide) {
      updateSliderCounter(slick, currentSlide);
    });

    $slider.slick({
      arrows: true,
      dots: false,
      infinite: true,
      slidesToShow: 1,
      slidesToScroll: 1,
      speed: 300,
      mobileFirst: true,
      swipe: true,
      fade: true,
      autoplay: false,
      adaptiveHeight: false,
      // prettier-ignore
      prevArrow: "<a class='slick-prev'><img src=\"/App_Themes/NACSMagazine/img/icon/icon-slider-left.png\">",
      // prettier-ignore
      nextArrow: "<a class='slick-next'><img src=\"/App_Themes/NACSMagazine/img/icon/icon-slider-right.png\">",
    });
  }
 });  

 $(document).ready(function() {

       $('.slider-programs').slick({
            arrows: true,
            dots: true,
            infinite: true,
            slidesToShow: 1,
            slidesToScroll: 1,
            speed: 300,
            mobileFirst: true,
            swipe: true,
            fade: false,
            autoplay: false,
            adaptiveHeight: false,
            // prettier-ignore
            prevArrow: "<a class='slick-prev'><svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 27 57\" style=\"enable-background:new 0 0 27 57\" xml:space=\"preserve\"><path d=\"M26.6 56.3c-.4.3-.9.5-1.3.5-.5 0-1.1-.2-1.5-.7L1 31.1c-1.4-1.5-1.4-3.9 0-5.4L23.8.7c.7-.8 2-.9 2.8-.1.8.7.9 2 .1 2.8L4 28.4l22.8 25.1c.7.8.7 2-.2 2.8z\"/></svg>",
            // prettier-ignore
            nextArrow: "<a class='slick-next'><svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 27 57\" xml:space=\"preserve\"><path d=\"M.7.5C1 .2 1.5 0 2 0s1.1.2 1.5.7l22.8 25c1.4 1.5 1.4 3.9 0 5.4L3.5 56.2c-.7.8-2 .9-2.8.1-.8-.7-.9-2-.1-2.8l22.8-25.1L.5 3.3c-.7-.8-.7-2 .2-2.8z\"/></svg>",
            responsive: [
               {
                  breakpoint: 768,
                  settings: {
                     slidesToShow: 2,
                  },
               },
               {
                  breakpoint: 1024,
                  settings: {
                     slidesToShow: 3,
                  },
               },
            ],
      });

     });

 $(document).ready(function(){
  $('#search-toggle').on('click', function () {
    $('#search-bar').toggleClass('is-hidden');
  });
 });