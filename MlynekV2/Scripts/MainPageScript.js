Gra = function () {
    var url = localStorage.getItem("api-inicjuj-url");
    var urlObsluzRuch = localStorage.getItem("api-obsluzRuch-url");

    var numerGraczaKtoryMaSieRuszyc = 1;
    var selectedPionek = null;
    var wielkoscPionka = 30;
    var gracz1 = 0;
    var gracz2 = 0;
    var iloscRozdanychPionkow = 0;

    this.rozdajPionki = function () {
        rozdajBiale();
        rozdajCzarne();
        send();
    }

    function send() {
        return $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: url,
            dataType: 'json',
            data: {
                gracz1: gracz1,
                gracz2: gracz2
            },
            success: function (msg) {
                console.log(msg);
            }
        })
    }

    function rozdajBiale() {
        console.log("dodaje biale");
        var posPola = $('#prawePole').position();
        var posOdLewej = posPola.left + 35;
        var posOdGory = posPola.top + 10;
        var imgHeight = $('#prawePole').height();
        var bottomPos = imgHeight + posOdGory - wielkoscPionka;
        for (i = 0; i < 9; i++) {
            left: ' + posOdLewej + '; top: ' + posOdGory + ';
            $('#prawy_div').prepend('<img id="bPionek" class="bPionekClass" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="b_pionek" src="/Resources/Images/bPionek.png" style="z-index:1; position:absolute; top:' + posOdGory + 'px; left:' + posOdLewej + 'px"/>')
            posOdGory = posOdGory + 35;
            if (posOdGory >= (bottomPos)) {
                posOdGory = posPola.top + 10;
                posOdLewej = posOdLewej + 35;
            }
        }

        $('body').on('click', '.bPionekClass', function () {
            if (numerGraczaKtoryMaSieRuszyc == 1) {
                var posPola = $('#plansza').offset();
                var posPionka = $(this).offset();
                console.log(posPionka.left);
                if (iloscRozdanychPionkow < 18 && posPionka.left < (posPola.left + $('#plansza').height())) {
                    $("#infoLabel").html("First drop to panel");
                } else {
                    iloscRozdanychPionkow = iloscRozdanychPionkow + 1;
                    zmienWszystkieBialePionkiNaBiale();
                    this.src = "/Resources/Images/bPionekK.png";
                    selectedPionek = this;
                }
            }
        });
    }

    function rozdajCzarne() {
        console.log("dodaje czarne");
        var posPola = $('#lewePole').position();
        var posOdLewej = posPola.left + 20;
        var posOdGory = posPola.top + 10;
        var imgHeight = $('#lewePole').height();
        var bottomPos = imgHeight + posOdGory - wielkoscPionka;
        for (i = 0; i < 9; i++) {
            $('#lewy_div').prepend('<img id="cPionek" class="cPionekClass" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="c_pionek" src="/Resources/Images/cPionek.png" style="z-index:1; position:absolute; top:' + posOdGory + 'px; left:' + posOdLewej + 'px"/>')
            posOdGory = posOdGory + 35;
            if (posOdGory >= (bottomPos)) {
                posOdGory = posPola.top + 10;
                posOdLewej = posOdLewej + 35;
            }
        }

        $('body').on('click', '.cPionekClass', function () {
            if (numerGraczaKtoryMaSieRuszyc == 2) {

            var posPola = $('#plansza').offset();
            var posPionka = $(this).offset();
            if (iloscRozdanychPionkow < 18 && posPionka.left > posPola.left) {
                $('#infoLabel').val("First drop stones from panel");
            } else {
                iloscRozdanychPionkow = iloscRozdanychPionkow + 1;
                zmienWszystkieBialePionkiNaCzarne();
                this.src = "/Resources/Images/cPionekK.png";
                selectedPionek = this;
            }
            }
        });
    }

    function zmienWszystkieBialePionkiNaBiale() {
        $(".bPionekClass").each(function () {
            this.src = '/Resources/Images/bPionek.png';
        })
    }

    function zmienWszystkieBialePionkiNaCzarne() {
        $(".cPionekClass").each(function () {
            this.src = '/Resources/Images/cPionek.png';
        })
    }

    this.obsluzRuch = function (mousePosX, mousePosY) {
        if (selectedPionek != null) {
            $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
             url: urlObsluzRuch,
            dataType: 'json',
                 data: {
                  x: mousePosX,
                  y: mousePosY,
                 kolorGracza: numerGraczaKtoryMaSieRuszyc
                    },
                    success: function (msg) {
                        console.log(msg);         
                        ruch(msg);
                    },
                    error: function (req, status, err) {
                        console.log('Something went wrong', status, err);
                    }
             })
    }
 }
    

    function ruch(punkt) {
        if(punkt.x != 0){
        var posPola = $('#plansza').offset();
        var posOdLewej = posPola.left;
        var posOdGory = posPola.top;
        if (numerGraczaKtoryMaSieRuszyc != 1) {
            $('#plansza_div').prepend('<img id="cPionek" class="cPionekClass" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="c_pionek" src="/Resources/Images/cPionek.png" style="z-index:1; position:absolute; top:' + (punkt.y - posOdGory - 12) + 'px; left:' + (punkt.x - posOdLewej - 12) + 'px"/>');
            numerGraczaKtoryMaSieRuszyc = numerGraczaKtoryMaSieRuszyc - 1;
            $('#infoLabel').html("Biale");
        } else {
            $('#plansza_div').prepend('<img id="bPionek" class="bPionekClass" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="b_pionek" src="/Resources/Images/bPionek.png" style="z-index:1; position:absolute; top:' + (punkt.y - posOdGory - 12) + 'px; left:' + (punkt.x - posOdLewej - 12) + 'px"/>');
            numerGraczaKtoryMaSieRuszyc = numerGraczaKtoryMaSieRuszyc + 1;
            $('#infoLabel').html("Czarne");
        }
        selectedPionek.remove();
        selectedPionek = null;
        }
    }
}

var game = new Gra();

window.onload = function () {
    console.log("DWA");
    $('#start_button').click(function () {
        game.rozdajPionki();
        $('#infoLabel').html("Biale");
    })

    $('#plansza').click(function (event) {
         console.log(event.pageX + " " + event.pageY);
         game.obsluzRuch(event.pageX, event.pageY);
    })
}
