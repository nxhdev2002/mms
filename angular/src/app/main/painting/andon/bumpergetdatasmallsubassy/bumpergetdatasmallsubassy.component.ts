import { DatePipe } from '@angular/common';
import { Component, HostListener, Injector, OnInit } from '@angular/core';
import { PtsAdoBumperGetDataSmallSubassyDto, PtsAdoBumperServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
    selector: 'app-bumpergetdatasmallsubassy',
    templateUrl: './bumpergetdatasmallsubassy.component.html',
    styleUrls: ['./bumpergetdatasmallsubassy.component.less'],
})
export class BumpergetdatasmallsubassyComponent implements OnInit {
    dataBumperSmallSubassy: any[] = [];
    pipe = new DatePipe('en-US');
    clearTimeLoadData;

    constructor(private _service: PtsAdoBumperServiceProxy) {}

    ngOnInit() {
        this.loadForm();
        console.log('ngOnInit');
    }

    ngAfterViewInit() {
        this.timeoutData();
        console.log('ngAfterViewInit');
    }

    ngOnDestroy(): void {
        clearTimeout(this.clearTimeLoadData);
    }

    timecount: number = 0;
    refeshPage: number = 600;
    timeoutData() {
        try {
            if (this.timecount > this.refeshPage) window.location.reload();
            this.timecount = this.timecount + 1;

            this.getData();
        } catch (ex) {
            console.log('1: ' + ex);

            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }

    getData() {
        this._service.getBumperDataSmallSubassy().pipe(finalize(() => {}))
            .subscribe((result) => {
                try {
                    this.dataBumperSmallSubassy = result.items ?? [];

                    this.bindataBumperSmallSubassy();
                    this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 1000);
                } catch (ex) {
                    console.log('2: ' + ex);

                    this.clearTimeLoadData = setTimeout(() => {
                        this.timeoutData();
                    }, 1000);
                }
            });
    }

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
    }

    loadForm() {
        var w = window.innerWidth;
        var h = window.innerHeight;
        var wCol6 = (w / 15) * 100; // with 6 cot
        var hRow_2 = h / 2; //height 2 khoang a1 - a2
        var hRow_a_0 = ((h / 2) * 10) / 100; //height hang A0
        var hRow = hRow_2 - hRow_a_0; // height hang A1
        var hRow_A1_P1_ALL = (hRow_2 - hRow_a_0 * 2) / 4; //height hang A1

        var PTA = document.querySelectorAll<HTMLElement>('.PTA');
        (PTA[0] as HTMLElement).style.width = w + 'px';
        (PTA[0] as HTMLElement).style.height = h + 'px';

        var PTA_PROCESS_Content_P6 = document.querySelectorAll<HTMLElement>(
            '.PTA .PTA_PROCESS_Content .a1 .p1 .p2,' +
                ' .PTA .PTA_PROCESS_Content .a3 .p1 .p2,' +
                ' .PTA .PTA_PROCESS_Content .a1 .p1 .p4,' +
                ' .PTA .PTA_PROCESS_Content .a3 .p1 .p4,' +
                ' .PTA .PTA_PROCESS_Content .a1 .p1 .p6,' +
                ' .PTA .PTA_PROCESS_Content .a3 .p1 .p6'
        );
        for (let i = 0; PTA_PROCESS_Content_P6[i]; i++) {
            (PTA_PROCESS_Content_P6[i] as HTMLElement).style.fontWeight = '900';
        }

        //get set class a0
        var PTA_PROCESS_Content_A0 = document.querySelectorAll<HTMLElement>('.PTA .PTA_PROCESS_Content .a0');
        for (let i = 0; PTA_PROCESS_Content_A0[i]; i++) {
            (PTA_PROCESS_Content_A0[i] as HTMLElement).style.width = w + 'px';
            (PTA_PROCESS_Content_A0[i] as HTMLElement).style.height = hRow_a_0 + 'px';
        }

        //get class a2
        var PTA_PROCESS_Content_A2 = document.querySelectorAll<HTMLElement>('.PTA .PTA_PROCESS_Content .a2');
        for (let i = 0; PTA_PROCESS_Content_A2[i]; i++) {
            (PTA_PROCESS_Content_A2[i] as HTMLElement).style.width = w + 'px';
            (PTA_PROCESS_Content_A2[i] as HTMLElement).style.height = hRow_a_0 + 'px';
        }

        //get set class a1,a3
        var PTA_PROCESS_Content_A0 = document.querySelectorAll<HTMLElement>(
            '.PTA .PTA_PROCESS_Content .a1,.PTA .PTA_PROCESS_Content .a3'
        );
        for (let i = 0; PTA_PROCESS_Content_A0[i]; i++) {
            (PTA_PROCESS_Content_A0[i] as HTMLElement).style.width = w + 'px';
            (PTA_PROCESS_Content_A0[i] as HTMLElement).style.height = hRow + 'px';
        }

        //get set  class a1 p1 and a3 p1
        var PTA_PROCESS_Content_A2 = document.querySelectorAll<HTMLElement>(
            '.PTA .PTA_PROCESS_Content .a1 .p1,.PTA .PTA_PROCESS_Content .a3 .p1'
        );
        for (let i = 0; PTA_PROCESS_Content_A2[i]; i++) {
            (PTA_PROCESS_Content_A2[i] as HTMLElement).style.width = wCol6 + 'px';
            (PTA_PROCESS_Content_A2[i] as HTMLElement).style.height = hRow + 'px';
        }

        // get class a1 p1 PROCESS
        var PTA_PROCESS_Content_A1_P1_PROCESS = document.querySelectorAll<HTMLElement>(
            '.PTA .PTA_PROCESS_Content .a1 .p1 .PROCESS, ' + '.PTA .PTA_PROCESS_Content .a3 .p1 .PROCESS'
        );
        for (let i = 0; PTA_PROCESS_Content_A1_P1_PROCESS[i]; i++) {
            (PTA_PROCESS_Content_A1_P1_PROCESS[i] as HTMLElement).style.height = hRow_a_0 + 'px';
        }

        //set width box in a1
        for (let i = 0; i <= 5; i++) {
            var MODEL_T = 'MODEL' + i;
            var PTA_PROCESS_Content_A1_P1_ALL = document.querySelectorAll<HTMLElement>(
                '.PTA .PTA_PROCESS_Content  .a1 .p1   .' +
                    MODEL_T +
                    ' .GRADE , ' +
                    '.PTA .PTA_PROCESS_Content .a1 .p1  .' +
                    MODEL_T +
                    ' .COLOR , ' +
                    '.PTA .PTA_PROCESS_Content .a1 .p1  .' +
                    MODEL_T +
                    ' .BODY_NO ,' +
                    '.PTA .PTA_PROCESS_Content .a1 .p1  .' +
                    MODEL_T +
                    ' .SEQ ,' +
                    '.PTA .PTA_PROCESS_Content .a3 .p1  .' +
                    MODEL_T +
                    ' .GRADE ,' +
                    '.PTA .PTA_PROCESS_Content .a3 .p1  .' +
                    MODEL_T +
                    ' .COLOR , ' +
                    '.PTA .PTA_PROCESS_Content .a3 .p1  .' +
                    MODEL_T +
                    ' .BODY_NO , ' +
                    '.PTA .PTA_PROCESS_Content .a3 .p1  .' +
                    MODEL_T +
                    ' .SEQ'
            );

            for (let i = 0; PTA_PROCESS_Content_A1_P1_ALL[i]; i++) {
                (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.height = hRow_A1_P1_ALL + 'px';
                (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.borderTop = '1px solid #000';
                (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.fontSize = '50px';
                (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.fontWeight = 'revert';
                (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.display = 'flex';
                (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.justifyContent = 'center';
                (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.alignItems = 'center';
                (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.textAlign = 'center';
            }
        }
    }

    bindataBumperSmallSubassy() {
        this.dataBumperSmallSubassy.forEach((element) => {
            var bindataSmallSubassy = element as PtsAdoBumperGetDataSmallSubassyDto;
            var $$ = document.querySelectorAll.bind(document);

            let getClassprocessA1 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .PROCESS');

            for (let j = 0; j <= 5; j++) {
                let i = j + 1;
                if (getClassprocessA1 != null && bindataSmallSubassy.line == 'LINE_1_' + i) {
                    getClassprocessA1[j].innerHTML = bindataSmallSubassy.process;
                }
            }
            // get class a1 Model0
            let getClassGRADE0 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL0 .GRADE');
            let getClassCOLOR0 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL0 .COLOR');
            let getClassBODY_NO0 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL0 .BODY_NO');
            let getClassSEQ0 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL0 .SEQ');

            if (getClassprocessA1 != null && bindataSmallSubassy.line == 'LINE_1_1') {
                getClassBODY_NO0[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE0[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR0[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ0[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL0', bindataSmallSubassy.model);
            }
            // get class a1 Model1
            let getClassGRADE1 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL1 .GRADE');
            let getClassCOLOR1 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL1 .COLOR');
            let getClassBODY_NO1 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL1 .BODY_NO');
            let getClassSEQ1 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL1 .SEQ');

            if (getClassprocessA1 != null && bindataSmallSubassy.line == 'LINE_1_2') {
                getClassBODY_NO1[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE1[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR1[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ1[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL1', bindataSmallSubassy.model);
            }
            // get class a1 Model2
            let getClassGRADE2 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL2 .GRADE');
            let getClassCOLOR2 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL2 .COLOR');
            let getClassBODY_NO2 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL2 .BODY_NO');
            let getClassSEQ2 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL2 .SEQ');

            if (getClassprocessA1 != null && bindataSmallSubassy.line == 'LINE_1_3') {
                getClassBODY_NO2[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE2[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR2[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ2[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL2', bindataSmallSubassy.model);
            }
            // get class a1 Model3
            let getClassGRADE3 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL3 .GRADE');
            let getClassCOLOR3 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL3 .COLOR');
            let getClassBODY_NO3 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL3 .BODY_NO');
            let getClassSEQ3 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL3 .SEQ');

            if (getClassprocessA1 != null && bindataSmallSubassy.line == 'LINE_1_4') {
                getClassBODY_NO3[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE3[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR3[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ3[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL3', bindataSmallSubassy.model);
            }
            // get class a1 Model4
            let getClassGRADE4 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL4 .GRADE');
            let getClassCOLOR4 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL4 .COLOR');
            let getClassBODY_NO4 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL4 .BODY_NO');
            let getClassSEQ4 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL4 .SEQ');

            if (getClassprocessA1 != null && bindataSmallSubassy.line == 'LINE_1_5') {
                getClassBODY_NO4[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE4[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR4[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ4[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL4', bindataSmallSubassy.model);
            }
            // get class a1 Model5
            let getClassGRADE5 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL5 .GRADE');
            let getClassCOLOR5 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL5 .COLOR');
            let getClassBODY_NO5 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL5 .BODY_NO');
            let getClassSEQ5 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL5 .SEQ');

            if (getClassprocessA1 != null && bindataSmallSubassy.line == 'LINE_1_6') {
                getClassBODY_NO5[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE5[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR5[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ5[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL5', bindataSmallSubassy.model);
            }

            let getClassprocessA2 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .PROCESS');
            //bind data
            for (let j = 0; j <= 5; j++) {
                let i = j + 1;
                if (getClassprocessA2 != null && bindataSmallSubassy.line == 'LINE_2_' + i) {
                    getClassprocessA2[j].innerHTML = bindataSmallSubassy.process;
                }
            }

            //get class a3 Model0

            let getClassGRADE0_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL0 .GRADE');
            let getClassCOLOR0_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL0 .COLOR');
            let getClassBODY_NO0_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL0 .BODY_NO');
            let getClassSEQ0_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL0 .SEQ');

            if (getClassprocessA2 != null && bindataSmallSubassy.line == 'LINE_2_1') {
                getClassBODY_NO0_A3[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE0_A3[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR0_A3[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ0_A3[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL0', bindataSmallSubassy.model);
            }

            //get class a3 Model1

            let getClassGRADE1_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL1 .GRADE');
            let getClassCOLOR1_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL1 .COLOR');
            let getClassBODY_NO1_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL1 .BODY_NO');
            let getClassSEQ1_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL1 .SEQ');

            if (getClassprocessA2 != null && bindataSmallSubassy.line == 'LINE_2_2') {
                getClassBODY_NO1_A3[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE1_A3[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR1_A3[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ1_A3[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL1', bindataSmallSubassy.model);
            }

            //get class a3 Model2
            let getClassGRADE2_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL2 .GRADE');
            let getClassCOLOR2_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL2 .COLOR');
            let getClassBODY_NO2_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL2 .BODY_NO');
            let getClassSEQ2_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL2 .SEQ');

            if (getClassprocessA2 != null && bindataSmallSubassy.line == 'LINE_2_3') {
                getClassBODY_NO2_A3[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE2_A3[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR2_A3[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ2_A3[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL2', bindataSmallSubassy.model);
            }

            //get class a3 Model3
            let getClassGRADE3_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL3 .GRADE');
            let getClassCOLOR3_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL3 .COLOR');
            let getClassBODY_NO3_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL3 .BODY_NO');
            let getClassSEQ3_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL3 .SEQ');

            if (getClassprocessA2 != null && bindataSmallSubassy.line == 'LINE_2_4') {
                getClassBODY_NO3_A3[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE3_A3[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR3_A3[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ3_A3[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL3', bindataSmallSubassy.model);
            }

            //get class a3 Model4
            let getClassGRADE4_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL4 .GRADE');
            let getClassCOLOR4_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL4 .COLOR');
            let getClassBODY_NO4_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL4 .BODY_NO');
            let getClassSEQ4_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL4 .SEQ');

            if (getClassprocessA2 != null && bindataSmallSubassy.line == 'LINE_2_5') {
                getClassBODY_NO4_A3[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE4_A3[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR4_A3[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ4_A3[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL4', bindataSmallSubassy.model);
            }
            //get class a3 Model5
            let getClassGRADE5_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL5 .GRADE');
            let getClassCOLOR5_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL5 .COLOR');
            let getClassBODY_NO5_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL5 .BODY_NO');
            let getClassSEQ5_A3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL5 .SEQ');

            if (getClassprocessA2 != null && bindataSmallSubassy.line == 'LINE_2_6') {
                getClassBODY_NO5_A3[0].innerHTML = bindataSmallSubassy.bodyNo;
                getClassGRADE5_A3[0].innerHTML = bindataSmallSubassy.grade;
                getClassCOLOR5_A3[0].innerHTML = bindataSmallSubassy.color;
                getClassSEQ5_A3[0].innerHTML = bindataSmallSubassy.carSequence;
                this.setColor('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL5', bindataSmallSubassy.model);
            }
        });
    }
    setColor(obj: any, model) {
        let a = document.querySelectorAll(obj);
        switch (model) {
            case 'K':
                for (let i = 0; i < a.length; i++) {
                    a[i].style.backgroundColor = '#FE97CA';
                }
                break;
            case 'F':
                for (let i = 0; i < a.length; i++) {
                    a[i].style.backgroundColor = '#FFFF04';
                }
                break;
            case 'C':
                for (let i = 0; i < a.length; i++) {
                    a[i].style.backgroundColor = '#8BB2E1';
                }
                '#8BB2E1';
                break;
            case 'V':
                for (let i = 0; i < a.length; i++) {
                    a[i].style.backgroundColor = '#F9BD8D';
                }
                break;
            case 'I':
                for (let i = 0; i < a.length; i++) {
                    a[i].style.backgroundColor = '#90CE50';
                }
                break;
            default:
                for (let i = 0; i < a.length; i++) {
                    a[i].style.backgroundColor = 'transparent';
                }
        }
    }
}
