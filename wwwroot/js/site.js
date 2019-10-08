// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

src="https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js"

function OnKeyUp(event, value) {

    var KeyID = event.keyCode;
    switch (KeyID) {
        case 13:
            //enter clears lists
            $("#ListDiv").find("ul[class='wordList']").empty();
            break;
        case 32:
            //space clears lists
            $("#ListDiv").find("ul[class='wordList']").empty();
            break;

        default:
            var snippet = value.substring(value.lastIndexOf(' ') + 1, value.length)

            //Get snippet matches from database and webservice, and write them to lists
            GetMatches(snippet);
            break;
    }

}

function GetMatches(snippet) {
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            var data = JSON.parse(this.responseText);

            for (var dataSource in data) {

                var output = "";

                for (var word in data[dataSource]) {
                    output += "<li>" + data[dataSource][word] + "</li>";
                }

                document.getElementById(dataSource).innerHTML = output;

            }
        }
    };

    xhttp.open("GET", "/api/" + snippet, true);
    xhttp.send();
}