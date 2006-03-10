using System;

namespace Janrain.OpenId.Store
{
    class FileStore: IAssociationStore
    {
        #region IAssociationStore Members

	public byte[] AuthKey
	{
	    get {
		throw new Exception(
		    "The method or operation is not implemented.");
	    }
	} 

        public bool IsDumb
        {
	    get {
		throw new Exception(
                    "The method or operation is not implemented.");
	    }
        }

        public void StoreAssociation ( Uri serverUri, 
				       Association assoc )
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Association GetAssociation ( Uri serverUri )
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Association GetAssociation ( Uri serverUri, 
					    string handle )
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool RemoveAssociation ( Uri serverUri, 
					string handle )
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void StoreNonce ( string nonce )
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool UseNonce ( string nonce )
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
