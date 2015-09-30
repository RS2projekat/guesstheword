app.controller("LandingPageController", ["$scope", "$cookies", "$location", "$window", function ($scope, $cookies, $location, $window) {
    var connection = new XSockets.WebSocket("ws://guesstheword-1.apphb.com:4502/", ["gameroominactive"]);
    var controller = connection.controller("gameroominactive");

    var user = {
        Username: $cookies.get("loggedUser"),
        Role: "User"
    };

    controller.on("register", function (result) {
        console.log("rezultat iz register: " + result);

        if (result == false)
            return;
    });

    var registerFunc = function ()
    {
        controller.invoke("register", user).then(function (response) {
            console.log(response);
        });
    };
    registerFunc();

    controller.on("listinactiverooms", function (result) {
        if( result.length > 0 ) {
            console.log("rezultat iz  listinactiverooms: " + result[0].Name);
        }
        else {
            console.log("rezultat iz  listinactiverooms: EMPTY_STRING");
        }

        $scope.rooms = result;
    });

    var listInactiveFunc = function ()
    {
        controller.invoke("listinactiverooms");
    };
    listInactiveFunc();

    $scope.newRoom = "";

    $scope.addRoom = function () {
        if ($scope.newRoom == "") {
            $window.alert("You must enter room name.");
            return;
        }

        var room = {
            Name: $scope.newRoom,
            Date: new Date()
        };

        $cookies.putObject("activeRoom", room);

        controller.on("makenewroom", function (result) {
            console.log("rezultat iz makenewroom: " + result);

            $location.path("/gameRoom");
        });

        var makeNewFunc = function ()
        {
            console.log("IN2");
            controller.invoke("makenewroom", room);
        };
        makeNewFunc();
    };

    $scope.enterRoom = function (roomIndex) {
        var room = $scope.rooms[roomIndex];

        $cookies.putObject("activeRoom", room);

        controller.on("getintoroom", function (result) {
            console.log("room: " + room.Name);
            console.log("rezultat iz geintoroom: " + result);

            $location.path("/gameRoom");
        });

        var enterRoomFunc = function ()
        {
            console.log("IN3");
            controller.invoke("getintoroom", room);
        };
        enterRoomFunc();
    };

    (function() {
        var temp =angular.fromJson($cookies.getObject('loggedUser'));
        if(  temp == undefined ) {
            $location.path("login");
        }
    })();
}]);