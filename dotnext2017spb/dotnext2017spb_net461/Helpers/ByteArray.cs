﻿using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace DotNext
{
  public static class ByteArray
  {
    public static T ConvertTo<T>(this byte[] source)
    {
      if (source == null)
      {
        return default(T);
      }

      var converter = new DataContractSerializer(typeof(T));

      using (var inputStream = new MemoryStream(source))
      using (var xmlReader = XmlDictionaryReader.CreateBinaryReader(inputStream, new XmlDictionaryReaderQuotas() { MaxArrayLength = 1024 * 1024 * 1024 }))
      {
        return (T)converter.ReadObject(xmlReader);
      }
    }

    public static byte[] CreateFrom<T>(T data)
    {
      var converter = new DataContractSerializer(typeof(T));

      using (var inputStream = new MemoryStream())
      {
        using (var xmlWriter = XmlDictionaryWriter.CreateBinaryWriter(inputStream))
        {
          converter.WriteObject(xmlWriter, data);
        }
        return inputStream.ToArray();
      }
    }
  }
}
