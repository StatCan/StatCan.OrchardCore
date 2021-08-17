using OrchardCore.ContentFields.Settings;
using OrchardCore.ContentManagement.Metadata;
using OrchardCore.ContentManagement.Metadata.Settings;
using OrchardCore.Data.Migration;
using OrchardCore.Flows.Models;
using StatCan.OrchardCore.Extensions;
using StatCan.OrchardCore.ContentFields.PredefinedGroup.Settings;
using OrchardCore.Modules;

namespace StatCan.OrchardCore.Vuetify.Migrations
{
    [Feature(Constants.Features.Grid)]
    public class GridMigrations : DataMigration
    {
        private readonly IContentDefinitionManager _contentDefinitionManager;
        public GridMigrations(IContentDefinitionManager contentDefinitionManager)
        {
            _contentDefinitionManager = contentDefinitionManager;
        }

        public int Create()
        {
            VCol();
            VRow();
            VContainer();
            VDivider();
            VContainerRow();

            return 1;
        }


        private void VCol()
        {
            _contentDefinitionManager.AlterTypeDefinition("VCol", type => type
                .DisplayedAs("VCol")
                .Stereotype("Widget")
                .WithPart("VCol", part => part
                    .WithPosition("0")
                )
                .WithFlow("1")
            );

            var colsSettings = new TextFieldPredefinedListEditorSettings()
            {
                Editor = EditorOption.Dropdown,
                DefaultValue = "None",
                Options = new ListValueOption[] {
                    new ListValueOption(){Name = "None", Value = ""},
                    new ListValueOption(){Name = "Auto", Value = "auto"},
                    new ListValueOption(){Name = "1", Value = "1"},
                    new ListValueOption(){Name = "2", Value = "2"},
                    new ListValueOption(){Name = "3", Value = "3"},
                    new ListValueOption(){Name = "4", Value = "4"},
                    new ListValueOption(){Name = "5", Value = "5"},
                    new ListValueOption(){Name = "6", Value = "6"},
                    new ListValueOption(){Name = "7", Value = "7"},
                    new ListValueOption(){Name = "8", Value = "8"},
                    new ListValueOption(){Name = "9", Value = "9"},
                    new ListValueOption(){Name = "10", Value = "10"},
                    new ListValueOption(){Name = "11", Value = "11"},
                    new ListValueOption(){Name = "12", Value = "12"},
                }
            };

            var offsetSettings = new TextFieldPredefinedListEditorSettings()
            {
                Editor = EditorOption.Dropdown,
                DefaultValue = "",
                Options = new ListValueOption[] {
                    new ListValueOption(){Name = "None", Value = ""},
                    new ListValueOption(){Name = "1", Value = "1"},
                    new ListValueOption(){Name = "2", Value = "2"},
                    new ListValueOption(){Name = "3", Value = "3"},
                    new ListValueOption(){Name = "4", Value = "4"},
                    new ListValueOption(){Name = "5", Value = "5"},
                    new ListValueOption(){Name = "6", Value = "6"},
                    new ListValueOption(){Name = "7", Value = "7"},
                    new ListValueOption(){Name = "8", Value = "8"},
                    new ListValueOption(){Name = "9", Value = "9"},
                    new ListValueOption(){Name = "10", Value = "10"},
                    new ListValueOption(){Name = "11", Value = "11"},
                    new ListValueOption(){Name = "12", Value = "12"},
                }
            };

            _contentDefinitionManager.AlterPartDefinition("VCol", part => part
                .WithTextFieldPredefinedList("AlignSelf", "Align Self", "0", new TextFieldPredefinedListEditorSettings()
                    {
                        Editor = EditorOption.Dropdown,
                        DefaultValue = "",
                        Options = new ListValueOption[] {
                                        new ListValueOption(){Name = "None", Value = ""},
                                        new ListValueOption(){Name = "Start", Value = "start"},
                                        new ListValueOption(){Name = "Center", Value = "center"},
                                        new ListValueOption(){Name = "End", Value = "end"},
                                        new ListValueOption(){Name = "Auto", Value = "auto"},
                                        new ListValueOption(){Name = "Baseline", Value = "baseline"},
                                        new ListValueOption(){Name = "Stretch", Value = "stretch"}
                                    }
                    }
                )
                .WithTextFieldPredefinedList("Cols", "Cols Xs", "1", colsSettings)
                .WithTextFieldPredefinedList("ColsSm", "Cols Sm", "2", colsSettings)
                .WithTextFieldPredefinedList("ColsMd", "Cols Md", "3", colsSettings)
                .WithTextFieldPredefinedList("ColsLg", "Cols Lg", "4", colsSettings)
                .WithTextFieldPredefinedList("ColsXl", "Cols Xl", "5", colsSettings)
                .WithTextFieldPredefinedList("Offset", "Offset Xs", "6", offsetSettings)
                .WithTextFieldPredefinedList("OffsetSm", "Offset Sm", "7", offsetSettings)
                .WithTextFieldPredefinedList("OffsetMd", "Offset Md", "8", offsetSettings)
                .WithTextFieldPredefinedList("OffsetLg", "Offset Lg", "9", offsetSettings)
                .WithTextFieldPredefinedList("OffsetXl", "Offset Xl", "10", offsetSettings)
                .WithNumericField("Order", "Order Xs", "11")
                .WithNumericField("OrderSm", "Order Sm", "12")
                .WithNumericField("OrderMd", "Order Md", "13")
                .WithNumericField("OrderLg", "Order Lg", "14")
                .WithNumericField("OrderXl", "Order Xl", "15")
            );
        }

        private void VRow()
        {
            _contentDefinitionManager.AlterTypeDefinition("VRow", type => type
                .DisplayedAs("VRow")
                .Stereotype("Widget")
                .WithPart("VRow", part => part
                    .WithPosition("0")
                )
                .WithFlow("1", new[] { "VCol" })
            );

            var justifyOptions = new ListValueOption[] {
                new ListValueOption(){Name = "None", Value = ""},
                new ListValueOption() {
                    Name = "Start",
                    Value = "start"
                },
                new ListValueOption() {
                    Name = "End",
                    Value = "end"
                },
                new ListValueOption() {
                    Name = "Center",
                    Value = "center"
                },
                new ListValueOption() {
                    Name = "Space-Around",
                    Value = "space-around"
                },
                new ListValueOption() {
                    Name = "Space-between",
                    Value = "space-between"
                }
            };

            var alignOptions = new ListValueOption[] {
                new ListValueOption(){Name = "None", Value = ""},
                new ListValueOption() {
                    Name = "Start",
                    Value = "start"
                },
                new ListValueOption() {
                    Name = "Center",
                    Value = "center"
                },
                new ListValueOption() {
                    Name = "End",
                    Value = "end"
                },
                new ListValueOption() {
                    Name = "Baseline",
                    Value = "baseline"
                },
                new ListValueOption() {
                    Name = "Stretch",
                    Value = "stretch"
                }
            };

            var alignContentOptions = new ListValueOption[] {
                new ListValueOption(){Name = "None", Value = ""},
                new ListValueOption() {
                    Name = "Start",
                    Value = "start"
                },
                new ListValueOption() {
                    Name = "Center",
                    Value = "center"
                },
                new ListValueOption() {
                    Name = "End",
                    Value = "end"
                },
                new ListValueOption() {
                    Name = "Space-between",
                    Value = "space-between"
                },
                new ListValueOption() {
                    Name = "Space-around",
                    Value = "space-around"
                },
                new ListValueOption() {
                    Name = "Stretch",
                    Value = "stretch"
                }
            };

            _contentDefinitionManager.AlterPartDefinition("VRow", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Dense", Value = "dense"},
                            new MultiTextFieldValueOption() {Name = "No Gutters", Value = "no-gutters"}
                        },
                    })
                )
                .WithField("Justify", field => field
                    .OfType("TextField")
                    .WithDisplayName("Justify")
                    .WithEditor("PredefinedList")
                    .WithPosition("1")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = justifyOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Applies the justify-content css property. Available options are start, center, end, space-between and space-around."
                    }))
                .WithField("JustifySm", field => field
                    .OfType("TextField")
                    .WithDisplayName("JustifySm")
                    .WithEditor("PredefinedList")
                    .WithPosition("2")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = justifyOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the justify-content property on small and greater breakpoints."
                    }))
                .WithField("JustifyMd", field => field
                    .OfType("TextField")
                    .WithDisplayName("JustifyMd")
                    .WithEditor("PredefinedList")
                    .WithPosition("3")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = justifyOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the justify-content property on medium and greater breakpoints."
                    }))
                .WithField("JustifyLg", field => field
                    .OfType("TextField")
                    .WithDisplayName("JustifyLg")
                    .WithEditor("PredefinedList")
                    .WithPosition("4")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = justifyOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the justify-content property on large and greater breakpoints."
                    }))
                .WithField("JustifyXl", field => field
                    .OfType("TextField")
                    .WithDisplayName("JustifyXl")
                    .WithEditor("PredefinedList")
                    .WithPosition("5")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = justifyOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the justify-content property on extra large and greater breakpoints."
                    }))
                .WithField("Align", field => field
                    .OfType("TextField")
                    .WithDisplayName("Align")
                    .WithEditor("PredefinedList")
                    .WithPosition("6")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = alignOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Applies the align-items css property. Available options are start, center, end, baseline, and stretch."
                    }))
                .WithField("AlignSm", field => field
                    .OfType("TextField")
                    .WithDisplayName("AlignSm")
                    .WithEditor("PredefinedList")
                    .WithPosition("7")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = alignOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the align-items property on small and greater breakpoints."
                    }))
                .WithField("AlignMd", field => field
                    .OfType("TextField")
                    .WithDisplayName("AlignMd")
                    .WithEditor("PredefinedList")
                    .WithPosition("8")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = alignOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the align-items property on medium and greater breakpoints."
                    }))
                .WithField("AlignLg", field => field
                    .OfType("TextField")
                    .WithDisplayName("AlignLg")
                    .WithEditor("PredefinedList")
                    .WithPosition("9")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = alignOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the align-items property on large and greater breakpoints."
                    }))
                .WithField("AlignXl", field => field
                    .OfType("TextField")
                    .WithDisplayName("AlignXl")
                    .WithEditor("PredefinedList")
                    .WithPosition("10")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = alignOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the align-items property on extra large and greater breakpoints."
                    }))
                .WithField("AlignContent", field => field
                    .OfType("TextField")
                    .WithDisplayName("AlignContent")
                    .WithEditor("PredefinedList")
                    .WithPosition("11")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = alignContentOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Applies the align-content css property. Available options are start, center, end, baseline, and stretch."
                    }))
                .WithField("AlignContentSm", field => field
                    .OfType("TextField")
                    .WithDisplayName("AlignContentSm")
                    .WithEditor("PredefinedList")
                    .WithPosition("12")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = alignContentOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the align-items property on small and greater breakpoints."
                    }))
                .WithField("AlignContentMd", field => field
                    .OfType("TextField")
                    .WithDisplayName("AlignContentMd")
                    .WithEditor("PredefinedList")
                    .WithPosition("13")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = alignContentOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the align-items property on medium and greater breakpoints."
                    }))
                .WithField("AlignContentLg", field => field
                    .OfType("TextField")
                    .WithDisplayName("AlignContentLg")
                    .WithEditor("PredefinedList")
                    .WithPosition("14")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = alignContentOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the align-items property on large and greater breakpoints."
                    }))
                .WithField("AlignContentXl", field => field
                    .OfType("TextField")
                    .WithDisplayName("AlignContentXl")
                    .WithEditor("PredefinedList")
                    .WithPosition("15")
                    .WithSettings(new TextFieldPredefinedListEditorSettings()
                    {
                        DefaultValue = "",
                        Editor = EditorOption.Dropdown,
                        Options = alignContentOptions
                    })
                    .WithSettings(new TextFieldSettings() {
                        Hint = "Changes the align-items property on extra large and greater breakpoints."
                    }))
            );
        }

        private void VContainer()
        {
            _contentDefinitionManager.AlterTypeDefinition("VContainer", type => type
                .DisplayedAs("VContainer")
                .Stereotype("Widget")
                .WithPart("VContainer", part => part
                    .WithPosition("0")
                )
                .WithFlow("1", new[] { "VRow" })
            );

            _contentDefinitionManager.AlterPartDefinition("VContainer", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithPosition("0")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                            new MultiTextFieldValueOption() {Name = "Fluid", Value = "fluid"},
                        }
                    })
                )
            );
        }
        private void VDivider() {
            _contentDefinitionManager.AlterTypeDefinition("VDivider", type => type
                .DisplayedAs("VDivider")
                .Stereotype("Widget")
                .WithPart("VDivider", part => part
                    .WithPosition("0")
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VDivider", part => part
                .WithField("Props", field => field
                    .OfType("MultiTextField")
                    .WithDisplayName("Props")
                    .WithEditor("Picker")
                    .WithSettings(new MultiTextFieldSettings
                    {
                        Options = new MultiTextFieldValueOption[] {
                        new MultiTextFieldValueOption() {Name = "Dark", Value = "dark"},
                        new MultiTextFieldValueOption() {Name = "Inset", Value = "inset"},
                        new MultiTextFieldValueOption() {Name = "Light", Value = "light"},
                        new MultiTextFieldValueOption() {Name = "Vertical", Value = "vertical"},
                    },
                    })
                )
            );
        }
        private void VContainerRow()
        {
            _contentDefinitionManager.AlterTypeDefinition("VContainerRow", type => type
                .DisplayedAs("VContainerRow")
                .Stereotype("Widget")
                .WithPart("VContainerRow", part => part
                    .WithPosition("0")
                )
                .WithPart("FlowPart", part => part
                    .WithPosition("1")
                    .WithSettings(new FlowPartSettings
                    {
                        ContainedContentTypes = new[] { "VCol" },
                    })
                )
            );

            _contentDefinitionManager.AlterPartDefinition("VContainerRow", part => part
                .WithField("Layout", field => field
                    .OfType("TextField")
                    .WithDisplayName("Layout")
                    .WithEditor("PredefinedGroup")
                    .WithSettings(new TextFieldPredefinedGroupEditorSettings
                    {
                        Options = new ListValueOption[] { new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(2.04358,0,0,0.978801,0.178752,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "12"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(1.00094,0,0,0.978801,0.59795,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(1.00094,0,0,0.978801,50.5979,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "6-6"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(0.667293,0,0,0.978801,0.731966,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.667293,0,0,0.978801,33.732,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.667293,0,0,0.978801,66.732,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "4-4-4"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(0.458764,0,0,0.978801,0.815727,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.479617,0,0,0.978801,23.8074,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.521323,0,0,0.978801,47.7906,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.521323,0,0,0.978801,73.7906,0.807915)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "3-3-3-3"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(0.834116,0,0,0.978801,0.664958,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(1.18862,0,0,0.978801,41.5226,0.807915)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "5-7"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(1.18862,0,0,0.978801,0.522565,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.834116,0,0,0.978801,58.665,0.807915)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "7-5"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(0.563029,0,0,0.978801,0.773847,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(1.4597,0,0,0.978801,28.4137,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "3-9"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(1.4597,0,0,0.978801,0.413676,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.563029,0,0,0.978801,71.7738,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "9-3"
                        }, new ListValueOption() {
                            Name = "<svg width=\"100%\" height=\"100%\" viewBox=\"0 0 100 50\" version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xml:space=\"preserve\" xmlns:serif=\"http://www.serif.com/\" style=\"fill-rule:evenodd;clip-rule:evenodd;stroke-linejoin:round;stroke-miterlimit:2;\">\n    <g transform=\"matrix(0.563029,0,0,0.978801,0.773847,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.875822,0,0,0.978801,28.6482,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n    <g transform=\"matrix(0.563029,0,0,0.978801,71.7738,0.581353)\">\n        <rect x=\"0.402\" y=\"0.428\" width=\"47.955\" height=\"49.04\" style=\"fill:rgb(255,165,0);\"/>\n    </g>\n</svg>",
                            Value = "3-6-3"
                        } },
                        DefaultValue = "6-6",
                    })
                )
            );
        }

    }
}
