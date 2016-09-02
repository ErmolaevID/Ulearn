﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using uLearn.Model.Edx.EdxComponents;

namespace uLearn.Model.Blocks
{
	public class MdBlock : SlideBlock, IXmlSerializable
	{
		private string markdown;

		[XmlText]
		public string Markdown
		{
			get { return markdown; }
			set { markdown = value.RemoveCommonNesting(); }
		}


		public MdBlock(string markdown)
		{
			if (markdown != null)
				Markdown = markdown.TrimEnd();
		}

		public MdBlock()
		{
		}

		public override string ToString()
		{
			return string.Format("Markdown {0}", Markdown);
		}

		public override Component ToEdxComponent(string displayName, Slide slide, int componentIndex)
		{
			var urlName = slide.NormalizedGuid + componentIndex;
			var htmlWithUrls = Markdown.GetHtmlWithUrls("/static/" + urlName + "_");
			return new HtmlComponent(urlName, displayName, urlName, htmlWithUrls.Item1, slide.Info.SlideFile.Directory.FullName, htmlWithUrls.Item2);
		}

		public Component ToEdxComponent(string urlName, string displayName, string directoryName)
		{
			var htmlWithUrls = Markdown.GetHtmlWithUrls("/static/" + urlName + "_");
			return new HtmlComponent(urlName, displayName, urlName, htmlWithUrls.Item1, directoryName, htmlWithUrls.Item2);
		}

		public override IEnumerable<SlideBlock> BuildUp(BuildUpContext context, IImmutableSet<string> filesInProgress)
		{
			return InnerBlocks?.SelectMany(b => b.BuildUp(context, filesInProgress)) ?? new[] { this };
		}

		public override string TryGetText()
		{
			return Markdown;
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			reader.MoveToContent();
			Hide = reader.GetAttribute("hide").IsOneOf("true", "1");
			var blocks = ReadBlocks(Hide, reader).ToArray();
			if (blocks.Length == 1 && blocks[0].GetType() == typeof(MdBlock))
				Markdown = ((MdBlock)blocks[0]).Markdown;
			else
				InnerBlocks = blocks;

		}

		public SlideBlock[] InnerBlocks { get; set; }

		private IEnumerable<SlideBlock> ReadBlocks(bool hide, XmlReader reader)
		{
			var tagName = reader.Name;
			if (reader.IsEmptyElement)
			{
				reader.Read();
				yield break;
			}
			reader.Read();
			while (!(reader.NodeType == XmlNodeType.EndElement && reader.Name == tagName))
			{
				if (reader.NodeType == XmlNodeType.Text || reader.NodeType == XmlNodeType.CDATA)
				{
					yield return new MdBlock(reader.ReadContentAsString()) { Hide = hide };
				}
				else if (reader.NodeType == XmlNodeType.Element)
				{
					if (reader.LocalName == "note")
					{
						yield return new MdBlock
						{
							Hide = true,
							Markdown = reader.ReadElementContentAsString()
						};
					}
					else if (reader.LocalName == "code")
					{
						yield return new CodeBlock(reader.ReadElementContentAsString(), null) { Hide = hide };
					}
					else
						throw new NotSupportedException(reader.LocalName);
				}
				else
					reader.Read();
			}
			reader.Read();
		}

		public void WriteXml(XmlWriter writer)
		{
			if (Hide)
				writer.WriteAttributeString("hide", "true");
			writer.WriteString(markdown);
		}
	}
}