// Tab switching functionality
document.addEventListener('DOMContentLoaded', function() {
    const tabButtons = document.querySelectorAll('.tab-button');
    const orderCards = document.querySelectorAll('.order-card');

    tabButtons.forEach(button => {
        button.addEventListener('click', function() {
            const targetTab = this.getAttribute('data-tab');
            
            // Remove active class from all tabs
            tabButtons.forEach(btn => btn.classList.remove('active'));
            
            // Add active class to clicked tab
            this.classList.add('active');
            
            // Filter orders based on tab
            // In a real application, this would filter based on order status
            // For now, we'll just show/hide based on the tab
            if (targetTab === 'current') {
                // Show only orders with "Shipped" status (current orders)
                orderCards.forEach(card => {
                    const statusBadge = card.querySelector('.status-badge');
                    if (statusBadge && statusBadge.classList.contains('shipped')) {
                        card.style.display = 'block';
                    } else {
                        card.style.display = 'none';
                    }
                });
            } else if (targetTab === 'completed') {
                // Show only orders with "Completed" status
                orderCards.forEach(card => {
                    const statusBadge = card.querySelector('.status-badge');
                    if (statusBadge && statusBadge.classList.contains('completed')) {
                        card.style.display = 'block';
                    } else {
                        card.style.display = 'none';
                    }
                });
            }
        });
    });

    // Initialize: Show completed orders by default (since that tab is active)
    const activeTab = document.querySelector('.tab-button.active');
    if (activeTab) {
        activeTab.click();
    }
});

