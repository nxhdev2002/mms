import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    {
                        path: 'home',
                        component: HomeComponent
                    },
                    {
                        path: 'dashboard',
                        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
                        data: { permission: 'Pages.Tenant.Dashboard' },
                    },
                    // {
                    //     path: 'cmm/reportrequest',
                    //     loadChildren: () => import('./cmm/report-request/report-request.module').then(m => m.ReportRequestModule),
                    //     data: { permission: 'Pages.Cmm.ReportRequest' }
                    // },

//NEW
//WORKING PATTERN
                    // {
                    //     path: 'master/workingpattern/calendar',
                    //     loadChildren: () => import('./master/workingpattern/calendar/calendar.module').then(m => m.CalendarModule),
                    //     data: { permission: 'Pages.WorkingPattern.Calendar.View' }
                    // },
                    // {
                    //     path: 'master/workingpattern/seasonmonth',
                    //     loadChildren: () => import('./master/workingpattern/seasonmonth/seasonmonth.module').then(m => m.SeasonmonthModule),
                    //     data: { permission: 'Pages.WorkingPattern.SeasonMonth.View' }
                    // },
                    // {
                    //     path: 'master/workingpattern/patternh',
                    //     loadChildren: () => import('./master/workingpattern/patternh/patternh.module').then(m => m.PatternHModule),
                    //     data: { permission: 'Pages.WorkingPattern.PatternH.View' }
                    // },
                    // {
                    //     path: 'master/workingpattern/patternd',
                    //     loadChildren: () => import('./master/workingpattern/patternd/patternd.module').then(m => m.PatternDModule),
                    //     data: { permission: 'Pages.WorkingPattern.PatternD.View' }
                    // },
                    // {
                    //     path: 'master/workingpattern/workingtime',
                    //     loadChildren: () => import('./master/workingpattern/workingtime/workingtime.module').then(m => m.WorkingTimeModule),
                    //     data: { permission: 'Pages.WorkingPattern.WorkingTime.View' }
                    // },
                    // {
                    //     path: 'master/workingpattern/workingtype',
                    //     loadChildren: () => import('./master/workingpattern/workingtype/workingtype.module').then(m => m.WorkingTypeModule),
                    //     data: { permission: 'Pages.WorkingPattern.WorkingType.View' }
                    // },
                    // {
                    //     path: 'master/workingpattern/shop',
                    //     loadChildren: () => import('./master/workingpattern/shop/shop.module').then(m => m.ShopModule),
                    //     data: { permission: 'Pages.Master.Common.Shop.View' }
                    // },
                    // {
                    //     path: 'master/workingpattern/dailyworkingtime',
                    //     loadChildren: () => import('./master/workingpattern/dailyworkingtime/dailyworkingtime.module').then(m => m.DailyWorkingTimeModule),
                    //     data: { permission: 'Pages.WorkingPattern.DailyWorkingTime.View' }
                    // },
                    // {
                    //     path: 'master/workingpattern/week',
                    //     loadChildren: () => import('./master/workingpattern/Week/week.module').then(m => m.WeekModule),
                    //     data: { permission: 'Pages.WorkingPattern.Week.View' }
                    // },

//MASTER
                    //Common
                    // {
                    //     path: 'master/common/exchangerate',
                    //     loadChildren: () => import('./master/common/exchangerate/exchangerate.module').then(m => m.ExchangeRateModule),
                    //     data: { permission: 'Pages.Master.Common.ExchangeRate.View' }
                    // },
                    // {
                    //     path: 'master/common/devanningcasetype',
                    //     loadChildren: () => import('./master/common/devanningcasetype/devanningcasetype.module').then(m => m.DevanningCaseTypeModule),
                    //     data: { permission: 'Pages.Master.Common.DevanningCaseType.View' }
                    // },
                    // {
                    //     path: 'master/common/model',
                    //     loadChildren: () => import('./master/common/model/model.module').then(m => m.ModelModule),
                    //     data: { permission: 'Pages.Master.Common.Model.View' }
                    // },
                    // {
                    //     path: 'master/common/takttime',
                    //     loadChildren: () => import('./master/common/takttime/takttime.module').then(m => m.TakttimeModule),
                    //     data: { permission: 'Pages.Master.Common.TaktTime.View' }
                    // },
                    // {
                    //     path: 'master/common/color',
                    //     loadChildren: () => import('./master/common/color/color.module').then(m => m.ColorModule),
                    //     data: { permission: 'Pages.Master.Common.Color.View' }
                    // },
                    // {
                    //     path: 'master/common/takttime',
                    //     loadChildren: () => import('./master/common/takttime/takttime.module').then(m => m.TakttimeModule),
                    //     data: { permission: 'Pages.Master.Common.TaktTime.View' }
                    // },
                    // {
                    //     path: 'master/common/shoptype',
                    //     loadChildren: () => import('./master/common/shoptype/shoptype.module').then(m => m.ShopTypeModule),
                    //     data: { permission: 'Pages.Master.Common.ShopType.View' }
                    // },
                    // {
                    //     path: 'master/common/vehiclecolortype',
                    //     loadChildren: () => import('./master/common/vehiclecolortype/vehiclecolortype.module').then(m => m.VehicleColorTypeModule),
                    //     data: { permission: 'Pages.Master.Common.VehicleColorType.View' }
                    // },
                    // {
                    //     path: 'master/common/mmcheckingrule',
                    //     loadChildren: () => import('./master/common/mmcheckingrule/mmcheckingrule.module').then(m => m.MMCheckingRuleModule),
                    //     data: { permission: 'Pages.Master.Cmm.MMCheckingRule.View' }
                    // },
                    // {
                    //     path: 'master/common/mmvalidationresult',
                    //     loadChildren: () => import('./master/common/mmvalidationresult/mmvalidationresult.module').then(m => m.CmmMMValidationResultModule),
                    //     data: { permission: 'Pages.Master.Cmm.MMValidationResult.View' }
                    // },
                    // {
                    //     path: 'master/common/vehiclecbu',
                    //     loadChildren: () => import('./master/common/vehiclecbu/vehiclecbu.module').then(m => m.VehicleCBUModule),
                    //     data: { permission: 'Pages.Master.Cmm.VehicleCBU.View' }
                    // },
                    // {
                    //     path: 'master/common/lookup',
                    //     loadChildren: () => import('./master/common/lookup/lookup.module').then(m => m.LookupModule),
                    //     data: { permission: 'Pages.Master.Common.Lookup.View' }
                    // },
                    // {
                    //     path: 'master/common/enginemaster',
                    //     loadChildren: () => import('./master/common/enginemaster/enginemaster.module').then(m => m.EngineMasterModule),
                    //     data: { permission: 'Pages.Master.Cmm.EngineMaster.View' }
                    // },
                    // {
                    //     path: 'master/common/businessparter',
                    //     loadChildren: () => import('./master/common/businessparter/businessparter.module').then(m => m.BusinessParterModule),
                    //     data: { permission: 'Pages.Master.Cmm.BusinessParter.View' }
                    // },
                    // {
                    //     path: 'master/common/productgroup',
                    //     loadChildren: () => import('./master/common/productgroup/productgroup.module').then(m => m.ProductGroupModule),
                    //     data: { permission: 'Pages.Master.Cmm.ProductGroup.View' }
                    // },
                    // {
                    //     path: 'master/common/vehiclename',
                    //     loadChildren: () => import('./master/common/vehiclename/vehiclename.module').then(m => m.VehicleNameModule),
                    //     data: { permission: 'Pages.Master.Common.VehicleName.View' }
                    // },

                    //CKD


                    {
                        path: 'master/inventory/customsport',
                        loadChildren: () => import('./master/inventory/customsport/customsport.module').then(m => m.CustomsPortModule),
                        data: { permission: 'Pages.Master.Ckd.CustomsPort.View' }
                    },
                    {
                    path: 'master/inventory/shippingcompany',
                    loadChildren: () => import('./master/inventory/shippingcompany/shippingcompany.module').then(m => m.ShippingCompanyModule),
                    data: { permission: 'Pages.Master.Ckd.ShippingCompany.View' }
                },
                {
                    path: 'master/inventory/containerstatus',
                    loadChildren: () => import('./master/inventory/containerstatus/containerstatus.module').then(m => m.ContainerStatusModule),
                    data: { permission: 'Pages.Master.Ckd.ContainerStatus.View' }
                },
                {
                    path: 'master/inventory/containerdeliverytype',
                    loadChildren: () => import('./master/inventory/containerdeliverytype/containerdeliverytype.module').then(m => m.ContainerDeliveryTypeModule),
                    data: { permission: 'Pages.Master.Ckd.ContainerDeliveryType.View' }
                },
                {
                    path: 'master/inventory/forwarder',
                    loadChildren: () => import('./master/inventory/forwarder/forwarder.module').then(m => m.ForwarderModule),
                    data: { permission: 'Pages.Master.Ckd.Forwarder.View' }
                },
                {
                    path: 'master/inventory/invoicestatus',
                    loadChildren: () => import('./master/inventory/invoicestatus/invoicestatus.module').then(m => m.InvoiceStatusModule),
                    data: { permission: 'Pages.Master.Ckd.InvoiceStatus.View' }
                },
                {
                    path: 'master/inventory/customsstatus',
                    loadChildren: () => import('./master/inventory/customsstatus/customsstatus.module').then(m => m.CustomsStatusModule),
                    data: { permission: 'Pages.Master.Ckd.CustomsStatus.View' }
                },
                {

                    path: 'master/inventory/supplierlist',
                    loadChildren: () => import('./master/inventory/supplierlist/supplierlist.module').then(m => m.SupplierListModule),
                    data: { permission: 'Pages.Master.Inventory.SupplierList.View' }
                },
                {
                    path: 'master/inventory/customsleadtime',
                    loadChildren: () => import('./master/inventory/customsleadtime/customsleadtime.module').then(m => m.CustomsLeadtimeModule),
                    data: { permission: 'Pages.Master.CKD.CustomsLeadtime.View' }
                },
                {
                    path: 'master/inventory/pioparttype',
                    loadChildren: () => import('./master/inventory/pioparttype/pioparttype.module').then(m => m.PIOPartTypeModule),
                    data: { permission: 'Pages.Master.Inventory.PIOPartType.View' }
                },
                {
                    path: 'master/inventory/pioimpsupplier',
                    loadChildren: () => import('./master/inventory/pioimpsupplier/mstpioimpsupplier.module').then(m => m.PioImpSupplierModule),
                    data: { permission: 'Pages.Master.MstPioImpSupplier.View' }
                },
                {
                    path: 'master/inventory/pioemail',
                    loadChildren: () => import('./master/inventory/pioemail/pioemail.module').then(m => m.PIOEmailModule),
                    data: { permission: 'Pages.Master.Inventory.PIOEmail.View' }
                },

                {
                    path: 'master/inventory/customsleadtimemaster',
                    loadChildren: () => import('./master/inventory/customsleadtimemaster/customsleadtimemaster.module').then(m => m.CustomsLeadTimeModule),
                    data: { permission: 'Pages.Master.CKD.CustomsLeadTimeMaster.View' }
                },
                {
                    path: 'master/inventory/demdetdays',
                    loadChildren: () => import('./master/inventory/demdetdays/demdetdays.module').then(m => m.DemDetDaysModule),
                    data: { permission: 'Pages.Master.CKD.DemDetDays.View' }
                },
                {
                    path: 'master/inventory/demdetfees',
                    loadChildren: () => import('./master/inventory/demdetfees/demdetfees.module').then(m => m.DemDetFeesModule),
                    data: { permission: 'Pages.Master.CKD.DemDetFees.View' }
                },



                {
                    path: 'master/inventory/rentalwarehouse',
                    loadChildren: () => import('./master/inventory/rentalwarehouse/rentalwarehouse.module').then(m => m.RentalWarehouseModule),
                    data: { permission: 'Pages.Master.Inventory.CkdRentalWarehouse' }
                },


                // //GPS
                // {
                //     path: 'master/inv/gpssupplierinfo',
                //     loadChildren: () => import('./master/inventory/gpssupplierinfo/gpssupplierinfo.module').then(m => m.GpsSupplierInfoModule),
                //     data: { permission: 'Pages.Master.Gps.GpsSupplierInfo.View' }
                // },
                // {
                //     path: 'master/inv/gpscalendar',
                //     loadChildren: () => import('./master/inventory/gpscalendar/gpscalendar.module').then(m => m.GpsCalendarModule),
                //     data: { permission: 'Pages.Master.Gps.GpsCalendar.View' }
                // },
                // {
                //     path: 'master/inv/gpstmvpic',
                //     loadChildren: () => import('./master/inventory/gpstmvpic/gpstmvpic.module').then(m => m.GpsTmvPicModule),
                //     data: { permission: 'Pages.Master.Gps.GpsTmvPic.View' }
                // },
                // {
                //     path: 'master/inv/gpstrucksupplier',
                //     loadChildren: () => import('./master/inventory/gpstrucksupplier/gpstrucksupplier.module').then(m => m.GpsTruckSupplierModule),
                //     data: { permission: 'Pages.Master.Gps.GpsTruckSupplier.View' }
                // },
                // {
                //     path: 'master/inv/gpssupplierpic',
                //     loadChildren: () => import('./master/inventory/gpssupplierpic/gpssupplierpic.module').then(m => m.GpsSupplierPicModule),
                //     data: { permission: 'Pages.Master.Gps.GpsSupplierPic.View' }
                // },
                // {
                //     path: 'master/inv/gpsscreensetting',
                //     loadChildren: () => import('./master/inventory/gpsscreensetting/gpsscreensetting.module').then(m => m.GpsScreenSettingModule),
                //     data: { permission: 'Pages.Master.Gps.GpsScreenSetting.View' }
                // },
                // {
                //     path: 'master/inv/gpscategory',
                //     loadChildren: () => import('./master/inventory/gpscategory/gpscategory.module').then(m => m.GpsCategoryModule),
                //     data: { permission: 'Pages.Master.Gps.GpsCategory.View' }
                // },
                // {
                //     path: 'master/inv/gpsmasterialcategory',
                //     loadChildren: () => import('./master/inventory/gpsmasterialcategory/gpsmasterialcategory.module').then(m => m.GpsMasterialCategoryModule),
                //     data: { permission: 'Pages.Master.Gps.MaterialCategory.View' }
                // },
                // {
                //     path: 'master/inv/gpsmasterialcategorymapping',
                //     loadChildren: () => import('./master/inventory/gpsmasterialcategorymapping/gpsmasterialcategorymapping.module').then(m => m.GpsMasterialCategoryMappingModule),
                //     data: { permission: 'Pages.Master.Gps.MaterialCategoryMapping.View' }
                // },
                // {
                //     path: 'master/inv/gpsmaterialregisterbyshop',
                //     loadChildren: () => import('./master/inventory/gpsmaterialregisterbyshop/gpsmaterialregisterbyshop.module').then(m => m.GpsMaterialRegisterByShopModule),
                //     data: { permission: 'Pages.Master.Gps.GpsMaterialRegisterByShop.View' }
                // },
                // {
                //     path: 'master/inv/gpscostcenter',
                //     loadChildren: () => import('./master/inventory/gpscostcenter/gpscostcenter.module').then(m => m.GpsMaterialRegisterByShopModule),
                //     data: { permission: 'Pages.Master.Gps.CostCenter.View' }
                // },
                // {
                //     path: 'master/inv/uom',
                //     loadChildren: () => import('./master/inventory/uom/uom.module').then(m => m.UomModule),
                //     data: { permission: 'Pages.Master.Gps.Uom.View' }
                // },
                // {
                //     path: 'master/inv/gpswbsccmapping',
                //     loadChildren: () => import('./master/inventory/gpswbsccmapping/gpswbsccmapping.module').then(m => m.GpsWbsCCMappingModule),
                //     data: { permission: 'Pages.Master.Gps.WbsCCMapping.View' }
                // },

                // //CPS
                // {
                //     path: 'master/inventory/cpssuppliers',
                //     loadChildren: () => import('./master/inventory/cpssuppliers/cpssuppliers.module').then(m => m.CpsSuppliersModule),
                //     data: { permission: 'Pages.Master.Cps.CpsSuppliers.View' }
                // },
                // {
                //     path: 'master/inventory/cpsinventorygroup',
                //     loadChildren: () => import('./master/inventory/cpsinventorygroup/cpsinventorygroup.module').then(m => m.CpsInventoryGroupModule),
                //     data: { permission: 'Pages.Master.Cps.CpsInventoryGroup.View' }
                // },
                // {
                //     path: 'master/inventory/cpsinventoryitems',
                //     loadChildren: () => import('./master/inventory/cpsinventoryitems/cpsinventoryitems.module').then(m => m.CpsInventoryItemsModule),
                //     data: { permission: 'Pages.Master.Inventory.CpsInventoryItems.View' }
                // },

                // //HR
                // {
                //     path: 'master/inventory/hrorgstructure',
                //     loadChildren: () => import('./master/inventory/hrorgstructure/hrorgstructure.module').then(m => m.HrOrgStructureModule),
                //     data: { permission: 'Pages.Master.Hr.HrOrgStructure.View' }
                // },
                // {
                //     path: 'master/inventory/hrtitles',
                //     loadChildren: () => import('./master/inventory/hrtitles/hrtitles.module').then(m => m.HrTitlesModule),
                //     data: { permission: 'Pages.Master.Hr.HrTitles.View' }
                // },
                // {
                //     path: 'master/inventory/invhremployee',
                //     loadChildren: () => import('./master/inventory/invhremployee/invhremployee.module').then(m => m.InvHrEmployeeModule),
                //     data: { permission: 'Pages.Master.Hr.HrEmployee.View' }
                // },
                // {

                //     path: 'master/inventory/hrglcodecombination',
                //     loadChildren: () => import('./master/inventory/hrglcodecombination/hrglcodecombination.module').then(m => m.HrGlCodeCombinationModule),
                //     data: { permission: 'Pages.Master.Hr.HrGlcodeCombination.View' }
                // },
                // {
                //     path: 'master/inventory/hrposition',
                //     loadChildren: () => import('./master/inventory/hrposition/hrposition.module').then(m => m.HrPositionModule),
                //     data: { permission: 'Pages.Master.Hr.HrPosition.View' }
                // },
//INVT. SETUP
                // {
                //     path: 'master/common/materialgroup',
                //     loadChildren: () => import('./master/common/materialgroup/materialgroup.module').then(m => m.MaterialGroupModule),
                //     data: { permission: 'Pages.InvtSetup.MaterialGroup.View' }
                // },
                // {

                //     path: 'master/common/plant',
                //     loadChildren: () => import('./master/common/plant/plant.module').then(m => m.PlantModule),
                //     data: { permission: 'Pages.InvtSetup.Plant.View' }
                // },
                // {

                //     path: 'master/common/storagelocation',
                //     loadChildren: () => import('./master/common/storagelocation/storagelocation.module').then(m => m.StorageLocationModule),
                //     data: { permission: 'Pages.InvtSetup.StorageLocation.View' }
                // },
                // {
                //     path: 'master/common/storagelocationcategory',
                //     loadChildren: () => import('./master/common/storagelocationcategory/storagelocationcategory.module').then(m => m.StorageLocationCategoryModule),
                //     data: { permission: 'Pages.InvtSetup.StorageLocationCategory.View' }
                // },
                // {
                //     path: 'master/common/brand',
                //     loadChildren: () => import('./master/common/brand/brand.module').then(m => m.BrandModule),
                //     data: { permission: 'Pages.InvtSetup.Brand.View' }
                // },
                // {
                //     path: 'master/common/fueltype',
                //     loadChildren: () => import('./master/common/fueltype/fueltype.module').then(m => m.FuelTypeModule),
                //     data: { permission: 'Pages.InvtSetup.FuelType.View' }
                // },
                // {
                //     path: 'master/common/producttype',
                //     loadChildren: () => import('./master/common/producttype/producttype.module').then(m => m.ProductTypeModule),
                //     data: { permission: 'Pages.InvtSetup.ProductType.View' }
                // },
                // {
                //     path: 'master/common/transmissiontype',
                //     loadChildren: () => import('./master/common/transmissiontype/transmissiontype.module').then(m => m.TransmissionTypeModule),
                //     data: { permission: 'Pages.InvtSetup.TransmissionType.View' }
                // },
                // {
                //     path: 'master/common/uom',
                //     loadChildren: () => import('./master/common/uom/uom.module').then(m => m.UomModule),
                //     data: { permission: 'Pages.InvtSetup.Uom.View' }
                // },
                // {
                //     path: 'master/common/valuationclass',
                //     loadChildren: () => import('./master/common/valuationclass/valuationclass.module').then(m => m.ValuationClassModule),
                //     data: { permission: 'Pages.InvtSetup.ValuationClass.View' }
                // },
                // {
                //     path: 'master/common/valuationtype',
                //     loadChildren: () => import('./master/common/valuationtype/valuationtype.module').then(m => m.ValuationTypeModule),
                //     data: { permission: 'Pages.InvtSetup.ValuationType.View' }
                // },
                // {
                //     path: 'master/common/materialtype',
                //     loadChildren: () => import('./master/common/materialtype/materialtype.module').then(m => m.MaterialTypeModule),
                //     data: { permission: 'Pages.InvtSetup.MaterialType.View' }
                // },
                // {
                //     path: 'master/inventory/genbomdata',
                //     loadChildren: () => import('./master/inventory/genbomdata/genbomdata.module').then(m => m.GenBomDataModule),
                //     data: { permission: 'Pages.InvtSetup.MstInvGenBOMData.View' }
                // },
                // {
                //     path: 'master/common/carseries',
                //     loadChildren: () => import('./master/common/carseries/carseries.module').then(m => m.CarSeriesModule),
                //     data: { permission: 'Pages.InvtSetup.CarSeries.View' }
                // },
                // {
                //     path: 'master/common/carfamily',
                //     loadChildren: () => import('./master/common/carfamily/carfamily.module').then(m => m.CarfamilyModule),
                //     data: { permission: 'Pages.InvtSetup.Carfamily.View' }
                // },

                // {
                //     path: 'master/common/drivetrain',
                //     loadChildren: () => import('./master/common/drivetrain/drivetrain.module').then(m => m.DriveTrainModule),
                //     data: { permission: 'Pages.InvtSetup.DriveTrain.View' }
                // },
                // {
                //     path: 'master/common/enginemodel',
                //     loadChildren: () => import('./master/common/enginemodel/enginemodel.module').then(m => m.EngineModelModule),
                //     data: { permission: 'Pages.Master.Cmm.EngineModel.View' }
                // },
                // {
                //     path: 'master/common/enginetype',
                //     loadChildren: () => import('./master/common/enginetype/enginetype.module').then(m => m.EngineTypeModule),
                //     data: { permission: 'Pages.Master.Cmm.EngineType.View' }
                // },
                // {
                //     path: 'master/common/tax',
                //     loadChildren: () => import('./master/common/tax/tax.module').then(m => m.TaxModule),
                //     data: { permission: 'Pages.Master.Cmm.Tax.View' }
                // },

//MATERIAL MASTER
//                 {
//                     path: 'master/common/materialmaster',
//                     loadChildren: () => import('./master/common/materialmaster/materialmaster.module').then(m => m.MaterialMasterModule),
//                     data: { permission: 'Pages.MaterialMaster.MaterialMaster.View' }
//                 },
//                 {
//                     path: 'master/common/vehicle',
//                     loadChildren: () => import('./master/common/vehicle/vehicle.module').then(m => m.VehicleModule),
//                     data: { permission: 'Pages.MaterialMaster.Vehicle.View' }
//                 },
// // Master Spp
//                 {
//                     path: 'master/spp/customer',
//                     loadChildren: () => import('./master/spp/customer/customer.module').then(m => m.CustomerModule),
//                     data: { permission: 'Pages.SPP.Master.Customer' }
//                 },
// // Master Account
//                 {
//                     path: 'master/spp/glaccount',
//                     loadChildren: () => import('./master/spp/glaccount/glaccount.module').then(m => m.GLAccountModule),
//                     data: { permission: 'Pages.SPP.Master.GLAccount' }
//                 },

//CPS LINKAGE
//                 {
//                     path: 'inventory/cps/sapassetmaster',
//                     loadChildren: () => import('./inventory/cps/sapassetmaster/sapassetmaster.module').then(m => m.SapAssetMasterModule),
//                     data: { permission: 'Pages.CpsLinkAge.SapAssetMaster.View' }
//                 },
//                 {
//                     path: 'inventory/cps/invoiceheaders',
//                     loadChildren: () => import('./inventory/cps/invoiceheaders/invoiceheaders.module').then(m => m.InvoiceHeadersModule),
//                     data: { permission: 'Pages.CpsLinkAge.InvoiceHeaders.View' }
//                 },
//                 {
//                     path: 'inventory/cps/inventoryitemprice',
//                     loadChildren: () => import('./inventory/cps/inventoryitemprice/inventoryitemprice.module').then(m => m.InventoryItemPriceModule),
//                     data: { permission: 'Pages.CpsLinkAge.InventoryItemPrice.View' }
//                 },
//                 {
//                     path: 'inventory/cps/poheaders',
//                     loadChildren: () => import('./inventory/cps/poheaders/poheaders.module').then(m => m.PoHeadersModule),
//                     data: { permission: 'Pages.CpsLinkAge.PoHeaders.View' }
//                 },
//                 {
//                     path: 'inventory/cps/shipmentheader',
//                     loadChildren: () => import('./inventory/cps/shipmentheader/shipmentheader.module').then(m => m.ShipmentHeaderModule),
//                     data: { permission: 'Pages.CpsLinkAge.RcvShipmentHeaders.View' }
//                 },
// //PROD. PLAN
//                 {
//                     path: 'inventory/ckd/productionplanmonthly',
//                     loadChildren: () => import('./inventory/ckd/productionplanmonthly/productionplanmonthly.module').then(m => m.ProductionPlanMonthlyModule),
//                     data: { permission: 'Pages.ProdPlan.ProductionPlanMonthly.View' }

//                 },
//                 {
//                     path: 'inventory/ckd/prodplandaily',
//                     loadChildren: () => import('./inventory/ckd/prodplandaily/prodplandaily.module').then(m => m.ProdPlanDailyModule),
//                     data: { permission: 'Pages.ProdPlan.ProdPlanDaily.View' }
//                 },
//                 {
//                     path: 'assy/andon/ainplan',
//                     loadChildren: () => import('./assy/andon/aInplan/ainplan.module').then(m => m.AinplanModule),
//                     data: { permission: 'Pages.ProdPlan.AInPlan.View' }
//                 },
//                 {
//                     path: 'assy/andon/vehicledetails',
//                     loadChildren: () => import('./assy/andon/vehicledetails/vehicledetails.module').then(m => m.VehicleDetailsModule),
//                     data: { permission: 'Pages.ProdPlan.VehicleDetails.View' }
//                 },
//                 {
//                     path: 'assy/andon/aplanshiftbase',
//                     loadChildren: () => import('./assy/andon/aplanshiftbase/aplanshiftbase.module').then(m => m.APlanShiftBaseModule),
//                     data: { permission: 'Pages.ProdPlan.APlanShiftBase.View' }
//                 },
//                 {
//                     path: 'welding/andon/weldingplan',
//                     loadChildren: () => import('./welding/andon/weldingplan/weldingplan.module').then(m => m.WeldingPlanModule),
//                     data: { permission: 'Pages.ProdPlan.WeldingPlan.View' }
//                 },
//                                     {
//                     path: 'welding/andon/weldingprogress',
//                     loadChildren: () => import('./welding/andon/weldingprogress/weldingprogress.module').then(m => m.WeldingProgressModule),
//                     data: { permission: 'Pages.ProdPlan.WeldingProgress.View' }
//                 },
//                 {
//                     path: 'painting/andon/paintingdata',
//                     loadChildren: () => import('./painting/andon/paintingdata/paintingdata.module').then(m => m.PaintingDataModule),
//                     data: { permission: 'Pages.ProdPlan.PaintingData.View' }
//                 },
//                                     {
//                     path: 'painting/andon/scaninfo',
//                     loadChildren: () => import('./painting/andon/scaninfo/scaninfo.module').then(m => m.ScanInfoModule),
//                     data: { permission: 'Pages.ProdPlan.ScanInfo.View' }
//                 },
//                                     {
//                     path: 'assy/andon/assemblydata',
//                     loadChildren: () => import('./assy/andon/assemblydata/assemblydata.module').then(m => m.AssemblyDataModule),
//                     data: { permission: 'Pages.ProdPlan.AssemblyData.View' }
//                 },
//                 {
//                     path: 'inventory/ckd/productionmapping',
//                     loadChildren: () => import('./inventory/ckd/productionmapping/productionmapping.module').then(m => m.ProductionMappingModule),
//                     data: { permission: 'Pages.ProdPlan.ProductionMapping.View' }
//                 },
//CKD
                //master
                {
                    path: 'inventory/ckd/partlist',
                    loadChildren: () => import('./inventory/ckd/partlist/partlist.module').then(m => m.PartListModule),
                    data: { permission: 'Pages.Ckd.Master.PartList.View' }
                },
                   //PartList Inl
                // {
                //     path: 'inventory/ckd/partlistinl',
                //     loadChildren: () => import('./inventory/ckd/partlistinl/partlistinl.module').then(m => m.PartListInlModule),
                //     data: { permission: 'Pages.PIO.Master.PartListInl.View' }
                // },
                // {
                //     path: 'master/inventory/lotpart',
                //     loadChildren: () => import('./master/inventory/lotpart/lotpart.module').then(m => m.LotPartModule),
                //     data: { permission: 'Pages.Ckd.Master.LotPart.View' }
                // },
                {
                    path: 'master/inventory/devanningcasetype',
                    loadChildren: () => import('./master/inventory/devanningcasetype/devanningcasetype.module').then(m => m.DevanningCaseTypeModule),
                    data: { permission: 'Pages.Master.Inv.DevanningCaseType.View' }
                },
                // {
                //     path: 'inventory/ckd/shippingschedule',
                //     loadChildren: () => import('./inventory/ckd/shippingschedule/shippingschedule.module').then(m => m.ShippingScheduleModule),
                //     data: { permission: 'Pages.Ckd.Rundown.ShipingSchedule.View' }
                // },
                // {
                //     path: 'inventory/ckd/shippingschedulefirm',
                //     loadChildren: () => import('./inventory/ckd/shippingschedulefirm/shippingschedulefirm.module').then(m => m.ShippingScheduleFirmModule),
                //     data: { permission: 'Pages.Ckd.Rundown.ShipingScheduleFirm.View' }
                // },
                // {
                //     path: 'inventory/ckd/stock-rundown-shipping-schedule',
                //     loadChildren: () => import('./inventory/ckd/stock-rundown-shipping-schedule/stock-rundown-shipping-schedule.module').then(m => m.StockRundownShippingScheduleModule),
                //     data: { permission: 'Pages.Ckd.Rundown.StockRundownShipingSchedule.View' }
                // },
                // {
                //     path: 'inventory/ckd/stock-rundown-shipping-result',
                //     loadChildren: () => import('./inventory/ckd/stock-rundown-shipping-result/stock-rundown-shipping-result.module').then(m => m.StockRundownShippingResultModule),
                //     data: { permission: 'Pages.Ckd.Rundown.StockRundownShipingResult.View' }
                // },
                // {
                //     path: 'inventory/ckd/stock-rundown-warehouse',
                //     loadChildren: () => import('./inventory/ckd/stock-rundown-warehouse/stock-rundown-warehouse.module').then(m => m.StockRundownWearehouseModule),
                //     data: { permission: 'Pages.Ckd.Rundown.StockRundownShipingWarehouse.View' }
                // },

                //Intransit
                {
                    path: 'inventory/ckd/shipment',
                    loadChildren: () => import('./inventory/ckd/shipment/shipment.module').then(m => m.ShipmentModule),
                    data: { permission: 'Pages.Ckd.Intransit.Shipment.View' }
                },
                  {
                    path: 'inventory/ckd/bill',
                    loadChildren: () => import('./inventory/ckd/bill/bill.module').then(m => m.BillModule),
                    data: { permission: 'Pages.Ckd.Intransit.Bill.View' }
                },
                 {
                    path: 'inventory/ckd/invoice',
                    loadChildren: () => import('./inventory/ckd/invoice/invoice.module').then(m => m.InvoiceModule),
                    data: { permission: 'Pages.Ckd.Intransit.Invoice.View' }
                },
                 {
                    path: 'inventory/ckd/frameengine',
                    loadChildren: () => import('./inventory/ckd/frameengine/frameengine.module').then(m => m.FrameEngineModule),
                    data: { permission: 'Pages.Ckd.Intransit.FrameEngine.View' }
                },
                 {
                    path: 'inventory/ckd/containerintransit',
                    loadChildren: () => import('./inventory/ckd/containerIntransit/containerintransit.module').then(m => m.ContainerIntransitModule),
                    data: { permission: 'Pages.Ckd.Intransit.ContainerIntransit.View' }
                },
                  {
                    path: 'inventory/ckd/containerinvoice',
                    loadChildren: () => import('./inventory/ckd/containerinvoice/containerinvoice.module').then(m => m.ContainerInvoiceModule),
                    data: { permission: 'Pages.Ckd.Intransit.ContainerInvoice.View' }
                },
                  {
                    path: 'inventory/ckd/precustoms',
                    loadChildren: () => import('./inventory/ckd/precustoms/precustoms.module').then(m => m.PreCustomsModule),
                    data: { permission: 'Pages.Ckd.Intransit.PreCustoms.View' }
                },
                 {
                    path: 'inventory/ckd/customsdeclare',
                    loadChildren: () => import('./inventory/ckd/customsdeclare/customsdeclare.module').then(m => m.CustomsDeclareModule),
                    data: { permission: 'Pages.Ckd.Intransit.CustomsDeclare.View' }
                },

                  {
                    path: 'inventory/ckd/paymentrequest',
                    loadChildren: () => import('./inventory/ckd/paymentrequest/paymentrequest.module').then(m => m.PaymentRequestModule),
                    data: { permission: 'Pages.Ckd.Intransit.PaymentRequest.View' }
                },
                 {
                    path: 'inventory/ckd/containertransitportplan',
                    loadChildren: () => import('./inventory/ckd/containertransitportplan/containertransitportplan.module').then(m => m.ContainerTransitPortPlanModule),
                    data: { permission: 'Pages.Ckd.Intransit.ContainerTransitPortPlan.View' }
                },


                //Warehouse
                 {
                    path: 'inventory/ckd/containerrentalwhplan',
                    loadChildren: () => import('./inventory/ckd/containerrentalwhplan/containerrentalwhplan.module').then(m => m.ContainerRentalWHPlanModule),
                    data: { permission: 'Pages.Ckd.Warehouse.ContainerRentalWHPlan.View' }
                },
                {
                    path: 'inventory/ckd/request',
                    loadChildren: () => import('./inventory/ckd/request/request.module').then(m => m.InvCkdRequestModule),
                    data: { permission: 'Pages.Ckd.Warehouse.Request.View' }
                },
                {
                    path: 'inventory/ckd/containerdeliverygatein',
                    loadChildren: () => import('./inventory/ckd/containerdeliverygatein/containerdeliverygatein.module').then(m => m.ContainerDeliveryGateInModule),
                    data: { permission: 'Pages.Ckd.Warehouse.ContainerDeliveryGateIn.View' }
                },
                {
                    path: 'inventory/ckd/containerlist',
                    loadChildren: () => import('./inventory/ckd/containerlist/containerlist.module').then(m => m.ContainerListModule),
                    data: { permission: 'Pages.Ckd.Warehouse.ContainerList.View' }
                },
                {
                    path: 'inventory/ckd/modulecase',
                    loadChildren: () => import('./inventory/ckd/modulecase/modulecase.module').then(m => m.ModuleCaseModule),
                    data: { permission: 'Pages.Ckd.Warehouse.ModuleCase.View' }

                },
                {
                    path: 'inventory/ckd/partmanagement',
                    loadChildren: () => import('./inventory/ckd/partmanagement/partmanagement.module').then(m => m.PartManagementModule),
                    data: { permission: 'Pages.Ckd.Warehouse.PartManagement.View' }
                },
                {
                    path: 'inventory/ckd/partrobbing',
                    loadChildren: () => import('./inventory/ckd/partrobbing/partrobbing.module').then(m => m.PartRobbingModule),
                    data: { permission: 'Pages.Ckd.Warehouse.PartManagement.View' }
                },
                {
                    path: 'inventory/ckd/smqdorder',
                    loadChildren: () => import('./inventory/ckd/smqdorder/smqdorder.module').then(m => m.SmqdOrderModule),
                    data: { permission: 'Pages.Ckd.SMQD.SmqdOrder.View' }
                },
                {
                    path: 'inventory/ckd/smqdorderleadtime',
                    loadChildren: () => import('./inventory/ckd/smqdorderleadtime/smqdorderleadtime.module').then(m => m.SmqdOrderLeadTimeModule),
                    data: { permission: 'Pages.Ckd.SMQD.OrderLeadTime.View' }
                },
                {
                    path: 'inventory/ckd/stock-part',
                    loadChildren: () => import('./inventory/ckd/stock-part/stock-part.module').then(m => m.StockPartModule),
                    data: { permission: 'Pages.Ckd.Warehouse.StockPart.View' }
                },
                {
                    path: 'inventory/ckd/stock-receiving',
                    loadChildren: () => import('./inventory/ckd/stock-receiving/stock-receiving.module').then(m => m.StockReceivingModule),
                    data: { permission: 'Pages.Ckd.Warehouse.StockReceiving.View' }
                },
                {
                    path: 'inventory/ckd/stock-issuing',
                    loadChildren: () => import('./inventory/ckd/stock-issuing/stock-issuing.module').then(m => m.StockIssuingModule),
                    data: { permission: 'Pages.Ckd.Warehouse.StockIssuing.View' }
                },
                {
                    path: 'inventory/ckd/stock-balance',
                    loadChildren: () => import('./inventory/ckd/stock-balance/stock-balance.module').then(m => m.StockBalanceModule),
                    data: { permission: 'Pages.Ckd.Warehouse.StockBalance.View' }
                },
                // //Prod. Area
                // {
                //     path: 'inventory/ckd/ckd-vehicle',
                //     loadChildren: () => import('./inventory/ckd/ckd-vehicle/ckd-vehicle.module').then(m => m.CkdVehicleModule),
                //     data: { permission: 'Pages.Ckd.Area.Vehicle.View' }
                // },

                // //physical stock period
                // {
                //     path: 'inventory/ckd/physical-stock-period',
                //     loadChildren: () => import('./inventory/ckd/physical-stock-period/physical-stock-period.module').then(m => m.PhysicalStockPeriodModule),
                //     data: { permission: 'Pages.Ckd.Physical.PhysicalStockPeriod.View' }
                // },
                // //physical stock part
                // {
                //     path: 'inventory/ckd/physical-stock-part',
                //     loadChildren: () => import('./inventory/ckd/physical-stock-part/physical-stock-part.module').then(m => m.PhysicalStockPartModule),
                //     data: { permission: 'Pages.Ckd.Physical.PhysicalStockPart.View' }
                // },
                // //physical stock receiving
                // {
                //     path: 'inventory/ckd/physical-stock-receiving',
                //     loadChildren: () => import('./inventory/ckd/physical-stock-receiving/physical-stock-receiving.module').then(m => m.PhysicalStockReceivingModule),
                //     data: { permission: 'Pages.Ckd.Physical.ReceivingPhysicalStock.View' }
                // },
                // //physical stock issuing
                // {
                //     path: 'inventory/ckd/physical-stock-issuing',
                //     loadChildren: () => import('./inventory/ckd/physical-stock-issuing/physical-stock-issuing.module').then(m => m.PhysicalStockIssuingModule),
                //     data: { permission: 'Pages.Ckd.Physical.PhysicalStockIssuing.View' }
                // },
                // //Physical Count
                // {
                //     path: 'inventory/ckd/physicalconfirmlot',
                //     loadChildren: () => import('./inventory/ckd/physicalconfirmlot/physicalconfirmlot.module').then(m => m.PhysicalConfirmLotModule),
                //     data: { permission: 'Pages.Ckd.Physical.PhysicalConfirmLot.View' }
                // },
                // //physical-stock-part-s4
                // {
                //     path: 'inventory/ckd/physical-stock-part-s4',
                //     loadChildren: () => import('./inventory/ckd/physical-stock-part-s4/physical-stock-part-s4.module').then(m => m.PhysicalStockPartModule),
                //     data: { permission: 'Pages.Ckd.Physical.PhysicalStockPartS4.View' }
                // },
                // //smqd management
                // {
                //     path: 'inventory/ckd/smqdmanagement',
                //     loadChildren: () => import('./inventory/ckd/smqdmanagement/smqdmanagement.module').then(m => m.SmqdModule),
                //     data: { permission: 'Pages.Ckd.SMQD.Management.View' }
                // },
                // {
                //     path: 'i',
                //     loadChildren: () => import('./inventory/ckd/smqdmanagement/smqdmanagement.module').then(m => m.SmqdModule),
                //     data: { permission: 'Pages.Ckd.SMQD.Management.View' }
                // },

//GPS
                //Master
//                 {
//                     path: 'inventory/gps/partlistbycategory',
//                     loadChildren: () => import('./inventory/gps/partlistbycategory/gpsmaster.module').then(m => m.GpsMaster),
//                     data: { permission: 'Pages.Gps.Master.PartListByCategory.View' }
//                 },
//                 {
//                     path: 'inventory/gps/partlist',
//                     loadChildren: () => import('./inventory/gps/partlist/partlist.module').then(m => m.PartListModule),
//                     data: { permission: 'Pages.Gps.Master.PartList.View' }
//                 },
//                 {
//                     path: 'inventory/gps/invgpsstockconcept',
//                     loadChildren: () => import('./inventory/gps/invgpsstockconcept/invgpsstockconcept.module').then(m => m.InvGpsStockConceptModule),
//                     data: { permission: 'Pages.Gps.Master.StockConcept.View' }
//                 },
//                 {
//                     path: 'inventory/gps/material',
//                     loadChildren: () => import('./inventory/gps/material/material.module').then(m => m.MaterialModule),
//                     data: { permission: 'Pages.Gps.Master.Material.View' }
//                 },
//                 {
//                     path: 'inventory/gps/user',
//                     loadChildren: () => import('./inventory/gps/user/gpsuser.module').then(m => m.GpsUserModule),
//                     data: { permission: 'Pages.Gps.User' }
//                 },
//                 {
//                     path: 'inventory/gps/findstock',
//                     loadChildren: () => import('./inventory/gps/findstock/findstock.module').then(m => m.InvGpsFindStockModule),
//                     data: { permission: 'Pages.Gps.Warehouse.FinStock.View' }
//                 },


//                 //Receive
//                 {
//                     path: 'inventory/gps/invgpscontentlist',
//                     loadChildren: () => import('./inventory/gps/invgpscontentlist/invgpscontentlist.module').then(m => m.InvGpsContentListModule),
//                     data: { permission: 'Pages.Gps.Receive.ContentList.View' }
//                 },
//                 {
//                     path: 'inventory/gps/invgpskanban',
//                     loadChildren: () => import('./inventory/gps/kanban/invgpskanban.module').then(m => m.InvGpsKanbanModule),
//                     data: { permission: 'Pages.Gps.Receive.Kanban.View' }
//                 },
//                 {
//                     path: 'inventory/gps/receiving',
//                     loadChildren: () => import('./inventory/gps/receiving/receiving.module').then(m => m.StockReceivingModule),
//                     data: { permission: 'Pages.Gps.Receive.View' }
//                 },
//                 //
//                 {
//                     path: 'inventory/gps/invgpsdailyorder',
//                     loadChildren: () => import('./inventory/gps/dailyorder/invgpsdailyorder.module').then(m => m.InvGpsDailyOrderModule),
//                     data: { permission: 'Pages.Gps.Order.DailyOrder.View' }
//                     //data: { preload: true }
//                 },
//                 {
//                     path: 'inventory/gps/gpsstockrundown',
//                     loadChildren: () => import('./inventory/gps/gpsstockrundown/gpsstockrundown.module').then(m => m.GpsStockRundownModule),
//                     data: { permission: 'Pages.Gps.Rundown.GpsStockRundown.View' }
//                 },
//                 {
//                     path: 'inventory/gps/gpsstockrundowntransaction',
//                     loadChildren: () => import('./inventory/gps/gpsstockrundowntransaction/gpsstockrundowntransaction.module').then(m => m.GpsStockRundownTransactionModule),
//                     data: { permission: 'Pages.Gps.Rundown.GpsStockRundownTransaction.View' }
//                 },
//                 {
//                     path: 'inventory/gps/gpsstock',
//                     loadChildren: () => import('./inventory/gps/gpsstock/gpsstock.module').then(m => m.GpsStockModule),
//                     data: { permission: 'Pages.Gps.Warehouse.GpsStock.View' }
//                 },
//                 //
//                 {
//                     path: 'inventory/gps/stockissuingtransaction',
//                     loadChildren: () => import('./inventory/gps/stockissuingtransaction/stockissuingtransaction.module').then(m => m.StockIssuingTransactionModule),
//                     data: { permission: 'Pages.Gps.Warehouse.StockIssuingTransDetails.View' }
//                 },
//                 {
//                     path: 'inventory/gps/stockreceivingtransaction',
//                     loadChildren: () => import('./inventory/gps/stockreceivingtransaction/stockreceivingtransaction.module').then(m => m.StockReceivingTransactionModule),
//                     data: { permission: 'Pages.Gps.Warehouse.StockReceivingTransDetails.View' }
//                 },
//                 // stock issuing
//                 {
//                     path: 'inventory/gps/issuing',
//                     loadChildren: () => import('./inventory/gps/issuing/issuing.module').then(m => m.IssuingModule),
//                     data: { permission: 'Pages.GPS.Issuing.View' }
//                 },
//                 {
//                     path: 'inventory/gps/issuings',
//                     loadChildren: () => import('./inventory/gps/issuings/issuings.module').then(m => m.IssuingsModule),
//                     data: { permission: 'Pages.GPS.Issuings.View' }
//                 },
//                 {
//                     path: 'inventory/gps/mapping',
//                     loadChildren: () => import('./inventory/gps/mapping/mapping.module').then(m => m.InvGpsMappingModule),
//                     data: { permission: 'Pages.GPS.Issuing.Mapping.View' }
//                 },

// // PIO
//                 //PIO Stock
//                 {
//                     path: 'inventory/pio/stock',
//                     loadChildren: () => import('./inventory/pio/stock/stock.module').then(m => m.StockModule),
//                     data: { permission: 'Pages.PIO.Warehouse.Stock.View' }
//                 },
//                 //PIO StockIssuing
//                 {
//                     path: 'inventory/pio/stockissuing',
//                     loadChildren: () => import('./inventory/pio/stockissuing/stockissuing.module').then(m => m.StockIssuingModule),
//                     data: { permission: 'Pages.PIO.Warehouse.StockIssuing.View' }
//                 },
//                 //PIO StockRundown
//                 {
//                     path: 'inventory/pio/stockrundown',
//                     loadChildren: () => import('./inventory/pio/stockrundown/stockrundown.module').then(m => m.StockRundownModule),
//                     data: { permission: 'Pages.PIO.Warehouse.StockRundown.View' }
//                 },
//                 //PIO StockRundownTransaction
//                 {
//                     path: 'inventory/pio/stockrundowntransaction',
//                     loadChildren: () => import('./inventory/pio/stockrundowntransaction/stockrundowntransaction.module').then(m => m.StockRundownTransactionModule),
//                     data: { permission: 'Pages.PIO.Warehouse.StockRundownTransaction.View' }
//                 },
//                 //PIO StockTransaction
//                 {
//                     path: 'inventory/pio/stocktransaction',
//                     loadChildren: () => import('./inventory/pio/stocktransaction/stocktransaction.module').then(m => m.StockTransactionModule),
//                     data: { permission: 'Pages.PIO.Warehouse.StockTransaction.View' }
//                 },
//                 {
//                     path: 'inventory/pio/tmssdispatchplan',
//                     loadChildren: () => import('./inventory/pio/tmssdispatchplan/tmssdispatchplan.module').then(m => m.TmssDispatchPlanModule),
//                     data: { permission: 'Pages.PIO.Master.Tmss.TmssDispatchPlan.View' }
//                 },
//                 // PIO PartList
//                 {
//                     path: 'inventory/pio/partlist',
//                     loadChildren: () => import('./inventory/pio/partlist/partlist.module').then(m => m.PartListModule),
//                     data: { permission: 'Pages.PIO.Master.PartList.View' }
//                 },

//                 //     // PIO PartList Inl
//                 // {
//                 //     path: 'inventory/pio/partlistinl',
//                 //     loadChildren: () => import('./inventory/pio/partlistinl/partlistinl.module').then(m => m.PartListInlModule),
//                 //     data: { permission: 'Pages.PIO.Master.PartListInl.View' }
//                 // },
//                 // Inv Topsse Invoice
//                 {
//                     path: 'inventory/pio/topsseinvoice',
//                     loadChildren: () => import('./inventory/pio/topsseinvoice/topsseinvoice.module').then(m => m.TopsseInvoiceModule),
//                     data: { permission: 'Pages.PIO.Master.TopsseInvoice.View' }
//                 },

//                   //PIO StockIReceiving
//                 {
//                     path: 'inventory/pio/stockreceiving',
//                     loadChildren: () => import('./inventory/pio/stockreceiving/stockreceiving.module').then(m => m.StockReceivingModule),
//                     data: { permission: 'Pages.PIO.Warehouse.StockReceiving.View' }

//                 },
//                 {
//                     path: 'inventory/pio/lspsupplierinfo',
//                     loadChildren: () => import('./inventory/pio/lspsupplierinfo/lspsupplierinfo.module').then(m => m.LspSupplierInfoModule),
//                     data: { permission: 'Pages.PIO.Master.SupplierInfo.View' }

//                 },
//                 // PIO Part List Off
//                 {
//                     path: 'inventory/pio/partlistoff',
//                     loadChildren: () => import('./inventory/pio/partlistoff/partlistoff.module').then(m => m.PartListOffModule),
//                     data: { permission: 'Pages.PIO.Master.PartListOff.View' }
//                 },
//                 {
//                     path: 'inventory/pio/pioproductionplanmonthly',
//                     loadChildren: () => import('./inventory/pio/pioproductionplanmonthly/pioproductionplanmonthly.module').then(m => m.PioProductionPlanMonthlyModule),
//                     data: { permission: 'Pages.PIO.Master.ProductionPlanMonthly.View' }
//                 },

// // DM & IHP
//                 //drm part list
//                 {
//                     path: 'inventory/dm/drmpartlist',
//                     loadChildren: () => import('./inventory/dm/drmpartlist/drmpartlist.module').then(m => m.DrmPartListModule),
//                     data: { permission: 'Pages.DMIHP.Mst.DRMPartList.View' }
//                 },
//                 //drm part list
//                 {
//                     path: 'inventory/dm/drmstockpart',
//                     loadChildren: () => import('./inventory/dm/drmstockpart/drmstockpart.module').then(m => m.DrmStockPartModule),
//                     data: { permission: 'Pages.DMIHP.Stock.StockPart.View' }
//                 },
//                  //drm stock part excel
//                  {
//                     path: 'inventory/dm/drmstockpartexcel',
//                     loadChildren: () => import('./inventory/dm/drmstockpartexcel/drmstockpartexcel.module').then(m => m.DrmStockPartExcelModule),
//                     data: { permission: 'Pages.DMIHP.Stock.StockPartExcel.View' }
//                 },
//                 //drm importplan
//                 {
//                     path: 'inventory/dm/drmimportplan',
//                     loadChildren: () => import('./inventory/dm/drmimportplan/drmimportplan.module').then(m => m.DrmImportPlanModule),
//                     data: { permission: 'Pages.DMIHP.GR.ImportPlan.View' }
//                 },

//                 //drm localplan
//                 {
//                     path: 'inventory/dm/drmlocalplan',
//                     loadChildren: () => import('./inventory/dm/drmlocalplan/drmlocalplan.module').then(m => m.DrmLocalPlanModule),
//                     data: { permission: 'Pages.DMIHP.GR.LocalPlan.View' }
//                 },

//                 //ihp part list
//                 {
//                     path: 'inventory/ihp/ihppartlist',
//                     loadChildren: () => import('./inventory/ihp/ihppartlist/ihppartlist.module').then(m => m.IhpPartListModule),
//                     data: { permission: 'Pages.DMIHP.Mst.IHPPartList.View' }
//                 },

//                  //drm rundown
//                  {
//                     path: 'inventory/dm/drmstockrundown',
//                     loadChildren: () => import('./inventory/dm/drmstockrundown/drmstockrundown.module').then(m => m.DrmStockRundownModule),
//                     data: { permission: 'Pages.DMIHP.Rundow.StockRundown.View' }
//                 },
//                 //ihp stock part
//                 {
//                     path: 'inventory/ihp/ihpstockpart',
//                     loadChildren: () => import('./inventory/ihp/ihpstockpart/ihpstockpart.module').then(m => m.IhpStockPartModule),
//                     data: { permission: 'Pages.DMIHP.Stock.InvIphStockPart.View' }
//                 },
//                 //matcustomdeclare
//                 {
//                     path: 'inventory/ihp/matcustomdeclare',
//                     loadChildren: () => import('./inventory/ihp/matcustomdeclare/matcustomdeclare.module').then(m => m.MATCustomsDeclareModule),
//                     data: { permission: 'Pages.DMIHP.Mst.InvIphMatCustomDeclare.View' }
//                 },
// //Interface
//                 //bom
//                 {
//                     path: 'inventory/bom/genbomdatat',
//                     loadChildren: () => import('./inventory/bom/genbomdatat/genbomdata.module').then(m => m.GenBomDataModule),
//                     data: { permission: 'Pages.Interface.Bom.InvGenBOMData.View' }
//                 },
//                 {
//                     path: 'inventory/if/fqf3mm04',
//                     loadChildren: () => import('./inventory/if/fqf3mm04/fqf3mm04.module').then(m => m.FQF3MM04Module),
//                     data: { permission: 'Pages.Interface.IF.FQF3MM04.View' }
//                 },
//                 {
//                     path: 'inventory/if/FQF3MM_LV2',
//                     loadChildren: () => import('./inventory/if/fqf3mm_lv2/fqf3mm_lv2.module').then(m => m.FQF3MM_LV2Module),
//                     data: { permission: 'Pages.Interface.IF.FQF3MM_LV2.View' }
//                 },
//                 {
//                     path: 'inventory/if/fqf3mm05',
//                     loadChildren: () => import('./inventory/if/fqf3mm05/fqf3mm05.module').then(m => m.FQF3MM05Module),
//                     data: { permission: 'Pages.Interface.IF.FQF3MM05.View' }
//                 },
//                 {
//                     path: 'inventory/if/fqf3mm01',
//                     loadChildren: () => import('./inventory/if/fqf3mm01/fqf3mm01.module').then(m => m.FQF3MM01Module),
//                     data: { permission: 'Pages.Interface.IF.FQF3MM01.View' }
//                 },
//                 {
//                     path: 'inventory/if/fqf3mm02',
//                     loadChildren: () => import('./inventory/if/fqf3mm02/fqf3mm02.module').then(m => m.FQF3MM02Module),
//                     data: { permission: 'Pages.Interface.IF.FQF3MM02.View' }
//                 },
//                 {
//                     path: 'inventory/if/fqf3mm06',
//                     loadChildren: () => import('./inventory/if/fqf3mm06/fqf3mm06.module').then(m => m.FQF3MM06Module),
//                     data: { permission: 'Pages.Interface.IF.FQF3MM06.View' }
//                 },
//                 {
//                     path: 'inventory/if/fqf3mm03',
//                     loadChildren: () => import('./inventory/if/fqf3mm03/fqf3mm03.module').then(m => m.FQF3MM03Module),
//                     data: { permission: 'Pages.Interface.IF.FQF3MM03.View' }
//                 },
//                 {
//                     path: 'inventory/if/fqf3mm07',
//                     loadChildren: () => import('./inventory/if/fqf3mm07/fqf3mm07.module').then(m => m.FQF3MM07Module),
//                     data: { permission: 'Pages.Interface.IF.FQF3MM07.View' }
//                 },
// //SPP
//                 {
//                     path: 'inventory/spp/cost',
//                     loadChildren: () => import('./inventory/spp/cost/cost.module').then(m => m.CostModule),
//                     data: { permission: 'Pages.SPP.Inventory.Cost.View' }
//                 },
//                 {
//                     path: 'inventory/spp/invoicedetails',
//                     loadChildren: () => import('./inventory/spp/invoicedetails/invoicedetails.module').then(m => m.InvoiceDetailsModule),
//                     data: { permission: 'Pages.SPP.Inventory.InvoiceDetails.View' }
//                 },
//                 {
//                     path: 'inventory/spp/shipping',
//                     loadChildren: () => import('./inventory/spp/shipping/shipping.module').then(m => m.ShippingModule),
//                     data: { permission: 'Pages.SPP.Inventory.Shipping.View' }
//                 },
//                 {
//                     path: 'inventory/spp/stock',
//                     loadChildren: () => import('./inventory/spp/stock/stock.module').then(m => m.StockModule),
//                     data: { permission: 'Pages.SPP.Inventory.Stock.View' }
//                 },
//                 {
//                     path: 'inventory/spp/costofsalesummary',
//                     loadChildren: () => import('./inventory/spp/costofsalesummary/costofsalesummary.module').then(m => m.CostOfSaleSummaryModule),
//                     data: { permission: 'Pages.SPP.Inventory.CostOfSaleSummary.View' }
//                 },
                { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                { path: '**', redirectTo: 'dashboard' },

                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class MainRoutingModule { }
