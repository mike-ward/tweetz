using System;
using System.Runtime.Serialization;

namespace tweetz.core.Models;

[Serializable]
public class TweetzException : Exception
{
    public TweetzException() { }

    public TweetzException(string message)
        : base(message) { }

    protected TweetzException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }

}