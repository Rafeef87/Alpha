document.addEventListener('DOMContentLoaded', () => {
    const previewSize = 150;

    // open modal
    document.querySelectorAll('[data-modal="true"]').forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target');
            const modal = document.querySelector(modalTarget);
            if (modal) modal.style.display = 'flex';
        });
    });

    // close modal & clear form
    document.querySelectorAll('[data-close="true"]').forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal');
            if (modal) {
                modal.style.display = 'none';
                modal.setAttribute('aria-hidden', 'true');

                modal.querySelectorAll('form').forEach(form => {
                    form.reset();
                    const imagePreview = form.querySelector('.image-preview');
                    if (imagePreview) imagePreview.src = '';

                    const imagePreviewer = form.querySelector('.image-previewer');
                    if (imagePreviewer) imagePreviewer.classList.remove('selected');
                });
            }
        });
        //// 💾 Submit listener for all forms in modals
        //document.querySelectorAll('.modal form').forEach(form => {
        //    form.addEventListener('submit', async (e) => {
        //        e.preventDefault();

        //        const formData = new FormData(form);
        //        const payload = Object.fromEntries(formData.entries());

        //        const actionUrl = form.getAttribute('data-action'); 
        //        if (!actionUrl) {
        //            alert("Missing form action URL.");
        //            return;
        //        }

        //        try {
        //            const response = await fetch(actionUrl, {
        //                method: 'POST',
        //                headers: { 'Content-Type': 'application/json' },
        //                body: JSON.stringify(payload)
        //            });

        //            if (response.ok) {
        //                alert('Saved successfully!');
        //                form.reset();
        //                const modal = form.closest('.modal');
        //                if (modal) modal.style.display = 'none';
        //                location.reload();
        //            } else {
        //                const error = await response.text();
        //                alert('Error saving: ' + error);
        //            }
        //        } catch (error) {
        //            console.error(error);
        //            alert('Something went wrong.');
        //        }
        //    });
        //});
        // Delete button in dropdown or list
        document.querySelectorAll('.dropdown-action.remove').forEach(button => {
            button.addEventListener('click', async e => {
                e.preventDefault();

                const id = button.getAttribute('data-id');
                const type = button.getAttribute('data-type'); // ex: "project" eller "contact"
                if (!id || !type) {
                    alert('Missing data-id or data-type on delete button.');
                    return;
                }

                const confirmed = confirm(`Are you sure you want to delete this ${type}?`);
                if (!confirmed) return;

                try {
                    const response = await fetch(`/${type}s/delete/${id}`, {
                        method: 'DELETE'
                    });

                    if (response.ok) {
                        alert(`${type} deleted successfully.`);
                        location.reload();
                    } else {
                        alert(`Failed to delete ${type}.`);
                    }
                } catch (error) {
                    console.error(error);
                    alert('Error deleting.');
                }
            });
        });

    });

    // handle image-previewer
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type="file"]');
        const imagePreview = previewer.querySelector('.image-preview');

        if (!fileInput || !imagePreview) return; // Protect against null values

        previewer.addEventListener('click', () => fileInput.click());

        fileInput.addEventListener('change', ({ target: { files} }) => {
            const file = files[0];
            if (file) processImage(file, imagePreview, previewer, previewSize);
        });
    });

});

// Load image helper function
async function loadImage(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();

        reader.onerror = () => reject(new Error("Failed to load file."));
        reader.onload = (e) => {
            const img = new Image();
            img.onerror = () => reject(new Error("Failed to load image."));
            img.onload = () => resolve(img);
            img.src = e.target.result;
        };
        reader.readAsDataURL(file);
    });
}

// Process image
async function processImage(file, imagePreview, previewer, previewSize = 150) {
    try {
        const img = await loadImage(file);
        const canvas = document.createElement('canvas');
        canvas.width = previewSize;
        canvas.height = previewSize;

        const ctx = canvas.getContext('2d');
        ctx.drawImage(img, 0, 0, previewSize, previewSize);
        imagePreview.src = canvas.toDataURL('image/jpeg');
        previewer.classList.add('selected');
    }
    catch (error) {
        console.error('Failed on image processing', error);
    }
}
// Toggle dropdown menu
document.querySelector('[data-type="dropdown"]').addEventListener("click", function (e) {
    e.stopPropagation(); // prevent event bubbling
    const targetId = this.getAttribute("data-target");
    const menu = document.getElementById(targetId);

    // Toggle visibility
    menu.style.display = (menu.style.display === "block") ? "none" : "block";
});

// Close dropdown if clicking outside
document.addEventListener("click", function (event) {
    const menu = document.getElementById("dropdown");
    const button = document.querySelector('[data-type="dropdown"]');

    if (menu && button && !menu.contains(event.target) && !button.contains(event.target)) {
        menu.style.display = "none";
    }
});

// Handle dropdown actions delete  - ChatGPT 
document.querySelectorAll('.dropdown-action.remove').forEach(button => {
    button.addEventListener('click', async function (e) {
        e.preventDefault();

        const memberId = this.getAttribute('data-id');
        const confirmed = confirm("Are you sure you want to delete this member?");
        if (!confirmed) return;

        try {
            const response = await fetch(`${memberId}`, {
                method: 'DELETE'
            });

            if (response.ok) {
                alert('Member deleted successfully.');
                
                location.reload();
            } else {
                alert('Failed to delete member.');
            }
        } catch (error) {
            console.error('Error deleting member:', error);
            alert('Something went wrong.');
        }
    });
});
// Notification and account Dropdown - ChatGPT 

document.addEventListener('DOMContentLoaded', function () {
    const dropdownButtons = document.querySelectorAll('[data-target]');

    dropdownButtons.forEach(button => {
        const targetSelector = button.getAttribute('data-target');
        const dropdown = document.querySelector(targetSelector);

        if (!dropdown) return;

        button.addEventListener('click', function (event) {
            event.stopPropagation();

            // Close all other dropdowns
            document.querySelectorAll('.dropdown').forEach(d => {
                if (d !== dropdown) d.classList.remove('show');
            });

            // Toggle current dropdown
            dropdown.classList.toggle('show');
        });
    });

    // Close on outside click
    document.addEventListener('click', function () {
        document.querySelectorAll('.dropdown').forEach(d => d.classList.remove('show'));
    });

    // Optional: Escape key closes dropdown
    document.addEventListener('keydown', function (e) {
        if (e.key === "Escape") {
            document.querySelectorAll('.dropdown').forEach(d => d.classList.remove('show'));
        }
    });
});