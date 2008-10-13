using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'FeedType' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'FeedType' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class FeedType: IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private System.Collections.Generic.IList<AccountFeed> m_AccountFeeds;
        private System.String m_Name;
        private System.Int32 m_SpanColumns;
        private System.Int32 m_SpanColumnsPreview;
        private System.Int32 m_SpanRows;
        private System.Int32 m_SpanRowsPreview;
        private System.String m_Xsl;
        private System.Boolean m_DefaultType;

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive identity property.
        ///</summary>
        ///<remarks>
        ///This property is an identity property.
        ///The identity index for this property is '0'.
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Id' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'FeedType_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Int32 Id
        {
            get
            {
                return m_Id;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountFeed'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountFeed.FeedType'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountFeeds' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountFeed' table in the data source.
        ///The property maps to the identity column 'FeedType_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Collections.Generic.IList<AccountFeed> AccountFeeds
        {
            get
            {
                return m_AccountFeeds;
            }
            set
            {
                m_AccountFeeds = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Name' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Name' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.String Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_SpanColumns' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'SpanColumns' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Int32 SpanColumns
        {
            get
            {
                return m_SpanColumns;
            }
            set
            {
                m_SpanColumns = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_SpanColumnsPreview' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'SpanColumnsPreview' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Int32 SpanColumnsPreview
        {
            get
            {
                return m_SpanColumnsPreview;
            }
            set
            {
                m_SpanColumnsPreview = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_SpanRows' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'SpanRows' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Int32 SpanRows
        {
            get
            {
                return m_SpanRows;
            }
            set
            {
                m_SpanRows = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_SpanRowsPreview' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'SpanRowsPreview' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Int32 SpanRowsPreview
        {
            get
            {
                return m_SpanRowsPreview;
            }
            set
            {
                m_SpanRowsPreview = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Xsl' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Xsl' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.String Xsl
        {
            get
            {
                return m_Xsl;
            }
            set
            {
                m_Xsl = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_DefaultType' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'DefaultType' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Boolean DefaultType
        {
            get
            {
                return m_DefaultType;
            }
            set
            {
                m_DefaultType = value;
            }
        }

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
