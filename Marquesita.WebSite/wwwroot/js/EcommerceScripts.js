function goToShoppingCart() {
    location.href = "/ShoppingCart/Index";
}

function getProductPanelList() {
    var url = "/Product/GetProductList";
    $.get(url, function (e) {
        $("#ProducList").html(e);
    });
}

function getProductPanelListTake10() {
    var url = "/Product/GetProductListTake10";
    $.get(url, function (e) {
        $("#ProducListTake10").html(e);
    });
}

function getCategoryProductPanelList() {
    var categoryId = $('#categoryId').val();
    console.log(categoryId)
    var url = "/Category/GetCategoryProductList?categoryId=" + categoryId;
    $.get(url, function (e) {
        $("#CategoryProductList").html(e);
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

function getSaleDetails(sale) {
    var Id = sale.dataset.id;
    location.href = "/Client/MyOrderDetail?saleId=" + Id;
}

function editAddress(address) {
    var Id = address.dataset.id;
    location.href = "/Client/EditAddress?Id=" + Id;
}

function deleteAddress(address) {
    var addressId = address.dataset.id;
    Swal.fire({
        title: '¿Desea eliminar esta direccion?',
        text: "Si estas seguro presiona eliminar, recuerde que si elimina la direccion no habra vuelta atras.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Eliminar'
    }).then((result) => {
        if (result.value) {
            console.log(result.value);
            $.ajax({
                url: "/Client/DeleteAddress",
                type: "POST",
                data: {
                    Id: addressId,
                },
                dataType: "JSON",
                success: function (response) {
                    Swal.fire({
                        icon: 'success',
                        title: 'Completado',
                        text: 'La direccion se ha eliminado con exito'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/Client/Addresses";
                        }
                    })
                },
                error: function (response) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'La direccion no se pudo eliminar, ya que tiene compras relacionadas con esta dirección, si desea eliminarla o desvincularla contactese con nosotros, gracias'
                    }).then((result) => {
                        if (result.value) {
                            location.href = "/Client/Addresses";
                        }
                    })
                }
            })
        }
    });
}

function getChekoutInfo() {
    var url = "/Sale/GetCheckout";
    $.get(url, function (response) {
        $("#checkout_info").html(response);
    });
}

function confirmEcommerceOrder() {
    var paymentType = $("#paymentType").val();
    var addressId = $("#addressId").val();
    var totalAmount = $("#TotalAmount").val();
    var checkStockUrl = "/Sale/CheckStock";
    var url = "/Sale/Payment?addressId=" + addressId + "&paymentType=" + paymentType + "&TotalAmount=" + totalAmount;
    Swal.fire({
        title: '¿Seguro que desea continuar?',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        cancelButtonText: 'Cancelar',
        confirmButtonText: 'Confirmar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.get(checkStockUrl, function (response) {
                if (response) {
                    Swal.fire({
                        title: 'Espere procesamos la orden',
                        onBeforeOpen: () => {
                            Swal.showLoading()
                        }
                    });
                    $.post(url, function (response) {
                        if (response) {
                            Swal.close();
                            Swal.fire({
                                icon: 'success',
                                title: 'Tu pedido se realizo con exito',
                                showClass: {
                                    popup: 'animate__animated animate__fadeInDown'
                                },
                                hideClass: {
                                    popup: 'animate__animated animate__fadeOutUp'
                                }
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    location.href = "/Client/MyOrders";
                                }
                            });
                        }
                        else {
                            Swal.close();
                            Swal.fire({
                                icon: 'error',
                                title: 'No se pudo realizar el pedido, vuelva a intentarlo',
                                showClass: {
                                    popup: 'animate__animated animate__fadeInDown'
                                },
                                hideClass: {
                                    popup: 'animate__animated animate__fadeOutUp'
                                }
                            }).then((result) => {
                                if (result.isConfirmed) {
                                    location.href = "/Sale/Checkout";
                                }
                            });
                        }
                    });
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Sin Stock',
                        text: 'Uno de los productos que quiere comprar no hay stock revise de nuevo porfavor',
                    })
                    getChekoutInfo();
                }
            });
        }
    })
}

function pagination() {
    jQuery(function ($) {
        var items = $(".wrapper .panel .panel-body .product-men");
        var numItems = items.length;
        var perPage = 12;
        items.slice(perPage).hide();
        $("#pagination-container").pagination({
            items: numItems,
            itemsOnPage: perPage,
            prevText: 'Anterior',
            nextText: 'Siguiente',
            cssStyle: "light-theme",
            onPageClick: function (pageNumber) {
                var showFrom = perPage * (pageNumber - 1);
                var showTo = showFrom + perPage;
                items.hide().slice(showFrom, showTo).show();
            }
        });
    });
}

function productSearchFilter() {
    (function ($) {
        $('#filtro').keyup(function () {
            var rex = new RegExp($(this).val(), 'i');
            $('.itemsFiltrar').hide();
            $('.itemsFiltrar').filter(function () {
                return rex.test($(this).text());
            }).show();
        })
    }(jQuery));
}

function HomeIndexActiveLink() {
    $('#IndexLink').addClass('active');

    $('#AboutLink').removeClass('active');
    $('#ProductsLink').removeClass('active');
    $('#ContactLink').removeClass('active');
}

function AboutActiveLink() {
    $('#AboutLink').addClass('active');

    $('#IndexLink').removeClass('active');
    $('#ProductsLink').removeClass('active');
    $('#ContactLink').removeClass('active');
}

function ProductsActiveLink() {
    $('#ProductsLink').addClass('active');

    $('#IndexLink').removeClass('active');
    $('#AboutLink').removeClass('active');
    $('#ContactLink').removeClass('active');
}

function ContactActiveLink() {
    $('#ContactLink').addClass('active');

    $('#IndexLink').removeClass('active');
    $('#AboutLink').removeClass('active');
    $('#ProductsLink').removeClass('active');
}

function RemoveActiveLink() {
    $('#IndexLink').removeClass('active');
    $('#AboutLink').removeClass('active');
    $('#ProductsLink').removeClass('active');
    $('#ContactLink').removeClass('active');
}

function searchProduct(evt) {
    var product = $('#selectedProductId').val();
    if (product === '¿Que producto busca?') {
        evt.preventDefault();
    }
    else {
        location.href = "/Product/Detail/" + product;
    }
}