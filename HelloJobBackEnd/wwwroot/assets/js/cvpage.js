//Range method
const range = document.querySelector('#ranger');
const intervals = document.querySelectorAll('.interval span');
const checkboxes = document.querySelectorAll('input[name="experienceIds"]');

const min = 0;
const max = 5;
const step = 1;

let currentValue = 0;
let currentPercent = 0;

range.addEventListener('mousedown', () => {
    document.addEventListener('mousemove', rangeMove);
});

document.addEventListener('mouseup', () => {
    document.removeEventListener('mousemove', rangeMove);
});

function rangeMove(event) {
    const rangeWidth = range.offsetWidth;
    const rangeLeft = range.getBoundingClientRect().left;
    let newValue = (event.clientX - rangeLeft) / rangeWidth * (max - min);

    if (newValue < min) {
        newValue = min;
    } else if (newValue > max) {
        newValue = max;
    }

    currentValue = Math.round(newValue / step) * step;
    currentPercent = ((currentValue - min) / (max - min)) * 100;

    document.querySelector('.range').style.width = `${currentPercent}%`;
    document.querySelectorAll('.handle').forEach((handle) => {
        handle.style.left = `${currentPercent}%`;
    });

    updateIntervals();
    updateCheckboxes();
}

function updateIntervals() {
    intervals.forEach((interval) => {
        const intervalValue = parseInt(interval.innerText);
        if (currentValue < intervalValue) {
            interval.classList.add('inactive');
        } else {
            interval.classList.remove('inactive');
        }
    });
}

function updateCheckboxes() {
    checkboxes.forEach((checkbox) => {
        const checkboxValue = parseInt(checkbox.value);
        if (checkboxValue <= currentValue) {
            checkbox.checked = true;
            checkbox.parentNode.style.color = '#2196F3';
        } else {
            checkbox.checked = false;
            checkbox.parentNode.style.color = 'initial';
        }
    });
}

updateIntervals();
//Filter's method '
document.querySelectorAll("#filterForm input").forEach(function (input) {
    input.addEventListener("change", function () {
        filterData();
    });
});

document.getElementById("filterForm").addEventListener("submit", function (event) {
    event.preventDefault();
    filterData();
});

function filterData() {
    var formData = new FormData(document.getElementById("filterForm"));

    fetch('/Cvpage/FilterData', {
        method: 'POST',
        body: formData
    })
        .then(response => response.text())
        .then(result => {
            document.getElementById("userBlocks").innerHTML = result;
        })
        .catch(error => console.log(error));
}

function resetForm() {
    document.getElementById("filterForm").reset();
    filterData();
}





//sort method
const sortSelect = document.getElementById('sort2');

sortSelect.addEventListener('change', () => {
    const selectedSort = sortSelect.value;

    const formData = new FormData();
    formData.append('sort', selectedSort);

    fetch(`/CvPage/Sorts?sort=${selectedSort}`, {
        method: 'POST',
        body: formData
    })
        .then(response => response.text())
        .then(result => {
            document.getElementById("userBlocks").innerHTML = result;
        })
        .catch(error => console.log(error));
});

