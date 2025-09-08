namespace BACKEND_STORE.Shared.ENTITIES
{
    // ==========================================
    // ============ TABLA DETALLE ORDEN ============
    // ==========================================

    public class OrderDetail
    {
        public int Id_OrderDetail { get; set; } // ID del detalle de la orden
        public int order_id { get; set; } // ID de la orden asociada
        public Orders Order { get; set; } // Navegación
        public int product_id { get; set; } // ID del producto asociado
        public Products Product { get; set; } // Navegación
        public int quantity { get; set; } // Cantidad del producto en el detalle de la orden
        public decimal unit_price { get; set; } // Precio unitario del producto en el detalle de la orden

    }
}
