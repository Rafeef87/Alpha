const previewSize = 150;

document.addEventListener('DOMContentLoaded', () => {


    // Open modal
    document.querySelectorAll('[data-modal="true"]').forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target');
            const modal = document.querySelector(modalTarget);
            if (modal) {
                modal.style.display = 'flex';
                modal.setAttribute('aria-hidden', 'false');
            }
        });
    });

    // Close modal & clear form
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

                    // Reset any edit status
                    form.removeAttribute('data-edit-id');

                    // Reset the submit button to "Add" if it was changed
                    const submitBtn = form.querySelector('button[type="submit"]');
                    if (submitBtn && submitBtn.getAttribute('data-default-text')) {
                        submitBtn.textContent = submitBtn.getAttribute('data-default-text');
                    }
                });
            }
        });
    });

    // Handle form submissions
    const modalForms = document.querySelectorAll('form[data-modal-form="true"]');
    modalForms.forEach(form => {
        form.addEventListener('submit', async function (event) {
            event.preventDefault();

            const formData = new FormData(form);
            const actionUrl = form.getAttribute('action') || window.location.pathname;

            // Lägg till antiforgery token om det behövs
            const antiForgeryToken = document.querySelector('input[name="__RequestVerificationToken"]');
            if (antiForgeryToken) {
                formData.append('__RequestVerificationToken', antiForgeryToken.value);
            }

            try {
                const response = await fetch(actionUrl, {
                    method: 'POST',
                    body: formData,
                    // Om du inte använder FormData och istället skickar JSON:
                    // headers: {
                    //     'Content-Type': 'application/json',
                    //     'RequestVerificationToken': antiForgeryToken ? antiForgeryToken.value : ''
                    // },
                    // body: JSON.stringify(Object.fromEntries(formData))
                });

                if (!response.ok) {
                    throw new Error(`HTTP error! Status: ${response.status}`);
                }

                const result = await response.json();

                if (result.succeeded) {
                    alert('Saved successfully!');
                    window.location.reload();
                } else {
                    alert('Error: ' + (result.error || 'Unknown error'));
                }
            } catch (error) {
                console.error('Error submitting form:', error);
                alert('An unexpected error occurred: ' + error.message);
            }
        });
    });

    // Lägg till bildförhandsvisning om du har filuppladdning
    document.querySelectorAll('input[type="file"]').forEach(input => {
        input.addEventListener('change', function () {
            const preview = this.closest('.form-group').querySelector('.image-preview');
            const previewer = this.closest('.form-group').querySelector('.image-previewer');

            if (this.files && this.files[0] && preview) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    preview.src = e.target.result;
                    if (previewer) previewer.classList.add('selected');
                };
                reader.readAsDataURL(this.files[0]);
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

        fileInput.addEventListener('change', ({ target: { files } }) => {
            const file = files[0];
            if (file) processImage(file, imagePreview, previewer, previewSize);
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

    }
    );

