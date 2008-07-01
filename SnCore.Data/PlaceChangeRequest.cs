using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'PlaceChangeRequest' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'PlaceChangeRequest' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class PlaceChangeRequest : IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private Account m_Account;
        private City m_City;
        private System.DateTime m_Created;
        private System.String m_CrossStreet;
        private System.String m_Description;
        private System.String m_Email;
        private System.String m_Fax;
        private System.DateTime m_Modified;
        private System.String m_Name;
        private Neighborhood m_Neighborhood;
        private System.String m_Phone;
        private Place m_Place;
        private System.String m_Street;
        private PlaceType m_Type;
        private System.String m_Zip;

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
        ///The property maps to the column 'PlaceChangeRequest_Id' in the data source.
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
        ///The inverse property for this property is 'Account.PlaceChangeRequests'.
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
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'City'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'City.PlaceChangeRequests'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_City' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'City_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public City City
        {
            get
            {
                return m_City;
            }
            set
            {
                m_City = value;
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
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_CrossStreet' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'CrossStreet' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.String CrossStreet
        {
            get
            {
                return m_CrossStreet;
            }
            set
            {
                m_CrossStreet = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Description' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Description' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.String Description
        {
            get
            {
                return m_Description;
            }
            set
            {
                m_Description = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Email' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Email' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.String Email
        {
            get
            {
                return m_Email;
            }
            set
            {
                m_Email = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Fax' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Fax' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.String Fax
        {
            get
            {
                return m_Fax;
            }
            set
            {
                m_Fax = value;
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
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Name' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Name' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.String Name
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
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Neighborhood'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Neighborhood.PlaceChangeRequests'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Neighborhood' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Neighborhood_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public Neighborhood Neighborhood
        {
            get
            {
                return m_Neighborhood;
            }
            set
            {
                m_Neighborhood = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Phone' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Phone' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.String Phone
        {
            get
            {
                return m_Phone;
            }
            set
            {
                m_Phone = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Place'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Place.PlaceChangeRequests'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Place' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Place_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public Place Place
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
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Street' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Street' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.String Street
        {
            get
            {
                return m_Street;
            }
            set
            {
                m_Street = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'PlaceType'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'PlaceType.PlaceChangeRequests'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Type' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Type' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public PlaceType Type
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

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Zip' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Zip' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        virtual public System.String Zip
        {
            get
            {
                return m_Zip;
            }
            set
            {
                m_Zip = value;
            }
        }

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
