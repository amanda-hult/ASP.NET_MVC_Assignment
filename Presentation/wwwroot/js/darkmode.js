let darkModeSwitch;

document.addEventListener('DOMContentLoaded', () => {
    const darkModeLabel = document.querySelector('.dark-mode-label')
    darkModeSwitch = document.querySelector('#darkModeToggle')

    const savedTheme = localStorage.getItem('darkmode')
    if (savedTheme === 'true') {
        document.body.classList.add('dark-theme')
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

    document.body.classList.toggle('dark-theme', darkModeSwitch.checked)

    localStorage.setItem('darkmode', isDarkMode ? 'true' : 'false')
}