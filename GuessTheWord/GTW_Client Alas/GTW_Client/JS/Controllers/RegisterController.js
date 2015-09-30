/*Sve je slicno kao kod login kontrolera, osim sto se ovde pravi dodatna promenjiva user*/
app.controller("RegisterController", ["$scope", "postJSON", "$location", "$window", "$cookies", "md5", function ($scope, postJSON, $location, $window, $cookies, md5) {
    $scope.user = {
        Username: "",
        Password: "",
        RepeatedPwd: ""
    };

    $scope.sendNewUser = function () {
        if ($scope.user.Username.length == 0) {
            $window.alert("Polje Username ne sme biti prazno!");
            $scope.user.Password = "";
            $scope.user.RepeatedPwd = "";
            return;
        }

        if ($scope.user.Password.length < 8) {
            $window.alert("Lozinka mora imati bar 8 karaktera!");
            $scope.user.Password = "";
            $scope.user.RepeatedPwd = "";
            return;
        }

        if ($scope.user.Password != $scope.user.RepeatedPwd) {
            $window.alert("Greska prilikom unosa lozinke!");
            $scope.user.Password = "";
            $scope.user.RepeatedPwd = "";
            return;
        }


        var user = {
            Username: $scope.user.Username,
            Password: md5.createHash($scope.user.Password || ''),
            Role: "User"
        };

        postJSON.postJSON("http://guesstheword-1.apphb.com/GTW/Users/Register", user).then(function (result) {

            if (result.data != {}) {
                $cookies.putObject("loggedUser", result.data);
                $location.path("landingPage");
            } else {/*TODO: Zasto ovde ne ulazi? */
                console.error("Korisnik nije registrovan!");
            }
        })
    };

    (function() {
        var temp =angular.fromJson($cookies.getObject('loggedUser'));
        if(  temp != undefined ) {
            $location.path("landingPage");
        }
    })();
}]);