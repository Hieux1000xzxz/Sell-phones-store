document.addEventListener('DOMContentLoaded', function() {
    const slider = document.querySelector('.slider');
    const sliderContainer = slider.querySelector('.slider-container');
    const slides = slider.querySelectorAll('.slide');
    const prevBtn = slider.querySelector('.prev-btn');
    const nextBtn = slider.querySelector('.next-btn');
    const dotsContainer = slider.querySelector('.slider-dots');

    let currentSlide = 0;
    const slideCount = slides.length;

    // Create dots
    slides.forEach((_, index) => {
        const dot = document.createElement('div');
        dot.classList.add('dot');
        if (index === 0) dot.classList.add('active');
        dot.addEventListener('click', () => goToSlide(index));
        dotsContainer.appendChild(dot);
    });

    const dots = dotsContainer.querySelectorAll('.dot');

    // Functions
    function updateSlider() {
        sliderContainer.style.transform = `translateX(-${currentSlide * 100}%)`;
        
        // Update dots
        dots.forEach(dot => dot.classList.remove('active'));
        dots[currentSlide].classList.add('active');
    }

    function nextSlide() {
        currentSlide = (currentSlide + 1) % slideCount;
        updateSlider();
    }

    function prevSlide() {
        currentSlide = (currentSlide - 1 + slideCount) % slideCount;
        updateSlider();
    }

    function goToSlide(index) {
        currentSlide = index;
        updateSlider();
    }

    // Event listeners
    nextBtn.addEventListener('click', nextSlide);
    prevBtn.addEventListener('click', prevSlide);

    // Auto slide
    let slideInterval = setInterval(nextSlide, 5000);

    // Pause auto slide on hover
    slider.addEventListener('mouseenter', () => {
        clearInterval(slideInterval);
    });

    slider.addEventListener('mouseleave', () => {
        slideInterval = setInterval(nextSlide, 5000);
    });

    // Touch events for mobile
    let touchStartX = 0;
    let touchEndX = 0;

    slider.addEventListener('touchstart', e => {
        touchStartX = e.changedTouches[0].screenX;
    });

    slider.addEventListener('touchend', e => {
        touchEndX = e.changedTouches[0].screenX;
        if (touchStartX - touchEndX > 50) {
            nextSlide();
        } else if (touchEndX - touchStartX > 50) {
            prevSlide();
        }
    });
}); 