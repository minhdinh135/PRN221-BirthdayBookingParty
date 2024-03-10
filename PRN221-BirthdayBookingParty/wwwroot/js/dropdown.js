//const selectBtn = document.querySelector(".select-btn"),
//    items = document.querySelectorAll(".item");

//selectBtn.addEventListener("click", () => {
//    selectBtn.classList.toggle("open");
//});

//items.forEach((item) => {
//    item.addEventListener("click", () => {
//        item.classList.toggle("checked");

//        let checked = document.querySelectorAll(".checked"),
//            btnText = document.querySelector(".btn-text");

//        if (checked && checked.length > 0) {
//            btnText.innerText = `${checked.length} Selected`;
//        } else {
//            btnText.innerText = "Select Language";
//        }
//    });
//});

const packageSelectBtn = document.getElementById("packageSelectBtn");
const packageItems = document.querySelectorAll("#packageList .item");

packageSelectBtn.addEventListener("click", () => {
    packageSelectBtn.classList.toggle("open");
});

packageItems.forEach((item) => {
    item.addEventListener("click", () => {
        item.classList.toggle("checked"); // Toggle the checked class on the checkbox

        const checkedPackages = document.querySelectorAll("#packageList .checked");
        const btnText = document.querySelector("#packageSelectBtn .btn-text");

        if (checkedPackages && checkedPackages.length > 0) {
            btnText.innerText = `${checkedPackages.length} Selected`;
        } else {
            btnText.innerText = "Select Package";
        }
    });
});

const serviceSelectBtn = document.getElementById("serviceSelectBtn");
const serviceItems = document.querySelectorAll("#serviceList .item");

serviceSelectBtn.addEventListener("click", () => {
    serviceSelectBtn.classList.toggle("open");
});

serviceItems.forEach((item) => {
    item.addEventListener("click", () => {
        item.classList.toggle("checked"); // Toggle the checked class on the checkbox

        const checkedServices = document.querySelectorAll("#serviceList .checked");
        const btnText = document.querySelector("#serviceSelectBtn .btn-text");

        if (checkedServices && checkedServices.length > 0) {
            btnText.innerText = `${checkedServices.length} Selected`;
        } else {
            btnText.innerText = "Select Service";
        }
    });
});


