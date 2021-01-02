﻿using System.IO;
using System.Text.Json;
using XOutput.Api.Message;

namespace XOutput.Api.Serialization
{
    public class MessageWriter
    {
        public string GetString<T>(T message) where T: MessageBase
        {
            return JsonSerializer.Serialize(message);
        }

        public void Write(MessageBase message, Stream output)
        {
            var bytes = JsonSerializer.SerializeToUtf8Bytes(message);
            output.Write(bytes);
            output.Flush();
        }
    }
}
