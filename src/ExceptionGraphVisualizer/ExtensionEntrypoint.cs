using Microsoft.VisualStudio.Extensibility;

namespace ExceptionGraphVisualizer;

[VisualStudioContribution]
internal class ExtensionEntrypoint : Extension
{
    public override ExtensionConfiguration ExtensionConfiguration => new()
    {
        Metadata = new(id: "ExceptionGraphVisualizer.295BB183-D720-4778-9A5A-175885ED2988",
                       version: ExtensionAssemblyVersion,
                       publisherName: "PolarGoose",
                       displayName: "Exception Graph Visualizer",
                       description: @"Shows exceptions as a graph with its inner exceptions and stack traces.<br>
More details: <a href=""https://github.com/PolarGoose/VisualStudio_ExceptionGraphVisualizer_plugin"">https://github.com/PolarGoose/VisualStudio_ExceptionGraphVisualizer_plugin</a>")
        {
            Tags = ["visualizer", "exception", "debug", "debugging"],
            InstallationTargetArchitecture = VisualStudioArchitecture.Amd64,
            MoreInfo = "https://github.com/PolarGoose/VisualStudio_ExceptionGraphVisualizer_plugin",
        }
    };
}
