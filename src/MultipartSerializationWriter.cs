// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;

namespace Microsoft.Kiota.Serialization.Multipart;
/// <summary>
/// Serialization writer for multipart payloads.
/// </summary>
public class MultipartSerializationWriter : ISerializationWriter
{
    private readonly MemoryStream _stream = new MemoryStream();
    /// <inheritdoc/>
    public Action<IParsable>? OnBeforeObjectSerialization { get; set; }
    /// <inheritdoc/>
    public Action<IParsable>? OnAfterObjectSerialization { get; set; }
    /// <inheritdoc/>
    public Action<IParsable, ISerializationWriter>? OnStartObjectSerialization { get; set; }
    private readonly StreamWriter writer;
    /// <summary>
    /// Instantiates a new multipart serialization writer.
    /// </summary>
    public MultipartSerializationWriter()
    {
        writer = new StreamWriter(_stream);
    }
    /// <inheritdoc/>
    public void Dispose()
    {
        writer.Dispose();
        _stream.Dispose();
        GC.SuppressFinalize(this);
    }
    /// <inheritdoc/>
    public Stream GetSerializedContent()
    {
        writer.Flush();
        _stream.Position = 0;
        return _stream;
    }
    /// <inheritdoc/>
    public void WriteAdditionalData(IDictionary<string, object> value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteBoolValue(string? key, bool? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteByteArrayValue(string? key, byte[]? value)
    {
        if(value != null && value.Length > 0)
        {
            _stream.Write(value, 0, value.Length);
        }
    }
    /// <inheritdoc/>
    public void WriteByteValue(string? key, byte? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteCollectionOfEnumValues<T>(string? key, IEnumerable<T?>? values) where T : struct, Enum => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteCollectionOfObjectValues<T>(string? key, IEnumerable<T>? values) where T : IParsable => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteCollectionOfPrimitiveValues<T>(string? key, IEnumerable<T>? values) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteDateTimeOffsetValue(string? key, DateTimeOffset? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteDateValue(string? key, Date? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteDecimalValue(string? key, decimal? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteDoubleValue(string? key, double? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteEnumValue<T>(string? key, T? value) where T : struct, Enum => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteFloatValue(string? key, float? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteGuidValue(string? key, Guid? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteIntValue(string? key, int? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteLongValue(string? key, long? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteNullValue(string? key) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteObjectValue<T>(string? key, T? value, params IParsable?[] additionalValuesToMerge) where T : IParsable
    {
        if(value is MultiPartBody)
        {
            OnBeforeObjectSerialization?.Invoke(value);
            OnStartObjectSerialization?.Invoke(value, this);
            value.Serialize(this);
            OnAfterObjectSerialization?.Invoke(value);
        }
        else
            throw new InvalidOperationException($"Expected a MultiPartBody instance, but got {value?.GetType().Name ?? "null"}");
    }
    /// <inheritdoc/>
    public void WriteSbyteValue(string? key, sbyte? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteStringValue(string? key, string? value)
    {
        if(!string.IsNullOrEmpty(key))
            writer.Write(key);
        if(!string.IsNullOrEmpty(value))
        {
            if(!string.IsNullOrEmpty(key))
                writer.Write(": ");
            writer.Write(value);
        }
        writer.WriteLine();
    }
    /// <inheritdoc/>
    public void WriteTimeSpanValue(string? key, TimeSpan? value) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteTimeValue(string? key, Time? value) => throw new NotImplementedException();
}