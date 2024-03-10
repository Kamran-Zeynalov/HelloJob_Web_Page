const images = document.querySelectorAll(".image");

images.forEach(img => {
    img.addEventListener("click", () => {
        img.classList.toggle("open-image");
        const parent = img.closest(".stick");
        const focus = parent.querySelector(".focus");
        focus.classList.toggle("opened");
    });
});
