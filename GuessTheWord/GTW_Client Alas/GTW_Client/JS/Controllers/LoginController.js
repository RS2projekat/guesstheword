app.controller("LoginController", ["$scope", "postJSON", "$location", "$cookies", "md5", function ($scope, postJSON, $location, $cookies, md5) {
    $scope.user = {
        Username: "",
        Password: ""
    };

    $scope.sendUser = function () {
        if ($scope.user.Username.length == 0 || $scope.user.Password.length == 0) {
            $window.alert("Morate popuniti sva polja!");
            return;
        }

        var logUser = {
            Username: $scope.user.Username,
            Password: md5.createHash($scope.user.Password || '')
        };
        postJSON.postJSON("http://guesstheword-1.apphb.com/GTW/Users/Login", logUser).then(function (result) {
            if (result.data != {}) {
                $cookies.putObject("loggedUser", result.data);
                $location.path("landingPage");
            } else {
                console.log("Korisnik nije ulogovan!");
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