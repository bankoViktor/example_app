using System.Xml.Serialization;

namespace ExampleApp.Models
{
    /// <summary>
    /// Элемент справочника валюты <see cref="Valuta"/>
    /// </summary>
    [XmlRoot(ElementName = "Item")]
    public class ValutaItem
    {
        /// <summary>
        /// Название валюты (рус.)
        /// </summary>
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Название валюты (анг.)
        /// </summary>
        [XmlElement(ElementName = "EngName")]
        public string EngName { get; set; }

        /// <summary>
        /// номинал. ед
        /// </summary>
        [XmlElement(ElementName = "Nominal")]
        public uint Nominal { get; set; }

        /// <summary>
        /// Внутренний уникальный код валюты, которая являлась базовой(предыдущей) для данной валюты
        /// </summary>
        [XmlElement(ElementName = "ParentCode")]
        public string ParentCode { get; set; }

        /// <summary>
        /// Внутренний уникальный код валюты
        /// </summary>
        [XmlAttribute(AttributeName = "ID")]
        public string ID { get; set; }
    }
}
