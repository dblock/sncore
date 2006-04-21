using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'Bug' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'Bug' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class Bug
    {
#region " Generated Code Region "
        //Private field variables

        //Holds property values
        private System.Int32 m_Id;
        private System.Int32 m_AccountId;
        private System.Collections.IList m_BugLinks;
        private System.Collections.IList m_BugNotes;
        private System.DateTime m_Created;
        private System.String m_Details;
        private BugPriority m_Priority;
        private BugProject m_Project;
        private System.Collections.IList m_RelatedBugBugLinks;
        private BugResolution m_Resolution;
        private BugSeverity m_Severity;
        private BugStatu m_Status;
        private System.String m_Subject;
        private BugType m_Type;
        private System.DateTime m_Updated;

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
        ///The property maps to the column 'Bug_Id' in the data source.
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
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountId' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Int32 AccountId
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
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'BugLink'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'BugLink.Bug'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_BugLinks' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'BugLink' table in the data source.
        ///The property maps to the identity column 'Bug_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList BugLinks
        {
            get
            {
                return m_BugLinks;
            }
            set
            {
                m_BugLinks = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'BugNote'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'BugNote.Bug'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_BugNotes' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'BugNote' table in the data source.
        ///The property maps to the identity column 'Bug_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList BugNotes
        {
            get
            {
                return m_BugNotes;
            }
            set
            {
                m_BugNotes = value;
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
        ///The accessibility level for the field 'm_Details' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Details' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String Details
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
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'BugPriority'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'BugPriority.Bugs'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Priority' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Priority_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  BugPriority Priority
        {
            get
            {
                return m_Priority;
            }
            set
            {
                m_Priority = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'BugProject'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'BugProject.Bugs'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Project' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Project_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  BugProject Project
        {
            get
            {
                return m_Project;
            }
            set
            {
                m_Project = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'BugLink'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'BugLink.RelatedBug'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_RelatedBugBugLinks' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'BugLink' table in the data source.
        ///The property maps to the identity column 'RelatedBug_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList RelatedBugBugLinks
        {
            get
            {
                return m_RelatedBugBugLinks;
            }
            set
            {
                m_RelatedBugBugLinks = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'BugResolution'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'BugResolution.Bugs'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Resolution' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Resolution_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  BugResolution Resolution
        {
            get
            {
                return m_Resolution;
            }
            set
            {
                m_Resolution = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'BugSeverity'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'BugSeverity.Bugs'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Severity' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Severity_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  BugSeverity Severity
        {
            get
            {
                return m_Severity;
            }
            set
            {
                m_Severity = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'BugStatu'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'BugStatu.Bugs'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Status' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Status_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  BugStatu Status
        {
            get
            {
                return m_Status;
            }
            set
            {
                m_Status = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Subject' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Subject' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String Subject
        {
            get
            {
                return m_Subject;
            }
            set
            {
                m_Subject = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'BugType'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'BugType.Bugs'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Type' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Type_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  BugType Type
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
        ///This property accepts values of the type 'System.DateTime'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Updated' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Updated' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.DateTime Updated
        {
            get
            {
                return m_Updated;
            }
            set
            {
                m_Updated = value;
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
