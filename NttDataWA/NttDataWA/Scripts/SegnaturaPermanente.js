$(document).ready(function () {
   
    $(".boxSegnaturePosition").click(function () {
        var boxSelect = this;
        $(".boxSegnaturePosition").each(function () {
            if (boxSelect === this) {
                $(this).addClass("selected");
                var id = $(boxSelect).attr("id");
                console.log("ID: " + id);
                var _position = '';
                switch (id) {
                    case 'boxSegnaturaPositionTopLeft':
                        _position = "TOP_L";
                        break;
                    case 'boxSegnaturaPositionTopCenter':
                        _position = "TOP_C";
                        break;
                    case 'boxSegnaturaPositionTopRight':
                        _position = "TOP_R";
                        break;
                    case 'boxSegnaturaPositionBottomLeft':
                        _position = "BOTTOM_L";
                        break;
                    case 'boxSegnaturaPositionBottomCenter':
                        _position = "BOTTOM_C";
                        break;
                    case 'boxSegnaturaPositionBottomRight':
                        _position = "BOTTOM_R";
                        break;
                    default:
                        _position = "TOP_L";
                }
                $("#valueFields")


                $("input[name*='segnaturaPermanentePosition']").val(_position);
            } else {
                $(this).removeClass("selected");
            }
        });
    });
});

function SetDocumentPreviewDimension(boxHeight, boxWidth, _selectPos) {
    try {
        $("#pageInfoHeight").html(boxHeight);
        $("#pageInfoWidth").html(boxWidth);

        var _actualBoxHeight = $("#segnaturaPermanenteMainContent").height();
        console.log("Actual Box Height: " + _actualBoxHeight + " --- Box Height da settare: " + boxHeight);
        // x : boxHeigh = 1 : _actualBoxHeight
        var _factor = 0;
        if (_actualBoxHeight > boxHeight) {
            _factor = boxHeight / (_actualBoxHeight - 20);
            console.log(">fattore : " + _factor);
        } else {
            _factor = (_actualBoxHeight - 20) / boxHeight;
            console.log("<fattore : " + _factor);
        }
        

        $("#blankDocumentPreview").height(boxHeight * _factor);
        $("#blankDocumentPreview").width(boxWidth * _factor);

        var _margineBoxSegnatura = 10;
        var _boxSegnaturaPositionHorizontalHeight =20;

        $("#boxSegnaturaPositionTopLeft").css('top', _margineBoxSegnatura);
        //$("#boxSegnaturaPositionTopCenter").css('top', _margineBoxSegnatura);
        $("#boxSegnaturaPositionTopRight").css('top', _margineBoxSegnatura);

        $("#boxSegnaturaPositionBottomLeft").css('bottom', _margineBoxSegnatura);
        //$("#boxSegnaturaPositionBottomCenter").css('bottom', _margineBoxSegnatura);
        $("#boxSegnaturaPositionBottomRight").css('bottom', _margineBoxSegnatura);
        $("#boxSegnaturaPositionLeft").css('top', _margineBoxSegnatura * 2 + _boxSegnaturaPositionHorizontalHeight);
        $("#boxSegnaturaPositionRight").css('top', _margineBoxSegnatura * 2 + _boxSegnaturaPositionHorizontalHeight);

        $("#boxSegnaturaPositionTopLeft").css('left', _margineBoxSegnatura);
        $("#boxSegnaturaPositionBottomLeft").css('left', _margineBoxSegnatura);

        //$("#boxSegnaturaPositionBottomLeft").css('center', _margineBoxSegnatura);
        //$("#boxSegnaturaPositionBottomLeft").css('right', _margineBoxSegnatura);

        $("#boxSegnaturaPositionLeft").css('left', _margineBoxSegnatura);
        $("#boxSegnaturaPositionRight").css('right', _margineBoxSegnatura);

        //$("#boxSegnaturaPositionTop").width(boxWidth * _factor - _margineBoxSegnatura * 2);
        $("#boxSegnaturaPositionTopLeft").width("45%");
        //$("#boxSegnaturaPositionTopCenter").width("30%");
        $("#boxSegnaturaPositionTopRight").width("45%");

        //$("#boxSegnaturaPositionBottom").width(boxWidth * _factor - _margineBoxSegnatura * 2);
        $("#boxSegnaturaPositionBottomLeft").width("45%");
        //$("#boxSegnaturaPositionBottomCenter").width("30%");
        $("#boxSegnaturaPositionBottomRight").width("45%");

        $("#boxSegnaturaPositionLeft").width(_boxSegnaturaPositionHorizontalHeight);
        $("#boxSegnaturaPositionRight").width(_boxSegnaturaPositionHorizontalHeight);

        $("#boxSegnaturaPositionTopLeft").height(_boxSegnaturaPositionHorizontalHeight);
        //$("#boxSegnaturaPositionTopCenter").height(_boxSegnaturaPositionHorizontalHeight);
        $("#boxSegnaturaPositionTopRight").height(_boxSegnaturaPositionHorizontalHeight);

        $("#boxSegnaturaPositionBottomLeft").height(_boxSegnaturaPositionHorizontalHeight);
        //$("#boxSegnaturaPositionBottomCenter").height(_boxSegnaturaPositionHorizontalHeight);
        $("#boxSegnaturaPositionBottomRight").height(_boxSegnaturaPositionHorizontalHeight);

        $("#boxSegnaturaPositionLeft").height((boxHeight * _factor) - _boxSegnaturaPositionHorizontalHeight * 2 - _margineBoxSegnatura * 4);
        $("#boxSegnaturaPositionRight").height(boxHeight * _factor - _boxSegnaturaPositionHorizontalHeight * 2 - _margineBoxSegnatura * 4);

        switch (_selectPos) {
            case 'TOP_L':
                $("#boxSegnaturaPositionTopLeft").addClass("selected");
                break;
            case 'TOP_C':
                //$("#boxSegnaturaPositionTopCenter").addClass("selected");
                break;
            case 'TOP_R':
                $("#boxSegnaturaPositionTopRight").addClass("selected");
                break;
            case 'BOTTOM_L':
                $("#boxSegnaturaPositionBottomLeft").addClass("selected");
                break;
            case 'BOTTOM_C':
                //$("#boxSegnaturaPositionBottomCenter").addClass("selected");
                break;
            case 'BOTTOM_R':
                $("#boxSegnaturaPositionBottomRight").addClass("selected");
                break;
            default:
                $("#boxSegnaturaPositionTopLeft").addClass("selected");
        }
        

    } catch (error) {
        console.error(error);
    }
}