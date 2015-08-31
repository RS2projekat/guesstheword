/*Ovaj kontroler se razlikuje od ostalih jer obavlja komunikaciju preko xsocketa*/
app.controller("ChatController", [
    "$scope", "xs",
        function ($scope, xs) {
            /*Funkcija za kreiranje modela koji saljemo serveru*/
            var chatMessageModel = function (message) {
                console.log("pravim novi model "+message+"\n");
                this.message = message;
                this.created = new Date();
            };

            /*Model koji se prikazuje u htmlu*/
            $scope.chatMessages = [];
            $scope.chatMessage = "";

            var controller = xs.controller("Chat");

            /*Ovo se izvrsava nakon sto server vrati odgovor*/
            /*TO DO: chat ne radi ispravno ako se ode na drugu stranicu pa se potom ponovo vratimo i pokusamo da ga pokrenemo.
                Da bi tada chat ponovo radio mora da se ode na reload. Namestiti to!*/
            controller.on("chatMessage", function (model) {

                console.log("OVDE se prihvata odgovor servera " + model.message + "\n");           
                $scope.chatMessages.unshift(model);
                console.log($scope.chatMessages.toString());
                
                //    $scope.$apply();
                
            });

            /*Ova funkcija se poziva klikom na dugme.*/
            $scope.sendChatMessage = function () {
                console.log("pritisnuto dugme\n");
              //  xs.get("ws://localhost:43474/Controllers/ChatController")

                /*Ovde se serveru salju podaci*/
                controller.invoke("chatMessage", new chatMessageModel($scope.chatMessage));
            };
        }
]);