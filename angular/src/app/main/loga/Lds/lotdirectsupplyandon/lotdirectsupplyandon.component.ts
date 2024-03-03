import { finalize, timeout } from 'rxjs/operators';
import { Component, HostListener, Injector, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LotDirectSupplyAndonServiceProxy, LotDirectSupplyAndonDto } from '@shared/service-proxies/service-proxies';
import { CommonFunction } from "@app/main/commonfuncton.component";

@Component({
    selector: 'app-lotdirectsupplyandon',
    templateUrl: './lotdirectsupplyandon.component.html',
    styleUrls: ['./lotdirectsupplyandon.component.less']
})
export class LotDirectSupplyAndonComponent implements OnInit {
    prodLine:string = '';
    coutDown: number = 0;
    dataLotDirect: LotDirectSupplyAndonDto[] = [];
    dataTime: any[] = [];
    wTemp:number = 0;
    reMain:number = 0;
    clearTimeCountDown;
    clearTimeLoadForm;
    fn:CommonFunction = new CommonFunction();

    constructor(
        private activatedRoute: ActivatedRoute,
        private _service: LotDirectSupplyAndonServiceProxy,) { }

    ngOnInit() {
        //this.prodLine = this.activatedRoute.snapshot.queryParams ['pline'] || '';
        let urlParams = new URLSearchParams(window.location.search);
        this.prodLine = urlParams.get('pline');

        this.loadForm();
         console.log('ngOnInit');
    }

    timecount:number = 0;
    refeshPage: number = 600;
    ngAfterViewInit() {
        this.timeoutData();
        console.log('ngAfterViewInit');
    }

    ngOnDestroy(): void {
        clearTimeout(this.clearTimeLoadForm);
    }

    timeoutData()
    {
        try {
            if (this.timecount > this.refeshPage) window.location.reload();
            this.timecount = this.timecount + 1;
            this.getData();
            this.fn.showtime('time_now_log');

        } catch(ex){
            console.log('1: '+ ex);

            this.clearTimeLoadForm = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        }
    }

    @HostListener('window:resize', ['$event'])
    onWindowResize() {
        this.loadForm();
        console.log('onWindowResize');
    }

    getData() {
        this._service.getDataLotDirectSupplyAndon(this.prodLine).pipe(finalize(() => {
        })).subscribe((result) => {
            try{

                this.dataLotDirect = result.items ?? [];
                this.dataTime = this.dataLotDirect;
                this.loadData();
                this.countDownData();

                this.loadData2(result.items);

                this.clearTimeLoadForm = setTimeout(()=>{
                    this.timeoutData();
                }, 1000);

            } catch(ex) {
                console.log('2: '+ ex);

                this.clearTimeLoadForm = setTimeout(()=>{
                    this.timeoutData();
                }, 1000);
            }
        },(error) => {
            console.log(error);
            this.clearTimeLoadForm = setTimeout(() => {
                this.timeoutData();
            }, 1000);
        })
    }

    w:number = 0;
    heightBox1A1:number = 0;
    heightBox2A2:number = 0;
    hBoxContent:number = 0;
    loadForm() {
        this.w = window.innerWidth -2;
        var h = window.innerHeight -2;
        var hTitle = h * 10 / 100 - 1;
        var hBoxContent = h - hTitle;
        this.heightBox1A1 = hBoxContent * 25 / 100 - 2;
        var heightBox1A2 = hBoxContent - this.heightBox1A1;
        this.heightBox2A2 = hBoxContent * 50 / 100 - 2;
        var heightBox2A3 = hBoxContent - this.heightBox2A2 - this.heightBox1A1;
        this.hBoxContent = hBoxContent;

        var ALL = document.querySelector<HTMLElement>('.ALL');
        (ALL).style.width = this.w + 'px';
        (ALL).style.height = h + 'px';

        var TITLE = document.querySelector<HTMLElement>('.ALL .TITLE');
        (TITLE).style.height = hTitle + 'px';

        var TITLE = document.querySelector<HTMLElement>('.ALL .BOX_CONTENT');
        (TITLE).style.height = hBoxContent + 'px';

        var BOX1_A1 = document.querySelector<HTMLElement>('.ALL .BOX_CONTENT .BOX1 .BOX1_A1');
        (BOX1_A1).style.height = this.heightBox1A1 + 'px';

        var BOX1_A2 = document.querySelector<HTMLElement>('.ALL .BOX_CONTENT .BOX1 .BOX1_A2');
        (BOX1_A2).style.height = heightBox1A2 + 'px';

        var BOX2_A1 = document.querySelector<HTMLElement>('.ALL .BOX_CONTENT .BOX2 .BOX2_A1');
        (BOX2_A1).style.height = this.heightBox1A1 - 2 + 'px';

        var BOX2_A2 = document.querySelector<HTMLElement>('.ALL .BOX_CONTENT .BOX2 .BOX2_A2');
        (BOX2_A2).style.height = this.heightBox2A2 + 'px';

        var BOX2_A3 = document.querySelector<HTMLElement>('.ALL .BOX_CONTENT .BOX2 .BOX2_A3');
        (BOX2_A3).style.height = heightBox2A3 + 'px';


        //triangle-down
        var triangle = document.getElementById("triangle-down");
        triangle.style.marginTop = (h*56.62/100) + 'px';
        triangle.style.display = 'none';

        //triangle-down-delay
        var triangledelay = document.getElementById("triangle-down-delay");
        triangledelay.style.marginTop = (h*56.62/100) + 'px';
        triangledelay.style.display = 'none';

    }

    countDownData() {
        var taktTime =  document.getElementById("progressBar");
        var reMain =  document.getElementById("reMain");
        // var w = window.innerWidth;
        // var h = window.innerHeight;
        // var hTitle = h * 10 / 100 - 1;
        // var hBoxContent = h - hTitle;
        // var heightBox1A1 = hBoxContent * 25 / 100 - 2;
        let wRemain = (this.w/12)*10;
        reMain.style.marginTop = this.heightBox1A1/2 - 74 + 'px';
        reMain.style.marginLeft = wRemain/2 - 36 + 'px';

        if(this.dataTime.length > 0){
            taktTime.style.display = 'block';
            reMain.style.display = 'block';

            var planT = this.dataTime[0].planTrim;
            var actT = this.dataTime[0].actualTrim;
            this.reMain = (planT - actT);

            taktTime.style.width = wRemain/planT*actT + 'px';
            taktTime.style.backgroundColor = '#92D050';

        }
        else{
            taktTime.style.display = 'none';
            reMain.style.display = 'none';
           // TaktTime[0].style.marginLeft = '-100%';
        }
    }

    loadData() {
        let htmTitle = document.querySelector('.ALL .TITLE');
        let htmListTrip = document.querySelector('.ALL .BOX2 .BOX2_A2');
        let htmTripNext = document.querySelector('.ALL .BOX2 .BOX2_A3');
        let signal = document.querySelector('.ALL .BOX_CONTENT .BOX1 .BOX1_A1 .SIGN');
        let plan = document.querySelector('.ALL .BOX_CONTENT .BOX1 .BOX1_A1 .PLAN');

        var w = window.innerWidth;
        // var h = window.innerHeight;
        var widthBox2 = w*(100/12*10)/100
        // var hTitle = h * 10 / 100 - 1;
        // var hBoxContent = h - hTitle;
        // var heightBox2A2 = this.hBoxContent * 50 / 100 - 2;
        var hWhite = this.heightBox2A2 * 25 /100;
        var wLeft = w*((100/12)*2)/100 - 54;

        var data = this.dataLotDirect;
        var countData = data.length

        if (countData > 0) {
    //title
            htmTitle.innerHTML = data[0].title;
            signal.textContent = data[0].ntSignalCount.toString();
            plan.textContent = data[0].planVolCount.toString();

    //Status Screen
            var screenStatus = document.getElementById("screen-status");
            if(data[0].screenStatus == "STOPED"){
                screenStatus.innerHTML = data[0].screenStatus.replace('ED','');
                screenStatus.style.display = "block";
            }
            else if(data[0].screenStatus == "PAUSED"){
                screenStatus.innerHTML = data[0].screenStatus.replace('D','');
                screenStatus.style.display = "block";
            }
            else {
                screenStatus.style.display = "none";
            }

    //data trip
            var triangleDisplay = document.getElementById("triangle-down");
            var triangleDelayDisplay = document.getElementById("triangle-down-delay");
            var htmTrip = "";
            var totalTripTatkTime = 0;
            var wNull = 0;
            for(var a = 0 ;a < countData;a++){
                totalTripTatkTime = totalTripTatkTime + data[a].tripTatkTime
            }

            var bigTrip = data[(countData-1)].trip;
            var totalTrip = data[0].totalTrip;
            var boxNull = totalTrip - countData;

            if(boxNull > 0){
                wNull = (widthBox2*((data[0].totalTaktTime - totalTripTatkTime)/data[0].totalTaktTime))/boxNull;
                if(wNull < 0) wNull = wNull*-1; //fix wNull âm
                // console.log(data[0].totalTaktTime - totalTripTatkTime)
            }else{
                wNull = (widthBox2*((data[0].totalTaktTime - totalTripTatkTime)/data[0].totalTaktTime))*1;
            }



        if(bigTrip == 3){

            for (var i = 0; i < totalTrip; i++) {

                if(i < boxNull){
                    htmTrip = htmTrip + '<div style="height: 100%;border-right:2px solid #000;width:' + wNull + 'px' + '"></div>';
                    this.wTemp = wNull * (i + 1);
                }
                else if(boxNull <= i)
                {
                    var j =  i - boxNull;
                    var wBox = widthBox2 * (data[j].tripTatkTime/data[j].totalTaktTime);
                    this.wTemp = (j > 0) ? (widthBox2 * (data[j-1].tripTatkTime/data[j-1].totalTaktTime) + this.wTemp ) : this.wTemp

                    if(data[j].status == 'FINISHED'){
                        htmTrip = htmTrip + '<div class="ItemTrip ItemTrip'+ data[j].trip +'" style="height: 100%;background-color: #00B0F0;width:' + wBox + 'px' + '">'
                                        + '<div style = "height:'+hWhite+ 'px' +'"></div>'
                                        + '<div style = "font-size: 50px;text-align: center;font-weight:700;">Trip ' + data[j].trip  + ': </div>'
                                        + '<div style = "font-size: 62px;text-align: center;padding-top:5%"> ' + data[j].dolly + ' </div>'
                                        + '<div id="trip'+data[j].trip+'" class="tamgiac"></div>'
                                        + '</div>';
                    }
                    else if(data[j].status == 'DELAYED'){
                        htmTrip = htmTrip + '<div class="ItemTrip ItemTrip'+data[j].trip+'" style="height: 100%; -webkit-animation: flashTrip linear 1s infinite;animation: flashTrip linear 1s infinite;animation-duration: 0.9s;animation-iteration-count: infinite;width:' + wBox + 'px' + '">'
                                        + '<div style = "height:'+hWhite+ 'px' +'"></div>'
                                        + '<div style = "font-size: 50px;text-align: center;font-weight:700;">Trip ' + data[j].trip  + ': </div>'
                                        + '<div style = "font-size: 62px;text-align: center;padding-top:5%"> ' + data[j].dolly + ' </div>'
                                        + '<div id="trip'+data[j].trip+'" class="tamgiac"></div>'
                                        + '</div>';
                    }
                    else {
                        htmTrip = htmTrip + '<div class="ItemTrip ItemTrip'+data[j].trip+'" style="height: 100%; width:' + wBox + 'px' + '">'
                                        + '<div style = "height:'+hWhite+ 'px' +'"></div>'
                                        + '<div style = "font-size: 50px;text-align: center;font-weight:700;">Trip ' + data[j].trip  + ': </div>'
                                        + '<div style = "font-size: 62px;text-align: center;padding-top:5%"> ' + data[j].dolly + ' </div>'
                                        + '<div id="trip'+data[j].trip+'" class="tamgiac"></div>'
                                        + '</div>';
                    }

                }
            }
        }else if(0 < bigTrip &&  bigTrip < 3){

            for (var i = 0; i < totalTrip; i++) {

                if(i < countData)
                {
                    var wBox = widthBox2 * (data[i].tripTatkTime/data[i].totalTaktTime);
                    this.wTemp = (i > 0) ? (widthBox2 * (data[i-1].tripTatkTime/data[i-1].totalTaktTime) + this.wTemp ) : this.wTemp

                    if(data[i].status == 'FINISHED'){
                        htmTrip = htmTrip + '<div class="ItemTrip ItemTrip'+data[i].trip+'" style="height: 100%; background-color: #00B0F0;width:' + wBox + 'px' + '">'
                                        + '<div style = "height:'+hWhite+ 'px' +'"></div>'
                                        + '<div style = "font-size: 50px;text-align: center;font-weight:700;">Trip ' + data[i].trip  + ': </div>'
                                        + '<div style = "font-size: 62px;text-align: center;padding-top:5%"> ' + data[i].dolly + ' </div>'
                                        + '<div id="trip'+data[i].trip+'" class="tamgiac"></div>'
                                        + '</div>';
                    }
                    else if(data[i].status == 'DELAYED'){
                        htmTrip = htmTrip + '<div class="ItemTrip ItemTrip'+data[i].trip+'" style="height: 100%; -webkit-animation: flashTrip linear 1s infinite;animation: flashTrip linear 1s infinite;animation-duration: 0.9s;animation-iteration-count: infinite;width:' + wBox + 'px' + '">'
                                        + '<div style = "height:'+hWhite+ 'px' +'"></div>'
                                        + '<div style = "font-size: 50px;text-align: center;font-weight:700;">Trip ' + data[i].trip  + ': </div>'
                                        + '<div style = "font-size: 62px;text-align: center;padding-top:5%"> ' + data[i].dolly + ' </div>'
                                        + '<div id="trip'+data[i].trip+'" class="tamgiac"></div>'
                                        + '</div>';
                    }
                    else {
                        htmTrip = htmTrip + '<div class="ItemTrip ItemTrip'+data[i].trip+'" style="height: 100%; width:' + wBox + 'px' + '">'
                                        + '<div style = "height:'+hWhite+ 'px' +'"></div>'
                                        + '<div style = "font-size: 50px;text-align: center;font-weight:700;">Trip ' + data[i].trip  + ': </div>'
                                        + '<div style = "font-size: 62px;text-align: center;padding-top:5%"> ' + data[i].dolly + ' </div>'
                                        + '<div id="trip'+data[i].trip+'" class="tamgiac"></div>'
                                        + '</div>';
                    }

                }else if(countData <= i)
                {
                    // console.log(wNull)
                    htmTrip = htmTrip + '<div class="ItemTrip " style="height: 100%;border-right:2px solid #000;width:' + wNull + 'px' + '"></div>';
                }

            }
        }
            htmListTrip.innerHTML = htmTrip;
            this.wTemp = 0;
    //trip next
            htmTripNext.innerHTML =  'Next: ' + data[0].nextDolly;
        }
        else{
            htmTitle.innerHTML = "LOT DIRECT SUPPLY";
            htmListTrip.innerHTML = "";
            htmTripNext.innerHTML = "";
        }
    }

    loadData2(data:LotDirectSupplyAndonDto[]){
        if(data){

            document.getElementById("triangle-down").style.display = 'none';
            document.getElementById("triangle-down-delay").style.display = 'none';

            for (let i = 0; i < data.length; i++) {

                let _tamgiac = document.querySelector<HTMLElement>('#trip' + data[i].trip);
                let _ItemTrip = document.querySelector<HTMLElement>('.ItemTrip' + data[i].trip);

                //clear data

                _tamgiac.classList.remove('RED', 'YELLOW');
                _tamgiac.style.display = "none";

                //
                if(data[i].status == "NEWTAKT" || data[i].status == null){  //1
                    console.log('#trip' + data[i].trip + ', ' +data[i].status);
                    _tamgiac.style.display = "block";
                    if(_ItemTrip.classList.contains('DELAYED')) _ItemTrip.classList.remove('DELAYED');



                    if(data[i].isDelayStart == "Y"){
                        _tamgiac.classList.remove('YELLOW');
                        _tamgiac.classList.add('RED');
                    }
                    else{
                        _tamgiac.classList.remove('RED');
                        _tamgiac.classList.add('YELLOW');
                    }

                    break;
                }
                else if (data[i].status == "STARTED"){
                    console.log('#trip' + data[i].trip + ', ' +data[i].status);
                    _tamgiac.style.display = "none";

                    _ItemTrip.style.backgroundColor = "transparent";
                    if(_ItemTrip.classList.contains('DELAYED')) _ItemTrip.classList.remove('DELAYED');

                    break;
                }
                else if (data[i].status == "DELAYED"){
                    console.log('#trip' + data[i].trip + ', ' +data[i].status);
                    _tamgiac.style.display = "none";

                    _ItemTrip.style.backgroundColor = "transparent";
                    if(!_ItemTrip.classList.contains('DELAYED')) _ItemTrip.classList.add('DELAYED');

                    break;

                }


            }
        }
    }

}


