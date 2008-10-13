using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'AccountGroupPlace' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'AccountGroupPlace' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class AccountGroupPlace: IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private AccountGroup m_AccountGroup;
        private System.DateTime m_Created;
        private Place m_Place;

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
        ///The property maps to the column 'AccountGroupPlace_Id' in the data source.
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
        ///This property accepts references to objects of the type 'AccountGroup'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'AccountGroup.AccountGroupPlaces'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountGroup' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'AccountGroup_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual AccountGroup AccountGroup
        {
            get
            {
                return m_AccountGroup;
            }
            set
            {
                m_AccountGroup = value;
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
        public virtual System.DateTime Created
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
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Place'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Place.AccountGroupPlaces'.
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

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
