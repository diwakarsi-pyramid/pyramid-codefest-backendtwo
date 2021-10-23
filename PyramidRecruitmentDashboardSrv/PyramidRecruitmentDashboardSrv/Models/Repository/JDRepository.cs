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
    public class JDRepository : IJDRepository
    {
        private readonly AppDb db;

        public JDRepository(AppDb db)
        {
            this.db = db;
        }
        public async Task<int> AddJD(JDProfileRequest  jDProfileRequest)
        {
            int result = 0;
            var genratedFile = getFileUrl(jDProfileRequest.ProfileUrl, jDProfileRequest.ProfileType);
            if (!string.IsNullOrEmpty(genratedFile))
            {
                var response = await ResumeJDParserUtility.ParseResume(genratedFile);
                JD jD = new JD();
                if (response != null)
                {
                    if (jDProfileRequest.ProfileType == 0)
                    {
                        jD.JDName = jDProfileRequest.JDName;
                        jD.InsBy = jDProfileRequest.InsBy;
                        var allSkill = response.EasyAccess().GetSkillNames();
                        string skill = string.Empty;
                        foreach (var item in allSkill)
                        {
                            skill += item + ", ";
                        }
                        //change Afer Discusion
                        jD.Skill_GoodToHave = skill.TrimEnd(',');
                        jD.Skill_Mandatory = skill.TrimEnd(',');
                        try
                        {
                            result =  InsertJDInDbAsync(jD);
                        }
                        catch (Exception ex)
                        {
                            result = 0;
                        }
                    }
                    else if(jDProfileRequest.ProfileType == 1)
                    {
                        CandidateProfile profile = new CandidateProfile();
                       
                        profile.InsBy = jDProfileRequest.InsBy;
                        profile.Name = response.EasyAccess().GetCandidateName()?.FormattedName;
                        profile.EmailAddress = response.EasyAccess().GetEmailAddresses()?.FirstOrDefault();
                        profile.MobileNumber= response.EasyAccess().GetPhoneNumbers()?.FirstOrDefault();
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
                            result = InsertJDInDbAsync(jD);
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

        private int InsertJDInDbAsync(JD jD)
        {
            using var cmd = db.Connection.CreateCommand();
            cmd.CommandText = "Insert_JD";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            BindParams(cmd, jD);
            db.Connection.Open();
             cmd.ExecuteNonQuery();
            db.Connection.Close();
            return (int)cmd.LastInsertedId; 
            
        }
        private int InsertProfileInDbAsync(JD jD)
        {
            using var cmd = db.Connection.CreateCommand();
            cmd.CommandText = "Insert_JD";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            BindParams(cmd, jD);
            db.Connection.Open();
            cmd.ExecuteNonQuery();
            db.Connection.Close();
            return (int)cmd.LastInsertedId;

        }
        private void BindParams(MySqlCommand cmd, JD jD)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@jdname",
                DbType = DbType.String,
                Value = jD.JDName,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@skill_good_to_have",
                DbType = DbType.String,
                Value = jD.Skill_GoodToHave,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@skill_mandatory",
                DbType = DbType.String,
                Value = jD.Skill_Mandatory,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@ins_by",
                DbType = DbType.String,
                Value = jD.InsBy,
            });
        }
        private string getFileUrl(byte[] arrayBytes,int type)
        {
            BinaryWriter Writer = null;
            string Name = string.Empty;
            if (type == 0)
                Name = @"C:\temp\JD.docx";
            else if(type==1)
                 Name = @"C:\temp\profile.docx";
            try
            {
                Writer = new BinaryWriter(File.OpenWrite(Name));
                Writer.Write(arrayBytes);
                Writer.Flush();
                Writer.Close();
            }
            catch(Exception ex)
            {
                return "";
            }
            return Name;
        }
    }
}
