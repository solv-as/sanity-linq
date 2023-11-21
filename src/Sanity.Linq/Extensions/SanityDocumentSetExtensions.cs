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

using Newtonsoft.Json;
using Sanity.Linq.CommonTypes;
using Sanity.Linq.DTOs;
using Sanity.Linq.Internal;
using Sanity.Linq.Mutations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sanity.Linq
{
    public static class SanityDocumentSetExtensions
    {
        /// <summary>
        /// Returns Sanity GROQ query for the expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetSanityQuery<T>(this IQueryable<T> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<T> dbSet && dbSet.Provider is SanityQueryProvider)
            {
                return ((SanityQueryProvider)dbSet.Provider).GetSanityQuery<T>(source.Expression);
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }

        public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<T> dbSet)
            {
                return (await dbSet.ExecuteAsync(cancellationToken).ConfigureAwait(false)).ToList();
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }

        public static async Task<T[]> ToArrayAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<T> dbSet)
            {
                return (await dbSet.ExecuteAsync(cancellationToken).ConfigureAwait(false)).ToArray();
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }

        public static async Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<T> dbSet)
            {
                return (await dbSet.Take(1).ExecuteSingleAsync(cancellationToken).ConfigureAwait(false));
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }

        public static async Task<IEnumerable<T>> ExecuteAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<T> dbSet)
            {
                return await dbSet.ExecuteAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }

        public static async Task<T> ExecuteSingleAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<T> dbSet)
            {
                return await dbSet.ExecuteSingleAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }

        public static async Task<int> CountAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<T> dbSet)
            {
                return (await dbSet.ExecuteCountAsync(cancellationToken).ConfigureAwait(false));
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }

        public static async Task<long> LongCountAsync<T>(this IQueryable<T> source, CancellationToken cancellationToken = default)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<T> dbSet)
            {
                return (await dbSet.ExecuteLongCountAsync(cancellationToken).ConfigureAwait(false));
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }


        public static IQueryable<TEntity> Include<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> property)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<TEntity> dbSet)
            {
                ((SanityDocumentSet<TEntity>)source).Include(property);
                return source;
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }

        public static IQueryable<TEntity> Include<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> property, string sourceName)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<TEntity> dbSet)
            {
                ((SanityDocumentSet<TEntity>)source).Include(property, sourceName);
                return source;
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }

        public static SanityMutationBuilder<TDoc> Patch<TDoc>(this IQueryable<TDoc> source, Action<SanityPatch> patch)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<TDoc> dbSet)
            {
                return dbSet.Mutations.PatchByQuery(dbSet.Expression, patch);
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }

        public static SanityMutationBuilder<TDoc> Delete<TDoc>(this IQueryable<TDoc> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is SanityDocumentSet<TDoc> dbSet)
            {
                return dbSet.Mutations.DeleteByQuery(dbSet.Expression);
            }
            else
            {
                throw new Exception("Queryable source must be a SanityDbSet<T>.");
            }
        }

        public static SanityMutationBuilder<TDoc> Create<TDoc>(this SanityDocumentSet<TDoc> docs, TDoc document)
        {
            return docs.Mutations.Create(document);
        }

        /// <summary>
        /// Sets only non-null values.
        /// </summary>
        /// <typeparam name="TDoc"></typeparam>
        /// <param name="docs"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public static SanityMutationBuilder<TDoc> SetValues<TDoc>(this SanityDocumentSet<TDoc> docs, TDoc document)
        {
            return docs.Mutations.SetValues(document);
        }

        public static SanityMutationBuilder<TDoc> Update<TDoc>(this SanityDocumentSet<TDoc> docs, TDoc document)
        {
            return docs.Mutations.Update(document);
        }

        public static SanityMutationBuilder<TDoc> DeleteById<TDoc>(this SanityDocumentSet<TDoc> docs, string id)
        {
            return docs.Mutations.DeleteById(id);
        }

        public static SanityMutationBuilder<TDoc> DeleteByQuery<TDoc>(this SanityDocumentSet<TDoc> docs, Expression<Func<TDoc,bool>> query)
        {
            return docs.Mutations.DeleteByQuery(query);
        }

        public static SanityMutationBuilder<TDoc> PatchById<TDoc>(this SanityDocumentSet<TDoc> docs, SanityPatchById<TDoc> patch)
        {
            return docs.Mutations.PatchById(patch);
        }

        //public static SanityTransactionBuilder<TDoc> PatchById<TDoc>(this SanityDocumentSet<TDoc> docs, string id, object patch)
        //{
        //    return docs.Mutations.PatchById(id, patch);
        //}

        public static SanityMutationBuilder<TDoc> PatchById<TDoc>(this SanityDocumentSet<TDoc> docs, string id, Action<SanityPatch> patch)
        {
            return docs.Mutations.PatchById(id, patch);
        }

        public static SanityMutationBuilder<TDoc> PatchByQuery<TDoc>(this SanityDocumentSet<TDoc> docs, SanityPatchByQuery<TDoc> patch)
        {
            return docs.Mutations.PatchByQuery(patch);
        }

        //public static SanityTransactionBuilder<TDoc> PatchByQuery<TDoc>(this SanityDocumentSet<TDoc> docs, Expression<Func<TDoc,bool>> query, object patch)
        //{
        //    return docs.Mutations.PatchByQuery(query, patch);
        //}

        public static SanityMutationBuilder<TDoc> PatchByQuery<TDoc>(this SanityDocumentSet<TDoc> docs, Expression<Func<TDoc, bool>> query, Action<SanityPatch> patch)
        {
            return docs.Mutations.PatchByQuery(query, patch);
        }

        public static void ClearChanges<TDoc>(this SanityDocumentSet<TDoc> docs)
        {
            docs.Mutations.Clear();
        }

        public static Task<SanityMutationResponse<TDoc>> CommitChangesAsync<TDoc>(this SanityDocumentSet<TDoc> docs, bool returnIds = false, bool returnDocuments = false, SanityMutationVisibility visibility = SanityMutationVisibility.Sync, CancellationToken cancellationToken = default)
        {
            return docs.Context.CommitAsync<TDoc>(returnIds, returnDocuments, visibility, cancellationToken);
        }


        public static Task<SanityDocumentResponse<SanityImageAsset>> UploadAsync(this SanityDocumentSet<SanityImageAsset> images, Stream stream, string filename, string contentType = null, string label = null, string title = null, string description = null, CancellationToken cancellationToken = default)
        {
            return images.Context.Client.UploadImageAsync(stream, filename, contentType: contentType, label: label, title: title, description: description, cancellationToken: cancellationToken);
        }

        public static Task<SanityDocumentResponse<SanityImageAsset>> UploadAsync(this SanityDocumentSet<SanityImageAsset> images, FileInfo file, string filename, string contentType = null, string label = null, CancellationToken cancellationToken = default)
        {
            return images.Context.Client.UploadImageAsync(file, label, cancellationToken);
        }
        public static Task<SanityDocumentResponse<SanityImageAsset>> UploadAsync(this SanityDocumentSet<SanityImageAsset> images, Uri uri, string label = null, string title = null, string description = null, CancellationToken cancellationToken = default)
        {
            return images.Context.Client.UploadImageAsync(uri, label, title, description, cancellationToken);
        }

        public static Task<SanityDocumentResponse<SanityFileAsset>> UploadAsync(this SanityDocumentSet<SanityFileAsset> images, Stream stream, string filename, string contentType = null, string label = null, string title = null, string description = null, CancellationToken cancellationToken = default)
        {
            return images.Context.Client.UploadFileAsync(stream, filename, contentType, label, title, description, cancellationToken);
        }

        public static Task<SanityDocumentResponse<SanityFileAsset>> UploadAsync(this SanityDocumentSet<SanityFileAsset> images, FileInfo file, string filename, string contentType = null, string label = null, string title = null, string description = null, CancellationToken cancellationToken = default)
        {
            return images.Context.Client.UploadFileAsync(file, label, title, description, cancellationToken);
        }
        public static Task<SanityDocumentResponse<SanityFileAsset>> UploadAsync(this SanityDocumentSet<SanityFileAsset> images, Uri uri, string label = null, string title = null, string description = null, CancellationToken cancellationToken = default)
        {
            return images.Context.Client.UploadFileAsync(uri, label, title, description, cancellationToken);
        }
    }
}
