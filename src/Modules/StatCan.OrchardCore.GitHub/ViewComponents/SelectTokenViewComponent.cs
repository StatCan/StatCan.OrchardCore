using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StatCan.OrchardCore.GitHub.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.GitHub.ViewComponents
{
    public class SelectTokenViewComponent : ViewComponent
    {
        private readonly IGitHubApiService _apiService;

        public SelectTokenViewComponent(IGitHubApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string selectedTokenName, string htmlName)
        {
            var tokens = await _apiService.GetTokens();

            var model = new SelectTokenViewModel() { HtmlName = htmlName };

            foreach (var token in tokens)
            {
                model.Tokens.Add(new SelectListItem { Text = token.Name, Value = token.Name, Selected = token.Name == selectedTokenName });
            }

            return View(model);
        }
    }

    public class SelectTokenViewModel
    {
        public string HtmlName { get; set; }
        public IList<SelectListItem> Tokens { get; } = new List<SelectListItem>();
    }
}
