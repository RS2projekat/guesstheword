var app = angular.module("myApp", ['ngRoute', 'ngCookies', 'angular-md5']);

app.config(['$locationProvider', '$routeProvider',
    function ($locationProvider, $routeProvider) {

        $routeProvider.when('/gameRoom', {
            templateUrl: 'Views/gameRoom.html'
        }).when('/login', {
            templateUrl: 'Views/login.html'
        }).when('/register', {
            templateUrl: 'Views/register.html'
        }).when('/landingPage', {
            templateUrl: 'Views/landingPage.html'
        }).when('/logout', {
            templateUrl: 'Views/logout.html'
        }).otherwise({
            redirectTo: 'login'
        });
    }
]);