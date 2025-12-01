// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;

namespace Microsoft.CmdPal.UI.Helpers.MarkdownImageProviders;

internal sealed partial class ImageProvider : IImageProvider
{
    private readonly CompositeImageSourceProvider _compositeProvider = new();

    public async Task<Image> GetImage(string url)
    {
        try
        {
            ImageSourceFactory.Initialize();

            var imageSource = await _compositeProvider.GetImageSource(url);
            return RtbInlineImageFactory.Create(imageSource.ImageSource, new RtbInlineImageFactory.InlineImageOptions
            {
                DownscaleOnly = imageSource.Hints.DownscaleOnly ?? true,
                FitColumnWidth = imageSource.Hints.FitMode == "fit",
                MaxWidthDip = imageSource.Hints.MaxPixelWidth,
                MaxHeightDip = imageSource.Hints.MaxPixelHeight,
                WidthDip = imageSource.Hints.DesiredPixelWidth,
                HeightDip = imageSource.Hints.DesiredPixelHeight,
            });
        }
        catch (Exception ex)
        {
            var logger = App.Current.Services.GetRequiredService<ILogger>();
            Log_ImageProviderError(logger, url, ex);
            return null!;
        }
    }

    public bool ShouldUseThisProvider(string url)
    {
        return _compositeProvider.ShouldUseThisProvider(url);
    }

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to provide an image from URI '{Url}'")]
    static partial void Log_ImageProviderError(ILogger logger, string url, Exception ex);
}
