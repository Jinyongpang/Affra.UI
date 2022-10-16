﻿using JXNippon.CentralizedDatabaseSystem.Shared.ResourceFiles;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class StringDescriptionItem
    {
        [Parameter] public bool IsEditing { get; set; }

        [Parameter] public string Title { get; set; }

        [Parameter] public string Value { get; set; }

        [Parameter] public int? Span { get; set; }
        [Inject] private IStringLocalizer<Resource> stringLocalizer { get; set; }
    }
}
