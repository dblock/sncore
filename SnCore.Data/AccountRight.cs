using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'AccountRight' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'AccountRight' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class AccountRight
    {
#region " : Generated Code Region "
        //Private field variables

        //Holds property values
        private System.Int32 m_Id;
        private System.Boolean m_AllowCreate;
        private System.Boolean m_AllowDelete;
        private System.Boolean m_AllowRetrieve;
        private System.Boolean m_AllowUpdate;
        private Account m_Account;
        private DataObject m_DataObject;

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
        ///The property maps to the column 'AccountRight_Id' in the data source.
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
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AllowCreate' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'AllowCreate' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Boolean AllowCreate
        {
            get
            {
                return m_AllowCreate;
            }
            set
            {
                m_AllowCreate = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AllowDelete' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'AllowDelete' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Boolean AllowDelete
        {
            get
            {
                return m_AllowDelete;
            }
            set
            {
                m_AllowDelete = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AllowRetrieve' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'AllowRetrieve' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Boolean AllowRetrieve
        {
            get
            {
                return m_AllowRetrieve;
            }
            set
            {
                m_AllowRetrieve = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AllowUpdate' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'AllowUpdate' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Boolean AllowUpdate
        {
            get
            {
                return m_AllowUpdate;
            }
            set
            {
                m_AllowUpdate = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Account'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Account.AccountRights'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Account' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  Account Account
        {
            get
            {
                return m_Account;
            }
            set
            {
                m_Account = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'DataObject'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'DataObject.AccountRights'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_DataObject' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'DataObject_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  DataObject DataObject
        {
            get
            {
                return m_DataObject;
            }
            set
            {
                m_DataObject = value;
            }
        }

#endregion //: Generated Code Region

        //Add your synchronized custom code here:
#region " : Synchronized Custom Code Region "
#endregion //: Synchronized Custom Code Region

        //Add your unsynchronized custom code here:
#region " : Unsynchronized Custom Code Region "



#endregion //: Unsynchronized Custom Code Region

    }
