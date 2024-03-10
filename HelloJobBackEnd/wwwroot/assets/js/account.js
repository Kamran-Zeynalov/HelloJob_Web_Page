const person = document.querySelector(".person");
const persons_menu = document.querySelector(".persons_menu");

person.addEventListener("click", (event) => {
    event.preventDefault();
    persons_menu.classList.toggle("active");
    hamburgers_menu.classList.remove("active");
});

const hamburgers = document.querySelector(".hamburgers");
const hamburgers_menu = document.querySelector(".hamburgers_menu");

hamburgers.addEventListener("click", (event) => {
    event.preventDefault();
    hamburgers_menu.classList.toggle("active");
    persons_menu.classList.remove("active");
});




const account_head_user = document.querySelector(".profile__header__user__name");
const account_head_user_drop = document.querySelector(".account_head_user_drop");



account_head_user.addEventListener("click", (event) => {
    event.preventDefault();
    account_head_user_drop.classList.toggle("active");
});




