/* 
  	* AtomContentTest.cs
	* [ part of Atom.NET library: http://atomnet.sourceforge.net ]
	* Author: Lawrence Oluyede
	* License: BSD-License (see below)
    
	Copyright (c) 2003, 2004 Lawrence Oluyede
    All rights reserved.

    Redistribution and use in source and binary forms, with or without
    modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
    * this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright
    * notice, this list of conditions and the following disclaimer in the
    * documentation and/or other materials provided with the distribution.
    * Neither the name of the copyright owner nor the names of its
    * contributors may be used to endorse or promote products derived from
    * this software without specific prior written permission.

    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
    AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
    IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
    ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
    LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
    CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
    SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
    INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
    CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
    ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
    POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using NUnit.Framework;
using Atom.Core;
using Atom.Core.Collections;
using Atom.Utils;

namespace Atom.Core.Test
{
	[TestFixture]
	public class AtomContentTest
	{
		[Test]
		[ExpectedException(typeof(OnlyOneMultipartContentAllowedException))]
		public void TestMultipleMultipartsContent()
		{
            AtomEntry entry = new AtomEntry(new Uri("http://purl.org/atom/ns#"));

            AtomContent content = new AtomContent(new Uri("http://purl.org/atom/ns#"));
			content.Content = "sample text";
			content.Mode = Mode.Xml;
			content.Type = MediaType.MultipartAlternative;
			content.XmlLang = Language.en_US;

			entry.Contents.Add(content);

            content = new AtomContent(new Uri("http://purl.org/atom/ns#"));
			content.Content = "sample text";
			content.Mode = Mode.Xml;
			content.Type = MediaType.MultipartAlternative;
			content.XmlLang = Language.en_US;
			entry.Contents.Add(content);
		}

		[Test]
		public void TestXmlBaseContentUri()
		{
            AtomFeed feed = AtomFeed.Load(@"..\..\tests\feeds\pilgrim.xml", new Uri("http://purl.org/atom/ns#"));
			foreach(AtomEntry entry in feed.Entries)
				foreach(AtomContent content in entry.Contents)
				{
					Assert.IsNotNull(content.XmlBase);
					Assert.IsTrue(content.XmlBase.ToString().StartsWith("http://diveintomark.org/"));
				}
		}
	}
}
