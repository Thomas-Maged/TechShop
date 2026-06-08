// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// ============================================
// COMMON JAVASCRIPT FOR ALL PAGES
// ============================================

// Navbar scroll effect
window.addEventListener('scroll', function () {
    const navbar = document.getElementById('mainNavbar');
    if (navbar && window.scrollY > 50) {
        navbar.classList.add('scrolled');
    } else if (navbar) {
        navbar.classList.remove('scrolled');
    }
});

// Active nav link highlighting
document.addEventListener('DOMContentLoaded', function () {
    const currentPath = window.location.pathname.toLowerCase();
    const navLinks = document.querySelectorAll('.nav-link');

    navLinks.forEach(link => {
        const linkPath = link.getAttribute('href');
        if (linkPath && (currentPath === linkPath.toLowerCase() ||
            (currentPath.includes(linkPath.toLowerCase()) && linkPath !== '/'))) {
            link.classList.add('active');
        }
    });
});

// Animate elements on scroll (Observer)
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -50px 0px'
};

const observer = new IntersectionObserver(function (entries) {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add('animate-in');
        }
    });
}, observerOptions);

// Observe all cards on page load
document.addEventListener('DOMContentLoaded', function () {
    const cards = document.querySelectorAll('.card');
    cards.forEach(card => observer.observe(card));
});

// Update cart badge function (can be called from any page)
function updateCartBadge(count) {
    const badge = document.getElementById('cartBadge');
    if (badge) {
        badge.textContent = count;
        if (count > 0) {
            badge.style.display = 'flex';
        } else {
            badge.style.display = 'none';
        }
    }
}

// Show toast notification (utility function)
function showToast(message, type = 'success') {
    // Simple toast notification - you can enhance this
    const toast = document.createElement('div');
    toast.className = `toast-notification toast-${type}`;
    toast.textContent = message;
    toast.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        background: ${type === 'success' ? 'var(--accent-primary)' : '#ef4444'};
        color: white;
        padding: 1rem 1.5rem;
        border-radius: var(--radius-md);
        box-shadow: var(--shadow-lg);
        z-index: 9999;
        animation: slideIn 0.3s ease;
    `;

    document.body.appendChild(toast);

    setTimeout(() => {
        toast.style.animation = 'slideOut 0.3s ease';
        setTimeout(() => toast.remove(), 300);
    }, 3000);
}

// Add animation keyframes for toast
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from {
            transform: translateX(400px);
            opacity: 0;
        }
        to {
            transform: translateX(0);
            opacity: 1;
        }
    }
    @keyframes slideOut {
        from {
            transform: translateX(0);
            opacity: 1;
        }
        to {
            transform: translateX(400px);
            opacity: 0;
        }
    }
`;
document.head.appendChild(style);

// ============================================
// PRODUCT DETAILS FUNCTIONS
// ============================================

// Update quantity
/**
 * Updates quantity and toggles stock warning visibility
 * @param {number} change - The amount to add or subtract (1 or -1)
 * @param {number} maxStock - The limit for this specific product
 */
function updateQuantity(change, maxStock) {
    const input = document.getElementById('quantityInput');
    const stockStatus = document.getElementById('stockStatusNumberOfProducts');
    const decreaseBtn = document.querySelector('.quantity-decrease');
    const increaseBtn = document.querySelector('.quantity-increase');

    if (!input) return;

    let currentValue = parseInt(input.value) || 1;
    let newValue = currentValue + change;

    // 1. Hard Constraints
    if (newValue < 1) newValue = 1;

    // Optional: Uncomment below if you want to physically block going over stock
    // if (newValue > maxStock) newValue = maxStock;

    input.value = newValue;

    // 2. Toggle Stock Warning Visibility
    if (stockStatusNumberOfProducts) {
        // Show warning if user tries to select the max or more
        if (newValue >= maxStock) {
            stockStatus.style.display = 'flex'; // Use flex to align with Bootstrap icons
        } else {
            stockStatus.style.display = 'none';
        }
    }

    // 3. Update Button States
    if (decreaseBtn) {
        decreaseBtn.disabled = (newValue <= 1);
    }

    // Optional: Disable plus button if max reached
    if (increaseBtn) {
        increaseBtn.disabled = (newValue >= maxStock);
    }
}

// ============================================
// CART FUNCTIONS
// ============================================

// Update cart item quantity
function updateCartQuantity(itemId, change) {
    const input = document.getElementById(`qty-${itemId}`);
    if (!input) return;

    let currentValue = parseInt(input.value) || 1;
    let newValue = currentValue + change;

    if (newValue < 1) newValue = 1;

    input.value = newValue;

    // Update decrease button state
    const decreaseBtn = document.getElementById(`decrease-${itemId}`);
    if (decreaseBtn) {
        decreaseBtn.disabled = newValue <= 1;
    }

    // Recalculate totals
    recalculateCartTotals();

    // TODO: Add backend API call to update cart
    // Example:
    // fetch(`/api/cart/update/${itemId}`, {
    //     method: 'PUT',
    //     headers: { 'Content-Type': 'application/json' },
    //     body: JSON.stringify({ quantity: newValue })
    // });
}

// Remove item from cart
function removeCartItem(itemId) {
    const cartItem = document.getElementById(`cart-item-${itemId}`);
    if (!cartItem) return;

    // Animate removal
    cartItem.style.opacity = '0';
    cartItem.style.transform = 'translateX(-100%)';

    setTimeout(() => {
        cartItem.remove();
        recalculateCartTotals();
        updateCartBadge(getCartItemCount());

        // Check if cart is empty
        const remainingItems = document.querySelectorAll('.cart-item');
        if (remainingItems.length === 0) {
            showEmptyCart();
        }

        showToast('Item removed from cart');
    }, 300);

    // TODO: Add backend API call to remove item
    // Example:
    // fetch(`/api/cart/remove/${itemId}`, {
    //     method: 'DELETE'
    // });
}

// Recalculate cart totals
function recalculateCartTotals() {
    let subtotal = 0;
    let totalSavings = 0;

    document.querySelectorAll('.cart-item').forEach(item => {
        const quantity = parseInt(item.querySelector('.cart-qty-input').value) || 1;
        const price = parseFloat(item.dataset.price) || 0;
        const oldPrice = parseFloat(item.dataset.oldPrice) || 0;

        const itemTotal = price * quantity;
        subtotal += itemTotal;

        if (oldPrice > 0) {
            totalSavings += (oldPrice - price) * quantity;
        }

        // Update item subtotal display
        const subtotalElement = item.querySelector('.cart-item-subtotal-value');
        if (subtotalElement) {
            subtotalElement.textContent = `$${itemTotal.toFixed(2)}`;
        }
    });

    // Update summary
    const subtotalElement = document.getElementById('cart-subtotal');
    const savingsElement = document.getElementById('cart-savings');
    const totalElement = document.getElementById('cart-total');

    if (subtotalElement) subtotalElement.textContent = `$${subtotal.toFixed(2)}`;
    if (savingsElement) savingsElement.textContent = `-$${totalSavings.toFixed(2)}`;
    if (totalElement) totalElement.textContent = `$${subtotal.toFixed(2)}`;

    // Update savings badge
    const savingsBadge = document.getElementById('total-savings-amount');
    if (savingsBadge && totalSavings > 0) {
        savingsBadge.textContent = `$${totalSavings.toFixed(2)}`;
        document.querySelector('.cart-savings-badge').style.display = 'block';
    } else if (savingsBadge) {
        document.querySelector('.cart-savings-badge').style.display = 'none';
    }
}

// Get cart item count
function getCartItemCount() {
    let count = 0;
    document.querySelectorAll('.cart-qty-input').forEach(input => {
        count += parseInt(input.value) || 0;
    });
    return count;
}

// Show empty cart state
function showEmptyCart() {
    const container = document.querySelector('.cart-items-section');
    if (container) {
        container.innerHTML = `
            <div class="cart-empty">
                <div class="cart-empty-icon">
                    <i class="bi bi-cart-x"></i>
                </div>
                <h3 class="cart-empty-text">Your cart is empty</h3>
                <a href="/products" class="btn-primary btn">
                    <i class="bi bi-shop me-2"></i>Continue Shopping
                </a>
            </div>
        `;
    }

    // Hide summary
    const summary = document.querySelector('.cart-summary');
    if (summary) {
        summary.style.display = 'none';
    }
}

// ============================================
// QUANTITY MODAL FUNCTIONS
// ============================================

let currentEditItemId = null;
let modalQuantityValue = 1;

// Open quantity modal
function openQuantityModal(itemId) {
    currentEditItemId = itemId;

    // Get item details
    const itemName = document.getElementById(`item-name-${itemId}`).textContent.trim();
    const itemPrice = document.getElementById(`item-price-${itemId}`).textContent;
    const itemImage = document.getElementById(`item-image-${itemId}`).src;
    const currentQty = parseInt(document.getElementById(`qty-${itemId}`).value) || 1;
    const productID = document.getElementById(`input-productID-cart-${itemId}`).value;
    console.log(productID);


    // Populate modal
    document.getElementById('modal-product-name').textContent = itemName;
    document.getElementById('modal-product-price').textContent = itemPrice;
    document.getElementById('modal-product-image').src = itemImage;
    document.getElementById('modal-quantity').textContent = currentQty;
    document.getElementById('modal-productID').value = productID;
    modalQuantityValue = currentQty;

    // Update decrease button state
    document.getElementById('modal-decrease').disabled = currentQty <= 1;

    // Show modal
    document.getElementById('quantityModal').classList.add('active');
    document.body.style.overflow = 'hidden';
}

// Close quantity modal
function closeQuantityModal() {
    document.getElementById('quantityModal').classList.remove('active');
    document.body.style.overflow = '';
    currentEditItemId = null;
    modalQuantityValue = 1;
}

// Update quantity in modal
function updateModalQuantity(change) {
    modalQuantityValue += change;

    if (modalQuantityValue < 1) modalQuantityValue = 1;

    document.getElementById('modal-quantity').textContent = modalQuantityValue;
    document.getElementById('input-quantity').value = modalQuantityValue;
    document.getElementById('modal-decrease').disabled = modalQuantityValue <= 1;
}

// Save quantity and close modal
function saveQuantity() {
    if (currentEditItemId === null) return;

    // Update hidden input
    const qtyInput = document.getElementById(`qty-${currentEditItemId}`);
    qtyInput.value = modalQuantityValue;

    // Update display
    const qtyDisplay = document.getElementById(`qty-display-${currentEditItemId}`);
    qtyDisplay.textContent = modalQuantityValue;

    // Recalculate totals
    recalculateCartTotals();
    updateCartBadge(getCartItemCount());

    // Show success message
    showToast('Quantity updated successfully!');

    // Close modal
    closeQuantityModal();

    // TODO: Add backend API call to update cart
    // Example:
    // fetch(`/api/cart/update/${currentEditItemId}`, {
    //     method: 'PUT',
    //     headers: { 'Content-Type': 'application/json' },
    //     body: JSON.stringify({ quantity: modalQuantityValue })
    // });
}

// Close modal when clicking outside
document.addEventListener('click', function (e) {
    const modal = document.getElementById('quantityModal');
    if (modal && e.target === modal) {
        closeQuantityModal();
    }
});

// Close modal with Escape key
document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') {
        const modal = document.getElementById('quantityModal');
        if (modal && modal.classList.contains('active')) {
            closeQuantityModal();
        }
    }
});

// ============================================
// CHECKOUT FUNCTIONS
// ============================================

// Form validation
function validateCheckoutForm() {
    let isValid = true;

    // Get form fields
    const fullName = document.getElementById('fullName');
    const email = document.getElementById('email');
    const phone = document.getElementById('phone');
    const address = document.getElementById('address');

    // Clear previous errors
    document.querySelectorAll('.form-control-checkout').forEach(field => {
        field.classList.remove('is-invalid');
    });

    // Validate Full Name
    if (!fullName.value.trim()) {
        fullName.classList.add('is-invalid');
        isValid = false;
    }

    // Validate Email
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!email.value.trim() || !emailPattern.test(email.value)) {
        email.classList.add('is-invalid');
        isValid = false;
    }

    // Validate Phone
    const phonePattern = /^[\d\s\-\+\(\)]+$/;
    if (!phone.value.trim() || !phonePattern.test(phone.value)) {
        phone.classList.add('is-invalid');
        isValid = false;
    }

    // Validate Address
    if (!address.value.trim()) {
        address.classList.add('is-invalid');
        isValid = false;
    }

    return isValid;
}

// Calculate order totals
function calculateOrderTotals() {
    let subtotal = 0;
    let discount = 0;

    // This would normally come from your cart data
    // For now, it's calculated from the displayed items
    document.querySelectorAll('.order-item').forEach(item => {
        const price = parseFloat(item.dataset.price) || 0;
        const quantity = parseInt(item.dataset.quantity) || 1;
        const oldPrice = parseFloat(item.dataset.oldPrice) || 0;

        subtotal += price * quantity;

        if (oldPrice > 0) {
            discount += (oldPrice - price) * quantity;
        }
    });

    const shipping = subtotal > 50 ? 0 : 10; // Free shipping over $50
    const total = subtotal - discount + shipping;

    // Update summary display
    const subtotalEl = document.getElementById('order-subtotal');
    const discountEl = document.getElementById('order-discount');
    const shippingEl = document.getElementById('order-shipping');
    const totalEl = document.getElementById('order-total');

    if (subtotalEl) subtotalEl.textContent = `$${subtotal.toFixed(2)}`;
    if (discountEl) discountEl.textContent = discount > 0 ? `-$${discount.toFixed(2)}` : '$0.00';
    if (shippingEl) shippingEl.textContent = shipping === 0 ? 'FREE' : `$${shipping.toFixed(2)}`;
    if (totalEl) totalEl.textContent = `$${total.toFixed(2)}`;

    return { subtotal, discount, shipping, total };
}