app.controller("GameController",
    function ($scope, $window, $timeout) {

        /*Deo za canvas*/

        var canvas = document.getElementById('mainCanvas');
        var context = canvas.getContext('2d');

        var canvaso = document.getElementById('imageView');
        var contexto = canvaso.getContext('2d');


        var rect = canvas.getBoundingClientRect();
        var movingIdn = false;

        $scope.img_update = function () {
            console.log("USLO");
            contexto.drawImage(canvas, 0, 0);
            context.clearRect(0, 0, canvas.width, canvas.height);
        }
        $scope.removeFlag = function () {
            drawing = false;
        };



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
        var drawing = false;
        var pencilFlag = false, rectFlag = false, lineFlag = false, circleFlag = false;
        var started = false;



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
                context.lineWidth = $scope.canvasModel.lineWidth;
                context.clearRect(0, 0, canvas.width, canvas.height);
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
                context.strokeStyle = $scope.canvasModel.color;
                context.lineWidth = $scope.canvasModel.lineWidth;
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
            }

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
            else {
                $scope.isDisabled = false;
            }




        };
        $scope.startTimer = function () {
            $scope.sec = 1;
            $scope.angle = 0;
            $timeout(moveTimer, 1000);
        };
    });