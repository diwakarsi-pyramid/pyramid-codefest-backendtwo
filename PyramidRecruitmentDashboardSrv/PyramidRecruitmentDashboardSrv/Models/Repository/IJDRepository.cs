﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PyramidRecruitmentDashboardSrv.Models.Repository
{
    public interface IJDRepository
    {
        Task<int> AddJD(JDProfileRequest jDProfileRequest);
    }
}
