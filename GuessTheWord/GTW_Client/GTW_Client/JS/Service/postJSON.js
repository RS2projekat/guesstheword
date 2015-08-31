app.factory('postJSON', function ($http) {
    pack = {};
    sending = {};
    sending.result = null;

    /*ovo je funkcija servisa koju cemo koristiti iz kontrolera za slanje podataka serveru*/
    var _postJSON = function (path, json) {
        return $http.post(path, json).success(function (result) {

            return result;

        })
    };

    sending.postJSON = _postJSON;
    return sending;
});