using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExampleApp.Models
{
    /// <summary>
    /// Справочник валют
    /// <para />
    /// Схема http://www.cbr.ru/StaticHtml/File/92172/Valuta.xsd
    /// </summary>
    [XmlRoot(ElementName = "Valuta")]
    public class Valuta
    {
        [XmlElement(ElementName = "Item")]
        public List<ValutaItem> Items { get; set; }

        /// <summary>
        /// Имя документа
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}
