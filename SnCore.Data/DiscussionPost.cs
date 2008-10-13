using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'DiscussionPost' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'DiscussionPost' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class DiscussionPost: IDbObject
    {
#region " Generated Code Region "

        private System.Int32 m_Id;
        private System.Int32 m_AccountId;
        private System.String m_Body;
        private System.DateTime m_Created;
        private DiscussionPost m_DiscussionPostParent;
        private System.Collections.Generic.IList<DiscussionPost> m_DiscussionPosts;
        private DiscussionThread m_DiscussionThread;
        private System.DateTime m_Modified;
        private System.String m_Subject;
        private System.Boolean m_Sticky;

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
        ///The property maps to the column 'DiscussionPost_Id' in the data source.
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
        public virtual System.String Body
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
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'DiscussionPost'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'DiscussionPost.DiscussionPosts'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_DiscussionPostParent' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'DiscussionPostParent_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual DiscussionPost DiscussionPostParent
        {
            get
            {
                return m_DiscussionPostParent;
            }
            set
            {
                m_DiscussionPostParent = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'DiscussionPost'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'DiscussionPost.DiscussionPostParent'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_DiscussionPosts' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'DiscussionPost' table in the data source.
        ///The property maps to the identity column 'DiscussionPostParent_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Collections.Generic.IList<DiscussionPost> DiscussionPosts
        {
            get
            {
                return m_DiscussionPosts;
            }
            set
            {
                m_DiscussionPosts = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'DiscussionThread'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'DiscussionThread.DiscussionPosts'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_DiscussionThread' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'DiscussionThread_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual DiscussionThread DiscussionThread
        {
            get
            {
                return m_DiscussionThread;
            }
            set
            {
                m_DiscussionThread = value;
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
        public virtual System.String Subject
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
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Sticky' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Sticky' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public virtual System.Boolean Sticky
        {
            get
            {
                return m_Sticky;
            }
            set
            {
                m_Sticky = value;
            }
        }

#endregion //Generated Code Region

#region " Synchronized Custom Code Region "
#endregion //Synchronized Custom Code Region

#region " Unsynchronized Custom Code Region "



#endregion //Unsynchronized Custom Code Region

    }
