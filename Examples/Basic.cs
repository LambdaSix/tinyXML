using System.Xml.Linq;

namespace TinyXml.Examples {
	public class TinyXMLExample {
		public string ElementWithContent {get;set;}

		public TinyXMLExample(XElement source) {
			ElementWithContent = source.ElementOr(() => ElementWithContent, _ => _.TryParseString());
		}
	}
}
