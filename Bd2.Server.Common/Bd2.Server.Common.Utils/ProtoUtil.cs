using System;
using Google.Protobuf;

namespace Bd2.Server.Common.Utils;

public static class ProtoUtil
{
	public static T Deserialize<T>(string data) where T : IMessage<T>, new()
	{
		string s = AesUtils.AesDecrypt(data);
		return new MessageParser<T>(() => new T()).ParseFrom(Convert.FromBase64String(s));
	}
}
