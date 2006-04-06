using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'Reminder' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'Reminder' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class Reminder
    {
#region " : Generated Code Region "
        //Private field variables

        //Holds property values
        private System.Int32 m_Id;
        private System.String m_Body;
        private DataObject m_DataObject;
        private System.String m_DataObjectField;
        private System.Int32 m_DeltaHours;
        private System.Boolean m_Enabled;
        private System.DateTime m_LastRun;
        private System.String m_LastRunError;
        private System.Boolean m_Recurrent;
        private System.Collections.IList m_ReminderEvents;
        private System.String m_Subject;

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
        ///The property maps to the column 'Reminder_Id' in the data source.
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
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Body' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Body' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String Body
        {
            get
            {
                return m_Body;
            }
            set
            {
                m_Body = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'DataObject'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'DataObject.Reminders'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_DataObject' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'DataObject_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  DataObject DataObject
        {
            get
            {
                return m_DataObject;
            }
            set
            {
                m_DataObject = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_DataObjectField' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'DataObjectField' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String DataObjectField
        {
            get
            {
                return m_DataObjectField;
            }
            set
            {
                m_DataObjectField = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_DeltaHours' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'DeltaHours' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Int32 DeltaHours
        {
            get
            {
                return m_DeltaHours;
            }
            set
            {
                m_DeltaHours = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Enabled' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Enabled' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Boolean Enabled
        {
            get
            {
                return m_Enabled;
            }
            set
            {
                m_Enabled = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.DateTime'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_LastRun' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'LastRun' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.DateTime LastRun
        {
            get
            {
                return m_LastRun;
            }
            set
            {
                m_LastRun = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_LastRunError' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'LastRunError' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String LastRunError
        {
            get
            {
                return m_LastRunError;
            }
            set
            {
                m_LastRunError = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Recurrent' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Recurrent' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Boolean Recurrent
        {
            get
            {
                return m_Recurrent;
            }
            set
            {
                m_Recurrent = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'ReminderEvent'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'ReminderEvent.Reminder'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_ReminderEvents' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///
        ///Mapping information:
        ///This class maps to the 'ReminderEvent' table in the data source.
        ///The property maps to the identity column 'Reminder_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList ReminderEvents
        {
            get
            {
                return m_ReminderEvents;
            }
            set
            {
                m_ReminderEvents = value;
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

#endregion //: Generated Code Region

        //Add your synchronized custom code here:
#region " : Synchronized Custom Code Region "
#endregion //: Synchronized Custom Code Region

        //Add your unsynchronized custom code here:
#region " : Unsynchronized Custom Code Region "



#endregion //: Unsynchronized Custom Code Region

    }
