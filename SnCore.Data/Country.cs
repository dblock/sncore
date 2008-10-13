using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'Country' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'Country' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class Country: IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private System.Collections.Generic.IList<AccountAddress> m_AccountAddresses;
        private System.Collections.Generic.IList<Account> m_Accounts;
        private System.Collections.Generic.IList<City> m_Cities;
        private System.String m_Name;
        private System.Collections.Generic.IList<State> m_States;

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
        ///The property maps to the column 'Country_Id' in the data source.
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
        ///This property accepts multiple references to objects of the type 'AccountAddress'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountAddress.Country'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountAddresses' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountAddress' table in the data source.
        ///The property maps to the identity column 'Country_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Collections.Generic.IList<AccountAddress> AccountAddresses
        {
            get
            {
                return m_AccountAddresses;
            }
            set
            {
                m_AccountAddresses = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'Account'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'Account.Country'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Accounts' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'Account' table in the data source.
        ///The property maps to the identity column 'Country_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Collections.Generic.IList<Account> Accounts
        {
            get
            {
                return m_Accounts;
            }
            set
            {
                m_Accounts = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'City'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'City.Country'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Cities' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'City' table in the data source.
        ///The property maps to the identity column 'Country_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Collections.Generic.IList<City> Cities
        {
            get
            {
                return m_Cities;
            }
            set
            {
                m_Cities = value;
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
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'State'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'State.Country'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_States' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'State' table in the data source.
        ///The property maps to the identity column 'Country_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Collections.Generic.IList<State> States
        {
            get
            {
                return m_States;
            }
            set
            {
                m_States = value;
            }
        }

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
