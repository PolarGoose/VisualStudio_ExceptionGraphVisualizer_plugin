using Microsoft.VisualStudio.Extensibility.UI;
using System.Runtime.Serialization;

namespace ExceptionGraphVisualizer;

[DataContract]
public class MessageControlViewModel(string errorMessage)
{
    [DataMember]
    public string ErrorMessage { get; } = errorMessage;
}


public class MessageControl(string errorMessage) : RemoteUserControl(new MessageControlViewModel(errorMessage))
{
}
