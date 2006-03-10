using System;
using System.Text;

namespace Janrain.OpenId.Store
{
    public interface IAssociationStore
    {
        byte[] AuthKey { get; }
        bool IsDumb { get; }

        void StoreAssociation(Uri serverUri, Association assoc);
        Association GetAssociation(Uri serverUri);
        Association GetAssociation(Uri serverUri, string handle);
        bool RemoveAssociation(Uri serverUri, string handle);
        void StoreNonce(string nonce);
        bool UseNonce(string nonce);
    }
}
