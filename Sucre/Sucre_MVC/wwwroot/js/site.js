// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const domainAddress = window.location.origin;
const EMAIL_REGEXP = /^(([^<>()[\].,;:\s@"]+(\.[^<>()[\].,;:\s@"]+)*)|(".+"))@(([^<>()[\].,;:\s@"]+\.)+[^<>()[\].,;:\s@"]{2,})$/iu;
const emailInput = document.getElementById('Email-input');
const emailSpan = document.getElementById('Email-span');
const pswdInput = document.getElementById('Passw-input');
const pswdSpan = document.getElementById('Passw-span');
const valClassFail = "form-control border border-1 border-danger";
const valClassOk = "form-control border border-1 border-black";
const valClassSpanFail = "text-danger";
const valClassSpanOk = "text-danger visually-hidden";
const textEmailEmpty = "Email cannot be empty";
const textEmailNoValid = "Email is not valid";
const textPswdEmpty = "Password cannot be empty";



function logoutLayout() {
    const myModal = new bootstrap.Modal('#modalLogout', {
        keyboard: false,
        focus: true,
        backdrop: false
    });
    myModal.show();
}

async function logoutBtn() {
    const logoutUrl = '/AppUser/AppUserLogoutJs';
    let surl = domainAddress + logoutUrl;
    console.log(surl);

    var logoutResult = await fetch(surl,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
        }
    );

    console.log(logoutResult.text());

    if (logoutResult.ok) {
        window.location.replace(domainAddress);
    }
}

function loginFormValidOk() {
    emailInput.setAttribute("class", valClassOk);
    emailInput.innerHTML = "";
    emailInput.value = "";
    emailSpan.setAttribute("class", valClassSpanOk)
    pswdInput.setAttribute("class", valClassOk);
    pswdInput.innerHTML = "";
    pswdInput.value = "";
    pswdSpan.setAttribute("class", valClassSpanOk)
}

function loginLayout() {
    const myModal = new bootstrap.Modal('#modalLogin', {
        keyboard: false,
        focus: true,
        backdrop: false
    });

    loginFormValidOk();  

    myModal.show();
}

async function loginBtn() {    
    const loginUrl = '/AppUser/AppUserLoginJs';                               
    console.log("loginbtn");
    const form = document.getElementById('login-form');
    form.addEventListener('onsubmit', function (e) {
        e.preventDefault();
    })

    let email = document.getElementById('Email-input').value;
    console.log(email);
    let pswd = document.getElementById('Passw-input').value;
    console.log(pswd);
    if (email && pswd) {
       /* if (isEmailValid(email)) {*/
            let data = {
                email: email,
                password: pswd,
                returnUrl: ""
            };
            let surl = domainAddress + loginUrl;
            console.log(surl);

            let response = await fetch(surl, {
                body: JSON.stringify(data),
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
            });

            if (response.ok) {
                let json = response.json();
                console.log(response);
                window.location.replace(domainAddress);
            }
            else {
                let headerLoginJs = response.headers.get("Login-Js");
                let statusCode = response.status;
                console.log(statusCode + " " + response.statusText);
                console.log(headerLoginJs);
                validateFormLogin(statusCode, '', '', headerLoginJs);
                //alert('FAIL!!!');

            }
        //}
        //else {
        //    validateFormLogin(1, email, pswd, '');
        //}
    }
    else {
        validateFormLogin(0, email, pswd,'');
    }
    
}

function validateFormLogin(stCode, iemail, ipswd, hLoginJs) {

    if (stCode==0 && !iemail) {
        emailInput.setAttribute("class", valClassFail);
        emailSpan.setAttribute("class", valClassSpanFail);
        emailSpan.innerHTML = textEmailEmpty+"!!!";        
    }
    if ((stCode == 1 && iemail && ipswd) || (stCode == 0 && iemail)) {
        if (isEmailValid(iemail)) {
            emailInput.setAttribute("class", valClassOk);
            emailSpan.setAttribute("class", valClassSpanOk);
        }
        else {
            emailInput.setAttribute("class", valClassFail);
            emailSpan.setAttribute("class", valClassSpanFail);
            emailSpan.innerHTML = textEmailNoValid+"!!!";
        }
    }
    if (stCode == 0 && !ipswd) {
        pswdInput.setAttribute("class", valClassFail);
        pswdSpan.setAttribute("class", valClassSpanFail);
        pswdSpan.innerHTML = textPswdEmpty + "!!!";
    }
    if (stCode == 470) {
        alert(hLoginJs);
    }
    if (stCode == 471) {
        emailInput.setAttribute("class", valClassFail);
        emailSpan.setAttribute("class", valClassSpanFail);
        emailSpan.innerHTML = hLoginJs + "!!!";        
        pswdInput.setAttribute("class", valClassFail);
        pswdSpan.setAttribute("class", valClassSpanFail);
        pswdSpan.innerHTML = hLoginJs + "!!!";
    }
    if (stCode == 472) {
        emailInput.setAttribute("class", valClassFail);
        emailSpan.setAttribute("class", valClassSpanFail);
        emailSpan.innerHTML = hLoginJs + "!!!";
        pswdInput.setAttribute("class", valClassFail);
        pswdSpan.setAttribute("class", valClassSpanFail);
        pswdSpan.innerHTML = hLoginJs + "!!!";
    }
    if (stCode == 473) {
        emailInput.setAttribute("class", valClassFail);
        emailSpan.setAttribute("class", valClassSpanFail);
        emailSpan.innerHTML = hLoginJs + "!!!";
        pswdInput.setAttribute("class", valClassOk);
        pswdSpan.setAttribute("class", valClassSpanOk);
    }
    if (stCode == 474) {  
        emailInput.setAttribute("class", valClassOk);
        emailSpan.setAttribute("class", valClassSpanOk);
        pswdInput.setAttribute("class", valClassFail);
        pswdSpan.setAttribute("class", valClassSpanFail);
        pswdSpan.innerHTML = hLoginJs + "!!!";
    }
    //let emailInput = document.getElementById('Email-input');
    //if (statusCode == 470) {
    //    //emailInput.style.borderColor = "red";

    //    let valClass = "form-control border border-1 border-danger";
    //    emailInput.setAttribute("class", valClass);

    //    //emailInput.style.background = 'blue';
    //}
}

function isEmailValid(valEmail) {
    //let xtmp = EMAIL_REGEXP.test(valEmail);
    return EMAIL_REGEXP.test(valEmail);
}