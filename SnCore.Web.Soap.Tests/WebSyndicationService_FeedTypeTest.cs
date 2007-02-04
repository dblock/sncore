using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Web.Services.Protocols;

namespace SnCore.Web.Soap.Tests.WebSyndicationServiceTests
{
    [TestFixture]
    public class FeedTypeTest : WebServiceTest<WebSyndicationService.TransitFeedType, WebSyndicationServiceNoCache>
    {
        public FeedTypeTest()
            : base("FeedType")
        {

        }

        public override WebSyndicationService.TransitFeedType GetTransitInstance()
        {
            WebSyndicationService.TransitFeedType t_instance = new WebSyndicationService.TransitFeedType();
            t_instance.Name = GetNewString();
            t_instance.SpanColumns = 1;
            t_instance.SpanColumnsPreview = 2;
            t_instance.SpanRows = 1;
            t_instance.SpanRowsPreview = 2;
            return t_instance;
        }
    }
}
