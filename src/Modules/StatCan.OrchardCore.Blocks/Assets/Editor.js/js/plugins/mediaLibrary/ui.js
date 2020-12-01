import { make } from '../utils/dom';

const buttonIcon = `<svg width="20" height="20" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg">
    <path d = "M3.15 13.628A7.749 7.749 0 0 0 10 17.75a7.74 7.74 0 0 0 6.305-3.242l-2.387-2.127-2.765 2.244-4.389-4.496-3.614 3.5zm-.787-2.303l4.446-4.371 4.52 4.63 2.534-2.057 3.533 2.797c.23-.734.354-1.514.354-2.324a7.75 7.75 0 1 0-15.387 1.325zM10 20C4.477 20 0 15.523 0 10S4.477 0 10 0s10 4.477 10 10-4.477 10-10 10z" />
</svg >`;

export default class Ui {
    constructor(api, onSelectFile) {
        this.api = api;
        this.onSelectFile = onSelectFile;
        this.nodes = {
            caption: make('div', ['cdx-input', 'media-library-item__caption'], {
                contentEditable: true,
            }),
            fileButton: this.createFileButton(),
            image: make('img', ['media-library-item__image']),
            imageWrapper: make('div', ['media-library-item__image']),
            item: make('div', ['media-library-item']),
            wrapper: make('div', ['cdx-block', 'media-library-tool']),
        };

        this.nodes.caption.dataset.placeholder = 'Caption...';

        this.nodes.imageWrapper.appendChild(this.nodes.image);
        this.nodes.item.appendChild(this.nodes.imageWrapper);
        this.nodes.item.appendChild(this.nodes.caption);
        this.nodes.wrapper.appendChild(this.nodes.item);

        this.nodes.wrapper.appendChild(this.nodes.fileButton);

        this.blockIndex = this.api.blocks.getCurrentBlockIndex() + 1;
    }

    createFileButton() {
        let button = make('div', [
            this.api.styles.button,
            'media-library-tool__select-file',
        ]);

        button.innerHTML = `${buttonIcon} Select an Image`;

        button.addEventListener('click', () => {
            this.onSelectFile();
        });

        return button;
    }

    getCaption() {
        return this.nodes.caption.innerHTML;
    }

    render(toolData) {
        this.nodes.image.src = toolData.url;
        this.nodes.image.onload = () => {
            this.api.blocks.stretchBlock(this.blockIndex, !!toolData.stretched);
        };

        this.nodes.caption.innerHTML = toolData.caption;

        if (!toolData.url) {
            this.nodes.wrapper.classList.add('is-empty');
        } else {
            this.nodes.wrapper.classList.remove('is-empty');
        }

        return this.nodes.wrapper;
    }
}
