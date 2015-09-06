app.factory('getJSON', function ($http) {
    var getRes = {};

    var _getJSON = function (path) {
        return $http.get(path).then(function (result) {

            return result;

        });
    };

    getRes.getJSON = _getJSON;

    return getRes;
});

/*
Koriscenje u kontroleru (u ovom primeru se listaju sve sobe):

getJSON.getJSON("http://localhost:43474/GTW/GameRooms/").then(function (result) {
            console.log(result.data);
            $scope.jsoni = result.data;
        })
*/