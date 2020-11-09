/* 
 * ArubaSignService
 *
 * <h1>ArubaSignService</h1>
 *
 * OpenAPI spec version: 2.3.2-SNAPSHOT
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
using SwaggerDateConverter = ArubaSignServiceRest.Client.SwaggerDateConverter;

namespace ArubaSignServiceRest.Model
{
    /// <summary>
    /// Options for a signature. This class can be used as-is, or as a base class for more specific use cases.
    /// </summary>
    [DataContract]
    public partial class JsonSignOptions :  IEquatable<JsonSignOptions>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSignOptions" /> class.
        /// </summary>
        /// <param name="identity">Object that cointains signer credentials for authentication..</param>
        /// <param name="certId">Cetificate id, for future use. For now set to \&quot;AS0\&quot;..</param>
        /// <param name="requiredMark">If true then a temporal mark is required..</param>
        /// <param name="signingTime">Signature date (time format is \&quot;dd/MM/yyyy HH:mm:ss\&quot;).&lt;br&gt;  If null, the current date is used..</param>
        /// <param name="sessionId">Session id used to create multiple signatures without specify PIN for every signature (in other case this value  must be null).&lt;br/&gt;  Use ASBService#opensession(it.arubapec.arubasignservice.Auth, java.lang.String) to get a session id and  ASBService#closesession(it.arubapec.arubasignservice.Auth, java.lang.String, java.lang.String)   to invalidate session..</param>
        /// <param name="tsaIdentity">Object that contains authentication data for TSA Server.&lt;br/&gt;  If not specified, a default account is used (if configured)..</param>
        public JsonSignOptions(JsonAuth identity = default(JsonAuth), string certId = default(string), bool? requiredMark = default(bool?), string signingTime = default(string), string sessionId = default(string), JsonTSAAuth tsaIdentity = default(JsonTSAAuth))
        {
            this.Identity = identity;
            this.CertId = certId;
            this.RequiredMark = requiredMark;
            this.SigningTime = signingTime;
            this.SessionId = sessionId;
            this.TsaIdentity = tsaIdentity;
        }
        
        /// <summary>
        /// Object that cointains signer credentials for authentication.
        /// </summary>
        /// <value>Object that cointains signer credentials for authentication.</value>
        [DataMember(Name="identity", EmitDefaultValue=false)]
        public JsonAuth Identity { get; set; }

        /// <summary>
        /// Cetificate id, for future use. For now set to \&quot;AS0\&quot;.
        /// </summary>
        /// <value>Cetificate id, for future use. For now set to \&quot;AS0\&quot;.</value>
        [DataMember(Name="certId", EmitDefaultValue=false)]
        public string CertId { get; set; }

        /// <summary>
        /// If true then a temporal mark is required.
        /// </summary>
        /// <value>If true then a temporal mark is required.</value>
        [DataMember(Name="requiredMark", EmitDefaultValue=false)]
        public bool? RequiredMark { get; set; }

        /// <summary>
        /// Signature date (time format is \&quot;dd/MM/yyyy HH:mm:ss\&quot;).&lt;br&gt;  If null, the current date is used.
        /// </summary>
        /// <value>Signature date (time format is \&quot;dd/MM/yyyy HH:mm:ss\&quot;).&lt;br&gt;  If null, the current date is used.</value>
        [DataMember(Name="signingTime", EmitDefaultValue=false)]
        public string SigningTime { get; set; }

        /// <summary>
        /// Session id used to create multiple signatures without specify PIN for every signature (in other case this value  must be null).&lt;br/&gt;  Use ASBService#opensession(it.arubapec.arubasignservice.Auth, java.lang.String) to get a session id and  ASBService#closesession(it.arubapec.arubasignservice.Auth, java.lang.String, java.lang.String)   to invalidate session.
        /// </summary>
        /// <value>Session id used to create multiple signatures without specify PIN for every signature (in other case this value  must be null).&lt;br/&gt;  Use ASBService#opensession(it.arubapec.arubasignservice.Auth, java.lang.String) to get a session id and  ASBService#closesession(it.arubapec.arubasignservice.Auth, java.lang.String, java.lang.String)   to invalidate session.</value>
        [DataMember(Name="sessionId", EmitDefaultValue=false)]
        public string SessionId { get; set; }

        /// <summary>
        /// Object that contains authentication data for TSA Server.&lt;br/&gt;  If not specified, a default account is used (if configured).
        /// </summary>
        /// <value>Object that contains authentication data for TSA Server.&lt;br/&gt;  If not specified, a default account is used (if configured).</value>
        [DataMember(Name="tsaIdentity", EmitDefaultValue=false)]
        public JsonTSAAuth TsaIdentity { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class JsonSignOptions {\n");
            sb.Append("  Identity: ").Append(Identity).Append("\n");
            sb.Append("  CertId: ").Append(CertId).Append("\n");
            sb.Append("  RequiredMark: ").Append(RequiredMark).Append("\n");
            sb.Append("  SigningTime: ").Append(SigningTime).Append("\n");
            sb.Append("  SessionId: ").Append(SessionId).Append("\n");
            sb.Append("  TsaIdentity: ").Append(TsaIdentity).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as JsonSignOptions);
        }

        /// <summary>
        /// Returns true if JsonSignOptions instances are equal
        /// </summary>
        /// <param name="input">Instance of JsonSignOptions to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(JsonSignOptions input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Identity == input.Identity ||
                    (this.Identity != null &&
                    this.Identity.Equals(input.Identity))
                ) && 
                (
                    this.CertId == input.CertId ||
                    (this.CertId != null &&
                    this.CertId.Equals(input.CertId))
                ) && 
                (
                    this.RequiredMark == input.RequiredMark ||
                    (this.RequiredMark != null &&
                    this.RequiredMark.Equals(input.RequiredMark))
                ) && 
                (
                    this.SigningTime == input.SigningTime ||
                    (this.SigningTime != null &&
                    this.SigningTime.Equals(input.SigningTime))
                ) && 
                (
                    this.SessionId == input.SessionId ||
                    (this.SessionId != null &&
                    this.SessionId.Equals(input.SessionId))
                ) && 
                (
                    this.TsaIdentity == input.TsaIdentity ||
                    (this.TsaIdentity != null &&
                    this.TsaIdentity.Equals(input.TsaIdentity))
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Identity != null)
                    hashCode = hashCode * 59 + this.Identity.GetHashCode();
                if (this.CertId != null)
                    hashCode = hashCode * 59 + this.CertId.GetHashCode();
                if (this.RequiredMark != null)
                    hashCode = hashCode * 59 + this.RequiredMark.GetHashCode();
                if (this.SigningTime != null)
                    hashCode = hashCode * 59 + this.SigningTime.GetHashCode();
                if (this.SessionId != null)
                    hashCode = hashCode * 59 + this.SessionId.GetHashCode();
                if (this.TsaIdentity != null)
                    hashCode = hashCode * 59 + this.TsaIdentity.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }
    }

}