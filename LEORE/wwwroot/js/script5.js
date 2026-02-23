document.addEventListener('DOMContentLoaded', function () {

    // Wishlist
    document.querySelectorAll('.wishlist-icon').forEach(icon => {
        icon.addEventListener('click', function () {
            this.classList.toggle('active');
            this.classList.toggle('far');
            this.classList.toggle('fas');
        });
    });

    

});


