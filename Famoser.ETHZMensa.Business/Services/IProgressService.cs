﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famoser.ETHZMensa.Business.Services
{
    public interface IProgressService
    {
        void InitializeProgressBar(int total);
        void IncrementProgress();
        void HideProgress();
    }
}