import 'bootstrap';
import $ from 'jquery';

import Ui from './ui';
import './index.css';

const selectors = {
    mediaApp: '#mediaApp',
    mediaFieldSelectButton: '.mediaFieldSelectButton',
    modalBody: '.modal-body',
};

export default class MediaLibraryTool {
    static get pasteConfig() {
        return {
            patterns: {
                image: /https?:\/\/\S+\.(gif|jpe?g|tiff|png)$/i,
            },
            tags: ['IMG'],
        };
    }

    static get toolbox() {
        return {
            title: 'Image',
            icon:
                '<svg width="17" height="15" viewBox="0 0 336 276" xmlns="http://www.w3.org/2000/svg"><path d="M291 150V79c0-19-15-34-34-34H79c-19 0-34 15-34 34v42l67-44 81 72 56-29 42 30zm0 52l-43-30-56 30-81-67-66 39v23c0 19 15 34 34 34h178c17 0 31-13 34-29zM79 0h178c44 0 79 35 79 79v118c0 44-35 79-79 79H79c-44 0-79-35-79-79V79C0 35 35 0 79 0z"/></svg>',
        };
    }

    constructor({ data, config, api }) {
        this.api = api;
        this.config = config || {};

        this.data = {
            url: data.url || '',
            caption: data.caption || '',
            stretched: data.stretched !== undefined ? data.stretched : false,
        };

        this.modalBodyElement = document.getElementById(
            `${config.id}-ModalBody`
        );

        this.ui = new Ui(this.api, () => {
            this._openMediaLibrary();
        });
    }

    appendCallback() {
        this._openMediaLibrary();
    }

    onPaste(event) {
        switch (event.type) {
            case 'pattern':
                const src = event.detail.data;

                this._setMedia({
                    mediaPath: src,
                    url: src,
                });

                break;
            case 'tag':
                const imgTag = event.detail.data;

                this._setMedia({
                    mediaPath: imgTag.src,
                    url: imgTag.src,
                });

                break;
        }
    }

    render() {
        return this.ui.render(this.data);
    }

    renderSettings() {
        const settings = [
            {
                name: 'stretched',
                icon: `<svg width="17" height="10" viewBox="0 0 17 10" xmlns="http://www.w3.org/2000/svg"><path d="M13.568 5.925H4.056l1.703 1.703a1.125 1.125 0 0 1-1.59 1.591L.962 6.014A1.069 1.069 0 0 1 .588 4.26L4.38.469a1.069 1.069 0 0 1 1.512 1.511L4.084 3.787h9.606l-1.85-1.85a1.069 1.069 0 1 1 1.512-1.51l3.792 3.791a1.069 1.069 0 0 1-.475 1.788L13.514 9.16a1.125 1.125 0 0 1-1.59-1.591l1.644-1.644z"/></svg>`,
            },
        ];

        const wrapper = document.createElement('div');

        settings.forEach(tune => {
            let button = document.createElement('div');

            button.classList.add('cdx-settings-button');
            button.innerHTML = tune.icon;

            if (this.data[tune.name]) {
                button.classList.add(this.api.styles.settingsButtonActive);
            } else {
                button.classList.remove(this.api.styles.settingsButtonActive);
            }

            wrapper.appendChild(button);

            button.addEventListener('click', () => {
                this._toggleTune(tune.name);
                button.classList.toggle(this.api.styles.settingsButtonActive);
            });
        });

        return wrapper;
    }

    save() {
        this.data.caption = this.ui.getCaption();

        return this.data;
    }

    /**
     * Opens the Orchard Core media library.
     */
    _openMediaLibrary() {
        const self = this;

        $(selectors.mediaApp)
            .detach()
            .appendTo($(this.modalBodyElement).find(selectors.modalBody));

        $(selectors.mediaApp).show();

        const modal = $(this.modalBodyElement).modal();

        $(this.modalBodyElement)
            .find(selectors.mediaFieldSelectButton)
            .off('click')
            .on('click', async function () {
                if (window.mediaApp.selectedMedias.length) {
                    self._setMedia(window.mediaApp.selectedMedias[0]);
                }

                window.mediaApp.selectedMedias = [];

                modal.modal('hide');
                return true;
            });
    }

    /**
     * Updates block with selected media item.
     */
    _setMedia(media) {
        this.data = {
            caption: '',
            mediaPath: media.mediaPath,
            url: media.url,
        };

        this.ui.render(this.data);
    }

    _toggleTune(tune) {
        this.data[tune] = !this.data[tune];

        if (tune === 'stretched') {
            const blockId = this.api.blocks.getCurrentBlockIndex();

            setTimeout(() => {
                this.api.blocks.stretchBlock(blockId, this.data[tune]);
            }, 0);
        }
    }
}
