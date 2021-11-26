using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;

namespace StatCan.OrchardCore.Radar.Services
{
    public class BagItemManager
    {
        private readonly IContentManager _contentManager;

        public BagItemManager(IContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public async Task DeleteBagItemAsync(string bagName, string parentId, string id)
        {
            var parentContentItem = await _contentManager.GetAsync(parentId);

            if(parentContentItem == null)
            {
                throw new Exception("Parent Content Item does not exist");
            }

            var bag = parentContentItem.Get<BagPart>(bagName);

            bag.ContentItems = bag.ContentItems.Where(contentItem => contentItem.ContentItemId != id).ToList();

            parentContentItem.Apply(bagName, bag);

            await _contentManager.UpdateAsync(parentContentItem);
        }
    }
}
