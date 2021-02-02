using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.Extensions;

namespace StatCan.OrchardCore.Tabulator
{
    public class Migrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public Migrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            TableCreator();
            return 1;
        }

        private void TableCreator() {
            _contentDefinitionManager.AlterTypeDefinition("TableCreator", type => type
                .DisplayedAs("TableCreator")
                .Stereotype("Widget")
                .WithPart("TableCreator", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("TableCreator", part => part
                .WithField("TableData", field => field
                    .OfType("TextField")
                    .WithDisplayName("TableData")
                    .WithPosition("0")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Enter JSON for Table Data here",
                        Required = true,
                    })
                )
                .WithField("ColumnsData", field => field
                    .OfType("TextField")
                    .WithDisplayName("ColumnsData")
                    .WithPosition("1")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "To specify the column headers, enter the JSON for the columns here. If using the AutoColumnizer feature, enter an empty set of quotation marks. Eg. \"\".",
                        Required = true,
                    })
                )
                .WithField("AutoColumnizer", field => field
                    .OfType("TextField")
                    .WithDisplayName("AutoColumnizer")
                    .WithPosition("2")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "Enter \"true\" to automatically set the columns as per the TableData. Enter \"false\" to set the columns as per the json in the ColumnsData field.",
                    })
                )
                .WithField("PaginationSize", field => field
                    .OfType("NumericField")
                    .WithDisplayName("PaginationSize")
                    .WithPosition("2")
                    .WithSettings(new NumericFieldSettings
                    {
                        Hint = "Enter the number of rows to be displayed per page. For example, entering \"5\" will display 5 rows per page.",
                        Minimum = 1,
                        Maximum = 100,
                    })
                )
            );
            }




    }
}
