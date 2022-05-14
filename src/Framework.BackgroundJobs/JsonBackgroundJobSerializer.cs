﻿using Framework.Core.Dependency;
using Framework.Json;
using System;

namespace Framework.BackgroundJobs
{
    public class JsonBackgroundJobSerializer : IBackgroundJobSerializer, ITransient
    {
        private readonly IJsonSerializer _jsonSerializer;

        public JsonBackgroundJobSerializer(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public string Serialize(object obj)
        {
            return _jsonSerializer.Serialize(obj);
        }

        public object Deserialize(string value, Type type)
        {
            return _jsonSerializer.Deserialize(type, value);
        }

        public T Deserialize<T>(string value)
        {
            return _jsonSerializer.Deserialize<T>(value);
        }
    }
}
