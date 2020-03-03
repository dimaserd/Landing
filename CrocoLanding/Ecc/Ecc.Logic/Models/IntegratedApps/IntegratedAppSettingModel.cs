using Ecc.Model.Enumerations;

namespace Ecc.Logic.Models.IntegratedApps
{
    public class IntegratedAppSettingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public IntegratedAppType AppType { get; set; }
        
        public string Uid { get; set; }

        public string ConfigurationJson { get; set; }
    }
}