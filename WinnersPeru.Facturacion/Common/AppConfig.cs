using System;
using System.Configuration;

namespace Common
{
    public class AppConfig
    {
        private const string Key_MSSQLConnection = "Config.MSSQLConnection";
        private const string Key_PDFPath = "Config.PDFPath";
        private const string Key_XMLPath = "Config.XMLPath";
        private const string Key_ActivePDFPath = "Config.ActivePDFPath";
        private const string Key_ActiveLogForSuccessful = "Config.ActiveLogForSuccessful";
        private const string Key_ActiveOnlyForJson = "Config.ActiveOnlyForJson";

        private const string Key_ActiveApiProd = "Config.ActiveApiProd";
        private const string Key_ApiProdUrl = "Config.ApiProdUrl";
        private const string Key_ApiProdToken = "Config.ApiProdToken";
        private const string Key_ApiTestUrl = "Config.ApiTestUrl";
        private const string Key_ApiTestToken = "Config.ApiTestToken";

        public static string MSSQLConnection => ConfigurationManager.AppSettings[Key_MSSQLConnection];
        public static string PDFPath => ConfigurationManager.AppSettings[Key_PDFPath];
        public static string XMLPath => ConfigurationManager.AppSettings[Key_XMLPath];
        public static bool ActivePDFPath => Convert.ToBoolean(ConfigurationManager.AppSettings[Key_ActivePDFPath]);
        public static bool ActiveLogForSuccessful => Convert.ToBoolean(ConfigurationManager.AppSettings[Key_ActiveLogForSuccessful]);
        public static bool ActiveOnlyForJson => Convert.ToBoolean(ConfigurationManager.AppSettings[Key_ActiveOnlyForJson]);
        public static bool ActiveApiProd => Convert.ToBoolean(ConfigurationManager.AppSettings[Key_ActiveApiProd]);
        public static string ApiProdUrl => ConfigurationManager.AppSettings[Key_ApiProdUrl];
        public static string ApiProdToken => ConfigurationManager.AppSettings[Key_ApiProdToken];
        public static string ApiTestUrl => ConfigurationManager.AppSettings[Key_ApiTestUrl];
        public static string ApiTestToken => ConfigurationManager.AppSettings[Key_ApiTestToken];

    }
}
