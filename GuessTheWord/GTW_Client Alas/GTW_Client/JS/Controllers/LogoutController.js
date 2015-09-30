app.controller("LogoutController", ["$location", "$cookies", function ($location, $cookies) {
    (function() {
        $cookies.remove("loggedUser");
        $location.path("login");
    })();
}]);