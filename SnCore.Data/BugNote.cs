using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'BugNote' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'BugNote' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class BugNote: IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private System.Int32 m_AccountId;
        private Bug m_Bug;
        private System.DateTime m_Created;
        private System.String m_Details;
        private System.DateTime m_Modified;

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
        ///The property maps to the column 'BugNote_Id' in the data source.
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
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountId' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Int32 AccountId
        {
            get
            {
                return m_AccountId;
            }
            set
            {
                m_AccountId = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Bug'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Bug.BugNotes'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Bug' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Bug_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual Bug Bug
        {
            get
            {
                return m_Bug;
            }
            set
            {
                m_Bug = value;
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
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Details' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Details' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.String Details
        {
            get
            {
                return m_Details;
            }
            set
            {
                m_Details = value;
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
        public virtual System.DateTime Modified
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

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
