// Wishlist functionality
document.addEventListener('DOMContentLoaded', function() {
    const wishlistIcon = document.querySelector('.wishlist-icon');
    
    if (wishlistIcon) {
        wishlistIcon.addEventListener('click', function() {
            this.classList.toggle('active');
            this.classList.toggle('far');
            this.classList.toggle('fas');
        });
    }

    // Add to cart functionality
    const addToCartBtn = document.querySelector('.add-to-cart-btn');
    
    if (addToCartBtn) {
        addToCartBtn.addEventListener('click', function() {
            // Add animation feedback
            this.textContent = 'Added!';
            this.style.backgroundColor = '#4a8b4a';
            
            setTimeout(() => {
                this.textContent = 'Add to cart';
                this.style.backgroundColor = '#8b4a4a';
            }, 1500);
        });
    }
});

