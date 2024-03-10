const upload_image=document.querySelector(".upload_image");
const choose=document.querySelector(".choose");

upload_image.addEventListener("click", function() {
    choose.click();
  });


  const fileInput = document.getElementById('fileInput');
const preview = document.getElementById('preview');

fileInput.addEventListener('change', function() {
  const file = this.files[0];
  
  if (file && file.type.startsWith('image/')) {
    const reader = new FileReader();
    reader.onload = function() {
      preview.src = reader.result;
    }
    reader.readAsDataURL(file);
  } else {
    preview.src = '';
  }
});
