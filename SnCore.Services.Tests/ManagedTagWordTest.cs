using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace SnCore.Services.Tests
{
    [TestFixture]
    public class ManagedTagWordTest : ManagedCRUDTest<TagWord, TransitTagWord, ManagedTagWord>
    {
        public ManagedTagWordTest()
        {

        }

        public override TransitTagWord GetTransitInstance()
        {
            TransitTagWord t_instance = new TransitTagWord();
            t_instance.Excluded = false;
            t_instance.Frequency = 1;
            t_instance.Promoted = false;
            t_instance.Word = Guid.NewGuid().ToString();
            return t_instance;
        }
    }
}
