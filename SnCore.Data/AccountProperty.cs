using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'AccountProperty' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'AccountProperty' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class AccountProperty: IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private AccountPropertyGroup m_AccountPropertyGroup;
        private System.Collections.Generic.IList<AccountPropertyValue> m_AccountPropertyValues;
        private System.String m_DefaultValue;
        private System.String m_Description;
        private System.String m_Name;
        private System.String m_TypeName;
        private System.Boolean m_Publish;
        private System.Collections.Generic.IList<ReminderAccountProperty> m_ReminderAccountProperties;

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
        ///The property maps to the column 'AccountProperty_Id' in the data source.
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
        ///This property accepts references to objects of the type 'AccountPropertyGroup'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'AccountPropertyGroup.AccountProperties'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountPropertyGroup' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'AccountPropertyGroup_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual AccountPropertyGroup AccountPropertyGroup
        {
            get
            {
                return m_AccountPropertyGroup;
            }
            set
            {
                m_AccountPropertyGroup = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountPropertyValue'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountPropertyValue.AccountProperty'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountPropertyValues' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountPropertyValue' table in the data source.
        ///The property maps to the identity column 'AccountProperty_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Collections.Generic.IList<AccountPropertyValue> AccountPropertyValues
        {
            get
            {
                return m_AccountPropertyValues;
            }
            set
            {
                m_AccountPropertyValues = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_DefaultValue' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'DefaultValue' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.String DefaultValue
        {
            get
            {
                return m_DefaultValue;
            }
            set
            {
                m_DefaultValue = value;
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
        public virtual System.String Description
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
        ///The accessibility level for the field 'm_Name' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Name' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.String Name
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
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_TypeName' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'TypeName' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.String TypeName
        {
            get
            {
                return m_TypeName;
            }
            set
            {
                m_TypeName = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Publish' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Publish' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Boolean Publish
        {
            get
            {
                return m_Publish;
            }
            set
            {
                m_Publish = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'ReminderAccountProperty'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'ReminderAccountProperty.AccountProperty'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_ReminderAccountProperties' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'ReminderAccountProperty' table in the data source.
        ///The property maps to the identity column 'AccountProperty_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Collections.Generic.IList<ReminderAccountProperty> ReminderAccountProperties
        {
            get
            {
                return m_ReminderAccountProperties;
            }
            set
            {
                m_ReminderAccountProperties = value;
            }
        }

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
