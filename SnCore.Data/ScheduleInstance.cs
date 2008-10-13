using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'ScheduleInstance' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'ScheduleInstance' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class ScheduleInstance: IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private System.DateTime m_Created;
        private System.DateTime m_EndDateTime;
        private System.Int32 m_Instance;
        private System.DateTime m_Modified;
        private Schedule m_Schedule;
        private System.DateTime m_StartDateTime;

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
        ///The property maps to the column 'ScheduleInstance_Id' in the data source.
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
        ///This property accepts values of the type 'System.DateTime'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_EndDateTime' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'EndDateTime' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.DateTime EndDateTime
        {
            get
            {
                return m_EndDateTime;
            }
            set
            {
                m_EndDateTime = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Instance' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Instance' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Int32 Instance
        {
            get
            {
                return m_Instance;
            }
            set
            {
                m_Instance = value;
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

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Schedule'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Schedule.ScheduleInstances'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Schedule' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Schedule_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual Schedule Schedule
        {
            get
            {
                return m_Schedule;
            }
            set
            {
                m_Schedule = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.DateTime'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_StartDateTime' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'StartDateTime' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.DateTime StartDateTime
        {
            get
            {
                return m_StartDateTime;
            }
            set
            {
                m_StartDateTime = value;
            }
        }

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
