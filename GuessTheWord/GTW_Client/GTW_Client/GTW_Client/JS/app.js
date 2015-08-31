var app = angular.module("myApp", ['ngRoute', 'xsockets']);

app.config(['$locationProvider', '$routeProvider', 'xsProvider',
    function ($locationProvider, $routeProvider, xsProvider) {

        /*Ovde postavljamo putanje*/
        $routeProvider.when('/gameRoom', {
            templateUrl: '/Views/gameRoom.html'
        }).when('/login', {
            templateUrl: '/Views/login.html'
        }).when('/register', {
            templateUrl: '/Views/register.html'
            /* 
                kontroler se moze dodati ili ovde ili 
                sa ng-controller u htmlu 
            controller: 'RegisterController'
            */
        }).otherwise({
            redirectTo: '/login'
        });

        /*Ovo je deo za rad sa xsocketima*/
        //xSocketi - konekcija
        xsProvider.url = "ws://localhost:43474/";

        //ovde navode kontroleri sa servera za xsockete
        xsProvider.controllers = ["chat"];
    }
]);