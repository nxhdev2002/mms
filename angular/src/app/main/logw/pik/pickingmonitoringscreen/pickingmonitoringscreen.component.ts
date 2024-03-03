import { finalize } from 'rxjs/operators';
import { Component, HostListener, Injector, OnInit } from '@angular/core';
import { PickingMonitoringScreenServiceProxy } from '@shared/service-proxies/service-proxies';
import { ActivatedRoute } from '@angular/router';
import { CommonFunction } from "@app/main/commonfuncton.component";

@Component({
    selector: 'app-pickingmonitoringscreen',
    templateUrl: './pickingmonitoringscreen.component.html',
    styleUrls: ['./pickingmonitoringscreen.component.less']
})
export class PickingmonitoringScreenComponent implements OnInit {
    coutDown: number = 0;
    dataMonitoring: any[] = [];
    dataUnderBody: any[] = [];
    dataSideMain: any[] = [];
    dataPikProMo: any[] = [];
    dataDelayPikProMo: any[] = [];
    pLine: string = '';
    pPositionUB: string = '';
    pPositionSM: string = '';
    reMain: number = 0;
    clearTimeLoadData;
    fn:CommonFunction = new CommonFunction();


    constructor(injector: Injector,
        private activatedRoute: ActivatedRoute,
        private _service: PickingMonitoringScreenServiceProxy,) { }


    ngOnInit(): void {
        //this.pLine = this.activatedRoute.snapshot.queryParams ['pline'] || '';
        let urlParams = new URLSearchParams(window.location.search);
        this.pLine = urlParams.get('pline');

        this.pPositionUB = 'UB_' + this.pLine;
        this.pPositionSM = 'SM_' + this.pLine;

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

    timecount:number = 0;
    refeshPage: number = 600;
    timeoutData()
    {
        try {
            if (this.timecount > this.refeshPage) window.location.reload();
            this.timecount = this.timecount + 1;

            this.getData();
            this.fn.showtime('time_now_log');

        } catch(ex){
            console.log('1: '+ ex);

            this.clearTimeLoadData = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }


    //set width height
    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
    }

    getData() {
        this._service.getDataMonitoringScreen(this.pLine, this.pPositionUB , this.pPositionSM).pipe(finalize(() => {
        })).subscribe((result) => {
            try {
                this.dataMonitoring = result ?? [];
                this.dataPikProMo = this.dataMonitoring[0];
                this.dataUnderBody = this.dataMonitoring[1];
                this.dataSideMain = this.dataMonitoring[2];
                this.dataDelayPikProMo = this.dataMonitoring[3];

                this.loadData();
                this.countDown();

                this.clearTimeLoadData = setTimeout(() => {
                    this.timeoutData();
                }, 1000);

            } catch(ex){
                console.log('2: '+ ex);

                this.clearTimeLoadData = setTimeout(() => {
                    this.timeoutData();
                }, 1000);
            }
        },(error) => {
            console.log(error);
            this.clearTimeLoadData = setTimeout(() => {
                    this.timeoutData();
                }, 1000);
        });
    }

    loadForm() {
        var w = window.innerWidth
        var h = window.innerHeight
        var hightBox_A1 = h * 12 / 100;
        var hightBox_A2 = h * 7 / 100;
        var hightBox_A5 = h * 6 / 100;
        var hightBox2_A789 = (h - (hightBox_A1 * 3) - (hightBox_A2 * 3) - hightBox_A5) / 3;


        var PTA = document.querySelectorAll<HTMLElement>('.ALL');
        (PTA[0] as HTMLElement).style.width = w + 'px';
        (PTA[0] as HTMLElement).style.height = h + 'px';

        var ALL_BOX1 = document.querySelectorAll<HTMLElement>('.ALL .ALL_BOX .BOX1');
        for (let i = 0; ALL_BOX1[i]; i++) {
            (ALL_BOX1[i] as HTMLElement).style.height = h + 'px';
        }

        var ALL_BOX2 = document.querySelectorAll<HTMLElement>('.ALL .ALL_BOX .BOX2');
        for (let i = 0; ALL_BOX2[i]; i++) {
            (ALL_BOX2[i] as HTMLElement).style.height = h + 'px';
        }

        var BOX1_A1 = document.querySelectorAll<HTMLElement>(
            '.ALL .ALL_BOX .BOX1 .BOX1_A1,'
            + '.ALL .ALL_BOX .BOX1 .BOX1_A1 .A1_ITEM1,'
            + '.ALL .ALL_BOX .BOX1 .BOX1_A1 .A1_ITEM2');
        for (let i = 0; BOX1_A1[i]; i++) {
            (BOX1_A1[i] as HTMLElement).style.height = hightBox_A1 + 'px';
            (BOX1_A1[i] as HTMLElement).style.fontSize = 20 + 'px';
        }
        var BOX1_A2 = document.querySelectorAll<HTMLElement>(
            '.ALL .ALL_BOX .BOX1 .BOX1_A2,'
            + '.ALL .ALL_BOX .BOX1 .BOX1_A3,'
            + '.ALL .ALL_BOX .BOX1 .BOX1_A4'
        );
        for (let i = 0; BOX1_A2[i]; i++) {
            (BOX1_A2[i] as HTMLElement).style.height = hightBox_A2 + 'px';
        }
        var BOX1_A5 = document.querySelectorAll<HTMLElement>('.ALL .ALL_BOX .BOX1 .BOX1_A5');
        for (let i = 0; BOX1_A5[i]; i++) {
            (BOX1_A5[i] as HTMLElement).style.height = hightBox_A5 + 'px';
        }
        var BOX1_A6 = document.querySelectorAll<HTMLElement>(
            '.ALL .ALL_BOX .BOX1 .BOX1_A6,'
            + '.ALL .ALL_BOX .BOX1 .BOX1_A6 .A6_ITEM1,'
            + '.ALL .ALL_BOX .BOX1 .BOX1_A6 .A6_ITEM2');
        for (let i = 0; BOX1_A6[i]; i++) {
            (BOX1_A6[i] as HTMLElement).style.height = hightBox_A1 - 1 + 'px';
        }
        var BOX1_A7 = document.querySelectorAll<HTMLElement>(
            '.ALL .ALL_BOX .BOX1 .BOX1_A7,'
            + '.ALL .ALL_BOX .BOX1 .BOX1_A7 .A7_ITEM1,'
            + '.ALL .ALL_BOX .BOX1 .BOX1_A7 .A7_ITEM2');
        for (let i = 0; BOX1_A7[i]; i++) {
            (BOX1_A7[i] as HTMLElement).style.height = h - (hightBox_A1 * 3) - (hightBox_A2 * 3) - hightBox_A5 + 1 + 'px';
        }
        var BOX1_A10 = document.querySelectorAll<HTMLElement>('.ALL .ALL_BOX .BOX1 .BOX1_A10');
        for (let i = 0; BOX1_A10[i]; i++) {
            (BOX1_A10[i] as HTMLElement).style.height = hightBox_A1 + 'px';
        }
        var BOX2_A1 = document.querySelectorAll<HTMLElement>('.ALL .ALL_BOX .BOX2 .BOX2_A1');
        for (let i = 0; BOX2_A1[i]; i++) {
            (BOX2_A1[i] as HTMLElement).style.height = hightBox_A1 + 'px';
        }
        var BOX2_A2 = document.querySelectorAll<HTMLElement>('.ALL .ALL_BOX .BOX2 .BOX2_A2');
        for (let i = 0; BOX2_A1[i]; i++) {
            (BOX2_A2[i] as HTMLElement).style.height = hightBox_A2 + 'px';
        }
        var BOX2_A3 = document.querySelectorAll<HTMLElement>('.ALL .ALL_BOX .BOX2 .BOX2_A3');
        for (let i = 0; BOX2_A3[i]; i++) {
            (BOX2_A3[i] as HTMLElement).style.height = hightBox_A2 + 'px';
            (BOX2_A3[i] as HTMLElement).style.display = 'flex';
        }
        var BOX2_A4 = document.querySelectorAll<HTMLElement>('.ALL .ALL_BOX .BOX2 .BOX2_A4');
        for (let i = 0; BOX2_A4[i]; i++) {
            (BOX2_A4[i] as HTMLElement).style.height = hightBox_A2 + 'px';
        }
        var BOX2_A5 = document.querySelectorAll<HTMLElement>('.ALL .ALL_BOX .BOX2 .BOX2_A5');
        for (let i = 0; BOX2_A5[i]; i++) {
            (BOX2_A5[i] as HTMLElement).style.height = hightBox_A5 + 'px';
        }
        var BOX2_A6 = document.querySelectorAll<HTMLElement>('.ALL .ALL_BOX .BOX2 .BOX2_A6');
        for (let i = 0; BOX2_A6[i]; i++) {
            (BOX2_A6[i] as HTMLElement).style.height = hightBox_A1 + 'px';
        }
        var BOX2_A789 = document.querySelectorAll<HTMLElement>(
            '.ALL .ALL_BOX .BOX2 .BOX2_A7,'
            + '.ALL .ALL_BOX .BOX2 .BOX2_A8,'
            + '.ALL .ALL_BOX .BOX2 .BOX2_A9'
        );
        for (let i = 0; BOX2_A789[i]; i++) {
            (BOX2_A789[i] as HTMLElement).style.height = hightBox2_A789 + 'px';
        }

        var BOX2_A10 = document.querySelectorAll<HTMLElement>(
            '.ALL .ALL_BOX .BOX2 .BOX2_A10,'
            + '.ALL .ALL_BOX .BOX2 .BOX2_A10 .A10_ITEM1,'
            + '.ALL .ALL_BOX .BOX2 .BOX2_A10 .A10_ITEM2'
        );
        for (let i = 0; BOX2_A10[i]; i++) {
            (BOX2_A10[i] as HTMLElement).style.height = hightBox_A1 + 'px';
        }

    }

    countDown() {
        var taktTime = document.getElementById("progressBar");
        var reMain = document.getElementById("reMain");
        var w = window.innerWidth;
        var h = window.innerHeight;
        var hRemain = h * 7 / 100;
        var wRemain = (w / 12) * 10;
        reMain.style.marginTop = hRemain / 2 - 24 + 'px';
        reMain.style.marginLeft = wRemain / 2 - 130 + 'px';


        if (this.dataPikProMo.length > 0) {
            taktTime.style.display = 'block';
            reMain.style.display = 'block';

            var planT = this.dataPikProMo[0].currentTaktTime;
            var actT = this.dataPikProMo[0].taktCountDown;
            this.reMain = (planT - actT);

            taktTime.style.width = (wRemain / planT * (this.reMain) <= 0 ? 0 : wRemain / planT * (this.reMain)) + 'px';
            taktTime.style.backgroundColor = 'rgb(66, 166, 233)';

        }
        else {
            taktTime.style.display = 'none';
            reMain.style.display = 'none';
        }
    }


    loadData() {
        var w = window.innerWidth;
        var wPUBs = w * 80 / 100;
        var dataUB_W = this.dataUnderBody;
        var dataSM_W = this.dataSideMain;
        var dataPKM = this.dataPikProMo;
        var dataSM_DPM = this.dataDelayPikProMo.filter(d => d.processGroup === 'SM');
        var dataUB_DPM = this.dataDelayPikProMo.filter(d => d.processGroup === 'UB');
        var countUB_W = dataUB_W.length;
        var countSM_W = dataSM_W.length;
        var countPKM = dataPKM.length;
        var countSM_DPM = dataSM_DPM.length;
        var countUB_DPM = dataUB_DPM.length;
        var wPUB = wPUBs / countUB_W
        var wPSM = wPUBs / countSM_W
        var wCycle = wPUBs / 12
        let htmUB_W = document.querySelector('.ALL .ALL_BOX .BOX2 .BOX2_A3');
        let htmSM_W = document.querySelector('.ALL .ALL_BOX .BOX2 .BOX2_A4');
        let htmCycle = document.querySelector('.ALL .ALL_BOX .BOX2 .BOX2_A5');
        let htmPkSmTotal = document.querySelector('.ALL .ALL_BOX .BOX2 .BOX2_A6');
        let htmPkUbTotal = document.querySelector('.ALL .ALL_BOX .BOX2 .BOX2_A7');
        let htmPkAvgTotal = document.querySelector('.ALL .ALL_BOX .BOX2 .BOX2_A8');
        let htmPkTotal = document.querySelector('.ALL .ALL_BOX .BOX2 .BOX2_A9');
        let htmTitle = document.querySelector('.ALL .ALL_BOX .BOX2 .BOX2_A1');
        let htmTactTime = document.querySelector('.ALL .ALL_BOX .BOX1 .BOX1_A1 .A1_ITEM2');
        let htmTotalStop = document.querySelector('.ALL .ALL_BOX .BOX1 .BOX1_A7 .A7_ITEM1');
        let htmTotalDelay = document.querySelector('.ALL .ALL_BOX .BOX1 .BOX1_A7 .A7_ITEM2');


        if (countPKM > 0) {

            //title
            htmTitle.innerHTML = dataPKM[0].screenTitle;

            //tactTime
            htmTactTime.innerHTML = dataPKM[0].currentTaktTime;

            //Total Stop
            htmTotalStop.innerHTML = dataPKM[0].totalStop;

            //Total Delay
            htmTotalDelay.innerHTML = dataPKM[0].totalDelay;

            //PickingDelay
            var pld = document.getElementById("pdl");
            dataPKM[0].isPickingDelay == 'Y' ? pld.classList.add("pdelay") : pld.classList.remove("pdelay");

            //AGVAndWDelay
            var pld = document.getElementById("avg");
            dataPKM[0].isAGVAndWDelay == 'Y' ? pld.classList.add("avgDelay") : pld.classList.remove("avgDelay");

            //LeaderSupport
            var pld = document.getElementById("ls");
            dataPKM[0].isLeaderSupport == 'Y' ? pld.classList.add("lSupport") : pld.classList.remove("lSupport");

        }
        //packing under body
        var htmub_wa = "";
        for (var i = 0; i < countUB_W; i++) {
            if (dataUB_W[i].isFinished == 'Y') {
                var htmub_wa = htmub_wa + '<div style="height: 100%;background-color:#92D050;display: flex;align-items: center;justify-content: center;border-right:1px solid black;width:' + wPUB + 'px' + '">' + dataUB_W[i].process + '</div>';
            } else {
                var htmub_wa = htmub_wa + '<div style="height: 100%;background-color:yellow;display: flex;align-items: center;justify-content: center;border-right:1px solid black;width:' + wPUB + 'px' + '">' + dataUB_W[i].process + '</div>';
            }
        }
        htmUB_W.innerHTML = htmub_wa;

        //picking sidemain
        var htmsm_wa = "";
        for (var i = 0; i < countSM_W; i++) {
            if (dataSM_W[i].isFinished == 'Y') {
                var htmsm_wa = htmsm_wa + '<div style="height: 100%;display: flex;align-items: center;justify-content: center;background-color:#92D050;border-right:1px solid black;width:' + wPSM + 'px' + '">' + dataSM_W[i].process + '</div>';
            } else {
                var htmsm_wa = htmsm_wa + '<div style="height: 100%;display: flex;align-items: center;justify-content: center;background-color:yellow;border-right:1px solid black;width:' + wPSM + 'px' + '">' + dataSM_W[i].process + '</div>';
            }
        }
        htmSM_W.innerHTML = htmsm_wa;

        //cycle
        var htmcycle1 = "";
        var htmcycle1 = htmcycle1 + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;background-color:#B1C6DD;border-right:1px solid black;width:' + (wCycle * 2) + 'px' + '">Cycle</div>';
        for (var i = 0; i < countSM_DPM; i++) {
            htmcycle1 = htmcycle1 + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;background-color:#B1C6DD;border-right:1px solid black;width:' + wCycle + 'px' + '">' + dataSM_DPM[i].seqNo + '</div>';
        }
        htmCycle.innerHTML = htmcycle1;

        //pk sm total
        var htmSmTotal = "";
        var htmSmTotal = htmSmTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;background-color:#B1C6DD;border-right:1px solid black;width:' + wCycle + 'px' + '">P/K SM TOTAL</div>'
            + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;width:' + wCycle + 'px' + '">' + dataPKM[0].pkSmTotal + '</div>';
        for (var i = 0; i < 10; i++) {
            if (i < countSM_DPM) {
                if (dataSM_DPM[i].isDelay === 'Y') {
                    htmSmTotal = htmSmTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
                        + '-webkit-animation: delay linear 1s infinite; animation: delay linear 1s infinite;animation-duration: 0.9s;animation-iteration-count: infinite;'
                        + 'width:' + wCycle + 'px' + '">' + dataSM_DPM[i].delayTime + '</div>';
                }
                else if (dataSM_DPM[i].isCallLeader === 'Y') {
                    htmSmTotal = htmSmTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
                        + '-webkit-animation: calling linear 1s infinite; animation: calling linear 1s infinite;animation-duration: 0.9s;animation-iteration-count: infinite;'
                        + 'width:' + wCycle + 'px' + '">' + dataSM_DPM[i].delayTime + '</div>';
                } else {
                    htmSmTotal = htmSmTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
                        + 'width:' + wCycle + 'px' + '">' + dataSM_DPM[i].delayTime + '</div>';
                }
            } else if (i < 10) {
                htmSmTotal = htmSmTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
                    + 'width:' + wCycle + 'px' + '"></div>';
            }

        }
        htmPkSmTotal.innerHTML = htmSmTotal;

        //pk ub total
        var htmUbTotal = "";
        var htmUbTotal = htmUbTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;background-color:#B1C6DD;border-right:1px solid black;'
            + 'width:' + wCycle + 'px' + '">P/K UB TOTAL</div>'
            + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
            + 'width:' + wCycle + 'px' + '">' + dataPKM[0].pkUbTotal + '</div>';
        for (var i = 0; i < 10; i++) {
            if (i < countUB_DPM) {
                if (dataUB_DPM[i].isDelay === 'Y') {
                    htmUbTotal = htmUbTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
                        + '-webkit-animation: delay linear 1s infinite; animation: delay linear 1s infinite;animation-duration: 0.9s;animation-iteration-count: infinite;'
                        + 'width:' + wCycle + 'px' + '">' + dataUB_DPM[i].delayTime + '</div>';
                }
                else if (dataUB_DPM[i].isCallLeader === 'Y') {
                    htmUbTotal = htmUbTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
                        + '-webkit-animation: calling linear 1s infinite; animation: calling linear 1s infinite;animation-duration: 0.9s;animation-iteration-count: infinite;'
                        + 'width:' + wCycle + 'px' + '">' + dataUB_DPM[i].delayTime + '</div>';
                }
                else {
                    htmUbTotal = htmUbTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
                        + 'width:' + wCycle + 'px' + '">' + dataUB_DPM[i].delayTime + '</div>';
                }
            } else if (i < 10) {
                htmUbTotal = htmUbTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
                    + 'width:' + wCycle + 'px' + '"></div>';
            }
        }
        htmPkUbTotal.innerHTML = htmUbTotal;

        //pk AGV TOTAL
        var htmAvgTotal = "";
        var htmAvgTotal = htmAvgTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;background-color:#B1C6DD;border-right:1px solid black;'
            + 'width:' + wCycle + 'px' + '">AGV TOTAL</div>'
            + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
            + 'width:' + wCycle + 'px' + '">' + dataPKM[0].agvTotal + '</div>';
        for (var i = 1; i <= 10; i++) {
            htmAvgTotal = htmAvgTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
                + 'width:' + wCycle + 'px' + '"></div>';
        }
        htmPkAvgTotal.innerHTML = htmAvgTotal;

        //pk (W) TOTAL
        var htmTotal = "";
        var htmTotal = htmTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;background-color:#B1C6DD;border-right:1px solid black;'
            + 'width:' + wCycle + 'px' + '">(W) TOTAL</div>'
            + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
            + 'width:' + wCycle + 'px' + '">' + dataPKM[0].wTotal + '</div>';
        for (var i = 1; i <= 10; i++) {
            htmTotal = htmTotal + '<div style=" display: flex; justify-content: center; align-items: center;height: 100%;border-right:1px solid black;'
                + 'width:' + wCycle + 'px' + '"></div>';
        }
        htmPkTotal.innerHTML = htmTotal;

    }

}

