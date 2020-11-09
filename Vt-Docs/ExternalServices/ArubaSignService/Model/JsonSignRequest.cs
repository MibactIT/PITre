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
    /// Object that contains a Signature Request &lt;br/&gt;  The order of fields is important for WSDL
    /// </summary>
    [DataContract]
    public partial class JsonSignRequest :  IEquatable<JsonSignRequest>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSignRequest" /> class.
        /// </summary>
        /// <param name="bynaryinput">Byte Array that contains the document (only for Transport &#x3D; BINARYNET).</param>
        /// <param name="certID">Cetificate Id (for future use). For now set to &#39;AS0&#39;.</param>
        /// <param name="dstNmae">Destination file path OR destination directory path (only for Transport &#x3D; FILENAME or DIRECTORYNAME).</param>
        /// <param name="identity">Object that cointains Signer Credentials for authentication.</param>
        /// <param name="notityId">ID associated with the signatures session for notifications.</param>
        /// <param name="notitymail">Email for notifications.</param>
        /// <param name="srcName">Path to file OR directory path that contains the files to be signed (only for Transport &#x3D; FILENAME or DIRECTORYNAME).</param>
        /// <param name="transport">Enum Class that indicates the type of input (trasport).</param>
        public JsonSignRequest(string bynaryinput = default(string), string certID = default(string), string dstNmae = default(string), JsonAuth identity = default(JsonAuth), string notityId = default(string), string notitymail = default(string), string srcName = default(string), JsonTypeTransport transport = default(JsonTypeTransport))
        {
            this.Bynaryinput = bynaryinput;
            this.CertID = certID;
            this.DstNmae = dstNmae;
            this.Identity = identity;
            this.NotityId = notityId;
            this.Notitymail = notitymail;
            this.SrcName = srcName;
            this.Transport = transport;
        }
        
        /// <summary>
        /// Byte Array that contains the document (only for Transport &#x3D; BINARYNET)
        /// </summary>
        /// <value>Byte Array that contains the document (only for Transport &#x3D; BINARYNET)</value>
        [DataMember(Name="bynaryinput", EmitDefaultValue=false)]
        public string Bynaryinput { get; set; }

        /// <summary>
        /// Cetificate Id (for future use). For now set to &#39;AS0&#39;
        /// </summary>
        /// <value>Cetificate Id (for future use). For now set to &#39;AS0&#39;</value>
        [DataMember(Name="certID", EmitDefaultValue=false)]
        public string CertID { get; set; }

        /// <summary>
        /// Destination file path OR destination directory path (only for Transport &#x3D; FILENAME or DIRECTORYNAME)
        /// </summary>
        /// <value>Destination file path OR destination directory path (only for Transport &#x3D; FILENAME or DIRECTORYNAME)</value>
        [DataMember(Name="dstNmae", EmitDefaultValue=false)]
        public string DstNmae { get; set; }

        /// <summary>
        /// Object that cointains Signer Credentials for authentication
        /// </summary>
        /// <value>Object that cointains Signer Credentials for authentication</value>
        [DataMember(Name="identity", EmitDefaultValue=false)]
        public JsonAuth Identity { get; set; }

        /// <summary>
        /// ID associated with the signatures session for notifications
        /// </summary>
        /// <value>ID associated with the signatures session for notifications</value>
        [DataMember(Name="notity_id", EmitDefaultValue=false)]
        public string NotityId { get; set; }

        /// <summary>
        /// Email for notifications
        /// </summary>
        /// <value>Email for notifications</value>
        [DataMember(Name="notitymail", EmitDefaultValue=false)]
        public string Notitymail { get; set; }

        /// <summary>
        /// Path to file OR directory path that contains the files to be signed (only for Transport &#x3D; FILENAME or DIRECTORYNAME)
        /// </summary>
        /// <value>Path to file OR directory path that contains the files to be signed (only for Transport &#x3D; FILENAME or DIRECTORYNAME)</value>
        [DataMember(Name="srcName", EmitDefaultValue=false)]
        public string SrcName { get; set; }

        /// <summary>
        /// Enum Class that indicates the type of input (trasport)
        /// </summary>
        /// <value>Enum Class that indicates the type of input (trasport)</value>
        [DataMember(Name="transport", EmitDefaultValue=false)]
        public JsonTypeTransport Transport { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class JsonSignRequest {\n");
            sb.Append("  Bynaryinput: ").Append(Bynaryinput).Append("\n");
            sb.Append("  CertID: ").Append(CertID).Append("\n");
            sb.Append("  DstNmae: ").Append(DstNmae).Append("\n");
            sb.Append("  Identity: ").Append(Identity).Append("\n");
            sb.Append("  NotityId: ").Append(NotityId).Append("\n");
            sb.Append("  Notitymail: ").Append(Notitymail).Append("\n");
            sb.Append("  SrcName: ").Append(SrcName).Append("\n");
            sb.Append("  Transport: ").Append(Transport).Append("\n");
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
            return this.Equals(input as JsonSignRequest);
        }

        /// <summary>
        /// Returns true if JsonSignRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of JsonSignRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(JsonSignRequest input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Bynaryinput == input.Bynaryinput ||
                    (this.Bynaryinput != null &&
                    this.Bynaryinput.Equals(input.Bynaryinput))
                ) && 
                (
                    this.CertID == input.CertID ||
                    (this.CertID != null &&
                    this.CertID.Equals(input.CertID))
                ) && 
                (
                    this.DstNmae == input.DstNmae ||
                    (this.DstNmae != null &&
                    this.DstNmae.Equals(input.DstNmae))
                ) && 
                (
                    this.Identity == input.Identity ||
                    (this.Identity != null &&
                    this.Identity.Equals(input.Identity))
                ) && 
                (
                    this.NotityId == input.NotityId ||
                    (this.NotityId != null &&
                    this.NotityId.Equals(input.NotityId))
                ) && 
                (
                    this.Notitymail == input.Notitymail ||
                    (this.Notitymail != null &&
                    this.Notitymail.Equals(input.Notitymail))
                ) && 
                (
                    this.SrcName == input.SrcName ||
                    (this.SrcName != null &&
                    this.SrcName.Equals(input.SrcName))
                ) && 
                (
                    this.Transport == input.Transport ||
                    (this.Transport != null &&
                    this.Transport.Equals(input.Transport))
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
                if (this.Bynaryinput != null)
                    hashCode = hashCode * 59 + this.Bynaryinput.GetHashCode();
                if (this.CertID != null)
                    hashCode = hashCode * 59 + this.CertID.GetHashCode();
                if (this.DstNmae != null)
                    hashCode = hashCode * 59 + this.DstNmae.GetHashCode();
                if (this.Identity != null)
                    hashCode = hashCode * 59 + this.Identity.GetHashCode();
                if (this.NotityId != null)
                    hashCode = hashCode * 59 + this.NotityId.GetHashCode();
                if (this.Notitymail != null)
                    hashCode = hashCode * 59 + this.Notitymail.GetHashCode();
                if (this.SrcName != null)
                    hashCode = hashCode * 59 + this.SrcName.GetHashCode();
                if (this.Transport != null)
                    hashCode = hashCode * 59 + this.Transport.GetHashCode();
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