import { LgwMwhPxpModuleInputAndonServiceProxy, GetPxpModuleInputAndonLayoutOutput, GetPxpModuleInputAndonDataOutput } from '@shared/service-proxies/service-proxies';

export class ModuleFunctionComponent {

    drawHeightContent(){
        let h = window.innerHeight - 12;
        let hline = Math.floor((h / 18) * 100) / 100;   //Title Location
        let hLayout = h - hline - 6; //6 = padding-bottom
        let hcontent = hLayout - hline - 12;

        let _loc_btn = document.querySelectorAll<HTMLElement>('.LOCATION_BTN .loc, .RLL_CASE .option, '+
                                                              '.LOCATION_BTN .btnLoc, .LOCATION_BTN .btnLoc .flag, .REMARK');
        for(let i=0; _loc_btn[i]; i++){ _loc_btn[i].style.height = hline  + 'px'; }

        let _rll_case = document.querySelectorAll<HTMLElement>('.RLL_CASE .select');
        for(let i=0; _rll_case[i]; i++){ _rll_case[i].style.marginTop = (hline + 5) + 'px'; }

        let _layout = document.querySelectorAll<HTMLElement>('.Layout');
        for(let i=0; _layout[i]; i++){ _layout[i].style.height = (hLayout) + 'px'; }

        let _hcontent = document.querySelectorAll<HTMLElement>('.Layout .pxp_big_content .content_tab');
        for(let i=0; _hcontent[i]; i++){ _hcontent[i].style.height = (hcontent) + 'px'; }

        let _htab = document.querySelectorAll<HTMLElement>('.Layout .pxp_big_tab');
        for(let i=0; _htab[i]; i++){ _htab[i].style.height = (hline - 2) + 'px'; }

        return hcontent;
    }

    drawHeightTab(cssClass:String, h:number, rowcount: number){

        cssClass = ' .' + cssClass;

        let hcont = document.querySelector<HTMLElement>('.Layout .pxp_big_content .content_tab ' + cssClass);
        hcont.style.height = (h) + 'px';

        let hrowitem = Math.floor((h / rowcount) * 100) / 100;
        let rowitem = document.querySelectorAll<HTMLElement>(cssClass + ' .box.firstRow, ' +
                                                                                                cssClass + ' .box.firstRow .flag, ' +
                                                                                                cssClass + ' .box.firstRow .first_sub, ' +
                                                                                                cssClass + ' .box:not(.firstRow), ' +
                                                                                                cssClass + ' .box.cellData, ' +
                                                                                                cssClass + ' .box.cellData .flag, ' +
                                                                                                cssClass + ' .box.cellData .boxCellText');
        for(let i=0; rowitem[i]; i++){
            rowitem[i].style.height = (hrowitem - 0.5) + 'px';
            rowitem[i].style.lineHeight = (hrowitem - 0.5) + 'px';
        }

        let flagmin  = document.querySelectorAll<HTMLElement>(cssClass + ' .box.cellData .flag_min');
        let hflag = (hrowitem / 3);
        for(let i=0; flagmin[i]; i++){ flagmin[i].style.height = hflag+ 'px'; }
    }

    drawWidthTab(cssClass:String, w:number, columncount: number) {
        cssClass = ' .' + cssClass;

        columncount = (columncount<=10) ? 10: columncount;
        let witem = Math.floor((w/ columncount) * 100) / 100;
        let colitem =  document.querySelectorAll<HTMLElement>(cssClass + ' .box.txt, ' +
                                                                                                cssClass + ' .box.firstColumn, ' +
                                                                                                cssClass + ' .box:not(.txt):not(.firstColumn),' +
                                                                                                cssClass + ' .box.cellData, ' +
                                                                                                cssClass + ' .box.cellData .box_sub, ' +
                                                                                                cssClass + ' .box.cellData .box_sub .boxCellText, ' +
                                                                                                cssClass + ' .flag');
        for(let i=0; colitem[i]; i++){ colitem[i].style.width = witem - 1 + 'px'; }

    }

    drawBoxNumberPopup(CSS_POPUP_NAME:string){

        let  hcontent2 = 220; //Math.floor((hcontent / 2) * 100) / 100;
        let hbox = Math.floor((hcontent2 / 4) * 100) / 100;


        let boxnumber = document.querySelectorAll<HTMLElement>(CSS_POPUP_NAME + ' .number .box');
        for(let i=0; boxnumber[i]; i++){
            boxnumber[i].style.height = hbox - 1 + 'px';
            boxnumber[i].style.lineHeight = hbox - 1  + 'px';
         }

         let lotcode = document.querySelector<HTMLElement>(CSS_POPUP_NAME+ ' .lotcode');
         if(lotcode) lotcode.style.height = hcontent2 + 'px';

         let lotitem = document.querySelectorAll<HTMLElement>(CSS_POPUP_NAME + ' .lotcode .lotcodeitem');
         for(let i=0; lotitem[i]; i++){
            lotitem[i].style.height = hbox - 1 + 'px';
            lotitem[i].style.lineHeight = hbox - 1  + 'px';
         }
    }

    changetab(tab:String){

        let _tab = document.querySelectorAll<HTMLElement>('.pxp_big_tab .title_tab, .pxp_big_content .content_tab');
        for(let i=0; _tab[i]; i++){ _tab[i].classList.remove('active'); }
        _tab = document.querySelectorAll<HTMLElement>('.pxp_big_tab .title_tab.tab' + tab + ', .pxp_big_content .content_tab._tab' + tab);
        for(let i=0; _tab[i]; i++){ _tab[i].classList.add('active'); }

    }

    getRowCount(result: GetPxpModuleInputAndonLayoutOutput[]){
        return Math.max(...result.map(function(o) { return o.rowId; })) + 1;
    }

    getRow(result: GetPxpModuleInputAndonLayoutOutput[], rowindex:number){
        if(result) return result.filter(a=> a.rowId == rowindex);
        return [];
    }

    fornumbersRangeDesc(start:number, stop:number, step:number){
        let numRangeDesc: number[] = [];
        for (let i = start; i >= stop;) {
            numRangeDesc.push(i);
            i = i + step;
        }
        return numRangeDesc;
    }


    fornumbers(num:number) {
        let numbers:Array<any> = Array.from({length:num},(v,k)=>k+1);
        return numbers;
    }

    setAttributes(element:Element, attributes:any) {
        Object.keys(attributes).forEach(attr => {
          element.setAttribute(attr, attributes[attr]);
        });
    }

    showtime(css_time:string){
        let _d = new Date();
        let _time = document.querySelector<HTMLElement>('.' + css_time);
        if(_time) _time.textContent = _d.getHours() + ":" + _d.getMinutes() + ":" + _d.getSeconds();
    }
}
