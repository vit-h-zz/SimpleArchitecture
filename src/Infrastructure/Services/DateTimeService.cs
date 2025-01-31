﻿using SimpleArchitecture.Application.Common.Interfaces;
using System;

namespace SimpleArchitecture.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
