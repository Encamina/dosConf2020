using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveSearch.Shared.Utils.Http
{
    public static class HttpRequestExtensions
    {
        private static readonly FormOptions defaultFormOptions = new FormOptions();

        public static async Task<FormValueProvider> StreamFile(this HttpRequest request, Stream targetStream)
        {
            ValidateMultipartRequest(request);

            // Used to accumulate all the form url encoded key value pairs in the 
            // request.
            var formAccumulator = new KeyValueAccumulator();

            var boundary = MultipartRequest.GetBoundary(MediaTypeHeaderValue.Parse(request.ContentType), defaultFormOptions.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, request.Body);

            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                var hasContentDispositionHeader = ContentDispositionHeaderValue
                                                        .TryParse(section.ContentDisposition, out ContentDispositionHeaderValue contentDisposition);

                if (hasContentDispositionHeader)
                {
                    if (MultipartRequest.HasFileContentDisposition(contentDisposition))
                    {
                        formAccumulator.Append("FileContentType", section.ContentType);
                        formAccumulator.Append("FileName", contentDisposition.FileName.Value);
                        await section.Body.CopyToAsync(targetStream);
                        targetStream.Seek(0, SeekOrigin.Begin);
                    }
                    else
                    {
                        if (MultipartRequest.HasFormDataContentDisposition(contentDisposition))
                        {
                            // Content-Disposition: form-data; name="key"
                            //
                            // value

                            // Do not limit the key name length here because the 
                            // multipart headers length limit is already in effect.
                            var key = HeaderUtilities.RemoveQuotes(contentDisposition.Name);
                            var encoding = GetEncoding(section);
                            using (var streamReader = new StreamReader(
                                section.Body,
                                encoding,
                                detectEncodingFromByteOrderMarks: true,
                                bufferSize: 1024,
                                leaveOpen: true))
                            {
                                // The value length limit is enforced by MultipartBodyLengthLimit
                                var value = await ReadValue(streamReader);
                                formAccumulator.Append(key.Value, value); // For .NET Core <2.0 remove ".Value" from key

                                CheckIfFormKeyLimitExceeded(formAccumulator);
                            }
                        }
                    }
                }

                // Drains any remaining section body that has not been consumed and
                // reads the headers for the next section.
                section = await reader.ReadNextSectionAsync();
            }

            // Bind form data to a model
            var formValueProvider = new FormValueProvider(
                BindingSource.Form,
                new FormCollection(formAccumulator.GetResults()),
                CultureInfo.CurrentCulture);

            return formValueProvider;
        }

        private static void ValidateMultipartRequest(HttpRequest request)
        {
            if (!MultipartRequest.IsMultipartContentType(request.ContentType))
            {
                throw new InvalidOperationException($"Expected a multipart request, but got {request.ContentType}");
            }
        }

        private static async Task<string> ReadValue(StreamReader streamReader)
        {
            var value = await streamReader.ReadToEndAsync();
            if (string.Equals(value, "undefined", StringComparison.OrdinalIgnoreCase))
            {
                value = string.Empty;
            }

            return value;
        }

        private static void CheckIfFormKeyLimitExceeded(KeyValueAccumulator formAccumulator)
        {
            if (formAccumulator.ValueCount > defaultFormOptions.ValueCountLimit)
            {
                throw new InvalidDataException($"Form key count limit {defaultFormOptions.ValueCountLimit} exceeded.");
            }
        }

        private static Encoding GetEncoding(MultipartSection section)
        {
            var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out MediaTypeHeaderValue mediaType);
            // UTF-7 is insecure and should not be honored. UTF-8 will succeed in 
            // most cases.
            if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
            {
                return Encoding.UTF8;
            }
            return mediaType.Encoding;
        }
    }
}
