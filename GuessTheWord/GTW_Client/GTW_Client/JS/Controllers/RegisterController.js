/*Sve je slicno kao kod login kontrolera, osim sto se ovde pravi dodatna promenjiva user*/
app.controller("RegisterController", [
    "$scope", "postJSON", "$location", "$window",
    function ($scope, postJSON, $location, $window) {

        $scope.user = {
            Username: "",
            Password: "",
            RepeatedPwd: ""
        };


        $scope.sendNewUser = function () {
            console.log("Pritisnuto SUBMIT dugme");

            //provera da li je polje username popunjeno
            if ($scope.user.Username.length == 0) {
                $window.alert("Polje Username ne sme biti prazno!");
                $scope.user.Password = "";
                $scope.user.RepeatedPwd = "";
                return;
            }

            //provera da li je pwd duzi od 8 karaktera
            if ($scope.user.Password.length < 8) {
                $window.alert("Lozinka mora imati bar 8 karaktera!");
                $scope.user.Password = "";
                $scope.user.RepeatedPwd = "";
                return;
            }

            //provera da li je oba puta unet isti pwd
            if ($scope.user.Password != $scope.user.RepeatedPwd) {
                $window.alert("Greska prilikom unosa lozinke!");
                $scope.user.Password = "";
                $scope.user.RepeatedPwd = "";
                return;
            }


            /*Ovo pravimo posebnu promenjivu koju saljemo jer ona gore ima polje viska, a server ocekuje promenjivu sa samo 2 polja: Username i Password*/
            var user = {
                Username: $scope.user.Username,
                Password: $scope.user.Password
            };

            postJSON.postJSON("http://localhost:43474/GTW/Users/Register", user).then(function (result) {
                console.log(result.data);

                if (result.data != {}) {
                    console.log("Uspesno registrovan korisnik");

                    /*TO DO: Ovo treba promeniti da otvori profil tog korisnika gde su 
                    njegovi poeni i medalje (ako ih bude bilo)*/
                    $location.path("/landingPage");

                } else {/*TO DO: Zasto ovde ne ulazi? */
                    console.log("Korisnik nije registrovan!");

                }
            })
        };
    }
]);