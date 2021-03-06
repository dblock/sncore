using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Wilco.Web.UI;
using SnCore.Services;
using AjaxControlToolkit;
using System.Text;
using SnCore.Tools.Web;

public partial class MadLibInstanceEditControl : Control
{
    private bool mValid = true;
    private bool mBlind = true;
    private StringBuilder mText = null;
    private int[] mIndices = null;
    private List<string> mTextTags = new List<string>();

    public int MadLibId
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<int>(ViewState, "MadLibId", 0);
        }
        set
        {
            ViewState["MadLibId"] = value;
        }
    }

    public bool IsBlind
    {
        get
        {
            return ViewStateUtility.GetViewStateValue<bool>(ViewState, "Blind", mBlind);
        }
        set
        {
            ViewState["Blind"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            DataBind();
        }
    }

    private delegate void OnText(string value, int pos);
    private delegate void OnTag(string value, int pos);

    private int[] Parse(string text, OnText OnTextCallback, OnTag OnTagCallback)
    {
        List<int> result = new List<int>();
        int startPos = 0;
        int tagPos = text.IndexOf("[");
        while (tagPos >= 0)
        {
            // find end tag
            int endPos = text.IndexOf("]", tagPos + 1);
            if (endPos < 0) break;

            // add text before
            if (tagPos != startPos)
            {
                if (OnTextCallback != null)
                {
                    string t = text.Substring(startPos, tagPos - startPos);
                    OnTextCallback(t, startPos);
                }
            }

            string tag = text.Substring(tagPos + 1, endPos - tagPos - 1);

            if (OnTagCallback != null)
            {
                result.Add(tagPos);
                OnTagCallback(tag, tagPos);
            }

            startPos = endPos + 1;
            tagPos = text.IndexOf("[", startPos);
        }

        if (OnTextCallback != null)
        {
            string t = text.Substring(startPos, text.Length - startPos);
            OnTextCallback(t, startPos);
        }

        return result.ToArray();
    }

    private void OnTextDataBind(string value, int pos)
    {
        string id = string.Format("inputText_{0}", pos);
        LiteralControl lc = (LiteralControl)FindControl(id);

        if (lc == null)
        {
            lc = new LiteralControl();
            lc.ID = id;
            Controls.Add(lc);
        }

        lc.Text = GetBlindValue(value);
    }

    private void OnTagDataBind(string value, int pos)
    {
        if (string.IsNullOrEmpty(value)) value = "blank";

        string id = string.Format("inputTag_{0}", pos);

        TextBox tb = (TextBox) FindControl(id);

        if (tb != null)
            return;

        tb = new TextBox();
        tb.ID = id;
        tb.CssClass = "sncore_madlib_textbox";

        string qs = Request[value];
        if (!string.IsNullOrEmpty(qs))
        {
            tb.Text = qs;
            tb.Width = qs.Length * 10;
        }
        else
        {
            tb.Width = value.Length * 10;
        }

        Controls.Add(tb);

        TextBoxWatermarkExtender tbex = new TextBoxWatermarkExtender();
        tbex.ID = string.Format("inputTagExtender_{0}", pos);
        tbex.WatermarkText = value;
        tbex.WatermarkCssClass = "sncore_madlib_watermark";
        tbex.TargetControlID = tb.ID;

        Controls.Add(tbex);
    }

    public override void DataBind()
    {
        if (MadLibId <= 0)
        {
            throw new Exception("Missing Mad Lib Id");
        }

        TransitMadLib ml = SessionManager.MadLibService.GetMadLibById(SessionManager.Ticket, MadLibId);
        mIndices = Parse(ml.Template, OnTextDataBind, OnTagDataBind);

        base.DataBind();
    }

    public void TextBind(string value)
    {
        Parse(value, null, OnTextTagCollect);
        for (int i = 0; i < Math.Min(mIndices.Length, mTextTags.Count); i++)
        {
            string id = string.Format("inputTag_{0}", mIndices[i]);
            TextBox tb = (TextBox)FindControl(id);
            if (tb == null)
            {
                throw new Exception(string.Format("Missing Control {0}", id));
            }

            tb.Text = mTextTags[i];
        }
    }

    private void OnTextTagCollect(string value, int pos)
    {
        mTextTags.Add(value);
    }

    private void OnTextCollect(string value, int pos)
    {
        mText.Append(value);
    }

    private void OnTagCollect(string value, int pos)
    {
        string id = string.Format("inputTag_{0}", pos);
        TextBox tb = (TextBox)FindControl(id);
        if (tb == null)
        {
            throw new Exception(string.Format("Missing Control {0}", id));
        }

        if (string.IsNullOrEmpty(tb.Text))
        {
            string idx = string.Format("inputTagExtender_{0}", pos);
            TextBoxWatermarkExtender tbex = (TextBoxWatermarkExtender)FindControl(idx);
            tbex.WatermarkCssClass = "sncore_madlib_watermark_missing";
            mValid = false;
        }

        mText.Append("[");
        mText.Append(tb.Text);
        mText.Append("]");
    }

    public bool TryGetText(ref string value)
    {
        if (mText == null)
        {
            mValid = true;
            mText = new StringBuilder();

            if (MadLibId <= 0)
            {
                throw new Exception("Missing Mad Lib Id");
            }

            TransitMadLib ml = SessionManager.MadLibService.GetMadLibById(SessionManager.Ticket, MadLibId);
            Parse(ml.Template, OnTextCollect, OnTagCollect);
        }

        value = mText.ToString();
        return mValid;
    }

    public string GetBlindValue(string value)
    {
        if (!IsBlind || string.IsNullOrEmpty(value))
        {
            return value;
        }
        else
        {
            StringBuilder blindvalue = new StringBuilder(value);
            for (int i = 0; i < blindvalue.Length; i++)
            {
                if (Char.IsLetterOrDigit(blindvalue[i]))
                    blindvalue[i] = '*';
            }

            return blindvalue.ToString();
        }
    }
}
