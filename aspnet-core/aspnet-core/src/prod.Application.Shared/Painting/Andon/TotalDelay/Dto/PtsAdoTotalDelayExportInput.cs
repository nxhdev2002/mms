using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoTotalDelayExportInput{

		public virtual string BodyNo { get; set; }

		public virtual string LotNo { get; set; }


		public virtual string Mode { get; set; }

		
	}

}


