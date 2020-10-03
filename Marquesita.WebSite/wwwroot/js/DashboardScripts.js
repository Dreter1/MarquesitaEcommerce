function dataTableScript() {
    $("#dataTable").DataTable({
        "responsive": true,
        "autoWidth": false,
        "language": {
            "sProcessing": "Procesando...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "Registros del _START_ al _END_ de un total de _TOTAL_",
            "sInfoEmpty": "Registros del 0 al 0 de un total de 0",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando...",
            "oPaginate": {
                "sFirst": "Primero",
                "sLast": "Último",
                "sNext": "Siguiente",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                "sSortDescending": ": Activar para ordenar la columna de manera descendente"
            }
        }
    });
}

function deleteRol(rol) {
    var roleId = rol.dataset.id;
    Swal.fire({
        title: '¿Desea eliminar el Rol?',
        text: "Si estas seguro presiona eliminar, recuerde que los usuarios que contengan este rol quedarán sin el. Por lo tanto luego debe asignarles un rol",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Eliminar'
    }).then((result) => {
        if (result.value) {
            console.log(result.value);
            $.ajax({
                url: "/Role/Delete",
                type: "POST",
                data: {
                    Id: roleId,
                },
                dataType: "JSON",
                success: function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Completado',
                        text: 'El rol fue eliminado con exito'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/Role/Index";
                        }
                    })
                },
                error: function (response) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'El rol no se pudo eliminar'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/Role/Index";
                        }
                    })
                }
            })
        }
    });
}

function deleteCategory(category) {
    var categoryId = category.dataset.id;
    Swal.fire({
        title: '¿Desea eliminar la Categoria?',
        text: "Si estas seguro presiona eliminar, recuerde que los productos que tengan esta categoria, tambien serán borrados!, tenga en cuenta eso",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Eliminar'
    }).then((result) => {
        if (result.value) {
            console.log(result.value);
            $.ajax({
                url: "/Category/Delete",
                type: "POST",
                data: {
                    Id: categoryId,
                },
                dataType: "JSON",
                success: function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Completado',
                        text: 'La categoría fue eliminado con exito'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/Category/Index";
                        }
                    })
                },
                error: function (response) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'La categoría no se pudo eliminar'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/Category/Index";
                        }
                    })
                }
            })
        }
    });
}

function deleteProduct(product) {
    var productId = product.dataset.id;
    console.log(productId);
    Swal.fire({
        title: '¿Desea eliminar este producto?',
        text: "Recuerda que cuando lo elimines, borrara todo registro que se tenga como: boletas, detalles de venta/compra. Si estas seguro presiona eliminar, ",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Eliminar'
    }).then((result) => {
        if (result.value) {
            console.log(result.value);
            $.ajax({
                url: "/Product/Delete",
                type: "POST",
                data: {
                    Id: productId,
                },
                dataType: "JSON",
                success: function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Completado',
                        text: 'El producto fue eliminado con exito'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/Product/List";
                        }
                    })
                },
                error: function (response) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'El producto no se pudo eliminar, vuelva a intentarlo más tarde'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/Product/List";
                        }
                    })
                }
            })
        }
    });
}

function removeUserCredentials(user) {
    var userId = user.dataset.id;
    Swal.fire({
        title: '¿Desea desactivar este usuario?',
        text: "Si estas seguro presiona desactivar, recuerde que puedes volver activarlo en cualquier momento.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Desactivar'
    }).then((result) => {
        if (result.value) {
            console.log(result.value);
            $.ajax({
                url: "/User/RemoveRestoreCredentials",
                type: "POST",
                data: {
                    Id: userId,
                },
                dataType: "JSON",
                success: function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Completado',
                        text: 'El usuario fue desactivado con exito'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/User/Index";
                        }
                    })
                },
                error: function (response) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'El usuario no se pudo desactivar, vuelva a intentarlo más tarde'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/User/Index";
                        }
                    })
                }
            })
        }
    });
}

function grantUserCredentials(user) {
    var userId = user.dataset.id;
    Swal.fire({
        title: '¿Desea activar este usuario?',
        text: "Si estas seguro presiona activar, recuerde que puedes volver a desactivarlo en cualquier momento.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Activar'
    }).then((result) => {
        if (result.value) {
            console.log(result.value);
            $.ajax({
                url: "/User/RemoveRestoreCredentials",
                type: "POST",
                data: {
                    Id: userId,
                },
                dataType: "JSON",
                success: function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Completado',
                        text: 'El usuario fue activado con exito'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/User/Index";
                        }
                    })
                },
                error: function (response) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'El usuario no se pudo activar, vuelva a intentarlo más tarde'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/User/Index";
                        }
                    })
                }
            })
        }
    });
}

function getPorductList() {
    var userId = $("#userId").val();
    var url = "/Sale/AddProductSale?userId=" + userId;
    $.get(url, function (e) {
        $("#vistaPro").html(e);
    });
}

function getDetailTempList() {
    var userId = $("#userId").val();
    var url = "/Sale/GetSaleDetailTempList?userId=" + userId;
    $.get(url, function (e) {
        $("#vistaTempItems").html(e);
    });
}

function addProductToList() {
    var productId = $("#productId").val();
    var Quantity = $("#Quantity").val();
    var userId = $("#userId").val();
    var url = "/Sale/AddProductSale?Productid=" + productId + "&Quantity=" + Quantity + "&UserId=" + userId;
    $.post(url, function (result) {
        if (result === true) {
            getPorductList();
            getDetailTempList();
        }
        else {
            getPorductList();
        }
    });
}

function incrementProductSaleTemp(product) {
    var id = product.dataset.id;
    var userId = $("#userId").val();
    var url = "/Sale/Increase?id=" + id + "&userId=" + userId;
    $.post(url, function () {
        getDetailTempList();
    });
}

function decreaseProductSaleTemp(product) {
    var id = product.dataset.id;
    var userId = $("#userId").val();
    var url = "/Sale/Decrease?id=" + id + "&userId=" + userId;
    $.post(url, function () {
        getDetailTempList();
    });
}

function deleteItemProductSaleTemp(product) {
    var id = product.dataset.id;
    var userId = $("#userId").val();
    var url = "/Sale/DeleteItem?id=" + id + "&userId=" + userId;
    $.post(url, function () {
        getDetailTempList();
    });
}

function confirmOrder() {
    var paymentType = $("#PaymentType").val();
    var userId = $("#UserId").val();
    var totalAmount = $("#TotalAmount").val();
    var url = "/Sale/CreateSale?PaymentType=" + paymentType + "&userId=" + userId + "&TotalAmount=" + totalAmount;
    Swal.fire({
        title: '¿Seguro que quiere realizar la venta?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Cancelar',
        confirmButtonText: 'Confirmar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.post(url, function (response) {
                if (response) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Venta realizada con exito',
                        showConfirmButton: false,
                        timer: 1500
                    });
                    location.href = "/Sale/Index";
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'No se pudo realizar la venta revise la venta',
                        showConfirmButton: false,
                        timer: 1500
                    });
                    location.href = "/Sale/CreateSale?userId=" + userId;
                }
                getDetailTempList();
            });
        }
    })
}