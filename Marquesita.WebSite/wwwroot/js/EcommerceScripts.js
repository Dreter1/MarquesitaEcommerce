function goToShoppingCart() {
    location.href = "/ShoppingCart/Index";
}

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
                                title: 'El producto se añadio al carrito de compras',
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
                            location.href = "/ShoppingCart/Index";
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

function incrementCartItemQuantity(itemCart) {
    var id = itemCart.dataset.id;
    var url = "/ShoppingCart/Increase?id=" + id;
    $.post(url, function () {
        getshoppingCartList();
    });
}

function decreaseCartItemQuantity(itemCart) {
    var id = itemCart.dataset.id;
    var url = "/ShoppingCart/Decrease?id=" + id;
    $.post(url, function () {
        getshoppingCartList();
    });
}

function deleteCartItem(itemCart) {
    var id = itemCart.dataset.id;
    var url = "/ShoppingCart/DeleteItem?id=" + id;
    $.post(url, function () {
        getshoppingCartList();
    });
}

function getWishList() {
    var url = "/WishList/GetUserWishList";
    $.get(url, function (response) {
        $("#wishList").html(response);
    });
}

function addProductToWishList(product) {
    var addProduct = product.dataset.id;
    var userId = $('#userId').val();
    var isLogedUrl = "/User/IsLogged";
    var productUserExistUrl = "/WishList/ProductAndUserExistInWishList?idProduct=" + addProduct + "&userId=" + userId;
    var addingProdcutToWishListUrl = "/WishList/AddProductToWishList?idProduct=" + addProduct + "&userId=" + userId;
    $.get(isLogedUrl, function (response) {
        if (response) {
            $.post(productUserExistUrl, function (response) {
                if (response) {
                    $.post(addingProdcutToWishListUrl, function (response) {
                        if (response) {
                            Swal.fire({
                                icon: 'success',
                                title: 'El producto se añadio a tu lista de deseos',
                                showConfirmButton: false,
                                timer: 2500
                            })
                            getWishList();
                        }
                        else {
                            Swal.fire({
                                title: 'No se pudo agregar el producto a la lista de deseos vuelva a intentarlo',
                                icon: 'error',
                                showCancelButton: false,
                                confirmButtonColor: '#3085d6',
                                confirmButtonText: 'Ok'
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    getWishList();
                                }
                            })
                        }
                    });
                }
                else {
                    Swal.fire({
                        title: 'Este producto ya esta en tu lista de deseos',
                        icon: 'error',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        confirmButtonText: 'Lista de deseos'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            location.href = "/WishList/Index";
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

function deleteWishListItem(itemCart) {
    var id = itemCart.dataset.id;
    var url = "/WishList/DeleteItem?id=" + id;
    $.post(url, function () {
        getWishList();
    });
}