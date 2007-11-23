//+------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// File: SmtpOutCommandContextWrapper.cs
//
// Contents: ISmtpOutCommandContext wrapper
//
// Classes: SmtpOutCommandContext
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
    // This class wraps the ISmtpOutCommandContext interface methods.
    //
    public class SmtpOutCommandContext
    {
        //
        // This constructor initializes the wrapper from an existing ISmtpOutCommandContext object.
        //
        public SmtpOutCommandContext(
            ISmtpOutCommandContext context)
        {
            this.context = context;
        }

        ~SmtpOutCommandContext()
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
            set
            {
                SetCommand(value);
            }
        }

        public string CommandKeyword
        {
            get
            {
                return QueryCommandKeyword();
            }
        }

        public string NativeCommand
        {
            get
            {
                return QueryNativeCommand();
            }
        }

        public uint CurrentRecipientIndex
        {
            get
            {
                return QueryCurrentRecipientIndex();
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


        //
        // ISmtpOutCommandContext method wrappers:
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

        internal uint QueryNativeCommandSize()
        {
            uint size;
            context.QueryNativeCommandSize(out size);
            return size - 1;
        }

        internal string QueryNativeCommand()
        {
            uint size = QueryNativeCommandSize() + 1;
            StringBuilder buffer = new StringBuilder((int) size);
            context.QueryNativeCommand(buffer, ref size);
            return buffer.ToString();
        }

        internal uint QueryCurrentRecipientIndex()
        {
            uint index;
            context.QueryCurrentRecipientIndex(out index);
            return index;
        }

        internal uint QueryCommandStatus()
        {
            uint status;
            context.QueryCommandStatus(out status);
            return status;
        }

        internal void SetCommand(
            string command)
        {
            context.SetCommand(command, (uint) command.Length + 1);
        }

        public void AppendCommand(
            string command)
        {
            context.AppendCommand(command, (uint) command.Length + 1);
        }

        internal void SetCommandStatus(
            uint status)
        {
            context.SetCommandStatus(status);
        }

        public void NotifyAsyncCompletion(
            int hrResult)
        {
            context.NotifyAsyncCompletion(hrResult);
        }

        
        ISmtpOutCommandContext context;
    }

}
