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

    // Submit (Add/Edit) listener for all forms in modals
    document.querySelectorAll('.modal form').forEach(form => {
        form.addEventListener('submit', function (e) {
            e.preventDefault();

            // Validate the form
            if (!validateForm(this)) {
                showNotification('Validation Error', 'Please fill in all required fields correctly.', 'error');
                return;
            }

            // Collect form data
            const formData = new FormData(this);

            // Check if this is an edit or a new entry
            const isEdit = this.hasAttribute('data-edit-id');
            const itemId = isEdit ? this.getAttribute('data-edit-id') : null;

            // Add ID if it is an edit
            if (isEdit) {
                formData.append('id', itemId);
            }

            // Show charging indicator
            const submitBtn = this.querySelector('button[type="submit"]');
            const originalText = submitBtn.textContent;
            submitBtn.disabled = true;
            submitBtn.innerHTML = '<span class="spinner"></span> Processing...';

            //Determine URL based on whether it is an edit or new post
            const url = isEdit
                ? `/api/members/${itemId}` // URL för uppdatering
                : '/api/members';          // URL för att skapa ny

            // Skicka data med fetch API
            fetch(url, {
                method: isEdit ? 'PUT' : 'POST',
                body: formData,
            })
                .then(response => {
                    if (!response.ok) {
                        // Logga mer information om felet
                        console.error('Server responded with:', response.status, response.statusText);
                        return response.text().then(text => {
                            console.error('Response body:', text);
                            throw new Error(`Server error: ${response.status} ${response.statusText}`);
                        });
                    }
                    return response.json();
                })
                .then(data => {
                    // Visa bekräftelsemeddelande
                    showNotification('Success', isEdit ? 'Item updated successfully!' : 'Item added successfully!', 'success');
                    // Stäng modalen
                    const modal = this.closest('.modal');
                    if (modal) {
                        modal.style.display = 'none';
                        modal.setAttribute('aria-hidden', 'true');
                    }
                    // Om det är en redigering, uppdatera objektet i UI
                    // Annars, lägg till det nya objektet i UI
                    fetch('/api/members')
                  
                })
                .catch(error => {
                    console.error('Error:', error);
                    showNotification('Error', error.message, 'error');
                });


                    // Uppdatera UI utan att ladda om sidan
                    if (isEdit) {
                        updateItemInUI(itemId, data);
                    } else {
                        addItemToUI(data);
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    showNotification('Error', error.message, 'error');
                })
                .finally(() => {
                    // Återställ knappen
                    submitBtn.disabled = false;
                    submitBtn.textContent = originalText;
                });
        });
    });

    // Delete button in dropdown or list
    document.addEventListener('click', function (e) {
        if (e.target.closest('.dropdown-action.remove') || e.target.matches('.dropdown-action.remove')) {
            e.preventDefault();

            const deleteButton = e.target.closest('.dropdown-action.remove') || e.target;
            const itemId = deleteButton.getAttribute('data-id');
            const itemElement = document.querySelector(`[data-item-id="${itemId}"]`);

            if (!itemId) {
                console.error('No item ID found for delete action');
                return;
            }

            // Visa bekräftelsedialog
            if (confirm('Are you sure you want to delete this item? This action cannot be undone.')) {
                // Visa laddningsindikator på knappen om möjligt
                if (deleteButton.tagName === 'BUTTON') {
                    const originalText = deleteButton.textContent;
                    deleteButton.disabled = true;
                    deleteButton.innerHTML = '<span class="spinner"></span>';
                }

                // Skicka borttagningsbegäran
                fetch(`/api/items/${itemId}`, {
                    method: 'DELETE',
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Failed to delete item');
                        }
                        return response.json();
                    })
                    .then(data => {
                        // Ta bort elementet från UI med animation
                        if (itemElement) {
                            itemElement.style.opacity = '0';
                            setTimeout(() => {
                                itemElement.remove();

                                // Om listan är tom, visa eventuellt "empty state"
                                const itemsList = document.querySelector('.items-list');
                                if (itemsList && itemsList.children.length === 0) {
                                    showEmptyState();
                                }
                            }, 300);
                        }

                        showNotification('Success', 'Item deleted successfully!', 'success');
                    })
                    .catch(error => {
                        console.error('Error:', error);
                        showNotification('Error', error.message, 'error');

                        // Återställ knappen om det är en knapp
                        if (deleteButton.tagName === 'BUTTON') {
                            deleteButton.disabled = false;
                            deleteButton.textContent = originalText;
                        }
                    });
            }
        }
    });

    // Edit button click handler
    document.addEventListener('click', function (e) {
        if (e.target.closest('.dropdown-action.edit') || e.target.matches('.dropdown-action.edit')) {
            e.preventDefault();

            const editButton = e.target.closest('.dropdown-action.edit') || e.target;
            const itemId = editButton.getAttribute('data-id');
            const modalTarget = editButton.getAttribute('data-target') || '#editModal';
            const modal = document.querySelector(modalTarget);

            if (!itemId) {
                console.error('No item ID found for edit action');
                return;
            }

            // Visa laddningsindikator
            if (editButton.tagName === 'BUTTON') {
                const originalText = editButton.textContent;
                editButton.disabled = true;
                editButton.innerHTML = '<span class="spinner"></span> Loading...';
            }

            // Hämta data för objektet som ska redigeras
            fetch(`/api/items/${itemId}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Failed to fetch item data');
                    }
                    return response.json();
                })
                .then(data => {
                    if (modal) {
                        // Öppna modalen
                        modal.style.display = 'flex';
                        modal.setAttribute('aria-hidden', 'false');

                        // Hitta formuläret i modalen
                        const form = modal.querySelector('form');
                        if (form) {
                            // Markera att detta är en redigering genom att lägga till ID
                            form.setAttribute('data-edit-id', itemId);

                            // Fyll i formuläret med befintliga data
                            Object.keys(data).forEach(key => {
                                const input = form.querySelector(`[name="${key}"]`);
                                if (input) {
                                    // Hantera olika typer av inmatningsfält
                                    if (input.type === 'checkbox') {
                                        input.checked = !!data[key];
                                    } else if (input.type === 'file') {
                                        // För filuppladdningar kan vi inte sätta värdet direkt
                                        // Men vi kan visa en förhandsgranskning om det finns en bild
                                        const imagePreview = form.querySelector('.image-preview');
                                        if (imagePreview && data[key]) {
                                            imagePreview.src = data[key];

                                            const imagePreviewer = form.querySelector('.image-previewer');
                                            if (imagePreviewer) {
                                                imagePreviewer.classList.add('selected');
                                            }
                                        }
                                    } else {
                                        input.value = data[key] || '';
                                    }
                                }
                            });

                            // Ändra submit-knappens text till "Update" istället för "Add"
                            const submitBtn = form.querySelector('button[type="submit"]');
                            if (submitBtn) {
                                // Spara originaltext om det inte redan är gjort
                                if (!submitBtn.hasAttribute('data-default-text')) {
                                    submitBtn.setAttribute('data-default-text', submitBtn.textContent);
                                }
                                submitBtn.textContent = 'Update';
                            }
                        }
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    showNotification('Error', error.message, 'error');
                })
                .finally(() => {
                    // Återställ knappen
                    if (editButton.tagName === 'BUTTON') {
                        editButton.disabled = false;
                        editButton.textContent = 'Edit';
                    }
                });
        }
    });

    // Hjälpfunktioner

    // Validera formulär
    function validateForm(form) {
        let isValid = true;

        // Kontrollera alla obligatoriska fält
        form.querySelectorAll('[required]').forEach(field => {
            if (!field.value.trim()) {
                field.classList.add('error');
                isValid = false;
            } else {
                field.classList.remove('error');
            }
        });

        // Kontrollera e-postfält
        form.querySelectorAll('input[type="email"]').forEach(field => {
            if (field.value && !isValidEmail(field.value)) {
                field.classList.add('error');
                isValid = false;
            }
        });

        return isValid;
    }

    // Validera e-post
    function isValidEmail(email) {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }

    // Visa notifikation
    function showNotification(title, message, type = 'info') {
        const notification = document.createElement('div');
        notification.className = `notification ${type}`;
        notification.innerHTML = `
            <div class="notification-header">
                <strong>${title}</strong>
                <button class="close-notification">&times;</button>
            </div>
            <div class="notification-body">
                ${message}
            </div>
        `;

        document.body.appendChild(notification);

        // Visa notifikationen med animation
        setTimeout(() => {
            notification.classList.add('show');
        }, 10);

        // Stäng-knapp
        notification.querySelector('.close-notification').addEventListener('click', () => {
            closeNotification(notification);
        });

        // Automatisk stängning efter 5 sekunder
        setTimeout(() => {
            closeNotification(notification);
        }, 5000);
    }

    // Stäng notifikation
    function closeNotification(notification) {
        notification.classList.remove('show');
        setTimeout(() => {
            notification.remove();
        }, 300);
    }

    // Uppdatera ett objekt i UI
    function updateItemInUI(itemId, data) {
        const itemElement = document.querySelector(`[data-item-id="${itemId}"]`);
        if (itemElement) {
            // Uppdatera relevanta fält i UI
            // Detta beror på din specifika HTML-struktur
            const nameElement = itemElement.querySelector('.item-name');
            if (nameElement && data.name) {
                nameElement.textContent = data.name;
            }

            const descriptionElement = itemElement.querySelector('.item-description');
            if (descriptionElement && data.description) {
                descriptionElement.textContent = data.description;
            }

            // Uppdatera bild om det finns
            const imageElement = itemElement.querySelector('.item-image');
            if (imageElement && data.imageUrl) {
                imageElement.src = data.imageUrl;
            }

            // Markera elementet som uppdaterat med en animation
            itemElement.classList.add('updated');
            setTimeout(() => {
                itemElement.classList.remove('updated');
            }, 2000);
        }
    }

    // Lägg till ett nytt objekt i UI
    function addItemToUI(data) {
        const itemsList = document.querySelector('.items-list');
        if (!itemsList) return;

        // Ta bort "empty state" om det finns
        const emptyState = document.querySelector('.empty-state');
        if (emptyState) {
            emptyState.remove();
        }

        // Skapa nytt element baserat på din HTML-struktur
        // Detta är ett exempel och behöver anpassas till din specifika layout
        const newItem = document.createElement('div');
        newItem.className = 'item';
        newItem.setAttribute('data-item-id', data.id);

        newItem.innerHTML = `
            <div class="item-content">
                ${data.imageUrl ? `<img src="${data.imageUrl}" alt="${data.name}" class="item-image">` : ''}
                <div class="item-details">
                    <h3 class="item-name">${data.name}</h3>
                    <p class="item-description">${data.description || ''}</p>
                </div>
            </div>
            <div class="item-actions">
                <div class="dropdown">
                    <button class="dropdown-toggle">
                        <span class="sr-only">Actions</span>
                        <svg>...</svg>
                    </button>
                    <div class="dropdown-menu">
                        <button class="dropdown-action edit" data-id="${data.id}" data-target="#editModal">Edit</button>
                        <button class="dropdown-action remove" data-id="${data.id}">Delete</button>
                    </div>
                </div>
            </div>
        `;

        // Lägg till i listan
        itemsList.appendChild(newItem);

        // Markera som ny med animation
        newItem.style.opacity = '0';
        setTimeout(() => {
            newItem.style.opacity = '1';
            newItem.classList.add('new');
            setTimeout(() => {
                newItem.classList.remove('new');
            }, 2000);
        }, 10);
    }

    // Visa "empty state" när listan är tom
    function showEmptyState() {
        const itemsList = document.querySelector('.items-list');
        if (!itemsList) return;

        const emptyState = document.createElement('div');
        emptyState.className = 'empty-state';
        emptyState.innerHTML = `
            <div class="empty-state-icon">
                <svg>...</svg>
            </div>
            <h3>No items found</h3>
            <p>Add your first item to get started</p>
            <button class="btn primary" data-modal="true" data-target="#addModal">Add Item</button>
        `;

        itemsList.appendChild(emptyState);
    }

    // Bilduppladdningsförhandsvisning (om du har detta)
    document.querySelectorAll('input[type="file"]').forEach(input => {
        input.addEventListener('change', function () {
            const imagePreviewer = this.closest('form').querySelector('.image-previewer');
            const imagePreview = this.closest('form').querySelector('.image-preview');

            if (this.files && this.files[0] && imagePreview) {
                const reader = new FileReader();

                reader.onload = function (e) {
                    imagePreview.src = e.target.result;
                    if (imagePreviewer) {
                        imagePreviewer.classList.add('selected');
                    }
                };

                reader.readAsDataURL(this.files[0]);
            }
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