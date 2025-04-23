document.addEventListener('DOMContentLoaded', () => {
    //updateRelativeTimes()
    //setInterval(updateRelativeTimes, 6000)
    //const previewSize = 144

    // open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]')
    modalButtons.forEach(button => {
        button.addEventListener("click", () => {
            const modalTarget = button.getAttribute('data-target')
            const modal = document.querySelector(modalTarget)

            if (modal)
                modal.style.display = 'flex';
        })
    })

    //close modal
    const closeButtons = document.querySelectorAll('[data-close="true"]')
    closeButtons.forEach(button => {
        button.addEventListener("click", () => {
            const modal = button.closest('.modal')
            if (modal) {
                modal.style.display = 'none'

                modal.querySelectorAll('form').forEach(form => {
                    form.reset()
                    clearErrorMessages(form)

                    const imagePreview = form.querySelector('.image-preview')
                    if (imagePreview)
                        imagePreview.src = ''

                    const imageContainer = form.querySelector('.image-preview-container')
                    if (imageContainer) {
                        imageContainer.classList.remove('selected')
                        imageContainer.classList.remove('borderless')
                    }

                })

            }
        })
    })


    // WYSIWYG editor
    const addProjectDescriptionTextArea = document.getElementById('add-project-description')
    const addProjectDescriptionQuill = new Quill('#add-project-description-wysiwyg-editor', {
        modules: {
            syntax: true,
            toolbar: '#add-project-description-wysiwyg-toolbar',
        },
        placeholder: 'Type something',
        theme: 'snow',
    });

    addProjectDescriptionQuill.on('text-change', () => {
        addProjectDescriptionTextArea.value = addProjectDescriptionQuill.root.innerHTML
    })

    editProjectDescriptionTextArea = document.getElementById('edit-project-description')
    window.editProjectDescriptionQuill = new Quill('#edit-project-description-wysiwyg-editor', {
        modules: {
            syntax: true,
            toolbar: '#edit-project-description-wysiwyg-toolbar',
        },
        theme: 'snow',
    });

    window.editProjectDescriptionQuill.on('text-change', () => {
        editProjectDescriptionTextArea.value = editProjectDescriptionQuill.root.innerHTML
    })

})

// handle image preview
function handleImagePreview(config) {
    const previewSize = 144
    const container = document.getElementById(config.containerId)
    if (!container) return

    const fileInput = container.querySelector('input[type="file"]')
    if (!fileInput) return

    const imagePreview = document.getElementById(config.imagePreview)
    if (!imagePreview) return

    container.addEventListener('click', () => fileInput.click())

    fileInput.addEventListener("change", (e) => {
        const file = e.target.files[0]
        const isRound = container.classList.contains('.circle')

        if (file) {
            processImage(file, imagePreview, container, previewSize, isRound)
        }
    })

}

async function loadImage(file) {
    return new Promise(function (resolve, reject) {

        const reader = new FileReader()

        reader.onerror = () => reject(new Error("Failed to load file"))
        reader.onload = (e) => {
            const img = new Image()
            img.onerror = () => reject(new Error("Failed to load image"))
            img.onload = () => resolve(img)
            img.src = e.target.result
        }

        reader.readAsDataURL(file)
    })
}



async function processImage(file, imagePreview, container, previewSize = 144, isRound = false) {
    try {
        const img = await loadImage(file)
        const canvas = document.createElement('canvas')
        canvas.height = previewSize
        canvas.width = previewSize
        const context = canvas.getContext('2d')

        const scale = Math.min(previewSize / img.width, previewSize / img.height)
        const newWidth = img.width * scale
        const newHeight = img.height * scale

        context.fillStyle = "white"
        context.fillRect(0, 0, previewSize, previewSize);


        if (isRound) {
            context.beginPath()
            context.arc(previewSize / 2, previewSize / 2, previewSize / 2, 0, 2 * Math.PI)
            context.closePath()
            context.clip()
        }

        context.drawImage(img, (previewSize - newWidth) / 2, (previewSize - newHeight) / 2, newWidth, newHeight)

        imagePreview.src = canvas.toDataURL('image/jpeg')
        container.classList.add('selected')
        imagePreview.classList.remove("d-none")
        container.classList.add("borderless")

    }
    catch (error) {
        console.error('Failed to process image:', error)
    }
}