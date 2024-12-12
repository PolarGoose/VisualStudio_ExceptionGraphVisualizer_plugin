using Microsoft.VisualStudio.Extensibility.DebuggerVisualizers;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.RpcContracts.RemoteUI;
using System.Diagnostics;
using ExceptionGraphVisualizerSource;

namespace ExceptionGraphVisualizer;

[VisualStudioContribution]
internal class ExceptionDebuggerVisualizerProvider(ExtensionEntrypoint extension, VisualStudioExtensibility extensibility) : DebuggerVisualizerProvider(extension, extensibility)
{
    public override DebuggerVisualizerProviderConfiguration DebuggerVisualizerProviderConfiguration => new("Exception Graph Visualizer", typeof(Exception))
    {
        VisualizerObjectSourceType = new(typeof(ExceptionVisualizerObjectSource)),
    };

    public override async Task<IRemoteUserControl> CreateVisualizerAsync(VisualizerTarget visualizerTarget, CancellationToken cancellationToken)
    {
        Trace.WriteLine("ExceptionDebuggerVisualizerProvider: RequestDataAsync");
        var data = await visualizerTarget.ObjectSource.RequestDataAsync<DataFromDebuggeeToDebugger>(jsonSerializer: null, CancellationToken.None);
        Trace.WriteLine("ExceptionDebuggerVisualizerProvider: Data retrieved");
        if (data.ErrorMessage != string.Empty)
        {
            Trace.WriteLine(data.ErrorMessage);
            return new MessageControl(data.ErrorMessage);
        }

        try
        {
            string tempFilePath = Path.GetTempFileName();

            string dotFilePath = tempFilePath.Replace(".tmp", ".dot");
            File.WriteAllText(dotFilePath, data.DotGraphDescription);

            var pdfData = GraphVizInterop.Generate(data.DotGraphDescription, "dot", "pdf");
            string pdfFilePath = tempFilePath.Replace(".tmp", ".pdf");
            File.WriteAllBytes(pdfFilePath, pdfData);

            new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = pdfFilePath,
                    UseShellExecute = true
                }
            }.Start();

            return new MessageControl($"Generated graph:\n{dotFilePath}\n{pdfFilePath}");
        }
        catch (Exception ex)
        {
            return new MessageControl($"Failed to generate the graph from the dot description:\n{data.DotGraphDescription}\n\nError message:\n{ex}");
        }
    }
}
