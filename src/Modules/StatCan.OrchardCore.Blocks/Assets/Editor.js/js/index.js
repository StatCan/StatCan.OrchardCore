import Delimiter from '@editorjs/delimiter';
import Embed from '@editorjs/embed';
import Header from '@editorjs/header';
import List from '@editorjs/list';
import Paragraph from '@editorjs/paragraph';
import Raw from '@editorjs/raw';
import Quote from '@editorjs/quote';

import EditorJS from '@editorjs/editorjs';

import LinkTool from './plugins/link';
import MediaLibrary from './plugins/mediaLibrary';

window.initializeEditorJS = (
    tenantPath,
    id,
    hiddenFieldId,
    typeName,
    partName,
    fieldName
) => {
    const $hiddenField = document.getElementById(hiddenFieldId);

    if (!$hiddenField) {
        return;
    }

    const $form = $hiddenField.closest('form');

    if (!$form) {
        return;
    }

    const editor = new EditorJS({
        holder: id,

        placeholder: 'Enter your content here...',

        tools: {
            delimiter: Delimiter,
            embed: {
                class: Embed,
                config: {
                    services: {
                        'twitch-channel': true,
                        'twitch-video': true,
                        vimeo: true,
                        youtube: true,
                    },
                },
                inlineToolbar: true,
            },
            header: {
                class: Header,
                inlineToolbar: true,
            },
            image: {
                class: MediaLibrary,
                config: {
                    id,
                },
            },
            link: {
                class: LinkTool,
                config: {
                    fieldName,
                    partName,
                    typeName,
                    tenantPath,
                },
            },
            list: {
                class: List,
                inlineToolbar: true,
            },
            paragraph: {
                class: Paragraph,
                inlineToolbar: true,
            },
            quote: Quote,
            raw: Raw,
        },

        data: !$hiddenField.value ? {} : JSON.parse($hiddenField.value),

        onChange: () => {
            editor
                .save()
                .then(outputData => {
                    $hiddenField.value = JSON.stringify(outputData);
                    document.dispatchEvent(new Event('contentpreview:render'));
                })
                .catch(error => {});
        },
    });

    const onSubmit = e => {
        editor
            .save()
            .then(outputData => {
                $hiddenField.value = JSON.stringify(outputData);
                $form.removeEventListener('submit', onSubmit);
                $form.submit();
            })
            .catch(error => {});
    };

    $form.addEventListener('submit', onSubmit);
};
