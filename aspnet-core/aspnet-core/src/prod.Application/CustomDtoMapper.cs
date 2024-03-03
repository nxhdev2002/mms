using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using IdentityServer4.Extensions;
using prod.Auditing.Dto;
using prod.Authorization.Accounts.Dto;
using prod.Authorization.Delegation;
using prod.Authorization.Permissions.Dto;
using prod.Authorization.Roles;
using prod.Authorization.Roles.Dto;
using prod.Authorization.Users;
using prod.Authorization.Users.Delegation.Dto;
using prod.Authorization.Users.Dto;
using prod.Authorization.Users.Importing.Dto;
using prod.Authorization.Users.Profile.Dto;
using prod.Chat;
using prod.Chat.Dto;
using prod.DynamicEntityProperties.Dto;
using prod.Editions;
using prod.Editions.Dto;
using prod.Friendships;
using prod.Friendships.Cache;
using prod.Friendships.Dto;
using prod.Localization.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common;
using prod.MultiTenancy;
using prod.MultiTenancy.Dto;
using prod.MultiTenancy.HostDashboard.Dto;
using prod.MultiTenancy.Payments;
using prod.MultiTenancy.Payments.Dto;
using prod.Notifications.Dto;
using prod.Organizations.Dto;
using prod.Sessions.Dto;
using prod.WebHooks.Dto;
using prod.Master.Cmm;
using prod.Master.Cmm.Dto;
using prod.Master.LogW.Dto;
using prod.Master.LogW;
using prod.Master.WorkingPattern.Dto;
using prod.Master.WorkingPattern;
using prod.Master.Welding.Dto;
using prod.Master.Welding;
using prod.Master.Frame;
using prod.Master.Frame.Dto;
using prod.Master.Painting.Dto;
using prod.Master.Painting;
using prod.Master.Painting.BmpPartType.Dto;
using prod.Master.LogA.Dto;
using prod.Master.LogA;
using prod.Master.LogA.Bp2PartListGrade.Dto;
using prod.Frame.Andon.Dto;
using prod.Frame.Andon;
using prod.LogA.Bar.Dto;
using prod.LogA.Bar;
using prod.LogA.Bp2.Dto;
using prod.LogA.Bp2.PxPUpPlan.Dto;
using prod.LogA.Bp2;
using prod.LogA.Ekb.Dto;
using prod.LogA.Ekb;
using prod.LogA.Lds.Dto;
using prod.LogA.Lds;
using prod.LogA.Pcs.Dto;
using prod.LogA.Pcs;
using prod.LogA.Sps.Dto;
using prod.LogA.Sps;
using prod.LogW.Ado;
using prod.LogW.Dvn;
using prod.LogW.Lup;
using prod.LogW.Mwh;
using prod.LogW.Pik;
using prod.LogW.Pup;
using prod.LogW.Mwh.Dto;
using prod.LogW.Ado.Dto;
using prod.LogW.Dvn.Dto;
using prod.LogW.Lup.Dto;
using prod.LogW.Pup.Dto;
using prod.LogW.Pik.Dto;
using prod.Inv.D125.Dto;
using prod.Inv.D125;
using prod.Inv.Dmr.Dto;
using prod.Inv.Dmr;
using prod.Inv.Proc.Dto;
using prod.Inv.Proc;
using prod.Painting.Andon.Dto;
using prod.Painting.Andon;
using prod.Plan.Ccr.Dto;
using prod.Plan.Ccr;
using prod.Welding.Andon.Dto;
using prod.Welding.Andon;
using prod.Assy.Andon.Dto;
using prod.Assy.Andon;
using prod.Master.Common.LookUp2.Dto;
using prod.LogA.Plc.Signal.Dto;
using prod.LogA.Plc;
using prod.Inventory.CKD;
using prod.Inventory.CKD.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory;
using prod.Master.Plm.Dto;
using prod.Master.Plm;
using prod.Master.Inventory.ContainerDeliveryType.Dto;
using prod.Master.Inventory.ContainerStatus.Dto;
using prod.Master.Inventory.Forwarder.Dto;
using prod.Inventory.Invoice.Dto;
using prod.Master.Inv;
using prod.Master.Inv.Dto;
using prod.LogA.Ekb.Ekanban.Dto;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS;
using prod.Inventory.CPS.Dto;
using prod.Inventory.CPS;
using prod.Inventory.Gps.PartList.Dto;
using prod.Inventory.CKD.PartRobbing.Dto;
using prod.Master.Inventory.LotPart.Dto;
using prod.Master.Inventory.HrTitles.Dto;
using prod.Master.CKD.Dto;
using prod.Master.CKD;
using prod.Inventory.IHP.Dto;
using prod.Inventory.IHP;
using prod.Inventory.DRM.Dto;
using prod.Inventory.DRM;
using prod.Inventory.DRM.StockPart.Dto;
using prod.Inventory.CKD.SmqdOrderLeadTime.Dto;
using prod.Inventory.Tmss.Dto;
using prod.Inventory.Tmss;
using prod.Inventory.PIO.PartList.Dto;
using prod.Inventory.PIO;
using prod.Inventory.PIO.StockReceiving.Dto;
using prod.Master.Common.CarSeries.Dto;
using prod.Master.Common.DriveTrain.Dto;
using prod.Master.Inventory.GpsCategory.Dto;
using prod.Inventory.SPP.CostOfSaleSummary.Dto;
using prod.Inventory.SPP;
using prod.Inventory.SPP.Cost.Dto;
using prod.Inventory.SPP.InvoiceDetails.Dto;
using prod.Inventory.SPP.Shipping.Dto;
using prod.Inventory.SPP.Stock.Dto;
using prod.Master.Spp;
using prod.Master.Spp.Dto;
using prod.SapIF.Dto;
using prod.SapIF;
using prod.Inventory.Gps.User.Dto;
using prod.Master.Inventory.GpsMaterialCategory.Dto;
using prod.Inventory.Gps.PartListByCategory.Dto;
using prod.Master.Pio.DTO;
using prod.Master.Pio;
using prod.Inventory.CKD.ShippingScheduleFirm.Dto;
using prod.Master.Pio.Dto;

namespace prod
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.DisplayName.IsNullOrEmpty() ? entity.DynamicProperty.PropertyName : entity.DynamicProperty.DisplayName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();
            
            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
            #region ASSY
            //ASSEMBLY - VehicleDetails
            configuration.CreateMap<AsyAdoVehicleDetailsDto, AsyAdoVehicleDetails>();
            configuration.CreateMap<AsyAdoVehicleDetails, AsyAdoVehicleDetailsDto>();
            // Asakai
            configuration.CreateMap<AsyAdoTotalDelayFinalAsakaiDto, AsyAdoTotalDelayFinalAsakai>();
            configuration.CreateMap<AsyAdoTotalDelayFinalAsakai, AsyAdoTotalDelayFinalAsakaiDto>();
            configuration.CreateMap<CreateOrEditAsyAdoTotalDelayFinalAsakaiDto, AsyAdoTotalDelayFinalAsakai>();

            // Alnplan
            configuration.CreateMap<AsyAdoAInPlanDto, AsyAdoAInPlan>();
            configuration.CreateMap<AsyAdoAInPlan, AsyAdoAInPlanDto>();

            // Assy - PlanShift
            configuration.CreateMap<AsyAdoAPlanShiftDto, AsyAdoAPlanShift>();
            configuration.CreateMap<AsyAdoAPlanShift, AsyAdoAPlanShiftDto>();

            //Asy-AssemblyData
            configuration.CreateMap<AsyAdoAssemblyDataDto, AsyAdoAssemblyData>();
            configuration.CreateMap<AsyAdoAssemblyData, AsyAdoAssemblyDataDto>();
            #endregion

            #region MASTER
            // MASTER 
            // MASTER - Working Pattern - Calendar
            configuration.CreateMap<MstWptCalendarDto, MstWptCalendar>();
            configuration.CreateMap<MstWptCalendar, MstWptCalendarDto>();
            configuration.CreateMap<CreateOrEditMstWptCalendarDto, MstWptCalendar>();

            //MASTER - Working Pattern - Shop
            configuration.CreateMap<MstWptShopDto, MstWptShop>();
            configuration.CreateMap<MstWptShop, MstWptShopDto>();
            configuration.CreateMap<CreateOrEditMstWptShopDto, MstWptShop>();

            //MASTER - Working Pattern - SeasonMonth
            configuration.CreateMap<MstWptSeasonMonthDto, MstWptSeasonMonth>();
            configuration.CreateMap<MstWptSeasonMonth, MstWptSeasonMonthDto>();
            configuration.CreateMap<CreateOrEditMstWptSeasonMonthDto, MstWptSeasonMonth>();

            //MASTER - Working Pattern - PatternH
            configuration.CreateMap<MstWptPatternHDto, MstWptPatternH>();
            configuration.CreateMap<MstWptPatternH, MstWptPatternHDto>();
            configuration.CreateMap<CreateOrEditMstWptPatternHDto, MstWptPatternH>();

            //MASTER - Working Pattern -PatternD
            configuration.CreateMap<MstWptPatternDDto, MstWptPatternD>();
            configuration.CreateMap<MstWptPatternD, MstWptPatternDDto>();
            configuration.CreateMap<CreateOrEditMstWptPatternDDto, MstWptPatternD>();

            // MASTER - Working Pattern - Week
            configuration.CreateMap<MstWptWeekDto, MstWptWeek>();
            configuration.CreateMap<MstWptWeek, MstWptWeekDto>();
            configuration.CreateMap<CreateOrEditMstWptWeekDto, MstWptWeek>();

            //MASTER - Working Pattern - WorkingTime
            configuration.CreateMap<MstWptWorkingTimeDto, MstWptWorkingTime>();
            configuration.CreateMap<MstWptWorkingTime, MstWptWorkingTimeDto>();
            configuration.CreateMap<CreateOrEditMstWptWorkingTimeDto, MstWptWorkingTime>();

            //MASTER - Working Pattern - WorkingType
            configuration.CreateMap<MstWptWorkingTypeDto, MstWptWorkingType>();
            configuration.CreateMap<MstWptWorkingType, MstWptWorkingTypeDto>();
            configuration.CreateMap<CreateOrEditMstWptWorkingTypeDto, MstWptWorkingType>();

            //MASTER - Working Pattern - DailyWorkingTime
            configuration.CreateMap<MstWptDailyWorkingTimeDto, MstWptDailyWorkingTime>();
            configuration.CreateMap<MstWptDailyWorkingTime, MstWptDailyWorkingTimeDto>();
            configuration.CreateMap<CreateOrEditMstWptDailyWorkingTimeDto, MstWptDailyWorkingTime>();

            //MASTER - Welding - Process
            configuration.CreateMap<MstWldProcessDto, MstWldProcess>();
            configuration.CreateMap<MstWldProcess, MstWldProcessDto>();
            configuration.CreateMap<CreateOrEditMstWldProcessDto, MstWldProcess>();

            //MASTER - Welding - PnchIndicator
            configuration.CreateMap<MstWldPunchIndicatorDto, MstWldPunchIndicator>();
            configuration.CreateMap<MstWldPunchIndicator, MstWldPunchIndicatorDto>();
            configuration.CreateMap<CreateOrEditMstWldPunchIndicatorDto, MstWldPunchIndicator>();


            // Master -- Frame
            configuration.CreateMap<MstFrmProcessDto, MstFrmProcess>();
            configuration.CreateMap<MstFrmProcess, MstFrmProcessDto>();
            configuration.CreateMap<CreateOrEditMstFrmProcessDto, MstFrmProcess>();



            // MASTER - Painting - Painting Process
            configuration.CreateMap<MstPtsPaintingProcessDto, MstPtsPaintingProcess>();
            configuration.CreateMap<MstPtsPaintingProcess, MstPtsPaintingProcessDto>();
            configuration.CreateMap<CreateOrEditMstPtsPaintingProcessDto, MstPtsPaintingProcess>();

            // MASTER - Painting - Bmp Part List
            configuration.CreateMap<MstPtsBmpPartListDto, MstPtsBmpPartList>();
            configuration.CreateMap<MstPtsBmpPartList, MstPtsBmpPartListDto>();
            configuration.CreateMap<CreateOrEditMstPtsBmpPartListDto, MstPtsBmpPartList>();

            //MASTER - Painting - Bmp Part Type
            configuration.CreateMap<MstPtsBmpPartTypeDto, MstPtsBmpPartType>();
            configuration.CreateMap<MstPtsBmpPartType, MstPtsBmpPartTypeDto>();
            configuration.CreateMap<CreateOrEditMstPtsBmpPartTypeDto, MstPtsBmpPartType>();

            configuration.CreateMap<MstPtsInventoryStdDto, MstPtsInventoryStd>();
            configuration.CreateMap<MstPtsInventoryStd, MstPtsInventoryStdDto>();
            configuration.CreateMap<CreateOrEditMstPtsInventoryStdDto, MstPtsInventoryStd>();


            //Master-LogW
            //Master - LogW - EciPart
            configuration.CreateMap<MstLgwEciPartDto, MstLgwEciPart>();
            configuration.CreateMap<MstLgwEciPart, MstLgwEciPartDto>();
            configuration.CreateMap<CreateOrEditMstLgwEciPartDto, MstLgwEciPart>();

            //Master - LogW - ModelGrade
            configuration.CreateMap<MstLgwModelGradeDto, MstLgwModelGrade>();
            configuration.CreateMap<MstLgwModelGrade, MstLgwModelGradeDto>();
            configuration.CreateMap<CreateOrEditMstLgwModelGradeDto, MstLgwModelGrade>();
            //Master - LogW -ScreenConfig

            configuration.CreateMap<MstLgwScreenConfigDto, MstLgwScreenConfig>();
            configuration.CreateMap<MstLgwScreenConfig, MstLgwScreenConfigDto>();
            configuration.CreateMap<CreateOrEditMstLgwScreenConfigDto, MstLgwScreenConfig>();

            //Master - LogW - PickingTablet
            configuration.CreateMap<MstLgwPickingTabletDto, MstLgwPickingTablet>();
            configuration.CreateMap<MstLgwPickingTablet, MstLgwPickingTabletDto>();
            configuration.CreateMap<CreateOrEditMstLgwPickingTabletDto, MstLgwPickingTablet>();

            //
            configuration.CreateMap<MstLgwAdoCallingLightDto, MstLgwAdoCallingLight>();
            configuration.CreateMap<MstLgwAdoCallingLight, MstLgwAdoCallingLightDto>();
            configuration.CreateMap<CreateOrEditMstLgwAdoCallingLightDto, MstLgwAdoCallingLight>();

            //Master - LogA
            //Master - LogA - PlcSignal
            configuration.CreateMap<MstLgaPlcSignalDto, MstLgaPlcSignal>();
            configuration.CreateMap<MstLgaPlcSignal, MstLgaPlcSignalDto>();
            configuration.CreateMap<CreateOrEditMstLgaPlcSignalDto, MstLgaPlcSignal>();
            //Master - LogA - LdsTrip
            configuration.CreateMap<MstLgaLdsTripDto, MstLgaLdsTrip>();
            configuration.CreateMap<MstLgaLdsTrip, MstLgaLdsTripDto>();
            configuration.CreateMap<CreateOrEditMstLgaLdsTripDto, MstLgaLdsTrip>();

            //Master - LogA -Bp2PartList
            configuration.CreateMap<MstLgaBp2PartListDto, MstLgaBp2PartList>();
            configuration.CreateMap<MstLgaBp2PartList, MstLgaBp2PartListDto>();
            configuration.CreateMap<CreateOrEditMstLgaBp2PartListDto, MstLgaBp2PartList>();

            //Master - LogA - LgaBp2Process
            configuration.CreateMap<MstLgaBp2ProcessDto, MstLgaBp2Process>();
            configuration.CreateMap<MstLgaBp2Process, MstLgaBp2ProcessDto>();
            configuration.CreateMap<CreateOrEditMstLgaBp2ProcessDto, MstLgaBp2Process>();


            //Master - LogA - Bp2PartListGrade
            configuration.CreateMap<MstLgaBp2PartListGradeDto, MstLgaBp2PartListGrade>();
            configuration.CreateMap<MstLgaBp2PartListGrade, MstLgaBp2PartListGradeDto>();
            configuration.CreateMap<CreateOrEditMstLgaBp2PartListGradeDto, MstLgaBp2PartListGrade>();

            //Master - LogA - EkbPartList
            configuration.CreateMap<MstLgaEkbPartListDto, MstLgaEkbPartList>();
            configuration.CreateMap<MstLgaEkbPartList, MstLgaEkbPartListDto>();
            configuration.CreateMap<CreateOrEditMstLgaEkbPartListDto, MstLgaEkbPartList>();

            ////Master - LogA - EkbPartListGrade
            configuration.CreateMap<MstLgaEkbPartListGradeDto, MstLgaEkbPartListGrade>();
            configuration.CreateMap<MstLgaEkbPartListGrade, MstLgaEkbPartListGradeDto>();
            configuration.CreateMap<CreateOrEditMstLgaEkbPartListGradeDto, MstLgaEkbPartListGrade>();
            // Master - LogA - EkbProcess
            configuration.CreateMap<MstLgaEkbProcessDto, MstLgaEkbProcess>();
            configuration.CreateMap<MstLgaEkbProcess, MstLgaEkbProcessDto>();
            configuration.CreateMap<CreateOrEditMstLgaEkbProcessDto, MstLgaEkbProcess>();

            //Master - LogA - EkbUser
            configuration.CreateMap<MstLgaEkbUserDto, MstLgaEkbUser>();
            configuration.CreateMap<MstLgaEkbUser, MstLgaEkbUserDto>();
            configuration.CreateMap<CreateOrEditMstLgaEkbUserDto, MstLgaEkbUser>();

            //Master - Lga - BarProcess
            configuration.CreateMap<MstLgaBarProcessDto, MstLgaBarProcess>();
            configuration.CreateMap<MstLgaBarProcess, MstLgaBarProcessDto>();
            configuration.CreateMap<CreateOrEditMstLgaBarProcessDto, MstLgaBarProcess>();

            //Master - Lga - BarUser
            configuration.CreateMap<MstLgaBarUserDto, MstLgaBarUser>();
            configuration.CreateMap<MstLgaBarUser, MstLgaBarUserDto>();
            configuration.CreateMap<CreateOrEditMstLgaBarUserDto, MstLgaBarUser>();

            //Master - Lga - SpsRack
            configuration.CreateMap<MstLgaSpsRackDto, MstLgaSpsRack>();
            configuration.CreateMap<MstLgaSpsRack, MstLgaSpsRackDto>();
            configuration.CreateMap<CreateOrEditMstLgaSpsRackDto, MstLgaSpsRack>();
            //Master - Lga - PcRack
            configuration.CreateMap<MstLgaPcRackDto, MstLgaPcRack>();
            configuration.CreateMap<MstLgaPcRack, MstLgaPcRackDto>();
            configuration.CreateMap<CreateOrEditMstLgaPcRackDto, MstLgaPcRack>();

			//MASTER - COMMON - PRODUCT TYPE
			configuration.CreateMap<MstCmmProductTypeDto, MstCmmProductType>();
			configuration.CreateMap<MstCmmProductType, MstCmmProductTypeDto>();
			configuration.CreateMap<CreateOrEditMstCmmProductTypeDto, MstCmmProductType>();
			// MASTER - Common - Lookup
			configuration.CreateMap<MstCmmLookupDto, MstCmmLookup>();
            configuration.CreateMap<MstCmmLookup, MstCmmLookupDto>();
            configuration.CreateMap<CreateOrEditMstCmmLookupDto, MstCmmLookup>();

            // MASTER - Common - Lookup     
            configuration.CreateMap<CreateOrEditMstLookUpDto, MstCmmLookup>().ReverseMap();

            // MASTER - Common Material Group
            configuration.CreateMap<MstCmmMaterialGroupDto, MstCmmMaterialGroup>();
            configuration.CreateMap<MstCmmMaterialGroup, MstCmmMaterialGroupDto>();

            // MASTER - Common Storage Location
            configuration.CreateMap<MstCmmStorageLocationDto, MstCmmStorageLocation>();
            configuration.CreateMap<MstCmmStorageLocation, MstCmmStorageLocationDto>();
         

            //MASTER - Common - TaktTime
            configuration.CreateMap<MstCmmTaktTimeDto, MstCmmTaktTime>();
            configuration.CreateMap<MstCmmTaktTime, MstCmmTaktTimeDto>();
            configuration.CreateMap<CreateOrEditMstCmmTaktTimeDto, MstCmmTaktTime>();

            //MASTER - Common - Model
            configuration.CreateMap<MstCmmModelDto, MstCmmModel>();
            configuration.CreateMap<MstCmmModel, MstCmmModelDto>();
            configuration.CreateMap<CreateOrEditMstCmmModelDto, MstCmmModel>();

            //Master_Common_DevanningCaseTypeConsts
            configuration.CreateMap<MstCmmDevanningCaseTypeDto, MstCmmDevanningCaseType>();
            configuration.CreateMap<MstCmmDevanningCaseType, MstCmmDevanningCaseTypeDto>();
            configuration.CreateMap<CreateOrEditMstCmmDevanningCaseTypeDto, MstCmmDevanningCaseType>();

            //Master_Common_FuelType
            configuration.CreateMap<MstCmmFuelTypeDto, MstCmmFuelType>();
            configuration.CreateMap<MstCmmFuelType, MstCmmFuelTypeDto>();
            configuration.CreateMap<CreateOrEditMstCmmFuelTypeDto, MstCmmFuelType>();


            //Master_Common_ShopType
            configuration.CreateMap<MstCmmShopTypeDto, MstCmmShopType>();
            configuration.CreateMap<MstCmmShopType, MstCmmShopTypeDto>();
            configuration.CreateMap<CreateOrEditMstCmmShopTypeDto, MstCmmShopType>();

            //Master_Common_Color
            configuration.CreateMap<MstCmmColorDto, MstCmmColor>();
            configuration.CreateMap<MstCmmColor, MstCmmColorDto>();
            configuration.CreateMap<CreateOrEditMstCmmColorDto, MstCmmColor>();

            //Master_Common_Uom
            configuration.CreateMap<MstCmmUomDto, MstCmmUom>();
            configuration.CreateMap<MstCmmUom, MstCmmUomDto>();
            configuration.CreateMap<CreateOrEditMstCmmUomDto, MstCmmUom>();

            //Master_Common_StorageLocationCategory
            configuration.CreateMap<MstCmmStorageLocationCategoryDto, MstCmmStorageLocationCategory>();
            configuration.CreateMap<MstCmmStorageLocationCategory, MstCmmStorageLocationCategoryDto>();

            //Master Common Brand
            configuration.CreateMap<MstCmmBrandDto, MstCmmBrand>();
            configuration.CreateMap<MstCmmBrand, MstCmmBrandDto>();
            configuration.CreateMap<CreateOrEditMstCmmBrandDto, MstCmmBrand>();

            //Master Common ProductGroup
            configuration.CreateMap<MstCmmProductGroupDto, MstCmmProductGroup>();
            configuration.CreateMap<MstCmmProductGroup, MstCmmProductGroupDto>();

            //MstCmmTransmissionType
            configuration.CreateMap<MstCmmTransmissionTypeDto, MstCmmTransmissionType>();
            configuration.CreateMap<MstCmmTransmissionType, MstCmmTransmissionTypeDto>();
            configuration.CreateMap<CreateOrEditMstCmmTransmissionTypeDto, MstCmmTransmissionType>();

            //Master Common VehicleColorType
            configuration.CreateMap<MstCmmVehicleColorTypeDto, MstCmmVehicleColorType>();
            configuration.CreateMap<MstCmmVehicleColorType, MstCmmVehicleColorTypeDto>();
            
            //Master Common VehicleName
            configuration.CreateMap<MstCmmVehicleNameDto, MstCmmVehicle>();
            configuration.CreateMap<MstCmmVehicle, MstCmmVehicleNameDto>();

            //Master Common MaterialType
            configuration.CreateMap<MstCmmMaterialTypeDto, MstCmmMaterialType>();
            configuration.CreateMap<MstCmmMaterialType, MstCmmMaterialTypeDto>();
            configuration.CreateMap<CreateOrEditMstCmmMaterialTypeDto, MstCmmMaterialType>();

            //Master Common ValuationClass
            configuration.CreateMap<MstCmmValuationClassDto, MstCmmValuationClass>();
            configuration.CreateMap<MstCmmValuationClass, MstCmmValuationClassDto>();

            //Master Common ValuationType
            configuration.CreateMap<MstCmmValuationTypeDto, MstCmmValuationType>();
            configuration.CreateMap<MstCmmValuationType, MstCmmValuationTypeDto>();

            //Master Common CustomDtoMapper
            configuration.CreateMap<MstCmmEngineMasterDto, MstCmmEngineMaster>();
            configuration.CreateMap<MstCmmEngineMaster, MstCmmEngineMasterDto>();
            configuration.CreateMap<CreateOrEditMstCmmEngineMasterDto, MstCmmEngineMaster>();

            //MstCmmMMCheckingRule
            configuration.CreateMap<MstCmmMMCheckingRuleDto, MstCmmMMCheckingRule>();
            configuration.CreateMap<MstCmmMMCheckingRule, MstCmmMMCheckingRuleDto>();
            configuration.CreateMap<CreateOrEditMstCmmMMCheckingRuleDto, MstCmmMMCheckingRule>();


            // MstCmmPlant
            configuration.CreateMap<MstCmmPlantDto, MstCmmPlant>();
            configuration.CreateMap<MstCmmPlant, MstCmmPlantDto>();

            //MstCmmCarSeries
            configuration.CreateMap<MstCmmCarSeriesDto, MstCmmCarSeries>();
            configuration.CreateMap<MstCmmCarSeries, MstCmmCarSeriesDto>();
            configuration.CreateMap<CreateOrEditMstCmmCarSeriesDto,MstCmmCarSeries>();
            
            //MstCmmCarfamily
            configuration.CreateMap<MstCmmCarfamilyDto, MstCmmCarfamily>();
            configuration.CreateMap<MstCmmCarfamily, MstCmmCarfamilyDto>();
            configuration.CreateMap<CreateOrEditMstCmmCarfamilyDto, MstCmmCarfamily>(); 


            //MstCmmDriveTrain
            configuration.CreateMap<MstCmmDriveTrainDto, MstCmmDriveTrain>();
            configuration.CreateMap<MstCmmDriveTrain, MstCmmDriveTrainDto>();
            configuration.CreateMap<CreateOrEditMstCmmDriveTrainDto, MstCmmDriveTrain>();

            // Master Common Material Master
            configuration.CreateMap<MstCmmMaterialMasterDto, MstCmmMaterialMaster>();
            configuration.CreateMap<MstCmmMaterialMaster, MstCmmMaterialMasterDto>();
            configuration.CreateMap<CreateOrEditMstCmmMaterialMasterDto, MstCmmMaterialMaster>();


            // Master Common BusinessParter
            configuration.CreateMap<MstCmmBusinessParterDto, MstCmmBusinessParter>();
            configuration.CreateMap<MstCmmBusinessParter, MstCmmBusinessParterDto>();
            configuration.CreateMap<CreateOrEditMstCmmBusinessParterDto, MstCmmBusinessParter>();

            // Master Common EngineModel
            configuration.CreateMap<MstCmmEngineModelDto, MstCmmEngineModel>();
            configuration.CreateMap<MstCmmEngineModel, MstCmmEngineModelDto>();
            configuration.CreateMap<CreateOrEditMstCmmEngineModelDto, MstCmmEngineModel>();

            // Master Common EngineType
            configuration.CreateMap<MstCmmEngineTypeDto, MstCmmEngineType>();
            configuration.CreateMap<MstCmmEngineType, MstCmmEngineTypeDto>();
            configuration.CreateMap<CreateOrEditMstCmmEngineTypeDto, MstCmmEngineType>();

            // Master Common Tax
            configuration.CreateMap<MstCmmTaxDto, MstCmmTax>();
            configuration.CreateMap<MstCmmTax, MstCmmTaxDto>();
            configuration.CreateMap<CreateOrEditMstCmmTaxDto, MstCmmTax>();

            //MASTER - logW
            configuration.CreateMap<MstLgwEciPartModuleDto, MstLgwEciPartModule>();
            configuration.CreateMap<MstLgwEciPartModule, MstLgwEciPartModuleDto>();

            //MASTER - LgwContDevanningLT
            configuration.CreateMap<MstLgwContDevanningLTDto, MstLgwContDevanningLT>();
            configuration.CreateMap<MstLgwContDevanningLT, MstLgwContDevanningLTDto>();
            configuration.CreateMap<CreateOrEditMstLgwContDevanningLTDto, MstLgwContDevanningLT>();

            //MASTER - LogWRenbanModule
            configuration.CreateMap<MstLgwRenbanModuleDto, MstLgwRenbanModule>();
            configuration.CreateMap<MstLgwRenbanModule, MstLgwRenbanModuleDto>();
            configuration.CreateMap<CreateOrEditMstLgwRenbanModuleDto, MstLgwRenbanModule>();

            //MASTER - LogWLayoutSetup
            configuration.CreateMap<MstLgwLayoutSetupDto, MstLgwLayoutSetup>();
            configuration.CreateMap<MstLgwLayoutSetup, MstLgwLayoutSetupDto>();
            configuration.CreateMap<CreateOrEditMstLgwLayoutSetupDto, MstLgwLayoutSetup>();

            //MASTER - LogWLayout
            configuration.CreateMap<MstLgwLayoutDto, MstLgwLayout>();
            configuration.CreateMap<MstLgwLayout, MstLgwLayoutDto>();
            //Master - LogWRobbingLane
            configuration.CreateMap<MstLgwRobbingLaneDto, MstLgwRobbingLane>();
            configuration.CreateMap<MstLgwRobbingLane, MstLgwRobbingLaneDto>();
            configuration.CreateMap<CreateOrEditMstLgwRobbingLaneDto, MstLgwRobbingLane>();
            //Master - LogWTabletProcess
            configuration.CreateMap<MstLgwPickingTabletProcessDto, MstLgwPickingTabletProcess>();
            configuration.CreateMap<MstLgwPickingTabletProcess, MstLgwPickingTabletProcessDto>();
            configuration.CreateMap<CreateOrEditMstLgwPickingTabletProcessDto, MstLgwPickingTabletProcess>();

            // //Master -lgwUnPackingPart
            configuration.CreateMap<MstLgwUnpackingPartDto, MstLgwUnpackingPart>();
            configuration.CreateMap<MstLgwUnpackingPart, MstLgwUnpackingPartDto>();
            configuration.CreateMap<CreateOrEditMstLgwUnpackingPartDto, MstLgwUnpackingPart>();

            // Master - InvCustomsPort
            configuration.CreateMap<MstInvCustomsPortDto, MstInvCustomsPort>();
            configuration.CreateMap<MstInvCustomsPort, MstInvCustomsPortDto>();


            //Master - InvShippingCompany
            configuration.CreateMap<MstInvShippingCompanyDto, MstInvShippingCompany>();
            configuration.CreateMap<MstInvShippingCompany, MstInvShippingCompanyDto>();

            //Master -Inventory
            // Master - Inv - Containerstatus
            configuration.CreateMap<MstInvContainerStatusDto, MstInvContainerStatus>();
            configuration.CreateMap<MstInvContainerStatus, MstInvContainerStatusDto>();

            // Master - Inv - ContainerdeliveryType
            configuration.CreateMap<MstInvContainerDeliveryTypeDto, MstInvContainerDeliveryType>();
            configuration.CreateMap<MstInvContainerDeliveryType, MstInvContainerDeliveryTypeDto>();

            // Master - Inv - Forwarder
            configuration.CreateMap<MstInvForwarderDto, MstInvForwarder>();
            configuration.CreateMap<MstInvForwarder, MstInvForwarderDto>();

            // Master - Inv - GpsSupplierInfo
            configuration.CreateMap<MstInvGpsSupplierInfoDto, MstInvGpsSupplierInfo>();
            configuration.CreateMap<MstInvGpsSupplierInfo, MstInvGpsSupplierInfoDto>();
            configuration.CreateMap<CreateOrEditMstInvGpsSupplierInfoDto, MstInvGpsSupplierInfo>();

            // Master - Inv - GpsSupplierOrderTime
            configuration.CreateMap<MstInvGpsSupplierOrderTimeDto, MstInvGpsSupplierOrderTime>();
            configuration.CreateMap<MstInvGpsSupplierOrderTime, MstInvGpsSupplierOrderTimeDto>();
            configuration.CreateMap<CreateOrEditMstInvGpsSupplierOrderTimeDto, MstInvGpsSupplierOrderTime>();

            // Master - Inv - MstInvGpsTmvPic
            configuration.CreateMap<MstInvGpsTmvPicDto, MstInvGpsTmvPic>();
            configuration.CreateMap<MstInvGpsTmvPic, MstInvGpsTmvPicDto>();
            configuration.CreateMap<CreateOrEditMstInvGpsTmvPicDto, MstInvGpsTmvPic>();

            //Master - InvCustomsStatus
            configuration.CreateMap<MstInvCustomsStatusDto, MstInvCustomsStatus>();
            configuration.CreateMap<MstInvCustomsStatus, MstInvCustomsStatusDto>();

            //Master-Inv-InvoiceStatus
            configuration.CreateMap<MstInvInvoiceStatusDto, MstInvInvoiceStatus>();
            configuration.CreateMap<MstInvInvoiceStatus, MstInvInvoiceStatusDto>();
            //Mst_Inv_PIO_Part_Type
            configuration.CreateMap<MstInvPIOPartTypeDto, MstInvPIOPartType>();
            configuration.CreateMap<MstInvPIOPartType, MstInvPIOPartTypeDto>();
            configuration.CreateMap<CreateOrEditMstInvPIOPartTypeDto, MstInvPIOPartType>();
            //MstPioImpSupplier
            configuration.CreateMap<MstPioImpSupplierDto, MstPioImpSupplier>();
            configuration.CreateMap<MstPioImpSupplier, MstPioImpSupplierDto>();
            configuration.CreateMap<CreateOrEditMstPioImpSupplierDto, MstPioImpSupplier>();

            //Master-Inv_PIO_Part_List
            configuration.CreateMap<InvPioPartListDto, InvPioPartList>();
            configuration.CreateMap<InvPioPartList, InvPioPartListDto>();
            configuration.CreateMap<CreateOrEditInvPioPartListDto, InvPioPartList>();

            //Master-Inv_GpsCalendar
            configuration.CreateMap<MstInvGpsCalendarDto, MstInvGpsCalendar>();
            configuration.CreateMap<MstInvGpsCalendar, MstInvGpsCalendarDto>();
            configuration.CreateMap<CreateOrEditMstInvGpsCalendarDto, MstInvGpsCalendar>();

            // Master - Inv -CustomsLeadTime
            configuration.CreateMap<MstInvCustomsLeadTimeDto, MstInvCustomsLeadTimeMaster>();
            configuration.CreateMap<MstInvCustomsLeadTimeMaster, MstInvCustomsLeadTimeDto>();
            configuration.CreateMap<CreateOrEditMstInvCustomsLeadTimeDto, MstInvCustomsLeadTimeMaster>();

            // Master - Inv -CkdRentalWarehouse
            configuration.CreateMap<MstInvCkdRentalWarehouseDto, MstInvCkdRentalWarehouse>();
            configuration.CreateMap<MstInvCkdRentalWarehouse, MstInvCkdRentalWarehouseDto>();
            configuration.CreateMap<CreateOrEditMstInvCkdRentalWarehouseDto, MstInvCkdRentalWarehouse>();


            //Master-Inv_GpsTruckSupplier
            configuration.CreateMap<MstInvGpsTruckSupplierDto, MstInvGpsTruckSupplier>();
            configuration.CreateMap<MstInvGpsTruckSupplier, MstInvGpsTruckSupplierDto>();
            configuration.CreateMap<CreateOrEditMstInvGpsTruckSupplierDto, MstInvGpsTruckSupplier>();

            //Master-Inv_GpsSupplierPicDto
            configuration.CreateMap<MstInvGpsSupplierPicDto, MstInvGpsSupplierPic>();
            configuration.CreateMap<MstInvGpsSupplierPic, MstInvGpsSupplierPicDto>();
            configuration.CreateMap<CreateOrEditMstInvGpsSupplierPicDto, MstInvGpsSupplierPic>();

            // Master-Inv-GpsScreenSetting
            configuration.CreateMap<MstInvGpsScreenSettingDto, MstInvGpsScreenSetting>();
            configuration.CreateMap<MstInvGpsScreenSetting, MstInvGpsScreenSettingDto>();
            configuration.CreateMap<CreateOrEditMstInvGpsScreenSettingDto, MstInvGpsScreenSetting>();

            // Master-Inv-GpsCategory
            configuration.CreateMap<MstInvGpsCategoryDto, MstInvGpsCategory>();
            configuration.CreateMap<MstInvGpsCategory, MstInvGpsCategoryDto>();
            configuration.CreateMap<CreateOrEditMstInvGpsCategoryDto, MstInvGpsCategory>();

            //DemDetFees
            configuration.CreateMap<MstInvDemDetFeesDto, MstInvDemDetFees>();
            configuration.CreateMap<MstInvDemDetFees, MstInvDemDetFeesDto>();
            configuration.CreateMap<CreateOrEditMstInvDemDetFeesDto, MstInvDemDetFees>();
            //
            configuration.CreateMap<MstInvGpsMaterialCategoryDto,MstInvGpsMaterialCategory>();
            configuration.CreateMap<MstInvGpsMaterialCategory, MstInvGpsMaterialCategoryDto>();
            configuration.CreateMap<CreateOrEditMstInvGpsMaterialCategoryDto, MstInvGpsMaterialCategory>();
			//MstGpsMaterialCategoryMapping
			configuration.CreateMap<MstGpsMaterialCategoryMappingDto, MstGpsMaterialCategoryMapping>();
			configuration.CreateMap<MstGpsMaterialCategoryMapping, MstGpsMaterialCategoryMappingDto>();
			configuration.CreateMap<CreateOrEditMstGpsMaterialCategoryMappingDto, MstGpsMaterialCategoryMapping>();

			// Master - MstGpsMaterialRegisterByShopDto
			configuration.CreateMap<MstGpsMaterialRegisterByShopDto, MstGpsMaterialRegisterByShop>();
            configuration.CreateMap<MstGpsMaterialRegisterByShop, MstGpsMaterialRegisterByShopDto>();
            configuration.CreateMap<CreateOrEditMstGpsMaterialRegisterByShopDto, MstGpsMaterialRegisterByShop>();

            //Master - MstGpsCostCenter
            configuration.CreateMap<MstGpsCostCenterDto, MstGpsCostCenterStructure>();
            configuration.CreateMap<MstGpsCostCenterStructure, MstGpsCostCenterDto>();
            configuration.CreateMap<CreateOrEditMstGpsCostCenterDto, MstGpsCostCenterStructure>();
            
            //Master - MstGpsUom
            configuration.CreateMap<MstGpsUomDto, MstGpsUom>();
            configuration.CreateMap<MstGpsUom, MstGpsUomDto>();
            configuration.CreateMap<CreateOrEditMstGpsUomDto, MstGpsUom>();

			//MstGpsWbsCCMapping
			configuration.CreateMap<MstGpsWbsCCMappingDto, MstGpsWbsCCMapping>();
			configuration.CreateMap<MstGpsWbsCCMapping, MstGpsWbsCCMappingDto>();
			configuration.CreateMap<CreateOrEditMstGpsWbsCCMappingDto, MstGpsWbsCCMapping>();


			// Master-Inv-CpsSuppliers
			configuration.CreateMap<MstInvCpsSuppliersDto, MstInvCpsSuppliers>();
            configuration.CreateMap<MstInvCpsSuppliers, MstInvCpsSuppliersDto>();


            //Master-Inv-CpsInventoryGroup
            configuration.CreateMap<MstInvCpsInventoryGroupDto, MstInvCpsInventoryGroup>();
            configuration.CreateMap<MstInvCpsInventoryGroup, MstInvCpsInventoryGroupDto>();

            //Master-Inv-InventoryItems
            configuration.CreateMap<MstInvCpsInventoryItemsDto, MstInvCpsInventoryItems>();
            configuration.CreateMap<MstInvCpsInventoryItems, MstInvCpsInventoryItemsDto>();

            //Master-Inv-PIOEmail
            configuration.CreateMap<MstInvPIOEmailDto, MstInvPIOEmail>();
            configuration.CreateMap<MstInvPIOEmail, MstInvPIOEmailDto>();
            configuration.CreateMap<CreateOrEditMstInvPIOEmailDto, MstInvPIOEmail>();


            // MstInvHrGlCodeCombination
            configuration.CreateMap<MstInvHrGlCodeCombinationDto, MstInvHrGlCodeCombination>();
            configuration.CreateMap<MstInvHrGlCodeCombination, MstInvHrGlCodeCombinationDto>();

            //MstInvLotPart
            configuration.CreateMap<MstInvLotPartDto, MstInvLotPart>();
            configuration.CreateMap<MstInvLotPart, MstInvLotPartDto>();

            //MstInvHrPosition
            configuration.CreateMap<MstInvHrPositionDto, MstInvHrPosition>();
            configuration.CreateMap<MstInvHrPosition, MstInvHrPositionDto>();


            //MstInvHrTitles
            configuration.CreateMap<MstInvHrTitlesDto, MstInvHrTitles>();
            configuration.CreateMap<MstInvHrTitles, MstInvHrTitlesDto>();

			//Master dem det days
			configuration.CreateMap<MstInvDemDetDaysDto, MstInvDemDetDays>();
            configuration.CreateMap<MstInvDemDetDays, MstInvDemDetDaysDto>();
			configuration.CreateMap<CreateOrEditMstInvDemDetDaysDto, MstInvDemDetDays>();

			//Mst Inv Devanning Case Type 
			configuration.CreateMap<MstInvDevanningCaseTypeDto, MstInvDevanningCaseType>();
			configuration.CreateMap<MstInvDevanningCaseType, MstInvDevanningCaseTypeDto>();
			configuration.CreateMap<CreateOrEditMstInvDevanningCaseTypeDto, MstInvDevanningCaseType>();

			//Master -Plm
			//Master -Plm - Matrix
			configuration.CreateMap<MasterPlmMatrixDto, MasterPlmMatrix>();
            configuration.CreateMap<MasterPlmMatrix, MasterPlmMatrixDto>();
            configuration.CreateMap<CreateOrEditMasterPlmMatrixDto, MasterPlmMatrix>();
            //Master -Plm - LotCodeGrade
            configuration.CreateMap<MstPlmLotCodeGradeDto, MstPlmLotCodeGrade>();
            configuration.CreateMap<MstPlmLotCodeGrade, MstPlmLotCodeGradeDto>();
            configuration.CreateMap<CreateOrEditMstPlmLotCodeGradeDto, MstPlmLotCodeGrade>();
            //Master -Plm - LotCode
            configuration.CreateMap<MasterPlmMatrixLotCodeDto, MasterPlmMatrixLotCode>();
            configuration.CreateMap<MasterPlmMatrixLotCode, MasterPlmMatrixLotCodeDto>();
            configuration.CreateMap<CreateOrEditMasterPlmMatrixLotCodeDto, MasterPlmMatrixLotCode>();

            //Master-Plm-Part
            configuration.CreateMap<MasterPlmPartDto, MasterPlmPart>();
            configuration.CreateMap<MasterPlmPart, MasterPlmPartDto>();
            configuration.CreateMap<CreateOrEditMasterPlmPartDto, MasterPlmPart>();

            //Master-Spp-Customer
            configuration.CreateMap<MstSppCustomerDto, MstSppCustomer>();
            configuration.CreateMap<MstSppCustomer, MstSppCustomerDto>();
            configuration.CreateMap<CreateOrEditMstSppCustomerDto, MstSppCustomer>();

            configuration.CreateMap<MstSppGlAccountDto, MstSppGlAccount>();
            configuration.CreateMap<MstSppGlAccount, MstSppGlAccountDto>();
            configuration.CreateMap<CreateOrEditMstSppGlAccountDto, MstSppGlAccount>();



            #endregion MASTER

            #region FRAME 
            //FRAME
            //FRAME - ANDON - FramePlan
            configuration.CreateMap<FrmAdoFramePlanDto, FrmAdoFramePlan>();
            configuration.CreateMap<FrmAdoFramePlan, FrmAdoFramePlanDto>();

            //FRAME - ANDON - FrameProgress
            configuration.CreateMap<FrmAdoFrameProgressDto, FrmAdoFrameProgress>();
            configuration.CreateMap<FrmAdoFrameProgress, FrmAdoFrameProgressDto>();
            //FRAME-ANDON-FrmAdoFramePlanA1
            configuration.CreateMap<FrmAdoFramePlanA1Dto, FrmAdoFramePlanA1>();
            configuration.CreateMap<FrmAdoFramePlanA1, FrmAdoFramePlanA1Dto>();
            configuration.CreateMap<CreateOrEditFrmAdoFramePlanA1Dto, FrmAdoFramePlanA1>();

            //FRAME-ANDON-FrmAdoFramePlanBMPV
            configuration.CreateMap<FrmAdoFramePlanBMPVDto, FrmAdoFramePlanBMPV>();
            configuration.CreateMap<FrmAdoFramePlanBMPV, FrmAdoFramePlanBMPVDto>();
            configuration.CreateMap<CreateOrEditFrmAdoFramePlanBMPVDto, FrmAdoFramePlanBMPV>();


            #endregion FRAME

            #region LogA
            //LogA - Lds
            configuration.CreateMap<LgaLdsLotPlanDto, LgaLdsLotPlan>();
            configuration.CreateMap<LgaLdsLotPlan, LgaLdsLotPlanDto>();
            configuration.CreateMap<CreateOrEditLgaLdsLotPlanDto, LgaLdsLotPlan>();

            //LogA - Bp2-progress
            configuration.CreateMap<LgaBp2ProgressDto, LgaBp2Progress>();
            configuration.CreateMap<LgaBp2Progress, LgaBp2ProgressDto>();

            configuration.CreateMap<MstLgaBp2EcarDto, MstLgaBp2Ecar>();
            configuration.CreateMap<MstLgaBp2Ecar, MstLgaBp2EcarDto>();
            configuration.CreateMap<CreateOrEditMstLgaBp2EcarDto, MstLgaBp2Ecar>();

            //LogA - Bp2 - PxPUpPlan
            configuration.CreateMap<LgaBp2PxPUpPlanDto, LgaBp2PxPUpPlan>();
            configuration.CreateMap<LgaBp2PxPUpPlan, LgaBp2PxPUpPlanDto>();

            //Lga - Bar - ScanInfo
            configuration.CreateMap<LgaBarScanInfoDto, LgaBarScanInfo>();
            configuration.CreateMap<LgaBarScanInfo, LgaBarScanInfoDto>();
            configuration.CreateMap<CreateOrEditLgaBarScanInfoDto, LgaBarScanInfo>();

            //Lga - Ekb - Progress
            configuration.CreateMap<LgaEkbProgressDto, LgaEkbProgress>();
            configuration.CreateMap<LgaEkbProgress, LgaEkbProgressDto>();
            configuration.CreateMap<CreateOrEditLgaEkbProgressDto, LgaEkbProgress>();

            //Lga Ekb ProgressDetails
            configuration.CreateMap<LgaEkbProgressDetailsDto, LgaEkbProgressDetails>();
            configuration.CreateMap<LgaEkbProgressDetails, LgaEkbProgressDetailsDto>();
            configuration.CreateMap<CreateOrEditLgaEkbProgressDetailsDto, LgaEkbProgressDetails>();

            //Lga - Ekb - Ekanban
            configuration.CreateMap<LgaEkbEkanbanDto, LgaEkbEkanban>();
            configuration.CreateMap<LgaEkbEkanban, LgaEkbEkanbanDto>();
            configuration.CreateMap<CreateOrEditLgaEkbEkanbanDto, LgaEkbEkanban>();

            //LogA - Pcs - Stock
            configuration.CreateMap<LgaPcsStockDto, LgaPcsStock>();
            configuration.CreateMap<LgaPcsStock, LgaPcsStockDto>();
            configuration.CreateMap<CreateOrEditLgaPcsStockDto, LgaPcsStock>();

            //LogA - Sps - Stock
            configuration.CreateMap<LgaSpsStockDto, LgaSpsStock>();
            configuration.CreateMap<LgaSpsStock, LgaSpsStockDto>();
            configuration.CreateMap<CreateOrEditLgaSpsStockDto, LgaSpsStock>();

            //LogA - Plc - Signal
            configuration.CreateMap<LgaPlcSignalDto, LgaPlcSignal>();
            configuration.CreateMap<LgaPlcSignal, LgaPlcSignalDto>();
            configuration.CreateMap<CreateOrEditLgaPlcSignalDto, LgaPlcSignal>();

            #endregion LogA

            #region LogW
            //Log - WMwh
            configuration.CreateMap<LgwMwhRobbingLaneDto, LgwMwhRobbingLane>();
            configuration.CreateMap<LgwMwhRobbingLane, LgwMwhRobbingLaneDto>();

            //logWmwh modulecalling
            configuration.CreateMap<LgwMwhModuleCallingDto, LgwMwhModuleCalling>();
            configuration.CreateMap<LgwMwhModuleCalling, LgwMwhModuleCallingDto>();

            //LogWAdo callinglightstatus
            configuration.CreateMap<LgwAdoCallingLightStatusDto, LgwAdoCallingLightStatus>();
            configuration.CreateMap<LgwAdoCallingLightStatus, LgwAdoCallingLightStatusDto>();
            configuration.CreateMap<CreateOrEditLgwAdoCallingLightStatusDto, LgwAdoCallingLightStatus>();

            //LogWCaseData
            configuration.CreateMap<LgwMwhCaseDataDto, LgwMwhCaseData>();
            configuration.CreateMap<LgwMwhCaseData, LgwMwhCaseDataDto>();

            //LogWMwh - ContList
            configuration.CreateMap<LgwMwhContListDto, LgwMwhContList>();
            configuration.CreateMap<LgwMwhContList, LgwMwhContListDto>();

            //LogWDvn-ContList
            configuration.CreateMap<LgwDvnContListDto, LgwDvnContList>();
            configuration.CreateMap<LgwDvnContList, LgwDvnContListDto>();
            configuration.CreateMap<CreateOrEditLgwDvnContListDto, LgwDvnContList>();

            //LogW - Lup - lotUpPlan
            configuration.CreateMap<LgwLupLotUpPlanDto, LgwLupLotUpPlan>();
            configuration.CreateMap<LgwLupLotUpPlan, LgwLupLotUpPlanDto>();
            configuration.CreateMap<CreateOrEditLgwLupLotUpPlanDto, LgwLupLotUpPlan>();

            //LogW - Lup - ContModule
            configuration.CreateMap<LgwLupContModuleDto, LgwLupContModule>();
            configuration.CreateMap<LgwLupContModule, LgwLupContModuleDto>();
            configuration.CreateMap<CreateOrEditLgwLupContModuleDto, LgwLupContModule>();

            //LogW - Pup - PxPUpPlan
            configuration.CreateMap<LgwPupPxPUpPlanDto, LgwPupPxPUpPlan>();
            configuration.CreateMap<LgwPupPxPUpPlan, LgwPupPxPUpPlanDto>();
            configuration.CreateMap<CreateOrEditLgwPupPxPUpPlanDto, LgwPupPxPUpPlan>();

            //LogW - Pup - PxPUpPlanBase
            configuration.CreateMap<LgwPupPxPUpPlanBaseDto, LgwPupPxPUpPlanBase>();
            configuration.CreateMap<LgwPupPxPUpPlanBase, LgwPupPxPUpPlanBaseDto>();
            configuration.CreateMap<CreateOrEditLgwPupPxPUpPlanBaseDto, LgwPupPxPUpPlanBase>();

            //Lgw - Pik - PickingProgress
            configuration.CreateMap<LgwPikPickingProgressDto, LgwPikPickingProgress>();
            configuration.CreateMap<LgwPikPickingProgress, LgwPikPickingProgressDto>();
            #endregion

            #region Inv
            //Inv_Stock
            configuration.CreateMap<InvStockDto, InvStock>();
            configuration.CreateMap<InvStock, InvStockDto>();

            //InvOutWipStock
            configuration.CreateMap<InvOutWipStockDto, InvOutWipStock>();
            configuration.CreateMap<InvOutWipStock, InvOutWipStockDto>();

            //Inv_OutLine
            configuration.CreateMap<InvOutLineOffDto, InvOutLineOff>();

            //InvImportByCont
            configuration.CreateMap<InvImportByContDto, InvImportByCont>();
            configuration.CreateMap<InvImportByCont, InvImportByContDto>();

            //InvProductionMapping
            configuration.CreateMap<InvProductionMappingDto, InvProductionMapping>();
            configuration.CreateMap<InvProductionMapping, InvProductionMappingDto>();



            //
            #endregion

            #region PAINTING

            // PAINTING ANDON - Scan Info
            configuration.CreateMap<PtsAdoScanInfoDto, PtsAdoScanInfo>();
            configuration.CreateMap<PtsAdoScanInfo, PtsAdoScanInfoDto>();
            configuration.CreateMap<CreateOrEditPtsAdoScanInfoDto, PtsAdoScanInfo>();

            // PAINTING ANDON - Painting Data
            configuration.CreateMap<PtsAdoPaintingDataDto, PtsAdoPaintingData>();
            configuration.CreateMap<PtsAdoPaintingData, PtsAdoPaintingDataDto>();

            // PAINTING ANDON - PAINTINGPROGRESS
            configuration.CreateMap<PtsAdoPaintingProgressDto, PtsAdoPaintingProgress>();
            configuration.CreateMap<PtsAdoPaintingProgress, PtsAdoPaintingProgressDto>();
            configuration.CreateMap<CreateOrEditPtsAdoPaintingProgressDto, PtsAdoPaintingProgress>();

            // PAINTING ANDON - LINEEFFICIENCY
            configuration.CreateMap<PtsAdoLineEfficiencyDto, PtsAdoLineEfficiency>();
            configuration.CreateMap<PtsAdoLineEfficiency, PtsAdoLineEfficiencyDto>();
            configuration.CreateMap<CreateOrEditPtsAdoLineEfficiencyDto, PtsAdoLineEfficiency>();



            // PAINTING ANDON - LINEEFFICIENCYDETAILS
            configuration.CreateMap<PtsAdoLineEfficiencyDetailsDto, PtsAdoLineEfficiencyDetails>();
            configuration.CreateMap<PtsAdoLineEfficiencyDetails, PtsAdoLineEfficiencyDetailsDto>();
            configuration.CreateMap<CreateOrEditPtsAdoLineEfficiencyDetailsDto, PtsAdoLineEfficiencyDetails>();

            //  PAINTING ANDON - TOTALDELAY
            configuration.CreateMap<PtsAdoTotalDelayDto, PtsAdoTotalDelay>();
            configuration.CreateMap<PtsAdoTotalDelay, PtsAdoTotalDelayDto>();
            configuration.CreateMap<CreateOrEditPtsAdoTotalDelayDto, PtsAdoTotalDelay>();

            //  PAINTING ANDON - Bumper
            configuration.CreateMap<PtsAdoBumperDto, PtsAdoBumper>();
            configuration.CreateMap<PtsAdoBumper, PtsAdoBumperDto>();
            configuration.CreateMap<CreateOrEditPtsAdoBumperDto, PtsAdoBumper>();

            /* // PAINTING ANDON -BumperGetdataClrIndicator
             configuration.CreateMap<PtsAdoBumperGetdataClrIndicator, PtsAdoBumperGetdataClrIndicator>();
             configuration.CreateMap<PtsAdoBumperGetdataClrIndicator, PtsAdoBumperGetdataClrIndicator>();*/
            #endregion

            #region Plan
            //Plan
            configuration.CreateMap<PlnCcrProductionPlanDto, PlnCcrProductionPlan>();
            configuration.CreateMap<PlnCcrProductionPlan, PlnCcrProductionPlanDto>();
            configuration.CreateMap<CreateOrEditPlnCcrProductionPlanDto, PlnCcrProductionPlan>();
            #endregion

            #region Welding
            // Welding - WeldingPlan
            configuration.CreateMap<WldAdoWeldingPlanDto, WldAdoWeldingPlan>();
            configuration.CreateMap<WldAdoWeldingPlan, WldAdoWeldingPlanDto>();
            configuration.CreateMap<CreateOrEditWldAdoWeldingPlanDto, WldAdoWeldingPlan>();

            // Welding - Weldingprogress
            configuration.CreateMap<WldAdoWeldingProgressDto, WldAdoWeldingProgress>();
            configuration.CreateMap<WldAdoWeldingProgress, WldAdoWeldingProgressDto>();
            configuration.CreateMap<CreateOrEditWldAdoWeldingProgressDto, WldAdoWeldingProgress>();

            // Welding - PunchQueue
            configuration.CreateMap<WldAdoPunchQueueDto, WldAdoPunchQueue>();
            configuration.CreateMap<WldAdoPunchQueue, WldAdoPunchQueueDto>();
            configuration.CreateMap<CreateOrEditWldAdoPunchQueueDto, WldAdoPunchQueue>();

            // Welding - WeldingSignal
            configuration.CreateMap<WldAdoWeldingSignalDto, WldAdoWeldingSignal>();
            configuration.CreateMap<WldAdoWeldingSignal, WldAdoWeldingSignalDto>();
            configuration.CreateMap<CreateOrEditWldAdoWeldingSignalDto, WldAdoWeldingSignal>();


            #endregion

                #region Inventory
            configuration.CreateMap<InvCkdBillDto, InvCkdBill>();
            configuration.CreateMap<InvCkdBill, InvCkdBillDto>();

            
            configuration.CreateMap<InvCkdShipmentDto, InvCkdShipment>();
            configuration.CreateMap<InvCkdShipment, InvCkdShipmentDto>();

            //Inventory - ContainerIntransit
            configuration.CreateMap<InvCkdContainerIntransitDto, InvCkdContainerIntransit>();
            configuration.CreateMap<InvCkdContainerIntransit, InvCkdContainerIntransitDto>();
			//configuration.CreateMap<CreateOrEditInvCkdContainerIntransitDto, InvCkdContainerIntransit>();

			//Inventory - CkdStockPart
			configuration.CreateMap<InvCkdStockPartDto, InvCkdStockPart>();
            configuration.CreateMap<InvCkdStockPart, InvCkdStockPartDto>();
			configuration.CreateMap<CreateOrEditInvCkdStockPartDto, InvCkdStockPart>();


			//Inventory - CkdContainerDeliveryGateIn
			configuration.CreateMap<InvCkdContainerDeliveryGateInDto, InvCkdContainerDeliveryGateIn>();
            configuration.CreateMap<InvCkdContainerDeliveryGateIn, InvCkdContainerDeliveryGateInDto>();

            //Inventory - CkdFrameEngine
            configuration.CreateMap<InvCkdFrameEngineDto, InvCkdFrameEngine>();
            configuration.CreateMap<InvCkdFrameEngine, InvCkdFrameEngineDto>();
            //configuration.CreateMap<CreateOrEditInvCkdFrameEngineDto, InvCkdFrameEngine>();
            configuration.CreateMap<CreateOrEditInvCkdShipmentDto, InvCkdShipment>();

            //Inventory -CkdContainerRentalWhPlan
            configuration.CreateMap<InvCkdContainerRentalWHPlanDto, InvCkdContainerRentalWHPlan>();
            configuration.CreateMap<InvCkdContainerRentalWHPlan, InvCkdContainerRentalWHPlanDto>();
            configuration.CreateMap<CreateOrEditInvCkdContainerRentalWHPlanDto, InvCkdContainerRentalWHPlan>();

            // Warehouse repack

            configuration.CreateMap<InvCkdContainerRentalWhRepackDto, InvCkdContainerRentalWhRepack>();
            configuration.CreateMap<InvCkdContainerRentalWhRepack, InvCkdContainerRentalWhRepackDto>();

            //
            configuration.CreateMap<InvCkdInvoiceDto, InvCkdInvoice>();
            configuration.CreateMap<InvCkdInvoice, InvCkdInvoiceDto>();

            configuration.CreateMap<InvCkdContainerTransitPortPlanDto, InvCkdContainerTransitPortPlan>();
            configuration.CreateMap<InvCkdContainerTransitPortPlan, InvCkdContainerTransitPortPlanDto>();
            configuration.CreateMap<CreateOrEditInvCkdContainerTransitPortPlanDto, InvCkdContainerTransitPortPlan>();

            configuration.CreateMap<InvCkdPreCustomsDto, InvCkdPreCustoms>();
            configuration.CreateMap<InvCkdPreCustoms, InvCkdPreCustomsDto>();

            configuration.CreateMap<MstCkdCustomsLeadtimeDto, MstCkdCustomsLeadtime>();
            configuration.CreateMap<MstCkdCustomsLeadtime, MstCkdCustomsLeadtimeDto>();
            configuration.CreateMap<CreateOrEditMstCkdCustomsLeadtimeDto, MstCkdCustomsLeadtime>();


            //Inventory - CKD - PaymentRequest
            configuration.CreateMap<InvCkdPaymentRequestDto, InvCkdPaymentRequest>();
            configuration.CreateMap<InvCkdPaymentRequest, InvCkdPaymentRequestDto>();

            //Inventory - CKD - PhysicalStockPart
            configuration.CreateMap<InvCkdPhysicalStockPartDto, InvCkdPhysicalStockPart>();
            configuration.CreateMap<InvCkdPhysicalStockPart, InvCkdPhysicalStockPartDto>();
            configuration.CreateMap<CreateOrEditInvCkdPhysicalStockPartDto, InvCkdPhysicalStockPart>();

            //Inventory - CKD - PhysicalConfirmLot
            configuration.CreateMap<InvCkdPhysicalConfirmLotDto, InvCkdPhysicalConfirmLot>();
            configuration.CreateMap<InvCkdPhysicalConfirmLot, InvCkdPhysicalConfirmLotDto>();


            configuration.CreateMap<InvGpsContentListDto, InvGpsContentList>();
            configuration.CreateMap<InvGpsContentList, InvGpsContentListDto>();
            configuration.CreateMap<CreateOrEditInvGpsContentListDto, InvGpsContentList>();

            configuration.CreateMap<InvGpsDailyOrderDto, InvGpsDailyOrder>();
            configuration.CreateMap<InvGpsDailyOrder, InvGpsDailyOrderDto>();
            configuration.CreateMap<CreateOrEditInvGpsDailyOrderDto, InvGpsDailyOrder>();

            configuration.CreateMap<InvGpsKanbanDto, InvGpsKanban>();
            configuration.CreateMap<InvGpsKanban, InvGpsKanbanDto>();
            configuration.CreateMap<CreateOrEditInvGpsKanbanDto, InvGpsKanban>();
            
            configuration.CreateMap<InvGpsStockDto, InvGpsStock>();
            configuration.CreateMap<InvGpsStock, InvGpsStockDto>();
            configuration.CreateMap<CreateOrEditInvGpsReceivingDto, InvGpsStock>();

            configuration.CreateMap<InvGpsUserDto, InvGpsUser>();
            configuration.CreateMap<InvGpsUser, InvGpsUserDto>();

            //Inventory - CKD - ProductionMapping
            configuration.CreateMap<InvCkdProductionMappingDto, InvCkdProductionMapping>();
            configuration.CreateMap<InvCkdProductionMapping, InvCkdProductionMappingDto>();
            configuration.CreateMap<CreateOrEditInvCkdProductionMappingDto, InvCkdProductionMapping>();

            //Inventory - CKD - InvCkdContainerInvoice
            configuration.CreateMap<InvCkdContainerInvoiceDto, InvCkdContainerInvoice>();
            configuration.CreateMap<InvCkdContainerInvoice, InvCkdContainerInvoiceDto>();
            configuration.CreateMap<CreateOrEditInvCkdContainerInvoiceDto, InvCkdContainerInvoice>();



            //InvCkdCustomsDeclare
            configuration.CreateMap<InvCkdCustomsDeclareDto, InvCkdCustomsDeclare>();
            configuration.CreateMap<InvCkdCustomsDeclare, InvCkdCustomsDeclareDto>();
            configuration.CreateMap<CreateOrEditInvCkdCustomsDeclareDto, InvCkdCustomsDeclare>();

            configuration.CreateMap<InvCkdPartRobbingDto, InvCkdPartRobbing>();
            configuration.CreateMap<InvCkdPartRobbing, InvCkdPartRobbingDto>();

            configuration.CreateMap<InvCkdPartRobbingDetailsDto, InvCkdPartRobbingDetails>();
            configuration.CreateMap<InvCkdPartRobbingDetails, InvCkdPartRobbingDetailsDto>();

            //InvCkdShippingScheduleFirm
            configuration.CreateMap<CreateOrEditInvCkdShippingScheduleDetailsFirmDto, InvCkdShippingScheduleDetailsFirm>();

            //InvCkdPhysicalStockPeriod
            configuration.CreateMap<InvCkdPhysicalStockPeriodDto, InvCkdPhysicalStockPeriod>();
            configuration.CreateMap<InvCkdPhysicalStockPeriod, InvCkdPhysicalStockPeriodDto>();
            configuration.CreateMap<CreateOrEditInvCkdPhysicalStockPeriodDto, InvCkdPhysicalStockPeriod>();

            //InvCkdPhysicalStockPartPeriod
            configuration.CreateMap<InvCkdPhysicalStockPartPeriodDto, InvCkdPhysicalStockPartPeriod>();
            configuration.CreateMap<InvCkdPhysicalStockPartPeriod, InvCkdPhysicalStockPartPeriodDto>();
            configuration.CreateMap<CreateOrEditInvCkdPhysicalStockPartPeriodDto, InvCkdPhysicalStockPartPeriod>();


            //InvCkdStockIssuing 

            configuration.CreateMap<InvCkdStockIssuingDto, InvCkdStockIssuing>();
            configuration.CreateMap<InvCkdStockIssuing, InvCkdStockIssuingDto>();
            configuration.CreateMap<CreateOrEditInvCkdStockIssuingDto, InvCkdStockIssuing>();

            //
            configuration.CreateMap<InvCkdProdPlanDailyDto, InvCkdProdPlanDaily>();
            configuration.CreateMap<InvCkdProdPlanDaily, InvCkdProdPlanDailyDto>();
            configuration.CreateMap<CreateOrEditInvCkdProdPlanDailyDto, InvCkdProdPlanDaily>();

            //
            configuration.CreateMap<InvCkdSmqdOrderLeadTimeDto, InvCkdSmqdOrderLeadTime>();
            configuration.CreateMap<InvCkdSmqdOrderLeadTime, InvCkdSmqdOrderLeadTimeDto>();

            //InvCkdSmqdOrder
            configuration.CreateMap<InvCkdSmqdOrderDto, InvCkdSmqdOrder>();
            configuration.CreateMap<InvCkdSmqdOrder, InvCkdSmqdOrderDto>();
            configuration.CreateMap<CreateOrEditInvCkdSmqdOrderDto, InvCkdSmqdOrder>();




            //containerList
            configuration.CreateMap<InvCkdContainerListDto, InvCkdContainerList>();
            configuration.CreateMap<InvCkdContainerList, InvCkdContainerListDto>();
            configuration.CreateMap<CreateOrEditInvCkdContainerListDto, InvCkdContainerList>();
            //stockpart
            configuration.CreateMap<InvCkdStockReceivingDto, InvCkdStockReceiving>();
            configuration.CreateMap<InvCkdStockReceiving, InvCkdStockReceivingDto>();
            configuration.CreateMap<CreateOrEditInvCkdStockReceivingDto, InvCkdStockReceiving>(); 

            //InvCkdModuleCase
            configuration.CreateMap<InvCkdModuleCaseDto, InvCkdModuleCase>();
            configuration.CreateMap<InvCkdModuleCase, InvCkdModuleCaseDto>();
            configuration.CreateMap<CreateOrEditInvCkdModuleCaseDto, InvCkdModuleCase>();

            //Inventory - CKD ProductionPLanMonthly
            configuration.CreateMap<InvCkdProductionPlanMonthlyDto, InvCkdProductionPlanMonthly>();
            configuration.CreateMap<InvCkdProductionPlanMonthly, InvCkdProductionPlanMonthlyDto>();

            //Inventory -CPSPoLines

            configuration.CreateMap<InvCpsPoLinesDto, InvCpsPoLines>();
            configuration.CreateMap<InvCpsPoLines, InvCpsPoLinesDto>();
            configuration.CreateMap<CreateOrEditInvCpsPoLinesDto, InvCpsPoLines>();

            configuration.CreateMap<InvCpsPoHeadersDto, InvCpsPoHeaders>();
            configuration.CreateMap<InvCpsPoHeaders, InvCpsPoHeadersDto>();

            configuration.CreateMap<InvCpsInvoiceHeadersDto, InvCpsInvoiceHeaders>();
            configuration.CreateMap<InvCpsInvoiceHeaders, InvCpsInvoiceHeadersDto>();

             configuration.CreateMap<InvCpsInventoryItemPriceDto, InvCpsInventoryItemPrice>();
            configuration.CreateMap<InvCpsInventoryItemPrice, InvCpsInventoryItemPriceDto>();

            configuration.CreateMap<InvCpsInvoiceLinesDto, InvCpsInvoiceLines>();
            configuration.CreateMap<InvCpsInvoiceLines, InvCpsInvoiceLinesDto>();

            // InvCpsRcvShipmentLines
            configuration.CreateMap<InvCpsRcvShipmentLinesDto, InvCpsRcvShipmentLines>();
            configuration.CreateMap<InvCpsRcvShipmentLines, InvCpsRcvShipmentLinesDto>();
            configuration.CreateMap<CreateOrEditInvCpsRcvShipmentLinesDto, InvCpsRcvShipmentLines>();

            //
            configuration.CreateMap<InvGpsStockConceptDto, InvGpsStockConcept>();
            configuration.CreateMap<InvGpsStockConcept, InvGpsStockConceptDto>();
            configuration.CreateMap<CreateOrEditInvGpsStockConceptDto, InvGpsStockConcept>();
            ////////////////////
            configuration.CreateMap<InvGpsMaterialDto, InvGpsMaterial>();
            configuration.CreateMap<InvGpsMaterial, InvGpsMaterialDto>();
            configuration.CreateMap<CreateOrEditInvGpsMaterialDto, InvGpsMaterial>();


            //Inventory - GPS - PartList
            configuration.CreateMap<InvGpsPartListDto, InvGpsPartList>();
            configuration.CreateMap<InvGpsPartList, InvGpsPartListDto>();
            //Inventory - GPS - InvGpsPartListByCategory
            configuration.CreateMap<InvGpsPartListByCategoryDto, InvGpsPartListByCategory>();
            configuration.CreateMap<InvGpsPartListByCategory, InvGpsPartListByCategoryDto>();
            configuration.CreateMap<CreateOrEditInvGpsPartListByCategoryDto, InvGpsPartListByCategory>();

            // Inv - Gps - StockRundown
            configuration.CreateMap<InvGpsStockRundownDto, InvGpsStockRundown>();
            configuration.CreateMap<InvGpsStockRundown, InvGpsStockRundownDto>();

            // Inv - Gps - StockRundownTransaction
            configuration.CreateMap<InvGpsStockRundownTransactionDto, InvGpsStockRundownTransaction>();
            configuration.CreateMap<InvGpsStockRundownTransaction, InvGpsStockRundownTransactionDto>();

            // Inv - Gps - GpsStock
            configuration.CreateMap<InvGpsStockDto, InvGpsStock>();
            configuration.CreateMap<InvGpsStock, InvGpsStockDto>();

            // Inv - Gps - StockReceiving
            configuration.CreateMap<InvGpsReceivingDto, InvGpsReceiving>();
            configuration.CreateMap<InvGpsReceiving, InvGpsReceivingDto>();
            configuration.CreateMap<CreateOrEditInvGpsReceivingDto, InvGpsReceiving>();

            // Inv - Gps - StockIssuing
            configuration.CreateMap<InvGpsIssuingDto, InvGpsIssuing>();
            configuration.CreateMap<InvGpsIssuing, InvGpsIssuingDto>();
            configuration.CreateMap<CreateOrEditInvGpsIssuingDto, InvGpsIssuing>();


            //Inventory - DMR - PartList
            configuration.CreateMap<InvDrmPartListDto, InvDrmPartList>();
            configuration.CreateMap<InvDrmPartList, InvDrmPartListDto>();
            configuration.CreateMap<CreateOrEditInvDrmPartListDto, InvDrmPartList>();

            //Inventory - DMR - StockPart
            configuration.CreateMap<InvDrmStockPartDto, InvDrmStockPart>();
            configuration.CreateMap<InvDrmStockPart, InvDrmStockPartDto>();

            //Inventory - IHP - PartList
            configuration.CreateMap<InvIhpPartListDto, InvIhpPartList>();
            configuration.CreateMap<InvIhpPartList, InvIhpPartListDto>();

            //
            configuration.CreateMap<InvIphMatCustomDeclareDto, InvIphMatCustomDeclare>();
            configuration.CreateMap<InvIphMatCustomDeclare, InvIphMatCustomDeclareDto>();
            //
            configuration.CreateMap<InvIphMatCustomDeclareDetailsDto, InvIphMatCustomDeclareDetails>();
            configuration.CreateMap<InvIphMatCustomDeclareDetails, InvIphMatCustomDeclareDetailsDto>();

            //Inventory - DMR - ImportPlan
            configuration.CreateMap<InvDrmImportPlanDto, InvDrmImportPlan>();
            configuration.CreateMap<InvDrmImportPlan, InvDrmImportPlanDto>();
            configuration.CreateMap<CreateOrEditInvDrmImportPlanDto, InvDrmImportPlan>();

            //InvTmssDispatchPlan
            configuration.CreateMap<InvTmssDispatchPlanDto, InvTmssDispatchPlan>();
            configuration.CreateMap<InvTmssDispatchPlan, InvTmssDispatchPlanDto>();

            //InvPIOStockReceiving
            configuration.CreateMap<InvPIOStockReceivingDto, InvPIOStockReceiving>();
            configuration.CreateMap<InvPIOStockReceiving, InvPIOStockReceivingDto>();

			//MstLspSupplierInfo
			configuration.CreateMap<MstLspSupplierInforDto, MstLspSupplierInfor>();
			configuration.CreateMap<MstLspSupplierInfor, MstLspSupplierInforDto>();
			configuration.CreateMap<CreateOrEditMstLspSupplierInforDto, MstLspSupplierInfor>();
			#endregion

			#region SPP
			configuration.CreateMap<InvSppCostOfSaleSummaryDto, InvSppCostOfSaleSummary>();
            configuration.CreateMap<InvSppCostOfSaleSummary, InvSppCostOfSaleSummaryDto>();
            configuration.CreateMap<InvSppCostDto, InvSppCost>();
            configuration.CreateMap<InvSppCost, InvSppCostDto>();
            configuration.CreateMap<InvSppInvoiceDetailsDto, InvSppInvoiceDetails>();
            configuration.CreateMap<InvSppInvoiceDetails, InvSppInvoiceDetailsDto>();
            configuration.CreateMap<InvSppShippingDto, InvSppShipping>();
            configuration.CreateMap<InvSppShipping, InvSppShippingDto>();
            configuration.CreateMap<InvSppStockDto, InvSppStock>();
            configuration.CreateMap<InvSppStock, InvSppStockDto>();
            #endregion


            #region SAPIF
            //Apr
            configuration.CreateMap<SapIFFundCommitmentHeaderDto, SapIFFundCommitmentHeader>();
            configuration.CreateMap<SapIFFundCommitmentHeaderDto, FundCommitmentRequestDocument>();
            configuration.CreateMap<SapIFFundCommitmentItem, FundCommitmentRequestDocumentItem>();
            configuration.CreateMap<FundCommitmentResponseDocument, SapIFLoggingResponseDetailsFundCommitment>();
            configuration.CreateMap<OnlineBudgetCheckAvailableBudget, SapIFLoggingResponseDetailsOnlineBudgetCheck>();
            configuration.CreateMap<OnlineBudgetCheckDataValidation, SapIFLoggingResponseDetailsOnlineBudgetCheck>();
            configuration.CreateMap<SapIFFundCommitmentItemDM, FundCommitmentRequestDocumentItem>();
            configuration.CreateMap<SapIFFundCommitmentHeaderDto, SapIFFundCommitmentHeaderDM>();


            #endregion
        }
    }
}
