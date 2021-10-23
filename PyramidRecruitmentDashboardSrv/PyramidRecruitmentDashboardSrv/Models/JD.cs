using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PyramidRecruitmentDashboardSrv.Models.Repository
{
    public class JD
    {
        public string JDName { get; set; }
        public String Skill_GoodToHave { get; set; }
        public String Skill_Mandatory { get; set; }
        public DateTime InsTS { get; set; }
        public String InsBy { get; set; }
    }
}
