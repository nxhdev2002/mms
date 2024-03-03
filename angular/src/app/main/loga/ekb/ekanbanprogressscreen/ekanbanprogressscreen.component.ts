import { Component, HostListener, Injector, OnInit } from '@angular/core';
import { CommonFunction } from '@app/main/commonfuncton.component';
import { LgaEkbEkanbanProgressScreenDto, LgaEkbEkanbanProgressScreenServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-ekanbanprogressscreen',
  templateUrl: './ekanbanprogressscreen.component.html',
  styleUrls: ['./ekanbanprogressscreen.component.less']
})
export class EkanbanProgressScreenComponent implements OnInit {
  prodLine;
  taktTime;
  title_ekb;
  totalCycle: number = 0;
  countData: number = 0;
  pickingUser;
  configData: LgaEkbEkanbanProgressScreenDto[] = []

  constructor(injector: Injector,
              private _serviceProxy: LgaEkbEkanbanProgressScreenServiceProxy,) {
  }

  ngOnInit(): void {
    console.log('1: ngOnInit');
    let urlParams = new URLSearchParams(window.location.search);
    this.prodLine = urlParams.get('pline');

    this._serviceProxy.getConfigScreen(this.prodLine)
      .subscribe((result) => {
        if (result) {
          this.configData = result;
          this.countData = this.configData.length;
          if (this.countData > 0) {
            this.totalCycle = this.configData[0].totalCycle;
            this.title_ekb = this.configData[0].title;
          }
          setTimeout(() => {
            this.loadForm();
          }, 500);
        }
      });
  }

  ngAfterViewInit() {

    console.log('2: ngAfterViewInit');
    this.repeatBindData();
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
  refeshPage: number = 600; // 10phut
  timer_repeat;
  secondDelay = 1000;
  fn: CommonFunction = new CommonFunction();
  repeatBindData() {

    if (this.timecount > this.refeshPage) window.location.reload();
    this.timecount = this.timecount + 1;
    try {

      this.getData();
      this.fn.showtime('time_now_log');

    } catch (ex) {
      console.log(ex);

      this.timer_repeat = setTimeout(() => {
        this.repeatBindData();
      }, this.secondDelay);
    }
  }

  getData() {
    this._serviceProxy.getDataEkabanProgressScreen(this.prodLine)
      .subscribe((result) => {
        if (result) {
          this.bindData(result);
        }

        this.timer_repeat = setTimeout(() => {
          this.repeatBindData();
        }, this.secondDelay);
      });
  }

  @HostListener('window:resize', ['$event'])
  onWindowResize() {
    this.loadForm();
  }

  loadForm() {
    var w = window.innerWidth;
    var h = window.innerHeight;
    var hPick = h / 100 * 92 / this.countData;
    var wNumCyc = (w / 100 * 82 - 10) / this.totalCycle;

    var EKP_PRG = document.querySelectorAll<HTMLElement>('.EKP_PRG');
    for (let i = 0; EKP_PRG[i]; i++) {
      EKP_PRG[i].style.width = w + 'px';
      EKP_PRG[i].style.height = h + 'px';
    }

    var CONTENT = document.querySelectorAll<HTMLElement>('.EKP_PRG .BODY .CONTENT');
    for (let i = 0; CONTENT[i]; i++) {
      CONTENT[i].style.width = w + 'px';
      CONTENT[i].style.height = hPick + 'px';
    }

    var cyctime = document.querySelectorAll<HTMLElement>('.EKP_PRG .BODY .CONTENT .cycletime .number_cycletime .cyctime,.EKP_PRG .BODY .CONTENT .pick .right_pick .process_no');
    for (let i = 0; cyctime[i]; i++) {
      cyctime[i].style.width = wNumCyc + 'px';
    }



  }

  bindData(_data: LgaEkbEkanbanProgressScreenDto[]) {
    //clear
    let _cycle_no = document.querySelectorAll<HTMLElement>('.EKP_PRG .BODY .CONTENT .pick .right_pick .process_no');
    for (let i = 0; _cycle_no[i]; i++)
    {
      _cycle_no[i].classList.remove('FINISHED', 'DELAYED');
    //   _cycle_no[i].classList.remove('DELAYED');
    }

    if (_data.length > 0) {
      //tacktime
      let _tt = "T.T : " + (_data[0].taktTime * 1 / 60).toFixed(1) + "'";
      let _txtHeader = document.querySelector<HTMLElement>('.EKP_PRG .HEADER .takttime');
      if (_txtHeader) _txtHeader.textContent = _tt;
    }

    //bind data
    for (let i = 0; _data[i]; i++) {

       //efficiency
       let _cssEfficiency = '.EKP_PRG .BODY .CONTENT.PK_'+_data[i].sorting+' .pick .left_pick .left .bot';
       let _valEfficiency = document.querySelector<HTMLElement>(_cssEfficiency);
       if(_valEfficiency){
        _valEfficiency.textContent = _data[i].efficiency.toString() + '%';
       }

        //pickingUser
        let _cssPickingUser = '.EKP_PRG .BODY .CONTENT.PK_'+_data[i].sorting+' .pick .left_pick .right';
        if(_cssPickingUser){
         let _valPickingUser = document.querySelector<HTMLElement>(_cssPickingUser);
         _valPickingUser.textContent = _data[i].pickingUser.toString().replace('_',' ');
        }

      if (_data[i].status == "FINISHED") {
        //process
        let _cssProcess = '.EKP_PRG .BODY .CONTENT.PK_'+_data[i].sorting+' .pick .right_pick .process_no.NO_' + _data[i].numberNo;
        let _noProcess = document.querySelector<HTMLElement>(_cssProcess);
        if (_noProcess) {
          _noProcess.classList.remove("STARTED", "FINISHED", "DELAYED", "NEWTAKT", "COMPLETED");
          _noProcess.classList.add("FINISHED");
        }
      }

      if (_data[i].isDelay == "Y") {

        let _delayFinish = (_data[i].delaySecond * 1 / 60).toFixed(1) + "'";
        let _cssDelay = '.EKP_PRG .BODY .CONTENT.PK_'+_data[i].sorting+' .pick .right_pick .process_no.NO_' + _data[i].numberNo;
        let _noTask = document.querySelector<HTMLElement>(_cssDelay);
        if(_noTask) {
        _noTask.classList.remove("STARTED", "FINISHED", "DELAYED", "NEWTAKT", "COMPLETED");
        _noTask.classList.add("DELAYED");
        _noTask.textContent = _delayFinish;
      }

      }
      else{
        let _cssDelay = '.EKP_PRG .BODY .CONTENT.PK_'+_data[i].sorting+' .pick .right_pick .process_no.NO_' + _data[i].numberNo;
        let _noTask = document.querySelector<HTMLElement>(_cssDelay);
        if(_noTask) {_noTask.textContent = '';}
      }
    }
  }
}
