using System;

namespace SnCore.Data.Hibernate
{
    public class AccountCity : IDbObject
    {
        public AccountCity()
        {

        }

        private System.Int32 mId;

        public int Id
        {
            get
            {
                return mId;
            }
            set
            {
                mId = value;
            }
        }

        private System.Int32 mTotal;

        public System.Int32 Total
        {
            get
            {
                return mTotal;
            }
            set
            {
                mTotal = value;
            }
        }

        private System.String mCity;

        public string City
        {
            get
            {
                return mCity;
            }
            set
            {
                mCity = value;
            }
        }

        private System.Int32 mState_Id;

        public System.Int32 State_Id
        {
            get
            {
                return mState_Id;
            }
            set
            {
                mState_Id = value;
            }
        }

        private System.Int32 mCountry_Id;

        public System.Int32 Country_Id
        {
            get
            {
                return mCountry_Id;
            }
            set
            {
                mCountry_Id = value;
            }
        }
    }
}