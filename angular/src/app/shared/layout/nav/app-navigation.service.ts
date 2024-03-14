import { PermissionCheckerService } from 'abp-ng2-module';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';

import { TABS } from '@app/shared/constants/tab-keys';


@Injectable()
export class AppNavigationService {
    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService
    ) { }
    /* tslint:disable */
    /* eslint-disable */
    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu',
            [
                /* WORKING PATTERN */
                // new AppMenuItem('WORKING PATTERN', 'Pages.WorkingPattern', '', '', '', [], [
                //     new AppMenuItem("Calendar", "Pages.WorkingPattern.Calendar.View", "", "/app/main/master/workingpattern/calendar", 'fa fa-calendar'),
                //     new AppMenuItem("Season Month", "Pages.WorkingPattern.SeasonMonth.View", "", "/app/main/master/workingpattern/seasonmonth"),
                //     new AppMenuItem("Pattern H", "Pages.WorkingPattern.PatternH.View", "", "/app/main/master/workingpattern/patternh"),
                //     new AppMenuItem("Working Time", "Pages.WorkingPattern.WorkingTime.View", "", "/app/main/master/workingpattern/workingtime"),
                //     new AppMenuItem("Working Type", "Pages.WorkingPattern.WorkingType.View", "", "/app/main/master/workingpattern/workingtype"),
                //     new AppMenuItem("Daily Working Time", "Pages.WorkingPattern.DailyWorkingTime.View", "", "/app/main/master/workingpattern/dailyworkingtime", 'fa fa-clock'),
                //     new AppMenuItem("Pattern D", "Pages.WorkingPattern.PatternD.View", "", "/app/main/master/workingpattern/patternd"),
                //     new AppMenuItem("Week", "Pages.WorkingPattern.Week.View", "", "/app/main/master/workingpattern/week"),
                // ]),

                /* MASTER */
                new AppMenuItem('MASTER', 'Pages.Master', '', '', '', [], [
                    /*Common*/
                    // new AppMenuItem('Common', 'Pages.Master.Common', '', '', '', [], [
                    //     new AppMenuItem("Exchange Rate", "Pages.Master.Common.ExchangeRate.View", "", "/app/main/master/common/exchangerate"),
                    //     new AppMenuItem("Model", "Pages.Master.Common.Model.View", "", "/app/main/master/common/model"),   //"/app/main/master/common/model"     //TAB
                    //     new AppMenuItem("Shop", "Pages.Master.Common.Shop.View", "", "/app/main/master/workingpattern/shop"),     //TAB
                    //     new AppMenuItem("Devanning Case Type", "Pages.Master.Common.DevanningCaseType.View", "", "/app/main/master/common/devanningcasetype"),
                    //     new AppMenuItem("Shop Type", "Pages.Master.Common.ShopType.View", "", "/app/main/master/common/shoptype"),
                    //     new AppMenuItem("Color", "Pages.Master.Common.Color.View", "","/app/main/master/common/color"),     //TAB
                    //     new AppMenuItem("Vehicle Color Type", "Pages.Master.Common.VehicleColorType.View", "", "/app/main/master/common/vehiclecolortype"),
                    //     new AppMenuItem("Takt Time", "Pages.Master.Common.TaktTime.View", "", "/app/main/master/common/takttime"), //"/app/main/master/common/takttime"     //TAB
                    //     new AppMenuItem('Lookup', 'Pages.Master.Common.Lookup.View', '', "/app/main/master/common/lookup"),     //TAB
                    //     new AppMenuItem("Business Parter", "Pages.Master.Cmm.BusinessParter.View", '', "/app/main/master/common/businessparter"),

                    // ]),

                    //CKD
                    new AppMenuItem('CKD', 'Pages.Master.Ckd', '', '', '', [], [
                        new AppMenuItem("Customs Port", "Pages.Master.Ckd.CustomsPort.View", "", "/app/main/master/inventory/customsport"),
                        new AppMenuItem("Shipping Company", "Pages.Master.Ckd.ShippingCompany.View", "", "/app/main/master/inventory/shippingcompany"),
                        new AppMenuItem("Container Status", "Pages.Master.Ckd.ContainerStatus.View", "", "/app/main/master/inventory/containerstatus"),
                        new AppMenuItem("Container Delivery Type", "Pages.Master.Ckd.ContainerDeliveryType.View", "", "/app/main/master/inventory/containerdeliverytype"),
                        new AppMenuItem("Forwarder", "Pages.Master.Ckd.Forwarder.View", "", "/app/main/master/inventory/forwarder"),
                        new AppMenuItem("Invoice Status", "Pages.Master.Ckd.InvoiceStatus.View", "", "/app/main/master/inventory/invoicestatus"),
                        new AppMenuItem("Customs Status", "Pages.Master.Ckd.CustomsStatus.View", "", "/app/main/master/inventory/customsstatus"),
                        new AppMenuItem("Customs Leadtime", "Pages.Master.CKD.CustomsLeadtime.View", "", "/app/main/master/inventory/customsleadtime"),
                        new AppMenuItem("Supplier List", "Pages.Master.Inventory.SupplierList.View", "", "/app/main/master/inventory/supplierlist"),

                        // new AppMenuItem("PIO Email", "Pages.Master.Inventory.PIOEmail.View", "", "/app/main/master/inventory/pioemail"),

                        //new AppMenuItem("Customs Leadtime (chưa có).View", "", "", ""),
                    ]),

                    //GPS
                    // new AppMenuItem('GPS', 'Pages.Master.Gps', '', '', '', [], [
                    //     new AppMenuItem("Gps Supplier Info", "Pages.Master.Gps.GpsSupplierInfo.View", "", "/app/main/master/inv/gpssupplierinfo"),
                    //     new AppMenuItem("Gps Calendar", "Pages.Master.Gps.GpsCalendar.View", "", "/app/main/master/inv/gpscalendar", 'fa fa-calendar'),
                    //     new AppMenuItem("Gps Tmv Pic", "Pages.Master.Gps.GpsTmvPic.View", "", "/app/main/master/inv/gpstmvpic"),
                    //     new AppMenuItem("Gps Truck Supplier", "Pages.Master.Gps.GpsTruckSupplier.View", "", "/app/main/master/inv/gpstrucksupplier", 'fa fa-truck'),
                    //     new AppMenuItem("Gps Supplier Pic", "Pages.Master.Gps.GpsSupplierPic.View", "", "/app/main/master/inv/gpssupplierpic"),
                    //     new AppMenuItem("Gps Screen Setting", "Pages.Master.Gps.GpsScreenSetting.View", "", "/app/main/master/inv/gpsscreensetting"),
                    //    // new AppMenuItem("Gps Category", "Pages.Master.Gps.GpsCategory.View", "", "/app/main/master/inv/gpscategory"),
                    //     new AppMenuItem("Gps Category", "Pages.Master.Gps.MaterialCategory.View", "", "/app/main/master/inv/gpsmasterialcategory"),
                    // ]),

                    //CPS
                    // new AppMenuItem('CPS', 'Pages.Master.Cps', '', '', '', [], [
                    //     new AppMenuItem("Cps Suppliers", "Pages.Master.Cps.CpsSuppliers.View", "", "/app/main/master/inventory/cpssuppliers"),
                    //     new AppMenuItem("Cps Inventory Group", "Pages.Master.Cps.CpsInventoryGroup.View", "", "/app/main/master/inventory/cpsinventorygroup"),
                    //     new AppMenuItem("Cps Inventory Items", "Pages.Master.Inventory.CpsInventoryItems.View", "", "/app/main/master/inventory/cpsinventoryitems"),

                    // ]),

                    // //HR
                    // new AppMenuItem('HR', 'Pages.Master.Hr', '', '', '', [], [
                    //     new AppMenuItem("Hr OrgStructure", "Pages.Master.Hr.HrOrgStructure.View", "", "/app/main/master/inventory/hrorgstructure"),
                    //     new AppMenuItem("Hr Employee", "Pages.Master.Hr.HrEmployee.View", "", "/app/main/master/inventory/invhremployee"),
                    //     new AppMenuItem("Hr Glcode Combination", "Pages.Master.Hr.HrGlcodeCombination.View", "", "/app/main/master/inventory/hrglcodecombination"),
                    //     new AppMenuItem("Hr Titles", "Pages.Master.Hr.HrTitles.View", "", "/app/main/master/inventory/hrtitles"),
                    //     new AppMenuItem("Hr Position", "Pages.Master.Hr.HrPosition.View", "", "/app/main/master/inventory/hrposition"),
                    // ]),

                    //BMS
                    // new AppMenuItem('BMS', 'Pages.Master.Hr', '', '', '',[], [
                    //     new AppMenuItem("WBS (chưa làm).View", "", "",   ""),
                    // ]),
                ]),

                /* INVT. SETUP */
                // new AppMenuItem('INVT. SETUP', 'Pages.InvtSetup', '', '', '', [], [
                //     //   new AppMenuItem("Business Partner (chưa làm).View", "", "", ""),
                //     //Common Setup
                //     new AppMenuItem('Common Setup', 'Pages.InvtSetupCommonSetup', '', '', '', [], [
                //         new AppMenuItem("Material Group", "Pages.InvtSetup.MaterialGroup.View", "", "/app/main/master/common/materialgroup"),
                //         new AppMenuItem("Plant Master", "Pages.InvtSetup.Plant.View", "", "/app/main/master/common/plant"),
                //         new AppMenuItem("Storage Location", "Pages.InvtSetup.StorageLocation.View", '', "/app/main/master/common/storagelocation"),
                //         new AppMenuItem("Storage Location Cat", "Pages.InvtSetup.StorageLocationCategory.View", "", "/app/main/master/common/storagelocationcategory"),
                //         new AppMenuItem("Brand Master", "Pages.InvtSetup.Brand.View", "", "/app/main/master/common/brand"),
                //         new AppMenuItem("Fuel Type", "Pages.InvtSetup.FuelType.View", "", "/app/main/master/common/fueltype"),
                //         new AppMenuItem("Product Type", "Pages.InvtSetup.ProductType.View", "", "/app/main/master/common/producttype"),
                //         new AppMenuItem("Transmission Type", "Pages.InvtSetup.TransmissionType.View", "", "/app/main/master/common/transmissiontype"),
                //         new AppMenuItem("Unit of Measure", "Pages.InvtSetup.Uom.View", "", "/app/main/master/common/uom"),
                //         new AppMenuItem("Valuation Class", "Pages.InvtSetup.ValuationClass.View", "", "/app/main/master/common/valuationclass"),
                //         new AppMenuItem("Valuation Type", "Pages.InvtSetup.ValuationType.View", "", "/app/main/master/common/valuationtype"),
                //         new AppMenuItem("Material Type", "Pages.InvtSetup.MaterialType.View", "", "/app/main/master/common/materialtype"),
                //         new AppMenuItem("Gen Bom Data", "Pages.InvtSetup.MstInvGenBOMData.View", "", "/app/main/master/inventory/genbomdata"),
                //         new AppMenuItem("Car Seris", "Pages.InvtSetup.CarSeries.View", "", "/app/main/master/common/carseries"),
                //         new AppMenuItem("Car Farmily", "Pages.InvtSetup.Carfamily.View", "", "/app/main/master/common/carfamily"),
                //         new AppMenuItem("Drive Train", "Pages.InvtSetup.DriveTrain.View", "", "/app/main/master/common/drivetrain"),
                //         new AppMenuItem("Product Group", "Pages.Master.Cmm.ProductGroup.View", "", "/app/main/master/common/productgroup"),
                //         new AppMenuItem("Engine Model", "Pages.Master.Cmm.EngineModel.View", "", "/app/main/master/common/enginemodel"),
                //         new AppMenuItem("Engine Type", "Pages.Master.Cmm.EngineType.View", "", "/app/main/master/common/enginetype"),
                //         new AppMenuItem("Tax", "Pages.Master.Cmm.Tax.View", "", "/app/main/master/common/tax"),
                //         new AppMenuItem("Vehicle Name", "Pages.Master.Common.VehicleName.View", "", "/app/main/master/common/vehiclename"),
                //     ]),
                //     //Checking Rule
                //     new AppMenuItem('Checking Rule', 'Pages.InvtSetupCheckingRule', '', '', '', [], [
                //         new AppMenuItem("MMChecking Rule", "Pages.Master.Cmm.MMCheckingRule.View", "", "/app/main/master/common/mmcheckingrule"),
                //         new AppMenuItem("MMValidation Result", "Pages.Master.Cmm.MMValidationResult.View", "", "/app/main/master/common/mmvalidationresult"),
                //     ]),
                // ]),

                // /* MATERIAL MASTER */
                // new AppMenuItem('MATERIAL MASTER', 'Pages.MaterialMaster', '', '', '', [], [
                //     new AppMenuItem("Material", "Pages.MaterialMaster.MaterialMaster.View", "", "/app/main/master/common/materialmaster"),
                //     new AppMenuItem("Engine Master", "Pages.Master.Cmm.EngineMaster.View", "", "/app/main/master/common/enginemaster"),
                //     new AppMenuItem("Vehicle (CKD)", "Pages.MaterialMaster.Vehicle.View", "", "/app/main/master/common/vehicle"),
                //     new AppMenuItem('Vehicle (CBU)', 'Pages.Master.Cmm.VehicleCBU.View', '', '/app/main/master/common/vehiclecbu'),
                // ]),

                // /* CPS LINKAGE */
                // new AppMenuItem('CPS LINKAGE', 'Pages.CpsLinkAge', '', '', '', [], [
                //     new AppMenuItem("SAP Asset Master", "Pages.CpsLinkAge.SapAssetMaster.View", "", "/app/main/inventory/cps/sapassetmaster"),
                //     new AppMenuItem("Invoice", "Pages.CpsLinkAge.InvoiceHeaders.View", "", "/app/main/inventory/cps/invoiceheaders", 'fa fa-file-text'),
                //     new AppMenuItem("PO", "Pages.CpsLinkAge.PoHeaders.View", "", "/app/main/inventory/cps/poheaders"),
                //     new AppMenuItem("Receipt", "Pages.CpsLinkAge.RcvShipmentHeaders.View", "", "/app/main/inventory/cps/shipmentheader"),
                //     new AppMenuItem("ItemPrice", "Pages.CpsLinkAge.InventoryItemPrice.View", "", "/app/main/inventory/cps/inventoryitemprice", 'fa fa-file-text'),
                // ]),

                // /* PROD. PLAN */
                // new AppMenuItem('PROD. DATA', 'Pages.ProdPlan', '', '', '', [], [
                //     new AppMenuItem('Prod. Plan', '', '', '', '', [], [
                //         new AppMenuItem("Prod Plan Monthly by Grade", "Pages.ProdPlan.ProductionPlanMonthly.View", "", "/app/main/inventory/ckd/productionplanmonthly"),
                //         new AppMenuItem("Plan Daily", "Pages.ProdPlan.ProdPlanDaily.View", "", "/app/main/inventory/ckd/prodplandaily"),
                //         new AppMenuItem("A Plan Shift Base", "Pages.ProdPlan.APlanShiftBase.View", "", "/app/main/assy/andon/aplanshiftbase"),
                //     ]),
                //     new AppMenuItem('Actual', '', '', '', '', [], [
                //         new AppMenuItem("A In", "Pages.ProdPlan.AInPlan.View", "", "/app/main/assy/andon/ainplan"),
                //         new AppMenuItem("Vehicle Details", "Pages.ProdPlan.VehicleDetails.View", "", '/app/main/assy/andon/vehicledetails'),
                //         new AppMenuItem("Welding", "Pages.ProdPlan.WeldingPlan.View", "", "/app/main/welding/andon/weldingplan"),
                //         new AppMenuItem("Production 1x1", "Pages.ProdPlan.ProductionMapping.View", "", "/app/main/inventory/ckd/productionmapping"),
                //     ]),
                //     new AppMenuItem('Progressive', '', '', '', '', [], [
                //         new AppMenuItem("Welding Progress", "Pages.ProdPlan.WeldingProgress.View", "", "/app/main/welding/andon/weldingprogress"),
                //         new AppMenuItem("Painting Data", "Pages.ProdPlan.PaintingData.View", "", "/app/main/painting/andon/paintingdata"),
                //         new AppMenuItem("Scan Info", "Pages.ProdPlan.ScanInfo.View", "", "/app/main/painting/andon/scaninfo"),
                //         new AppMenuItem("Assembly Data", "Pages.ProdPlan.AssemblyData.View", "", "/app/main/assy/andon/assemblydata"),
                //     ]),
                // ]),

                /* CKD */
                new AppMenuItem('CKD', 'Pages.Ckd', '', '', '', [], [
                    //Master
                    new AppMenuItem('Master', 'Pages.Ckd.Master', '', '', '', [], [
                        new AppMenuItem("PartList", "Pages.Ckd.Master.PartList.View", "", "/app/main/inventory/ckd/partlist", 'fa fa-cubes'),
                        // new AppMenuItem("Part List Inl", "Pages.PIO.Master.PartListInl.View", "", "/app/main/inventory/ckd/partlistinl"),
                        // new AppMenuItem("Inv Lot", "Pages.Ckd.Master.LotPart.View", "", "/app/main/master/inventory/lotpart"),
                        new AppMenuItem("Devanning Case Type", "Pages.Master.Inv.DevanningCaseType.View", "", "/app/main/master/inventory/devanningcasetype"),
                        // new AppMenuItem("SMQD Order Lead Time", "Pages.Ckd.SMQD.OrderLeadTime.View", "", "/app/main/inventory/ckd/smqdorderleadtime"),
                        // new AppMenuItem("Module Master(chưa làm).View", "", "",   ""),
                        // new AppMenuItem("Irregular Part List(chưa làm).View", "", "",   ""),
                        // new AppMenuItem("Color Part List(chưa làm).View", "", "",   ""),
                        new AppMenuItem("Rental Warehouse", "Pages.Master.Inventory.CkdRentalWarehouse", "",   "/app/main/master/inventory/rentalwarehouse"),
                        new AppMenuItem("Customs Lead Time", "Pages.Master.CKD.CustomsLeadTimeMaster.View", "", "/app/main/master/inventory/customsleadtimemaster"),
                        new AppMenuItem("DEM/DET Days", "Pages.Master.CKD.DemDetDays.View", "", "/app/main/master/inventory/demdetdays"),
                        new AppMenuItem("DEM/DET Fees", "Pages.Master.CKD.DemDetFees.View", "", "/app/main/master/inventory/demdetfees"),

                    ]),
                    //Intransit
                    new AppMenuItem('Intransit', 'Pages.Ckd.Intransit', '', '', '', [], [
                        new AppMenuItem("Shipment", "Pages.Ckd.Intransit.Shipment.View", "", "/app/main/inventory/ckd/shipment"),
                        new AppMenuItem("Bill of Lading", "Pages.Ckd.Intransit.Bill.View", "", "/app/main/inventory/ckd/bill", 'fa fa-fax'),
                        new AppMenuItem("Invoice", "Pages.Ckd.Intransit.Invoice.View", "", "/app/main/inventory/ckd/invoice", 'fa fa-file-text'),
                        // new AppMenuItem("Frame Engine", "Pages.Ckd.Intransit.FrameEngine.View", "", "/app/main/inventory/ckd/frameengine"),
                        new AppMenuItem("Container Intransit", "Pages.Ckd.Intransit.ContainerIntransit.View", "", "/app/main/inventory/ckd/containerintransit", 'fa fa-truck'),
                        new AppMenuItem("Container Invoice", "Pages.Ckd.Intransit.ContainerInvoice.View", "", "/app/main/inventory/ckd/containerinvoice", 'fa fa-clipboard'),
                        // new AppMenuItem("Pre-Customs", "Pages.Ckd.Intransit.PreCustoms.View", "", "/app/main/inventory/ckd/precustoms"),
                        new AppMenuItem("Customs Declare", "Pages.Ckd.Intransit.CustomsDeclare.View", "", "/app/main/inventory/ckd/customsdeclare"),
                        new AppMenuItem("Payment Request", "Pages.Ckd.Intransit.PaymentRequest.View", "", "/app/main/inventory/ckd/paymentrequest"),
                        new AppMenuItem("Transit Port Plan", "Pages.Ckd.Intransit.ContainerTransitPortPlan.View", "", "/app/main/inventory/ckd/containertransitportplan"),
                    ]),
                    //Warehouse
                    new AppMenuItem('Warehouse', 'Pages.Ckd.Warehouse', '', '', '', [], [
                        new AppMenuItem("Container at Rental WH", "Pages.Ckd.Warehouse.ContainerRentalWHPlan.View", "", "/app/main/inventory/ckd/containerrentalwhplan"),
                        new AppMenuItem("CKD Request", "Pages.Ckd.Warehouse.Request.View", "", "/app/main/inventory/ckd/request"),
                        new AppMenuItem("Container Gate In", "Pages.Ckd.Warehouse.ContainerDeliveryGateIn.View", "", "/app/main/inventory/ckd/containerdeliverygatein"),
                        new AppMenuItem("Container List", "Pages.Ckd.Warehouse.ContainerList.View", "", "/app/main/inventory/ckd/containerlist"),
                        new AppMenuItem("Module List", "Pages.Ckd.Warehouse.ModuleCase.View", "", "/app/main/inventory/ckd/modulecase"),
                        new AppMenuItem("Part Management", "Pages.Ckd.Warehouse.PartManagement.View", "", "/app/main/inventory/ckd/partmanagement"),

                        // new AppMenuItem("Stock Part", "Pages.Ckd.Warehouse.StockPart.View", '', '/app/main/inventory/ckd/stock-part'),//TAB
                        // new AppMenuItem("Stock Receiving", "Pages.Ckd.Warehouse.StockReceiving.View", "", '/app/main/inventory/ckd/stock-receiving'),//TAB
                        // new AppMenuItem("Stock Issuing", "Pages.Ckd.Warehouse.StockIssuing.View", "", '/app/main/inventory/ckd/stock-issuing'),//TAB
                        // new AppMenuItem("Stock Balance", "Pages.Ckd.Warehouse.StockBalance.View", "", '/app/main/inventory/ckd/stock-balance'),//TAB
                    ]),
                    // //Prod. Area
                    // new AppMenuItem('Prod. Area', 'Pages.Ckd.Area', '', '', '', [], [
                    //     new AppMenuItem("Vehicle", "Pages.Ckd.Area.Vehicle.View", "", '/app/main/inventory/ckd/ckd-vehicle'),
                    // ]),
                    // //Physical Count
                    // new AppMenuItem('Theory Stk. Acct', 'Pages.Ckd.Physical', '', '', '', [], [
                    //     new AppMenuItem("Phys Stock Period", "Pages.Ckd.Physical.PhysicalStockPeriod.View", "", '/app/main/inventory/ckd/physical-stock-period'),    //TAB
                    //     new AppMenuItem("Phys Stock Part", "Pages.Ckd.Physical.PhysicalStockPart.View", "", '/app/main/inventory/ckd/physical-stock-part'),    //TAB
                    //     new AppMenuItem("Phys Stock Receiving", "Pages.Ckd.Physical.ReceivingPhysicalStock.View", "", '/app/main/inventory/ckd/physical-stock-receiving'),    //TAB
                    //     new AppMenuItem("Phys Stock Issuing", "Pages.Ckd.Physical.PhysicalStockIssuing.View", "", '/app/main/inventory/ckd/physical-stock-issuing'),    //TAB
                    //     new AppMenuItem("Phys Confirm Lot", "Pages.Ckd.Physical.PhysicalConfirmLot.View", "", "/app/main/inventory/ckd/physicalconfirmlot"),
                    //     new AppMenuItem("Phys Stock Part S4", "Pages.Ckd.Physical.PhysicalStockPartS4.View", "", "/app/main/inventory/ckd/physical-stock-part-s4"),
                    // ]),
                    // //SMQD
                    // new AppMenuItem('SMQD', 'Pages.Ckd.SMQD', '', '', '', [], [
                    //     new AppMenuItem("SMQD Management", "Pages.Ckd.SMQD.Management.View", "", '/app/main/inventory/ckd/smqdmanagement'),
                    //     new AppMenuItem("SMQD Order", "Pages.Ckd.SMQD.SmqdOrder.View", "", "/app/main/inventory/ckd/smqdorder"),
                    //     new AppMenuItem("Part Robbing", "Pages.Ckd.SMQD.PartRobbing.View", "", "/app/main/inventory/ckd/partrobbing"),
                    // ]),
                    // //Rundown
                    // new AppMenuItem('Rundown', 'Pages.Ckd.Rundown', '', '', '', [], [
                    //     new AppMenuItem("Shipping Schedule", "Pages.Ckd.Rundown.ShipingSchedule.View", "", '/app/main/inventory/ckd/shippingschedule'),
                    //     new AppMenuItem("Shipping Schedule Firm", "Pages.Ckd.Rundown.ShipingScheduleFirm.View", "", '/app/main/inventory/ckd/shippingschedulefirm'),
                    //     new AppMenuItem("Stock RD Shipping Sche", "Pages.Ckd.Rundown.StockRundownShipingSchedule.View", "", '/app/main/inventory/ckd/stock-rundown-shipping-schedule'),
                    //     new AppMenuItem("Stock RD Shipping Result", "Pages.Ckd.Rundown.StockRundownShipingResult.View", "", '/app/main/inventory/ckd/stock-rundown-shipping-result'),
                    //     new AppMenuItem("Stock RD Warehouse", "Pages.Ckd.Rundown.StockRundownShipingWarehouse.View", "", '/app/main/inventory/ckd/stock-rundown-warehouse'),
                    // ]),
                ]),

                /*GPS*/
            //     new AppMenuItem('GPS', 'Pages.Gps', '', '', '', [], [
            //         new AppMenuItem('Master ', 'Pages.Gps.Master', '', '', '', [], [
            //             new AppMenuItem("Part List By Category", "Pages.Gps.Master.PartListByCategory.View", "", "/app/main/inventory/gps/partlistbycategory"),
            //             new AppMenuItem("Part List", "Pages.Gps.Master.PartList.View", "", "/app/main/inventory/gps/partlist", 'fa fa-cube'),
            //             new AppMenuItem("Stock Concept", "Pages.Gps.Master.StockConcept.View", "", "/app/main/inventory/gps/invgpsstockconcept"),
            //             new AppMenuItem("Material", "Pages.Gps.Master.Material.View", "", "/app/main/inventory/gps/material"),
            //             new AppMenuItem("User", "Pages.Gps.User", "", "/app/main/inventory/gps/user"),
            //             new AppMenuItem("Gps Category Mapping", "Pages.Master.Gps.MaterialCategoryMapping.View", "", "/app/main/master/inv/gpsmasterialcategorymapping"),
            //             new AppMenuItem("Gps Register By Shop", "Pages.Master.Gps.GpsMaterialRegisterByShop.View", "", "/app/main/master/inv/gpsmaterialregisterbyshop"),
            //             new AppMenuItem("Gps Cost Center", "Pages.Master.Gps.CostCenter.View", "", "/app/main/master/inv/gpscostcenter"),
            //             new AppMenuItem("Gps Cost Uom", "Pages.Master.Gps.Uom.View", "", "/app/main/master/inv/uom"),
            //             new AppMenuItem("Gps Wbs CC Mapping", "Pages.Master.Gps.WbsCCMapping.View", "", "/app/main/master/inv/gpswbsccmapping"),
            //         ]),
            //         new AppMenuItem('Receive ', 'Pages.Gps.Receive', '', '', '', [], [
            //             new AppMenuItem("Content/Pallet", "Pages.Gps.Receive.ContentList.View", "", "/app/main/inventory/gps/invgpscontentlist"),
            //             new AppMenuItem("Kanban", "Pages.Gps.Receive.Kanban.View", "", "/app/main/inventory/gps/invgpskanban"),
            //             new AppMenuItem("Receiving", "Pages.Gps.Receive.View", "", "/app/main/inventory/gps/receiving"),
            //         ]),
            //         new AppMenuItem('Issuing ', 'Pages.GPS.Issuing', '', '', '', [], [
            //            // new AppMenuItem("Issuing", "Pages.GPS.Issuing.View", "", "/app/main/inventory/gps/issuing"),
            //             new AppMenuItem("Gps Issuing", "Pages.GPS.Issuings.View", "", "/app/main/inventory/gps/issuings"),
            //             new AppMenuItem("Mapping", "Pages.GPS.Issuing.Mapping.View", "", "/app/main/inventory/gps/mapping"),
            //         ]),

            //         new AppMenuItem('Order ', 'Pages.Gps.Order', '', '', '', [], [
            //             new AppMenuItem("Daily Order", "Pages.Gps.Order.DailyOrder.View", "", "/app/main/inventory/gps/invgpsdailyorder"),
            //         ]),
            //         new AppMenuItem('Rundown ', 'Pages.Gps.Rundown', '', '', '', [], [
            //             new AppMenuItem("Stock Rundown", "Pages.Gps.Rundown.GpsStockRundown.View", "", "/app/main/inventory/gps/gpsstockrundown"),
            //             //  new AppMenuItem("StockRundownTransaction", "Pages.Gps.Rundown.GpsStockRundownTransaction.View", "",   "/app/main/inventory/gps/gpsstockrundowntransaction"),

            //         ]),
            //         //Warehouse
            //         new AppMenuItem('Warehouse', 'Pages.Gps.Warehouse', '', '', '', [], [
            //             new AppMenuItem("Fin Stock", "Pages.Gps.Warehouse.FinStock.View", "", "/app/main/inventory/gps/findstock"),
            //             new AppMenuItem("Gps Stock", "Pages.Gps.Warehouse.GpsStock.View", "", "/app/main/inventory/gps/gpsstock"),
            //             new AppMenuItem("Stock Issuing Transaction Details ", "Pages.Gps.Warehouse.StockIssuingTransDetails.View", "", "/app/main/inventory/gps/stockissuingtransaction"),
            //             new AppMenuItem("Stock Receiving Transaction Details ", "Pages.Gps.Warehouse.StockReceivingTransDetails.View", "", "/app/main/inventory/gps/stockreceivingtransaction"),
            //         ]),

            //     ]),

            //     /* DM & IHP */
            //     new AppMenuItem('DM & IHP', 'Pages.DMIHP', '', '', '', [], [
            //         new AppMenuItem('Master ', 'Pages.DMIHP.Mst', '', '', '', [], [
            //             new AppMenuItem("DRM Part List", "Pages.DMIHP.Mst.DRMPartList.View", "", "/app/main/inventory/dm/drmpartlist"),
            //             new AppMenuItem("IHP Part List", "Pages.DMIHP.Mst.IHPPartList.View", "", "/app/main/inventory/ihp/ihppartlist"),
            //             new AppMenuItem("IHP MAT Customs Declare", "Pages.DMIHP.Mst.InvIphMatCustomDeclare.View", "", "/app/main/inventory/ihp/matcustomdeclare"),
            //         ]),
            //         new AppMenuItem('GR ', 'Pages.DMIHP.GR', '', '', '', [], [
            //             new AppMenuItem("Import Plan", "Pages.DMIHP.GR.ImportPlan.View", "", "/app/main/inventory/dm/drmimportplan"),
            //             new AppMenuItem("Local Plan", "Pages.DMIHP.GR.LocalPlan.View", "", "/app/main/inventory/dm/drmlocalplan"),
            //         ]),
            //         new AppMenuItem('GI ', 'Pages.DMIHP.GI', '', '', '', []),
            //         new AppMenuItem('Drm Stock ', 'Pages.DMIHP.Stock', '', '', '', [], [
            //             new AppMenuItem("Stock Part", "Pages.DMIHP.Stock.StockPart.View", "", "/app/main/inventory/dm/drmstockpart"),
            //             new AppMenuItem("Stock Part Excel", "Pages.DMIHP.Stock.StockPartExcel.View", "", "/app/main/inventory/dm/drmstockpartexcel"),
            //         ]),
            //         new AppMenuItem('Ihp Stock ', 'Pages.DMIHP.Stock', '', '', '', [], [
            //             new AppMenuItem("IHP Stock Part", "Pages.DMIHP.Stock.InvIphStockPart.View", "", "/app/main/inventory/ihp/ihpstockpart"),
            //         ]),
            //         new AppMenuItem('Rundown ', 'Pages.DMIHP.Rundow', '', '', '', [], [
            //             new AppMenuItem("Stock Rundown", "Pages.DMIHP.Rundow.StockRundown.View", "", "/app/main/inventory/dm/drmstockrundown"),
            //         ]),
            //         // new AppMenuItem('Order ', 'Pages.DMIHP.Order', '', '', '',[], '',[]),
            //     ]),

            //     //PIO
            //     new AppMenuItem('PIO & ACC', 'Pages.PIO', '', '', '', [], [
            //         new AppMenuItem('Master', 'Pages.PIO.Master', '', '', '', [], [
            //             new AppMenuItem("Part List", "Pages.PIO.Master.PartList.View", "", "/app/main/inventory/pio/partlist"),
            //             new AppMenuItem("Part List Offline", "Pages.PIO.Master.PartListOff.View", "", "/app/main/inventory/pio/partlistoff"),
            //             // new AppMenuItem("Tmss Dispatch Plan", "Pages.PIO.Master.Tmss.TmssDispatchPlan.View", "", "/app/main/inventory/pio/tmssdispatchplan"),
            //             //new AppMenuItem("Topsse Invoice", "Pages.PIO.Master.TopsseInvoice.View", "", "/app/main/inventory/pio/topsseinvoice"),
            //             new AppMenuItem("Supplier Info", "Pages.PIO.Master.SupplierInfo.View", "", "/app/main/inventory/pio/lspsupplierinfo"),
            //             new AppMenuItem("PIO Part Type", "Pages.Master.Inventory.PIOPartType.View", "", "/app/main/master/inventory/pioparttype"),
            //             new AppMenuItem("Pio Imp Supplier", "Pages.Master.MstPioImpSupplier.View", "", "/app/main/master/inventory/pioimpsupplier"),
            //         ]),
            //         new AppMenuItem('Production plan', 'Pages.PIO', '', '', '', [], [
            //             new AppMenuItem("Tmss Dispatch Plan", "Pages.PIO.Master.Tmss.TmssDispatchPlan.View", "", "/app/main/inventory/pio/tmssdispatchplan"),
            //             new AppMenuItem("Production Plan Monthly", "Pages.PIO.Master.ProductionPlanMonthly.View", "", "/app/main/inventory/pio/pioproductionplanmonthly"),
            //         ]),
            //         new AppMenuItem('Receiving', 'Pages.PIO', '', '', '', [], [
            //             new AppMenuItem("Topsse Invoice", "Pages.PIO.Master.TopsseInvoice.View", "", "/app/main/inventory/pio/topsseinvoice"),
            //         ]),
            //         // new AppMenuItem('Issuing', 'Pages.PIO', '', '', '', [], [

            //         // ]),
            //         new AppMenuItem('Warehouse', 'Pages.PIO.Warehouse', '', '', '', [], [
            //             new AppMenuItem("Stock ", "Pages.PIO.Warehouse.Stock.View", "", "/app/main/inventory/pio/stock"),
            //             // new AppMenuItem("Stock Transaction", "Pages.PIO.Warehouse.StockTransaction.View", "", "/app/main/inventory/pio/stocktransaction"),
            //             new AppMenuItem("Stock Issuing ", "Pages.PIO.Warehouse.StockIssuing.View", "", "/app/main/inventory/pio/stockissuing"),
            //             new AppMenuItem("Stock Rundown", "Pages.PIO.Warehouse.StockRundown.View", "", "/app/main/inventory/pio/stockrundown"),
            //             new AppMenuItem("Stock Rundown Transaction", "Pages.PIO.Warehouse.StockRundownTransaction.View", "", "/app/main/inventory/pio/stockrundowntransaction"),
            //             new AppMenuItem("Stock Receiving ", "Pages.PIO.Warehouse.StockReceiving.View", "", "/app/main/inventory/pio/stockreceiving"),
            //         ]),
            //         // new AppMenuItem('Rundown', 'Pages.PIO', '', '', '', [], [
            //         // ]),
            //         // new AppMenuItem('NQC', 'Pages.PIO', '', '', '', [], [
            //         // ]),
            //         // new AppMenuItem('Order', 'Pages.PIO', '', '', '', [], [
            //         // ]),
            //     ]),

            //     /* INTERFACE */
            //     new AppMenuItem('INTERFACE', 'Pages.Interface', '', '', '', [], [
            //         new AppMenuItem('Bom', 'Pages.Interface.Bom', '', '', '', [], [
            //             new AppMenuItem("Gen BOM Data", "Pages.Interface.Bom.InvGenBOMData.View", "", "/app/main/inventory/bom/genbomdatat"),
            //         ]),
            //         new AppMenuItem('IF', 'Pages.Interface.IF', '', '', '', [], [
            //             new AppMenuItem("FQF3MM01", "Pages.Interface.IF.FQF3MM01.View", "", "/app/main/inventory/if/fqf3mm01", 'fa fa-car'),
            //             new AppMenuItem("FQF3MM02", "Pages.Interface.IF.FQF3MM02.View", "", "/app/main/inventory/if/fqf3mm02", 'fa fa-car'),
            //             new AppMenuItem("FQF3MM_LV2", "Pages.Interface.IF.FQF3MM_LV2.View", "", "/app/main/inventory/if/FQF3MM_LV2", 'fa fa-car'),
            //             new AppMenuItem("FQF3MM03", "Pages.Interface.IF.FQF3MM03.View", "", "/app/main/inventory/if/fqf3mm03", 'fa fa-car'),
            //             new AppMenuItem("FQF3MM04", "Pages.Interface.IF.FQF3MM04.View", "", "/app/main/inventory/if/fqf3mm04", 'fa fa-car'),
            //             new AppMenuItem("FQF3MM05", "Pages.Interface.IF.FQF3MM05.View", "", "/app/main/inventory/if/fqf3mm05", 'fa fa-car'),
            //             new AppMenuItem("FQF3MM06", "Pages.Interface.IF.FQF3MM06.View", "", "/app/main/inventory/if/fqf3mm06", 'fa fa-car'),
            //             new AppMenuItem("FQF3MM07", "Pages.Interface.IF.FQF3MM07.View", "", "/app/main/inventory/if/fqf3mm07", 'fa fa-car'),
            //         ]),
            //     ]),

            //     //SPP
            //     new AppMenuItem('SPP', 'Pages.SPP', '', '', '', [], [
            //         /* MASTER SPP*/
            //         new AppMenuItem('Master', 'Pages.SPP.Master', '', '', '', [], [
            //             new AppMenuItem("Customer", "Pages.SPP.Master.Customer", "",   "/app/main/master/spp/customer"),
            //             new AppMenuItem("GLAccount", "Pages.SPP.Master.GLAccount", "",   "/app/main/master/spp/glaccount"),

            //         ]),
            //         new AppMenuItem('Inventory', 'Pages.SPP.Inventory', '', '', '', [],
            //             [
            //                 new AppMenuItem("Cost", "Pages.SPP.Inventory.Cost.View", "", "/app/main/inventory/spp/cost"),
            //                 new AppMenuItem("Invoice Details", "Pages.SPP.Inventory.InvoiceDetails.View", "", "/app/main/inventory/spp/invoicedetails"),
            //                 new AppMenuItem("Shipping", "Pages.SPP.Inventory.Shipping.View", "", "/app/main/inventory/spp/shipping"),
            //                 new AppMenuItem("Stock", "Pages.SPP.Inventory.Stock.View", "", "/app/main/inventory/spp/stock"),
            //                 new AppMenuItem("Cost of Sale Sum", "Pages.SPP.Inventory.CostOfSaleSummary.View", "", "/app/main/inventory/spp/costofsalesummary"),
            //             ]
            //         )
            //     ]),

                new AppMenuItem('Administration', '', '', '', '', [],
                    [
                        new AppMenuItem('OrganizationUnits', 'Pages.Administration.OrganizationUnits', 'flaticon-map', '/app/admin/organization-units'),
                        new AppMenuItem('Roles', 'Pages.Administration.Roles', 'flaticon-suitcase', '/app/admin/roles'),
                        new AppMenuItem('Users', 'Pages.Administration.Users', 'flaticon-users', '/app/admin/users'),
                        new AppMenuItem('Languages', 'Pages.Administration.Languages', 'flaticon-tabs', '/app/admin/languages', '', ['/app/admin/languages/{name}/texts']),
                        new AppMenuItem('AuditLogs', 'Pages.Administration.AuditLogs', 'flaticon-folder-1', '/app/admin/auditLogs'),
                        new AppMenuItem('Maintenance', 'Pages.Administration.Host.Maintenance', 'flaticon-lock', '/app/admin/maintenance'),
                        new AppMenuItem('Subscription', 'Pages.Administration.Tenant.SubscriptionManagement', 'flaticon-refresh', '/app/admin/subscription-management'),
                        new AppMenuItem('VisualSettings', 'Pages.Administration.UiCustomization', 'flaticon-medical', '/app/admin/ui-customization'),
                        new AppMenuItem('WebhookSubscriptions', 'Pages.Administration.WebhookSubscription', 'flaticon2-world', '/app/admin/webhook-subscriptions'),
                        new AppMenuItem('DynamicProperties', 'Pages.Administration.DynamicProperties', '', '/app/admin/dynamic-property'),
                        new AppMenuItem('Settings', 'Pages.Administration.Host.Settings', 'flaticon-settings', '/app/admin/hostSettings'),
                        new AppMenuItem('Settings', 'Pages.Administration.Tenant.Settings', 'flaticon-settings', '/app/admin/tenantSettings'),
                        new AppMenuItem('Notifications', '', 'flaticon-alarm', '', '', [],
                            [
                                new AppMenuItem('Inbox', '', 'flaticon-mail-1', '/app/notifications'),
                                new AppMenuItem('MassNotifications', 'Pages.Administration.MassNotification', 'flaticon-paper-plane', '/app/admin/mass-notifications')
                            ]
                        )
                    ]
                ),
            //     // new AppMenuItem('DemoUiComponents', 'Pages.DemoUiComponents', '', '/app/admin/demo-ui-components'),
            //     // // new AppMenuItem('Dashboard', 'Pages.Tenant.Dashboard', 'flaticon-line-graph', '/app/main/dashboard'),
            //     // new AppMenuItem('Dashboard', 'Pages.Administration.Host.Dashboard', 'flaticon-line-graph', '/app/admin/hostDashboard'),
            //     // new AppMenuItem('Tenants', 'Pages.Tenants', 'flaticon-list-3', '/app/admin/tenants'),
            //     // new AppMenuItem('Editions', 'Pages.Editions', 'flaticon-app', '/app/admin/editions'),

            ])
    }

    checkChildMenuItemPermission(menuItem): boolean {
        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName === '' || subMenuItem.permissionName === null) {
                if (subMenuItem.route) {
                    return true;
                }
            } else if (this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            }

            if (subMenuItem.items && subMenuItem.items.length) {
                let isAnyChildItemActive = this.checkChildMenuItemPermission(subMenuItem);
                if (isAnyChildItemActive) {
                    return true;
                }
            }
        }
        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (
            menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' &&
            this._appSessionService.tenant &&
            !this._appSessionService.tenant.edition
        ) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }

    /**
     * Returns all menu items recursively
     */
    getAllMenuItems(): AppMenuItem[] {
        let menu = this.getMenu();
        let allMenuItems: AppMenuItem[] = [];
        menu.items.forEach((menuItem) => {
            allMenuItems = allMenuItems.concat(this.getAllMenuItemsRecursive(menuItem));
        });

        return allMenuItems;
    }

    private getAllMenuItemsRecursive(menuItem: AppMenuItem): AppMenuItem[] {
        if (!menuItem.items) {
            return [menuItem];
        }

        let menuItems = [menuItem];
        menuItem.items.forEach((subMenu) => {
            menuItems = menuItems.concat(this.getAllMenuItemsRecursive(subMenu));
        });

        return menuItems;
    }
}
