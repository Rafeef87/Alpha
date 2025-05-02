document.addEventListener('DOMContentLoaded', () => {
    if (!getCookie("cookieConsent"))
    showCookieModal()
})

function showCookieModal() {
    const modal = document.getElementById('cookieModal')
    if (modal) modal.style.display = 'flex'

    const consentValue = getCookie("cookieConsent")
    if (!consentValue) return
    // Parse the cookie value and set the checkboxes accordingly
    try {
        const consent = JSON.parse(consentValue)
        document.getElementById('cookieFunctional').checked = consent.functional
        document.getElementById('cookieAnalytics').checked = consent.analytics
        document.getElementById('cookieMarketing').checked = consent.marketing
    }
    catch (error) {
        console.error('Error parsing cookie consent:', error)
    }
}


function hideCookieModal() {
    const modal = document.getElementById('cookieModal')
    if (modal) modal.style.display = 'none'
}
function getCookie(name) {
    const nameEQ = name + "="
    const cookies = document.cookie.split(';')
    for (let cookie of cookies) {
        cookie = cookie.trim()
        if (cookie.indexOf(nameEQ) === 0) {
            return decodeURIComponent(cookie.substring(nameEQ.length))
        }
    }
    return null
}

function setCookie(name, value, days) {
    let expires = ""
    if (days) {
        const date = new Date()
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000))
        expires = "; expires=" + date.toUTCString()
    }
    const encodedValue = encodeURIComponent(value || "")
    document.cookie = `${name} =${encodedValue}${expires}; path=/"; SameSite=Lax`
}

async function acceptAll() {
    const consent = {
        essential: true,
        functional: true,
        analytics: true,
        marketing: true
    }
  
    await handelConsent(consent)
    hideCookieModal()
}

async function acceptSelected() {
    const form = document.getElementById('cookieConsentForm');
    const formData = new FormData(form);
    const consent = {
        essential: true,
        functional: formData.get('functional') === 'on',
        analytics: formData.get('analytics') === 'on',
        marketing: formData.get('marketing') === 'on'
    }
   
    await handelConsent(consent)
    hideCookieModal()
}

async function handelConsent(consent) {
    try { 
        const response = await fetch('/cookies/setcookies', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(consent)
        });
        if (!response.ok) {
            console.error('Error sending cookie consent:', await response.text());
        }
    }
    catch (error) {
        console.error('Error:', error);
    }
}

