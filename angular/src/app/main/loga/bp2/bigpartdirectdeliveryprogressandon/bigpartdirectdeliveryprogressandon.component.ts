import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LgaBp2BigPartDDLServiceProxy, LgaBp2BigPartDDpDto } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';
import { CommonFunction } from "@app/main/commonfuncton.component";

@Component({
    selector: 'app-bigpartdirectdeliveryprogressandon',
    templateUrl: './bigpartdirectdeliveryprogressandon.component.html',
    styleUrls: ['./bigpartdirectdeliveryprogressandon.component.less'],
})
export class BigPartDirectDeliveryProgressAndonComponent implements OnInit {
    dataBigPartDDL: LgaBp2BigPartDDpDto[] = [];
    prodLine: string = '';
    reMain: number = 0;
    hContentBottom1: number;
    clearTimeCountDown;
    clearTimeLoadForm;
    isFirsttime:boolean =false;
    fn:CommonFunction = new CommonFunction();

    constructor(private activatedRoute: ActivatedRoute, private _service: LgaBp2BigPartDDLServiceProxy) { }

    ngOnInit() {
       // this.prodLine = this.activatedRoute.snapshot.queryParams['pline'] || '';
        let urlParams = new URLSearchParams(window.location.search);
        this.prodLine = urlParams.get('pline');

        // this.getDataBigPartDDL();
         this.loadForm();
         //console.log('ngOnInit');

    }
    ngAfterViewInit() {
        this.timeoutData();
    }

    ngOnDestroy(): void {
        clearTimeout(this.clearTimeLoadForm);
    }

    timecount: number = 0;
    refeshPage: number = 600;
    timeoutData() {
        try {
            if (this.timecount > this.refeshPage) window.location.reload();
            this.timecount = this.timecount + 1;

            this.getDataBigPartDDL();
            this.fn.showtime('time_now_log');

        } catch (ex) {
            console.log('1: ' + ex);

            this.clearTimeLoadForm = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
    }

    getDataBigPartDDL() {
        this._service .getDataBigPartDDL(this.prodLine).pipe(finalize(() => {}))
            .subscribe((result) => {
                try {
                    this.dataBigPartDDL = result.items ?? [];
                    this.loadData();
                    this.countDown();

                    this.clearTimeLoadForm = setTimeout(() => {
                        this.timeoutData();
                    }, 1000);

                } catch (ex) {
                    console.log('2: ' + ex);

                    this.clearTimeLoadForm = setTimeout(() => {
                        this.timeoutData();
                    }, 1000);
                }
            },(error) => {
                console.log(error);
                this.clearTimeLoadForm = setTimeout(() => {
                    this.timeoutData();
                }, 1000);
            });
    }

    loadForm() {
        var w = window.innerWidth;
        var h = window.innerHeight;
        var hTitle = (h * 10) / 100 - 2;
        var hContentTop = (h * 30) / 100 - 2;
        var hContentTopAct = hContentTop / 2;
        var hContentBottom = h - hTitle - hContentTop;
        this.hContentBottom1 = hContentBottom;
        //2E
        var hContent = (h - hTitle) / 3;

        var BIGPART = document.querySelectorAll<HTMLElement>('.BIGPART');
        (BIGPART[0] as HTMLElement).style.width = w + 'px';
        (BIGPART[0] as HTMLElement).style.height = h + 'px';

        var TITLE = document.querySelectorAll<HTMLElement>('.BIGPART .TITLE');
        (TITLE[0] as HTMLElement).style.height = hTitle + 'px';

        var CTTOP = document.querySelectorAll<HTMLElement>('.BIGPART .CONTENT_TOP');
        (CTTOP[0] as HTMLElement).style.height = hContentTop + 'px';

        var CTTOPA = document.querySelectorAll<HTMLElement>('.BIGPART .CONTENT_TOP .NUMCAR .ACT,'
            + '.BIGPART .CONTENT_TOP .NUMCAR .PLAN'
        );
        for (let i = 0; CTTOPA[i]; i++) {
            (CTTOPA[i] as HTMLElement).style.height = hContentTopAct + 'px';
        }

        var CTBOTTOM = document.querySelectorAll<HTMLElement>('.BIGPART .CONTENT_BOTTOM_1E');
        (CTBOTTOM[0] as HTMLElement).style.height = hContentBottom + 'px';
        (CTBOTTOM[0] as HTMLElement).style.width = w + 'px';

        var HeightPK = document.querySelectorAll<HTMLElement>('.PK_CT');
        (HeightPK[0] as HTMLElement).style.height = hContentBottom + 'px';

        var HeightPK = document.querySelectorAll<HTMLElement>('.PK');
        (HeightPK[0] as HTMLElement).style.height = hContentBottom + 'px';

        var HeightDEL = document.querySelectorAll<HTMLElement>('.DEL');
        (HeightDEL[0] as HTMLElement).style.height = hContentBottom + 'px';

        var HeightProgress = document.getElementById('progressBar');
        HeightProgress.style.height = hContentTop + 'px';

        //2E
        var CTBOTTOM2E = document.querySelectorAll<HTMLElement>('.BIGPART .CONTENT_BOTTOM_2E');
        (CTBOTTOM2E[0] as HTMLElement).style.height = hContentBottom + 'px';
        (CTBOTTOM2E[0] as HTMLElement).style.width = w + 'px';

        var CONTENT = document.querySelectorAll<HTMLElement>('.BIGPART .CONTENT_BOTTOM_2E .CONTENT');
        for (let i = 0; CONTENT[i]; i++) {
            (CONTENT[i] as HTMLElement).style.width = w + 'px';
            (CONTENT[i] as HTMLElement).style.height = hContent + 'px';
        }

        var CONTENT_ITEM = document.querySelectorAll<HTMLElement>('.E-NAME1,'
            + '.E-NAME2,'
            + '.FINISHEDPK,'
            + '.FINISHEDDEL,'
            + '.PK_2E,'
            + '.PK_CT_2E,'
            + '.DEL_2E,'
            + '.DEL_CT_2E'
        );
        for (let i = 0; CONTENT_ITEM[i]; i++) {
            (CONTENT_ITEM[i] as HTMLElement).style.height = hContent + 'px';
        }
    }


    countDown() {
        var actualTRIM = document.getElementById('progressBar');
        var reMain = document.getElementById('PLAN');

        var w = window.innerWidth;
        var h = window.innerHeight;
        var hTitle = (h * 10) / 100 - 2;

        var hBoxContent = h - hTitle;
        var heightBox1E2 = (hBoxContent * 25) / 100 - 2;
        var wRemain = (w * 79) / 100;
        reMain.style.marginTop = heightBox1E2 / 2 - 74 + 'px';
        reMain.style.marginLeft = wRemain / 2 - 35 + 'px';
        var datatime = this.dataBigPartDDL;

        if (datatime.length > 0) {
            actualTRIM.style.display = 'block';
            reMain.style.display = 'block';

            var planT = datatime[0].planTrim;
            var actT = datatime[0].actualTrim;
            //var actT = 1;
            this.reMain = planT - actT;

            actualTRIM.style.width = (wRemain / planT) * actT + 'px';
            actualTRIM.style.backgroundColor = '#00B050';
        } else {
            actualTRIM.style.display = 'none';
            reMain.style.display = 'none';
            // TaktTime[0].style.marginLeft = '-100%';
        }
    }

    loadData() {

        if (this.dataBigPartDDL.length > 0 && !this.isFirsttime) {
            var Ecar1 = this.dataBigPartDDL[0].ecarName;
            var Ecar2 = this.dataBigPartDDL[1].ecarName;
            if (Ecar1 == Ecar2) {
                var e1 = document.getElementById('Contend1E');
                e1.style.visibility = 'visible';
            } else {
                var e2 = document.getElementById('Contend2E');
                e2.style.marginTop = -this.hContentBottom1 + 'px';
                e2.style.visibility = 'visible';
            }
            this.isFirsttime = true;
        }

        //load data
        let htmTitle = document.querySelector('.BIGPART .TITLE');
        let htmPLINE = document.querySelector('.PLINE');
        let htmECAR = document.querySelector('.ECAR');
        let htmACT = document.querySelector('.ACT');
        let htmPLAN = document.querySelector('.PLAN');
        let htmPK = document.querySelector('.PK_CT');
        let htmDEL = document.querySelector('.DEL');
        let htmFINSH = document.querySelector('.FINISHED');

        //2E
        let htmECAR_PK_2E = document.querySelector('.E-NAME1');
        let htmECAR_DEL_2E = document.querySelector('.E-NAME2');
        let htmPK_2E = document.querySelector('.PK_CT_2E');
        let htmDEL_2E = document.querySelector('.DEL_CT_2E');
        let htmFINISHEDPK = document.querySelector('.FINISHEDPK');
        let htmFINISHEDDEL = document.querySelector('.FINISHEDDEL');

        var data = this.dataBigPartDDL;
        var dataPK = this.dataBigPartDDL.filter((p) => p.processName === 'PK');
        var dataDEL = this.dataBigPartDDL.filter((d) => d.processName === 'DEL');

        if (dataPK.length > 0 && dataDEL.length > 0 && data.length > 0) {
            htmTitle.innerHTML = data[0].title;
            htmPLINE.innerHTML = data[0].prodLine;
            htmECAR.innerHTML = data[0].ecarName;
            htmACT.innerHTML = data[0].actualVolCount.toString();
            htmPLAN.innerHTML = data[0].planVolCount.toString();
            htmPK.innerHTML = dataPK[0].processName.toString();
            htmDEL.innerHTML = dataDEL[0].processName.toString();

            //2E
            htmECAR_PK_2E.innerHTML = dataPK[0].ecarName;
            htmECAR_DEL_2E.innerHTML = dataDEL[0].ecarName;
            htmPK_2E.innerHTML = dataPK[0].processName;
            htmDEL_2E.innerHTML = dataDEL[0].processName;
            htmFINISHEDPK.innerHTML = dataPK[0].ecarCount.toString();
            htmFINISHEDDEL.innerHTML = dataDEL[0].ecarCount.toString();

            var sumTotalTaktTime = data[0].totalTaktTime + data[1].totalTaktTime;
            var whtmPK = (dataPK[0].totalTaktTime / sumTotalTaktTime) * 100;
            var whtmDEL = (dataDEL[0].totalTaktTime / sumTotalTaktTime) * 100;

            var WidthPK = document.querySelectorAll<HTMLElement>('.PK_CT');
            (WidthPK[0] as HTMLElement).style.width = whtmPK + '%';

            var WidthPK = document.querySelectorAll<HTMLElement>('.PK');
            (WidthPK[0] as HTMLElement).style.width = whtmPK + '%';

            var WidthDEL = document.querySelectorAll<HTMLElement>('.DEL');
            (WidthDEL[0] as HTMLElement).style.width = whtmDEL + '%';

            var finishsTRIP = Math.floor(data[0].actualVolCount / 2) * 2;
            htmFINSH.innerHTML = data[0].ecarCount.toString();

            //Status Screen
            var screenStatus = document.getElementById('screen-status');

            if (data[0].screenStatus == 'STOPED') {
                screenStatus.innerHTML = data[0].screenStatus.replace('ED', '');
                screenStatus.style.display = 'block';
            } else if (data[0].screenStatus == 'PAUSED') {
                screenStatus.innerHTML = data[0].screenStatus.replace('D', '');
                screenStatus.style.display = 'block';
            } else {
                screenStatus.style.display = 'none';
            }

            //#region triangle

            //2Ecar
            var trianglePK_2E = document.getElementById('trianglePK_2E');
            var triangleDEL_2E = document.getElementById('triangleDEL_2E');

            if (
                dataPK[0].processName == 'PK' &&
                dataPK[0].isDelayStart == 'Y' &&
                dataPK[0].status != 'STARTED' &&
                this.dataBigPartDDL[0].ecarName != this.dataBigPartDDL[1].ecarName
            ) {
                trianglePK_2E.style.display = 'block';
                trianglePK_2E.style.animation = 'flash linear 1s infinite';
            } else {
                trianglePK_2E.style.display = 'none';
            }
            if (
                dataDEL[0].processName == 'DEL' &&
                dataDEL[0].isDelayStart == 'Y' &&
                dataDEL[0].status != 'STARTED' &&
                this.dataBigPartDDL[0].ecarName != this.dataBigPartDDL[1].ecarName
            ) {
                triangleDEL_2E.style.display = 'block';
                triangleDEL_2E.style.animation = 'flashs linear 1s infinite';
            } else {
                triangleDEL_2E.style.display = 'none';
            }

            //1Ecar
            var trianglePK_1E = document.getElementById('trianglePK_1E');
            var triangleDEL_1E = document.getElementById('triangleDEL_1E');

            if (
                dataPK[0].processName == 'PK' &&
                dataPK[0].isDelayStart == 'Y' &&
                dataPK[0].status != 'STARTED' &&
                this.dataBigPartDDL[0].ecarName == this.dataBigPartDDL[1].ecarName
            ) {
                trianglePK_1E.style.display = 'block';
                trianglePK_1E.style.animation = 'flash linear 1s infinite';
            } else {
                trianglePK_1E.style.display = 'none';
            }

            if (
                dataDEL[0].processName == 'DEL' &&
                dataDEL[0].isDelayStart == 'Y' &&
                dataDEL[0].status != 'STARTED' &&
                this.dataBigPartDDL[0].ecarName == this.dataBigPartDDL[1].ecarName &&
                dataPK[0].status == 'FINISHED'
            ) {
                trianglePK_1E.style.display = 'none';
                triangleDEL_1E.style.display = 'block';
                triangleDEL_1E.style.animation = 'flashs linear 1s infinite';
            } else {
                triangleDEL_1E.style.display = 'none';
            }

            //#endregion

            //#region background color DEL,PK
            //1ECAR
            //1ECAR-PK
            if (dataPK[0].status == 'STARTED') {
                var ColorPKST = document.querySelectorAll<HTMLElement>('.PK');
                (ColorPKST[0] as HTMLElement).style.animation = 'none';
                (ColorPKST[0] as HTMLElement).style.background = '#FFFFFF';
                trianglePK_1E.style.display = 'none';
            } else if (dataPK[0].status == 'FINISHED') {
                var ColorPKFN = document.querySelectorAll<HTMLElement>('.PK');
                (ColorPKFN[0] as HTMLElement).style.animation = 'none';
                (ColorPKFN[0] as HTMLElement).style.background = '#92D14F';
                trianglePK_1E.style.display = 'none';
            } else if (dataPK[0].status == 'DELAYED') {
                var ColorPKDL = document.querySelectorAll<HTMLElement>('.PK');
                for (let i = 0; ColorPKDL[i]; i++) {
                    (ColorPKDL[i] as HTMLElement).style.animation = 'flashsback linear 1s infinite';
                    trianglePK_1E.style.display = 'none';
                }
            } else {
                var ColorPKST = document.querySelectorAll<HTMLElement>('.PK');
                (ColorPKST[0] as HTMLElement).style.animation = 'none';
                (ColorPKST[0] as HTMLElement).style.background = '#FFFFFF';
            }

            //1ECAR-DEL
            if (dataDEL[0].status == 'STARTED') {
                var ColorDELST = document.querySelectorAll<HTMLElement>('.DEL');
                (ColorDELST[0] as HTMLElement).style.animation = 'none';
                (ColorDELST[0] as HTMLElement).style.background = '#FFFFFF';
                triangleDEL_1E.style.display = 'none';
            } else if (dataDEL[0].status == 'FINISHED') {
                var ColorDELFN = document.querySelectorAll<HTMLElement>('.DEL');
                (ColorDELFN[0] as HTMLElement).style.animation = 'none';
                (ColorDELFN[0] as HTMLElement).style.background = '#01B0F1';
                triangleDEL_1E.style.display = 'none';
            } else if (dataDEL[0].status == 'DELAYED') {
                var ColorDELDL = document.querySelectorAll<HTMLElement>('.DEL');
                for (let i = 0; ColorDELDL[i]; i++) {
                    (ColorDELDL[i] as HTMLElement).style.animation = 'flashsback linear 1s infinite';
                }
                triangleDEL_1E.style.display = 'none';
            } else {
                var ColorDELST = document.querySelectorAll<HTMLElement>('.DEL');
                (ColorDELST[0] as HTMLElement).style.animation = 'none';
                (ColorDELST[0] as HTMLElement).style.background = '#FFFFFF';
            }

            //2ECAR-PK
            if (dataPK[0].status == 'STARTED') {
                var ColorPKST2E = document.querySelectorAll<HTMLElement>('.PK_2E');
                (ColorPKST2E[0] as HTMLElement).style.animation = 'none';
                (ColorPKST2E[0] as HTMLElement).style.backgroundColor = '#FFFFFF';
                trianglePK_2E.style.display = 'none';
            } else if (dataPK[0].status == 'FINISHED') {
                var ColorPKFN2E = document.querySelectorAll<HTMLElement>('.PK_2E');
                (ColorPKFN2E[0] as HTMLElement).style.animation = 'none';
                (ColorPKFN2E[0] as HTMLElement).style.background = '#92D14F';
                trianglePK_2E.style.display = 'none';
            } else if (dataPK[0].status == 'DELAYED') {
                var ColorPKDL2E = document.querySelectorAll<HTMLElement>('.PK_2E');
                (ColorPKDL2E[0] as HTMLElement).style.animation = 'flashsback linear 1s infinite';
                trianglePK_2E.style.display = 'none';
            } else {
                var ColorPKST2E = document.querySelectorAll<HTMLElement>('.PK_2E');
                (ColorPKST2E[0] as HTMLElement).style.animation = 'none';
                (ColorPKST2E[0] as HTMLElement).style.backgroundColor = '#FFFFFF';
            }

            //2ECAR-DEL
            if (dataDEL[0].status == 'STARTED') {
                var ColorDELST2E = document.querySelectorAll<HTMLElement>('.DEL_2E');
                (ColorDELST2E[0] as HTMLElement).style.animation = 'none';
                (ColorDELST2E[0] as HTMLElement).style.background = '#FFFFFF';
                triangleDEL_2E.style.display = 'none';
            } else if (dataDEL[0].status == 'FINISHED') {
                var ColorDELFN2E = document.querySelectorAll<HTMLElement>('.DEL_2E');
                (ColorDELFN2E[0] as HTMLElement).style.animation = 'none';
                (ColorDELFN2E[0] as HTMLElement).style.background = '#01B0F1';
                triangleDEL_2E.style.display = 'none';
            } else if (dataDEL[0].status == 'DELAYED') {
                var ColorDELDL2E = document.querySelectorAll<HTMLElement>('.DEL_2E');
                (ColorDELDL2E[0] as HTMLElement).style.animation = 'flashsback linear 1s infinite';
                triangleDEL_2E.style.display = 'none';
            } else {
                var ColorDELST2E = document.querySelectorAll<HTMLElement>('.DEL_2E');
                (ColorDELST2E[0] as HTMLElement).style.animation = 'none';
                (ColorDELST2E[0] as HTMLElement).style.background = '#FFFFFF';
            }
            //#endregion
        } else {
            htmTitle.innerHTML = 'Big Part Direct Delivery Progress';
            htmPLINE.innerHTML = '';
        }
    }
}
