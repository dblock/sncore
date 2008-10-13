using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'Configuration' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'Configuration' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class Configuration: IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private System.String m_OptionName;
        private System.String m_OptionValue;
        private System.Boolean m_Password;

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
        ///The property maps to the column 'Configuration_Id' in the data source.
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
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_OptionName' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'OptionName' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.String OptionName
        {
            get
            {
                return m_OptionName;
            }
            set
            {
                m_OptionName = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_OptionValue' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'OptionValue' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.String OptionValue
        {
            get
            {
                return m_OptionValue;
            }
            set
            {
                m_OptionValue = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Password' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Password' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Boolean Password
        {
            get
            {
                return m_Password;
            }
            set
            {
                m_Password = value;
            }
        }

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
