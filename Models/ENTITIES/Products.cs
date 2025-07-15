namespace BACKEND_STORE.Models.ENTITIES
{
    // ==========================================
    // ============ TABLA PRODUCTO ============
    // ==========================================

    public class Products
    {
        public int Id_Product { get; set; } // ID del producto
        public string product_name { get; set; } // Nombre del producto
        public string product_description { get; set; } // Descripción del producto
        public decimal price { get; set; } // Precio del producto
        public int stock_quantity { get; set; } // Cantidad en stock del producto
        public DateTime created_at { get; set; } // Fecha de creación del producto
        public DateTime? updated_at { get; set; } // Fecha de actualización del producto
        public DateTime? deleted_at { get; set; } // Fecha de eliminación del producto
        public bool is_active { get; set; } // Estado del producto (activo/inactivo)
        public string created_by { get; set; } // Usuario que creó el producto
        public string updated_by { get; set; } // Usuario que actualizó el producto
        public string deleted_by { get; set; } // Usuario que eliminó el producto
    }
}
