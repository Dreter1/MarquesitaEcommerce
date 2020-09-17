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
    console.log(roleId);
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
    console.log(categoryId);
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
        text: "Si estas seguro presiona eliminar, recuerde que al eliminarlo no habra vuelta atras.",
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
