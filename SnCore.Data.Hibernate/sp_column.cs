using System;

namespace SnCore.Data.Hibernate
{
    public class sp_column
    {
        public sp_column()
        {

        }

        private System.String mTYPE_NAME;

        public string TYPE_NAME
        {
            get
            {
                return mTYPE_NAME;
            }
            set
            {
                mTYPE_NAME = value;
            }
        }

        private System.String mTABLE_NAME;

        public string TABLE_NAME
        {
            get
            {
                return mTABLE_NAME;
            }
            set
            {
                mTABLE_NAME = value;
            }
        }

        private System.String mTABLE_QUALIFIER;

        public string TABLE_QUALIFIER
        {
            get
            {
                return mTABLE_QUALIFIER;
            }
            set
            {
                mTABLE_QUALIFIER = value;
            }
        }

        private System.String mTABLE_OWNER;

        public string TABLE_OWNER
        {
            get
            {
                return mTABLE_OWNER;
            }
            set
            {
                mTABLE_OWNER = value;
            }
        }

        private System.String mCOLUMN_NAME;

        public string COLUMN_NAME
        {
            get
            {
                return mCOLUMN_NAME;
            }
            set
            {
                mCOLUMN_NAME = value;
            }
        }

        private System.Int16 mDATA_TYPE;

        public short DATA_TYPE
        {
            get
            {
                return mDATA_TYPE;
            }
            set
            {
                mDATA_TYPE = value;
            }
        }

        private System.Int32 mPRECISION;

        public int PRECISION
        {
            get
            {
                return mPRECISION;
            }
            set
            {
                mPRECISION = value;
            }
        }

        private System.Int32 mLENGTH;

        public int LENGTH
        {
            get
            {
                return mLENGTH;
            }
            set
            {
                mLENGTH = value;
            }
        }

        public int MaxLengthInChars
        {
            get
            {
                switch (TYPE_NAME)
                {
                    case "nvarchar":
                    case "ntext":
                    case "nchar":
                        return LENGTH / 2;                        
                }

                return LENGTH;
            }
        }

        private System.Int16 mSCALE;

        public short SCALE
        {
            get
            {
                return mSCALE;
            }
            set
            {
                mSCALE = value;
            }
        }

        private System.Int16 mRADIX;

        public short RADIX
        {
            get
            {
                return mRADIX;
            }
            set
            {
                mRADIX = value;
            }
        }

        private System.Int16 mNULLABLE;

        public short NULLABLE
        {
            get
            {
                return mNULLABLE;
            }
            set
            {
                mNULLABLE = value;
            }
        }

        private System.String mREMARKS;

        public string REMARKS
        {
            get
            {
                return mREMARKS;
            }
            set
            {
                mREMARKS = value;
            }
        }

        private System.String mCOLUMN_DEF;

        public string COLUMN_DEF
        {
            get
            {
                return mCOLUMN_DEF;
            }
            set
            {
                mCOLUMN_DEF = value;
            }
        }

        private System.Int16 mSQL_DATA_TYPE;

        public short SQL_DATA_TYPE
        {
            get
            {
                return mSQL_DATA_TYPE;
            }
            set
            {
                mSQL_DATA_TYPE = value;
            }
        }

        private System.Int16 mSQL_DATETIME_SUB;

        public short SQL_DATETIME_SUB
        {
            get
            {
                return mSQL_DATETIME_SUB;
            }
            set
            {
                mSQL_DATETIME_SUB = value;
            }
        }

        private System.Int32 mCHAR_OCTET_LENGTH;

        public int CHAR_OCTET_LENGTH
        {
            get
            {
                return mCHAR_OCTET_LENGTH;
            }
            set
            {
                mCHAR_OCTET_LENGTH = value;
            }
        }

        private System.Int32 mORDINAL_POSITION;

        public int ORDINAL_POSITION
        {
            get
            {
                return mORDINAL_POSITION;
            }
            set
            {
                mORDINAL_POSITION = value;
            }
        }

        private System.String mIS_NULLABLE;

        public string IS_NULLABLE
        {
            get
            {
                return mIS_NULLABLE;
            }
            set
            {
                mIS_NULLABLE = value;
            }
        }

        private System.Int16 mSS_DATA_TYPE;

        public short SS_DATA_TYPE
        {
            get
            {
                return mSS_DATA_TYPE;
            }
            set
            {
                mSS_DATA_TYPE = value;
            }
        }
    }
}