// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CmdPal.Core.ViewModels;
using Microsoft.CommandPalette.Extensions;
using Microsoft.Extensions.Logging;

namespace Microsoft.CmdPal.UI.ViewModels;

public partial class CommandPaletteContentPageViewModel : ContentPageViewModel
{
    private readonly ILogger logger;

    public CommandPaletteContentPageViewModel(IContentPage model, TaskScheduler scheduler, AppExtensionHost host, ILogger logger)
        : base(model, scheduler, host, logger)
    {
        this.logger = logger;
    }

    public override ContentViewModel? ViewModelFromContent(IContent content, WeakReference<IPageContext> context)
    {
        ContentViewModel? viewModel = content switch
        {
            IFormContent form => new ContentFormViewModel(form, context, logger),
            IMarkdownContent markdown => new ContentMarkdownViewModel(markdown, context, logger),
            ITreeContent tree => new ContentTreeViewModel(tree, context, logger),
            _ => null,
        };
        return viewModel;
    }
}
