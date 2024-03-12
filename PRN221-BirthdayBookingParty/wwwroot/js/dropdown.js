const serviceSelectBtn = document.getElementById("serviceSelectBtn");
const serviceItems = document.querySelectorAll("#serviceList .item");

serviceSelectBtn.addEventListener("click", () => {
    serviceSelectBtn.classList.toggle("open");
});

serviceItems.forEach((item) => {
    item.addEventListener("click", () => {
        item.classList.toggle("checked");

        const checkedServices = document.querySelectorAll("#serviceList .checked");
        const btnText = document.querySelector("#serviceSelectBtn .btn-text");

        if (checkedServices && checkedServices.length > 0) {
            btnText.innerText = `${checkedServices.length} Selected`;
        } else {
            btnText.innerText = "Select Service";
        }
    });
});