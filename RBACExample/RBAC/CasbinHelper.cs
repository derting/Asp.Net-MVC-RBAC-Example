using NetCasbin.Model;
using NetCasbin.Persist.FileAdapter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace RBACExample.RBAC
{
    public static class CasbinHelper
    {
        internal static string _reacConfig = ReadTestFile("rbac_model.conf");
        internal static string _reacCsv = ReadTestFile("rbac_policy.csv");

        public static Model GetRBACModel()
        {
            return GetNewTestModel(_reacConfig, _reacCsv);
        }

        public static Model GetNewTestModel(string modelText)
        {
            return Model.CreateDefaultFromText(modelText);
        }

        public static Model GetNewTestModel(string modelText, string policyText)
        {
            return LoadModelFromMemory(GetNewTestModel(modelText), policyText);
        }

        public static string GetTestFile(string fileName)
        {
            return Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", fileName);
        }

        public static void ForceLoadFile()
        {
            _reacConfig = ReadTestFile("rbac_model.conf");
            _reacCsv = ReadTestFile("rbac_policy.csv");
        }

        private static Model LoadModelFromMemory(Model model, string policy)
        {
            model.ClearPolicy();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(policy)))
            {
                DefaultFileAdapter fileAdapter = new DefaultFileAdapter(ms);
                fileAdapter.LoadPolicy(model);
            }
            model.RefreshPolicyStringSet();
            return model;
        }

        private static string ReadTestFile(string fileName)
        {
            return File.ReadAllText(GetTestFile(fileName));
        }
    }
}