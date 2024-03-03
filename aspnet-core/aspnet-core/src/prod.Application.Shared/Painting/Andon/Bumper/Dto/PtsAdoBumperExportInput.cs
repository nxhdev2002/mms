using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoBumperExportInput
	{

		public virtual long? WipId { get; set; }

		public virtual string Model { get; set; }

		public virtual string Grade { get; set; }

		public virtual string BodyNo { get; set; }

		public virtual string LotNo { get; set; }

		public virtual int? NoInLot { get; set; }

		public virtual string Color { get; set; }

		public virtual int? BumperStatus { get; set; }

		public virtual DateTime? InitialDate { get; set; }

		public virtual DateTime? I1Date { get; set; }

		public virtual DateTime? I2Date { get; set; }

		public virtual DateTime? InlInDate { get; set; }

		public virtual DateTime? InlOutDate { get; set; }

		public virtual DateTime? PreparationDate { get; set; }

		public virtual DateTime? UnpackingDate { get; set; }

		public virtual DateTime? JigSettingDate { get; set; }

		public virtual DateTime? BoothDate { get; set; }

		public virtual DateTime? FinishedDate { get; set; }

		public virtual int? ExtSeq { get; set; }

		public virtual int? UnpSeq { get; set; }

		public virtual string IsActive { get; set; }

	}

}


