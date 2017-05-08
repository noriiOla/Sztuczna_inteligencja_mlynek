Gra = function () {

    var numerGraczaKtoryMaSieRuszyc = 1;
    var selectedPionek = null;

    var wielkoscPionka = 30;

    this.rozdajPionki = function () {
        rozdajBiale();
        rozdajCzarne();
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
                zmienWszystkieBialePionkiNaBiale();
                this.src = "/Resources/Images/bPionekK.png";
                selectedPionek = this;
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
                zmienWszystkieBialePionkiNaCzarne();
                this.src = "/Resources/Images/cPionekK.png";
                selectedPionek = this;
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
            console.log(mousePosX + " " + mousePosY);
            var posPola = $('#plansza').offset();
            var posOdLewej = posPola.left + wielkoscPionka / 2;
            var posOdGory = posPola.top + wielkoscPionka / 2;
            if (numerGraczaKtoryMaSieRuszyc != 1) {
                $('#plansza_div').prepend('<img id="cPionek" class="cPionekClass" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="c_pionek" src="/Resources/Images/cPionek.png" style="z-index:1; position:absolute; top:' + (mousePosY - posOdGory) + 'px; left:' + (mousePosX - posOdLewej) + 'px"/>');
                numerGraczaKtoryMaSieRuszyc = numerGraczaKtoryMaSieRuszyc - 1;
            } else {
                $('#plansza_div').prepend('<img id="bPionek" class="bPionekClass" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="b_pionek" src="/Resources/Images/bPionek.png" style="z-index:1; position:absolute; top:' + (mousePosY - posOdGory) + 'px; left:' + (mousePosX - posOdLewej) + 'px"/>');
                numerGraczaKtoryMaSieRuszyc = numerGraczaKtoryMaSieRuszyc + 1;
            }
            selectedPionek.remove();
            selectedPionek = null;
        }
    }
}

var game = new Gra();

window.onload = function () {

    $('#start_button').click(function () {
        game.rozdajPionki();
    })

    $('#plansza').click(function (event) {
        // alert(event.pageX + " " + event.pageY);
         game.obsluzRuch(event.pageX, event.pageY);
    })
}
