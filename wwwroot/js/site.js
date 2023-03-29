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