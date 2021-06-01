using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Scripting;
using OrchardCore.Media;
using System.IO;
using Microsoft.Extensions.Localization;
using OrchardCore.FileStorage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore;

namespace StatCan.OrchardCore.Scripting
{

    public class SaveMediaResult
    {
        public string name { get; set; }
        public long size { get; set; }
        public string folder { get; set; }
        public string mediaPath { get; set; }
        public bool hasError { get; set; } = false;
        public string errorMessage { get; set; }
    }
    public class MediaGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _saveMedia;

        public MediaGlobalMethodsProvider(IStringLocalizer<FormsGlobalMethodsProvider> S)
        {
             _saveMedia = new GlobalMethod
            {
                Name = "saveMedia",
                Method = serviceProvider => (Func<string, bool, SaveMediaResult[]>)((path, generateFileNames) => {
                    var mediaFileStore = serviceProvider.GetRequiredService<IMediaFileStore>();
                    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                    var mediaOptions = serviceProvider.GetRequiredService<IOptions<MediaOptions>>().Value;
                    var mediaNameNormalizerService = serviceProvider.GetRequiredService<IMediaNameNormalizerService>();

                    var files = httpContextAccessor.HttpContext.Request.Form.Files.ToList();

                    if (string.IsNullOrEmpty(path))
                    {
                        path = "";
                    }

                    var result = new List<SaveMediaResult>();

                    // Loop through each file in the request
                    foreach (var file in files)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        if (!mediaOptions.AllowedFileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                        {
                            result.Add(new SaveMediaResult()
                            {
                                name = file.FileName,
                                size = file.Length,
                                folder = path,
                                hasError = true,
                                errorMessage = S["This file extension is not allowed: {0}", extension].ToString()
                            });

                            continue;
                        }

                        var fileName = "";
                        if (generateFileNames)
                        {
                            fileName = IdGenerator.GenerateId() + extension;
                        }
                        else
                        {
                            fileName = mediaNameNormalizerService.NormalizeFileName(file.FileName);
                        }

                        Stream stream = null;
                        try
                        {
                            var mediaFilePath = mediaFileStore.Combine(path, fileName);
                            stream = file.OpenReadStream();
                            mediaFilePath = mediaFileStore.CreateFileFromStreamAsync(mediaFilePath, stream).GetAwaiter().GetResult();

                            var mediaFile = mediaFileStore.GetFileInfoAsync(mediaFilePath).GetAwaiter().GetResult();

                            result.Add(
                                new SaveMediaResult() {
                                    name = mediaFile.Name,
                                    size = mediaFile.Length,
                                    folder = mediaFile.DirectoryPath,
                                    mediaPath = mediaFile.Path,
                                }
                            );
                        }
                        catch (Exception ex)
                        {
                            result.Add(new SaveMediaResult()
                            {
                                name = fileName,
                                size = file.Length,
                                folder = path,
                                hasError = true,
                                // should be careful when returning these values to the client, they may have internal path / file disclosures
                                errorMessage = ex.Message
                            });
                        }
                        finally
                        {
                            stream?.Dispose();
                        }
                    }
                    return result.ToArray();
                })
            };
        }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _saveMedia };
        }
    }
}
