using Ecc.Model.Enumerations;

namespace Ecc.Logic.Workers.Emails
{
    public static class LocalViewEmailRenderer
    {
        public const string LocalEmailDirectory = "EmailTemplates";

        public const string ExecuteFileNameForBody = "BodyView";

        private const string ExecuteFileNameForSubject = "Subject";

        public const string StandardFolderName = "Standard";

        public static string GetRenderFileNameForBody(EmailTemplateType emailType)
        {
            return $"{emailType.ToString()}/{StandardFolderName}/{ExecuteFileNameForBody}";
        }

        public static string GetRenderFileNameForSubject(EmailTemplateType emailType)
        {
            return $"{emailType.ToString()}/{StandardFolderName}/{ExecuteFileNameForSubject}";
        }

        public static string RenderLocalEmailBody(EmailTemplateType emailType, object model)
        {
            return null;
        }

        public static string RenderLocalEmailSubject(EmailTemplateType emailType, object model)
        {
            return null;
        }
    }
}