app.controller("GameController",
    function ($scope, $window, $timeout) {

        /*Deo za canvas*/

        var canvas = document.getElementById('mainCanvas');
        var context = canvas.getContext('2d');
        var rect = canvas.getBoundingClientRect();
        var movingIdn = false;

        $scope.mouseIsDown = false;//flag

        $scope.canvasModel = {
            color: black,
            lineWidth: 2,
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
        var pencilFlag = true, rectFlag = false;

        $scope.mouseMove = function (event) {
            if (pencilFlag == true) {
                $scope.canvasModel.prevX = $scope.canvasModel.currX;
                $scope.canvasModel.prevY = $scope.canvasModel.currY;
                $scope.canvasModel.currX = event.clientX - rect.left;
                $scope.canvasModel.currY = event.clientY - rect.top;

                $scope.draw(event);
            }
           /* if (rectFlag == true && $scope.mouseIsDown == true) {

                $scope.canvasModel.currX = event.clientX - rect.left;
                $scope.canvasModel.currY = event.clientY - rect.top;
                $scope.draw(event);
            }*/
        };

        $scope.draw = function (event) {

            if (rectFlag == true) {//crtanje pravougaonika
                context.beginPath();
                var x = $scope.canvasModel.prevX < $scope.canvasModel.currX ? $scope.canvasModel.prevX : $scope.canvasModel.currX;
                var y = $scope.canvasModel.prevY < $scope.canvasModel.currY ? $scope.canvasModel.prevY : $scope.canvasModel.currY;
                context.rect(x, y, Math.abs($scope.canvasModel.currX - $scope.canvasModel.prevX), Math.abs($scope.canvasModel.currY - $scope.canvasModel.prevY));
                context.strokeStyle = $scope.canvasModel.color;
                context.lineWidth = $scope.canvasModel.lineWidth;
                context.stroke();
                context.closePath();
            }
            else if ($scope.mouseIsDown) {//crtanje linije ili olovkom
                context.beginPath();
                context.moveTo($scope.canvasModel.prevX, $scope.canvasModel.prevY);
                context.lineTo($scope.canvasModel.currX, $scope.canvasModel.currY);
                context.strokeStyle = $scope.canvasModel.color;
                context.lineWidth = $scope.canvasModel.lineWidth;
                context.stroke();
                context.closePath();
            }
        };

        $scope.setFlag = function (event) {
            $scope.mouseIsDown = true;
            if (pencilFlag == true) {
                $scope.canvasModel.prevX = $scope.canvasModel.currX;
                $scope.canvasModel.prevY = $scope.canvasModel.currY;
                $scope.canvasModel.currX = event.clientX - rect.left;
                $scope.canvasModel.currY = event.clientY - rect.top;
                $scope.draw(event);
            }
            else {
                //ucitala sam jedno teme
                $scope.canvasModel.prevX = event.clientX - rect.left;
                $scope.canvasModel.prevY = event.clientY - rect.top;
               
            }
        };

        $scope.removeFlag = function () {
            $scope.mouseIsDown = false;
        };

        $scope.mouseUp = function (event) {
            if (rectFlag == true) {
                $scope.canvasModel.currX = event.clientX - rect.left;
                $scope.canvasModel.currY = event.clientY - rect.top;
             
                $scope.draw(event);
            }
            $scope.mouseIsDown = false;
        };

        //postavljlanje boje
        $scope.color = function (color) {
            $scope.canvasModel.color = color;

            if (color == "white") {
                $scope.canvasModel.lineWidth = 14;
                pencilFlag = true;
                rectFlag = false;
            }
            else
                $scope.canvasModel.lineWidth = 2;
        };

        //tehnike crtanja
        $scope.drawingStyle = function (style) {
            
            if (style == "line") {
                pencilFlag = false;
                rectFlag = false;
            }
            if (style == "rect") {
                pencilFlag = false;
                rectFlag = true;
            }
            if (style == "pencil") {//else {
                pencilFlag = true;
                rectFlag = false;
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

            if (val != null) 
            {
                context.clearRect(0, 0, canvas.width, canvas.height);
                $scope.startTimer();
                $scope.isDisabled = true;
            }
        };

        /*Deo za tajmer*/

        $scope.angle = 0;
        $scope.sec = 0;

        var moveTimer = function () {
            $scope.angle += 6;

            if ($scope.sec < 60) {
                $scope.sec++;
                $timeout(moveTimer, 1000);
            }
            else
            {
                $scope.isDisabled = false;
            }

        }

        $scope.startTimer = function () {
            $scope.sec = 1;
            $scope.angle = 0;
            $timeout(moveTimer, 1000);
        };
    });