function clearSearch(inputField) {
    if (inputField.value == inputField.defaultValue) {
        inputField.value = "";
        inputField.style.color = "black";
    }
}

function submitSearch(inputField) {
    if (inputField.value != "") {
        inputField.form.submit();
    }
    else {
        inputField.value = inputField.defaultValue;
        inputField.style.color = "silver";
    }
}
