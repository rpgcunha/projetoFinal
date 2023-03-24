var navbar = document.getElementById("myNavbar");
var links = navbar.getElementsByTagName("a");

for (var i = 0; i < links.length; i++) {
    if (links[i].href == document.URL && links[i] != "http://localhost:5268/") {
        links[i].className = "linkAtivo";
    }
}
