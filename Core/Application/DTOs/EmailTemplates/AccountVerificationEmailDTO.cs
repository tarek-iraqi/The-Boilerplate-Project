﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AccountVerificationEmailDTO
    {
        public string name { get; set; }
        public string token { get; set; }
    }
}
