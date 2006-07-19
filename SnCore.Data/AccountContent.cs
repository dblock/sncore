using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'AccountContent' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'AccountContent' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class AccountContent
    {
#region " Generated Code Region "
        //Private field variables

        //Holds property values
        private System.Int32 m_Id;
        private AccountContentGroup m_AccountContentGroup;
        private System.DateTime m_Created;
        private System.DateTime m_Modified;
        private System.Int32 m_Position;
        private System.String m_Tag;
        private System.String m_Text;

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
        ///The property maps to the column 'AccountContent_Id' in the data source.
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
        ///This property accepts references to objects of the type 'AccountContentGroup'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'AccountContentGroup.AccountContents'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountContentGroup' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'AccountContentGroup_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  AccountContentGroup AccountContentGroup
        {
            get
            {
                return m_AccountContentGroup;
            }
            set
            {
                m_AccountContentGroup = value;
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
        ///This property accepts values of the type 'System.DateTime'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Modified' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Modified' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.DateTime Modified
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
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Position' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Position' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Int32 Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Tag' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Tag' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String Tag
        {
            get
            {
                return m_Tag;
            }
            set
            {
                m_Tag = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Text' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Text' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String Text
        {
            get
            {
                return m_Text;
            }
            set
            {
                m_Text = value;
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
