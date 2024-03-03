using Abp.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;
using prod.Master.LogA.Dto; 

namespace prod.LogA.Ekb
{ 
    public class LgaEkbProgressScreenAppService : prodAppServiceBase, ILgaEkbProgressScreenAppService
    {
        private readonly IDapperRepository<LgaEkbProgress, long> _dapperRepo; 

        public LgaEkbProgressScreenAppService(IDapperRepository<LgaEkbProgress, long> dapperRepo)
        {
            _dapperRepo = dapperRepo; 
        }


        public async Task<List<MstLgaEkbUserDto>> LgaEkbEkanbanProgressGetPickingMember()
        {
            string _sql = " SELECT * FROM MstLgaEkbUser WHERE IsActive = 'Y'";

            var filtered = await _dapperRepo.QueryAsync<MstLgaEkbUserDto>(_sql, new
            {
            });  
            return filtered.ToList();
        }


        public async Task<List<LgaEkbProgressScreenDto>> LgaEkbEkanbanProgressGetDataScreen(string _picking_member, string _mode)
        {
            string _sql = "Exec  LGA_EKB_EKANBAN_PROGRESS_GET_DATA @p_picking_member, @p_mode";

            var filtered = await _dapperRepo.QueryAsync<LgaEkbProgressScreenDto>(_sql, new {
                p_picking_member = _picking_member,
                p_mode = _mode
            });

            return filtered.ToList();
        }

        public async Task<int>  LgaEkbEkanbanProgressNextPageDel(string _prod_line, string _picking_member, string _ekanban_id, string _mode)
        {  
            try
            {
                string _sql = "Exec  LGA_EKB_EKANBAN_PROGRESS_NEXT_PAGE @p_prod_line, @p_picking_member, @p_id, @p_mode";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_prod_line = _prod_line,
                    p_picking_member = _picking_member,
                    p_id = _ekanban_id,
                    p_mode = _mode
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        public async Task<int> LgaEkbEkanbanProgressUndo(string _prod_line, string _picking_member)
        {
            try
            {
                string _sql = @"EXEC LGA_EKB_EKANBAN_PROGRESS_UNDO @p_prod_line, @p_picking_member";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_prod_line = _prod_line,
                    p_picking_member = _picking_member
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }



        public async Task<int> LgaEkbEkanbanProgressFinishDel(string _prod_line, string _picking_member, string _p_mode)
        {
            try
            {
                string _sql = @"EXEC LGA_EKB_EKANBAN_PROGRESS_FINISH_DEL @p_prod_line, @p_picking_member, @p_mode";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_prod_line = _prod_line,
                    p_picking_member = _picking_member,
                    p_mode = _p_mode
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }


        public async Task<int> LgaEkbEkanbanProgressConfirmDel(string _prod_line, string _picking_member, string _p_id, string _p_mode)
        {
            try
            {
                string _sql = @"EXEC LGA_EKB_EKANBAN_PROGRESS_CONFIRM_DEL @p_prod_line, @p_picking_member, @p_id, @p_mode";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_prod_line = _prod_line,
                    p_picking_member = _picking_member,
                    p_id = _p_id,
                    p_mode = _p_mode
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }


        public async Task<int> LgaEkbEkanbanProgressNextC(string _prod_line, string _picking_member)
        {
            try
            {
                string _sql = @"EXEC LGA_EKB_EKANBAN_PROGRESS_NEXTC @p_prod_line, @p_picking_member";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_prod_line = _prod_line,
                    p_picking_member = _picking_member
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        public async Task<int> LgaEkbEkanbanProgressCallLD(string _prod_line, string _picking_member)
        {
            try
            {
                string _sql = @"EXEC LGA_EKB_EKANBAN_PROGRESS_CALL_LD @p_prod_line, @p_picking_member";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_prod_line = _prod_line,
                    p_picking_member = _picking_member
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        public async Task<int> LgaEkbEkanbanProgressInputQty(string _prod_line, string _picking_member, string _eKanban_id, string _input_qty)
        {
            try
            {
                string _sql = @"EXEC LGA_EKB_EKANBAN_PROGRESS_INPUT_QTY @p_prod_line, @p_picking_member, @p_id, @p_input_qty";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_prod_line = _prod_line,
                    p_picking_member = _picking_member,
                    p_id = _eKanban_id,
                    p_input_qty = _input_qty
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }


    }
}
