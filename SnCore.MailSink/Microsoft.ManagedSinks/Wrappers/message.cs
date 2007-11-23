//+------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// File: message.cs
//
// Contents: Mailmsg wrapper
//
// Classes: Message
//
// Functions:
//
//-------------------------------------------------------------
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using Microsoft.Exchange.Transport.EventInterop;

namespace Microsoft.Exchange.Transport.EventWrappers
{
    //
    // Managed wrapper for mailmsg.
    //
    public class Message : 
        PropertyAccessor
    {
        //
        // public methods:
        //

        //
        // CoCreate an in-memory mailmsg from scratch.
        //
        public Message()
        {
            pMsg = (IMailMsgProperties) new MailMsgClass();
            this.PrivateRecips = new RecipCollection(this);
        }
        //
        // Construct a new wrapper message over an existing mailmsg.
        //
        public Message(object pMsg) :
            base((IMailMsgProperties) pMsg)
        {
            this.PrivateRecips = new RecipCollection(this);
        }

        public void Commit()
        {
            pMsg.Commit(null);
        }
        public void ForkForRecipients(
            out Message msgNew,
            out RecipsAdd recipsAddNew)
        {
            MailMsg pMsgNew;
            IMailMsgRecipientsAdd pRecipsAddNew;

            pMsg.ForkForRecipients(
                out pMsgNew,
                out pRecipsAddNew);
            
            msgNew = new Message(pMsgNew);
            recipsAddNew = new RecipsAdd(this, pRecipsAddNew);
        }
        public void RebindAfterFork(
            Message msgOrig,
            object storeDriver)
        {
            pMsg.RebindAfterFork(
                (MailMsg) msgOrig.pMsg,
                storeDriver);
        }
        public byte[] ReadContent(
            uint dwOffset,
            uint dwLength)
        {
            IntPtr pBuffer = IntPtr.Zero;

            try
            {
                uint dwLengthRead;

                pBuffer = Marshal.AllocCoTaskMem(
                    (int) dwLength);

                pMsg.ReadContent(
                    dwOffset,
                    dwLength,
                    out dwLengthRead,
                    pBuffer,
                    null);

                byte[] rgb;
                rgb = new byte[dwLengthRead];
                Marshal.Copy(pBuffer, rgb, 0, (int) dwLengthRead);
                return rgb;
            }
            finally
            {
                if(pBuffer != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pBuffer);
                }
            }
                    
        }
        public void WriteContent(
            uint dwOffset,
            uint dwLength,
            out uint dwLengthWritten,
            byte[] rgb)
        {
            pMsg.WriteContent(
                dwOffset,
                dwLength,
                out dwLengthWritten,
                rgb,
                null);
        }
        public uint GetContentSize()
        {
            uint dwRet;
            pMsg.GetContentSize(
                out dwRet,
                null);
            return dwRet;
        }
        public void SetContentSize(
            uint dwSize)
        {
            pMsg.SetContentSize(
                dwSize,
                null);
        }
        public RecipsAdd AllocNewList()
        {
            IMailMsgRecipientsAdd pRecipsAdd;

            ((IMailMsgRecipients) pMsg).AllocNewList(
                out pRecipsAdd);

            return new RecipsAdd(this, pRecipsAdd);
        }
        public void WriteList(
            RecipsAdd ra)
        {
            ((IMailMsgRecipients) pMsg).WriteList(
                ra.pRecipsAdd);
        }

        //
        // Write the RFC 822 content to a stream
        // This only works on mailmsgs with a store driver backing.
        //
        public void CopyContentToStream(
            Stream UserStream)
        {
            uint dwOffset = 0;
            byte[] rgb;

            do
            {
                rgb = ReadContent(
                    dwOffset,
                    1024);

                if(rgb.Length > 0)
                {
                    UserStream.Write(
                        rgb,
                        0,
                        rgb.Length);
                }
                dwOffset += (uint) rgb.Length;

            } while(rgb.Length == 1024);
            //
            // Set the position back
            //
            UserStream.Position = 0;
        }
        //
        // Convert to a string.
        //
        public override string ToString()
        {
            return "Microsoft.Exchange.Transport.Interop.Message: msgId = " + 
                Rfc822MsgId + 
                "; subject = " + Rfc822MsgSubject + 
                "; 821 sender = " + SenderAddressSMTP +
                "; MessageStatus = " + MessageStatus;
        }
        //
        // Public properties:
        //
        
        // Recipient collection.
        //
        public RecipCollection Recips
        {
            get
            {
                return PrivateRecips;
            }
        }
        //
        // Gets a direct pointer to the COM mailmsg interface.
        //
        public IMailMsgProperties MailMsg
        {
            get
            {
                return pMsg;
            }
        }
        //
        // Mailmsg Property accessors:
        //
        public string SenderAddressSMTP
        { 
            get 
            {
                return GetStringA(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_SENDER_ADDRESS_SMTP );
            }
            set
            {
                PutStringA(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_SENDER_ADDRESS_SMTP, 
                    value);
            }
        }
        public string SenderAddressX500
        {
            get
            {
                return GetStringA( 
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_SENDER_ADDRESS_X500 );
            }
            set
            {
                PutStringA(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_SENDER_ADDRESS_X500,
                    value);
            }
        }
        public int HrCatStatus
        {
            get
            {
                return (int) GetDWORD(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_HR_CAT_STATUS );
            }
            set
            {
                PutDWORD(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_HR_CAT_STATUS,
                    (uint) value);
            }
        }
        public enum MessageStatusEnum : uint
        {
            Success = 0,
            Retry = 1,
            AbortDelivery = 2,
            BadMail = 3,
            Submitted = 4,
            Categorized = 5,
            AbandonDelivery = 6
        }
        public MessageStatusEnum MessageStatus
        {
            get
            {
                return (MessageStatusEnum) GetDWORD(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_MESSAGE_STATUS );
            }
            set
            {
                PutDWORD(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_MESSAGE_STATUS,
                    (uint) value);
            }
        }
        public enum MsgClassEnum : uint
        {
            System = 0,
            Replication = 1,
            DeliveryRepot = 2,
            NonDelieryReport = 3
        }
                     
        public MsgClassEnum MsgClass
        {
            get
            {
                return (MsgClassEnum) GetDWORD(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_MSGCLASS );
            }
            set
            {
                PutDWORD(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_MSGCLASS,
                    (uint) value);
            }
        }
        public uint SizeHint
        {
            get
            {
                return GetDWORD(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_MSG_SIZE_HINT );
            }
            set
            {
                PutDWORD(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_MSG_SIZE_HINT,
                    value);
            }
        }
        public string Rfc822BccAddress
        {
            get
            {
                return GetStringA(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_RFC822_BCC_ADDRESS );
            }
        }
        public string Rfc822CcAddress
        {
            get
            {
                return GetStringA(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_RFC822_CC_ADDRESS );
            }
        }
        public string Rfc822FromAddress
        {
            get
            {
                return GetStringA(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_RFC822_FROM_ADDRESS );
            }
        }
        public string Rfc822MsgId
        {
            get
            {
                return GetStringA(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_RFC822_MSG_ID );
            }
            set
            {
                PutStringA(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_RFC822_MSG_ID, 
                    value);
            }
        }
        public string Rfc822MsgSubject
        {
            get
            {
                return GetStringA(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_RFC822_MSG_SUBJECT );
            }
            set
            {
                PutStringA(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_RFC822_MSG_SUBJECT, 
                    value);
            }
        }
        public string Rfc822ToAddress
        {
            get
            {
                return GetStringA(
                    (uint) IMMPID_MP_ENUM.IMMPID_MP_RFC822_TO_ADDRESS );
            }
        }

        //
        // Add more per-msg property accessors above this line.
        //

        //
        // Protected methods.
        //
        protected virtual Recip CreateRecip(
            IMailMsgRecipientsBase pRecipBase,
            uint idx)
        {
            return new Recip(pRecipBase, idx);
        }
        //
        // Internal method.
        //
        internal Recip CreateRecipInternal(
            IMailMsgRecipientsBase pRecipBase,
            uint idx)
        {
            return CreateRecip(pRecipBase, idx);
        }

        //
        // Private methods.
        //
        private Recip GetRecip(uint idx)
        {
            if(idx >= RecipCount)
            {
                return null;
            }
            else
            {
                return CreateRecip((IMailMsgRecipientsBase)pMsg, idx);
            }
        }
        private uint RecipCount
        {
            get
            {
                uint cRecips;
                ((IMailMsgRecipients)pMsg).Count(out cRecips);
                return cRecips;
            }
        }
        protected uint AllocPropIDRange(
            Guid guidProps,
            uint dwNumProps)
        {
            uint dwPropOffset;
            
            ((IMailMsgPropertyManagement)pMsg).AllocPropIDRange(
                ref guidProps,
                dwNumProps,
                out dwPropOffset);

            return dwPropOffset;
        }
        //
        // Private data.
        //
        private RecipCollection PrivateRecips;

        //
        // Subclasses
        //


        //
        // RecipCollection
        //
        public class RecipCollection :
            System.MarshalByRefObject,
            IEnumerable
        {
            internal RecipCollection(Message msg)
            {
                this.msg = msg;
            }
            public IEnumerator GetEnumerator()
            {
                return new RecipEnumerator(msg);
            }        
            public Recip this[uint index]
            {
                get
                {
                    return msg.GetRecip(index);
                }
            }
            public uint Count
            {
                get
                {
                    return msg.RecipCount;
                }
            }
            private Message msg;
        }
        //
        // RecipEnumerator
        //
        public class RecipEnumerator : IEnumerator
        {
            public RecipEnumerator(Message msg)
            {
                m_msg = msg;
            }
            public bool MoveNext()
            {
                Recip nextRecip = m_msg.GetRecip(m_nPos);
                if(nextRecip != null)
                {
                    m_currentRecip = nextRecip;
                    m_nPos++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            public void Reset()
            {
                m_nPos = 0;
            }
            public object Current
            {
                get
                {
                    return m_currentRecip;
                }
            }
            private uint m_nPos = 0;
            private Message m_msg;
            private Recip m_currentRecip = null;
        }
    }   

    //
    // Wrapper for mailmsg recip.
    //
    public class Recip : PropertyAccessor
    {
        public Recip(IMailMsgRecipientsBase pRecips, uint idx) :
            base(pRecips, idx)
        {
        }

        //
        // Convert to a string.
        //
        public override string ToString()
        {
            return "Microsoft.Exchange.Transport.Interop.Recip: SMTPAddress = " + 
                SMTPAddress + 
                "; domain = " + Domain +
                "; flags = " + RecipientFlags +
                "; error code = " + ErrorCode;
        }
        //
        // Mailmsg Property accessors:
        //
        public string SMTPAddress
        {
            get
            {
                return GetStringA( 
                    (uint) IMMPID_RP_ENUM.IMMPID_RP_ADDRESS_SMTP );
            }
        }
        public string SMTPAddressDomain
        {
            //
            // Return the domain part of the smtp address.  
            // Example: return sample.com for the address "someone@sample.com".
            //
            get
            {
                string strSMTPAddress = SMTPAddress;
                int atIndex = strSMTPAddress.IndexOf('@');
                if(atIndex != -1)
                {
                    return strSMTPAddress.Substring(atIndex+1);
                }
                else
                {
                    return null;
                }
            }
        }
        public string X500Address
        {
            get
            {
                return GetStringA( 
                    (uint) IMMPID_RP_ENUM.IMMPID_RP_ADDRESS_X500 );
            }
        }
        public string Domain
        {
            get
            {
                return GetStringA(
                    (uint) IMMPID_RP_ENUM.IMMPID_RP_DOMAIN );
            }
            set
            {
                PutStringA(
                    (uint) IMMPID_RP_ENUM.IMMPID_RP_DOMAIN, 
                    value);
            }
        }
        public int ErrorCode
        {
            get
            {
                return (int) GetDWORD(
                    (uint) IMMPID_RP_ENUM.IMMPID_RP_ERROR_CODE );
            }
            set
            {
                PutDWORD(
                    (uint) IMMPID_RP_ENUM.IMMPID_RP_ERROR_CODE,
                    (uint) value);
            }
        }

        public uint RecipientFlags
        {
            get
            {
                return GetDWORD(
                    (uint) IMMPID_RP_ENUM.IMMPID_RP_RECIPIENT_FLAGS );
            }
            set
            {
                PutDWORD(
                    (uint) IMMPID_RP_ENUM.IMMPID_RP_RECIPIENT_FLAGS,
                    value);
            }
        }
        // You should not modify or use these bits.
        static public uint RP_RECIP_FLAGS_RESERVED      = 0x0000000F;
        //  Notify on success - set if RFC1891 NOTIFY=SUCCESS is used.
        static public uint RP_DSN_NOTIFY_SUCCESS        = 0x01000000;
        //  Notify on failure - set if RFC1891 NOTIFY=FAILURE is used.
        static public uint RP_DSN_NOTIFY_FAILURE        = 0x02000000;
        //  Notify on delay - set if RFC1891 NOTIFY=DELAY is used.            
        static public uint RP_DSN_NOTIFY_DELAY          = 0x04000000;
        //  Never notify - set if RFC1891 NOTIFY=NEVER is used.
        static public uint RP_DSN_NOTIFY_NEVER          = 0x08000000;
        //  Mask of all notify parameters.
        static public uint RP_DSN_NOTIFY_MASK           = 0x0F000000;

        //The following flags can be used in searches, but should not be set directly.

        //  Recipient has either been delivered or should not be delivered.
        //  (This flag is provided to check status of recipient; it should never be used
        //  directly.)
        static public uint  RP_HANDLED                  = 0x00000010;

        //  Some form of hard failure happend.
        //  (This flag is provided to check status of recipient; it should never be used
        //  directly).
        static public uint  RP_GENERAL_FAILURE          = 0x00000020;

        //  Final DSN has been sent (or no DSN needs to be sent).
        //  (This flag is provided to check status of recipient... it should never be used
        //  directly).
        static public uint  RP_DSN_HANDLED              = 0x00000040;

        //  The recipient has been delivered successfully.
        static public uint  RP_DELIVERED                = 0x00000110;

        //  NDR (FAILED DSN) for this recipient has been sent.
        static public uint  RP_DSN_SENT_NDR             = 0x00000450;

        //  Recipient has a hard failure.
        static public uint  RP_FAILED                   = 0x00000830;

        //  This recipient was not resolved by categorization.
        static public uint  RP_UNRESOLVED               = 0x00001030;

        //  This recipient is an expanded distribution list.
        static public uint  RP_EXPANDED                 = 0x00002010;

        //  At least one Delay DSN sent.
        static public uint  RP_DSN_SENT_DELAYED         = 0x00004000;

        //  Expanded DSN has been sent.
        static public uint  RP_DSN_SENT_EXPANDED        = 0x00008040;

        //  Relayed DSN has been sent.
        static public uint  RP_DSN_SENT_RELAYED         = 0x00010040;

        //  Delivered DSN has been sent.
        static public uint  RP_DSN_SENT_DELIVERED       = 0x00020040;

        //  Remote MTA does not advertise DSN support (relay might be needed).
        static public uint  RP_REMOTE_MTA_NO_DSN        = 0x00080000;

        //  Error happened in store driver.
        static public uint  RP_ERROR_CONTEXT_STORE      = 0x00100000;

        //  Error happened during categorization.
        static public uint  RP_ERROR_CONTEXT_CAT        = 0x00200000;

        //  Error happened in a MTA (for example SMTP stack).
        static public uint  RP_ERROR_CONTEXT_MTA        = 0x00400000;

        // Flags that can be used for temp storage, while a component has access to recipients.
        //Once control of recipients is passed, value
        //is undefined.        
        static public uint  RP_VOLATILE_FLAGS_MASK      = 0xF0000000;

        public string SmtpStatusString
        {
            get
            {
                return GetStringA(
                    (uint) IMMPID_RP_ENUM.IMMPID_RP_SMTP_STATUS_STRING);
            }
            set
            {
                PutStringA(
                    (uint) IMMPID_RP_ENUM.IMMPID_RP_SMTP_STATUS_STRING,
                    value);
            }
        }

        //
        // Add more per-recip property accessors above this line.
        //
    }

    //
    // RecipsAdd
    //
    public class RecipsAdd :
        System.MarshalByRefObject,
        IDisposable
    {
        internal RecipsAdd(
            Message msg,
            IMailMsgRecipientsAdd pRecipsAdd)
        {
            this.msg = msg;
            this.pRecipsAdd = pRecipsAdd;
        }
        public void Dispose()
        {
            //
            // Release our reference to IMailMsgRecipientsAdd.
            //
            Marshal.ReleaseComObject(pRecipsAdd);
            GC.SuppressFinalize(this);
        }
        //
        // Add a new (primary) recipient via their SMTP address.
        //
        public Recip AddSMTPRecipient(
            string strSMTPAddress)
        {
            return AddSMTPRecipient(
                strSMTPAddress,
                null);
        }
        //
        // Add a new (primary) recipient via their SMTP address.
        // Copy over all non-address properties from an existing recipient
        // if sourceRecip is non-null.
        //
        public Recip AddSMTPRecipient(
            string strSMTPAddress,
            Recip sourceRecip)
        {
            return AddPrimary(
                strSMTPAddress,
                (uint) IMMPID_RP_ENUM.IMMPID_RP_ADDRESS_SMTP,
                sourceRecip);
        }

        //
        // Add a new (secondary) recipient via their SMTP address.
        // Throws a DuplicateRecipientException if the recipient address 
        // is already in mailmsg.
        // Copy over all non-address properties from an existing recipient
        // if sourceRecip is non-null.
        //
        public Recip AddSMTPRecipientSecondary(
            string strSMTPAddress,
            Recip sourceRecip)
        {
            return AddSecondary(
                strSMTPAddress,
                (uint) IMMPID_RP_ENUM.IMMPID_RP_ADDRESS_SMTP,
                sourceRecip);
        }                    

        //
        // Add a recipient to mailmsg.
        //
        private Recip AddPrimary(
            string strAddr,
            uint dwAddrProp,
            Recip sourceRecip)
        {
            uint dwNewIdx;

            pRecipsAdd.AddPrimary(
                1,
                ref strAddr,
                ref dwAddrProp,
                out dwNewIdx,
                (sourceRecip == null) ? null : sourceRecip.pRecips,
                (sourceRecip == null) ? 0 : sourceRecip.dwRecipIdx);

            return msg.CreateRecipInternal(pRecipsAdd, dwNewIdx);
        }
        private Recip AddSecondary(
            string strAddr,
            uint dwAddrProp,
            Recip sourceRecip)
        {
            uint dwNewIdx;

            try
            {
                pRecipsAdd.AddSecondary(
                    1,
                    ref strAddr,
                    ref dwAddrProp,
                    out dwNewIdx,
                    (sourceRecip == null) ? null : sourceRecip.pRecips,
                    (sourceRecip == null) ? 0 : sourceRecip.dwRecipIdx);
            }
            catch(COMException e)
            {
                if(e.ErrorCode != Constants.MAILMSG_E_DUPLICATE)
                    throw;
                throw new DuplicateRecipientException(
                    dwAddrProp,
                    strAddr);
            }

            return msg.CreateRecipInternal(pRecipsAdd, dwNewIdx);
        }
        //
        // Subclasses
        //
        public class DuplicateRecipientException :
            Exception
        {
            internal DuplicateRecipientException(
                uint dwAddrPropId,
                string strAddress)
            {
                this.HResult = Constants.MAILMSG_E_DUPLICATE;
                this.dwAddrPropId = dwAddrPropId;
                this.strAddress = strAddress;
            }
            public override string ToString()
            {
                return "Could not add a secondary recipient because of a duplicate collision.  Address: " +
                    + dwAddrPropId + ":" + strAddress;
            }
            private uint dwAddrPropId;
            private string strAddress;
        }

        //
        // Member data
        //
        protected Message msg;
        internal IMailMsgRecipientsAdd pRecipsAdd;
    }


    //
    // Wrapper around a mailmsg or a mailmsg recipient for accessing
    // properties.
    //
    public class PropertyAccessor :
        System.MarshalByRefObject,
        IDisposable
    {
        internal PropertyAccessor()
        {
            // pMsg or pRecips/dwRecipIdx must be set before using
            // other methods on this class.
        }
        //
        // Construct a PropertyAccessor for mailmsg per-msg properties.
        // As of this point, the PropertyAccessor "owns" the runtime callable wrapper (RCW).
        // It will release the mailmsg object when it is disposed.
        //
        internal PropertyAccessor(
            IMailMsgProperties pMsg)
        {
            this.pMsg = pMsg;
        }
        //
        // Construct a PropertyAccessor for mailmsg per-recip
        // properties.  This instance of PropertyAccessor does not
        // "own" the runtime callable wrapper (RCW) -- Dispose will do nothing of significance.
        // The RCW is owned by the PropertyAccessor instance used for
        // accessing mailmsg per-msg properties.
        //
        internal PropertyAccessor(
            IMailMsgRecipientsBase pRecips,
            uint dwRecipIdx)
        {
            this.pRecips = pRecips;
            this.dwRecipIdx = dwRecipIdx;
        }

        public void Dispose() 
        {
            if (this.pMsg != null) {
                Marshal.ReleaseComObject(pMsg);
                this.pMsg = null;
            }
            //
            // Both pMsg and pRecips point to the same runtime callable wrapper (RCW).
            // So, you only need to call ReleaseComObject on one of them.
            // After a client disposes a message object, none of the
            // Recip objects (on that message) will function.
            //
        }

        public override string ToString()
        {
            return "PropertyAccessor: pMsg = " + pMsg + "; pRecips = "
                + pRecips + "; dwRecipIdx = " + dwRecipIdx;
        }
        //
        // Throws a PropNotSetException if the property is not set.
        //
        protected Guid GetGuid(
            uint dwPropertyId)
        {
            byte[] rgb;
            rgb = GetProperty(dwPropertyId);
            if(rgb == null)
            {
                throw new PropNotSetException(this, dwPropertyId);
            }
            return new Guid(rgb);
        }
        //
        // Throws a PropNotSetException if the property is not set.
        //
        protected bool GetBool(
            uint dwPropertyId)
        {
            bool fSet;
            bool fValue;

            fSet = GetBool(
                dwPropertyId,
                out fValue);
            if(!fSet)
            {
                throw new PropNotSetException(this, dwPropertyId);
            }
            return fValue;
        }            
        //
        // Returns true if bool is set.
        // Returns false if bool is not set.
        //
        protected bool GetBool(
            uint dwPropertyId,
            out bool nBoolValue)
        {
            bool fSet;
            uint dw;
            nBoolValue = false;
            //
            // In mailmsg terms, DWORDs and BOOLs are the same thing,
            // so use GetDWORD here.
            //
            fSet = GetDWORD(
                dwPropertyId,
                out dw);
            if(fSet)
            {
                nBoolValue = (dw != 0);
            }
            return fSet;
        }
        //
        // Returns the set value.  If value is not set,
        // returns fDefaultValue.
        //
        protected bool GetBool(
            uint dwPropertyId,
            bool fDefaultValue)
        {
            bool fSet;
            bool fRet;

            fSet = GetBool(
                dwPropertyId,
                out fRet);
            if(!fSet)
            {
                fRet = fDefaultValue;
            }
            return fRet;
        }
        //
        // Throws a PropNotSetException if the property is not set.
        //
        protected uint GetDWORD(
            uint dwPropertyId)
        {
            bool fSet;
            uint dwValue;
            
            fSet = GetDWORD(
                dwPropertyId,
                out dwValue);
            if(!fSet)
            {
                throw new PropNotSetException(this, dwPropertyId);
            }
            return dwValue;
        }
        //
        // Returns true if DWORD property is set.
        // Returns false if DWORD property is not set.
        //
        protected bool GetDWORD(
            uint dwPropertyId,
            out uint dwValue)
        {
            byte[] rgb;
            rgb = GetProperty(dwPropertyId);
            if(rgb == null)
            {
                dwValue = 0;
                return false;
            }
            dwValue = BitConverter.ToUInt32(rgb, 0);
            return true;
        }
        //
        // Returns the DWORD property value.  Returns dwDefaultValue 
        // if property is not set.
        //
        protected uint GetDWORD(
            uint dwPropertyId,
            uint dwDefaultValue)
        {
            bool fSet;
            uint dwRet;

            fSet = GetDWORD(
                dwPropertyId,
                out dwRet);
            if(!fSet)
            {
                dwRet = dwDefaultValue;
            }
            return dwRet;
        }
        //
        // Returns null if property is not set.
        //
        protected string GetStringA(
            uint dwPropertyId)
        {
            byte[] rgb;
            rgb = GetProperty(dwPropertyId);
            if((rgb == null) || (rgb.Length < 1))
                return null;
            else 
            	  //-1 to Remove null termination.
                return new string(Encoding.Default.GetChars( rgb, 0, rgb.Length - 1 )); 
        }
        //
        // Returns null if property is not set.
        //
        protected string GetStringW(
            uint dwPropertyId)
        {
            UnicodeEncoding ue = new UnicodeEncoding();
            byte[] rgb;
            rgb = GetProperty(dwPropertyId);
            if((rgb == null) || (rgb.Length < 2))
                return null;
            // -2 to remove null term for wide character.
            else return new string(ue.GetChars( rgb, 0, rgb.Length - 2 )); 
        }
        //
        // Returns null if property is not set.
        //
        protected byte[] GetProperty(
            uint dwPropertyId)
        {
            IntPtr pbBuffer = IntPtr.Zero;

            try
            {
                byte[] rgbRet;
                IntPtr pbStupid = (IntPtr) 1;
                uint nSize = 0;
                int hr = 0;

                if(pMsg != null)
                {
                    hr = pMsg.GetProperty(
                        dwPropertyId,
                        0,
                        ref nSize,
                        pbStupid);
                }
                else
                {
                    hr = pRecips.GetProperty(
                        dwRecipIdx,
                        dwPropertyId,
                        0,
                        ref nSize,
                        pbStupid);
                }

                if(hr == Constants.MAILMSG_E_PROPNOTFOUND)
                {
                    //
                    // Property is not set.
                    //
                    return null;
                }
                else if(hr != Constants.HRFW32_ERROR_INSUFFICIENT_BUFFER)
                {
                    //
                    // Unexpected error from mailmsg.
                    //
                    throw(new COMException("unexpected error from mailmsg", hr));
                }

                pbBuffer = Marshal.AllocCoTaskMem( (int) nSize);

                if(pMsg != null)
                {
                    hr = pMsg.GetProperty(
                        dwPropertyId,
                        nSize,
                        ref nSize,
                        pbBuffer);
                }
                else
                {
                    hr = pRecips.GetProperty(
                        dwRecipIdx,
                        dwPropertyId,
                        nSize,
                        ref nSize,
                        pbBuffer);
                }

		  // hr is a failure code.
                if(hr < 0) 
                {
                    //
                    // Unexpected error from mailmsg.
                    //
                    throw(new COMException("unexpected error from mailmsg", hr));
                }

                rgbRet = new byte[nSize];
                Marshal.Copy(pbBuffer, rgbRet, 0, (int) nSize);
                return rgbRet;
            }
            finally
            {
                if(pbBuffer != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem( pbBuffer );
                }
            }

        }

        protected void PutGuid(
            uint dwPropertyId,
            Guid gValue)
        {
            PutProperty(
                dwPropertyId,
                gValue.ToByteArray());
        }

        protected void PutBool(
            uint dwPropertyId,
            bool fValue)
        {
            PutDWORD(
                dwPropertyId,
                fValue ? (uint) 1 : (uint) 0);
        }
        protected void PutDWORD(
            uint dwPropertyId,
            uint dwValue)
        {
            PutProperty(
                dwPropertyId,
                BitConverter.GetBytes(dwValue));
        }

        protected void PutStringA(
            uint dwPropertyId,
            string str)
        {
            if(pMsg != null)
            {
                pMsg.PutStringA(
                    dwPropertyId,
                    str);
            }
            else
            {
                pRecips.PutStringA(
                    dwRecipIdx,
                    dwPropertyId,
                    str);
            }
        }
        protected void PutStringW(
            uint dwPropertyId,
            string str)
        {
            if(pMsg != null)
            {
                pMsg.PutStringW(
                    dwPropertyId,
                    str);
            }
            else
            {
                pRecips.PutStringW(
                    dwRecipIdx,
                    dwPropertyId,
                    str);
            }
        }
        protected void PutProperty(
            uint dwPropertyId,
            byte[] rgbValue)
        {
            if(pMsg != null)
            {
                pMsg.PutProperty(
                    dwPropertyId,
                    (uint) rgbValue.Length,
                    rgbValue);
            }
            else
            {
                pRecips.PutProperty(
                    dwRecipIdx,
                    dwPropertyId,
                    (uint) rgbValue.Length,
                    rgbValue);
            }
        }            

        //
        // Private member data.
        //
        internal IMailMsgProperties pMsg = null;
        internal IMailMsgRecipientsBase pRecips = null;
        internal uint dwRecipIdx = 0;

        //
        // Subclasses
        //
        [Serializable]
        public class PropNotSetException :
            Exception,
            ISerializable
        {
            internal PropNotSetException(
                PropertyAccessor pa,
                uint dwPropId)
            {
                this.HResult = Constants.MAILMSG_E_PROPNOTFOUND;
                this.pa = pa;
                this.dwPropId = dwPropId;
            }
            public PropNotSetException(
                SerializationInfo info, StreamingContext context) : 
                base(info, context)
            {
                pa = (PropertyAccessor) info.GetValue("pa", typeof(PropertyAccessor));
                dwPropId = info.GetUInt32("dwPropId");
            }
            void ISerializable.GetObjectData(
                SerializationInfo info,
                StreamingContext context)
            {
                base.GetObjectData(info,context);
                info.AddValue("pa", pa);
                info.AddValue("dwPropId", dwPropId);
            }
            public override string ToString()
            {
                return "Attempt to read an unset property.  PropId: "
                    + dwPropId + "; PropertyAccessor: " + pa;
            }
            private PropertyAccessor pa;
            private uint dwPropId;
            //
            // PropNotSetException is serializable.  So, if you add
            // member variables, you need to update the marshaling
            // code above.
            //
        }
    }

    
    public class Constants
    {
        //$$TODO: Get these from somewhere else?
        static public int S_OK = 0;
        static public int CAT_W_SOME_UNDELIVERABLE_MSGS = unchecked((int)0x80040546);
        static public int E_NOTIMPL = unchecked((int)0x80000001);
        static public int HRFW32_ERROR_FILE_NOT_FOUND = unchecked((int)0x80070002);
        static public int HRFW32_ERROR_HANDLE_EOF = unchecked((int)0x80070026);
        static public int HRFW32_ERROR_INSUFFICIENT_BUFFER = unchecked((int)0x8007007A);
        static public int MAILMSG_E_DUPLICATE = unchecked((int)0x80070050);
        static public int MAILMSG_E_PROPNOTFOUND = unchecked((int)0x800300FD);
        static public int STOREDRV_E_RETRY = unchecked((int)0x800404D5);
    }
}
