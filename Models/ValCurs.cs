using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace ExampleApp.Models
{
    /// <summary>
    /// Официальные курсы валют на заданную дату, устанавливаемые ежедневно
    /// <para />
    /// Схема http://www.cbr.ru/StaticHtml/File/92172/ValCurs.xsd
    /// </summary>
    [XmlRoot(ElementName = "ValCurs")]
    public class ValCurs
    {
        [XmlElement(ElementName = "Valute")]
        public List<Valute> Valutes { get; set; }

        /// <summary>
        /// Дата установления курса (может отличатся от запрашиваемой если на запрашиваемою дату курс не устанавливался)
        /// </summary>
        [XmlAttribute(AttributeName = "Date")]
        public string DateString { get; set; }

        /// <summary>
        /// Дата установления курса (может отличатся от запрашиваемой если на запрашиваемою дату курс не устанавливался)
        /// </summary>
        [XmlIgnore]
        public DateTime? Date
        {
            get
            {
                DateTime? result = null;
                if (!string.IsNullOrWhiteSpace(DateString) &&
                    DateTime.TryParseExact(DateString, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    result = dt;
                }
                return result;
            }
        }

        /// <summary>
        /// Имя документа
        /// </summary>
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}
