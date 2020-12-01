export const make = function make(tagName, classNames = null, attributes = {}) {
    let el = document.createElement(tagName);

    if (Array.isArray(classNames)) {
        el.classList.add(...classNames);
    } else if (classNames) {
        el.classList.add(classNames);
    }

    for (let attrName in attributes) {
        el[attrName] = attributes[attrName];
    }

    return el;
};
