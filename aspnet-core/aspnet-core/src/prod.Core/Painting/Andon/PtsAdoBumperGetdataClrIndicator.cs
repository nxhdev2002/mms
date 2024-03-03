using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Painting.Andon
{
    

		[Table("PtsAdoBumperGetdataClrIndicator")]
		[Index(nameof(BodyNo), Name = "IX_PtsAdoBumperGetdataClrIndicator_BodyNo")]
		[Index(nameof(LotNo), Name = "IX_PtsAdoBumperGetdataClrIndicator_LotNo")]
		[Index(nameof(Model), Name = "IX_PtsAdoBumperGetdataClrIndicator_Model")]
		[Index(nameof(Grade), Name = "IX_PtsAdoBumperGetdataClrIndicator_Grade")]
		[Index(nameof(Color), Name = "IX_PtsAdoBumperGetdataClrIndicator_Color")]
		[Index(nameof(OrderDate), Name = "IX_PtsAdoBumperGetdataClrIndicator_OrderDate")]
		[Index(nameof(Line), Name = "IX_PtsAdoBumperGetdataClrIndicator_Line")]
		[Index(nameof(Process), Name = "IX_PtsAdoBumperGetdataClrIndicator_Process")]
	public class PtsAdoBumperGetdataClrIndicator : FullAuditedEntity<long>, IEntity<long>
		{

		public virtual string BodyNo { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string Model { get; set; }

		public virtual string Grade { get; set; }

		public virtual string Color { get; set; }

		[Column(TypeName = "datetime2(7)")]
		public virtual DateTime? OrderDate { get; set; }

		public virtual string Line { get; set; }

		public virtual string Process { get; set; }
	}
}
