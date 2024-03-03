import { DatePipe } from '@angular/common';
import { Component, HostListener, OnInit } from '@angular/core';
import { PtsAdoBumperGetdataClrIndicatorDto, PtsAdoBumperServiceProxy } from '@shared/service-proxies/service-proxies';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-bumpergetdataclrindicator',
  templateUrl: './bumpergetdataclrindicator.component.html',
  styleUrls: ['./bumpergetdataclrindicator.component.less']
})
export class BumpergetdataclrindicatorComponent implements OnInit {
  dataDataBumperGetdataClrIndicator: any[] = [];
  clearTimeLoadData;
  pipe = new DatePipe('en-US');

  constructor(private _service: PtsAdoBumperServiceProxy,) { }

  ngOnInit(): void {
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
    this._service.getBumperGetdataClrIndicator().pipe(finalize(()=> {}))
    .subscribe((result) => {
        try{

            this.dataDataBumperGetdataClrIndicator = result.items ?? [];
        this.bindDataBumperGetdataClrIndicator();

        this.clearTimeLoadData = setTimeout(() => {
            this.timeoutData();
        }, 1000);

    } catch (ex) {
        console.log('2: ' + ex);

        this.clearTimeLoadData = setTimeout(() => {
            this.timeoutData();
        }, 1000);
    }
    })
}


  @HostListener('window:resize', ['$event'])
  onWindowResize() {
      this.loadForm();
  }

  loadForm() {
    var w = window.innerWidth;
    var h = window.innerHeight;
    var wCol7 = w / 15 * 100 // with 7 cot
    var hRow_2 = h / 2; //height 2 hang
    var hRow_a_0 = h / 2 * 10 / 100 //height hang A0
    var hRow = hRow_2 - hRow_a_0 // height hang A1
    var hRow_A1_P1_ALL = (hRow_2 - hRow_a_0 * 2) / 3 //height hang A1



    var PTA = document.querySelectorAll<HTMLElement>('.PTA');
    (PTA[0] as HTMLElement).style.width = w + 'px';
    (PTA[0] as HTMLElement).style.height = h + 'px';


    //get set class a0
    var PTA_PROCESS_Content_A0 = document.querySelectorAll<HTMLElement>('.PTA .PTA_PROCESS_Content .a0');
    for (let i = 0; PTA_PROCESS_Content_A0[i]; i++) {
      (PTA_PROCESS_Content_A0[i] as HTMLElement).style.width = w + 'px';
      (PTA_PROCESS_Content_A0[i] as HTMLElement).style.height = hRow_a_0 + 'px';
    }

    //get set class a1 and a3
    var PTA_PROCESS_Content_A0 = document.querySelectorAll<HTMLElement>('.PTA .PTA_PROCESS_Content .a1,.PTA .PTA_PROCESS_Content .a3');
    for (let i = 0; PTA_PROCESS_Content_A0[i]; i++) {
      (PTA_PROCESS_Content_A0[i] as HTMLElement).style.width = w + 'px';
      (PTA_PROCESS_Content_A0[i] as HTMLElement).style.height = hRow + 'px';
    }

    //get set  class a1 p1 and a3 p1
    var PTA_PROCESS_Content_A2 = document.querySelectorAll<HTMLElement>('.PTA .PTA_PROCESS_Content .a1 .p1,.PTA .PTA_PROCESS_Content .a3 .p1');
    for (let i = 0; PTA_PROCESS_Content_A2[i]; i++) {
      (PTA_PROCESS_Content_A2[i] as HTMLElement).style.width = wCol7 + 'px';
      (PTA_PROCESS_Content_A2[i] as HTMLElement).style.height = hRow + 'px';

    }

    // get class a1 p1 PROCESS
    var PTA_PROCESS_Content_A1_P1_PROCESS = document.querySelectorAll<HTMLElement>
      ('.PTA .PTA_PROCESS_Content .a1 .p1 .PROCESS, '
        + '.PTA .PTA_PROCESS_Content .a3 .p1 .PROCESS');
    for (let i = 0; PTA_PROCESS_Content_A1_P1_PROCESS[i]; i++) {
      (PTA_PROCESS_Content_A1_P1_PROCESS[i] as HTMLElement).style.height = hRow_a_0 + 'px';
      (PTA_PROCESS_Content_A1_P1_PROCESS[i] as HTMLElement).style.backgroundColor = "rgb(191,226,219)";

    }

    //set width box in a1
    for (let i = 0; i <= 6; i++) {
      var MODEL_T = "MODEL" + i
      var PTA_PROCESS_Content_A1_P1_ALL = document.querySelectorAll<HTMLElement>
        (".PTA .PTA_PROCESS_Content  .a1 .p1   ." + MODEL_T + " .GRADE , "
          + ".PTA .PTA_PROCESS_Content .a1 .p1  ." + MODEL_T + " .COLOR , "
          + ".PTA .PTA_PROCESS_Content .a1 .p1  ." + MODEL_T + " .BODY_NO ,"
          + ".PTA .PTA_PROCESS_Content .a3 .p1  ." + MODEL_T + " .GRADE ,"
          + ".PTA .PTA_PROCESS_Content .a3 .p1  ." + MODEL_T + " .COLOR , "
          + ".PTA .PTA_PROCESS_Content .a3 .p1  ." + MODEL_T + " .BODY_NO");

      for (let i = 0; PTA_PROCESS_Content_A1_P1_ALL[i]; i++) {
        (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.height = hRow_A1_P1_ALL + 'px';
        (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.borderTop = "1px solid #000";
        (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.fontSize = "50px";
        (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.fontWeight = "revert";
        (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.display = "flex";
        (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.justifyContent = "center";
        (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.alignItems = "center";
        (PTA_PROCESS_Content_A1_P1_ALL[i] as HTMLElement).style.textAlign = "center";
      }
    }

    //get class a2
    var PTA_PROCESS_Content_A2 = document.querySelectorAll<HTMLElement>('.PTA .PTA_PROCESS_Content .a2');
    for (let i = 0; PTA_PROCESS_Content_A2[i]; i++) {
      (PTA_PROCESS_Content_A2[i] as HTMLElement).style.width = w + 'px';
      (PTA_PROCESS_Content_A2[i] as HTMLElement).style.height = hRow_a_0 + 'px';
    }


  }

  bindDataBumperGetdataClrIndicator() {

    //bind data
    this.dataDataBumperGetdataClrIndicator.forEach(element => {
      var bindDataClrIndicator = (element as PtsAdoBumperGetdataClrIndicatorDto);
      console.log(bindDataClrIndicator);

      var $$ = document.querySelectorAll.bind(document);

      //get class A1 process
      let getClassprocessA1 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .PROCESS');

      //bind data
      for (let j = 0; j <= 6; j++) {
        let i = j + 1;
        if (getClassprocessA1 != null && bindDataClrIndicator.line == "LINE_1_" + i) {
          getClassprocessA1[j].innerHTML = bindDataClrIndicator.process;
        }
      }

      //   //get class a1 Model...
      let getClassGRADE = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL0 .GRADE');
      let getClassCOLOR = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL0 .COLOR');
      let getClassBODY_NO = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL0 .BODY_NO');

      //   //bind data
      if (getClassprocessA1 != null && bindDataClrIndicator.line == "LINE_1_1") {
        getClassBODY_NO[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a1 .p1 .MODEL0", bindDataClrIndicator.model);
      }

      //get class a1 Model1...
      let getClassGRADE1 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL1 .GRADE');
      let getClassCOLOR1 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL1 .COLOR');
      let getClassBODY_NO1 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL1 .BODY_NO');

      //bind data
      if (getClassprocessA1 != null && bindDataClrIndicator.line == "LINE_1_2") {
        getClassBODY_NO1[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE1[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR1[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a1 .p1 .MODEL1", bindDataClrIndicator.model);
      }

      //get class a1 Model2...
      let getClassGRADE2 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL2 .GRADE');
      let getClassCOLOR2 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL2 .COLOR');
      let getClassBODY_NO2 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL2 .BODY_NO');

      //bind data
      if (getClassprocessA1 != null && bindDataClrIndicator.line == "LINE_1_3") {
        getClassBODY_NO2[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE2[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR2[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a1 .p1 .MODEL2", bindDataClrIndicator.model);
      }

      //get class a1 Model3...
      let getClassGRADE3 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL3 .GRADE');
      let getClassCOLOR3 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL3 .COLOR');
      let getClassBODY_NO3 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL3 .BODY_NO');

      //bind data
      if (getClassprocessA1 != null && bindDataClrIndicator.line == "LINE_1_4") {
        getClassBODY_NO3[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE3[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR3[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a1 .p1 .MODEL3", bindDataClrIndicator.model);
      }

      //get class a1 Model4...
      let getClassGRADE4 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL4 .GRADE');
      let getClassCOLOR4 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL4 .COLOR');
      let getClassBODY_NO4 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL4 .BODY_NO');

      //bind data
      if (getClassprocessA1 != null && bindDataClrIndicator.line == "LINE_1_5") {
        getClassBODY_NO4[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE4[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR4[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a1 .p1 .MODEL4", bindDataClrIndicator.model);
      }

      //get class a1 Model5...
      let getClassGRADE5 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL5 .GRADE');
      let getClassCOLOR5 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL5 .COLOR');
      let getClassBODY_NO5 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL5 .BODY_NO');

      //bind data
      if (getClassprocessA1 != null && bindDataClrIndicator.line == "LINE_1_6") {
        getClassBODY_NO5[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE5[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR5[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a1 .p1 .MODEL5", bindDataClrIndicator.model);
      }


      //get class a1 Model5...
      let getClassGRADE6 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL6 .GRADE');
      let getClassCOLOR6 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL6 .COLOR');
      let getClassBODY_NO6 = $$('.PTA .PTA_PROCESS_Content .a1 .p1 .MODEL6 .BODY_NO');

      //bind data
      if (getClassprocessA1 != null && bindDataClrIndicator.line == "LINE_1_7") {
        getClassBODY_NO6[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE6[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR6[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a1 .p1 .MODEL6", bindDataClrIndicator.model);
      }


      //get class a3 process
      let getClassprocessA2 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .PROCESS');
      //bind data
      for (let j = 0; j <= 6; j++) {
        let i = j + 1;
        if (getClassprocessA2 != null && bindDataClrIndicator.line == "LINE_2_" + i) {
          getClassprocessA2[j].innerHTML = bindDataClrIndicator.process;
        }
      }

      //get class a3 Model...
      let getClassGRADE_A3_MODEL0 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL0 .GRADE');
      let getClassCOLOR_A3_MODEL0 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL0 .COLOR');
      let getClassBODY_NO_A3_MODEL0 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL0 .BODY_NO');

      //bind data
      if (getClassprocessA2 != null && bindDataClrIndicator.line == "LINE_2_1") {
        getClassBODY_NO_A3_MODEL0[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE_A3_MODEL0[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR_A3_MODEL0[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a3 .p1 .MODEL0", bindDataClrIndicator.model);
      }

      //get class a3 Model1...
      let getClassGRADE_A3_MODEL1 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL1 .GRADE');
      let getClassCOLOR_A3_MODEL1 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL1 .COLOR');
      let getClassBODY_NO_A3_MODEL1 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL1 .BODY_NO');

      //bind data
      if (getClassprocessA2 != null && bindDataClrIndicator.line == "LINE_2_2") {
        getClassBODY_NO_A3_MODEL1[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE_A3_MODEL1[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR_A3_MODEL1[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a3 .p1 .MODEL1", bindDataClrIndicator.model);
      }

      //get class a3 Model2...
      let getClassGRADE_A3_MODEL2 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL2 .GRADE');
      let getClassCOLOR_A3_MODEL2 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL2 .COLOR');
      let getClassBODY_NO_A3_MODEL2 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL2 .BODY_NO');

      //bind data
      if (getClassprocessA2 != null && bindDataClrIndicator.line == "LINE_2_3") {
        getClassBODY_NO_A3_MODEL2[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE_A3_MODEL2[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR_A3_MODEL2[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a3 .p1 .MODEL2", bindDataClrIndicator.model);
      }

      //get class a3 Model2...
      let getClassGRADE_A3_MODEL3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL3 .GRADE');
      let getClassCOLOR_A3_MODEL3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL3 .COLOR');
      let getClassBODY_NO_A3_MODEL3 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL3 .BODY_NO');

      //bind data
      if (getClassprocessA2 != null && bindDataClrIndicator.line == "LINE_2_4") {
        getClassBODY_NO_A3_MODEL3[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE_A3_MODEL3[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR_A3_MODEL3[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a3 .p1 .MODEL3", bindDataClrIndicator.model);
      }

      //get class a3 Model2...
      let getClassGRADE_A3_MODEL4 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL4 .GRADE');
      let getClassCOLOR_A3_MODEL4 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL4 .COLOR');
      let getClassBODY_NO_A3_MODEL4 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL4 .BODY_NO');

      //bind data
      if (getClassprocessA2 != null && bindDataClrIndicator.line == "LINE_2_5") {
        getClassBODY_NO_A3_MODEL4[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE_A3_MODEL4[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR_A3_MODEL4[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a3 .p1 .MODEL4", bindDataClrIndicator.model);
      }

      //get class a3 Model2...
      let getClassGRADE_A3_MODEL5 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL5 .GRADE');
      let getClassCOLOR_A3_MODEL5 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL5 .COLOR');
      let getClassBODY_NO_A3_MODEL5 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL5 .BODY_NO');

      //bind data
      if (getClassprocessA2 != null && bindDataClrIndicator.line == "LINE_2_6") {
        getClassBODY_NO_A3_MODEL5[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE_A3_MODEL5[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR_A3_MODEL5[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a3 .p1 .MODEL5", bindDataClrIndicator.model);
      }

      //get class a3 Model2...
      let getClassGRADE_A3_MODEL6 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL6 .GRADE');
      let getClassCOLOR_A3_MODEL6 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL6 .COLOR');
      let getClassBODY_NO_A3_MODEL6 = $$('.PTA .PTA_PROCESS_Content .a3 .p1 .MODEL6 .BODY_NO');

      //bind data
      if (getClassprocessA2 != null && bindDataClrIndicator.line == "LINE_2_7") {
        getClassBODY_NO_A3_MODEL6[0].innerHTML = bindDataClrIndicator.bodyNo;
        getClassGRADE_A3_MODEL6[0].innerHTML = bindDataClrIndicator.grade;
        getClassCOLOR_A3_MODEL6[0].innerHTML = bindDataClrIndicator.color;
        this.setColor(".PTA .PTA_PROCESS_Content .a3 .p1 .MODEL6", bindDataClrIndicator.model);
      }
    });
  }

  setColor(obj: any, model) {
    let a = document.querySelectorAll(obj)
    switch (model) {
      case "K":
        for (let i = 0; i < a.length; i++) {
          a[i].style.backgroundColor = "#FE97CA";
        };
        break;
      case "F":
        for (let i = 0; i < a.length; i++) {
          a[i].style.backgroundColor = "#FFFF04";
        }; break;
      case "C":
        for (let i = 0; i < a.length; i++) {
          a[i].style.backgroundColor = "#8BB2E1";
        }; "#8BB2E1"; break;
      case "V":
        for (let i = 0; i < a.length; i++) {
          a[i].style.backgroundColor = "#F9BD8D";
        };
        break;
      case "I":
        for (let i = 0; i < a.length; i++) {
          a[i].style.backgroundColor = "#90CE50";
        };
        break;
      default:
        for (let i = 0; i < a.length; i++) {
          a[i].style.backgroundColor = "transparent";
        };
    }


  }

}
