Gra = function () {
    var url = localStorage.getItem("api-inicjuj-url");
    var urlObsluzRuch = localStorage.getItem("api-obsluzRuch-url");
    var urlUsunPionek = localStorage.getItem("api-usunPionek-url");
    var znajdzRuchyUrl = localStorage.getItem("api-znajdzRuchy-url");
    var znajdzNajlepszyRuchUrl = localStorage.getItem("api-znajdzNajlRuch-url");

    var numerGraczaKtoryMaSieRuszyc = 1;
    var selectedPionek = null;
    var wielkoscPionka = 30;
    var gracz1 = 0;
    var gracz2 = 0;
    var iloscRozdanychPionkow = 0;
    var mlynek = false;
    var iloscZabranychBialych = 0;
    var iloscZabranychCzarnych = 0;
    var poczatekGry = 0;

    this.rozdajPionki = function () {
        rozdajBiale();
        rozdajCzarne();
    }

    this.send = function () {
        return $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: url,
            dataType: 'json',
            data: {
                gracz1: gracz1,
                gracz2: gracz2,
                typBialych: $('#graczBialy').find(":selected").text(),
                typCzarnych: $('#graczCzarny').find(":selected").text(),
                poziom: $('#poziom').find(":selected").text(),
                kolejnoscWezlow: $('#kolejnoscW').find(":selected").text()
            },
            success: function (msg) {
                console.log("koneic inincjacji");
            }
        })
    }

    function usunPionek(x, y) {
        return $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: urlUsunPionek,
            dataType: 'json',
            data: {
                x: x,
                y: y
            },
            success: function (msg) {
            }
        })
    }

    function rozdajBiale() {
        var posPola = $('#prawePole').position();
        var posOdLewej = posPola.left + 35;
        var posOdGory = posPola.top + 10;
        var imgHeight = $('#prawePole').height();
        var bottomPos = imgHeight + posOdGory - wielkoscPionka;
        for (i = 0; i < 9; i++) {
            left: ' + posOdLewej + '; top: ' + posOdGory + ';
            $('#prawy_div').prepend('<img id="bPionek" class="bPionekClass" name="b'+ i +'" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="b_pionek" src="/Resources/Images/bPionek.png" style="z-index:1; position:absolute; top:' + posOdGory + 'px; left:' + posOdLewej + 'px"/>')
            posOdGory = posOdGory + 35;
            if (posOdGory >= (bottomPos)) {
                posOdGory = posPola.top + 10;
                posOdLewej = posOdLewej + 35;
            }
        }

        $('body').on('click', '.bPionekClass', function () {
            console.log(numerGraczaKtoryMaSieRuszyc);
            if (numerGraczaKtoryMaSieRuszyc == 2 && mlynek) {
                mlynek = false;
                numerGraczaKtoryMaSieRuszyc = numerGraczaKtoryMaSieRuszyc - 1;
                var xPionkaDoUsuniecia = $(this).offset().left + wielkoscPionka / 2;
                var yPionkaDoUsuniecia = $(this).offset().top + wielkoscPionka / 2;
                usunPionek(xPionkaDoUsuniecia, yPionkaDoUsuniecia);
                $(this).remove();
                iloscZabranychBialych = iloscZabranychBialych - 1;
                $("#infoLabel").html("Biale");
                if ($('#graczBialy').find(":selected").text() !== 'Czlowiek') {                         ////mlynek
                    console.log("szukam miejsca dla bialyn")
                    game.kompZnajdzNajlepszyRucha("1", false);
                }

            } else {
                if (numerGraczaKtoryMaSieRuszyc == 1) {
                    var posPola = $('#plansza').offset();
                    var posPionka = $(this).offset();
                    if (iloscRozdanychPionkow < 18 && posPionka.left < (posPola.left + $('#plansza').height())) {
                        $("#infoLabel").html("First drop to panel");
                    } else {
                        iloscRozdanychPionkow = iloscRozdanychPionkow + 1;
                        zmienWszystkieBialePionkiNaBiale();
                        this.src = "/Resources/Images/bPionekK.png";
                        selectedPionek = this;
                    }
                }
            }
        });
    }

    function rozdajCzarne() {
        var posPola = $('#lewePole').position();
        var posOdLewej = posPola.left + 20;
        var posOdGory = posPola.top + 10;
        var imgHeight = $('#lewePole').height();
        var bottomPos = imgHeight + posOdGory - wielkoscPionka;
        for (i = 0; i < 9; i++) {
            $('#lewy_div').prepend('<img id="cPionek" class="cPionekClass" name="c' + i + '" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="c_pionek" src="/Resources/Images/cPionek.png" style="z-index:1; position:absolute; top:' + posOdGory + 'px; left:' + posOdLewej + 'px"/>')
            posOdGory = posOdGory + 35;
            if (posOdGory >= (bottomPos)) {
                posOdGory = posPola.top + 10;
                posOdLewej = posOdLewej + 35;
            }
        }

        $('body').on('click', '.cPionekClass', function () {
            console.log(numerGraczaKtoryMaSieRuszyc);
            if (numerGraczaKtoryMaSieRuszyc == 1 && mlynek) {
                mlynek = false;
                numerGraczaKtoryMaSieRuszyc = numerGraczaKtoryMaSieRuszyc + 1;
                var xPionkaDoUsuniecia = $(this).offset().left + wielkoscPionka / 2;
                var yPionkaDoUsuniecia = $(this).offset().top + wielkoscPionka / 2;
                usunPionek(xPionkaDoUsuniecia, yPionkaDoUsuniecia);
                $(this).remove();
                iloscZabranychCzarnych = iloscZabranychCzarnych - 1;
                $("#infoLabel").html("Czarne");
                if ($('#graczCzarny').find(":selected").text() !== 'Czlowiek') {                         ////mlynek
                    console.log("szukam miejsca dla czaarnych")
                    game.kompZnajdzNajlepszyRucha("2", false);
                }
            } else {
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
                    xNowe: mousePosX,
                    yNowe: mousePosY,
                    kolorGracza: numerGraczaKtoryMaSieRuszyc,
                    xStare: $(selectedPionek).offset().left + wielkoscPionka/2,
                    yStare: $(selectedPionek).offset().top + wielkoscPionka / 2,
                    nazwaPionka: $(selectedPionek).attr('name')                                //dodana nazwa pionka
                },
                success: function (msg) {       
                    ruch(msg, $(selectedPionek).attr('name'));          //wyslana  nazwa pionka
                },
                error: function (req, status, err) {
                    console.log('Something went wrong', status, err);
                }
            })
        }
    }

    this.znajdzRuchy = function (kolor) {
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: znajdzRuchyUrl,
            dataType: 'json',
            data: {
                kolor: kolor
            },
            success: function (msg) {
            },
            error: function (req, status, err) {
                console.log('Something went wrong', status, err);
            }
        })
    }

    function ruch(wynikRuchu, nazwaPionka) {
        if(wynikRuchu.punkt.x != 0){
        var posPola = $('#plansza').offset();
        var posOdLewej = posPola.left;
        var posOdGory = posPola.top;
        $("[name='" + nazwaPionka + "']").remove();
        selectedPionek = null;
        console.log("ruch");
        console.log(wynikRuchu);
        if (numerGraczaKtoryMaSieRuszyc != 1) {
            iloscRozdanychPionkow = iloscRozdanychPionkow + 1;
            $('#plansza_div').prepend('<img id="cPionek" name="' + nazwaPionka + '" class="cPionekClass" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="c_pionek" src="/Resources/Images/cPionek.png" style="z-index:1; position:absolute; top:' + (wynikRuchu.punkt.y - posOdGory - 12) + 'px; left:' + (wynikRuchu.punkt.x - posOdLewej - 12) + 'px"/>');
            if (wynikRuchu.czyJestMlynek) {
                $('#infoLabel').html("Mlynek czarnych, kliknij na pionek przeciwnika ktory chcesz zabrac");
                mlynek = true;
     
                //if ($('#graczCzarny').find(":selected").text() !== 'Czlowiek') {
                //    console.log("szukam miejsca dla czaarnych")
                //    game.kompZnajdzNajlepszyRucha("2", true);
                //}
            } else {
                numerGraczaKtoryMaSieRuszyc = numerGraczaKtoryMaSieRuszyc - 1;
                $('#infoLabel').html("Biale");

                if ($('#graczBialy').find(":selected").text() !== 'Czlowiek') {
                    game.kompZnajdzNajlepszyRucha("1", false);
                }
            }
        } else {
            iloscRozdanychPionkow = iloscRozdanychPionkow + 1;
            $('#plansza_div').prepend('<img id="bPionek" name="' + nazwaPionka + '" class="bPionekClass" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="b_pionek" src="/Resources/Images/bPionek.png" style="z-index:1; position:absolute; top:' + (wynikRuchu.punkt.y - posOdGory - 12) + 'px; left:' + (wynikRuchu.punkt.x - posOdLewej - 12) + 'px"/>');
            if (wynikRuchu.czyJestMlynek) {
                $('#infoLabel').html("Mlynek bialych, kliknij na pionek przeciwnika ktory chcesz zabrac");
                mlynek = true;

                //if ($('#graczBialy').find(":selected").text() !== 'Czlowiek') {
                //    game.kompZnajdzNajlepszyRucha("1", true);
                //}
            } else {
                numerGraczaKtoryMaSieRuszyc = numerGraczaKtoryMaSieRuszyc + 1;
                $('#infoLabel').html("Czarne");
  
                if ($('#graczCzarny').find(":selected").text() !== 'Czlowiek') {
                    game.kompZnajdzNajlepszyRucha("2", false);
                }
            }
        }
        }
    }

    this.kompZnajdzNajlepszyRucha = function (kolor, jestMlynek) {
        console.log("Ajas");
        $.ajax({
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            url: znajdzNajlepszyRuchUrl,
            dataType: 'json',
            data: {
                kolor: kolor,
                jestMlynek: jestMlynek
            },
            success: function (msg) {
                obsluzRuchKomputera(msg);               
            },
            error: function (req, status, err) {
                console.log('Something went wrong', status, err);
            }
        })
    }

    function obsluzRuchKomputera(msg) {
        console.log(msg);
        var posPola = $('#plansza').offset();
        var posOdLewej = posPola.left;
        var posOdGory = posPola.top;
       
        if (msg.miejscePionkaDoPostawienia == null && msg.miejscePionkaDoUsuniecia == null) {
            if (numerGraczaKtoryMaSieRuszyc != 1) {
                $('#infoLabel').html("Czarne nie maja sie gdzie ruszyc, wygraly biale! BRAWO!");
                console.log("Czas gry:");
                console.log(new Date().getTime() - poczatekGry);
            } else {
                $('#infoLabel').html("Biale nie maja sie gdzie ruszyc, wygraly czarne! BRAWO!");
                console.log("Czas gry:");
                console.log(new Date().getTime() - poczatekGry);
            }
        }
        console.log(numerGraczaKtoryMaSieRuszyc);
        if (numerGraczaKtoryMaSieRuszyc != 1) {
           
            if ($('#graczCzarny').find(":selected").text() !== 'Czlowiek') {
                $("[name='" + msg.nazwaPionka + "']").remove();
           
                if (msg.stanGry !== "oblugujeMlynek") {
                    $('#plansza_div').prepend('<img id="cPionek" name="' + msg.nazwaPionka + '" class="cPionekClass" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="c_pionek" src="/Resources/Images/cPionek.png" style="z-index:1; position:absolute; top:' + (msg.miejscePionkaDoPostawienia.y - posOdGory - 12) + 'px; left:' + (msg.miejscePionkaDoPostawienia.x - posOdLewej - 12) + 'px"/>');
                }
                if (msg.jestMlynek) {
                    iloscZabranychCzarnych = iloscZabranychCzarnych + 1;
                    if (iloscZabranychCzarnych < 7) {
                        $('#infoLabel').html("Mlynek czarnych, kliknij na pionek przeciwnika ktory chcesz zabrac");
                        mlynek = true;

                       // if ($('#graczCzarny').find(":selected").text() !== 'Czlowiek') {
                            game.kompZnajdzNajlepszyRucha("2", true);
                      //  }
                    } else {
                        $('#infoLabel').html("Wygraly czarne! BRAWO!");
                        console.log("Czas gry:");
                        console.log(new Date().getTime() - poczatekGry);
                    }
                } else {
                    iloscRozdanychPionkow = iloscRozdanychPionkow + 1;
                    numerGraczaKtoryMaSieRuszyc = numerGraczaKtoryMaSieRuszyc - 1;
                    $('#infoLabel').html("Biale");

                    if ($('#graczBialy').find(":selected").text() !== 'Czlowiek') {
                        game.kompZnajdzNajlepszyRucha("1", false);
                    }
                }
            }
        } else {
            
            if ($('#graczBialy').find(":selected").text() !== 'Czlowiek') {
                $("[name='" + msg.nazwaPionka + "']").remove();
            if (msg.stanGry !== "oblugujeMlynek") {
                $('#plansza_div').prepend('<img id="bPionek" name="' + msg.nazwaPionka + '" class="bPionekClass" height="' + wielkoscPionka + '" width="' + wielkoscPionka + '" class="b_pionek" src="/Resources/Images/bPionek.png" style="z-index:1; position:absolute; top:' + (msg.miejscePionkaDoPostawienia.y - posOdGory - 12) + 'px; left:' + (msg.miejscePionkaDoPostawienia.x - posOdLewej - 12) + 'px"/>');
            }
            
            if (msg.jestMlynek) {
                iloscZabranychBialych = iloscZabranychBialych + 1;
                if (iloscZabranychBialych < 7) {
                    $('#infoLabel').html("Mlynek bialych, kliknij na pionek przeciwnika ktory chcesz zabrac");
                    mlynek = true;

                    //if ($('#graczBialy').find(":selected").text() !== 'Czlowiek') {
                        game.kompZnajdzNajlepszyRucha("1", true);
                   // }
                } else {
                    $('#infoLabel').html("Wygraly biale! BRAWO!");
                    console.log("Czas gry:");
                    console.log(new Date().getTime() - poczatekGry);
                }
            } else {
                numerGraczaKtoryMaSieRuszyc = numerGraczaKtoryMaSieRuszyc + 1;
                $('#infoLabel').html("Czarne");

                if ($('#graczCzarny').find(":selected").text() !== 'Czlowiek') {
                    game.kompZnajdzNajlepszyRucha("2", false);
                }
            }
        }
        }
    }
}

var game = new Gra();

window.onload = function () {
    $('#start_button').click(function () {
        game.rozdajPionki()
        var promise = $.when(game.send());

        promise.then(function () {
            $('#infoLabel').html("Biale");
            if ($('#graczBialy').find(":selected").text() !== 'Czlowiek') {
                game.poczatekGry = new Date().getTime();
                game.kompZnajdzNajlepszyRucha("1", false);
            }
        });
    })

    $('#plansza').click(function (event) {
         game.obsluzRuch(event.pageX, event.pageY);
    })

    $('#znajdzRuchyB').click(function (event) {
        game.znajdzRuchy("1");
    })

    $('#znajdzRuchyC').click(function (event) {
        game.znajdzRuchy("2");
    })
}
