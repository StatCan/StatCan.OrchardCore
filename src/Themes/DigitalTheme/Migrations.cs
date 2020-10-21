using OrchardCore.Autoroute.Models;
using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Contents.Models;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using OrchardCore.Title.Models;
using StatCan.OrchardCore.ContentFields.PredefinedGroup.Settings;
using ListValueOption = StatCan.OrchardCore.ContentFields.PredefinedGroup.Settings.ListValueOption;

namespace StatCan.Themes.DigitalTheme
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
            ShowcaseBlurb();
            MenuItems();
            FatFooter();
            Hero();
            Page();
            Section();
            SimpleContent();
            NavBar();
            HtmlBlurb();
            PageHeader();
            return 1;
        }

        private void ShowcaseBlurb()
        {
            _contentDefinitionManager.AlterTypeDefinition("ShowcaseBlurb", type => type
                .DisplayedAs("Showcase Blurb")
                .Stereotype("Widget")
                .WithSettings(new FullTextAspectSettings
                {
                    IncludeFullTextTemplate = true,
                    FullTextTemplate = @"{{Model.Content.ShowcaseBlurb.PrimaryText.Text}}
 {{Model.Content.ShowcaseBlurb.SecondaryText.Text}}"
                })
                .WithPart("ShowcaseBlurb", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("ShowcaseBlurb", part => part
                .WithField("Image", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Image")
                    .WithPosition("0")
                )
                .WithField("PrimaryText", field => field
                    .OfType("TextField")
                    .WithDisplayName("Primary Text")
                    .WithPosition("1")
                )
                .WithField("SecondaryText", field => field
                    .OfType("TextField")
                    .WithDisplayName("Secondary Text")
                    .WithPosition("2")
                )
            );
        }
        private void MenuItems()
        {
            _contentDefinitionManager.AlterTypeDefinition("LoginMenuItem", type => type
                .DisplayedAs("Login Menu Item")
                .Stereotype("MenuItem")
                .WithPart("LoginMenuItem", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("FeedbackMenuItem", type => type
                .DisplayedAs("Feedback Menu Item")
                .Stereotype("MenuItem")
                .WithPart("FeedbackMenuItem", part => part
                    .WithPosition("0")
                )
            );
            _contentDefinitionManager.AlterPartDefinition("FeedbackMenuItem", part => part
                .WithField("FormEndpoint", field => field
                    .OfType("TextField")
                    .WithDisplayName("Form Endpoint")
                )
            );

            _contentDefinitionManager.AlterTypeDefinition("CultureMenuItem", type => type
                .DisplayedAs("Culture Menu Item")
                .Stereotype("MenuItem")
                .WithPart("CultureMenuItem", part => part
                    .WithPosition("0")
                )
            );
        }
        private void FatFooter()
        {
            _contentDefinitionManager.AlterTypeDefinition("FatFooter", type => type
                .DisplayedAs("Fat Footer")
                .Stereotype("Widget")
                .WithPart("FatFooter", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("FatFooter", part => part
                .WithField("HubLink", field => field
                    .OfType("LinkField")
                    .WithDisplayName("Hub Link")
                    .WithPosition("0")
                )
                .WithField("DigitalLink", field => field
                    .OfType("LinkField")
                    .WithDisplayName("Digital Link")
                    .WithPosition("1")
                )
                .WithField("MenuAlias", field => field
                    .OfType("TextField")
                    .WithDisplayName("Menu Alias")
                    .WithPosition("2")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "(optional) The alias of the menu to render if it's present"
                    })
                )
            );
        }
        private void Hero()
        {
            _contentDefinitionManager.AlterTypeDefinition("Hero", type => type
                .DisplayedAs("Hero")
                .Stereotype("Widget")
                .WithPart("Hero", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                )
            );
        }
        private void Page()
        {
            _contentDefinitionManager.AlterTypeDefinition("Page", type => type
                .DisplayedAs("Page")
                .Creatable()
                .Listable()
                .Draftable()
                .Versionable()
                .Securable()
                .WithPart("Page", part => part
                    .WithPosition("2")
                )
                .WithPart("LocalizationPart", part => part
                    .WithPosition("3")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                )
                .WithPart("AutoroutePart", part => part
                    .WithPosition("1")
                    .WithSettings(new AutoroutePartSettings
                    {
                        AllowCustomPath = true,
                        ShowHomepageOption = true
                    })
                )
                .WithPart("FlowPart", part => part
                    .WithPosition("4")
                )
            );
        }
        private void Section()
        {
            _contentDefinitionManager.AlterTypeDefinition("Section", type => type
                .DisplayedAs("Section")
                .Stereotype("Widget")
                .WithPart("Section", part => part
                    .WithPosition("0")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("1")
                )
                .WithPart("FlowPart", part => part
                    .WithPosition("3")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("Section", part => part
                .WithField("Section", field => field
                    .OfType("TextField")
                    .WithDisplayName("Section")
                    .WithEditor("PredefinedGroup")
                    .WithPosition("0")
                    .WithSettings(new TextFieldPredefinedGroupEditorSettings
                    {
                        DefaultValue = "container",
                        Options = new ListValueOption[] { new ListValueOption() {
                    Name= "<svg width=\"61.44\" height=\"36\" viewBox=\"0 0 1024 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n                <g transform=\"matrix(2.69463,0,0,1.0616,-39.5648,-16.0489)\">\n                    <path d=\"M394.697,72.987C394.697,41.048 384.482,15.118 371.899,15.118L37.481,15.118C24.899,15.118 14.683,41.048 14.683,72.987L14.683,522.431C14.683,554.37 24.899,580.3 37.481,580.3L371.899,580.3C384.482,580.3 394.697,554.37 394.697,522.431L394.697,72.987Z\" fill=\"#D0DEF2\"></path>\n                </g>\n                <g transform=\"matrix(1.64204,0,0,0.920057,175.89,26.091)\">\n                    <path d=\"M394.697,72.987C394.697,41.048 380.168,15.118 362.273,15.118L47.108,15.118C29.212,15.118 14.683,41.048 14.683,72.987L14.683,522.431C14.683,554.37 29.212,580.3 47.108,580.3L362.273,580.3C380.168,580.3 394.697,554.37 394.697,522.431L394.697,72.987Z\" fill=\"#8FBEE7\"></path>\n                </g>\n            </svg>",
                    Value= "container"
        }, new ListValueOption()  {
        Name = "<svg width=\"61.44\" height=\"36\" viewBox=\"0 0 1024 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n                <g transform=\"matrix(2.69463,0,0,1.0616,-39.5648,-16.0489)\">\n                    <path d=\"M394.697,72.987C394.697,41.048 384.482,15.118 371.899,15.118L37.481,15.118C24.899,15.118 14.683,41.048 14.683,72.987L14.683,522.431C14.683,554.37 24.899,580.3 37.481,580.3L371.899,580.3C384.482,580.3 394.697,554.37 394.697,522.431L394.697,72.987Z\" fill=\"#D0DEF2\"></path>\n                </g>\n                <g transform=\"matrix(2.56306,0,0,0.920057,-12.633,26.091)\">\n                    <path d=\"M394.697,72.987C394.697,41.048 385.389,15.118 373.924,15.118L35.456,15.118C23.991,15.118 14.683,41.048 14.683,72.987L14.683,522.431C14.683,554.37 23.991,580.3 35.456,580.3L373.924,580.3C385.389,580.3 394.697,554.37 394.697,522.431L394.697,72.987Z\" fill=\"#8FBEE7\"></path>\n                </g>\n            </svg>",
        Value = "container-fluid"
        } }
                    })
                )
                .WithField("ColumnLayout", field => field
                    .OfType("TextField")
                    .WithDisplayName("Column Layout")
                    .WithEditor("PredefinedGroup")
                    .WithPosition("2")
                    .WithSettings(new TextFieldPredefinedGroupEditorSettings
                    {
                        DefaultValue = "half-half",
                        Options = new[] { new ListValueOption()  {
        Name = "<svg width=\"48\" height=\"36\" viewBox=\"0 0 800 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n                <g transform=\"matrix(2.10518,0,0,1.0616,-30.91,-16.0489)\">\n                    <path d=\"M394.697,41.105C394.697,26.762 388.825,15.118 381.592,15.118L27.788,15.118C20.555,15.118 14.683,26.762 14.683,41.105L14.683,554.313C14.683,568.656 20.555,580.3 27.788,580.3L381.592,580.3C388.825,580.3 394.697,568.656 394.697,554.313L394.697,41.105Z\" fill=\"#8FBEE7\"></path>\n                </g>\n            </svg>",
        Value = "full"
        }, new ListValueOption()  {
        Name = "<svg width=\"48\" height=\"36\" viewBox=\"0 0 800 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n                <g transform=\"matrix(0.999962,0,0,1.0616,-14.6823,-16.0489)\">\n                    <path d=\"M394.697,31.576C394.697,22.492 386.868,15.118 377.224,15.118L32.156,15.118C22.512,15.118 14.683,22.492 14.683,31.576L14.683,563.841C14.683,572.925 22.512,580.3 32.156,580.3L377.224,580.3C386.868,580.3 394.697,572.925 394.697,563.841L394.697,31.576Z\" fill=\"#8FBEE7\"></path>\n                </g>\n                <g transform=\"matrix(0.999962,0,0,1.0616,405.318,-16.0489)\">\n                    <path d=\"M394.697,31.576C394.697,22.492 386.868,15.118 377.224,15.118L32.156,15.118C22.512,15.118 14.683,22.492 14.683,31.576L14.683,563.841C14.683,572.925 22.512,580.3 32.156,580.3L377.224,580.3C386.868,580.3 394.697,572.925 394.697,563.841L394.697,31.576Z\" fill=\"#8FBEE7\"></path>\n                </g>\n            </svg>",
        Value = "half-half"
        }, new ListValueOption()  {
        Name = "<svg width=\"48\" height=\"36\" viewBox=\"0 0 800 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n                <g transform=\"matrix(0.631555,0,0,1.0616,-9.27301,-16.0489)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n                <g transform=\"matrix(0.631555,0,0,1.0616,270.727,-16.0489)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n                <g transform=\"matrix(0.631555,0,0,1.0616,550.727,-16.0489)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n            </svg>",
        Value = "third-third-third"
        }, new ListValueOption()  {
        Name = "<svg width=\"48\" height=\"36\" viewBox=\"0 0 800 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n                <g transform=\"matrix(0.447351,0,0,1.0616,-6.56838,-16.0489)\">\n                    <path d=\"M394.697,31.131C394.697,22.293 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,22.293 14.683,31.131L14.683,564.287C14.683,573.125 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,573.125 394.697,564.287L394.697,31.131Z\" fill=\"#8FBEE7\"></path>\n                </g>\n                <g transform=\"matrix(0.447351,0,0,1.0616,203.432,-16.0489)\">\n                    <path d=\"M394.697,31.131C394.697,22.293 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,22.293 14.683,31.131L14.683,564.287C14.683,573.125 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,573.125 394.697,564.287L394.697,31.131Z\" fill=\"#8FBEE7\"></path>\n                </g>\n                <g transform=\"matrix(0.447351,0,0,1.0616,413.432,-16.0489)\">\n                    <path d=\"M394.697,31.131C394.697,22.293 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,22.293 14.683,31.131L14.683,564.287C14.683,573.125 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,573.125 394.697,564.287L394.697,31.131Z\" fill=\"#8FBEE7\"></path>\n                </g>\n                <g transform=\"matrix(0.447351,0,0,1.0616,623.432,-16.0489)\">\n                    <path d=\"M394.697,31.131C394.697,22.293 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,22.293 14.683,31.131L14.683,564.287C14.683,573.125 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,573.125 394.697,564.287L394.697,31.131Z\" fill=\"#8FBEE7\"></path>\n                </g>\n            </svg>",
        Value = "quarter-quarter-quarter-quarter"
        } }
                    })
                )
                .WithField("JustifyContent", field => field
                    .OfType("TextField")
                    .WithDisplayName("Justify Content")
                    .WithEditor("PredefinedGroup")
                    .WithPosition("3")
                    .WithSettings(new TextFieldPredefinedGroupEditorSettings
                    {
                        DefaultValue = "justify-content-start",
                        Options = new[] { new ListValueOption()  {
        Name = "<svg width=\"48\" height=\"36\" viewBox=\"0 0 800 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n                <g transform=\"matrix(2.10518,0,0,1.0616,-30.91,-16.0489)\">\n                    <path d=\"M394.697,43.377C394.697,27.78 388.312,15.118 380.447,15.118L28.933,15.118C21.068,15.118 14.683,27.78 14.683,43.377L14.683,552.041C14.683,567.638 21.068,580.3 28.933,580.3L380.447,580.3C388.312,580.3 394.697,567.638 394.697,552.041L394.697,43.377Z\" fill=\"#D0DEF2\"></path>\n                </g>\n                <g transform=\"matrix(0.578925,0,0,0.973137,16.4997,10.2885)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n                <g transform=\"matrix(0.578925,0,0,0.973137,261.5,10.2885)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n            </svg>",
        Value = "justify-content-start"
        }, new ListValueOption()  {
        Name = "<svg width=\"48\" height=\"36\" viewBox=\"0 0 800 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n                <g transform=\"matrix(2.10518,0,0,1.0616,-30.91,-16.0489)\">\n                    <path d=\"M394.697,43.377C394.697,27.78 388.312,15.118 380.447,15.118L28.933,15.118C21.068,15.118 14.683,27.78 14.683,43.377L14.683,552.041C14.683,567.638 21.068,580.3 28.933,580.3L380.447,580.3C388.312,580.3 394.697,567.638 394.697,552.041L394.697,43.377Z\" fill=\"#D0DEF2\"></path>\n                </g>\n                <g transform=\"matrix(0.578925,0,0,0.973137,546.5,10.2885)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n                <g transform=\"matrix(0.578925,0,0,0.973137,301.5,10.2885)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n            </svg>",
        Value = "justify-content-end"
        }, new ListValueOption()  {
        Name = "<svg width=\"48\" height=\"36\" viewBox=\"0 0 800 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n                <g transform=\"matrix(2.10518,0,0,1.0616,-30.91,-16.0489)\">\n                    <path d=\"M394.697,43.377C394.697,27.78 388.312,15.118 380.447,15.118L28.933,15.118C21.068,15.118 14.683,27.78 14.683,43.377L14.683,552.041C14.683,567.638 21.068,580.3 28.933,580.3L380.447,580.3C388.312,580.3 394.697,567.638 394.697,552.041L394.697,43.377Z\" fill=\"#D0DEF2\"></path>\n                </g>\n                <g transform=\"matrix(0.578925,0,0,0.973137,404,10.2885)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n                <g transform=\"matrix(0.578925,0,0,0.973137,159,10.2885)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n            </svg>",
        Value = "justify-content-center"
        }, new ListValueOption()  {
        Name = "<svg width=\"48\" height=\"36\" viewBox=\"0 0 800 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n                <g transform=\"matrix(2.10518,0,0,1.0616,-30.91,-16.0489)\">\n                    <path d=\"M394.697,43.377C394.697,27.78 388.312,15.118 380.447,15.118L28.933,15.118C21.068,15.118 14.683,27.78 14.683,43.377L14.683,552.041C14.683,567.638 21.068,580.3 28.933,580.3L380.447,580.3C388.312,580.3 394.697,567.638 394.697,552.041L394.697,43.377Z\" fill=\"#D0DEF2\"></path>\n                </g>\n                <g transform=\"matrix(0.578925,0,0,0.973137,546.5,10.2885)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n                <g transform=\"matrix(0.578925,0,0,0.973137,16.4997,10.2885)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n            </svg>",
        Value = "justify-content-between"
        }, new ListValueOption()  {
        Name = "<svg width=\"48\" height=\"36\" viewBox=\"0 0 800 600\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n                <g transform=\"matrix(2.10518,0,0,1.0616,-30.91,-16.0489)\">\n                    <path d=\"M394.697,43.377C394.697,27.78 388.312,15.118 380.447,15.118L28.933,15.118C21.068,15.118 14.683,27.78 14.683,43.377L14.683,552.041C14.683,567.638 21.068,580.3 28.933,580.3L380.447,580.3C388.312,580.3 394.697,567.638 394.697,552.041L394.697,43.377Z\" fill=\"#D0DEF2\"></path>\n                </g>\n                <g transform=\"matrix(0.578925,0,0,0.973137,451.5,10.2885)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n                <g transform=\"matrix(0.578925,0,0,0.973137,101.5,10.2885)\">\n                    <path d=\"M394.697,37.725C394.697,25.248 377.669,15.118 356.696,15.118L52.684,15.118C31.711,15.118 14.683,25.248 14.683,37.725L14.683,557.693C14.683,570.17 31.711,580.3 52.684,580.3L356.696,580.3C377.669,580.3 394.697,570.17 394.697,557.693L394.697,37.725Z\" fill=\"#8FBEE7\"></path>\n                </g>\n            </svg>",
        Value = "justify-content-around"
        } }
                    })
                )
                .WithField("TitleLayout", field => field
                    .OfType("TextField")
                    .WithDisplayName("Title Layout")
                    .WithEditor("PredefinedGroup")
                    .WithPosition("1")
                    .WithSettings(new TextFieldPredefinedGroupEditorSettings
                    {
                        DefaultValue = "d-none",
                        Options = new[] { new ListValueOption()  {
        Name = "<i class=\"fas fa-eye-slash\"></i>",
        Value = "d-none"
        }, new ListValueOption()  {
        Name = "<i class=\"fas fa-align-left\"></i>",
        Value = "text-left"
        }, new ListValueOption()  {
        Name = "<i class=\"fas fa-align-center\"></i>",
        Value = "text-center"
        }, new ListValueOption()  {
        Name = "<i class=\"fas fa-align-right\"></i>",
        Value = "text-right"
        } }
                    })
                )
                .WithField("Style", field => field
                    .OfType("TextField")
                    .WithDisplayName("Style")
                    .WithPosition("4")
                )
            );
        }
        private void SimpleContent()
        {
            _contentDefinitionManager.AlterTypeDefinition("SimpleContent", type => type
                .DisplayedAs("SimpleContent")
                .Stereotype("Widget")
                .WithSettings(new FullTextAspectSettings
                {
                    IncludeFullTextTemplate = true,
                    FullTextTemplate = "{{ Model.Content.SimpleContent.Content.Html }}"
                })
                .WithPart("SimpleContent", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("SimpleContent", part => part
                .WithField("Link", field => field
                    .OfType("LinkField")
                    .WithDisplayName("Link")
                    .WithPosition("1")
                )
                .WithField("Content", field => field
                    .OfType("HtmlField")
                    .WithDisplayName("Content")
                    .WithPosition("0")
                )
            );
        }
        private void NavBar()
        {
            _contentDefinitionManager.AlterTypeDefinition("NavBar", type => type
                .DisplayedAs("NavBar")
                .Stereotype("Widget")
                .WithPart("NavBar", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("NavBar", part => part
                .WithField("Logo", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Logo")
                    .WithPosition("2")
                )
                .WithField("BrandName", field => field
                    .OfType("TextField")
                    .WithDisplayName("Brand Name")
                    .WithPosition("0")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "(optional) The brand name to display beside the logo."
                    })
                )
                .WithField("LogoSize", field => field
                    .OfType("NumericField")
                    .WithDisplayName("Logo Size")
                    .WithPosition("1")
                    .WithSettings(new NumericFieldSettings
                    {
                        DefaultValue = "4",
                        Maximum = 10,
                        Minimum = 0,
                        Required = true,
                        Scale = 1
                    })
                )
                .WithField("MenuAlias", field => field
                    .OfType("TextField")
                    .WithDisplayName("Menu Alias")
                    .WithPosition("3")
                    .WithSettings(new TextFieldSettings
                    {
                        Hint = "(optional) The alias of the menu to render if it's present"
                    })
                )
            );
        }
        private void HtmlBlurb()
        {
            _contentDefinitionManager.AlterTypeDefinition("HtmlBlurb", type => type
                .DisplayedAs("HTML Blurb")
                .Stereotype("Widget")
                .WithPart("HtmlBlurb", part => part
                    .WithPosition("0")
                )
                .WithPart("HtmlBodyPart", part => part
                    .WithPosition("1")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("HtmlBlurb", part => part
                .WithField("FluidWidth", field => field
                    .OfType("BooleanField")
                    .WithDisplayName("Fluid Width")
                    .WithEditor("Switch")
                    .WithPosition("0")
                    .WithSettings(new BooleanFieldSettings
                    {
                        Hint = "Instead of respecting the Section layout, uses the full width of the container.",
                        Label = "Fluid Width"
                    })
                )
            );
        }
        private void PageHeader()
        {
            _contentDefinitionManager.AlterTypeDefinition("PageHeader", type => type
                .DisplayedAs("Page Header")
                .Stereotype("Widget")
                .WithSettings(new FullTextAspectSettings
                {
                    IncludeFullTextTemplate = true,
                    FullTextTemplate = "{{ Model.Content.PageHeader.Subtitle.Text }}"
                })
                .WithPart("PageHeader", part => part
                    .WithPosition("1")
                )
                .WithPart("TitlePart", part => part
                    .WithPosition("0")
                    .WithSettings(new TitlePartSettings
                    {
                        RenderTitle = false
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("PageHeader", part => part
                .WithField("Subtitle", field => field
                    .OfType("TextField")
                    .WithDisplayName("Subtitle")
                    .WithPosition("0")
                )
                .WithField("Image", field => field
                    .OfType("MediaField")
                    .WithDisplayName("Image")
                    .WithPosition("1")
                )
            );
        }
    }
}