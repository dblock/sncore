using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'AccountEmail' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'AccountEmail' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class AccountEmail : IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private Account m_Account;
        private System.Collections.Generic.IList<AccountEmailConfirmation> m_AccountEmailConfirmations;
        private System.String m_Address;
        private System.DateTime m_Created;
        private System.DateTime m_Modified;
        private System.Boolean m_Principal;
        private System.Boolean m_Verified;
        private System.Boolean m_Failed;
        private System.String m_LastError;

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
        ///The property maps to the column 'AccountEmail_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.Int32 Id
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
        ///The inverse property for this property is 'Account.AccountEmails'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Account' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public Account Account
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
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountEmailConfirmation'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountEmailConfirmation.AccountEmail'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountEmailConfirmations' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountEmailConfirmation' table in the data source.
        ///The property maps to the identity column 'AccountEmail_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.Collections.Generic.IList<AccountEmailConfirmation> AccountEmailConfirmations
        {
            get
            {
                return m_AccountEmailConfirmations;
            }
            set
            {
                m_AccountEmailConfirmations = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Address' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Address' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.String Address
        {
            get
            {
                return m_Address;
            }
            set
            {
                m_Address = value;
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
        virtual public System.DateTime Created
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
        ///This property accepts values of the type 'System.DateTime'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Modified' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Modified' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.DateTime Modified
        {
            get
            {
                return m_Modified;
            }
            set
            {
                m_Modified = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Principal' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Principal' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.Boolean Principal
        {
            get
            {
                return m_Principal;
            }
            set
            {
                m_Principal = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Verified' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Verified' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.Boolean Verified
        {
            get
            {
                return m_Verified;
            }
            set
            {
                m_Verified = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Failed' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Failed' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.Boolean Failed
        {
            get
            {
                return m_Failed;
            }
            set
            {
                m_Failed = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_LastError' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'LastError' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.String LastError
        {
            get
            {
                return m_LastError;
            }
            set
            {
                m_LastError = value;
            }
        }

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
