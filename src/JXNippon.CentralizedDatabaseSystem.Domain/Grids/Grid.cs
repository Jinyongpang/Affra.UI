﻿using System.Text.Json.Serialization;
using JXNippon.CentralizedDatabaseSystem.Domain.Views;
using Microsoft.OData.Client;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Grids
{
    public class Grid
    {
        public string Type { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        public ICollection<GridColumn> GridColumns { get; set; }

        [IgnoreClientProperty]
        [JsonIgnore]
        public Type ActualType
        {
            get
            {
                return System.Type.GetType(ViewHelper.GetTypeMapping()[this.Type]);
            }
            set
            {
                this.Type = value.Name;
            }
        }
    }
}