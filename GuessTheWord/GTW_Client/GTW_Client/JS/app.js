﻿var app = angular.module("myApp", ['ngRoute', 'xsockets', 'ngCookies']);

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
        }).when('/landingPage', {
            templateUrl: '/Views/landingPage.html'
        }).otherwise({
            redirectTo: '/login'
        });

        /*Ovo je deo za rad sa xsocketima*/
        //xSocketi - konekcija
        xsProvider.url = "ws://guesstheword-1.apphb.com/";

        //ovde navode kontroleri sa servera za xsockete
        xsProvider.controllers = ["chat", "gameroominactive", "gameroomactive"];
    }
]);