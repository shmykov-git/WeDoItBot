using System.IO;
using System.Text;
using System.Xml;
using Common.Logs;

namespace Common.Extensions
{
	public static class XmlHelper
	{
		public static string ToIndentedXml(this string xml)
		{
			using (var mStream = new MemoryStream())
			using (var writer = new XmlTextWriter(mStream, Encoding.Unicode))
			{
				var document = new XmlDocument();
				document.LoadXml(xml);

				writer.Formatting = Formatting.Indented;

				document.WriteContentTo(writer);
				writer.Flush();
				mStream.Flush();

				mStream.Position = 0;

				var sReader = new StreamReader(mStream);

				var indentedXml = sReader.ReadToEnd();

				return indentedXml;
			}
		}
	}
}