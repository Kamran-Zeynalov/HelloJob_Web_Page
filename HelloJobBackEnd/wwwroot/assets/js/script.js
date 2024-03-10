





const menubtn = document.querySelector(".menu_open");
const mobmenu = document.querySelector(".mobile_menu")
menubtn.addEventListener("click", function () {
    mobmenu.classList.add("active");
});

const closebtn = document.querySelector(".menu_close")
closebtn.addEventListener("click", function () {
    mobmenu.classList.remove("active");
});

let unlikedIcons = document.querySelectorAll('.unliked');
let likedIcons = document.querySelectorAll('.liked');

unlikedIcons.forEach((btn, index) => {
    btn.addEventListener('click', () => {
        likedIcons[index].classList.toggle('hidden');
    });
});

likedIcons.forEach((btn, index) => {
    btn.addEventListener('click', () => {
        btn.classList.toggle('hidden');
    });
});

let likehref = document.querySelectorAll('.like-button');
let unlikehref = document.querySelectorAll('.unliked-button');

likehref.forEach((btn) => {
    btn.addEventListener('click', () => {
        let itemId = btn.getAttribute('data-item-id');
        let itemType = btn.getAttribute('data-item-type');

        let formData = new FormData();
        formData.append('itemId', itemId);
        formData.append('itemType', itemType);

        fetch('/liked/AddToWishlist', {
            method: 'POST',
            body: formData
        })

    });
});

unlikehref.forEach((btn) => {
    btn.addEventListener('click', () => {
        let itemId = btn.getAttribute('data-item-id');
        let itemType = btn.getAttribute('data-item-type');

        let formData = new FormData();
        formData.append('itemId', itemId);
        formData.append('itemType', itemType);

        fetch('/liked/RemoveFromWishlist', {
            method: 'POST',
            body: formData
        })
    });
});

$(".responsive").slick({
    dots: false,
    infinite: true,
    variableWidth: true,
    speed: 300,
    autoplay: false,
    autoplaySpeed: 2000,
    slidesToShow: 4,
    slidesToScroll: 1,
    prevArrow: '<i class="fa-solid fa-chevron-right left_arrow">',
    nextArrow: ' <i class="fa-solid fa-chevron-left right_arrow"> ',

    responsive: [
        {
            breakpoint: 1024,
            settings: {
                slidesToShow: 3,
                slidesToScroll: 3,
                infinite: true,
                dots: false,
            },
        },
        {
            breakpoint: 600,
            settings: {
                slidesToShow: 2,
                slidesToScroll: 2,
            },
        },
        {
            breakpoint: 480,
            settings: {
                slidesToShow: 1,
                slidesToScroll: 1,
            },
        },
    ],
});
const login_btn = document.querySelectorAll(".login_btn");
const backgray = document.querySelector(".back");
const close_button = document.querySelectorAll(".close_button")

login_btn.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        backgray.classList.add("animate__fadeInDown");
        backgray.classList.add("active");
        document.body.style.overflowY = "hidden";

        setTimeout(function () {
            backgray.classList.remove("animate__fadeInDown");
        }, 1000);
    })
})


close_button.forEach(btn => {
    btn.addEventListener("click", (e) => {
        backgray.classList.add("animate__fadeOutUp");
        document.body.style.overflowY = "scroll";
        registration_body.classList.add("d-none");
        login_body.classList.remove("d-none");
        forgot_password.classList.add("d-none");
        setTimeout(function () {
            backgray.classList.remove("active");
            backgray.classList.remove("animate__fadeOutUp");
        }, 1000);
    })

})

const goRegister = document.querySelector(".goRegister");
const goLogin = document.querySelectorAll(".goLogin");
const login_body = document.querySelector(".login_body")
const registration_body = document.querySelector(".registration_body");
const regs_bopup_open = document.querySelector(".regs_bopup_open");
const forgot_password = document.querySelector(".forgot_password");



regs_bopup_open.addEventListener("click", (e) => {
    e.preventDefault();
    login_body.classList.add("d-none");
    forgot_password.classList.remove("d-none");
})


goRegister.addEventListener("click", (e) => {
    e.preventDefault();
    registration_body.classList.remove("d-none");
    login_body.classList.add("d-none");
})


goLogin.forEach(btn => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        registration_body.classList.add("d-none");
        login_body.classList.remove("d-none");
        forgot_password.classList.add("d-none");
    })
})


const account_head_user = document.querySelector(".profile__header__user__name");
const account_head_user_drop = document.querySelector(".account_head_user_drop");



account_head_user.addEventListener("click", (event) => {
    event.preventDefault();
    account_head_user_drop.classList.toggle("active");
});





