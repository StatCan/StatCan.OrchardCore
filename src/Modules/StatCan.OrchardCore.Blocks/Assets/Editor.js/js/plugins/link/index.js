import './index.css';

const linkIcon = `<svg width="13" height="14" xmlns="http://www.w3.org/2000/svg">
	<path d="M8.567 13.629c.728.464 1.581.65 2.41.558l-.873.873A3.722 3.722 0 1 1 4.84 9.794L6.694 7.94a3.722 3.722 0 0 1 5.256-.008L10.484 9.4a5.209 5.209 0 0 1-.017.016 1.625 1.625 0 0 0-2.29.009l-1.854 1.854a1.626 1.626 0 0 0 2.244 2.35zm2.766-7.358a3.722 3.722 0 0 0-2.41-.558l.873-.873a3.722 3.722 0 1 1 5.264 5.266l-1.854 1.854a3.722 3.722 0 0 1-5.256.008L9.416 10.5a5.2 5.2 0 0 1 .017-.016 1.625 1.625 0 0 0 2.29-.009l1.854-1.854a1.626 1.626 0 0 0-2.244-2.35z" transform="translate(-3.667 -2.7)" />
</svg>
`;

const unlinkIcon = `<svg width="16" height="18" viewBox="0 0 16 18" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">
    <path transform="rotate(-45 8.358 11.636)" d="M9.14 9.433c.008-.12-.087-.686-.112-.81a1.4 1.4 0 0 0-1.64-1.106l-3.977.772a1.4 1.4 0 0 0 .535 2.749l.935-.162s.019 1.093.592 2.223l-1.098.148A3.65 3.65 0 1 1 2.982 6.08l3.976-.773c1.979-.385 3.838.919 4.28 2.886.51 2.276-1.084 2.816-1.073 2.935.011.12-.394-1.59-1.026-1.696zm3.563-.875l2.105 3.439a3.65 3.65 0 0 1-6.19 3.868L6.47 12.431c-1.068-1.71-.964-2.295-.49-3.07.067-.107 1.16-1.466 1.48-.936-.12.036.9 1.33.789 1.398-.656.41-.28.76.13 1.415l2.145 3.435a1.4 1.4 0 0 0 2.375-1.484l-1.132-1.941c.42-.435 1.237-1.054.935-2.69zm1.88-2.256h3.4a1.125 1.125 0 0 1 0 2.25h-3.4a1.125 1.125 0 0 1 0-2.25zM11.849.038c.62 0 1.125.503 1.125 1.125v3.4a1.125 1.125 0 0 1-2.25 0v-3.4c0-.622.503-1.125 1.125-1.125z"/>
</svg>`;

const ENTER_KEY = 13;

export default class LinkTool {
    static get isInline() {
        return true;
    }

    static get sanitize() {
        return {
            a: {
                href: true,
                target: '_blank',
                rel: 'nofollow',
            },
        };
    }

    constructor({ api, config }) {
        this.state = false;

        this.nodes = {
            button: null,
            editor: null,
            input: null,
            list: null,
        };

        this.tag = 'a';
        this.class = 'cdx-link';

        this.api = api;
        this.config = config;
        this.inlineToolbar = api.inlineToolbar;
        this.notifier = api.notifier;
        this.toolbar = api.toolbar;
        this.CSS = {
            button: 'ce-inline-tool',
            buttonActive: 'ce-inline-tool--active',
            buttonModifier: 'ce-inline-tool--link',
            editorActive: 'link-tool-editor--active',
            input: 'ce-inline-tool-input',
            inputShowed: 'ce-inline-tool-input--showed',
        };
    }

    clear() {
        this.closeActions();
    }

    render() {
        this.nodes.button = document.createElement('button');
        this.nodes.button.type = 'button';
        this.nodes.button.classList.add(this.CSS.button);
        this.nodes.button.innerHTML = linkIcon;

        return this.nodes.button;
    }

    renderActions() {
        this.nodes.editor = document.createElement('div');
        this.nodes.editor.classList.add('link-tool-editor');

        this.nodes.input = document.createElement('input');
        this.nodes.input.placeholder = 'Type link or search by title';
        this.nodes.input.classList.add(this.CSS.input);
        this.nodes.input.classList.add(this.CSS.inputShowed);
        this.nodes.input.addEventListener('keydown', event => {
            if (event.keyCode === ENTER_KEY) {
                this.enterPressed(event);
                return;
            }
        });

        this.nodes.input.addEventListener('keyup', event => {
            const _this = this;

            if (event.keyCode === ENTER_KEY) {
                return;
            }

            if (this.nodes.input.value.length > 2) {
                fetch(
                    `${this.config.tenantPath}/Blocks/SearchContentItems?type=${this.config.typeName}&part=${this.config.partName}&field=${this.config.fieldName}&query=${this.nodes.input.value}`
                )
                    .then(response => response.json())
                    .then(contentItems =>
                        _this.displayContentItems(contentItems)
                    );
            }
        });

        this.nodes.editor.appendChild(this.nodes.input);

        this.nodes.list = document.createElement('ul');
        this.nodes.list.classList.add('link-tool-editor__content-items');
        this.nodes.editor.appendChild(this.nodes.list);

        return this.nodes.editor;
    }

    surround(range) {
        if (this.state) {
            this.removeLink();
            this.closeActions();
            this.inlineToolbar.close();
            return;
        }

        if (range) {
            const selectedText = range.extractContents();
            this.placeholder = document.createElement('span');
            this.placeholder.style.backgroundColor = '#a8d6ff';
            this.placeholder.appendChild(selectedText);
            range.insertNode(this.placeholder);
        }

        this.toggleActions();
    }

    checkState() {
        const anchorTag = this.api.selection.findParentTag('A');

        if (anchorTag) {
            this.nodes.button.classList.add(this.CSS.buttonActive);
            this.nodes.button.innerHTML = unlinkIcon;
            this.nodes.input.value = anchorTag.getAttribute('href');
            this.state = anchorTag;
            this.openActions();
        } else {
            this.nodes.button.innerHTML = linkIcon;
            this.nodes.button.classList.remove(this.CSS.buttonActive);
        }
    }

    applyUrl(url) {
        if (!this.placeholder) {
            return;
        }

        let link = document.createElement('a');
        link.innerHTML = this.placeholder.innerText;
        link.href = url;

        this.placeholder.parentNode.replaceChild(link, this.placeholder);

        this.placeholder = null;
    }

    closeActions() {
        if (this.placeholder) {
            this.placeholder.parentNode.innerHTML = this.placeholder.parentNode.innerHTML.replace(
                this.placeholder.outerHTML,
                this.placeholder.innerText
            );
            this.placeholder = null;
        }

        if (this.nodes.editor) {
            this.nodes.editor.classList.remove(this.CSS.editorActive);
            this.nodes.input.value = '';
            this.nodes.list.innerHTML = '';
        }

        this.inputOpened = false;
    }

    displayContentItems(contentItems) {
        this.nodes.list.innerHTML = '';

        contentItems.forEach(contentItem => {
            let button = document.createElement('button');
            button.innerText = contentItem.displayText;
            button.setAttribute('data-href', contentItem.url);

            let listItem = document.createElement('li');
            listItem.title = contentItem.displayText;
            listItem.appendChild(button);

            button.addEventListener('click', event => {
                this.selectContentItem(event);
            });

            this.nodes.list.appendChild(listItem);
        });
    }

    enterPressed(event) {
        let value = this.nodes.input.value || '';

        if (!value.trim()) {
            event.preventDefault();
            this.closeActions();
            return;
        }

        this.applyUrl(value);

        /**
         * Preventing events that will be able to happen
         */
        event.preventDefault();
        event.stopPropagation();
        event.stopImmediatePropagation();
        this.inlineToolbar.close();
    }

    openActions(needFocus) {
        this.nodes.editor.classList.add(this.CSS.editorActive);

        if (needFocus) {
            this.nodes.input.focus();
        }

        this.inputOpened = true;
    }

    removeLink() {
        if (!this.state) {
            return;
        }

        this.state.parentNode.innerHTML = this.state.parentNode.innerHTML.replace(
            this.state.outerHTML,
            this.state.innerText
        );

        this.state = null;
    }

    selectContentItem(event) {
        const itemUrl = event.target.getAttribute('data-href');

        this.applyUrl(itemUrl);
        this.closeActions();
        this.inlineToolbar.close();
    }

    toggleActions() {
        if (!this.inputOpened) {
            this.openActions(true);
        } else {
            this.closeActions(false);
        }
    }
}
