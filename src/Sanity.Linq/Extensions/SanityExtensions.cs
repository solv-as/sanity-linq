﻿// Copywrite 2018 Oslofjord Operations AS

// This file is part of Sanity LINQ (https://github.com/oslofjord/sanity-linq).

//  Sanity LINQ is free software: you can redistribute it and/or modify
//  it under the terms of the MIT Licence.

//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//  MIT Licence for more details.

//  You should have received a copy of the MIT Licence
//  along with this program.

using Sanity.Linq.CommonTypes;
using System;
using System.Reflection;
using Sanity.Linq.Attributes;

namespace Sanity.Linq.Extensions
{
    public static class SanityExtensions
    {
        /// <summary>
        /// Used to retreived sanity type name from a class name by convention.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetSanityTypeName(this Type type)
        {
            switch (type.Name)
            {
                case nameof(SanityImageAsset):
                    return "sanity.imageAsset";

                case nameof(SanityFileAsset):
                    return "sanity.fileAsset";

                default:
                    // Get document name from attribute if set
                    var jsonPropertyAttribute = type.GetCustomAttribute<SanityDocumentName>();
                    if (jsonPropertyAttribute != null && !string.IsNullOrWhiteSpace(jsonPropertyAttribute.DocumentName))
                    {
                        return jsonPropertyAttribute.DocumentName;
                    }

                    // Remove Sanity and generic type marker from class name
                    var name = type.Name
                        .Replace("Sanity", "")
                        .Replace("`1", "");

                    // Make first letter lowercase (i.e., camelCase)
                    return name.ToCamelCase();
            }
        }
    }
}
