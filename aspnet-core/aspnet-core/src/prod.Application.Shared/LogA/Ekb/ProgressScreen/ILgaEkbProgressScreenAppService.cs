using Abp.Application.Services; 
using System.Collections.Generic; 
using System.Threading.Tasks;
using prod.Master.LogA.Dto;

namespace prod.LogA.Ekb
{

    public interface ILgaEkbProgressScreenAppService : IApplicationService
    {
        Task<List<MstLgaEkbUserDto>> LgaEkbEkanbanProgressGetPickingMember();
        Task<List<LgaEkbProgressScreenDto>> LgaEkbEkanbanProgressGetDataScreen(string _picking_member, string _mode);
        Task<int> LgaEkbEkanbanProgressUndo(string _prod_line, string _picking_member);
        Task<int> LgaEkbEkanbanProgressNextC(string _prod_line, string _picking_member);
        Task<int> LgaEkbEkanbanProgressCallLD(string _prod_line, string _picking_member);
        Task<int> LgaEkbEkanbanProgressInputQty(string _prod_line, string _picking_member, string _eKanban_id, string _input_qty); 
        Task<int> LgaEkbEkanbanProgressNextPageDel(string _prod_line, string _picking_member, string _ekanban_id, string _mode);
        Task<int> LgaEkbEkanbanProgressFinishDel(string _prod_line, string _picking_member, string _p_mode);
        Task<int> LgaEkbEkanbanProgressConfirmDel(string _prod_line, string _picking_member, string p_id, string _p_mode);

    }

}