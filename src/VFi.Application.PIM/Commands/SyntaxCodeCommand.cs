using Consul;
using VFi.Application.PIM.Commands.Validations;
using VFi.Application.PIM.DTOs;
using VFi.Domain.PIM.Interfaces;
using VFi.Domain.PIM.Models;
using VFi.NetDevPack.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.Application.PIM.Commands
{
    public class SyntaxCodeCommand : Command
    { 
    }
      
    public class UseCodeCommand : SyntaxCodeCommand
    {
            public string Syntax { get; set; }
            public string Code { get; set; }

        public UseCodeCommand(string syntax, string code)
        {
            Syntax = syntax;
            Code = code;
        }
    }
}
