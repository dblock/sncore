using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'AccountPlaceRequest' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'AccountPlaceRequest' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class AccountPlaceRequest: IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private Account m_Account;
        private System.String m_Message;
        private Place m_Place;
        private System.DateTime m_Submitted;
        private AccountPlaceType m_Type;

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
        ///The property maps to the column 'AccountPlaceRequest_Id' in the data source.
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
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Account'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Account.AccountPlaceRequests'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Account' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual Account Account
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
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Message' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Message' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.String Message
        {
            get
            {
                return m_Message;
            }
            set
            {
                m_Message = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Place'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Place.AccountPlaceRequests'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Place' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Place_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual Place Place
        {
            get
            {
                return m_Place;
            }
            set
            {
                m_Place = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.DateTime'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Submitted' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Submitted' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.DateTime Submitted
        {
            get
            {
                return m_Submitted;
            }
            set
            {
                m_Submitted = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'AccountPlaceType'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'AccountPlaceType.AccountPlaceRequests'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Type' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Type' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual AccountPlaceType Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
