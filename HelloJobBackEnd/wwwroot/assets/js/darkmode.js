const currentTheme = localStorage.getItem('theme');
const dayBtn = document.querySelector('.day');
const nightBtn = document.querySelector('.night');
const header=document.querySelector(".header");
const main=document.querySelector(".main");
function toggleTheme() {
  const currentTheme = localStorage.getItem('theme');

  if (currentTheme === 'dark') {
    document.documentElement.setAttribute('data-theme', 'light');
    dayBtn.style.display="none"
    localStorage.setItem('theme', 'light');
    header.classList.remove("dark");
    main.classList.remove("dark");
    
  } else {
    document.documentElement.setAttribute('data-theme', 'dark');
    localStorage.setItem('theme', 'dark');
    dayBtn.style.display="block"
    header.classList.add("dark");
    main.classList.add("dark");
  }
}

if (currentTheme === 'dark') {
  document.documentElement.setAttribute('data-theme', 'dark');
  dayBtn.style.display="block";
  header.classList.add("dark");
  main.classList.add("dark");
} else {
  document.documentElement.setAttribute('data-theme', 'light');
  dayBtn.style.display="none";
  header.classList.remove("dark");
  main.classList.remove("dark");
 
}
nightBtn.addEventListener('click', toggleTheme);
dayBtn.addEventListener('click', toggleTheme);