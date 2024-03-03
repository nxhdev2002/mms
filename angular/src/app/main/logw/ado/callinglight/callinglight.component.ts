import { DateTime } from 'luxon';
import { style } from '@angular/animations';
import { QuerySelector } from '@ag-grid-enterprise/all-modules';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { LgwAdoCallingLightStatusDto, LgwAdoCallingLightStatusServiceProxy } from '@shared/service-proxies/service-proxies';
import { getTime } from 'ngx-bootstrap/chronos/utils/date-getters';
import { finalize } from 'rxjs/operators';

@Component({
    templateUrl: './callinglight.component.html',
    styleUrls: ['./callinglight.component.less']
})
export class CallinglightComponent implements OnInit {
    dataScreen: LgwAdoCallingLightStatusDto[] = [];
    prodLine: string = '';
    clearTimeLoadForm;
    clearTimeLoadData;
    countData: number = 0;
    c_DataTop: number = 0;
    blockcol: number = 0;
    blockrow: number = 0;
    isclick: boolean = false;
    // time_test;
    constructor(private _service: LgwAdoCallingLightStatusServiceProxy) {

    }

    ngOnInit(): void {
        let urlParams = new URLSearchParams(window.location.search);
        this.prodLine = urlParams.get('prod_line');
        this.getData();
    }
    timecount: number = 0;
    refeshPage: number = 600;

    timeoutData() {
        try {
            if (this.timecount > this.refeshPage) window.location.reload();
            this.timecount = this.timecount + 1;
            this.getCallingLightData();
        } catch (ex) {
            console.log('1: ' + ex);
            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }

    ngAfterViewInit() {
        setTimeout(() => {
            this.loadForm();
        }, 2000);
        setTimeout(() => {
            this.timeoutData();
        }, 1000);
        console.log('ngAfterViewInit');
    }

    ngOnDestroy(): void {
        clearTimeout(this.clearTimeLoadData);
    }
    
    fornumbersRange(start: number, stop: number, step: number) {
        let numRange: number[] = [];
        for (let i = start; i <= stop;) {
            numRange.push(i);
            i = i + step;
        }
        return numRange;
    }

    getData() {
        this._service.getCallingLightData(this.prodLine)
            .subscribe((result) => {
                this.dataScreen = result ?? [];
                this.countData = this.dataScreen.length;
                this.blockcol = this.dataScreen[0].blockPerLine;
                this.blockrow = Math.ceil(this.countData / this.blockcol);
            });
    }

    getCallingLightData() {
        this._service.getCallingLightData(this.prodLine)
            .subscribe((result) => {
                this.dataScreen = result ?? [];
                this.byData(this.dataScreen);
                this.clearTimeLoadData = setTimeout(() => {
                    this.timeoutData();
                }, 1000);
            }, (error) => {
                console.log(error);
                this.clearTimeLoadData = setTimeout(() => {
                    this.timeoutData();
                }, 1000);
            });
    }
    
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm()
    }

    loadForm() {
        var w = window.innerWidth;
        var ccs_col_process = document.querySelectorAll<HTMLElement>('.body .col_body');
        let w_col_process = w / ccs_col_process.length;
        for (let i = 0; ccs_col_process[i]; i++) {
            ccs_col_process[i].style.width = w_col_process + 'px';
        }
        var h = window.innerHeight * 90 / 100;
        let col_row = document.querySelectorAll<HTMLElement>('.body .col_body .row_body');
        for (let i = 0; col_row[i]; i++) {
            col_row[i].style.height = (h / this.blockrow) + 'px';
        }
    }

    byData(jdata: LgwAdoCallingLightStatusDto[]) {

        jdata.forEach((element) => {
            var _row = Math.ceil(Number(element.sorting) / this.blockcol);
            var _col = Number(element.sorting) - this.blockcol * (_row - 1);
            let item = document.querySelector<HTMLElement>('.body .col_body.col_' + _col);
            let item2 = item.querySelector<HTMLElement>('.body .row_body.row_' + _row);
            if (item2) {
                let lightname = item2.querySelector<HTMLElement>('.main_content');
                if (lightname) {
                    lightname.textContent = element.lightName
                }
                let _time = item2.querySelector<HTMLElement>('.sub_content');
                if(_time){
                    _time.setAttribute("CallingLightId", (element.id) ? element.id.toString() : '');
                }

                if (element.startDate != null) {
                    lightname.style.backgroundColor = 'yellow'
                    lightname.style.borderBottom = 'none'
                    _time.style.backgroundColor = 'yellow'
                    _time.textContent = (element.timeAction) ? element.timeAction.replace(/:000/g, '') : '0:00:00';
                } else {
                    lightname.style.backgroundColor = 'white'
                    lightname.style.borderBottom = '1px solid black'
                    _time.style.backgroundColor = 'white'
                    _time.textContent = '';
                }
            }

            if (Number(element.sorting) == this.countData) {
                for (var i = 1; i <= this.blockcol * this.blockrow - this.countData; i++) {
                    let item = document.querySelector<HTMLElement>('.body .col_body.col_' + (_col + i));
                    let item2 = item.querySelector<HTMLElement>('.body .row_body.row_' + _row);
                    let lightname = item2.querySelector<HTMLElement>('.main_content');
                    let lightname1 = item2.querySelector<HTMLElement>('.sub_content');
                    if (lightname) {
                        lightname.textContent = 'Spare';
                        lightname.style.backgroundColor = 'gray';
                    }
                    if (lightname1) {
                        lightname1.style.backgroundColor = 'gray';
                    }
                }
            }
        })
    }

    onCallingLightClick(cols, rows) {
        var _callingLightId = 0;
        let item = document.querySelector<HTMLElement>('.body .col_body.col_' + cols + ' .row_body.row_' + rows + ' .main_content');
        let _time = document.querySelector<HTMLElement>('.body .col_body.col_' + cols + ' .row_body.row_' + rows + ' .sub_content');
        if (item.textContent != 'Spare') {
            item.style.backgroundColor = 'yellow';
            item.style.borderBottom = 'none';
            _time.style.backgroundColor = 'yellow';
        }
        if (_time) {
            if (_time.hasAttribute('CallingLightId')) {
                _callingLightId = Number(_time.getAttribute('CallingLightId'));
                console.log(_callingLightId);
            } else {
                console.log('The attribute does not exist');
            }
        }
        if (_callingLightId != 0) {
                this._service.updateCallingLightData(_callingLightId.toString())
                            .pipe(finalize(() => { }))
                            .subscribe((result) => { });
            }
        }
    }

