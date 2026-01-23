let quill;
window.initQuill = (divId) => {
    quill = new Quill(`#${divId}`, { theme: "snow" });
};
window.getQuillHTML = () => quill.root.innerHTML;
window.setQuillHTML = (htmlContent) => {
    quill.root.innerHTML = htmlContent;
};