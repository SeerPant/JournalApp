//creating quill object
let quill;

//Creates quill object
//window lets us access the variable from anywhere
window.initQuill = (divId) => {
  //id is used
  //backtick being used for string interpolation
  quill = new Quill(`#${divId}`, { theme: "snow" });
};

window.getQuillHtml = () => {
  return quill.root.innerHTML; //taking HTML of tag, gets content written in quill
};

window.setQuillHTML = (htmlContent) => {
  quill.root.innerHTML = htmlContent;
};
