using OrchardCore.DisplayManagement.Descriptors;

namespace Lombiq.HelpfulExtensions.Extensions.Flows
{
    internal class FlowPartShapeTableProvider : IShapeTableProvider
    {
        public void Discover(ShapeTableBuilder builder)
        {
            builder.Describe("FlowPart")
                .OnDisplaying(displaying => displaying.Shape.Metadata.Alternates.Add("Lombiq_HelpfulExtensions_Flows_FlowPart"));
        }
    }
}
