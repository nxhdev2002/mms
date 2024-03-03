namespace prod.Master.Pio.DTO
{
	public class MstLspSupplierInfoExportInput
	{
		public virtual string SupplierCode { get; set; }
		public virtual string DeliveryMethod { get; set; }
		public virtual string DeliveryFrequency { get; set; }
		public virtual string OrderDateType { get; set; }
		public virtual string KeihenType { get; set; }

	}
}
