document.addEventListener("DOMContentLoaded", function () {
    var currentRole = document.querySelector("#RoleSelect");
    var nameField = document.querySelector("#MyName");
    var errorField = document.querySelector("#nameError");
    var nameId = document.querySelector("#nameId");

    var phoneId = document.querySelector("#phoneId");
    var presentAddressId = document.querySelector("#presentAddressId");
    var parmanentAddressId = document.querySelector("#parmanentAddressId");
    var zipId = document.querySelector("#zipId");

    var establishId = document.querySelector("#establishId");
    var joingId = document.querySelector("#joingId");

    var Company_Employee = document.querySelector("#Company_Employee");
    Company_Employee.style.display = "none";
    var establish_div = document.querySelector("#establish_div");
    var joing_div = document.querySelector("#joing_div");
    establish_div.style.display = "none";
    joing_div.style.display = "none";

    var MyCompanyName = document.querySelector("#MyCompanyName");
    var comanyId = document.querySelector("#comanyId");
    nameField.style.display = "none";
    MyCompanyName.style.display = "none";

    currentRole.addEventListener("change", function () {
        var value = this.value;
        if (value === "Admin" || value === "Client" || value === "" || value === null) {
            MyCompanyName.style.display = "none";
            nameField.style.display = "none";
            comanyId.removeAttribute("required");
            nameId.removeAttribute("required");
            phoneId.removeAttribute("required");
            presentAddressId.removeAttribute("required");
            parmanentAddressId.removeAttribute("required");
            zipId.removeAttribute("required");
            Company_Employee.style.display = "none";
            establish_div.style.display = "none";
            joing_div.style.display = "none";
        } else if (value === "Company") {
            comanyId.setAttribute("required", "required");
            nameId.removeAttribute("required");
            Company_Employee.style.display = "block";
            MyCompanyName.style.display = "block";
            establish_div.style.display = "block";
            joing_div.style.display = "none";

            phoneId.setAttribute("required", "required");
            presentAddressId.setAttribute("required", "required");
            parmanentAddressId.setAttribute("required", "required");
            zipId.setAttribute("required", "required");
            joingId.removeAttribute("required");
            establishId.setAttribute("required", "required");
        } else if (value === "Employee") {
            nameId.setAttribute("required", "required");
            nameField.style.display = "block";
            MyCompanyName.style.display = "none";
            Company_Employee.style.display = "block";
            establish_div.style.display = "none";
            joing_div.style.display = "block";
            comanyId.removeAttribute("required");

            phoneId.setAttribute("required", "required");
            presentAddressId.setAttribute("required", "required");
            parmanentAddressId.setAttribute("required", "required");
            zipId.setAttribute("required", "required");
            establishId.removeAttribute("required");
            joingId.setAttribute("required", "required");
        }
    });

    // Validation on form submission
    const registerForm = document.querySelector("#registerForm");

    registerForm.addEventListener("submit", function (e) {
        let isValid = true; // Flag to track validation status
        const nameFieldValue = nameId.value.trim();
        const phoneIdValue = phoneId.value.trim();
        const presentAddressValue = presentAddressId.value.trim();
        const parmanentAddressValue = parmanentAddressId.value.trim();
        const zipValue = zipId.value.trim();
        const establishValue = establishId.value.trim();
        const joingValue = joingId.value.trim();
        const companyValue = comanyId.value.trim();

        // Check if nameId is required and validate
        if (comanyId.hasAttribute("required")) {
            if (companyValue.length <= 8) {
                e.preventDefault();
                document.querySelector("#companyError").textContent = "Company Name must be greater than 8 characters";
                document.querySelector("#companyError").style.display = "block";
                isValid = false;
            } else {
                document.querySelector("#companyError").style.display = "none";
            }
        }

        if (nameId.hasAttribute("required")) {
            if (nameFieldValue.length <= 5) {
                e.preventDefault();
                errorField.style.display = "block";
                errorField.textContent = "Name must be greater than 5 characters";
                isValid = false;
            } else {
                errorField.style.display = "none";
                errorField.textContent = "";
            }
        }

        // Check if phoneId is required and validate
        if (phoneId.hasAttribute("required")) {
            const regex = /^(017|018|019|016)\d{8}$/;
            if (!regex.test(phoneIdValue)) {
                e.preventDefault();
                document.querySelector("#errorPhone").textContent = "Phone number must be 11 character characters";
                document.querySelector("#errorPhone").style.display = "block";
                isValid = false;
            } else {
                document.querySelector("#errorPhone").style.display = "none";
            }
        }

        // Check if presentAddressId is required and validate
        if (presentAddressId.hasAttribute("required")) {
            if (presentAddressValue.length === 0) {
                e.preventDefault();
                document.querySelector("#errorPresentAddress").textContent = "Present Address is required";
                document.querySelector("#errorPresentAddress").style.display = "block";
                isValid = false;
            } else {
                document.querySelector("#errorPresentAddress").style.display = "none";
            }
        }

        // Check if parmanentAddressId is required and validate
        if (parmanentAddressId.hasAttribute("required")) {
            if (parmanentAddressValue.length === 0) {
                e.preventDefault();
                document.querySelector("#errorParmanetAddress").textContent = "Permanent Address is required";
                document.querySelector("#errorParmanetAddress").style.display = "block";
                isValid = false;
            } else {
                document.querySelector("#errorParmanetAddress").style.display = "none";
            }
        }

        // Check if zipId is required and validate
        if (zipId.hasAttribute("required")) {
            if (zipValue.length === 0) {
                e.preventDefault();
                document.querySelector("#errorZIP").textContent = "ZIP Code is required";
                document.querySelector("#errorZIP").style.display = "block";
                isValid = false;
            } else {
                document.querySelector("#errorZIP").style.display = "none";
            }
        }

        // Check if establishId is required and validate
        if (establishId.hasAttribute("required")) {
            if (establishValue.length === 0) {
                e.preventDefault();
                document.querySelector("#errorEstablishDate").textContent = "Establish Date is required";
                document.querySelector("#errorEstablishDate").style.display = "block";
                isValid = false;
            } else {
                document.querySelector("#errorEstablishDate").style.display = "none";
            }
        }

        // Check if joingId is required and validate
        if (joingId.hasAttribute("required")) {
            if (joingValue.length === 0) {
                e.preventDefault();
                document.querySelector("#errorJoingDate").textContent = "Joining Date is required";
                document.querySelector("#errorJoingDate").style.display = "block";
                isValid = false;
            } else {
                document.querySelector("#errorJoingDate").style.display = "none";
            }
        }

        if (!isValid) {
            e.preventDefault(); // Prevent form submission if validation fails
        }
    });
});
