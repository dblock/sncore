using System;
using System.Collections.Generic;
using System.Text;

namespace SnCore.Web.Soap.Tests
{
    public class TransitServiceCollection<TransitType>
    {
        private IEnumerable<TransitType> mCollection = null;

        public TransitServiceCollection()
        {

        }

        public IEnumerable<TransitType> Collection
        {
            get
            {
                return mCollection;
            }
            set
            {
                mCollection = value;
            }
        }

        public TransitServiceCollection(IEnumerable<TransitType> value)
        {
            mCollection = value;
        }

        public bool Contains(TransitType item)
        {
            return ContainsId((int) item.GetType().GetProperty("Id").GetValue(item, null));
        }

        public bool ContainsId(int id)
        {
            return ContainsId(id, "Id");
        }

        public bool ContainsId(int id, string propertyname)
        {
            if (mCollection == null)
                return false;

            foreach (object instance in mCollection)
                if (id == (int) instance.GetType().GetProperty(propertyname).GetValue(instance, null))
                    return true;

            return false;
        }
    }
}
