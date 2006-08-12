using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'AccountAttribute' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'AccountAttribute' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class AccountAttribute
    {
#region " Generated Code Region "
        //Private field variables

        //Holds property values
        private System.Int32 m_Id;
        private Account m_Account;
        private Attribute m_Attribute;
        private System.DateTime m_Created;
        private System.String m_Url;
        private System.String m_Value;

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
        ///The property maps to the column 'AccountAttribute_Id' in the data source.
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
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Account'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Account.AccountAttributes'.
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
        ///This property accepts references to objects of the type 'Attribute'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Attribute.AccountAttributes'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Attribute' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Attribute_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  Attribute Attribute
        {
            get
            {
                return m_Attribute;
            }
            set
            {
                m_Attribute = value;
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
        ///The accessibility level for the field 'm_Url' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Url' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String Url
        {
            get
            {
                return m_Url;
            }
            set
            {
                m_Url = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Value' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Value' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
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
