/* --- IndieVault Carousel Navigation Logic ---*/
document.addEventListener('DOMContentLoaded', function () {
    const navZones = document.querySelectorAll('.iv-nav-zone');

    navZones.forEach(zone => {
        zone.addEventListener('mouseenter', function () {
            // Check if it's the 'prev' (left) or 'next' (right) button
            if (this.classList.contains('carousel-control-prev')) {
                this.classList.add('iv-active-left');
            } else {
                this.classList.add('iv-active-right');
            }
        });

        zone.addEventListener('mouseleave', function () {
            // Remove the shadow classes when mouse leaves
            this.classList.remove('iv-active-left', 'iv-active-right');
        });
    });
});