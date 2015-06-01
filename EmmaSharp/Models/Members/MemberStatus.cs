﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmmaSharp.Models.Members
{
    public sealed class MemberStatus
    {
        private readonly string name;

        public static readonly MemberStatus Active = new MemberStatus("a");
        public static readonly MemberStatus Optout = new MemberStatus("o");
        public static readonly MemberStatus Error = new MemberStatus("e");
        public static readonly MemberStatus Forwarded = new MemberStatus("f");

        private MemberStatus(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
