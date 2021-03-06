//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repository.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Repository.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DECLARE @EstadoDoc VARCHAR(2)
        ///
        ///--PE Doc. Pendiente
        ///--DR Doc. Rechazado
        ///--DA Doc. Aprobado
        ///--DE Doc. Con Error
        ///--DC Doc. Corregido
        ///--DS Doc. En Seguimiento
        ///
        ///--BP Baja Pendiente
        ///--BS Baja En Seguimiento
        ///--BE Baja Con Error
        ///--BA Baja Aprobado
        ///--BR Baja Rechazado
        ///
        /// --DocEnSeguimiento = 0, DocConError = 1, BajaEnSeguimiento = 2, BajaConError = 3
        ///
        ///SET @EstadoDoc =  CASE &apos;{0}&apos; WHEN 0 THEN &apos;DS&apos;
        ///							 WHEN 1 THEN &apos;DE&apos;
        ///
        ///							 WHEN 2 THEN &apos;BS&apos;
        ///							 WHEN 3 THEN &apos;BE&apos;
        ///							 END
        ///
        ///UPDATE OI [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PLUS_FE_UPDATE_ESTADO_INTEGRADOR {
            get {
                return ResourceManager.GetString("PLUS_FE_UPDATE_ESTADO_INTEGRADOR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DECLARE @EstadoPdf VARCHAR(2)
        ///
        ///--PP Pdf Pendiente
        ///--PD Pdf Descargado
        ///--PE Pdf Error al descargar
        ///--PN Pdf No descargar
        ///
        ///--Descargado = 0, Error = 1
        ///
        ///SET @EstadoPdf =  CASE &apos;{0}&apos; WHEN 0 THEN &apos;PD&apos;
        ///							 WHEN 1 THEN &apos;PE&apos;
        ///							 END
        ///
        ///UPDATE OINV SET U_PLUS_ESTADOPDF = @EstadoPdf, U_PLUS_RESPDF = &apos;{1}&apos; WHERE DocEntry = &apos;{2}&apos; AND ObjType = &apos;{3}&apos;
        ///
        ///UPDATE ORIN SET U_PLUS_ESTADOPDF = @EstadoPdf, U_PLUS_RESPDF = &apos;{1}&apos; WHERE DocEntry = &apos;{2}&apos; AND ObjType = &apos;{3}&apos;
        ///
        ///UPDATE ODPI SET U_PLUS_ESTADOPDF =  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PLUS_FE_UPDATE_ESTADO_PDF {
            get {
                return ResourceManager.GetString("PLUS_FE_UPDATE_ESTADO_PDF", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DECLARE @EstadoSunat VARCHAR(2)
        ///
        ///-- 0 | Por procesar
        ///-- 1 | Aceptado
        ///-- 2 | Aceptado con observacion
        ///-- 3 | Rechazado
        ///-- 4 | En cola
        ///-- 5 | Pendiente Respuesta
        ///-- 6 | Anulado
        ///
        ///SET @EstadoSunat =  CASE &apos;{0}&apos; WHEN 1 THEN &apos;DA&apos; --Documento aprobado
        ///							   WHEN 2 THEN &apos;DA&apos; --Documento aprobado
        ///							   WHEN 3 THEN &apos;DR&apos; --Documento rechazado
        ///							   WHEN 6 THEN &apos;BA&apos; --Com. Baja aprobado
        ///							   ELSE &apos;DS&apos; END --Documento en seguimiento
        ///
        ///UPDATE OINV SET U_IDC_FESTADO = @EstadoSunat, U_IDC_FE [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PLUS_FE_UPDATE_ESTADO_SUNAT {
            get {
                return ResourceManager.GetString("PLUS_FE_UPDATE_ESTADO_SUNAT", resourceCulture);
            }
        }
    }
}
