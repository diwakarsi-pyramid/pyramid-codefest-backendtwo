using Sovren;
using Sovren.Models;
using Sovren.Models.API.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PyramidRecruitmentDashboardSrv.Parser
{
    public class ResumeJDParserUtility
    {
        public static async Task<ParseResumeResponse> ParseResume(string url)
        {
            return await CommonParse(url);
        }

        private static async Task<ParseResumeResponse> CommonParse(string url)
        {
            SovrenClient client = new SovrenClient("35465333", "WNK7G33tw0puuBJM0cpgZ+hwoS5iiKjZMIbkJMPJ", DataCenter.US);
            var file = url;
            Document doc = new Document(file);
            ParseRequest request = new ParseRequest(doc, new ParseOptions());
            try
            {
                ParseResumeResponse response = await client.ParseResume(request);
                return response;
            }
            catch (SovrenException e)
            {

                return null;
            }
        }
    }
}
