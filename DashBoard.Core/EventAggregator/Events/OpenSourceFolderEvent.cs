﻿using DashBoard.Core.EventAggregator.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashBoard.Core.EventAggregator.Events
{
    public class OpenSourceFolderEvent: IEvent
    {
        public string FolderPath { get; set; }
    }
}
