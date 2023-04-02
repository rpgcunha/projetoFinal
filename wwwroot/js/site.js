var navbar = document.getElementById("myNavbar");
var links = navbar.getElementsByTagName("a");

for (var i = 0; i < links.length; i++) {
    if (links[i].href == document.URL && links[i] != "http://localhost:5268/") {
        links[i].className = "linkAtivo";
    }
}

const passwordSpans = document.querySelectorAll('.password');

passwordSpans.forEach(function (passwordSpan) {
    const passwordText = passwordSpan.getAttribute('data-password');
    passwordSpan.addEventListener('mouseover', function () {
        passwordSpan.textContent = passwordText;
    });
    passwordSpan.addEventListener('mouseout', function () {
        passwordSpan.textContent = '********';
    });
});



// Salva a posição do scroll no localStorage com o identificador exclusivo
localStorage.setItem('scrollPosition_processo', window.pageYOffset);

// Obtém a posição do scroll armazenada no localStorage com o identificador exclusivo
var scrollPosition = localStorage.getItem('scrollPosition_processo');

// Restaura a posição do scroll se ela foi armazenada
if (scrollPosition !== null) {
    window.scrollTo(0, scrollPosition);
}
