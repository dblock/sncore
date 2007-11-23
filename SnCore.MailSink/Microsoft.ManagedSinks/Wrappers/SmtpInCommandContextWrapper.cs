//+------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// File: SmtpInCommandContextWrapper.cs
//
// Contents: ISmtpInCommandContext wrapper
//
// Classes: SmtpInCommandContext
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
    // This class wraps the ISmtpInCommandContext interface methods.
    //
    public class SmtpInCommandContext
    {
        //
        // This constructor initializes the wrapper from an existing ISmtpInCommandContext object.
        //
        public SmtpInCommandContext(
            ISmtpInCommandContext context)
        {
            this.context = context;
        }

        ~SmtpInCommandContext()
        {
            this.context = null;
        }


        //
        // Properties that wrap Set/Query methods:
        //

        public string Command
        {
            get
            {
                return QueryCommand();
            }
        }

        public string CommandKeyword
        {
            get
            {
                return QueryCommandKeyword();
            }
        }

        public string NativeResponse
        {
            get
            {
                return QueryNativeResponse();
            }
            set
            {
                SetNativeResponse(value);
            }
        }

        public string Response
        {
            get
            {
                return QueryResponse();
            }
            set
            {
                SetResponse(value);
            }
        }

        public uint CommandStatus
        {
            get
            {
                return QueryCommandStatus();
            }
            set
            {
                SetCommandStatus(value);
            }
        }

        public uint SmtpStatusCode
        {
            get
            {
                return QuerySmtpStatusCode();
            }
            set
            {
                SetSmtpStatusCode(value);
            }
        }

        public bool ProtocolErrorFlag
        {
            get
            {
                return QueryProtocolErrorFlag();
            }
            set
            {
                SetProtocolErrorFlag(value);
            }
        }


        //
        // ISmtpInCommandContext method wrappers:
        //

        internal uint QueryCommandSize()
        {
            uint size;
            context.QueryCommandSize(out size);
            return size - 1;
        }

        internal string QueryCommand()
        {
            uint size = QueryCommandSize() + 1;
            StringBuilder buffer = new StringBuilder((int) size);
            context.QueryCommand(buffer, ref size);
            return buffer.ToString();
        }

        internal uint QueryCommandKeywordSize()
        {
            uint size;
            context.QueryCommandKeywordSize(out size);
            return size - 1;
        }

        internal string QueryCommandKeyword()
        {
            uint size = QueryCommandKeywordSize() + 1;
            StringBuilder buffer = new StringBuilder((int) size);
            context.QueryCommandKeyword(buffer, ref size);
            return buffer.ToString();
        }

        internal uint QueryNativeResponseSize()
        {
            uint size;
            context.QueryNativeResponseSize(out size);
            return size - 1;
        }

        internal string QueryNativeResponse()
        {
            uint size = QueryNativeResponseSize() + 1;
            StringBuilder buffer = new StringBuilder((int) size);
            context.QueryNativeResponse(buffer, ref size);
            return buffer.ToString();
        }

        internal uint QueryResponseSize()
        {
            uint size;
            context.QueryResponseSize(out size);
            return size - 1;
        }

        internal string QueryResponse()
        {
            uint size = QueryResponseSize() + 1;
            StringBuilder buffer = new StringBuilder((int) size);
            context.QueryResponse(buffer, ref size);
            return buffer.ToString();
        }

        internal uint QueryCommandStatus()
        {
            uint status;
            context.QueryCommandStatus(out status);
            return status;
        }

        internal uint QuerySmtpStatusCode()
        {
            uint status;
            context.QuerySmtpStatusCode(out status);
            return status;
        }

        internal bool QueryProtocolErrorFlag()
        {
            int flag;
            context.QueryProtocolErrorFlag(out flag);
            return (flag == 1);
        }

        internal void SetResponse(
            string response)
        {
            context.SetResponse(response, (uint) response.Length + 1);
        }

        public void AppendResponse(
            string response)
        {
            context.AppendResponse(response, (uint) response.Length + 1);
        }

        internal void SetNativeResponse(
            string response)
        {
            context.SetNativeResponse(response, (uint) response.Length + 1);
        }

        public void AppendNativeResponse(
            string response)
        {
            context.AppendNativeResponse(response, (uint) response.Length + 1);
        }

        internal void SetCommandStatus(
            uint status)
        {
            context.SetCommandStatus(status);
        }

        internal void SetSmtpStatusCode(
            uint status)
        {
            context.SetSmtpStatusCode(status);
        }

        internal void SetProtocolErrorFlag(
            bool flag)
        {
            context.SetProtocolErrorFlag(flag == true ? 1 : 0);
        }


        public void NotifyAsyncCompletion(
            int hrResult)
        {
            context.NotifyAsyncCompletion(hrResult);
        }

        public void SetCallback(
            ISmtpInCallbackSink callback)
        {
            context.SetCallback(callback);
        }


        private ISmtpInCommandContext context;
    }

    [ComVisible(false)]
    public class ProtocolEventConstants
    {
        // sink return values
        static public int S_OK = 0;
        static public int MAILTRANSPORT_S_PENDING =     0x103;
        static public int EXPE_S_CONSUMED =             unchecked((int)0x00000002);
        
        // well-known status codes
        static public int EXPE_SUCCESS =                unchecked((int) 0x00000000);
        static public int EXPE_NOT_PIPELINED =          unchecked((int) 0x00000000);
        static public int EXPE_PIPELINED =              unchecked((int) 0x00000001);
        static public int EXPE_REPEAT_COMMAND =         unchecked((int) 0x00000002);
        static public int EXPE_BLOB_READY =             unchecked((int) 0x00000004);
        static public int EXPE_BLOB_DONE =              unchecked((int) 0x00000008);
        static public int EXPE_DROP_SESSION =           unchecked((int) 0x00010000);
        static public int EXPE_CHANGE_STATE =           unchecked((int) 0x00020000);
        static public int EXPE_TRANSIENT_FAILURE =      unchecked((int) 0x00040000);
        static public int EXPE_COMPLETE_FAILURE =       unchecked((int) 0x00080000);

        static public int EXPE_UNHANDLED =              unchecked((int) 0xFFFFFFFF);
    }

}
