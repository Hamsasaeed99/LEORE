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


