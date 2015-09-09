app.controller("LandingPageController", ["$scope", "getJSON", "$location",
    function ($scope, getJSON, $location) {
        getJSON.getJSON("http://localhost:43474/GTW/WeeklyScores/").then(function (result) {
            $scope.weeklyscore = result.data;
        })
        
        getJSON.getJSON("http://localhost:43474/GTW/OverallScores/").then(function (result) {
            $scope.overallscore = result.data;
        })
    }
]);