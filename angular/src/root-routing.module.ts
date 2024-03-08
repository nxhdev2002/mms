import { NgModule } from '@angular/core';
import { NavigationEnd, Router, RouterModule, Routes } from '@angular/router';
import { AppUiCustomizationService } from '@shared/common/ui/app-ui-customization.service';

const routes: Routes = [
    { path: '', redirectTo: '/app/main/dashboard', pathMatch: 'full' },
    {
        path: 'account',
        loadChildren: () => import('account/account.module').then((m) => m.AccountModule), //Lazy load account module
        data: { preload: true },
    },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'assemblyscreen',
    //             loadChildren: () => import('@app/main/assy/sps/assemblyscreen/assemblyscreen.module').then(m => m.AssemblyScreenModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },

    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'assemblydatascreen',
    //             loadChildren: () => import('app/main/assy/sps/assemblydatascreen/assemblydatascreen.module').then(m => m.AssemblyDataScreenModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'bigparttablet2',
    //             loadChildren: () => import('app/main/loga/bp2/bigparttablet2/bigparttablet2.module').then(m => m.BigPartTablet2Module), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },

    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'lotdirectsupplyandon',
    //             loadChildren: () => import('app/main/loga/Lds/lotdirectsupplyandon/lotdirectsupplyandon.module').then(m => m.LotDirectSupplyAndonModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'jiririkokuscreen',
    //             loadChildren: () => import('app/main/loga/Lds/jiririkokuscreen/jiririkokuscreen.module').then(m => m.JiririkokuScreenModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },

    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'bigpartpxpup',
    //             loadChildren: () => import('app/main/loga/bp2/bigpartpxpup/bigpartpxpup.module').then(m => m.PxPUpPlanModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'LgaBp2ProgressScreen',
    //             loadChildren: () => import('@app/main/loga/bp2/LgaBp2ProgressScreen/LgaBp2ProgressScreen.module').then(m => m.LgaBp2ProgressScreenModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'LgaBp2ProgressMonitorScreen',
    //             loadChildren: () => import('@app/main/loga/bp2/LgaBp2ProgressMonitorScreen/LgaBp2ProgressMonitorScreen.module').then(m => m.LgaBp2ProgressMonitorScreenModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'bigpartdirectdeliveryprogressandon',
    //             loadChildren: () => import('app/main/loga/bp2/bigpartdirectdeliveryprogressandon/bigpartdirectdeliveryprogressandon.module').then(m => m.BigPartDirectDeliveryProgressAndonModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'progressscreen',
    //             loadChildren: () => import('@app/main/loga/ekb/progressscreen/progressscreen.module').then(m => m. ProgressScreenModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // // {
    // //     path: 'screens',
    // //     children: [
    // //         {
    // //             path: 'ekanban',
    // //             loadChildren: () => import('@app/main/loga/ekb/ekanban/ekanban.module').then(m => m.EkanbanModule), //Lazy load main module
    // //             data: { preload: true }
    // //         },
    // //     ]
    // // },

    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'ekanbanprogressscreen',
    //             loadChildren: () => import('@app/main/loga/ekb/ekanbanprogressscreen/ekanbanprogressscreen.module').then(m => m.EkanbanProgressScreenModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // // {
    // //     path: 'screens',
    // //     children: [
    // //         {
    // //             path: 'ekanbansupplymonitoring',
    // //             loadChildren: () => import('@app/main/loga/ekb/ekanbansupplymonitoring/ekanbansupplymonitoring.module').then(m => m.EkanbansupplymonitoringModule),//Lazy load main module
    // //             data: { preload: true }
    // //         },
    // //     ]
    // // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'bigpartjiririkokuscreen',
    //             loadChildren: () => import('app/main/loga/bp2/bigpartjiririkokuscreen/bigpartjiririkokuscreen.module').then(m => m.BigPartJiririkokuScreenModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'ccrmonitor',
    //             loadChildren: () => import('app/main/painting/andon/ccrmonitor/ccrmonitor.module').then(m => m.CCRMonitorModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'bumpergetdatasmallsubassy',
    //             loadChildren: () => import('app/main/painting/andon/bumpergetdatasmallsubassy/bumpergetdatasmallsubassy.module').then(m => m.BumpergetdatasmallsubassyModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'ccrmonitor',
    //             loadChildren: () => import('app/main/painting/andon/ccrmonitor/ccrmonitor.module').then(m => m.CCRMonitorModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'bumpergetdatasmallsubassy',
    //             loadChildren: () => import('app/main/painting/andon/bumpergetdatasmallsubassy/bumpergetdatasmallsubassy.module').then(m => m.BumpergetdatasmallsubassyModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'linerealtimecontrols',
    //             loadChildren: () => import('app/main/painting/andon/linerealtimecontrol/linerealtimecontrol.module').then(m => m.LineRealTimeControlModule),//Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'processinstruction',
    //             loadChildren: () => import('app/main/welding/andon/processinstruction/processinstruction.module').then(m => m.ProcessInstructionModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'punchqueueindicatorV2',
    //             loadChildren: () => import('app/main/welding/andon/punchqueueindicatorv2/punchqueueindicatorv2.module').then(m => m.Punchqueueindicatorv2Module), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },

    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'punchqueueindicator',
    //             loadChildren: () => import('app/main/welding/andon/punchqueueindicator/punchqueueindicator.module').then(m => m.PunchQueueIndicatorModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },


    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'bumperin',
    //             loadChildren: () => import('@app/main/painting/andon/bumperin/bumperin.module').then(m => m.BumperInModule),//Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'bumpersubassy',
    //             loadChildren: () => import('@app/main/painting/andon/bumpersubassy/bumpersubassy.module').then(m => m.BumperSubAssyModule),//Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },

    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'nextedin',
    //             loadChildren: () => import('app/main//painting/andon/nextedin/nextedin.module').then(m => m.NextEdInModule),//Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },

    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'bumpergetdataclrindicator',
    //             loadChildren: () => import('app/main//painting/andon/bumpergetdataclrindicator/bumpergetdataclrindicator.module').then(m => m.BumpergetdataclrindicatorModule),//Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },

    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'delaycontrolscreen',
    //             loadChildren: () => import('app/main//painting/andon/delaycontrolscreen/delaycontrolscreen.module').then(m => m.DelaycontrolscreenModule),//Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'pickingmonitoringscreen',
    //             loadChildren: () => import('app/main/logw/pik/pickingmonitoringscreen/pickingmonitoringscreen.module').then(m => m.PickingmonitoringScreenModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'assemblyscreen',
    //             loadChildren: () => import('@app/main/assy/sps/assemblyscreen/assemblyscreen.module').then(m => m.AssemblyScreenModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'lotupackingandon',
    //             loadChildren: () => import('app/main/logw/lup/lotupackingandon/lotupackingandon.module').then(m => m.LotUpackingAndonModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'pxpsmallpartinput',
    //             loadChildren: () => import('app/main/logw/mwh/pxpsmallpartinput/pxpsmallpartinput.module').then(m => m.PxpsmallpartinputModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'pxpbigpartinput',
    //             loadChildren: () => import('app/main/logw/mwh/pxpbigpartinput/pxpbigpartinput.module').then(m => m.PxpbigpartinputModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'pickingtabletandon',
    //             loadChildren: () => import('app/main/logw/pik/pickingtabletandon2/pickingtabletandon2.module').then(m => m.PickingTabletAndon2Module), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'pickingtabletandon2',
    //             loadChildren: () => import('app/main/logw/pik/pickingtabletandon2/pickingtabletandon2.module').then(m => m.PickingTabletAndon2Module), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'pickingmonitoringscreen',
    //             loadChildren: () => import('app/main/logw/pik/pickingmonitoringscreen/pickingmonitoringscreen.module').then(m => m.PickingmonitoringScreenModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'callinglight',
    //             loadChildren: () => import('app/main/logw/ado/callinglight/callinglight.module').then(m => m.CallinglightModule), //Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'moduleunpackingandon',
    //             loadChildren: () => import('app/main/logw/pup/moduleunpackingandon/module-unpacking-andon-screen.module').then(m => m.ModuleUnpackingAndonModule),//Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },
    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'devanningscreen',
    //             loadChildren: () => import('app/main/logw/dvn/devanningscreen/devanning-screen.module').then(m => m.DevanningScreenModule),//Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },

    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'stockatwh',
    //             loadChildren: () => import('@app/main/logw/mwh/stock/stock.module').then(m => m.StockModule),//Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },

    // {
    //     path: 'screens',
    //     children: [
    //         {
    //             path: 'lotmakingtablet',
    //             loadChildren: () => import('app/main/logw/lup/lotmakingtablet/lotmakingtablet.module').then(m => m.LotMakingTabletModule),//Lazy load main module
    //             data: { preload: true }
    //         },
    //     ]
    // },

    { path: '**', redirectTo: '/app/main/dashboard' },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: [],
})
export class RootRoutingModule {
    constructor(private router: Router, private _uiCustomizationService: AppUiCustomizationService) {
        router.events.subscribe((event: NavigationEnd) => {
            setTimeout(() => {
                this.toggleBodyCssClass(event.url);
            }, 0);
        });
    }

    toggleBodyCssClass(url: string): void {
        if (url) {
            if (url === '/') {
                if (abp.session.userId > 0) {
                    this.setAppModuleBodyClassInternal();
                } else {
                    this.setAccountModuleBodyClassInternal();
                }
            }

            if (url.indexOf('/account/') >= 0) {
                this.setAccountModuleBodyClassInternal();
            } else {
                this.setAppModuleBodyClassInternal();
            }
        }
    }

    setAppModuleBodyClassInternal(): void {
        let currentBodyClass = document.body.className;
        let classesToRemember = '';

        if (currentBodyClass.indexOf('brand-minimize') >= 0) {
            classesToRemember += ' brand-minimize ';
        }

        if (currentBodyClass.indexOf('aside-left-minimize') >= 0) {
            classesToRemember += ' aside-left-minimize';
        }

        if (currentBodyClass.indexOf('brand-hide') >= 0) {
            classesToRemember += ' brand-hide';
        }

        if (currentBodyClass.indexOf('aside-left-hide') >= 0) {
            classesToRemember += ' aside-left-hide';
        }

        if (currentBodyClass.indexOf('swal2-toast-shown') >= 0) {
            classesToRemember += ' swal2-toast-shown';
        }

        document.body.className = this._uiCustomizationService.getAppModuleBodyClass() + ' ' + classesToRemember;
    }

    setAccountModuleBodyClassInternal(): void {
        let currentBodyClass = document.body.className;
        let classesToRemember = '';

        if (currentBodyClass.indexOf('swal2-toast-shown') >= 0) {
            classesToRemember += ' swal2-toast-shown';
        }

        document.body.className = this._uiCustomizationService.getAccountModuleBodyClass() + ' ' + classesToRemember;
    }

    getSetting(key: string): string {
        return abp.setting.get(key);
    }
}
