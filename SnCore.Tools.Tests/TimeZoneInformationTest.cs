using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using System.Resources;
using System.IO;
using SnCore.Tools.Drawing;
using System.Drawing;
using Gif.Components;

namespace SnCore.Tools.Tests
{
    [TestFixture]
    public class TimezoneInformationTest
    {
        [Test]
        public void TestKnownTimeZones()
        {
            int previousHours = 14;
            TimeZoneInformation[] tzs = TimeZoneInformation.EnumZones();
            foreach (TimeZoneInformation ti in tzs)
            {
                Assert.IsTrue(ti.CurrentUtcBias.Minutes == 0 
                    || Math.Abs(ti.CurrentUtcBias.Minutes) == 30 
                    || Math.Abs(ti.CurrentUtcBias.Minutes) == 45,
                    string.Format("Minutes = {0}", ti.CurrentUtcBias.Minutes));
                Console.WriteLine("{0}: {1}", ti.DisplayName, ti.CurrentUtcBias);
                Assert.IsTrue(ti.CurrentUtcBias.Hours <= 13 && ti.CurrentUtcBias.Hours >= -12);
                Assert.IsTrue(previousHours + 1 >= ti.CurrentUtcBias.Hours); // adjust an extra hour for daylight savings
                previousHours = ti.CurrentUtcBias.Hours;
            }
        }

        [Test]
        public void TestTryParseTimezoneOffsetToTimeSpan()
        {
            TimeZoneInformation[] tzs = TimeZoneInformation.EnumZones();
            foreach (TimeZoneInformation ti in tzs)
            {
                string tz = ti.CurrentUtcBiasString;
                TimeSpan span = TimeSpan.Zero;
                Assert.IsTrue(TimeZoneInformation.TryParseTimezoneOffsetToTimeSpan(tz, out span),
                    string.Format("Error parsing {0}", tz));
                Console.WriteLine("{0}: {1} - {2}", ti.DisplayName, tz, span);
                Assert.AreEqual(span, ti.CurrentUtcBias);
            }
        }
    }
}
