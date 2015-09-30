/*Ovaj kontroler se razlikuje od ostalih jer obavlja komunikaciju preko xsocketa*/
app.controller("ChatController", ["$scope", function ($scope) {
    /*Funkcija za kreiranje modela koji saljemo serveru*/
    var chatMessageModel = function (message) {
        console.log("pravim novi model "+message+"\n");
        this.message = message;
        this.created = new Date();
    };

    $scope.chatMessages = [];
    $scope.chatMessage = "";

    var konekcija = new XSockets.WebSocket("ws://guesstheword-1.apphb.com:4502/", ['chat']);
    var controller = konekcija.controller("chat");

    controller.on("chatMessage", function (model) {

        console.log("OVDE se prihvata odgovor servera " + model.message + "\n");
        $scope.chatMessages.unshift(model);
        console.log($scope.chatMessages.toString());
    });

    $scope.sendChatMessage = function() {
        console.log("pritisnuto dugme\n");
        controller.invoke("chatMessage", new chatMessageModel($scope.chatMessage));
    };
}]);