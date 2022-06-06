﻿using System.Reflection;
using JXNippon.CentralizedDatabaseSystem.Domain.Interfaces;
using ViewODataService.Affra.Service.View.Domain.Views;

namespace JXNippon.CentralizedDatabaseSystem.Domain.Views
{
    public static class ViewHelper
    {
        private static IDictionary<string, string> TypeMappings;
        public static IDictionary<string, string> GetTypeMapping()
        {
            Assembly assembly = typeof(View).Assembly;
            return TypeMappings ??= assembly.GetTypes()
                .DistinctBy(t => t.Name)
                .Where(t => t.GetInterface(nameof(IDaily)) != null)
                .ToDictionary(x => x.Name, x => x.AssemblyQualifiedName);
        }
    }
}
