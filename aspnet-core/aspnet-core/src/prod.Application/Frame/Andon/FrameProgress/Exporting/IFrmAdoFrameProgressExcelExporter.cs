using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using prod.Dto;
using System.Threading.Tasks;
using prod.Frame.Andon.Dto;

namespace prod.Frame.Andon.Exporting
{

	public interface IFrmAdoFrameProgressExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<FrmAdoFrameProgressDto> frmadoframeprogress);

	}

}


