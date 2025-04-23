﻿document.addEventListener('DOMContentLoaded', () => {
    if (!getCookie("cookieConsent"))
        showCookieModal()
})

function showCookieModal() {
    const cookieModal = document.querySelector('#cookieModal')
    if (cookieModal)
        cookieModal.style.display = "flex"

    const consentValue = getCookie("cookieConsent")
    if (!consentValue) return

    try {
        const consent = JSON.parse(consentValue)
        document.getElementById("cookieFunctional").checked = consent.functional
        document.getElementById("cookieMarketing").checked = consent.marketing
        document.getElementById("cookieAnalytics").checked = consent.analytics
    }
    catch (error) {
        console.error("Unable to handle cookie consent values", error)
    }
}

function hideCookieModal() {
    const cookieModal = document.querySelector('#cookieModal')
    if (cookieModal)
        cookieModal.style.display = "none"
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
        date.setTime(date.getTime() + days * 1000 * 60 * 60 * 24)
        expires = "; expires=" + date.toUTCString()
    }

    const encodedValue = encodeURIComponent(value || "")
    document.cookie = `${name}=${encodedValue}${expires}; path=/; SameSite=Lax`
}

async function acceptAllCookies() {
    const consent = {
        essential: true,
        functional: true,
        analytics: true,
        marketing: true
    }

    setCookie("cookieConsent", JSON.stringify(consent), 365)
    await setConsent(consent)
    hideCookieModal()
}

async function acceptSelectedCookies() {
    const form = document.getElementById('cookieConsentForm')
    const formData = new FormData(form)

    const consent = {
        essential: true,
        functional: formData.get("functional") === "on",
        analytics: formData.get("analytics") === "on",
        marketing: formData.get("marketing") === "on"
    }

    setCookie("cookieConsent", JSON.stringify(consent), 365)
    await setConsent(consent)
    hideCookieModal()
}

async function setConsent(consent) {
    try {
        const res = await fetch('/cookies/setcookies', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(consent)
        })

        if (!res.ok) {
            console.error('Unable to set cookie consent', await res.text())
        }
    }
    catch (error) {
        console.log("Error: ", error)
    }
}