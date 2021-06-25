using System.Diagnostics;
using System.Xml.Serialization;

namespace ExampleApp.Models
{
    /// <summary>
    /// Схема http://www.cbr.ru/StaticHtml/File/92172/ValCurs.xsd
    /// </summary>
    [DebuggerDisplay("{CharCode} = {Value}")]
    [XmlRoot(ElementName = "Valute")]
    public class Valute
    {
        /// <summary>
        /// ISO Цифр. код валюты 
        /// </summary>
        [XmlElement(ElementName = "NumCode")]
        public uint NumCode { get; set; }

        /// <summary>
        /// ISO Букв. код валюты
        /// </summary>
        [XmlElement(ElementName = "CharCode")]
        public string CharCode { get; set; }

        /// <summary>
        /// номинал. ед
        /// </summary>
        [XmlElement(ElementName = "Nominal")]
        public uint Nominal { get; set; }

        /// <summary>
        /// Название валюты
        /// </summary>
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// Значение (строка)
        /// </summary>
        [XmlElement(ElementName = "Value")]
        public string ValueString { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        [XmlIgnore]
        public decimal? Value
        {
            get
            {
                decimal? result = null;

                if (!string.IsNullOrWhiteSpace(ValueString) && decimal.TryParse(ValueString, out decimal val))
                {
                    result = val;
                }

                return result;
            }
        }

        /// <summary>
        /// Внутренний уникальный код валюты
        /// </summary>
        [XmlAttribute(AttributeName = "ID")]
        public string Id { get; set; }
    }
}
