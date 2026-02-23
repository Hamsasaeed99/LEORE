// Quantity Selector Functionality
document.addEventListener('DOMContentLoaded', function() {
    const decreaseBtn = document.getElementById('decreaseQty');
    const increaseBtn = document.getElementById('increaseQty');
    const quantityDisplay = document.getElementById('quantity');
    const addToCartBtn = document.querySelector('.add-to-cart-btn');

    let quantity = 1;

    // Decrease quantity
    decreaseBtn.addEventListener('click', function() {
        if (quantity > 1) {
            quantity--;
            quantityDisplay.textContent = quantity;
        }
    });

    // Increase quantity
    increaseBtn.addEventListener('click', function() {
        quantity++;
        quantityDisplay.textContent = quantity;
    });

    // Add to cart functionality
    addToCartBtn.addEventListener('click', function() {
        // Prevent multiple clicks
        if (this.classList.contains('added')) {
            return;
        }

        // Add animation feedback
        this.style.transform = 'scale(0.95)';
        setTimeout(() => {
            this.style.transform = '';
        }, 150);

        // Change button to green and display "added!"
        this.classList.add('added');
        this.textContent = 'added!';
        
        // Here you would typically send the product data to a cart system
        console.log(`Added ${quantity} item(s) to cart`);

        // Return to original state after 1.5 seconds
        setTimeout(() => {
            this.classList.remove('added');
            this.textContent = 'Add To Cart';
        }, 1500);
    });

    // Search bar functionality
    const searchInput = document.querySelector('.search-input');
    searchInput.addEventListener('focus', function() {
        this.style.backgroundColor = '#FFE5D9';
    });

    searchInput.addEventListener('blur', function() {
        this.style.backgroundColor = '#FFFFFF';
    });

    // Add review button functionality (bottom bar)
    const addReviewButton = document.querySelector('.add-review-submit');
    if (addReviewButton) {
        addReviewButton.addEventListener('click', function () {
            if (this.classList.contains('added')) {
                return;
            }

            this.classList.add('added');
            this.textContent = 'Added!';

            // Return to original state after 1.5 seconds
            setTimeout(() => {
                this.classList.remove('added');
                this.textContent = 'Add review';
            }, 1500);
        });
    }
});

// Simple notification function
function showNotification(message) {
    // Create notification element
    const notification = document.createElement('div');
    notification.textContent = message;
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background-color: #8B4A4A;
        color: white;
        padding: 15px 25px;
        border-radius: 8px;
        box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        z-index: 1000;
        animation: slideIn 0.3s ease-out;
    `;

    // Add animation
    const style = document.createElement('style');
    style.textContent = `
        @keyframes slideIn {
            from {
                transform: translateX(100%);
                opacity: 0;
            }
            to {
                transform: translateX(0);
                opacity: 1;
            }
        }
    `;
    document.head.appendChild(style);

    document.body.appendChild(notification);

    // Remove notification after 3 seconds
    setTimeout(() => {
        notification.style.animation = 'slideIn 0.3s ease-out reverse';
        setTimeout(() => {
            notification.remove();
            style.remove();
        }, 300);
    }, 3000);
}

