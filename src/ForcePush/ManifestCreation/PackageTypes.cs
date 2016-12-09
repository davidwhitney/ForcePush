using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ForcePush.ManifestCreation
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://soap.sforce.com/2006/04/metadata")]
    public class PackageTypes
    {
        [XmlElement("members")]
        public string[] members { get; set; }

        public string name { get; set; }
    }
}