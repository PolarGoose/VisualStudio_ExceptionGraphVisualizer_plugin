using Microsoft.VisualStudio.DebuggerVisualizers;
using System.Runtime.Serialization;

namespace ExceptionGraphVisualizerSource;

[DataContract]
public class DataFromDebuggeeToDebugger
{
    [DataMember]
    public string DotGraphDescription { get; set; } = "";

    [DataMember]
    public string ErrorMessage { get; set; } = "";
}

public class ExceptionVisualizerObjectSource : VisualizerObjectSource
{
    public override void GetData(object target, Stream outgoingData)
    {
        var serializer = new DataContractSerializer(typeof(DataFromDebuggeeToDebugger));
        try
        {
            SerializeAsJson(outgoingData, new DataFromDebuggeeToDebugger
            {
                DotGraphDescription = ExceptionToDotConverter.ToDot((Exception)target)
            });
        }
        catch (Exception ex)
        {
            SerializeAsJson(outgoingData, new DataFromDebuggeeToDebugger
            {
                ErrorMessage = $"Failed to generate the graph from the exception:\n{target as Exception}\n\nError:\n{ex}"
            });
        }
    }
}
