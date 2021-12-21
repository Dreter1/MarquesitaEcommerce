using Marquesita.Infrastructure.Interfaces;
using System.Collections.Generic;

namespace Marquesita.Infrastructure.Services
{
    public class ConstantsService : IConstantsService
    {
        public struct UserType
        {
            public const string CLIENT = "Cliente";
            public const string ADMINISTRATOR = "Administrator";

            public const string CLIENT_UPPERCASE = "CLIENTE";
            public const string ADMINISTRATOR_UPPERCASE = "ADMINISTRATOR";
        }

        public struct RoleTypes 
        {
            public const string VIEW_USERS = "Ver Usuarios";
            public const string ADD_USER = "Agregar Usuario";
            public const string EDIT_USER = "Editar Usuario";
            public const string DELETE_USER = "Eliminar Usuario";

            public const string VIEW_ROLES = "Ver Roles";
            public const string ADD_ROLE = "Agregar Rol";
            public const string EDIT_ROLE = "Editar Rol";
            public const string DELETE_ROLE = "Eliminar Rol";

            public const string VIEW_PRODUCTS = "Ver Productos";
            public const string ADD_PRODUCT = "Agregar Producto";
            public const string EDIT_PRODUCT = "Editar Producto";
            public const string DELETE_PRODUCT = "Eliminar Producto";

            public const string VIEW_CATEGORYS = "Ver Categorias";
            public const string ADD_CATEGORY = "Agregar Categoria";
            public const string EDIT_CATEGORY = "Editar Categoria";
            public const string DELETE_CATEGORY = "Eliminar Categoria";

            public const string VIEW_SALES = "Ver Ventas";
            public const string ADD_SALE = "Agregar Venta";
            public const string EDIT_SALE = "Editar Venta";


            public const string CLIENT = "Compras";
        }

        public struct InitialsUsers 
        {
            public const string ADMIN_USERNAME = "administrator";
            public const string ADMIN_FIRSTNAME = "Administrator";
            public const string ADMIN_LASTNAME = "Company Name";
            public const string ADMIN_EMAIL = "administrator@company.com";
            public const string ADMIN_EMAIL_UPPERCASE = "ADMINISTRATOR@COMPANY.COM";
            public const string ADMIN_PASSWORD = "LosAlpes@2022";

            public const string CLIENT_USERNAME = "cliente";
            public const string CLIENT_FIRSTNAME = "Client";
            public const string CLIENT_LASTNAME = "Test";
            public const string CLIENT_EMAIL = "cliente@gmail.com";
            public const string CLIENT_EMAIL_UPPERCASE = "CLIENTE@GMAIL.COM";
            public const string CLIENT_PASSWORD = "Cliente@2022";

            public const string PHONE = "123456789";
            public const bool EMAIL_CONFIRMED = true;
            public const bool LOCKOUT_ENABLED = false;
        }

        public struct Images
        {
            public const string IMG_ROUTE_COLABORATOR = "~/Images/Users/Employees/";
            public const string IMG_ROUTE_CLIENT = "~/Images/Users/Clients/";
            public const string IMG_ROUTE_PRODUCT = "~/Images/Products/";
        }

        public struct SaleType
        {
            public const string STORE_SALE = "Tienda";
            public const string ECOMMERCE_SALE = "E-commerce";
        }

        public struct SaleStatus
        {
            public const string IN_PROCESS = "En proceso";
            public const string CONFIRMED = "Confirmada";
            public const string CANCELED = "Cancelada";
        }

        public struct PaymentType
        {
            public const string CASH = "Efectivo";
            public const string DEBIT_CREDIT_CARD = "Tarjeta debito/credito";
            public const string CASH_CARD = "Efectivo y Tarjeta";
        }

        public struct PaymentMethod
        {
            public const string UPON_DELIVERY = "Contra Entrega";
            public const string WIRE_TRANSFER = "Transferencia Bancaria";
            public const string VIRTUAL_WALLET = "Monedero Virtual";
        }

        public struct Messages
        {
            public const string SALE_SUCCESS = "Se realizo la venta con exito";
        }

        public struct Errors
        {
            public const string INVALID_USER = "Usuario o Contraseña Incorrecta";
            public const string INVALID_ACCOUNT = "Cuenta no valida, comuniquese con un administrador";
            public const string INVALID_USER_TYPE = "Cuenta no valida, por favor revise sus credenciales";
            public const string VALIDATE_EMAIL = "Porfavor valide su correo electronico para poder ingresar";
            public const string USER_INACTIVE = "Su cuenta fue desactiva, comuniquese con un administrador";
            public const string STOCK_NOT_AVALIBLE = "No contamos con el stock que solicita, vuelva a intentarlo";
            public const string INVALID_QUANTITY = "Porfavor ingrese la cantidad del producto";
            public const string EMPTY_SALE = "No puede realizar una venta sin productos";
        }

        public struct EmailSubject
        {
            public const string CONFIRM_EMAIL = "Confirma tu correo para La Marquesita";
            public const string FORGOT_PASSWORD = "Cambiar contraseña";
            public const string SALE_CLIENT_CONFIRMATION = "Confirmación de su compra";
        }

        public struct WhatsApp
        {
            public const string LINK = "#";
        }

        public List<string> PermissionList()
        {
            return new List<string>() {
                RoleTypes.VIEW_USERS,
                RoleTypes.ADD_USER,
                RoleTypes.EDIT_USER,
                RoleTypes.DELETE_USER,
                RoleTypes.VIEW_ROLES,
                RoleTypes.ADD_ROLE,
                RoleTypes.EDIT_ROLE,
                RoleTypes.DELETE_ROLE,
                RoleTypes.VIEW_PRODUCTS,
                RoleTypes.ADD_PRODUCT,
                RoleTypes.EDIT_PRODUCT,
                RoleTypes.DELETE_PRODUCT,
                RoleTypes.VIEW_CATEGORYS,
                RoleTypes.ADD_CATEGORY,
                RoleTypes.EDIT_CATEGORY,
                RoleTypes.DELETE_CATEGORY,
                RoleTypes.VIEW_SALES,
                RoleTypes.ADD_SALE,
                RoleTypes.EDIT_SALE,
            };
        }

        public List<string> GetPaymentList()
        {
            return new List<string>() {
                PaymentType.CASH,
                PaymentType.DEBIT_CREDIT_CARD,
                PaymentType.CASH_CARD
            };
        }

        public List<string> GetSaleStatusList()
        {
            return new List<string>() {
                SaleStatus.IN_PROCESS,
                SaleStatus.CONFIRMED,
                SaleStatus.CANCELED
            };
        }

        public List<string> GetEcommercePaymentList()
        {
            return new List<string>() {
                PaymentMethod.UPON_DELIVERY,
                PaymentMethod.WIRE_TRANSFER,
                PaymentMethod.VIRTUAL_WALLET
            };
        }
    }
}
