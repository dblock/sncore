using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'ReminderAccountProperty' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'ReminderAccountProperty' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class ReminderAccountProperty: IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private AccountProperty m_AccountProperty;
        private Reminder m_Reminder;
        private System.Boolean m_Unset;
        private System.String m_Value;

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
        ///The property maps to the column 'ReminderAccountProperty_Id' in the data source.
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
        ///This property accepts references to objects of the type 'AccountProperty'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'AccountProperty.ReminderAccountProperties'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountProperty' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'AccountProperty_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual AccountProperty AccountProperty
        {
            get
            {
                return m_AccountProperty;
            }
            set
            {
                m_AccountProperty = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Reminder'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Reminder.ReminderAccountProperties'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Reminder' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Reminder_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual Reminder Reminder
        {
            get
            {
                return m_Reminder;
            }
            set
            {
                m_Reminder = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Unset' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Unset' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Boolean Unset
        {
            get
            {
                return m_Unset;
            }
            set
            {
                m_Unset = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Value' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Value' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.String Value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
