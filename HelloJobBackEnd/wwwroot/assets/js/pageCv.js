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




const inputFile = document.getElementById('cv_inp');
const cvLabel = document.querySelector('.cv_label');
const cvFile = document.querySelector('.cv-file');

inputFile.addEventListener('change', function() {
const file = this.files[0];
if (file) {
if (!['application/pdf', 'application/vnd.openxmlformats-officedocument.wordprocessingml.document'].includes(file.type)) {
alert('Zəhmət olmasa pdf və ya docx fayl yükləyin');
return;
} else if (file.size > 3 * 1024 * 1024) {
alert('Faylın ölçüsü 3MB-dan az olmalıdır.');
return;
}
cvFile.style.display = 'flex';
cvLabel.style.display = 'none';
cvFile.querySelector('.file_name').textContent = file.name;
}
});

cvFile.querySelector('.remove').addEventListener('click', function() {
inputFile.value = '';
cvFile.style.display = 'none';
cvLabel.style.display = 'block';
});