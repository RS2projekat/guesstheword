app.controller("gameroom", [
    "$scope", "xs",
        function ($scope, xs) {
            var controller = xs.controller("gameroom");

            $scope.register = function ()
            {
                var data = {};
            }


            controller.on("listinactiverooms", function (result) {

                console.log("rezultat: " + result[0].Name);

                $scope.rooms = result;
                //$scope.$apply();
            });

            var listInactive = function ()
            {
                console.log("IN1");
                controller.invoke('listinactiverooms');
            }

            listInactive();


            $scope.addRoom = function () {
                if ($scope.newRoom == "") {
                    $window.alert("You must enter room name");
                    return;
                }

                var room = {
                    Name: $scope.newRoom,
                    Date: new Date()
                };

                controller.on("makenewroom", function (result) {
                    
                    console.log("rezultat: " + result);

                    //$scope.rooms = result;
                    //$scope.$apply();

                });

                var add = function () {
                    console.log("IN2");
                    controller.invoke('makenewroom', room);
                }

                add();
            }


            $scope.enterRoom = function (roomIndex) {
                var room = $scope.rooms[roomIndex];

                controller.on("getintoroom", function (result) {

                    console.log("rezultat: " + result);

                    //$scope.rooms = result;
                    //$scope.$apply();

                });

                var makeRoom = function () {
                    console.log("IN3");
                    controller.invoke('getintoroom', room);
                }

                makeRoom();
            }
        }
]);