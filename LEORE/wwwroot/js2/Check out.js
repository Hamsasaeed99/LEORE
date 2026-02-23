// Handle checkbox interactions
document.addEventListener('DOMContentLoaded', function() {
    const checkboxes = document.querySelectorAll('.checkbox-input');
    
    checkboxes.forEach(checkbox => {
        checkbox.addEventListener('change', function() {
            const customCheckbox = this.nextElementSibling;
            if (this.checked) {
                customCheckbox.classList.add('checked');
            } else {
                customCheckbox.classList.remove('checked');
            }
        });
    });

    // Initialize checked state for "Cash On Delivery"
    const cashOnDelivery = document.querySelector('.payment-method .checkbox-input');
    if (cashOnDelivery && cashOnDelivery.checked) {
        const customCheckbox = cashOnDelivery.nextElementSibling;
        customCheckbox.classList.add('checked');
    }

    // Handle form submission
    const completeOrderBtn = document.querySelector('.complete-order-btn');
    const shippingForm = document.querySelector('.shipping-form');
    
    if (completeOrderBtn && shippingForm) {
        completeOrderBtn.addEventListener('click', function(e) {
            e.preventDefault();
            
            // Validate form
            const inputs = shippingForm.querySelectorAll('.form-input[required]');
            let isValid = true;
            
            inputs.forEach(input => {
                if (!input.value.trim()) {
                    isValid = false;
                    input.style.borderColor = '#ff6b6b';
                } else {
                    input.style.borderColor = '#e0e0e0';
                }
            });
            
            if (isValid) {
                // Change button to green and update text
                completeOrderBtn.classList.add('completed');
                completeOrderBtn.textContent = 'Order Completed!';
                
                // Disable the button to prevent multiple clicks
                completeOrderBtn.disabled = true;
                
                // Here you would typically send the data to a server
            } else {
                alert('Please fill in all required fields.');
            }
        });
    }
});

