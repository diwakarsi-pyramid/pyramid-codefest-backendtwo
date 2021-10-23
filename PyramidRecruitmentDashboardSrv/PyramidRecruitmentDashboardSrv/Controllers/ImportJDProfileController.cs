using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PyramidRecruitmentDashboardSrv.Models;
using PyramidRecruitmentDashboardSrv.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PyramidRecruitmentDashboardSrv.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ImportJDProfileController : ControllerBase
    {
        private readonly IJDRepository iJDRepository;
        private readonly ICandidateProfileRepository candidateProfileRepository;

        public ImportJDProfileController(IJDRepository iJDRepository,ICandidateProfileRepository candidateProfileRepository)
        {
            this.iJDRepository = iJDRepository;
            this.candidateProfileRepository = candidateProfileRepository;
        }
        [Route("addjd")]
        [HttpPost]
        public async Task<ActionResult<string>> addjd(JDProfileRequest jDProfileRequest)
        {
            if (jDProfileRequest == null)
            {
                return BadRequest("Reuest is not valid");
            }
            else
            {
                var result = await iJDRepository.AddJD(jDProfileRequest);
                if (result >= 0)
                {
                    return StatusCode(StatusCodes.Status200OK,
                    "Jd has been saved successfully");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error while saving the JD.");
                }
            }
        }

        [Route("addprofile")]
        [HttpPost]
        public async Task<ActionResult<string>> addprofile(JDProfileRequest jDProfileRequest)
        {
            if (jDProfileRequest == null)
            {
                return BadRequest("Reuest is not valid");
            }
            else
            {
                var result = await candidateProfileRepository.AddCandidateProfile(jDProfileRequest);
                if (result >= 0)
                {
                    return StatusCode(StatusCodes.Status200OK,
                    "Jd has been saved successfully");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error while saving the JD.");
                }
            }
        }
    }
}
