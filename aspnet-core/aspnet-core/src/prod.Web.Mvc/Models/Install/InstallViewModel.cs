﻿using System.Collections.Generic;
using Abp.Localization;
using prod.Install.Dto;

namespace prod.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
