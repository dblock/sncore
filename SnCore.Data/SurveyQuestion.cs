using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'SurveyQuestion' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'SurveyQuestion' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class SurveyQuestion
    {
#region " : Generated Code Region "
        //Private field variables

        //Holds property values
        private System.Int32 m_Id;
        private System.Collections.IList m_AccountSurveyAnswers;
        private System.String m_Question;
        private Survey m_Survey;

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
        ///The property maps to the column 'SurveyQuestion_Id' in the data source.
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
        ///This property accepts multiple references to objects of the type 'AccountSurveyAnswer'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountSurveyAnswer.SurveyQuestion'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountSurveyAnswers' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountSurveyAnswer' table in the data source.
        ///The property maps to the identity column 'SurveyQuestion_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountSurveyAnswers
        {
            get
            {
                return m_AccountSurveyAnswers;
            }
            set
            {
                m_AccountSurveyAnswers = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Question' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Question' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String Question
        {
            get
            {
                return m_Question;
            }
            set
            {
                m_Question = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Survey'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Survey.SurveyQuestions'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Survey' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Survey_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  Survey Survey
        {
            get
            {
                return m_Survey;
            }
            set
            {
                m_Survey = value;
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
