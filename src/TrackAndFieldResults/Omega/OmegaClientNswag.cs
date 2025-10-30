/*
 * SPDX - FileCopyrightText: Copyright © 2025 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * loosly taken from https://github.com/RicoSuter/NSwag
 * 
The MIT License (MIT)

Copyright (c) 2021 Rico Suter

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

 */
using System.Text;

namespace TrackAndFieldResults.Omega
{
    /// <summary>
    /// Json client for getting Omega results
    /// </summary>
    /// <remarks>NSwag inspired, because Seltec uses swagger. so sehen die APIs ähnlich aus.</remarks>
    public partial class OmegaClient
    {
        private readonly HttpClient _httpClient;
        private string _baseUrl;
        private static System.Lazy<System.Text.Json.JsonSerializerOptions> _settings = new System.Lazy<System.Text.Json.JsonSerializerOptions>(CreateSerializerSettings, true);
        private System.Text.Json.JsonSerializerOptions _instanceSettings;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public OmegaClient(System.Net.Http.HttpClient httpClient)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            BaseUrl = "https://ps-cache.web.swisstiming.com";
            _httpClient = httpClient;
            Initialize();
        }

        private static System.Text.Json.JsonSerializerOptions CreateSerializerSettings()
        {
            var settings = new System.Text.Json.JsonSerializerOptions();
            UpdateJsonSerializerSettings(settings);
            return settings;
        }
        protected System.Text.Json.JsonSerializerOptions JsonSerializerSettings { get { return _instanceSettings ?? _settings.Value; } }

        static partial void UpdateJsonSerializerSettings(System.Text.Json.JsonSerializerOptions settings);

        partial void Initialize();
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url);
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder);
        partial void ProcessResponse(System.Net.Http.HttpClient client, System.Net.Http.HttpResponseMessage response);

        public string BaseUrl
        {
            get { return _baseUrl; }
            set
            {
                _baseUrl = value;
                if (!string.IsNullOrEmpty(_baseUrl) && !_baseUrl.EndsWith("/"))
                    _baseUrl += '/';
            }
        }

        /// <summary>
        /// Generiert aus dem Wettkampfnamen die Url
        /// für den Download der Wettkmapf-Details
        /// </summary>
        /// <param name="competitionKey">Name des Wettkampfes aus Omega-Liste</param>
        /// <returns></returns>
        public string GetCompetitionDetailsUrl(string competitionKey)
        {
            return $"node/db/ATH_PROD/{competitionKey.ToUpper()}_CURRENTEVENT_JSON.json?s=unknown&t=0";
        }

        /// <summary>
        /// Gibt die Url für die Übersichtsseite einiger
        /// Wettkämpfe zurück. Meist sind ältere Wettkämpfe
        /// noch erreichbar
        /// </summary>
        /// <returns></returns>
        public string GetCompetitionsUrl()
        {
            return $"node/db/ATH_PROD/CIS_CONFIG_STATUS_JSON.json";
        }

        /// <summary>
        /// Erstellt die Url für den Zeitplan eines Wettkampfes
        /// </summary>
        /// <param name="competitionKey">Event-Name des Wettkampfes aus der competitioins list</param>
        /// <returns></returns>
        public string GetScheduleUrl(string competitionKey)
        {
            return $"node/db/ATH_PROD/{competitionKey.ToUpper()}_SCHEDULE_JSON.json";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="competitionKey">Event-Id des Wettkampfes</param>
        /// <param name="eventKey"></param>
        /// <returns></returns>
        public string GetEventUrl(string competitionKey, string eventKey)
        {
            return $"node/db/ATH_PROD/{competitionKey.ToUpper()}_TIMING_{eventKey}_JSON.json";
        }

        /// <summary>
        /// Get a list of known competitions, but you won't get the results 
        /// of all competitons in this list. Some older competitons are 
        /// removed by Omega. Sometimes you can get the data of previous
        /// competitions, which are not in the list.
        /// </summary>
        /// <param name="cancellationToken">A cancellationToken</param>
        /// <returns>A list of available competitions</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public virtual async Task<CompetitionsRoot> GetCompetitionsAsync(System.Threading.CancellationToken cancellationToken)
        {
            return await GetJsonAsync<CompetitionsRoot>(GetCompetitionsUrl(), cancellationToken);
        }

        /// <summary>
        /// Get a list of known competitions, but you won't get the results 
        /// of all competitons in this list. Some older competitons are 
        /// removed by Omega. Sometimes you can get the data of previous
        /// competitions, which are not in the list.
        /// </summary>
        /// <returns>A list of available competitions</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public virtual async Task<CompetitionsRoot> GetCompetitionsAsync()
        {
            return await GetJsonAsync<CompetitionsRoot>(GetCompetitionsUrl(), System.Threading.CancellationToken.None);
        }


        /// <summary>
        /// Gets the details of a competition. You will find the name
        /// and date of competition here.
        /// </summary>
        /// <param name="competitionKey">The competition-Id from the competitions list</param>
        /// <param name="cancellationToken">A cancellationToken</param>
        /// <returns>the details of a competition</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public virtual async Task<CompetitionRoot> GetCompetitionDetailsAsync(string competitionKey, System.Threading.CancellationToken cancellationToken)
        {
            return await GetJsonAsync<CompetitionRoot>(GetCompetitionDetailsUrl(competitionKey), cancellationToken);
            
        }

        /// <summary>
        /// Gets the details of a competition. You will find the name
        /// and date of competition here.
        /// </summary>
        /// <param name="competitionKey">The competition-Id from the competitions list</param>
        /// <returns>the details of a competition</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public virtual async Task<CompetitionRoot> GetCompetitionDetailsAsync(string competitionKey)
        {
            return await GetJsonAsync<CompetitionRoot>(GetCompetitionDetailsUrl(competitionKey), System.Threading.CancellationToken.None);
            
        }

        /// <summary>
        /// Gets the schedule of a competition.You will find all events/disiplins of
        /// a competition here.
        /// </summary>
        /// <param name="competitionKey">The competition-Id from the competitions list</param>
        /// <param name="cancellationToken">A cancellationToken</param>
        /// <returns>the schedule of a competition</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public virtual async Task<ScheduleRoot> GetScheduleAsync(string competitionKey, System.Threading.CancellationToken cancellationToken)
        {
            return await GetJsonAsync<ScheduleRoot>(GetScheduleUrl(competitionKey), cancellationToken);
        }

        /// <summary>
        /// Gets the schedule of a competition.You will find all events/disiplins of
        /// a competition here.
        /// </summary>
        /// <param name="competitionKey">The competition-Id from the competitions list</param>
        /// <returns>the schedule of a competition</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public virtual async Task<ScheduleRoot> GetScheduleAsync(string competitionKey)
        {
            return await GetJsonAsync<ScheduleRoot>(GetScheduleUrl(competitionKey), System.Threading.CancellationToken.None);
        }

        /// <summary>
        /// Gets the details of a event of a competition. You will find the start-, 
        /// competitor- and resultlists.
        /// </summary>
        /// <param name="competitionKey">The competition-Id from the competitions list</param>
        /// <param name="cancellationToken">A cancellationToken</param>
        /// <returns>the schedule of a competition</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public virtual async Task<EventRoot> GetEventDetailsAsync(string competitionKey, string eventKey, System.Threading.CancellationToken cancellationToken)
        {
            return await GetJsonAsync<EventRoot>(GetEventUrl(competitionKey, eventKey), cancellationToken);
            
        }

        /// <summary>
        /// Gets the details of a event of a competition. You will find the start-, 
        /// competitor- and resultlists.
        /// </summary>
        /// <param name="competitionKey">The competition-Id from the competitions list</param>
        /// <returns>the schedule of a competition</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public virtual async Task<EventRoot> GetEventDetailsAsync(string competitionKey, string eventKey)
        {
            return await GetJsonAsync<EventRoot>(GetEventUrl(competitionKey, eventKey), System.Threading.CancellationToken.None);
            
        }

        private async Task<T> GetJsonAsync<T>(string url, System.Threading.CancellationToken cancellationToken)
        {
            var client_ = _httpClient;
            var disposeClient_ = false;
            try
            {
                using (var request_ = new System.Net.Http.HttpRequestMessage())
                {

                    request_.Method = new System.Net.Http.HttpMethod("GET");

                    var urlBuilder_ = new System.Text.StringBuilder();
                    if (!string.IsNullOrEmpty(_baseUrl)) urlBuilder_.Append(_baseUrl);
                    
                    urlBuilder_.Append(url);
                    
                    PrepareRequest(client_, request_, urlBuilder_);

                    var url_ = urlBuilder_.ToString();
                    request_.RequestUri = new System.Uri(url_, System.UriKind.RelativeOrAbsolute);

                    PrepareRequest(client_, request_, url_);

                    var response_ = await client_.SendAsync(request_, System.Net.Http.HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
                    var disposeResponse_ = true;
                    try
                    {
                        var headers_ = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.IEnumerable<string>>();
                        foreach (var item_ in response_.Headers)
                            headers_[item_.Key] = item_.Value;
                        if (response_.Content != null && response_.Content.Headers != null)
                        {
                            foreach (var item_ in response_.Content.Headers)
                                headers_[item_.Key] = item_.Value;
                        }

                        ProcessResponse(client_, response_);

                        var status_ = (int)response_.StatusCode;
                        if (status_ == 200)
                        {
                            var objectResponse_ = await ReadObjectResponseAsync<T>(response_, headers_, cancellationToken).ConfigureAwait(false);
                            if (objectResponse_.Object == null)
                            {
                                throw new ApiException("Response was null which was not expected.", status_, objectResponse_.Text, headers_, null);
                            }
                            return objectResponse_.Object;
                        }
                        else
                        if (status_ == 403)
                        {
                            string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                            throw new ApiException("A server side error occurred.", status_, responseText_, headers_, null);
                        }
                        else
                        if (status_ == 404)
                        {
                            string responseText_ = (response_.Content == null) ? string.Empty : await response_.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                            throw new ApiException("A server side error occurred.", status_, responseText_, headers_, null);
                        }
                        else
                        {
                            var responseData_ = response_.Content == null ? null : await response_.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                            throw new ApiException("The HTTP status code of the response was not expected (" + status_ + ").", status_, responseData_, headers_, null);
                        }
                    }
                    finally
                    {
                        if (disposeResponse_)
                            response_.Dispose();
                    }
                }
            }
            finally
            {
                if (disposeClient_)
                    client_.Dispose();
            }
        }


        private string ConvertToString(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return "";
            }

            if (value is System.Enum)
            {
                var name = System.Enum.GetName(value.GetType(), value);
                if (name != null)
                {
                    var field = System.Reflection.IntrospectionExtensions.GetTypeInfo(value.GetType()).GetDeclaredField(name);
                    if (field != null)
                    {
                        var attribute = System.Reflection.CustomAttributeExtensions.GetCustomAttribute(field, typeof(System.Runtime.Serialization.EnumMemberAttribute))
                            as System.Runtime.Serialization.EnumMemberAttribute;
                        if (attribute != null)
                        {
                            return attribute.Value != null ? attribute.Value : name;
                        }
                    }

                    var converted = System.Convert.ToString(System.Convert.ChangeType(value, System.Enum.GetUnderlyingType(value.GetType()), cultureInfo));
                    return converted == null ? string.Empty : converted;
                }
            }
            else if (value is bool)
            {
                return System.Convert.ToString((bool)value, cultureInfo).ToLowerInvariant();
            }
            else if (value is byte[])
            {
                return System.Convert.ToBase64String((byte[])value);
            }
            else if (value is string[])
            {
                return string.Join(",", (string[])value);
            }
            else if (value.GetType().IsArray)
            {
                var valueArray = (System.Array)value;
                var valueTextArray = new string[valueArray.Length];
                for (var i = 0; i < valueArray.Length; i++)
                {
                    valueTextArray[i] = ConvertToString(valueArray.GetValue(i), cultureInfo);
                }
                return string.Join(",", valueTextArray);
            }

            var result = System.Convert.ToString(value, cultureInfo);
            return result == null ? "" : result;
        }

        protected virtual async System.Threading.Tasks.Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(System.Net.Http.HttpResponseMessage response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Threading.CancellationToken cancellationToken)
        {
            if (response == null || response.Content == null)
            {
                return new ObjectResponseResult<T>(default(T), string.Empty);
            }

            if (ReadResponseAsString)
            {
                ResponseText = await ReadAsStringAsync(response.Content, cancellationToken).ConfigureAwait(false);
                try
                {
                    var typedBody = System.Text.Json.JsonSerializer.Deserialize<T>(ResponseText, JsonSerializerSettings);
                    return new ObjectResponseResult<T>(typedBody, ResponseText);
                }
                catch (System.Text.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, ResponseText, headers, exception);
                }
            }
            else
            {
                try
                {
                    using (var responseStream = await ReadAsStreamAsync(response.Content, cancellationToken).ConfigureAwait(false))
                    {
                        var typedBody = await System.Text.Json.JsonSerializer.DeserializeAsync<T>(responseStream, JsonSerializerSettings, cancellationToken).ConfigureAwait(false);
                        return new ObjectResponseResult<T>(typedBody, string.Empty);
                    }
                }
                catch (System.Text.Json.JsonException exception)
                {
                    var message = "Could not deserialize the response body stream as " + typeof(T).FullName + ".";
                    throw new ApiException(message, (int)response.StatusCode, string.Empty, headers, exception);
                }
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static System.Threading.Tasks.Task<string> ReadAsStringAsync(System.Net.Http.HttpContent content, System.Threading.CancellationToken cancellationToken)
        {
#if NET5_0_OR_GREATER
            return content.ReadAsStringAsync(cancellationToken);
#else
            return content.ReadAsStringAsync();
#endif
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static System.Threading.Tasks.Task<System.IO.Stream> ReadAsStreamAsync(System.Net.Http.HttpContent content, System.Threading.CancellationToken cancellationToken)
        {
#if NET5_0_OR_GREATER
            return content.ReadAsStreamAsync(cancellationToken);
#else
            return content.ReadAsStreamAsync();
#endif
        }

        protected struct ObjectResponseResult<T>
        {
            public ObjectResponseResult(T responseObject, string responseText)
            {
                this.Object = responseObject;
                this.Text = responseText;
            }

            public T Object { get; }

            public string Text { get; }
        }

        public bool ReadResponseAsString { get; set; }
        /// <summary>
        /// Raw text of the server response for caching or writing
        /// to a file
        /// </summary>
        public string ResponseText { get; set; }

        /// <summary>
        /// Writes the contents of the response text to a file
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveResponseText(string fileName)
        {
            using (var fs = new StreamWriter(fileName,
                    false, Encoding.UTF8))
            {
                fs.Write(ResponseText);
            }
        }


    }

    [System.CodeDom.Compiler.GeneratedCode("NSwag", "13.8.2.0 (NJsonSchema v10.2.1.0 (Newtonsoft.Json v11.0.0.0))")]
    public partial class ApiException : System.Exception
    {
        public int StatusCode { get; private set; }

        public string Response { get; private set; }

        public System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> Headers { get; private set; }

        public ApiException(string message, int statusCode, string response, System.Collections.Generic.IReadOnlyDictionary<string, System.Collections.Generic.IEnumerable<string>> headers, System.Exception innerException)
            : base(message + "\n\nStatus: " + statusCode + "\nResponse: \n" + ((response == null) ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)), innerException)
        {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }

        public override string ToString()
        {
            return string.Format("HTTP Response: \n\n{0}\n\n{1}", Response, base.ToString());
        }
    }
}

