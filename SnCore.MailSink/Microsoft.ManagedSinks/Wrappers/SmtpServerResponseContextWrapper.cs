//+------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// File: SmtpServerResponseContextWrapper.cs
//
// Contents: ISmtpServerResponseContext wrapper
//
// Classes: SmtpServerResponseContext
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
    // This class wraps the ISmtpServerResponseContext interface methods.
    //
    public class SmtpServerResponseContext
    {
        //
        // This constructor initializes the wrapper with an existing ISmtpServerResponseContext object.
        //
        public SmtpServerResponseContext(
            ISmtpServerResponseContext context)
        {
            this.context = context;
        }

        ~SmtpServerResponseContext()
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

        public string Response
        {
            get
            {
                return QueryResponse();
            }
        }

        public uint SmtpStatusCode
        {
            get
            {
                return QuerySmtpStatusCode();
            }
        }

        public uint ResponseStatus
        {
            get
            {
                return QueryResponseStatus();
            }
            set
            {
                SetResponseStatus(value);
            }
        }

        public bool PipelinedFlag
        {
            get
            {
                return QueryPipelinedFlag();
            }
        }

        public uint NextEventState
        {
            get
            {
                return QueryNextEventState();
            }
            set
            {
                SetNextEventState(value);
            }
        }

        
        //
        // ISmtpServerResponseContext method wrappers:
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

        internal uint QuerySmtpStatusCode()
        {
            uint status;
            context.QuerySmtpStatusCode(out status);
            return status;
        }

        internal uint QueryResponseStatus()
        {
            uint status;
            context.QueryResponseStatus(out status);
            return status;
        }

        internal bool QueryPipelinedFlag()
        {
            int flag;
            context.QueryPipelinedFlag(out flag);
            return (flag == 1);
        }

        internal uint QueryNextEventState()
        {
            uint state;
            context.QueryNextEventState(out state);
            return state;
        }

        internal void SetResponseStatus(
            uint status)
        {
            context.SetResponseStatus(status);
        }

        internal void SetNextEventState(
            uint state)
        {
            context.SetNextEventState(state);
        }

        public void NotifyAsyncCompletion(
            int hrResult)
        {
            context.NotifyAsyncCompletion(hrResult);
        }

        
        //
        // These constants are defined in smtpevent.idl (but only in a format usable by C++).
        //
        public class NextState
        {
            public static uint PE_STATE_DEFAULT         = 0;
            public static uint PE_STATE_SESSION_START   = 1;
            public static uint PE_STATE_MESSAGE_START   = 2;
            public static uint PE_STATE_PER_RECIPIENT   = 3;
            public static uint PE_STATE_DATA_OR_BDAT    = 4;
            public static uint PE_STATE_SESSION_END     = 5;
        }


        private ISmtpServerResponseContext context;
    }
}
