window.QuillFunctions = {
    createQuill: function (quillElement) {
        var options = {
            debug: 'info',
            modules: {
                toolbar: [
                    [{ 'header': [1, 2, false] }],
                    ['bold', 'italic', 'underline', 'strike'],
                    ['blockquote', 'code-block'],
                    [{ 'list': 'ordered' }, { 'list': 'bullet' }],
                    [{ 'color': [] }, { 'background': [] }],
                    ['clean']
                ]
            },
            placeholder: 'Write your thoughts here...',
            theme: 'snow'
        };

        while (quillElement.firstChild) {
            quillElement.removeChild(quillElement.firstChild);
        }

        new Quill(quillElement, options);
    },
    getQuillContent: function (quillElement) {
        return quillElement.__quill.root.innerHTML;
    },
    getQuillText: function (quillElement) {
        return quillElement.__quill.getText();
    },
    loadQuillContent: function (quillElement, content) {
        return quillElement.__quill.root.innerHTML = content;
    },
    setQuillContent: function (quillElement, content) {
        if (quillElement && quillElement.__quill) {
            quillElement.__quill.root.innerHTML = content;
        }
    }
};

const originalQuill = Quill;
window.Quill = function (container, options) {
    const quill = new originalQuill(container, options);
    container.__quill = quill;
    return quill;
};
Object.assign(window.Quill, originalQuill);
