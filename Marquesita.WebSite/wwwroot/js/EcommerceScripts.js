function getProductPanelList() {
    var url = "/Product/GetProductList";
    $.get(url, function (e) {
        $("#ProducList").html(e);
    });
}

function getshoppingCartList() {
    var url = "/ShoppingCart/GetUserCartItems";
    $.get(url, function (response) {
        $("#shoppingCartList").html(response);
    });
}

function addProductToCart(product) {
    var addProduct = product.dataset.id;
    var userId = $('#userId').val();
    var isLogedUrl = "/User/IsLogged";
    var productUserExistUrl = "/ShoppingCart/ProductAndUserExistInCart?idProduct=" + addProduct + "&userId=" + userId;
    var addingProdcutToCartUrl = "/ShoppingCart/AddProductToCart?idProduct=" + addProduct + "&userId=" + userId;
    $.get(isLogedUrl, function (response) {
        if (response) {
            $.post(productUserExistUrl, function (response) {
                if (response) {
                    $.post(addingProdcutToCartUrl, function (response) {
                        if (response) {
                            Swal.fire({
                                icon: 'success',
                                title: 'Your work has been saved',
                                showConfirmButton: false,
                                timer: 2500
                            })
                            getProductPanelList();
                        }
                        else {
                            Swal.fire({
                                title: 'No se pudo agregar el producto al carrito vuelva a intentarlo',
                                icon: 'error',
                                showCancelButton: false,
                                confirmButtonColor: '#3085d6',
                                confirmButtonText: 'Ok'
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    getProductPanelList();
                                }
                            })
                        }
                    });
                }
                else {
                    Swal.fire({
                        title: 'Este producto ya esta en tu carrito, aumenta la cantidad desde tu carrito',
                        icon: 'error',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Ir a carrito'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            location.href = "/Home/Index";
                        }
                    })
                }
            });
        }
        else {
            Swal.fire({
                title: 'Error',
                text: "No has iniciado sesión, no puedes agregar un producto a tu carrito",
                icon: 'error',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Iniciar Sesion'
            }).then((result) => {
                if (result.isConfirmed) {
                    location.href = "/Home/Login";
                }
            })
        }
    });

}