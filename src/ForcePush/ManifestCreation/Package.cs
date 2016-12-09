using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace ForcePush.ManifestCreation
{
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://soap.sforce.com/2006/04/metadata")]
    [XmlRoot(Namespace = "http://soap.sforce.com/2006/04/metadata", IsNullable = false)]
    public class Package
    {
        [XmlElement("types")]
        public PackageTypes[] types { get; set; }

        public decimal version { get; set; }
    }
}