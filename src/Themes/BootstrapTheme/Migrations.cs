using System.Threading.Tasks;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Recipes.Services;
using StatCan.OrchardCore.Extensions;

namespace BootstrapTheme
{
    public class Migrations : DataMigration
    {
        private readonly IRecipeMigrator _recipeMigrator;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        public Migrations(IRecipeMigrator recipeMigrator, IContentDefinitionManager contentDefinitionManager)
        {
            _recipeMigrator = recipeMigrator;
            _contentDefinitionManager = contentDefinitionManager;
        }

        public async Task<int> CreateAsync()
        {
            // feature recipe is executed as a seperate step due to the
            // recipeMigrator not flushing the database session between steps.
            // Any new feature would not be enabled prior to the additional steps running.
            await _recipeMigrator.ExecuteAsync("features.recipe.json", this);
            await _recipeMigrator.ExecuteAsync("initial.recipe.json", this);
            await _recipeMigrator.ExecuteAsync("placements.recipe.json", this);
            Col();
            Row();
            Container();
            return 2;
        }

        public async Task<int> UpdateFrom1Async()
        {
            Col();
            Row();
            Container();
            await _recipeMigrator.ExecuteAsync("placements.recipe.json", this);
            return 2;
        }

        private void Col()
        {
            _contentDefinitionManager.AlterTypeDefinition("Col", type => type
                .DisplayedAs("Col")
                .Stereotype("Widget")
                .WithPart("Col", part => part
                    .WithPosition("0")
                )
                .WithFlow("1")
            );

            _contentDefinitionManager.AlterPartDefinition("Col", part => part
                .WithField("Classes", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Classes")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "col", Value = "col", Default = true},
                            new MultiTextFieldValueOption() {Name = "col-sm", Value = "col-sm"},
                            new MultiTextFieldValueOption() {Name = "col-md", Value = "col-md"},
                            new MultiTextFieldValueOption() {Name = "col-lg", Value = "col-lg"},
                            new MultiTextFieldValueOption() {Name = "col-1", Value = "col-1"},
                            new MultiTextFieldValueOption() {Name = "col-2", Value = "col-2"},
                            new MultiTextFieldValueOption() {Name = "col-3", Value = "col-3"},
                            new MultiTextFieldValueOption() {Name = "col-4", Value = "col-4"},
                            new MultiTextFieldValueOption() {Name = "col-5", Value = "col-5"},
                            new MultiTextFieldValueOption() {Name = "col-6", Value = "col-6"},
                            new MultiTextFieldValueOption() {Name = "col-7", Value = "col-7"},
                            new MultiTextFieldValueOption() {Name = "col-8", Value = "col-8"},
                            new MultiTextFieldValueOption() {Name = "col-9", Value = "col-9"},
                            new MultiTextFieldValueOption() {Name = "col-10", Value = "col-10"},
                            new MultiTextFieldValueOption() {Name = "col-11", Value = "col-11"},
                            new MultiTextFieldValueOption() {Name = "col-12", Value = "col-12"},
                            new MultiTextFieldValueOption() {Name = "col-sm-1", Value = "col-sm-1"},
                            new MultiTextFieldValueOption() {Name = "col-sm-2", Value = "col-sm-2"},
                            new MultiTextFieldValueOption() {Name = "col-sm-3", Value = "col-sm-3"},
                            new MultiTextFieldValueOption() {Name = "col-sm-4", Value = "col-sm-4"},
                            new MultiTextFieldValueOption() {Name = "col-sm-5", Value = "col-sm-5"},
                            new MultiTextFieldValueOption() {Name = "col-sm-6", Value = "col-sm-6"},
                            new MultiTextFieldValueOption() {Name = "col-sm-7", Value = "col-sm-7"},
                            new MultiTextFieldValueOption() {Name = "col-sm-8", Value = "col-sm-8"},
                            new MultiTextFieldValueOption() {Name = "col-sm-9", Value = "col-sm-9"},
                            new MultiTextFieldValueOption() {Name = "col-sm-10", Value = "col-sm-10"},
                            new MultiTextFieldValueOption() {Name = "col-sm-11", Value = "col-sm-11"},
                            new MultiTextFieldValueOption() {Name = "col-sm-12", Value = "col-sm-12"},
                            new MultiTextFieldValueOption() {Name = "col-md-1", Value = "col-md-1"},
                            new MultiTextFieldValueOption() {Name = "col-md-2", Value = "col-md-2"},
                            new MultiTextFieldValueOption() {Name = "col-md-3", Value = "col-md-3"},
                            new MultiTextFieldValueOption() {Name = "col-md-4", Value = "col-md-4"},
                            new MultiTextFieldValueOption() {Name = "col-md-5", Value = "col-md-5"},
                            new MultiTextFieldValueOption() {Name = "col-md-6", Value = "col-md-6"},
                            new MultiTextFieldValueOption() {Name = "col-md-7", Value = "col-md-7"},
                            new MultiTextFieldValueOption() {Name = "col-md-8", Value = "col-md-8"},
                            new MultiTextFieldValueOption() {Name = "col-md-9", Value = "col-md-9"},
                            new MultiTextFieldValueOption() {Name = "col-md-10", Value = "col-md-10"},
                            new MultiTextFieldValueOption() {Name = "col-md-11", Value = "col-md-11"},
                            new MultiTextFieldValueOption() {Name = "col-md-12", Value = "col-md-12"},
                            new MultiTextFieldValueOption() {Name = "col-lg-1", Value = "col-lg-1"},
                            new MultiTextFieldValueOption() {Name = "col-lg-2", Value = "col-lg-2"},
                            new MultiTextFieldValueOption() {Name = "col-lg-3", Value = "col-lg-3"},
                            new MultiTextFieldValueOption() {Name = "col-lg-4", Value = "col-lg-4"},
                            new MultiTextFieldValueOption() {Name = "col-lg-5", Value = "col-lg-5"},
                            new MultiTextFieldValueOption() {Name = "col-lg-6", Value = "col-lg-6"},
                            new MultiTextFieldValueOption() {Name = "col-lg-7", Value = "col-lg-7"},
                            new MultiTextFieldValueOption() {Name = "col-lg-8", Value = "col-lg-8"},
                            new MultiTextFieldValueOption() {Name = "col-lg-9", Value = "col-lg-9"},
                            new MultiTextFieldValueOption() {Name = "col-lg-10", Value = "col-lg-10"},
                            new MultiTextFieldValueOption() {Name = "col-lg-11", Value = "col-lg-11"},
                            new MultiTextFieldValueOption() {Name = "col-lg-12", Value = "col-lg-12"},
                            new MultiTextFieldValueOption() {Name = "col-xl-1", Value = "col-xl-1"},
                            new MultiTextFieldValueOption() {Name = "col-xl-2", Value = "col-xl-2"},
                            new MultiTextFieldValueOption() {Name = "col-xl-3", Value = "col-xl-3"},
                            new MultiTextFieldValueOption() {Name = "col-xl-4", Value = "col-xl-4"},
                            new MultiTextFieldValueOption() {Name = "col-xl-5", Value = "col-xl-5"},
                            new MultiTextFieldValueOption() {Name = "col-xl-6", Value = "col-xl-6"},
                            new MultiTextFieldValueOption() {Name = "col-xl-7", Value = "col-xl-7"},
                            new MultiTextFieldValueOption() {Name = "col-xl-8", Value = "col-xl-8"},
                            new MultiTextFieldValueOption() {Name = "col-xl-9", Value = "col-xl-9"},
                            new MultiTextFieldValueOption() {Name = "col-xl-10", Value = "col-xl-10"},
                            new MultiTextFieldValueOption() {Name = "col-xl-11", Value = "col-xl-11"},
                            new MultiTextFieldValueOption() {Name = "col-xl-12", Value = "col-xl-12"},
                            //offset
                            new MultiTextFieldValueOption() {Name = "offset-1", Value = "offset-1"},
                            new MultiTextFieldValueOption() {Name = "offset-2", Value = "offset-2"},
                            new MultiTextFieldValueOption() {Name = "offset-3", Value = "offset-3"},
                            new MultiTextFieldValueOption() {Name = "offset-4", Value = "offset-4"},
                            new MultiTextFieldValueOption() {Name = "offset-5", Value = "offset-5"},
                            new MultiTextFieldValueOption() {Name = "offset-6", Value = "offset-6"},
                            new MultiTextFieldValueOption() {Name = "offset-7", Value = "offset-7"},
                            new MultiTextFieldValueOption() {Name = "offset-8", Value = "offset-8"},
                            new MultiTextFieldValueOption() {Name = "offset-9", Value = "offset-9"},
                            new MultiTextFieldValueOption() {Name = "offset-10", Value = "offset-10"},
                            new MultiTextFieldValueOption() {Name = "offset-11", Value = "offset-11"},
                            new MultiTextFieldValueOption() {Name = "offset-12", Value = "offset-12"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-1", Value = "offset-sm-1"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-2", Value = "offset-sm-2"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-3", Value = "offset-sm-3"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-4", Value = "offset-sm-4"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-5", Value = "offset-sm-5"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-6", Value = "offset-sm-6"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-7", Value = "offset-sm-7"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-8", Value = "offset-sm-8"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-9", Value = "offset-sm-9"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-10", Value = "offset-sm-10"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-11", Value = "offset-sm-11"},
                            new MultiTextFieldValueOption() {Name = "offset-sm-12", Value = "offset-sm-12"},
                            new MultiTextFieldValueOption() {Name = "offset-md-1", Value = "offset-md-1"},
                            new MultiTextFieldValueOption() {Name = "offset-md-2", Value = "offset-md-2"},
                            new MultiTextFieldValueOption() {Name = "offset-md-3", Value = "offset-md-3"},
                            new MultiTextFieldValueOption() {Name = "offset-md-4", Value = "offset-md-4"},
                            new MultiTextFieldValueOption() {Name = "offset-md-5", Value = "offset-md-5"},
                            new MultiTextFieldValueOption() {Name = "offset-md-6", Value = "offset-md-6"},
                            new MultiTextFieldValueOption() {Name = "offset-md-7", Value = "offset-md-7"},
                            new MultiTextFieldValueOption() {Name = "offset-md-8", Value = "offset-md-8"},
                            new MultiTextFieldValueOption() {Name = "offset-md-9", Value = "offset-md-9"},
                            new MultiTextFieldValueOption() {Name = "offset-md-10", Value = "offset-md-10"},
                            new MultiTextFieldValueOption() {Name = "offset-md-11", Value = "offset-md-11"},
                            new MultiTextFieldValueOption() {Name = "offset-md-12", Value = "offset-md-12"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-1", Value = "offset-lg-1"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-2", Value = "offset-lg-2"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-3", Value = "offset-lg-3"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-4", Value = "offset-lg-4"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-5", Value = "offset-lg-5"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-6", Value = "offset-lg-6"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-7", Value = "offset-lg-7"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-8", Value = "offset-lg-8"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-9", Value = "offset-lg-9"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-10", Value = "offset-lg-10"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-11", Value = "offset-lg-11"},
                            new MultiTextFieldValueOption() {Name = "offset-lg-12", Value = "offset-lg-12"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-1", Value = "offset-xl-1"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-2", Value = "offset-xl-2"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-3", Value = "offset-xl-3"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-4", Value = "offset-xl-4"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-5", Value = "offset-xl-5"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-6", Value = "offset-xl-6"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-7", Value = "offset-xl-7"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-8", Value = "offset-xl-8"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-9", Value = "offset-xl-9"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-10", Value = "offset-xl-10"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-11", Value = "offset-xl-11"},
                            new MultiTextFieldValueOption() {Name = "offset-xl-12", Value = "offset-xl-12"},
                            //alignment
                            new MultiTextFieldValueOption() {Name = "align-self-start", Value = "align-self-start"},
                            new MultiTextFieldValueOption() {Name = "align-self-center", Value = "align-self-center"},
                            new MultiTextFieldValueOption() {Name = "align-self-end", Value = "align-self-end"},

                        },
                    })
                )
            );
        }

        private void Row()
        {
            _contentDefinitionManager.AlterTypeDefinition("Row", type => type
                .DisplayedAs("Row")
                .Stereotype("Widget")
                .WithPart("Row", part => part
                    .WithPosition("0")
                )
                .WithFlow("1", new[] { "Col" })
            );

            _contentDefinitionManager.AlterPartDefinition("Row", part => part
                .WithField("Classes", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Classes")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "row-cols-1", Value = "row-cols-1"},
                            new MultiTextFieldValueOption() {Name = "row-cols-2", Value = "row-cols-2"},
                            new MultiTextFieldValueOption() {Name = "row-cols-3", Value = "row-cols-3"},
                            new MultiTextFieldValueOption() {Name = "row-cols-4", Value = "row-cols-4"},
                            new MultiTextFieldValueOption() {Name = "row-cols-5", Value = "row-cols-5"},
                            new MultiTextFieldValueOption() {Name = "row-cols-6", Value = "row-cols-6"},
                            new MultiTextFieldValueOption() {Name = "row-cols-7", Value = "row-cols-7"},
                            new MultiTextFieldValueOption() {Name = "row-cols-8", Value = "row-cols-8"},
                            new MultiTextFieldValueOption() {Name = "row-cols-9", Value = "row-cols-9"},
                            new MultiTextFieldValueOption() {Name = "row-cols-10", Value = "row-cols-10"},
                            new MultiTextFieldValueOption() {Name = "row-cols-11", Value = "row-cols-11"},
                            new MultiTextFieldValueOption() {Name = "row-cols-12", Value = "row-cols-12"},

                            new MultiTextFieldValueOption() {Name = "row-cols-sm-1", Value = "row-cols-sm-1"},
                            new MultiTextFieldValueOption() {Name = "row-cols-sm-2", Value = "row-cols-sm-2"},
                            new MultiTextFieldValueOption() {Name = "row-cols-sm-3", Value = "row-cols-sm-3"},
                            new MultiTextFieldValueOption() {Name = "row-cols-sm-4", Value = "row-cols-sm-4"},
                            new MultiTextFieldValueOption() {Name = "row-cols-sm-5", Value = "row-cols-sm-5"},
                            new MultiTextFieldValueOption() {Name = "row-cols-sm-6", Value = "row-cols-sm-6"},
                            new MultiTextFieldValueOption() {Name = "row-cols-sm-7", Value = "row-cols-sm-7"},
                            new MultiTextFieldValueOption() {Name = "row-cols-sm-8", Value = "row-cols-sm-8"},
                            new MultiTextFieldValueOption() {Name = "row-cols-sm-9", Value = "row-cols-sm-9"},
                            new MultiTextFieldValueOption() {Name = "row-cols-sm-10", Value = "row-cols-sm-10"},
                            new MultiTextFieldValueOption() {Name = "row-cols-sm-11", Value = "row-cols-sm-11"},
                            new MultiTextFieldValueOption() {Name = "row-cols-sm-12", Value = "row-cols-sm-12"},

                            new MultiTextFieldValueOption() {Name = "row-cols-md-1", Value = "row-cols-md-1"},
                            new MultiTextFieldValueOption() {Name = "row-cols-md-2", Value = "row-cols-md-2"},
                            new MultiTextFieldValueOption() {Name = "row-cols-md-3", Value = "row-cols-md-3"},
                            new MultiTextFieldValueOption() {Name = "row-cols-md-4", Value = "row-cols-md-4"},
                            new MultiTextFieldValueOption() {Name = "row-cols-md-5", Value = "row-cols-md-5"},
                            new MultiTextFieldValueOption() {Name = "row-cols-md-6", Value = "row-cols-md-6"},
                            new MultiTextFieldValueOption() {Name = "row-cols-md-7", Value = "row-cols-md-7"},
                            new MultiTextFieldValueOption() {Name = "row-cols-md-8", Value = "row-cols-md-8"},
                            new MultiTextFieldValueOption() {Name = "row-cols-md-9", Value = "row-cols-md-9"},
                            new MultiTextFieldValueOption() {Name = "row-cols-md-10", Value = "row-cols-md-10"},
                            new MultiTextFieldValueOption() {Name = "row-cols-md-11", Value = "row-cols-md-11"},
                            new MultiTextFieldValueOption() {Name = "row-cols-md-12", Value = "row-cols-md-12"},

                            new MultiTextFieldValueOption() {Name = "row-cols-lg-1", Value = "row-cols-lg-1"},
                            new MultiTextFieldValueOption() {Name = "row-cols-lg-2", Value = "row-cols-lg-2"},
                            new MultiTextFieldValueOption() {Name = "row-cols-lg-3", Value = "row-cols-lg-3"},
                            new MultiTextFieldValueOption() {Name = "row-cols-lg-4", Value = "row-cols-lg-4"},
                            new MultiTextFieldValueOption() {Name = "row-cols-lg-5", Value = "row-cols-lg-5"},
                            new MultiTextFieldValueOption() {Name = "row-cols-lg-6", Value = "row-cols-lg-6"},
                            new MultiTextFieldValueOption() {Name = "row-cols-lg-7", Value = "row-cols-lg-7"},
                            new MultiTextFieldValueOption() {Name = "row-cols-lg-8", Value = "row-cols-lg-8"},
                            new MultiTextFieldValueOption() {Name = "row-cols-lg-9", Value = "row-cols-lg-9"},
                            new MultiTextFieldValueOption() {Name = "row-cols-lg-10", Value = "row-cols-lg-10"},
                            new MultiTextFieldValueOption() {Name = "row-cols-lg-11", Value = "row-cols-lg-11"},
                            new MultiTextFieldValueOption() {Name = "row-cols-lg-12", Value = "row-cols-lg-12"},

                            new MultiTextFieldValueOption() {Name = "row-cols-xl-1", Value = "row-cols-xl-1"},
                            new MultiTextFieldValueOption() {Name = "row-cols-xl-2", Value = "row-cols-xl-2"},
                            new MultiTextFieldValueOption() {Name = "row-cols-xl-3", Value = "row-cols-xl-3"},
                            new MultiTextFieldValueOption() {Name = "row-cols-xl-4", Value = "row-cols-xl-4"},
                            new MultiTextFieldValueOption() {Name = "row-cols-xl-5", Value = "row-cols-xl-5"},
                            new MultiTextFieldValueOption() {Name = "row-cols-xl-6", Value = "row-cols-xl-6"},
                            new MultiTextFieldValueOption() {Name = "row-cols-xl-7", Value = "row-cols-xl-7"},
                            new MultiTextFieldValueOption() {Name = "row-cols-xl-8", Value = "row-cols-xl-8"},
                            new MultiTextFieldValueOption() {Name = "row-cols-xl-9", Value = "row-cols-xl-9"},
                            new MultiTextFieldValueOption() {Name = "row-cols-xl-10", Value = "row-cols-xl-10"},
                            new MultiTextFieldValueOption() {Name = "row-cols-xl-11", Value = "row-cols-xl-11"},
                            new MultiTextFieldValueOption() {Name = "row-cols-xl-12", Value = "row-cols-xl-12"},

                            new MultiTextFieldValueOption() {Name = "align-items-star", Value = "align-items-star"},
                            new MultiTextFieldValueOption() {Name = "align-items-center", Value = "align-items-center"},
                            new MultiTextFieldValueOption() {Name = "align-items-end", Value = "align-items-end"},

                            new MultiTextFieldValueOption() {Name = "justify-content-start", Value = "justify-content-start"},
                            new MultiTextFieldValueOption() {Name = "justify-content-center", Value = "justify-content-center"},
                            new MultiTextFieldValueOption() {Name = "justify-content-end", Value = "justify-content-end"},
                            new MultiTextFieldValueOption() {Name = "justify-content-around", Value = "justify-content-around"},
                            new MultiTextFieldValueOption() {Name = "justify-content-between", Value = "justify-content-between"},

                            new MultiTextFieldValueOption() {Name = "no-gutters", Value = "no-gutters"},
                        },
                    })
                )
            );
        }

        private void Container()
        {
            _contentDefinitionManager.AlterTypeDefinition("Container", type => type
                .DisplayedAs("Container")
                .Stereotype("Widget")
                .WithPart("Container", part => part
                    .WithPosition("0")
                )
                .WithFlow("1")
            );

            _contentDefinitionManager.AlterPartDefinition("Container", part => part
                .WithField("Classes", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Classes")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "container", Value = "container", Default=true},
                            new MultiTextFieldValueOption() {Name = "container-fluid", Value = "container-fluid"},
                        }
                    })
                )
            );
        }


    }
}
