function initTagSelector(config) {
    let activeIndex = -1
    let selectedIds = []

    const tagContainer = document.getElementById(config.containerId)
    const input = document.getElementById(config.inputId)
    const result = document.getElementById(config.resultsId)
    const selectedInputIds = document.getElementById(config.selectedInputIds)

    // if config.preselected is an array, send every item in array to addTag function
    if (Array.isArray(config.preselected)) {
        config.preselected.forEach(item => {
            addTag(item)
        })
    }

    input.addEventListener('focus', () => {
        tagContainer.classList.add('focused')
        result.classList.add('focused')
    })

    input.addEventListener('blur', () => {
        setTimeout(() => {
            tagContainer.classList.remove('focused')
            result.classList.remove('focused')
        }, 100)
    })

    input.addEventListener('input', () => {
        const query = input.value.trim()
        activeIndex = -1

        // if input is empty, hide result div
        if (query.length === 0) {
            result.style.display = 'none'
            result.innerHTML = ''
            return
        }
        // otherwise, send query request, convert to json and send data to renderSearchResults function
        fetch(config.searchUrl(query))
            .then(r => r.json())
            .then(data => renderSearchResults(data))
    })

    input.addEventListener('keydown', (e) => {
        const items = result.querySelectorAll('.search-item')

        switch (e.key) {
            case 'ArrowDown':
                e.preventDefault()
                if (items.length > 0) {
                    activeIndex = (activeIndex + 1) % items.length
                    updateActiveItems(items)
                }
                break

            case 'ArrowUp':
                e.preventDefault()
                if (items.length > 0) {
                    activeIndex = (activeIndex - 1 + items.length) % items.length
                    updateActiveItems(items)
                }
                break

            case 'Enter':
                e.preventDefault()
                if (activeIndex >= 0 && items[activeIndex]) {
                    items[activeIndex].click()
                }
                break

            case 'Backspace':
                if (input.value === '') {
                    removeLastTag()
                }
                break
        }
    })

    function updateActiveItems(items) {
        items.forEach(item => item.classList.remove('active'))

        if (items[activeIndex]) {
            items[activeIndex].classList.add('active')
            items[activeIndex].scrollIntoView({ block: 'nearest' })
        }
    }

    function renderSearchResults(data) {
        result.innerHTML = ''

        if (data.length === 0) {
            const noResult = document.createElement('div')
            noResult.classList.add('search-item')
            noResult.textContent = config.emptyMessage || 'No results.'
            result.appendChild(noResult)
        } else {
            data.forEach(item => {
                if (!selectedIds.includes(item.id)) {
                    const resultItem = document.createElement('div')
                    resultItem.classList.add('search-item')
                    resultItem.dataset.id = item.id

                    if (config.tagClass === 'user-tag') {
                        resultItem.innerHTML =
                            `
                            <img class="user-avatar" src="${config.avatarFolder || ''} ${item[config.imageProperty]}">
                            <span>${item[config.displayProperty]}</span>
                            `
                    } else {
                        resultItem.innerHTML =
                            `
                            <span>${item[config.displayProperty]}</span>
                            `
                    }

                    resultItem.addEventListener('click', () => {
                        addTag(item)
                    })
                    result.appendChild(resultItem)
                }
            })
            result.style.display = 'block'
        }
    }


    function addTag(item) {
        const id = parseInt(item.id)

        // do not add tag if already added
        if (selectedIds.includes(id)) {
            return
        }

        selectedIds.push(id)

        // add tag to list
        const tag = document.createElement('div')
        tag.classList.add(config.tagClass || 'tag')
        if (config.tagClass === 'user-tag') {
            tag.innerHTML =
                `
                <img class="user-avatar" src="${config.avatarFolder || ''} ${item[config.imageProperty]}">
                <span>${item[config.displayProperty]}</span>
                `
        } else {
            tag.innerHTML =
                `
                <span>${item[config.displayProperty]}</span>
                `
        }

        const removeBtn = document.createElement('span')
        removeBtn.textContent = 'x'
        removeBtn.classList.add('btn-remove')
        removeBtn.dataset.id = id

        removeBtn.addEventListener('click', (e) => {
            selectedIds = selectedIds.filter(i => i !== id)
            tag.remove()
            updateSelectedIdsInput()
            e.stopPropagation()
        })

        tag.appendChild(removeBtn)
        tagContainer.insertBefore(tag, input)
        input.value = ''
        result.innerHTML = ''
        result.style.display = 'none'

        updateSelectedIdsInput()
    }

    function removeLastTag() {
        const tags = tagContainer.querySelectorAll(`.${config.tagClass}`)

        if (tags.length === 0) {
            return
        }

        const lastTag = tags[tags.length - 1]
        const lastId = parseInt(lastTag.querySelector('.btn-remove').dataset.id)

        selectedIds = selectedIds.filter(id => id !== lastId)
        lastTag.remove()
        updateSelectedIdsInput()
    }

    function updateSelectedIdsInput() {
        const hiddenInputs = selectedInputIds
        if (hiddenInputs) {
            hiddenInputs.value = JSON.stringify(selectedIds)
        }
    }
}