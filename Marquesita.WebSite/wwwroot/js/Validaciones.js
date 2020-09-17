$(document).ready(function () {
    validacionNumeros()
});

function validacionNumeros() {
    $(".numeric").numeric();
    $(".integer").numeric(false, function () { alert("Integers only"); this.value = ""; this.focus(); });
    $(".positive").numeric({ negative: false }, function () { alert("No negative values"); this.value = ""; this.focus(); });
    $(".positive-integer").numeric({ decimal: false, negative: false }, function () { alert("Positive integers only"); this.value = ""; this.focus(); });
    $(".decimal-2-places").numeric({ decimalPlaces: 2 });
    $(".decimal-3-places").numeric({ decimalPlaces: 3 });
    $("#remove").click(
        function (e) {
            e.preventDefault();
            $(".numeric,.integer,.positive,.positive-integer,.decimal-2-places").removeNumeric();
        }
        );
}

function validarLetras(e) {
    var key = e.keyCode || e.which;

    var tecla = String.fromCharCode(key).toLowerCase();
    var letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
    var especiales = [8, 37, 39, 46];
    var tecla_Especial = false;

    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_Especial = false;
            break;
        };
    }

    if (letras.indexOf(tecla) == -1 && !tecla_Especial)
        return false;
}