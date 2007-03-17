using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

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
            return ContainsId(item, "Id");
        }

        public bool ContainsId(TransitType item, string propertyname)
        {
            return ContainsId((int)item.GetType().GetProperty(propertyname).GetValue(item, null));
        }

        public bool ContainsId(int id)
        {
            return ContainsId(id, "Id");
        }

        public bool ContainsId(int id, string propertyname)
        {
            TransitType result;
            return ContainsId(id, propertyname, out result);
        }

        public bool ContainsId(int id, string propertyname, out TransitType result)
        {
            result = default(TransitType);

            if (mCollection == null)
                return false;

            foreach (TransitType instance in mCollection)
            {
                PropertyInfo pi = instance.GetType().GetProperty(propertyname);
                if (pi == null)
                {
                    throw new Exception(string.Format("Object of type {0} doesn't have a property named {1}.",
                        instance.GetType().Name, propertyname));
                }
                if (id == (int)pi.GetValue(instance, null))
                {
                    result = instance;
                    return true;
                }
            }
            return false;
        }
    }
}
