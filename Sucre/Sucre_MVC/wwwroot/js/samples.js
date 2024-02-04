// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function logout() {
    const logoutConfirmMessage = "Are you sure?";
    var result = confirm(logoutConfirmMessage);
    console.log(result);

    if (result) {
        var cNod2 = document.getElementById('idscr2');
        cNod2.style.background = 'green';
    }
}

function loginPr() {
    //const myModal = new bootstrap.Modal(document.getElementById('modal1'));
    // or
    const myModal = new bootstrap.Modal('#modalLoginPr', {
        keyboard: false,
        focus: true,
        backdrop: false
    });
    //myModal.toggle();
    myModal.show()
}


