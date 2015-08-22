var myApp = angular.module("myApp", ['ngRoute', 'myControllers', 'xsockets']);

myApp.config(['$locationProvider', '$routeProvider', 'xsProvider',
    function ($locationProvider, $routeProvider, xsProvider) {
        // just set up one simple route
        $routeProvider.when('/gameRoom', {
            templateUrl: '/Views/gameRoom.html',
            controller: 'ChatController'
        }).when('/login', {
            templateUrl: '/Views/login.html',
           // controller: 'LoginController'
        }).when('/register', {
            templateUrl: '/Views/register.html',
           // controller: 'RegisterController'
        }).otherwise({
            redirectTo: '/login'
        });
        xsProvider.url = "ws://localhost:43474/"; //+ location.host;
        xsProvider.controllers = ["chat"];
       // xsProvider.controllers = ["login"];
    }
]);