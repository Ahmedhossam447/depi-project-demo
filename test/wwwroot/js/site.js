// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deleteconfirm(UserId , IsdDeletecClicked) {
    var spanid = "deleteConfirmSpan" + UserId;
    var deletespan = "deletespan" + UserId;
    if (IsdDeletecClicked) {
        document.getElementById(spanid).style.display = "block";
        document.getElementById(deletespan).style.display = "none";
    }
    else {
        document.getElementById(spanid).style.display = "none";
        document.getElementById(deletespan).style.display = "block";
    }
}
document.addEventListener("DOMContentLoaded", function () {
    // Use event delegation to handle all forms (works for dynamically added forms too)
    document.addEventListener("submit", function (event) {
        const form = event.target;
        
        // Check if this is a form that should be handled (adjust selector as needed)
        // You can use: form.id === "add-product" or form.classList.contains("add-product-form")
        // For now, checking if form has the id or if it's any form (you can customize this)
        if (form.tagName === "FORM" && (form.id === "add-product" || form.action.includes("Order/Create") || form.action.includes("Order/create"))) {
            event.preventDefault();
            const formData = new FormData(form);
            
            // The anti-forgery token is already in the FormData from @Html.AntiForgeryToken()
            // No need to add it as a header
            
            fetch("/Order/Create", {
                method: "POST",
                body: formData
            })
            .then(response => response.json())
                .then(data => {
                if (data.message === "success" || data.Message === "success") {
                    const cartCount = data.cartCount || data.CartCount;
                    const cartCountEl = document.getElementById("cart-count");
                    if (cartCountEl && cartCount !== undefined) {
                        cartCountEl.innerText = cartCount;
                        cartCountEl.setAttribute("data-count", cartCount);
                    }
                    showToast("Product added to cart successfully!", "success");
                } else {
                    showToast("Failed to add product to cart.", "error");
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showToast("An error occurred while adding the product.", "error");
            });     
        }
    });
})

// Cart Modal Functionality
document.addEventListener("DOMContentLoaded", function() {

    // Get all the modal elements
    const openCartButton = document.getElementById("open-cart-button");
    const closeCartButton = document.getElementById("cart-close-button");
    const cartOverlay = document.getElementById("cart-overlay");
    const cartModal = document.getElementById("cart-modal-container");
    const modalBody = document.getElementById("cart-modal-body");

    // --- Function to Open the Modal ---
    function openCartModal() {
        // Set a "loading" state
        if (modalBody) {
            modalBody.innerHTML = "<p>Loading cart contents...</p>";
        }

        // Show the overlay and modal
        if (cartOverlay) {
            cartOverlay.style.display = "block";
            // Trigger animation after display
            setTimeout(() => {
                cartOverlay.classList.add("show");
            }, 10);
        }

        if (cartModal) {
            cartModal.style.display = "flex";
            // Force reflow to ensure display change is applied
            cartModal.offsetHeight;
            // Trigger animation after display
            setTimeout(() => {
                cartModal.classList.add("show");
            }, 10);
        }

        // Prevent body scroll when modal is open
        document.body.style.overflow = "hidden";

        // Fetch the cart HTML from our controller
        fetch("/Order/orderdetails", {
            method: "GET",
            headers: {
                // We don't need anti-forgery for a GET request
                "X-Requested-With": "XMLHttpRequest" // Identifies it as AJAX
            }
        }).then(response => response.text())
            .then(html => {
                modalBody.innerHTML = html;
            }).catch(error => {
                console.error("Error loading cart:", error);
                modalBody.innerHTML = "<p class='text-danger'>Sorry, we couldn't load your cart.</p>";
            });


        // TODO: Handle the backend logic here
    }

    // --- Function to Close the Modal ---
    function closeCartModal() {
        // Remove show class for animation
        if (cartOverlay) {
            cartOverlay.classList.remove("show");
        }
        if (cartModal) {
            cartModal.classList.remove("show");
        }

        // Wait for animation to complete before hiding
        setTimeout(() => {
            if (cartOverlay) {
                cartOverlay.style.display = "none";
            }
            if (cartModal) {
                cartModal.style.display = "none";
            }
            // Restore body scroll
            document.body.style.overflow = "";
        }, 300); // Match the CSS transition duration
    }

    // --- Attach Event Listeners ---
    
    // 1. Open the modal when the cart button is clicked
    if (openCartButton) {
        openCartButton.addEventListener("click", function(event) {
            event.preventDefault(); // Stop the link from navigating
            openCartModal();
        });
    }

    // 2. Close the modal when the 'X' button is clicked
    if (closeCartButton) {
        closeCartButton.addEventListener("click", closeCartModal);
    }

    // 3. Close the modal when the user clicks on the dark overlay
    if (cartOverlay) {
        cartOverlay.addEventListener("click", closeCartModal);
    }

    // 4. Prevent modal from closing when clicking inside it
    if (cartModal) {
        cartModal.addEventListener("click", function(event) {
            event.stopPropagation();
        });
    }
});

// Toast Notification Function
function showToast(message, type = "info") {
    // Remove existing toast if any
    const existingToast = document.querySelector('.toast-notification');
    if (existingToast) {
        existingToast.remove();
    }

    // Create toast element
    const toast = document.createElement('div');
    toast.className = `toast-notification ${type}`;
    toast.innerHTML = `
        <div class="toast-content">
            <p class="toast-message">${message}</p>
            <button class="toast-close" aria-label="Close">&times;</button>
        </div>
    `;

    // Add to page
    document.body.appendChild(toast);

    // Show toast
    setTimeout(() => {
        toast.classList.add('show');
    }, 10);

    // Close button handler
    const closeBtn = toast.querySelector('.toast-close');
    closeBtn.addEventListener('click', () => {
        hideToast(toast);
    });

    // Click anywhere to close
    const clickHandler = (e) => {
        if (!toast.contains(e.target)) {
            hideToast(toast);
            document.removeEventListener('click', clickHandler);
        }
    };
    
    // Add click listener after a small delay to prevent immediate closing
    setTimeout(() => {
        document.addEventListener('click', clickHandler);
    }, 100);

    // Auto-hide after 5 seconds
    setTimeout(() => {
        hideToast(toast);
        document.removeEventListener('click', clickHandler);
    }, 5000);
}

function hideToast(toast) {
    toast.classList.add('hiding');
    setTimeout(() => {
        if (toast.parentNode) {
            toast.parentNode.removeChild(toast);
        }
    }, 300);
}

// Payment Form Handler
document.addEventListener("DOMContentLoaded", function() {
    const paymentForm = document.getElementById("payment-form");
    if (paymentForm) {
        paymentForm.addEventListener("submit", function(event) {
            event.preventDefault();
            
            const formData = new FormData(paymentForm);
            
            fetch("/Transaction/ProccessPayment", {
                method: "POST",
                body: formData
            })
            .then(response => response.json())
            .then(data => {
                if (data.status === "succeeded" || data.status === "success") {
                    // Reset cart count to 0
                    const cartCountEl = document.getElementById("cart-count");
                    if (cartCountEl) {
                        cartCountEl.innerText = "0";
                        cartCountEl.setAttribute("data-count", "0");
                    }
                    showToast(data.message || "Payment processed successfully!", "success");
                    // Redirect to UserView after a short delay
                    setTimeout(() => {
                        window.location.href = "/Shelter/UserView";
                    }, 1500);
                } else {
                    showToast(data.message || "Payment processing failed.", "error");
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showToast("An error occurred while processing payment.", "error");
            });
        });
    }
});