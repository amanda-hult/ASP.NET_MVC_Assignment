let darkModeSwitch;

document.addEventListener('DOMContentLoaded', () => {

    const cookie = getCookie("cookieConsent")
    if (cookie) {
        const parsedCookie = JSON.parse(cookie)

        if (!parsedCookie.functional) {
            const darkModeContainer = document.querySelector('.dark-mode-container')
            darkModeContainer.style.display = 'none'
        }
    }

    const darkModeLabel = document.querySelector('.dark-mode-label')
    darkModeSwitch = document.querySelector('#darkModeToggle')

    const savedTheme = localStorage.getItem('darkmode')
    if (savedTheme === 'true') {
        document.documentElement.classList.add('dark-theme')
        darkModeSwitch.checked = true
    }

    darkModeSwitch.addEventListener('change', toggleDarkMode)

    darkModeLabel.addEventListener('click', () => {
        darkModeSwitch.checked = !darkModeSwitch.checked
        toggleDarkMode()
    })
})

function toggleDarkMode() {
    const isDarkMode = darkModeSwitch.checked
    document.documentElement.classList.toggle('dark-theme', darkModeSwitch.checked)
    localStorage.setItem('darkmode', isDarkMode ? 'true' : 'false')
}