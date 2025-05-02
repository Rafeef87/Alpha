// Get the toggle element
const toggle = document.getElementById('darkModeToggle');

// Function to set theme
function setTheme(isDark) {
    // Set the data-theme attribute on the document element
    document.documentElement.setAttribute('data-theme', isDark ? 'dark' : 'light');

    // Add or remove the 'dark' class (useful if using Tailwind or similar)
    if (isDark) {
        document.documentElement.classList.add('dark');
    } else {
        document.documentElement.classList.remove('dark');
    }

    // Update the toggle state
    if (toggle) {
        toggle.checked = isDark;
    }

    // Save preference to localStorage
    localStorage.setItem('theme', isDark ? 'dark' : 'light');
}

// Toggle event listener
if (toggle) {
    toggle.addEventListener('change', () => {
        const isDark = toggle.checked;
        setTheme(isDark);
    });
}

// Initialize theme on page load
function initializeTheme() {
    // Check for saved preference
    const savedTheme = localStorage.getItem('theme');

    // If no saved preference, check for system preference
    if (!savedTheme) {
        const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        setTheme(prefersDark);
        return;
    }

    // Apply saved preference
    const isDark = savedTheme === 'dark';
    setTheme(isDark);
}

// Initialize when DOM is loaded
if (document.readyState === 'loading') {
    window.addEventListener('DOMContentLoaded', initializeTheme);
} else {
    // If DOMContentLoaded already fired
    initializeTheme();
}

// Optional: Listen for system theme changes
window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
    // Only apply if no user preference is saved
    if (!localStorage.getItem('theme')) {
        setTheme(e.matches);
    }
});
