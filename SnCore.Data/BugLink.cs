using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'BugLink' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'BugLink' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class BugLink
    {
#region " : Generated Code Region "
        //Private field variables

        //Holds property values
        private System.Int32 m_Id;
        private Bug m_Bug;
        private Bug m_RelatedBug;

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
        ///The property maps to the column 'BugLink_Id' in the data source.
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
        ///This property accepts references to objects of the type 'Bug'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Bug.BugLinks'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Bug' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Bug_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  Bug Bug
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
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Bug'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Bug.RelatedBugBugLinks'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_RelatedBug' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'RelatedBug_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  Bug RelatedBug
        {
            get
            {
                return m_RelatedBug;
            }
            set
            {
                m_RelatedBug = value;
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
