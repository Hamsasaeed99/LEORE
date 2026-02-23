const formatCurrency = (value) => `$${value.toFixed(2)}`;

const updateTotals = () => {
    const rows = Array.from(document.querySelectorAll('.product-row'));
    let subtotal = 0;

    rows.forEach((row) => {
        const price = parseFloat(row.dataset.price || '0');
        const qty = parseInt(row.querySelector('.qty-value').textContent, 10) || 0;
        subtotal += price * qty;
    });

    const subtotalEl = document.querySelector('.subtotal-value');
    const totalEl = document.querySelector('.total-value');
    if (subtotalEl) subtotalEl.textContent = formatCurrency(subtotal);
    if (totalEl) totalEl.textContent = formatCurrency(subtotal); // shipping calculated at checkout
};

document.querySelectorAll('.product-row').forEach((row) => {
    row.addEventListener('click', (e) => {
        const target = e.target;
        const qtyValue = row.querySelector('.qty-value');
        let qty = parseInt(qtyValue.textContent, 10) || 1;

        if (target.classList.contains('qty-btn')) {
            if (target.textContent === '+' && qty < 99) {
                qty += 1;
            } else if (target.textContent === '-' && qty > 1) {
                qty -= 1;
            }
            qtyValue.textContent = qty;
            updateTotals();
        }

        if (target.classList.contains('remove-btn')) {
            row.remove();
            updateTotals();
        }
    });
});

const applyBtn = document.querySelector('.apply-btn');
if (applyBtn) {
    applyBtn.addEventListener('click', () => {
        alert('Promo will be calculated at checkout.');
    });
}

const checkoutBtn = document.querySelector('.primary-btn');
if (checkoutBtn) {
    checkoutBtn.addEventListener('click', () => {
        alert('Checkout flow coming soon!');
    });
}

const continueBtn = document.querySelector('.secondary-btn');
if (continueBtn) {
    continueBtn.addEventListener('click', () => {
        window.history.back();
    });
}

updateTotals();

function showCenterMessage(message) {
    document.getElementById('centerMessageText').textContent = message;
    document.getElementById('centerMessageOverlay').classList.add('show');
}

function closeCenterMessage() {
    document.getElementById('centerMessageOverlay').classList.remove('show');
}

document.querySelectorAll('.product-row').forEach((row) => {
    row.addEventListener('click', (e) => {
        const target = e.target;
        const qtyValue = row.querySelector('.qty-value');
        let qty = parseInt(qtyValue.textContent, 10) || 1;

        if (target.classList.contains('qty-btn')) {
           
            qtyValue.textContent = qty;
            updateTotals();
        }

        if (target.classList.contains('remove-btn')) {
            row.remove();
            updateTotals();
        }
    });
});

document.addEventListener('click', function (e) {

    if (!e.target.classList.contains('plus') &&
        !e.target.classList.contains('minus')) return;

    const container = e.target.closest('.quantity-selector');
    if (!container) return;

    const qtySpan = container.querySelector('.qty-value');
    let qty = parseInt(qtySpan.textContent) || 1;

    if (e.target.classList.contains('plus')) qty++;
    if (e.target.classList.contains('minus') && qty > 1) qty--;

    qtySpan.textContent = qty;
});

document.addEventListener('DOMContentLoaded', function () {

    document.querySelectorAll('.add-to-cart-btn').forEach(btn => {

        btn.addEventListener('click', function () {

            const productId = this.dataset.productId;
            const button = this;
            const originalText = button.textContent;

            let quantity = 1;
            const container = button.closest('.product-card, .product-row, .product-actions');
            if (container) {
                const qtyElement = container.querySelector('.qty-value');
                if (qtyElement) {
                    quantity = parseInt(qtyElement.textContent) || 1;
                }
            }

            fetch('/Cart/AddToCart', {
                method: 'POST',
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                body: `productId=${productId}&quantity=${quantity}`
            })
            .then(res => res.json())
            .then(data => {
                if (data.success) {
                    button.textContent = 'Added ✔';
                    button.disabled = true;
                    button.style.backgroundColor = '#4a8b4a';

                    setTimeout(() => {
                        button.textContent = originalText;
                        button.disabled = false;
                        button.style.backgroundColor = '';
                    }, 1200);
                } else {
                    showCenterMessage(data.message);
                }
            })
            .catch(() => {
                showCenterMessage('Something went wrong. Please try again.');
            });

        });
    });
});
