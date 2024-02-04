const profileSpan = document.getElementById('Profile-span');
const nameProfile = document.getElementById('Name-profile');
const descriptionProfile = document.getElementById('Description-profile');
const emailEmail = document.getElementById('Email - email');
const EmailNewEmail = document.getElementById('EmailNew-email');
const EmailNewEmailSpan = document.getElementById('EmailNew-email-span');
const pswdCurrentPswd = document.getElementById('PswdCurrent-pswd');
const pswdCurrentPswdSpan = document.getElementById('PswdCurrent-pswd-span');
const pswdNewPswd = document.getElementById('PswdNew-pswd');
const pswdNewPswdSpan = document.getElementById('PswdNew-pswd-span');
const pswdNewConfPswd = document.getElementById('PswdNewConf-pswd');
const pswdNewConfPswdSpan = document.getElementById('PswdNewConf-pswd-span');
const textProfileFail = "No need to save profile data";
const textMethodFail = "Error in request method";
const textFail = "Unknow Error";
const textEmailUnknow = "Current email unknow";
const textNotEmpty = "Field not empty";
const textNotEquald = "Passwords do not match";

function profileFormValidOk() {
    //nameProfile.setAttribute("class", valClassOk);
    //nameProfile.innerHTML = "";
    //nameProfile.value = "";
    
    //descriptionProfile.setAttribute("class", valClassOk);
    //descriptionProfile.innerHTML = "";
    //descriptionProfile.value = "";

    profileSpan.setAttribute("class", valClassSpanOk);
    profileSpan.innerHTML = "span";
    profileSpan.value = "span";
}

function profileWindow() {
    const myModal = new bootstrap.Modal('#modalAppUserProfile', {
        keyboard: false,
        focus: true,
        backdrop: false
    });
    profileFormValidOk();
    myModal.show();
}

async function profileSaveBtn() {
    const profileUrl = '/AppUser/ChangeProfileCurUser';    
    const form = document.getElementById('profile-form');
    form.addEventListener('onsubmit', function (e) {
        e.preventDefault();
    })

    let email = document.getElementById('Email-profile').value;
    let name = document.getElementById('Name-profile').value;
    let description = document.getElementById('Description-profile').value;

    if (email) {
        let data = {
            email: email,
            name: name,
            description: description
        }
        let surl = domainAddress + profileUrl;
        let response = await fetch(surl, {
            body: JSON.stringify(data),
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
        });

        if (response.ok) {
            window.location.replace(domainAddress);
        }
        else {
            let statusCode = response.status;
            profileSpan.setAttribute("class", valClassSpanFail);
            if (statusCode == 475) {
                //profileSpan.setAttribute("class", valClassSpanFail);
                profileSpan.innerHTML = textProfileFail + "!!!";
            }
            else if (statusCode == 470) {
                //profileSpan.setAttribute("class", valClassSpanFail);
                profileSpan.innerHTML = textMethodFail + "!!!";
            }
            else {
                profileSpan.innerHTML = textFail + " " + statusCode.toString() + "!!!";
            };

        }
    }

}

function emailFormValidOk() {
    //nameProfile.setAttribute("class", valClassOk);
    //nameProfile.innerHTML = "";
    //nameProfile.value = "";

    EmailNewEmail.setAttribute("class", valClassOk);
    EmailNewEmail.innerHTML = "";
    EmailNewEmail.value = "";

    EmailNewEmailSpan.setAttribute("class", valClassSpanOk);
    EmailNewEmailSpan.innerHTML = "span";
    EmailNewEmailSpan.value = "span";
}

function emailWindow() {
    const myModal = new bootstrap.Modal('#modalAppUserEmail', {
        keyboard: false,
        focus: true,
        backdrop: false
    });

    emailFormValidOk();

    myModal.show();
}

async function emailSaveBtn() {
    const profileUrl = '/AppUser/ChangeEmailCurUser';
    const form = document.getElementById('email-form');
    form.addEventListener('onsubmit', function (e) {
        e.preventDefault();
    })

    let email = document.getElementById('Email-email').value;
    let emailNew = document.getElementById('EmailNew-email').value;

    if (email && emailNew) {

        if (isEmailValid(emailNew)) {
            EmailNewEmailSpan.setAttribute("class", valClassSpanOk);
            EmailNewEmailSpan.innerHTML = "span";
            let data = {
                email: email,
                emailnew: emailNew
            }
            let surl = domainAddress + profileUrl;
            let response = await fetch(surl, {
                body: JSON.stringify(data),
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (response.ok) {
                window.location.replace(domainAddress);
            }
            else {
                let statusCode = response.status;
                EmailNewEmailSpan.setAttribute("class", valClassSpanFail);
                if (statusCode == 470) {
                    //profileSpan.setAttribute("class", valClassSpanFail);
                    EmailNewEmailSpan.innerHTML = textMethodFail + "!!!";
                }
                else if (statusCode == 475) {
                    EmailNewEmailSpan.innerHTML = textProfileFail + "!!!";
                }
                else {
                    EmailNewEmailSpan.innerHTML = textFail + " " + statusCode.toString() + "!!!";
                };
            }
        }
        else {
            //error newemail
            EmailNewEmailSpan.setAttribute("class", valClassSpanFail);
            EmailNewEmailSpan.innerHTML = textEmailNoValid + "!!!";
        }       
    }
    else {
        if (!email) {
            alert(textEmailUnknow);
        }
        //error curemail
        if (!emailNew) {
            EmailNewEmailSpan.setAttribute("class", valClassSpanFail);
            EmailNewEmailSpan.innerHTML = textEmailEmpty + "!!!";
        }
    }

}

function pswdFormValidOk() {
    pswdCurrentPswd.setAttribute("class", valClassOk);
    pswdCurrentPswd.innerHTML = "";
    pswdCurrentPswd.value = "";
    pswdCurrentPswdSpan.setAttribute("class", valClassSpanOk);
    pswdCurrentPswdSpan.innerHTML = "span";
    pswdCurrentPswdSpan.value = "span";

    pswdNewPswd.setAttribute("class", valClassOk);
    pswdNewPswd.innerHTML = "";
    pswdNewPswd.value = "";
    pswdNewPswdSpan.setAttribute("class", valClassSpanOk);
    pswdNewPswdSpan.innerHTML = "span";
    pswdNewPswdSpan.value = "span";

    pswdNewConfPswd.setAttribute("class", valClassOk);
    pswdNewConfPswd.innerHTML = "";
    pswdNewConfPswd.value = "";
    pswdNewConfPswdSpan.setAttribute("class", valClassSpanOk);
    pswdNewConfPswdSpan.innerHTML = "span";
    pswdNewConfPswdSpan.value = "span";
}

function pswdWindow() {
    const myModal = new bootstrap.Modal('#modalAppUserPswd', {
        keyboard: false,
        focus: true,
        backdrop: false
    });

    pswdFormValidOk();

    myModal.show();
}

async function pswdSaveBtn() {
    const profileUrl = '/AppUser/ChangePswdCurUser';
    let form = document.getElementById('pswd-form');
    form.addEventListener('onsubmit', function (e) {
        e.preventDefault();
    })

    let pswdCur = pswdCurrentPswd.value;
    let pswdNew = pswdNewPswd.value;
    let pswdNewConf = pswdNewConfPswd.value;

    if (!pswdCur) {
        pswdCurrentPswd.setAttribute("class", valClassFail);
        pswdCurrentPswdSpan.setAttribute("class", valClassSpanFail);
        pswdCurrentPswdSpan.innerText = textNotEmpty + "!!!";
    }
    else {
        pswdCurrentPswd.setAttribute("class", valClassOk);
        pswdCurrentPswdSpan.setAttribute("class", valClassSpanOk);
        pswdCurrentPswdSpan.innerText = "span";
    }
    if (!pswdNew) {
        pswdNewPswd.setAttribute("class", valClassFail);
        pswdNewPswdSpan.setAttribute("class", valClassSpanFail);
        pswdNewPswdSpan.innerText = textNotEmpty + "!!!";
    }
    else {
        pswdNewPswd.setAttribute("class", valClassOk);
        pswdNewPswdSpan.setAttribute("class", valClassSpanOk);
        pswdNewPswdSpan.innerText = "span";
    }
    if (!pswdNewConf) {
        pswdNewConfPswd.setAttribute("class", valClassFail);
        pswdNewConfPswdSpan.setAttribute("class", valClassSpanFail);
        pswdNewConfPswdSpan.innerText = textNotEmpty + "!!!";
    }
    else {
        pswdNewConfPswd.setAttribute("class", valClassOk);
        pswdNewConfPswdSpan.setAttribute("class", valClassSpanOk);
        pswdNewConfPswdSpan.innerText = "span";
    }

    if (pswdCur && pswdNew && pswdNewConf) {

        if (pswdNew == pswdNewConf) {

            pswdNewPswd.setAttribute("class", valClassOk);
            pswdNewPswdSpan.setAttribute("class", valClassSpanOk);
            pswdNewPswdSpan.innerText = "span";
            pswdNewConfPswd.setAttribute("class", valClassOk);
            pswdNewConfPswdSpan.setAttribute("class", valClassSpanOk);
            pswdNewConfPswdSpan.innerText = "span";
            
            let data = {
                pswd: pswdCur,
                pswdnew: pswdNew,
                pswdnewconf: pswdNewConf
            }
            let surl = domainAddress + profileUrl;
            let response = await fetch(surl, {
                body: JSON.stringify(data),
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (response.ok) {
                window.location.replace(domainAddress);
            }
            else {
                let statusCode = response.status;
                pswdCurrentPswdSpan.setAttribute("class", valClassSpanFail);
                pswdNewPswdSpan.setAttribute("class", valClassSpanFail);
                pswdNewConfPswdSpan.setAttribute("class", valClassSpanFail);
                if (statusCode == 470) {
                    pswdCurrentPswdSpan.innerHTML = textMethodFail + "!!!";
                    pswdNewPswdSpan.innerHTML = textMethodFail + "!!!";
                    pswdNewConfPswdSpan.innerHTML = textMethodFail + "!!!";
                }
                else if (statusCode == 475) {
                    pswdCurrentPswdSpan.innerHTML = textProfileFail + "!!!";
                    pswdNewPswdSpan.innerHTML = textProfileFail + "!!!";
                    pswdNewConfPswdSpan.innerHTML = textProfileFail + "!!!";
                     
                }
                else if (statusCode == 476) {
                    pswdCurrentPswdSpan.innerHTML = "Failed password" + "!!!";
                    //pswdNewPswdSpan.innerHTML = textProfileFail + "!!!";
                    //pswdNewConfPswdSpan.innerHTML = textProfileFail + "!!!";

                }
                else {
                    pswdCurrentPswdSpan.innerHTML = textFail + " " + statusCode.toString() + "!!!";
                    pswdNewPswdSpan.innerHTML = textFail + " " + statusCode.toString() + "!!!";
                    pswdNewConfPswdSpan.innerHTML = textFail + " " + statusCode.toString() + "!!!";
                };
            }
        }
        else {
            pswdNewPswd.setAttribute("class", valClassFail);
            pswdNewPswdSpan.setAttribute("class", valClassSpanFail);
            pswdNewPswdSpan.innerText = textNotEquald + "!!!";
            pswdNewConfPswd.setAttribute("class", valClassFail);
            pswdNewConfPswdSpan.setAttribute("class", valClassSpanFail);
            pswdNewConfPswdSpan.innerText = textNotEquald + "!!!";
        }
    }
    
}