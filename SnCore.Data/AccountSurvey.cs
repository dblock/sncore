using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'AccountSurvey' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'AccountSurvey' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class AccountSurvey
    {
#region " : Generated Code Region "
        //Private field variables

        //Holds property values
        private System.Int32 m_Id;
        private System.Collections.IList m_AccountSurveyQuestions;
        private System.String m_Name;

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
        ///The property maps to the column 'AccountSurvey_Id' in the data source.
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
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountSurveyQuestion'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountSurveyQuestion.AccountSurvey'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountSurveyQuestions' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountSurveyQuestion' table in the data source.
        ///The property maps to the identity column 'AccountSurvey_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountSurveyQuestions
        {
            get
            {
                return m_AccountSurveyQuestions;
            }
            set
            {
                m_AccountSurveyQuestions = value;
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
        public  System.String Name
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

#endregion //: Generated Code Region

        //Add your synchronized custom code here:
#region " : Synchronized Custom Code Region "
#endregion //: Synchronized Custom Code Region

        //Add your unsynchronized custom code here:
#region " : Unsynchronized Custom Code Region "



#endregion //: Unsynchronized Custom Code Region

    }
