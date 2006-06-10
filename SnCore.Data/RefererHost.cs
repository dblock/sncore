using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'RefererHost' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'RefererHost' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class RefererHost
    {
#region " Generated Code Region "
        //Private field variables

        //Holds property values
        private System.Int32 m_Id;
        private System.DateTime m_Created;
        private System.String m_Host;
        private System.String m_LastRefererUri;
        private System.String m_LastRequestUri;
        private System.Int64 m_Total;
        private System.DateTime m_Updated;
        private System.Collections.IList m_RefererHostDups;
        private System.Collections.IList m_RefererAccounts;

        //Public properties
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
        ///The property maps to the column 'RefererHost_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Int32 Id
        {
            get
            {
                return m_Id;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.DateTime'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Created' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Created' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.DateTime Created
        {
            get
            {
                return m_Created;
            }
            set
            {
                m_Created = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Host' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Host' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String Host
        {
            get
            {
                return m_Host;
            }
            set
            {
                m_Host = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_LastRefererUri' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'LastRefererUri' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String LastRefererUri
        {
            get
            {
                return m_LastRefererUri;
            }
            set
            {
                m_LastRefererUri = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_LastRequestUri' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'LastRequestUri' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String LastRequestUri
        {
            get
            {
                return m_LastRequestUri;
            }
            set
            {
                m_LastRequestUri = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Int64'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Total' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Total' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Int64 Total
        {
            get
            {
                return m_Total;
            }
            set
            {
                m_Total = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.DateTime'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Updated' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Updated' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.DateTime Updated
        {
            get
            {
                return m_Updated;
            }
            set
            {
                m_Updated = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'RefererHostDup'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'RefererHostDup.RefererHost'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_RefererHostDups' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'RefererHostDup' table in the data source.
        ///The property maps to the identity column 'RefererHost_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList RefererHostDups
        {
            get
            {
                return m_RefererHostDups;
            }
            set
            {
                m_RefererHostDups = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'RefererAccount'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'RefererAccount.RefererHost'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_RefererAccounts' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'RefererAccount' table in the data source.
        ///The property maps to the identity column 'RefererHost_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList RefererAccounts
        {
            get
            {
                return m_RefererAccounts;
            }
            set
            {
                m_RefererAccounts = value;
            }
        }

#endregion //Generated Code Region

        //Add your synchronized custom code here:
#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

        //Add your unsynchronized custom code here:
#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
