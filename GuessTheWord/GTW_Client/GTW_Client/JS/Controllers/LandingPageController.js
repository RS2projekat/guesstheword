app.controller("LandingPageController", ["$scope", "getJSON", "postJSON", "$location", "$window",
    function ($scope, getJSON, postJSON, $location, $window) {
        getJSON.getJSON("http://localhost:43474/GTW/WeeklyScores/").then(function (result) {
            $scope.weeklyscore = result.data;
        })
        
        getJSON.getJSON("http://localhost:43474/GTW/OverallScores/").then(function (result) {
            $scope.overallscore = result.data;
        })

        getJSON.getJSON("http://localhost:43474/GTW/GameRooms/").then(function (result) {
            $scope.rooms = result.data;
        })
        
        $scope.addRoom = function()
        {
            console.log("usao u addRoom(), name: " + $scope.newRoom.Name);
            if ($scope.newRoom.Name == "")
            {
                $window.alert("You must enter room name");
                console.log($scope.newRoom.Name);
                return;
            }

            var room = {
                Name: $scope.newRoom.Name,
                Date: new Date()
            };

            postJSON.postJSON("http://localhost:43474/GTW/GameRooms/", room).then(function (result) {
                console.log(result.data);
            })

            getJSON.getJSON("http://localhost:43474/GTW/GameRooms/").then(function (result) {
                $scope.rooms = result.data;
            })
        }
    }
]);