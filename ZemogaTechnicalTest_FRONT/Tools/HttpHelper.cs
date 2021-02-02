using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ZemogaTechnicalTest_FRONT.Tools
{
    public static class HttpHelper
    {
        /// <summary>
        ///     Tipo de contenido que acepta la petición.
        /// </summary>
        public enum Accept
        {
            [Description("application/json")] JSON,
            [Description("application/xml")] XML
        }

        /// <summary>
        ///     Tipo de contenido que se puede enviar al servicio.
        /// </summary>
        public enum ContentType
        {
            [Description("application/json")] JSON,
            [Description("application/xml")] XML
        }

        /// <summary>
        ///     Protocolo de la petición
        /// </summary>
        public enum RequestType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

        /// <summary>
        ///     Obtiene la descripción (anotación) del valor del enum
        /// </summary>
        /// <param name="value">Valor del enumerador para obtener la opción</param>
        /// <returns>Texto incluido en el campo description del valor del enum.</returns>
        private static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            return value.ToString();
        }

        /// <summary>
        ///     Deserializa el objeto de salida de la peticion JSON al objeto enviado como entidad T
        /// </summary>
        /// <typeparam name="T">Tipo de objeto para cast</typeparam>
        /// <param name="data">Datos a reserializar</param>
        /// <returns>Entidad T como objeto compuesto.</returns>
        private static T DeserializeJSON<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        ///     Realiza la conversión de una cadena de texto XML a un objeto compuesto.
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a convertir</typeparam>
        /// <param name="data">Cadena de texto con el objeto a convertir</param>
        /// <returns>Objeto compuesto con los valores del XML.</returns>
        private static T DeserializeXML<T>(string data)
        {
            var _response = new XmlSerializer(typeof(T));
            using (var sr = new StringReader(data))
            {
                try
                {
                    return (T)_response.Deserialize(sr);
                }
#pragma warning disable CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                catch (Exception ex)
#pragma warning restore CS0168 // La variable 'ex' se ha declarado pero nunca se usa
                {
                    return (T)Convert.ChangeType(data, typeof(T));
                }
            }
        }

        /// <summary>
        ///     Realiza las peticiones HTTP sobre los servicios CLOUD que cuenten con un API REST consumible desde C#
        /// </summary>
        /// <typeparam name="T">Tipo de objeto a tratar</typeparam>
        /// <param name="URI">Endpoint del servicio</param>
        /// <param name="token">Data de autenticación</param>
        /// <param name="type">Protocolo de la petición</param>
        /// <param name="contentType">Tipo de contenido a enviarse</param>
        /// <param name="accept">Tipo de contenido que acepta la petición</param>
        /// <param name="data">Datos que se envian en caso de requerirse</param>
        /// <returns>El resultado de la petición</returns>
        public static T CreateNetworkRequest<T>(string URI, string token, RequestType type, ContentType contentType,
            Accept accept, string customHeader = null, string data = null)
        {
            var _request = (HttpWebRequest)WebRequest.Create(URI);
            if (customHeader == null)
                _request.Headers.Add(HttpRequestHeader.Authorization, token);
            else
                _request.Headers.Add(customHeader, token);
            _request.ContentType = GetEnumDescription(contentType);
            _request.Method = type.ToString();
            _request.Accept = GetEnumDescription(accept);
            _request.KeepAlive = false;
            _request.Timeout = 200000;

            switch (type)
            {
                case RequestType.POST:
                case RequestType.PUT:
                    if (data != null)
                    {
                        var _requestData = Encoding.UTF8.GetBytes(data);
                        _request.ContentLength = _requestData.Length;
                        using (var st = _request.GetRequestStream())
                        {
                            st.Write(_requestData, 0, _requestData.Length);
                        }
                    }

                    break;
            }

            using (var _response = _request.GetResponse())
            {
                using (var s = _response.GetResponseStream())
                {
                    using (var sr = new StreamReader(s))
                    {
                        var _output = sr.ReadToEnd();
                        if (!string.IsNullOrEmpty(_output))
                            switch (contentType)
                            {
                                case ContentType.JSON:
                                    return DeserializeJSON<T>(_output);
                                case ContentType.XML:
                                    return DeserializeXML<T>(_output);
                                default:
                                    return default(T);
                            }

                        return default(T);
                    }
                }
            }
        }
    }
}
