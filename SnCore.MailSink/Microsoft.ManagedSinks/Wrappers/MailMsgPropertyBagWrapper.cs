//+------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// File: MailMsgPropertyBagWrapper.cs
//
// Contents: IMailMsgPropertyBag and IMailMsgLoggingPropertyBag wrappers
//
// Classes: MailMsgPropertyBag
//          MailMsgLoggingPropertyBag
//
// Functions:
//
//-------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Exchange.Transport.EventInterop;

namespace Microsoft.Exchange.Transport.EventWrappers
{
    //
    // This class wraps the IMailMsgPropertyBag interface methods.
    //
    public class MailMsgPropertyBag
    {
        //
        // Constructors:
        //

        //
        // Initialize the wrapper using an existing IMailMsgPropertyBag object.
        //
        public MailMsgPropertyBag(
            IMailMsgPropertyBag propBag)
        {
            this.propBag = propBag;
        }
        
        //
        // Initialize the wrapper using a variant (presumably a server or session object).
        // This constructor will throw an exception if QueryInterface for IMailMsgPropertyBag fails.
        //
        public MailMsgPropertyBag(
            object propBag)
        {
            this.propBag = (IMailMsgPropertyBag) propBag;
        }

        ~MailMsgPropertyBag()
        {
            this.propBag = null;
        }


        //
        // Wrap Get/Put methods for well known properties:
        //

        public uint Server_Instance
        {
            get
            {
                return GetDWORD(Constants.PE_ISERVID_DW_INSTANCE);
            }
            set
            {
                PutDWORD(Constants.PE_ISERVID_DW_INSTANCE, value);
            }
        }

        public string Server_DefaultDomain
        {
            get
            {
                return GetStringA(Constants.PE_ISERVID_SZ_DEFAULTDOMAIN);
            }
            set
            {
                PutStringA(Constants.PE_ISERVID_SZ_DEFAULTDOMAIN, value);
            }
        }

        public uint Server_CATEnable
        {
            get
            {
                return GetDWORD(Constants.PE_ISERVID_DW_CATENABLE);
            }
            set
            {
                PutDWORD(Constants.PE_ISERVID_DW_CATENABLE, value);
            }
        }

        public uint Server_CATFlags
        {
            get
            {
                return GetDWORD(Constants.PE_ISERVID_DW_CATFLAGS);
            }
            set
            {
                PutDWORD(Constants.PE_ISERVID_DW_CATFLAGS, value);
            }
        }

        public uint Server_CATPort
        {
            get
            {
                return GetDWORD(Constants.PE_ISERVID_DW_CATPORT);
            }
            set
            {
                PutDWORD(Constants.PE_ISERVID_DW_CATPORT, value);
            }
        }

        public string Server_CATUser
        {
            get
            {
                return GetStringA(Constants.PE_ISERVID_SZ_CATUSER);
            }
            set
            {
                PutStringA(Constants.PE_ISERVID_SZ_CATUSER, value);
            }
        }

        public string Server_CATSchema
        {
            get
            {
                return GetStringA(Constants.PE_ISERVID_SZ_CATSCHEMA);
            }
            set
            {
                PutStringA(Constants.PE_ISERVID_SZ_CATSCHEMA, value);
            }
        }

        public string Server_CATBindType
        {
            get
            {
                return GetStringA(Constants.PE_ISERVID_SZ_CATBINDTYPE);
            }
            set
            {
                PutStringA(Constants.PE_ISERVID_SZ_CATBINDTYPE, value);
            }
        }

        public string Server_CATPassword
        {
            get
            {
                return GetStringA(Constants.PE_ISERVID_SZ_CATPASSWORD);
            }
            set
            {
                PutStringA(Constants.PE_ISERVID_SZ_CATPASSWORD, value);
            }
        }

        public string Server_CATDomain
        {
            get
            {
                return GetStringA(Constants.PE_ISERVID_SZ_CATDOMAIN);
            }
            set
            {
                PutStringA(Constants.PE_ISERVID_SZ_CATDOMAIN, value);
            }
        }

        public string Server_CATNamingContext
        {
            get
            {
                return GetStringA(Constants.PE_ISERVID_SZ_CATNAMINGCONTEXT);
            }
            set
            {
                PutStringA(Constants.PE_ISERVID_SZ_CATNAMINGCONTEXT, value);
            }
        }

        public string Server_CATDSType
        {
            get
            {
                return GetStringA(Constants.PE_ISERVID_SZ_CATDSTYPE);
            }
            set
            {
                PutStringA(Constants.PE_ISERVID_SZ_CATDSTYPE, value);
            }
        }

        public string Server_CATDSHost
        {
            get
            {
                return GetStringA(Constants.PE_ISERVID_SZ_CATDSHOST);
            }
            set
            {
                PutStringA(Constants.PE_ISERVID_SZ_CATDSHOST, value);
            }
        }


        //
        // IMailMsgPropertyBag method wrappers
        //

        public void PutProperty(
            uint propId,
            byte[] propValue)
        {
            propBag.PutProperty(propId, (uint) propValue.Length, propValue);
        }


        //
        // This function makes two calls to the underlying GetProperty function.
        // The first call obtains the size of the buffer required to hold the property
        // and the second call obtains the actual value of the property.
        //
        public byte[] GetProperty(
            uint propId)
        {
            IntPtr buffer;
            uint size = 0;
            int hr;
            
            // Allocate a buffer of size 1 because GetProperty has an access violation otherwise.
            buffer = Marshal.AllocCoTaskMem(1);

            // Get the size of the property.
            hr = propBag.GetProperty(propId, 1, ref size, buffer);

            // If HR indicates that the property was not found, then throw an exception.
            if (hr == Constants.MAILMSG_E_PROPNOTFOUND)
                throw new PropNotFoundException(propId);

            // Ignore an insufficient buffer error (because we gave it a buffer of size 1),
            // but all other errors are propogated.
            if (hr != Constants.HRFW32_ERROR_INSUFFICIENT_BUFFER)
                throw new COMException("Unexpected error in MailMsgPropertyBag", hr);

            Marshal.FreeCoTaskMem(buffer);


            // Allocate a buffer of the right size.
            buffer = Marshal.AllocCoTaskMem((int) size);
            byte[] retVal = new byte[size];

            try
            {
                // Read the actual property value.
                propBag.GetProperty(propId, size, ref size, buffer);
                
                Marshal.Copy(buffer, retVal, 0, (int) size);
            }
            finally
            {
                // Free the buffer.
                if (buffer != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(buffer);
            }

            return retVal;
        }

        public void PutStringA(
            uint propId,
            string propValue)
        {
            propBag.PutStringA(propId, propValue);
        }

        public string GetStringA(
            uint propId)
        {
            byte[] buffer = GetProperty(propId);
            return new string(Encoding.Default.GetChars(buffer, 0, buffer.Length - 1));
        }

        public void PutStringW(
            uint propId,
            string propValue)
        {
            propBag.PutStringW(propId, propValue);
        }

        public string GetStringW(
            uint propId)
        {
            byte[] buffer = GetProperty(propId);
            return new string(Encoding.Unicode.GetChars(buffer, 0, buffer.Length - 2));
        }

        public void PutDWORD(
            uint propId,
            uint propValue)
        {
            propBag.PutDWORD(propId, propValue);
        }

        public uint GetDWORD(
            uint propId)
        {
            byte[] buffer = GetProperty(propId);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public void PutBool(
            uint propId,
            bool propValue)
        {
            propBag.PutBool(propId, propValue == true ? (uint) 1 : (uint) 0);
        }

        public bool GetBool(
            uint propId)
        {
            return GetDWORD(propId) == 1 ? true : false;
        }


        IMailMsgPropertyBag propBag;
        
        class Constants
        {
            // HRESULT error codes:
            static internal int MAILMSG_E_PROPNOTFOUND = unchecked((int)0x800300FD);
            static internal int HRFW32_ERROR_INSUFFICIENT_BUFFER = unchecked((int)0x8007007A);

            // Well-known server properties:
            static internal uint PE_ISERVID_DW_INSTANCE             = 0;
            static internal uint PE_ISERVID_SZ_DEFAULTDOMAIN        = 1;
            static internal uint PE_ISERVID_DW_CATENABLE            = 2;
            static internal uint PE_ISERVID_DW_CATFLAGS             = 3;
            static internal uint PE_ISERVID_DW_CATPORT              = 4;
            static internal uint PE_ISERVID_SZ_CATUSER              = 5;
            static internal uint PE_ISERVID_SZ_CATSCHEMA            = 6;
            static internal uint PE_ISERVID_SZ_CATBINDTYPE          = 7;
            static internal uint PE_ISERVID_SZ_CATPASSWORD          = 8;
            static internal uint PE_ISERVID_SZ_CATDOMAIN            = 9;
            static internal uint PE_ISERVID_SZ_CATNAMINGCONTEXT     = 10;
            static internal uint PE_ISERVID_SZ_CATDSTYPE            = 11;
            static internal uint PE_ISERVID_SZ_CATDSHOST            = 12;
        }

        //
        // This exception is thrown whenever GetProperty returns MAILMSG_E_PROPNOTFOUND.
        //
        public class PropNotFoundException :
            Exception
        {
            internal PropNotFoundException(
                uint propId)
            {
                this.propId = propId;
            }

            public override string ToString()
            {
                return "Property " + propId + " does not exist and cannot be read.";
            }

            private uint propId;
        }

    }


    //
    // This class wraps the IMailMsgLoggingPropertyBag interface methods.
    // It inherits its IMailMsgPropertyBag functionality from the MailMsgPropertyBag wrapper.
    //
    public class MailMsgLoggingPropertyBag :
        MailMsgPropertyBag
    {

        //
        // Constructors
        //

        //
        // Initialize the wrapper with an existing IMailMsgLoggingPropertyBag object.
        //
        public MailMsgLoggingPropertyBag(
            IMailMsgLoggingPropertyBag propBag) :
            base((IMailMsgPropertyBag) propBag)
        {
            this.propBag = propBag;
        }
        
        //
        // Initialize the wrapper with an existing IMailMsgPropertyBag object.
        //
        public MailMsgLoggingPropertyBag(
            IMailMsgPropertyBag propBag) :
            base(propBag)
        {
            this.propBag = (IMailMsgLoggingPropertyBag) propBag;
        }

        //
        // Initialize the wrapper with an existing variant (presumably a server or session object).
        // This constructor will throw an exception if QueryInterface for IMailMsgLoggingPropertyBag fails.
        //
        public MailMsgLoggingPropertyBag(
            Object propBag) :
            base(propBag)
        {
            this.propBag = (IMailMsgLoggingPropertyBag) propBag;
        }

        ~MailMsgLoggingPropertyBag()
        {
            this.propBag = null;
        }

        //
        // IMailMsgLoggingPropertyBag method wrappers:
        //

        public void WriteToLog(
            string clientHostName,
            string clientUserName,
            string serverAddress,
            string operation,
            string target,
            string parameters,
            string version,
            uint bytesSent,
            uint bytesReceived,
            uint processingTimeMS,
            uint win32Status,
            uint protocolStatus,
            uint port,
            string HTTPHeader)
        {
            propBag.WriteToLog(
                clientHostName,
                clientUserName,
                serverAddress,
                operation,
                target,
                parameters,
                version,
                bytesSent,
                bytesReceived,
                processingTimeMS,
                win32Status,
                protocolStatus,
                port,
                HTTPHeader);
        }


        IMailMsgLoggingPropertyBag  propBag;
    }
}
