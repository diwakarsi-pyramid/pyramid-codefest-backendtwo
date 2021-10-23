using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PyramidRecruitmentDashboardSrv.Models.Repository
{
    public class CandidateProfile
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public double OverallMatchScore { get; set; }
        public string Skills { get; set; }
        public string TP1Comment { get; set; }
        public DateTime InsTS { get; set; }
        public string InsBy { get; set; }
    }
}
