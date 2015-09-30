app.controller("GameRoomController", ["$scope", "$location", "$cookies", "$window", "$timeout", function ($scope, $location, $cookies, $window, $timeout) {
    var connection = new XSockets.WebSocket("ws://guesstheword-1.apphb.com:4502/", ["gameroomactive"]);
    var controller = connection.controller("gameroomactive");

    (function() {
        var temp =angular.fromJson($cookies.getObject('loggedUser'));
        if(  temp == undefined ) {
            $location.path("login");
        }
    })();

    var userReg = {
        Username: $cookies.getObject("loggedUser").Username,
        Role: "Admin"
    };

    var roomReg = $cookies.getObject("activeRoom");

    var userRoom = {
        user: userReg,
        gameRoom: roomReg
    };

    controller.on("registeruserroom", function (result) {
        console.log("rezultat iz registerUR: " + result);

        if (result == false)
            return;
    });

    var registerURFunc = function () {
        console.log("u registerURFunc");
        controller.invoke("registeruserroom", userRoom);
    }
    registerURFunc();

    /*Deo za canvas*/

    var canvas = document.getElementById('mainCanvas');
    var context = canvas.getContext('2d');

    var canvaso = document.getElementById('imageView');
    var contexto = canvaso.getContext('2d');


    var rect = canvas.getBoundingClientRect();
    var movingIdn = false;

    $scope.undoDisabled = true;
    $scope.redoDisabled = true;

    $scope.img_update = function () {
        console.log("USLO");
        var v = canvaso.toDataURL();
        $scope.undoArray.push(v);
        contexto.drawImage(canvas, 0, 0);
        $scope.redoArray.length = 0;
        context.clearRect(0, 0, canvas.width, canvas.height);
        $scope.redoDisabled = true;
        $scope.undoDisabled = false;
    }
    $scope.removeFlag = function () {
        drawing = false;
    };



    $scope.mouseIsDown = false;//flag

    $scope.canvasModel = {
        color: 'black',
        fillColor: 'white',
        lineWidth: 2,
        rubberWidth: 4,
        prevX: 0,
        prevY: 0,
        currX: 0,
        currY: 0
    };

    $scope.scale = {
        w: canvas.offsetWidth,
        h: canvas.offsetHeight
    };

    //crtanje
    var idCurr = false;
    var drawing = false;
    var pencilFlag = false, rectFlag = false, lineFlag = false, circleFlag = false;
    var started = false;

    $scope.undoArray = [];
    $scope.redoArray = [];

    $scope.mouseDown = function (event) {
        console.log("pocelo da crta");
        if (pencilFlag == true) {
            console.log("uslo u pencil");
            drawing = true;
            context.beginPath();
            $scope.canvasModel.currX = event.clientX - rect.left;
            $scope.canvasModel.currY = event.clientY - rect.top;
            context.moveTo($scope.canvasModel.currX, $scope.canvasModel.currY);
            started = true;
        }
        else if (rectFlag == true || lineFlag == true || circleFlag == true) {
            console.log("uslo u rect");
            drawing = true;
            $scope.canvasModel.prevX = event.clientX - rect.left;
            $scope.canvasModel.prevY = event.clientY - rect.top;
            started = true;
        }

    };

    $scope.mouseMove = function (event) {
        if (drawing == false) {
            return;
        }
        if (started == true) {
            if (pencilFlag == true) {

                $scope.canvasModel.prevX = $scope.canvasModel.currX;
                $scope.canvasModel.prevY = $scope.canvasModel.currY;
                $scope.canvasModel.currX = event.clientX - rect.left;
                $scope.canvasModel.currY = event.clientY - rect.top;

                $scope.draw(event);
            }

            if (rectFlag == true || lineFlag == true || circleFlag == true) {

                $scope.canvasModel.currX = event.clientX - rect.left;
                $scope.canvasModel.currY = event.clientY - rect.top;
                $scope.draw(event);
            }
        }

    };
    $scope.draw = function (event) {

        if (rectFlag == true) {//crtanje pravougaonika
            var x = Math.min($scope.canvasModel.prevX, $scope.canvasModel.currX);
            var y = Math.min($scope.canvasModel.prevY, $scope.canvasModel.currY);
            var h = Math.abs($scope.canvasModel.currY - $scope.canvasModel.prevY);
            var w = Math.abs($scope.canvasModel.currX - $scope.canvasModel.prevX);


            if (!w || !h)
                return;
            context.strokeStyle = $scope.canvasModel.color;
            context.fillStyle = $scope.canvasModel.fillColor;
            context.lineWidth = $scope.canvasModel.lineWidth;
            context.clearRect(0, 0, canvas.width, canvas.height);
            context.fillRect(x, y, w, h);
            context.strokeRect(x, y, w, h);

        }
        else if (pencilFlag == true) {//crtanje olovkom
            context.beginPath();
            context.moveTo($scope.canvasModel.prevX, $scope.canvasModel.prevY);
            context.lineTo($scope.canvasModel.currX, $scope.canvasModel.currY);
            context.strokeStyle = $scope.canvasModel.color;
            context.lineWidth = $scope.canvasModel.lineWidth;
            context.stroke();
            context.closePath();
        }
        else if (lineFlag == true) {

            context.clearRect(0, 0, canvas.width, canvas.height);

            context.beginPath();
            context.moveTo($scope.canvasModel.prevX, $scope.canvasModel.prevY);
            context.lineTo($scope.canvasModel.currX, $scope.canvasModel.currY);
            context.strokeStyle = $scope.canvasModel.color;
            context.lineWidth = $scope.canvasModel.lineWidth;
            context.stroke();
            context.closePath();
        }
        else if (circleFlag == true) {
            context.clearRect(0, 0, canvas.width, canvas.height);
            context.beginPath();
            var x1 = $scope.canvasModel.prevX;
            var y1 = $scope.canvasModel.prevY;
            var x2 = $scope.canvasModel.currX;
            var y2 = $scope.canvasModel.currY;

            var radius = Math.sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2))
            context.arc($scope.canvasModel.prevX, $scope.canvasModel.prevY, radius, 0, 2 * Math.PI, false);
            context.fillStyle = $scope.canvasModel.fillColor;
            context.strokeStyle = $scope.canvasModel.color;
            context.lineWidth = $scope.canvasModel.lineWidth;
            context.fill();
            context.stroke();

            context.closePath();
        }
    };


    $scope.mouseUp = function (event) {
        if (started == true) {

            $scope.mouseMove(event);
            started = false;
            $scope.removeFlag();
            $scope.img_update();
            $scope.undoDisabled = false;
        }

    };
    $scope.fillColor = function (color) {
        $scope.canvasModel.fillColor = color;

    }
    //postavljlanje boje
    $scope.color = function (color) {
        $scope.canvasModel.color = color;

        if (color == "white") {
            $scope.canvasModel.lineWidth = $scope.canvasModel.rubberWidth;
            pencilFlag = true;
            rectFlag = false;
            lineFlag = false;
            circleFlag = false;
        }
    };

    //tehnike crtanja
    $scope.drawingStyle = function (style) {

        if (style == "line") {
            circleFlag = false;
            pencilFlag = false;
            rectFlag = false;
            lineFlag = true;
        }
        if (style == "rect") {
            circleFlag = false;
            pencilFlag = false;
            lineFlag = false;
            rectFlag = true;
        }
        if (style == "pencil") {
            circleFlag = false;
            pencilFlag = true;
            rectFlag = false;
            lineFlag = false;
        }
        if (style == "circle") {
            circleFlag = true;
            pencilFlag = false;
            rectFlag = false;
            lineFlag = false;
        }
        idCurr = false;
    };


    /*Pokretanje tajmera i omogucavanje crtanja(samo se obrise sadrzaj canvasa)*/
    /*TO DO: napraviti niz elemenata (data) koji su nacrtani na canvasu. Ovde ce da se taj niz isprazni i
        sadrzaj canvasa obrise. Nakon toga trebalo bi proveriti da li je trenutni korisnik onaj koji crta i
        samo njemu omoguciti crtanje.*/
    $scope.isDisabled = false;

    $scope.startGame = function () {
        var val = $window.prompt("Unesite zagonetnu rec:");

        if (val != null) {
            context.clearRect(0, 0, canvas.width, canvas.height);
            $scope.startTimer();
            $scope.isDisabled = true;

            controller.on("start", function (retUser) {
                console.log("korisnik koji je zapoceo igru: " + retUser.Username);
            });

            var startGameFunc = function () {
                console.log("u startGameFunc");
                controller.invoke("settheword", val);
                controller.invoke("startgame", val);
            }
            startGameFunc();
        }
    };


    $scope.undo = function () {

        console.log("USLO U UNDO");
        var canv1 = $scope.undoArray.pop();
        var state = canvaso.toDataURL();
        $scope.redoArray.push(state);
        $scope.redoDisabled = false;
        if (canv1 != null) {
            contexto.clearRect(0, 0, canvaso.width, canvaso.height);
            var img = new Image;
            img.onload = function () {
                contexto.drawImage(img, 0, 0);

            };
            img.src = canv1;
        }
        if ($scope.undoArray.length == 0)
            $scope.undoDisabled = true;
    };


    $scope.redo = function () {
        console.log("USLO U REDO");
        var canv1 = $scope.redoArray.pop();
        var state = canvaso.toDataURL();
        $scope.undoArray.push(state);
        $scope.undoDisabled = false;
        if (canv1 != null) {
            contexto.clearRect(0, 0, canvaso.width, canvaso.height);
            var img = new Image;
            img.onload = function () {
                contexto.drawImage(img, 0, 0);

            };
            img.src = canv1;
        }
        if ($scope.redoArray.length == 0)
            $scope.redoDisabled = true;
    };

    $scope.sizeChanged = function () {
        $scope.canvasModel.lineWidth = $scope.sizeSelect;
    };
    $scope.rubberChanged = function () {
        $scope.canvasModel.rubberWidth = $scope.rubberSelect;
    };

    /*Deo za tajmer*/

    $scope.angle = 0;
    $scope.c = 0;

    var moveTimer = function () {
        $scope.angle += 6;

        if ($scope.sec < 60) {
            $scope.sec++;
            $timeout(moveTimer, 1000);
        }
        else {
            $scope.isDisabled = false;

            var endGameFunc = function () {
                console.log("u endGameFunc");
                controller.invoke("endgame");
            }
            endGameFunc();
        }
    };
    $scope.startTimer = function () {
        $scope.sec = 1;
        $scope.angle = 0;
        $timeout(moveTimer, 1000);
    };

  /*  $scope.colorChanged = function(){
        $scope.canvasModel.color = $scope.colorInput;
    };*/

    controller.on("end", function (retGameRoom) {
        console.log("zavrsena igra");
        console.log("end soba: " + retGameRoom.Name);
    });

    controller.on("win", function (retGameRoom) {
        console.log("u win; retGameRoom je: " + retGameRoom.Name);
    });

    controller.on("wrong", function (userMessage) {
        console.log("u wrong");
        console.log("message: " + userMessage.message);
        console.log("user: " + userMessage.user);
    });

    $scope.makeGuess = function () {
        console.log("u makeGuessFunc");
        console.log("guessMessage je: " + $scope.guessMessage);
        $scope.guesses = $scope.guesses + "\n" + $scope.guessMessage;
        controller.invoke("guesstheword", $scope.guessMessage);
    };
}]);