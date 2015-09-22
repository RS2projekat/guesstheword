app.controller("gameroom", [
    "$scope", "xs", "$cookies", "$location", "$window",
    function ($scope, xs, $cookies, $location, $window) {

            var controller = xs.controller("gameroominactive");
            
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
                console.log("IN0");
                controller.invoke("register", user);
            }
            registerFunc();            

            controller.on("listinactiverooms", function (result) {
                console.log("rezultat iz listinactiverooms: " + result[0].Name);

                $scope.rooms = result;
                //$scope.$apply();
            });

            var listInactiveFunc = function ()
            {
                console.log("IN1");
                controller.invoke("listinactiverooms");
            }
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

                controller.on("makenewroom", function (result) {
                    console.log("rezultat iz makenewroom: " + result);

                    $location.path("/gameRoom");
                });

                var makeNewFunc = function ()
                {
                    console.log("IN2");
                    controller.invoke("makenewroom", room);
                }
                makeNewFunc();
            }

            $scope.enterRoom = function (roomIndex) {
                var room = $scope.rooms[roomIndex];

                controller.on("getintoroom", function (result) {
                    console.log("room: " + room.Name);
                    console.log("rezultat iz geintoroom: " + result);

                    $location.path("/gameRoom");
                });

                var enterRoomFunc = function ()
                {
                    console.log("IN3");
                    controller.invoke("getintoroom", room);
                }
                enterRoomFunc();
            }
        }
]);