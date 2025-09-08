namespace BACKEND_STORE.Shared.ENTITIES
{
    // ==========================================
    // ============ TABLA LOGS DE AUDITORIA ============
    // ==========================================

    public class Audit_Logs
    {
        public int Id_AuditLog { get; set; }        // ID del registro de auditoría
        public string TableName { get; set; }       // Nombre de la tabla afectada
        public string Operation { get; set; }       // Tipo de operación (INSERT, UPDATE, DELETE)
        public DateTime Timestamp { get; set; }     // Fecha y hora de la operación
        public string PrimaryKey { get; set; }      // Clave primaria afectada
        public string OldValues { get; set; }       // JSON con datos antes del cambio
        public string NewValues { get; set; }       // JSON con datos después del cambio
        public string ColumnNames { get; set; }     // Columnas afectadas
        public int UserId { get; set; }             // ID del usuario que realizó la operación
        public Users ChangedBy { get; set; }         // Usuario relacionado con el registro de auditoría
    }
}
