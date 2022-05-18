﻿using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class GenericDeleteDialog
    {
        [Inject] private DialogService DialogService { get; set; }
        private void ConfirmClicked() => DialogService.Close(true);
        private void CancelClicked() => DialogService.Close(false);
    }
}