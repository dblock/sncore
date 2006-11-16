using System;
    ///--------------------------------------------------------------------------------
    ///<summary>
    ///Persistent domain entity class representing 'Account' entities.
    ///</summary>
    ///<remarks>
    ///
    ///Mapping information:
    ///This class maps to the 'Account' table in the data source.
    ///</remarks>
    ///--------------------------------------------------------------------------------
    public class Account
    {
#region " Generated Code Region "
        //Private field variables

        //Holds property values
        private System.Int32 m_Id;
        private System.Collections.IList m_AccountAddresses;
        private System.Collections.IList m_AccountBlogAuthors;
        private System.Collections.IList m_AccountBlogs;
        private System.Collections.IList m_AccountEmailMessages;
        private System.Collections.IList m_AccountEmails;
        private System.Collections.IList m_AccountEvents;
        private System.Collections.IList m_AccountFeeds;
        private System.Collections.IList m_AccountFriendRequests;
        private System.Collections.IList m_AccountFriends;
        private System.Collections.IList m_AccountInvitations;
        private System.Collections.IList m_AccountMessageFolders;
        private System.Collections.IList m_AccountMessages;
        private System.Collections.IList m_AccountOpenIds;
        private System.Collections.IList m_AccountPictures;
        private System.Collections.IList m_AccountPlaceFavorites;
        private System.Collections.IList m_AccountPlaceRequests;
        private System.Collections.IList m_AccountPlaces;
        private System.Collections.IList m_AccountStories;
        private System.Collections.IList m_AccountSurveyAnswers;
        private System.Collections.IList m_AccountWebsites;
        private System.DateTime m_Birthday;
        private System.String m_City;
        private Country m_Country;
        private System.DateTime m_Created;
        private System.Collections.IList m_Discussions;
        private System.Boolean m_Enabled;
        private System.Boolean m_IsAdministrator;
        private System.Collections.IList m_KeenAccountFriendRequests;
        private System.Collections.IList m_KeenAccountFriends;
        private System.DateTime m_LastLogin;
        private System.DateTime m_Modified;
        private System.String m_Name;
        private System.String m_Password;
        private System.Collections.IList m_Places;
        private System.Collections.IList m_ReminderEvents;
        private System.String m_Signature;
        private State m_State;
        private System.Collections.IList m_Schedules;
        private System.Collections.IList m_RefererAccounts;
        private System.Collections.IList m_AccountContentGroups;
        private System.Collections.IList m_AccountPropertyValues;
        private System.Collections.IList m_AccountAttributes;
        private System.Collections.IList m_AccountLicenses;
        private System.Boolean m_IsPasswordExpired;
        private System.Collections.IList m_CampaignAccountRecepients;
        private System.Int32 m_TimeZone;
        private System.Collections.IList m_MadLibs;
        private System.Collections.IList m_PlaceQueues;
        private System.Collections.IList m_PlacePictures;

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
        ///The property maps to the column 'Account_Id' in the data source.
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
        ///This property accepts multiple references to objects of the type 'AccountAddress'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountAddress.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountAddresses' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountAddress' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountAddresses
        {
            get
            {
                return m_AccountAddresses;
            }
            set
            {
                m_AccountAddresses = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountBlogAuthor'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountBlogAuthor.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountBlogAuthors' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountBlogAuthor' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountBlogAuthors
        {
            get
            {
                return m_AccountBlogAuthors;
            }
            set
            {
                m_AccountBlogAuthors = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountBlog'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountBlog.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountBlogs' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountBlog' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountBlogs
        {
            get
            {
                return m_AccountBlogs;
            }
            set
            {
                m_AccountBlogs = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountEmailMessage'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountEmailMessage.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountEmailMessages' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountEmailMessage' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountEmailMessages
        {
            get
            {
                return m_AccountEmailMessages;
            }
            set
            {
                m_AccountEmailMessages = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountEmail'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountEmail.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountEmails' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountEmail' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountEmails
        {
            get
            {
                return m_AccountEmails;
            }
            set
            {
                m_AccountEmails = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountEvent'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountEvent.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountEvents' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountEvent' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountEvents
        {
            get
            {
                return m_AccountEvents;
            }
            set
            {
                m_AccountEvents = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountFeed'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountFeed.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountFeeds' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountFeed' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountFeeds
        {
            get
            {
                return m_AccountFeeds;
            }
            set
            {
                m_AccountFeeds = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountFriendRequest'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountFriendRequest.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountFriendRequests' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountFriendRequest' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountFriendRequests
        {
            get
            {
                return m_AccountFriendRequests;
            }
            set
            {
                m_AccountFriendRequests = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountFriend'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountFriend.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountFriends' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountFriend' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountFriends
        {
            get
            {
                return m_AccountFriends;
            }
            set
            {
                m_AccountFriends = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountInvitation'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountInvitation.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountInvitations' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountInvitation' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountInvitations
        {
            get
            {
                return m_AccountInvitations;
            }
            set
            {
                m_AccountInvitations = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountMessageFolder'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountMessageFolder.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountMessageFolders' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountMessageFolder' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountMessageFolders
        {
            get
            {
                return m_AccountMessageFolders;
            }
            set
            {
                m_AccountMessageFolders = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountMessage'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountMessage.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountMessages' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountMessage' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountMessages
        {
            get
            {
                return m_AccountMessages;
            }
            set
            {
                m_AccountMessages = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountOpenId'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountOpenId.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountOpenIds' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountOpenId' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountOpenIds
        {
            get
            {
                return m_AccountOpenIds;
            }
            set
            {
                m_AccountOpenIds = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountPicture'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountPicture.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountPictures' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountPicture' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountPictures
        {
            get
            {
                return m_AccountPictures;
            }
            set
            {
                m_AccountPictures = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountPlaceFavorite'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountPlaceFavorite.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountPlaceFavorites' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountPlaceFavorite' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountPlaceFavorites
        {
            get
            {
                return m_AccountPlaceFavorites;
            }
            set
            {
                m_AccountPlaceFavorites = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountPlaceRequest'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountPlaceRequest.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountPlaceRequests' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountPlaceRequest' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountPlaceRequests
        {
            get
            {
                return m_AccountPlaceRequests;
            }
            set
            {
                m_AccountPlaceRequests = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountPlace'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountPlace.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountPlaces' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountPlace' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountPlaces
        {
            get
            {
                return m_AccountPlaces;
            }
            set
            {
                m_AccountPlaces = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountStory'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountStory.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountStories' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountStory' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountStories
        {
            get
            {
                return m_AccountStories;
            }
            set
            {
                m_AccountStories = value;
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
        ///The inverse property for this property is 'AccountSurveyAnswer.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountSurveyAnswers' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountSurveyAnswer' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
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
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountWebsite'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountWebsite.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountWebsites' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountWebsite' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountWebsites
        {
            get
            {
                return m_AccountWebsites;
            }
            set
            {
                m_AccountWebsites = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.DateTime'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Birthday' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Birthday' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.DateTime Birthday
        {
            get
            {
                return m_Birthday;
            }
            set
            {
                m_Birthday = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_City' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'City' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String City
        {
            get
            {
                return m_City;
            }
            set
            {
                m_City = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'Country'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'Country.Accounts'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Country' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Country_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  Country Country
        {
            get
            {
                return m_Country;
            }
            set
            {
                m_Country = value;
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
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'Discussion'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'Discussion.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Discussions' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'Discussion' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList Discussions
        {
            get
            {
                return m_Discussions;
            }
            set
            {
                m_Discussions = value;
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
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_IsAdministrator' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'IsAdministrator' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Boolean IsAdministrator
        {
            get
            {
                return m_IsAdministrator;
            }
            set
            {
                m_IsAdministrator = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountFriendRequest'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountFriendRequest.Keen'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_KeenAccountFriendRequests' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountFriendRequest' table in the data source.
        ///The property maps to the identity column 'Keen_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList KeenAccountFriendRequests
        {
            get
            {
                return m_KeenAccountFriendRequests;
            }
            set
            {
                m_KeenAccountFriendRequests = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountFriend'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountFriend.Keen'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_KeenAccountFriends' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountFriend' table in the data source.
        ///The property maps to the identity column 'Keen_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList KeenAccountFriends
        {
            get
            {
                return m_KeenAccountFriends;
            }
            set
            {
                m_KeenAccountFriends = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.DateTime'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_LastLogin' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'LastLogin' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.DateTime LastLogin
        {
            get
            {
                return m_LastLogin;
            }
            set
            {
                m_LastLogin = value;
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
        public  System.DateTime Modified
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

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.String'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Password' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Password' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String Password
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

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'Place'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'Place.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Places' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'Place' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList Places
        {
            get
            {
                return m_Places;
            }
            set
            {
                m_Places = value;
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
        ///The inverse property for this property is 'ReminderEvent.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_ReminderEvents' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'ReminderEvent' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
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
        ///The accessibility level for the field 'm_Signature' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'Signature' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.String Signature
        {
            get
            {
                return m_Signature;
            }
            set
            {
                m_Signature = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent one-many reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts references to objects of the type 'State'.
        ///This property is part of a 'OneToMany' relationship.
        ///The inverse property for this property is 'State.Accounts'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_State' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'State_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  State State
        {
            get
            {
                return m_State;
            }
            set
            {
                m_State = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'Schedule'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'Schedule.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_Schedules' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as Read-Only.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'Schedule' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList Schedules
        {
            get
            {
                return m_Schedules;
            }
            set
            {
                m_Schedules = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'RefererAccount'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'RefererAccount.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_RefererAccounts' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'RefererAccount' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList RefererAccounts
        {
            get
            {
                return m_RefererAccounts;
            }
            set
            {
                m_RefererAccounts = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountContentGroup'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountContentGroup.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountContentGroups' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountContentGroup' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountContentGroups
        {
            get
            {
                return m_AccountContentGroups;
            }
            set
            {
                m_AccountContentGroups = value;
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
        ///The inverse property for this property is 'AccountPropertyValue.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountPropertyValues' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountPropertyValue' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountPropertyValues
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
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountAttribute'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountAttribute.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountAttributes' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountAttribute' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountAttributes
        {
            get
            {
                return m_AccountAttributes;
            }
            set
            {
                m_AccountAttributes = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'AccountLicense'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'AccountLicense.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_AccountLicenses' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'AccountLicense' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList AccountLicenses
        {
            get
            {
                return m_AccountLicenses;
            }
            set
            {
                m_AccountLicenses = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Boolean'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_IsPasswordExpired' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'IsPasswordExpired' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Boolean IsPasswordExpired
        {
            get
            {
                return m_IsPasswordExpired;
            }
            set
            {
                m_IsPasswordExpired = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'CampaignAccountRecepient'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'CampaignAccountRecepient.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_CampaignAccountRecepients' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'CampaignAccountRecepient' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList CampaignAccountRecepients
        {
            get
            {
                return m_CampaignAccountRecepients;
            }
            set
            {
                m_CampaignAccountRecepients = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent primitive property.
        ///</summary>
        ///<remarks>
        ///This property accepts values of the type 'System.Int32'.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_TimeZone' that holds the value for this property is 'PrivateAccess'.
        ///
        ///Mapping information:
        ///The property maps to the column 'TimeZone' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Int32 TimeZone
        {
            get
            {
                return m_TimeZone;
            }
            set
            {
                m_TimeZone = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'MadLib'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'MadLib.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_MadLibs' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'MadLib' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList MadLibs
        {
            get
            {
                return m_MadLibs;
            }
            set
            {
                m_MadLibs = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'PlaceQueue'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'PlaceQueue.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_PlaceQueues' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'PlaceQueue' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList PlaceQueues
        {
            get
            {
                return m_PlaceQueues;
            }
            set
            {
                m_PlaceQueues = value;
            }
        }

        ///--------------------------------------------------------------------------------
        ///<summary>
        ///Persistent many-one reference property.
        ///</summary>
        ///<remarks>
        ///This property accepts multiple references to objects of the type 'PlacePicture'.
        ///This property is part of a 'ManyToOne' relationship.
        ///The data type for this property is 'System.Collections.IList'.
        ///The inverse property for this property is 'PlacePicture.Account'.
        ///This property inherits its mapping information from its inverse property.
        ///The accessibility level for this property is 'PublicAccess'.
        ///The accessibility level for the field 'm_PlacePictures' that holds the value for this property is 'PrivateAccess'.
        ///This property is marked as slave.
        ///
        ///Mapping information:
        ///This class maps to the 'PlacePicture' table in the data source.
        ///The property maps to the identity column 'Account_Id' in the data source.
        ///</remarks>
        ///--------------------------------------------------------------------------------
        public  System.Collections.IList PlacePictures
        {
            get
            {
                return m_PlacePictures;
            }
            set
            {
                m_PlacePictures = value;
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
