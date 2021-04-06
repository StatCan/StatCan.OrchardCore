using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Records;
using OrchardCore.Deployment.Services;
using OrchardCore.Mvc.Utilities;
using OrchardCore.Scripting;
using YesSql;

namespace StatCan.OrchardCore.Scripting
{
    public enum RecipeJsonResult
    {
        Success=0,
        InvalidJson=1,
        Exception=2,
    }
    public class RecipeGlobalMethodsProvider : IGlobalMethodProvider
    {
        private readonly GlobalMethod _importRecipeJson;

        public RecipeGlobalMethodsProvider(ILogger<RecipeGlobalMethodsProvider> logger)
        {
            _importRecipeJson = new GlobalMethod
            {
                Name = "importRecipeJson",
                Method = serviceProvider => (Func<string, RecipeJsonResult>)((json) =>
                {
                    var deploymentManager = serviceProvider.GetRequiredService<IDeploymentManager>();

                    if (!json.IsJson())
                    {
                        return RecipeJsonResult.InvalidJson;
                    }

                    var tempArchiveFolder = PathExtensions.Combine(Path.GetTempPath(), Path.GetRandomFileName());

                    try
                    {
                        Directory.CreateDirectory(tempArchiveFolder);
                        System.IO.File.WriteAllText(Path.Combine(tempArchiveFolder, "Recipe.json"), json);

                        deploymentManager.ImportDeploymentPackageAsync(new PhysicalFileProvider(tempArchiveFolder)).GetAwaiter().GetResult();

                       return RecipeJsonResult.Success;
                    }
                    catch(Exception ex)
                    {
                        logger.LogError(ex, "Error occurred whilst running the importRecipeJson() global method");
                        return RecipeJsonResult.Exception;
                    }
                    finally
                    {
                        if (Directory.Exists(tempArchiveFolder))
                        {
                            Directory.Delete(tempArchiveFolder, true);
                        }
                    }
                }
                )
                };
            }

        public IEnumerable<GlobalMethod> GetMethods()
        {
            return new[] { _importRecipeJson };
        }
    }
}
