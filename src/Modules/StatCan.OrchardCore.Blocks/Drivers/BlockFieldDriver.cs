using StatCan.OrchardCore.Blocks.Fields;
using StatCan.OrchardCore.Blocks.Parsers;
using StatCan.OrchardCore.Blocks.ViewModels;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Display.Models;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using System.Threading.Tasks;

namespace StatCan.OrchardCore.Blocks.Drivers
{
    public class BlockFieldDriver : ContentFieldDisplayDriver<BlockField>
    {
        #region Dependencies

        private readonly IBlocksParser _blocksParser;

        #endregion

        #region Constructor

        public BlockFieldDriver(IBlocksParser blocksParser)
        {
            _blocksParser = blocksParser;
        }

        #endregion

        public override async Task<IDisplayResult> DisplayAsync(BlockField field, BuildFieldDisplayContext fieldDisplayContext)
        {
            if (fieldDisplayContext.DisplayType != "Detail")
            {
                return null;
            }

            var blocks = await _blocksParser.RenderAsync(field);

            return Initialize<DisplayBlockFieldViewModel>(GetDisplayShapeType(fieldDisplayContext), model =>
            {
                model.Field = field;
                model.Part = fieldDisplayContext.ContentPart;
                model.PartFieldDefinition = fieldDisplayContext.PartFieldDefinition;
                model.Html = field.Html;
                model.Blocks = blocks;
            })
            .Location("Content");
        }

        public override IDisplayResult Edit(BlockField field, BuildFieldEditorContext context)
        {
            return Initialize<EditBlockFieldViewModel>(GetEditorShapeType(context), model =>
            {
                model.Field = field;
                model.Part = context.ContentPart;
                model.PartFieldDefinition = context.PartFieldDefinition;
                model.Data = field.Data;
            });
        }

        public override async Task<IDisplayResult> UpdateAsync(BlockField field, IUpdateModel updater, UpdateFieldEditorContext context)
        {
            await updater.TryUpdateModelAsync(field, Prefix, f => f.Data);

            return Edit(field, context);
        }
    }
}
