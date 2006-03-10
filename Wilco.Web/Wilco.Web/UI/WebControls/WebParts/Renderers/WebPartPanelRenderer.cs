using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Wilco.Web.UI.WebControls.WebParts
{
	/// <summary>
	/// Represents a renderer which renders webpart panels.
	/// </summary>
	public class WebPartPanelRenderer
	{
		private WebPartZone zone;

		/// <summary>
		/// Gets the webpart zone for which the renderer will render the webpart panels.
		/// </summary>
		protected WebPartZone Zone
		{
			get
			{
				return this.zone;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Wilco.Web.UI.WebControls.WebParts.WebPartPanelRenderer"/> class.
		/// </summary>
		/// <param name="zone">The zone for which the renderer will render the webpart panels.</param>
		public WebPartPanelRenderer(WebPartZone zone)
		{
			this.zone = zone;
		}

		/// <summary>
		/// Renders a webpart panel.
		/// </summary>
		/// <param name="writer">The writer to write to.</param>
		/// <param name="panel">The webpart panel to render.</param>
		public void RenderWebPartPanel(HtmlTextWriter writer, WebPartPanel panel)
		{
			if (panel.IsMinimized)
				panel.Style[HtmlTextWriterStyle.Display] = "none";
			else
				panel.Style[HtmlTextWriterStyle.Display] = String.Empty;

			string boxClientID = panel.ContainerClientID;

			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0px");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0px");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			writer.RenderBeginTag(HtmlTextWriterTag.Tr);

			writer.AddAttribute("relatedWebPart", panel.ClientID);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0");
			writer.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "0px");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Padding, "0px");
			writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "100%");
			writer.RenderBeginTag(HtmlTextWriterTag.Table);

			panel.HeaderStyle.AddAttributesToRender(writer);
			writer.RenderBeginTag(HtmlTextWriterTag.Tr);

			writer.AddAttribute(HtmlTextWriterAttribute.Title, panel.ToolTip);
			if (WebPartManager.SupportsClientSideDragDrop(this.zone.Page))
			{
				string dragCaption = (panel.DragCaption.Length > 0) ? panel.DragCaption : panel.Caption;
				writer.AddAttribute("onmousedown", String.Format("MoveWebPartStart('{0}', '{1}');", boxClientID, dragCaption));

				writer.AddStyleAttribute(HtmlTextWriterStyle.Cursor, "move");
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			Label titleBarCaption = new Label();
			titleBarCaption.ControlStyle.CopyFrom(panel.CaptionStyle);
			titleBarCaption.Text = panel.Caption;
			titleBarCaption.RenderControl(writer);

			writer.RenderEndTag();

			writer.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "right");
			panel.Verbs.ControlStyle.AddAttributesToRender(writer);
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			foreach (Control verb in panel.Verbs.Controls)
			{
				if (verb.Visible && !(verb is LiteralControl))
				{
					verb.RenderControl(writer);
				}
			}

			writer.RenderEndTag();

			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();
			writer.RenderEndTag();

			writer.RenderBeginTag(HtmlTextWriterTag.Tr);

			writer.AddStyleAttribute("vertical-align", "top");
			writer.RenderBeginTag(HtmlTextWriterTag.Td);

			panel.RenderControl(writer);

			writer.RenderEndTag();
			writer.RenderEndTag();

			writer.RenderEndTag();
		}
	}
}