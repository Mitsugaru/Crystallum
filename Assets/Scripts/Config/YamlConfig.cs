using UnityEngine;
using System.Collections;
using System.IO;
using YamlDotNet.Serialization;

public class YamlConfig {

    protected string path;

    public YamlConfig(string path)
    {
        this.path = path;
    }

    protected T Parse<T>()
    {
        T conf = default(T);
        if(File.Exists(path))
        {
            using(StreamReader reader = new StreamReader(path))
            {
                Deserializer deserializer = new Deserializer(ignoreUnmatched: true);
                conf = deserializer.Deserialize<T>(reader);
            }
        }
        return conf;
    }

    protected void Write(object source)
    {
        using(TextWriter writer = File.CreateText(path))
        {
            Serializer serializer = new Serializer();
            serializer.Serialize(writer, source);
        }
    }
}
