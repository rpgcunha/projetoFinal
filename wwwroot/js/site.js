var navbar = document.getElementById("myNavbar");
var links = navbar.getElementsByTagName("a");

for (var i = 0; i < links.length; i++) {
    if (links[i].href == document.URL && links[i] != "http://localhost:5268/") {
        links[i].className = "linkAtivo";
    }
}

    var submitButton = document.getElementById("submitButton");
    submitButton.onclick = function () {
    // Armazena a posição atual do scroll
    var scrollTop = window.pageYOffset || document.documentElement.scrollTop;

    // Define o valor do campo hidden com a posição do scroll
    var scrollField = document.getElementById("scrollField");
    scrollField.value = scrollTop;

    // Submete o formulário
    document.forms[0].submit();

    // Define a posição do scroll para a posição armazenada após o envio
    window.scrollTo(0, scrollTop);

    // Retorna false para evitar o comportamento padrão de envio do formulário
    return false;
};