import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { PtsAdoBumperGetDataBumperInDto, PtsAdoPaintingDataServiceProxy, PtsBmpPartTypeDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { ConfirmPartAssyModalComponent } from './confirmpart-modal.component';
import { log } from 'console';
import { CommonFunction } from '@app/main/commonfuncton.component';

@Component({
    selector: 'app-bumpersubassy',
    templateUrl: './bumpersubassy.component.html',
    styleUrls: ['./bumpersubassy.component.less']
})
export class BumperSubAssyComponent implements OnInit {

    @ViewChild('popUpConfirm', { static: true }) popUpConfirm: | ConfirmPartAssyModalComponent | undefined;

    dataBumperIn: PtsAdoBumperGetDataBumperInDto[] = [];
    dataPartTypeBumperIn: PtsBmpPartTypeDto[] = [];
    clearTimeLoadData;
    prodLine;
    actPart: number = 0;
    planPart: number = 0;
    dataProgress;
    reMain;
    fn:CommonFunction = new CommonFunction();

    constructor(private _service: PtsAdoPaintingDataServiceProxy) { }

    ngOnInit() {
        let urlParams = new URLSearchParams(window.location.search);
        this.prodLine = urlParams.get('pline');
        this.getDataPartType();
    }

    ngAfterViewInit() {
        this.getDataBumperIn();

        /* phuongdv
            setTimeout(() => {
                this.loadForm();
            }, 1000);
        */
    }
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
    }

    ngOnDestroy(): void {
        clearTimeout(this.clearTimeLoadData);
    }

    /*  phuongdv
        timeoutData() {
            try {

            } catch (ex) {
                console.log('1: ' + ex);

                this.clearTimeLoadData = setTimeout(() => {
                    this.timeoutData();
                }, 1000);
            }
        }
    */

    getDataPartType() {
        this._service
            .getPartTypeBumperIn(this.prodLine, 'Y')
            .pipe(finalize(() => { }))
            .subscribe((result) => {
                this.dataPartTypeBumperIn = result ?? [];
                // console.log(this.dataPartTypeBumperIn);

                setTimeout(() => {
                    this.loadForm();
                }, 1000);
            });
    }

    loadForm() {
        //check vẽ giao diện: chỉ vẽ 1 lần duy nhất
        if(this.dataPartTypeBumperIn) {

            var dom_content = document.querySelector<HTMLElement>('.CONTENT');
            var h_contenpartType = document.querySelectorAll<HTMLElement>('.CONTENT .PARTTYPE .DATA_BUMPER_IN .PART-TYPE');
            var count_PartType = h_contenpartType.length;

            var h_PartType = (dom_content as HTMLElement).offsetHeight; // varible get hight value PART_TYPE
            var h_ValueconPartType = h_PartType / count_PartType; // varible set height value CONTENT_PARTTYPE_VALUE

            for (let i = 0; h_contenpartType[i]; i++) {
                (h_contenpartType[i] as HTMLElement).style.height = h_ValueconPartType - 0.5 + 'px';
                (h_contenpartType[i] as HTMLElement).style.borderBottom = '1px solid #000';
            }
        }
        else {
            setTimeout(() => {
                this.loadForm();
            }, 1000);
        }
        /*  phuongdv
            setTimeout(() => {
                this.loadForm();
            }, 2000);
        */
    }

    fornumbersRange(start: number, stop: number, step: number) {
        let numRange: number[] = [];
        for (let i = start; i <= stop;) {
            numRange.push(i);
            i = i + step;
        }
        return numRange;
    }

    timecount: number = 0;
    refeshPage: number = 300;
    getDataBumperIn() {
        try {

            if (this.timecount > this.refeshPage) window.location.reload();
            this.timecount = this.timecount + 1;
            this.fn.showtime('time_now_log');
            this.fn.loading(true);
            console.log('load-data');
            this._service
                .getBumperGetdataBumperIn(this.prodLine, 'SUB_ASSY')
                .pipe(finalize(() => { }))
                .subscribe((result) => {
                        if (result) {
                            this.dataBumperIn = result?? [];
                            this.countDown(result);
                            this.byData(result);
                            this.fn.loading(false);
                            // this.actPart = this.dataBumperIn[0].actualCount;
                            // this.planPart = this.dataBumperIn[0].planCount;
                        }
                        this.clearTimeLoadData = setTimeout(() => {
                            this.getDataBumperIn();
                        }, 1000);
                    },(error) => {
                        this.clearTimeLoadData = setTimeout(() => {
                            this.getDataBumperIn();
                        }, 1000);
                });
        } catch (ex) {
            console.log('2: ' + ex);
            this.clearTimeLoadData = setTimeout(() => {
                this.getDataBumperIn();
            }, 1000);
        }
    }

    countDown(data) {
        var actualProgress = document.getElementById('ProgressBar');
        var reMain = document.getElementById('Actual');

        var w = window.innerWidth;
        var h = window.innerHeight;
        var hTitle = (h * 10) / 100 - 2;
        var hBoxContent = h - hTitle;
        var heightBox1E2 = (hBoxContent * 25) / 100 - 2;
        var wRemain = (w * 79) / 100;
        reMain.style.marginTop = heightBox1E2 / 2 - 74 + 'px';
        reMain.style.marginLeft = wRemain / 2 - 35 + 'px';

        if (data.length > 0) {
            actualProgress.style.display = 'block';
            reMain.style.display = 'block';

            var planT = data[0].planProgress;
            var actT = data[0].actualProgress;
            //var actT = 1;
            this.reMain = Number(planT - actT);

            actualProgress.style.width = (wRemain / planT) * actT + 'px';
            actualProgress.style.backgroundColor = '#00B050';
        } else {
            actualProgress.style.display = 'none';
            reMain.style.display = 'none';
        }
    }





    cleanData(){
        var parttype = this.dataPartTypeBumperIn
        // console.log('cleanData');
        //clean data
        for(var q = 0 ; q < parttype.length ; q++){
            let item_clear = document.querySelector<HTMLElement>('.CONTENT .PARTTYPE.' + parttype[q].partTypeName.replace(' ', '_'));
                for(var j = 1 ; j < 8 ; j++){
                    let TOP_CLEAN = item_clear.querySelector<HTMLElement>('.UPK_' + j  + ' .TOP_VALUE');
                    let BOT_CLEAN = item_clear.querySelector<HTMLElement>('.UPK_' + j + ' .BOTTOM_VALUE');
                    if(TOP_CLEAN){
                        TOP_CLEAN.textContent = '';
                        BOT_CLEAN.textContent = '';
                    }
                    let RM_COLOR = item_clear.querySelector<HTMLElement>('.UPK_'+ j);
                    if(RM_COLOR){
                    RM_COLOR.style.backgroundColor = 'white';
                    RM_COLOR.classList.remove('flash');
                    }
                    }
                }
    }

    cleanData2() {
        let _done_car = document.querySelector<HTMLElement>('.DONE_CAR');
        if(_done_car) _done_car.textContent = "";
        let _clearValue = document.querySelectorAll<HTMLElement>('.TOP_VALUE, .BOTTOM_VALUE');
        for(let c=0; _clearValue[c]; c++) {
            _clearValue[c].textContent = "";
            _clearValue[c].classList.remove("UNPACKING", "UNPACK", "RECOAT_BPI", "NOT_UNPACK", "UNPACK_RC");
        }
        let _clearBg = document.querySelectorAll<HTMLElement>('.CLEAN_STATUS');
        for(let b=0; _clearBg[b]; b++) {
            _clearBg[b].style.backgroundColor = 'white';
            _clearBg[b].classList.remove('flash');
        }
    }

    byData(jdata: PtsAdoBumperGetDataBumperInDto[]) {
        // this.cleanData();
        // console.log('byData');

        this.cleanData2();

        if(jdata.length > 0) {
            let _done_car = document.querySelector<HTMLElement>('.DONE_CAR');
            if(_done_car) _done_car.textContent = jdata[0].actualCount + "/" + jdata[0].planCount;
        }

        jdata.forEach((element) => {
            if (element.rowNo <= 7) {
                let item = document.querySelector<HTMLElement>('.CONTENT .PARTTYPE.' + element.partTypeName.replace(' ', '_'));
                if (item) {
                    var _row =Number(element.rowNo) < 7 ? element.rowNo : '';

                    let TOP = item.querySelector<HTMLElement>('.UPK_' + _row + ' .TOP_VALUE');
                    let BOT = item.querySelector<HTMLElement>('.UPK_' + _row + ' .BOTTOM_VALUE');
                    let TOP_RECOAT = item.querySelector<HTMLElement>('.UPK_7 .TOP_VALUE');
                    let BOT_RECOAT = item.querySelector<HTMLElement>('.UPK_7 .BOTTOM_VALUE');
                    let TOP_ATT = item.querySelector<HTMLElement>('.UPK_' + element.rowNo  + ' .TOP_VALUE');
                    let COLOR3 = item.querySelector<HTMLElement>('.UPK_3');
                    let COLOR4 = item.querySelector<HTMLElement>('.UPK_4');
                    let COLOR5 = item.querySelector<HTMLElement>('.UPK_5');
                    let COLOR_SKIP2 = item.querySelector<HTMLElement>('.UPK_6');
                    let COLOR_RE = item.querySelector<HTMLElement>('.UPK_7');
                    let COL_RE_ATT = item.querySelector<HTMLElement>('.UPK_7 .TOP_VALUE');


                    //col3
                    let _isPunch = this.fn.isStatus(element.isPunch, "");
                    if (_isPunch == '' && element.rowNo == 3 && element.status == 'PAINTED' && COLOR3) {
                        COLOR3.style.backgroundColor = '#92D050';
                    } else if (_isPunch == 'Y' && element.rowNo == 3 && element.status == 'PAINTED' && COLOR3) {
                        COLOR3.style.backgroundColor = '#92D050';
                        COLOR3.classList.add('flash');
                    }
                    else if (element.rowNo == 3 && element.status == 'NOT_PAINTED' && COLOR3) {
                        COLOR3.style.backgroundColor = '#FFFF00';
                    }

                    //col4
                    if (_isPunch == '' && element.rowNo == 4 && element.status == 'PAINTED' && COLOR4) {
                        COLOR4.style.backgroundColor = '#92D050';
                    } else if (_isPunch == 'Y' && element.rowNo == 4 && element.status == 'PAINTED' && COLOR4) {
                        COLOR4.style.backgroundColor = '#92D050';
                        COLOR4.classList.add('flash');
                    }
                    else if (element.rowNo == 4 && element.status == 'NOT_PAINTED' && COLOR4) {
                        COLOR4.style.backgroundColor = '#FFFF00';
                    }


                    //col5
                    if (_isPunch == '' && element.rowNo == 5 && element.status == 'PAINTED' && COLOR5) {
                        COLOR5.style.backgroundColor = '#92D050';
                    } else if (_isPunch == 'Y' && element.rowNo == 5 && element.status == 'PAINTED' && COLOR5) {
                        COLOR5.style.backgroundColor = '#92D050';
                        COLOR5.classList.add('flash');
                    }
                    else if (element.rowNo == 5 && element.status == 'NOT_PAINTED' && COLOR5) {
                        COLOR5.style.backgroundColor = '#FFFF00';
                    }


                    if (element.displayInfor && element.rowNo == 6  && COLOR_SKIP2) {
                        COLOR_SKIP2.style.backgroundColor = 'red';
                    }


                   // bind data
                   if (element.displayInfor && element.status == 'RECOAT_ASY' && COLOR_RE && TOP_RECOAT && COL_RE_ATT) {
                    COLOR_RE.style.backgroundColor = '#FF66FF';
                    TOP_RECOAT.textContent = element.displayInfor;
                    BOT_RECOAT.textContent = element.bodyNo;
                    COL_RE_ATT.setAttribute("progressId", element.progressId.toString());
                }

                if(TOP && element.status != 'RECOAT_ASY' && element.status != 'COMPLETED'){
                    TOP.textContent = element.displayInfor;
                    TOP.classList.add(element.status);
                    BOT.textContent = element.bodyNo;
                }

                // set attribute
                if(TOP_ATT){
                    var _value = TOP_ATT.textContent;
                    if(_value != '' && element.rowNo != 7){
                        TOP_ATT.setAttribute("progressId", element.progressId.toString());
                    }
                }

                }
            }
        });
    }


    showPopupConfirmPart(v_partType, v_loc, v_ind) {
        var _progressId = 0;
        let item = document.querySelector<HTMLElement>('.CONTENT .PARTTYPE.' + v_partType.replace(' ', '_') + ' .UPK_' + v_loc);
        let attr_progressId = item.querySelector<HTMLElement>('.UPK_' + v_loc + ' .TOP_VALUE');
        if (attr_progressId) {
            if (attr_progressId.hasAttribute('progressId')) {
                _progressId = Number(attr_progressId.getAttribute('progressId'));
            } else {
                console.log('The attribute does not exist');
                return;
            }
        }



        let dataLocalion = this.dataBumperIn.filter(x => x.progressId == _progressId);
        let v_displayInfor = dataLocalion[0].displayInfor;
        let v_bodyNo = dataLocalion[0].bodyNo;
        let v_status = dataLocalion[0].status;
        let v_isPunch = this.fn.isStatus(dataLocalion[0].isPunch, "");

        //show pop confirm
        if (v_ind > 4 && _progressId != 0 ) {
            this.popUpConfirm.show(_progressId, v_displayInfor, v_bodyNo,'confirmRc');
        }

        // update status
        if (v_ind > 1 && v_ind < 5 && _progressId != 0) {

            console.log(v_status + '- '+ v_isPunch + '- ' + _progressId);
            if(v_status != 'PAINTED')
            {
                this._service.updateStatusBumperIn(_progressId.toString()).pipe(finalize(() => {}))
                .subscribe((result) => {
                });
            }
            else if(v_status == 'PAINTED' && v_isPunch == 'Y')
            {
                this._service.updateStatusBumperIn(_progressId.toString()).pipe(finalize(() => {}))
                .subscribe((result) => {
                });
            }
            else if(v_isPunch == '' && v_status == 'PAINTED'){
                this.popUpConfirm.show(_progressId, v_displayInfor, v_bodyNo,'updateStt');
            }

        }
    }



}

