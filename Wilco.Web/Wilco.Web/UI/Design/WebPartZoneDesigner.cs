using System;
using System.ComponentModel.Design;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

using Wilco.Web.UI.WebControls.WebParts;
using System.Web.UI;

namespace Wilco.Web.UI.Design.WebControls.WebParts
{
	// TODO: Create a proper implementation of this designer.
	public class WebPartZoneDesigner : TemplatedControlDesigner
	{
		private WebPartZone zone;
		private TemplateEditingVerb[] templateEditingVerbs;

		public override bool AllowResize
		{
			get
			{
				return true;
			}
		}

		public WebPartZoneDesigner()
		{
			//
		}

		public override void Initialize(System.ComponentModel.IComponent component)
		{
			base.Initialize(component);
			this.zone = (WebPartZone)component;
		}

		public override string GetDesignTimeHtml(DesignerRegionCollection regions)
		{
			string html;
			try
			{
				html = base.GetDesignTimeHtml();
			}
			catch (Exception exception)
			{
				html = this.GetErrorDesignTimeHtml(exception);
			}
			return html;
		}

		protected override ITemplateEditingFrame CreateTemplateEditingFrame(TemplateEditingVerb verb)
		{
			ITemplateEditingFrame frame = null;

			if (this.templateEditingVerbs != null)
			{
				ITemplateEditingService teService = (ITemplateEditingService)this.GetService(typeof(ITemplateEditingService));

				if (teService != null)
				{
					Style style = ((WebPartZone)Component).ControlStyle;
					String templateName = verb.Text.Replace(" ", "");
					frame = teService.CreateFrame(this, verb.Text, new string[] { templateName }, style, null);
				}
			}

			return frame;
		}

		protected override TemplateEditingVerb[] GetCachedTemplateEditingVerbs()
		{
			if (this.templateEditingVerbs == null)
			{
				this.templateEditingVerbs = new TemplateEditingVerb[1];
				this.templateEditingVerbs[0] = new TemplateEditingVerb("Zone Template", 0, this);
			}
			return this.templateEditingVerbs;
		}

		public override string GetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, out bool allowEditing)
		{
			string content = String.Empty;

			allowEditing = true;

			if (this.templateEditingVerbs != null)
			{
				ITemplate currentTemplate = null;
				if (templateName == "ZoneTemplate")
				{
					currentTemplate = ((WebPartZone)Component).ZoneTemplate;
				}

				if (currentTemplate != null)
				{
					content = this.GetTextFromTemplate(currentTemplate);
				}
			}

			return content;
		}

		public override void SetTemplateContent(ITemplateEditingFrame editingFrame, string templateName, string templateContent)
		{
			if (this.templateEditingVerbs != null)
			{
				WebPartZone control = (WebPartZone)Component;
				ITemplate newTemplate = null;

				if ((templateContent != null) && (templateContent.Length > 0))
				{
					newTemplate = this.GetTemplateFromText(templateContent);
				}

				if (templateName == "ZoneTemplate")
				{
					control.ZoneTemplate = newTemplate;
				}
			}
		}
	}
}