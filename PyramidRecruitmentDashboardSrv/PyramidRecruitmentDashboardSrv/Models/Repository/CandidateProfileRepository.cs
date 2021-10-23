using MySqlConnector;
using PyramidRecruitmentDashboardSrv.Models.Repository;
using PyramidRecruitmentDashboardSrv.Parser;
using Sovren;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PyramidRecruitmentDashboardSrv.Models
{
    public class CandidateProfileRepository : ICandidateProfileRepository
    {


        private readonly AppDb db;

        public CandidateProfileRepository(AppDb db)
        {
            this.db = db;
        }
        public async Task<int> AddCandidateProfile(JDProfileRequest jDProfileRequest)
        {

            int result = 0;
            var genratedFile = getFileUrl(jDProfileRequest.ProfileUrl, jDProfileRequest.ProfileType);
            if (!string.IsNullOrEmpty(genratedFile))
            {
                var response = await ResumeJDParserUtility.ParseResume(genratedFile);
                JD jD = new JD();
                if (response != null)
                {
                    if (jDProfileRequest.ProfileType == 1)
                    {
                        CandidateProfile profile = new CandidateProfile();

                        profile.InsBy = jDProfileRequest.InsBy;
                        profile.Name = response.EasyAccess().GetCandidateName()?.FormattedName;
                        profile.EmailAddress = response.EasyAccess().GetEmailAddresses()?.FirstOrDefault();
                        profile.MobileNumber = response.EasyAccess().GetPhoneNumbers()?.FirstOrDefault();
                        var allSkill = response.EasyAccess().GetSkillNames();
                        string skill = string.Empty;
                        foreach (var item in allSkill)
                        {
                            skill += item + ", ";
                        }
                        //change Afer Discusion
                        profile.Skills = skill.TrimEnd(',');

                        try
                        {
                            result = InsertProfileInDbAsync(profile);
                        }
                        catch (Exception ex)
                        {
                            result = 0;
                        }
                    }


                }
            }

            return result;
        }


        private int InsertProfileInDbAsync(CandidateProfile candidate)
        {
            using var cmd = db.Connection.CreateCommand();
            cmd.CommandText = "Insert_Profile";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            BindParams(cmd, candidate);
            db.Connection.Open();
            cmd.ExecuteNonQuery();
            db.Connection.Close();
            return (int)cmd.LastInsertedId;

        }
        private void BindParams(MySqlCommand cmd, CandidateProfile jD)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Name",
                DbType = DbType.String,
                Value = jD.Name,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@EmailAddress",
                DbType = DbType.String,
                Value = jD.EmailAddress,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@MobileNumber",
                DbType = DbType.String,
                Value = jD.MobileNumber,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@Skills",
                DbType = DbType.String,
                Value = jD.Skills,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@InsBy",
                DbType = DbType.String,
                Value = jD.InsBy,
            });
        }
        private string getFileUrl(byte[] arrayBytes, int type)
        {
            BinaryWriter Writer = null;
            string Name = string.Empty;
            if (type == 0)
                Name = @"C:\temp\JD.docx";
            else if (type == 1)
                Name = @"C:\temp\profile.docx";
            try
            {
                Writer = new BinaryWriter(File.OpenWrite(Name));
                Writer.Write(arrayBytes);
                Writer.Flush();
                Writer.Close();
            }
            catch (Exception ex)
            {
                return "";
            }
            return Name;
        }
    }
}

