document.addEventListener('DOMContentLoaded', () => {

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")
        .build()

    connection.on("RecieveNotification", function (notification) {
        const notifications = document.querySelector('.notifications')

        const item = document.createElement('div')
        item.className = 'notification-item'
        item.setAttribute('data-id', notification.id)
        item.innerHTML =
            `
                <img class="image" src="${notification.image}" />
                <div class="message">${notification.message}</div >
                <div class="time" data-created="${new Date(notification.created).toISOString()}">${notification.created}</div>
                <button class="btn-close" onclick="dismissNotification('${notification.id}')"></button>
            `

        notifications.insertBefore(item, notifications.firstChild)

        updateRelativeTimes()
        updateNotificationCount()
    })

    connection.on("NotificationDismissed", function (notificatinoId) {
        const element = document.querySelector(`.notification-item[data-id="${notificationId}]"`)
        if (element) {
            element.remove()
            updateNotificationCount()
        }
    })

    connection.start().catch(error => console.error(error))

})


async function dismissNotification(notificationId) {
    try {
        const res = await fetch(`/api/notifications/dismiss/${notificationId}`, { method: 'POST' })
        if (res.ok) {
            const element = document.querySelector(`.notification-item[data-id="${notificationId}]"`)
            if (element) {
                element.remove()
                updateNotificationCount()
            }
            else {
                console.error('Error removing notification: ')
            }
        }
    }
    catch (error) {
        console.error('Error removing notification: ', error)
    }
}

function updateNotificationCount() {
    const notifications = document.querySelector('.notifications')
    const notificationNumber = document.querySelector('.notification-number')
    const notificationDropdownButton = document.querySelector('#notification-dropdown-button')
    const count = notifications.querySelectorAll('.notification-item').length

    if (notificationNumber) {
        notificationNumber.textContent = count
    }

    let dot = notificationDropdownButton.querySelector('.dot.dot-red')
    if (count > 0 && !dot) {
        dot = document.createElement('div')
        dot.className = 'dot dot-red'
        notificationDropdownButton.appendChild(dot)
    }

    if (count === 0 && dot) {
        dot.remove()
    }
}

function updateRelativeTimes() {
    const elements = document.querySelectorAll('.notification-item.time')
    const now = new Date();

    elements.forEach(element => {
        const created = new Date(element.getAttribute('data-created'))
        const diff = now - created
        const diffSeconds = Math.floor(diff / 1000)
        const diffMinutes = Math.floor(diffSeconds / 60)
        const diffHours = Math.floor(diffMinutes / 60)
        const diffDays = Math.floor(diffHours / 24)
        const diffWeeks = Math.floor(diffDays / 7)

        let relativeTime = ''

        if (diffMinutes < 1) {
            relativeTime = '0 min ago'
        }
        else if (diffMinutes < 60) {
            relativeTime = diffMinutes + ' min ago'
        }
        else if (diffHours < 2) {
            relativeTime = diffHours + ' hour ago'
        }
        else if (diffHours < 24) {
            relativeTime = diffHours + ' hours ago'
        }
        else if (diffDays < 2) {
            relativeTime = diffDays + ' day ago'
        }
        else if (diffDays < 7) {
            relativeTime = diffDays + ' days ago'
        }
        else {
            relativeTime = diffWeeks + ' weeks ago'
        }

        element.texContent = relativeTime;
    })
}