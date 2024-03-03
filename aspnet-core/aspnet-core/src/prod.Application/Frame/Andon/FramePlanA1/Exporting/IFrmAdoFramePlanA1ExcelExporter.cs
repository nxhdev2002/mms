using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Collections.Generic;using prod.Dto;
using System.Threading.Tasks;
using prod.Frame.Andon.Dto;

namespace prod.Frame.Andon.Exporting
{

	public interface IFrmAdoFramePlanA1ExcelExporter : IApplicationService
	{

		FileDto ExportToFile(List<FrmAdoFramePlanA1Dto> frmadoframeplana1);
		FileDto ExportToFileErr(List<FrmAdoFramePlanA1Dto> frameplana1_Err);

    }

}


