using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PyramidRecruitmentDashboardSrv.Models
{
    public class JDProfileRequest
    {
        public string JDName { get; set; } //In Case JD name is mandatory In case of candidate profile is optional
        public byte[] ProfileUrl { get; set; }
        public int  ProfileType { get; set; }//example jd=0, candidateprofile=1
        public string InsBy { get; set; }
    }
}
