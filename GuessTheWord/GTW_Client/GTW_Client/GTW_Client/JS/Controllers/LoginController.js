app.controller("LoginController", [
    "$scope", "postJSON", "$location",
    /*Ako hocemo da koristimo neki servis, moramo ga ovde navesti*/
    function ($scope, postJSON, $location) {
        
        /*Model: promenjiva user koja ima polja Username i Password. 
            Povezali smo je sa html-om pomocu ng-model */
        $scope.user = {
            Username: "",
            Password: ""
        };


        /*Funkcija koja se poziva u htmlu na ng-click*/
        $scope.sendUser = function () {
            //ispis poruke u konzoli
            console.log("Pritisnuto LOGIN dugme\n");

            //provera da li je polje username popunjeno
            if ($scope.user.Username.length == 0 || $scope.user.Password.length == 0) {
                $window.alert("Morate popuniti sva polja!");
                return;
            }

            /*Poziva se servis. U servisu imam funkciju postJSON. Svaki put kada hocemo nesto da posaljemo na server treba da koristimo taj servis.
                Servisu prosledjujemo lokaciju na serveru (sve lokacije su u fajlu rutiranje.txt)
                Drugi argument je ono sto saljemo*/
            postJSON.postJSON("http://localhost:43474/GTW/Users/Login", $scope.user).then(function (result) {
                console.log(result.data);


                if (result.data != {}) {
                    console.log("Uspesno ulogovan korisnik");

                    /*TO DO: Ova putanja ce se promeniti kada napravimo stranicu sa skorovima i aktivnim sobama.
                        Takodje, na kraju treba da obrisemo sve console.log naredbe*/
                    $location.path("/gameRoom");
                    
                } else {
                    console.log("Korisnik nije ulogovan!");

                }
            })
        };
    }
]);