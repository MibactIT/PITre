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
    /// Complex object that containing a verify request
    /// </summary>
    [DataContract]
    public partial class JsonVerifyRequest :  IEquatable<JsonVerifyRequest>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonVerifyRequest" /> class.
        /// </summary>
        /// <param name="binaryinput">Byte Array that contains the document to signature OR the hash for hash signature (only for Transport &#x3D; BINARYNET).</param>
        /// <param name="dstName">Destination file path OR destination directory path (only for Transport &#x3D; FILENAME or DIRECTORYNAME).</param>
        /// <param name="notityId">ID associated to the signatures session for notifications.</param>
        /// <param name="notitymail">Email for notifications (withe relative ID).</param>
        /// <param name="srcName">Path to file OR directory path that contains the files to be signed (only for Transport &#x3D; FILENAME or DIRECTORYNAME).</param>
        /// <param name="stream">Stream used to load large size file (only for Transport &#x3D; STREAM).</param>
        /// <param name="transport">Enum Class that indicates the type of input (trasport). &lt;br/&gt;  BYNARYNET, FILENAME, DIRECTORYNAME, STREAM.</param>
        /// <param name="type">Enum Class that indicates the type of document. &lt;br/&gt;  PKCS7, PDF, XML, TSD.</param>
        /// <param name="verdate">Date of verification..</param>
        public JsonVerifyRequest(string binaryinput = default(string), string dstName = default(string), string notityId = default(string), string notitymail = default(string), string srcName = default(string), string stream = default(string), JsonTypeTransport transport = default(JsonTypeTransport), JsonDocumentType type = default(JsonDocumentType), string verdate = default(string))
        {
            this.Binaryinput = binaryinput;
            this.DstName = dstName;
            this.NotityId = notityId;
            this.Notitymail = notitymail;
            this.SrcName = srcName;
            this.Stream = stream;
            this.Transport = transport;
            this.Type = type;
            this.Verdate = verdate;
        }
        
        /// <summary>
        /// Byte Array that contains the document to signature OR the hash for hash signature (only for Transport &#x3D; BINARYNET)
        /// </summary>
        /// <value>Byte Array that contains the document to signature OR the hash for hash signature (only for Transport &#x3D; BINARYNET)</value>
        [DataMember(Name="binaryinput", EmitDefaultValue=false)]
        public string Binaryinput { get; set; }

        /// <summary>
        /// Destination file path OR destination directory path (only for Transport &#x3D; FILENAME or DIRECTORYNAME)
        /// </summary>
        /// <value>Destination file path OR destination directory path (only for Transport &#x3D; FILENAME or DIRECTORYNAME)</value>
        [DataMember(Name="dstName", EmitDefaultValue=false)]
        public string DstName { get; set; }

        /// <summary>
        /// ID associated to the signatures session for notifications
        /// </summary>
        /// <value>ID associated to the signatures session for notifications</value>
        [DataMember(Name="notity_id", EmitDefaultValue=false)]
        public string NotityId { get; set; }

        /// <summary>
        /// Email for notifications (withe relative ID)
        /// </summary>
        /// <value>Email for notifications (withe relative ID)</value>
        [DataMember(Name="notitymail", EmitDefaultValue=false)]
        public string Notitymail { get; set; }

        /// <summary>
        /// Path to file OR directory path that contains the files to be signed (only for Transport &#x3D; FILENAME or DIRECTORYNAME)
        /// </summary>
        /// <value>Path to file OR directory path that contains the files to be signed (only for Transport &#x3D; FILENAME or DIRECTORYNAME)</value>
        [DataMember(Name="srcName", EmitDefaultValue=false)]
        public string SrcName { get; set; }

        /// <summary>
        /// Stream used to load large size file (only for Transport &#x3D; STREAM)
        /// </summary>
        /// <value>Stream used to load large size file (only for Transport &#x3D; STREAM)</value>
        [DataMember(Name="stream", EmitDefaultValue=false)]
        public string Stream { get; set; }

        /// <summary>
        /// Enum Class that indicates the type of input (trasport). &lt;br/&gt;  BYNARYNET, FILENAME, DIRECTORYNAME, STREAM
        /// </summary>
        /// <value>Enum Class that indicates the type of input (trasport). &lt;br/&gt;  BYNARYNET, FILENAME, DIRECTORYNAME, STREAM</value>
        [DataMember(Name="transport", EmitDefaultValue=false)]
        public JsonTypeTransport Transport { get; set; }

        /// <summary>
        /// Enum Class that indicates the type of document. &lt;br/&gt;  PKCS7, PDF, XML, TSD
        /// </summary>
        /// <value>Enum Class that indicates the type of document. &lt;br/&gt;  PKCS7, PDF, XML, TSD</value>
        [DataMember(Name="type", EmitDefaultValue=false)]
        public JsonDocumentType Type { get; set; }

        /// <summary>
        /// Date of verification.
        /// </summary>
        /// <value>Date of verification.</value>
        [DataMember(Name="verdate", EmitDefaultValue=false)]
        public string Verdate { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class JsonVerifyRequest {\n");
            sb.Append("  Binaryinput: ").Append(Binaryinput).Append("\n");
            sb.Append("  DstName: ").Append(DstName).Append("\n");
            sb.Append("  NotityId: ").Append(NotityId).Append("\n");
            sb.Append("  Notitymail: ").Append(Notitymail).Append("\n");
            sb.Append("  SrcName: ").Append(SrcName).Append("\n");
            sb.Append("  Stream: ").Append(Stream).Append("\n");
            sb.Append("  Transport: ").Append(Transport).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Verdate: ").Append(Verdate).Append("\n");
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
            return this.Equals(input as JsonVerifyRequest);
        }

        /// <summary>
        /// Returns true if JsonVerifyRequest instances are equal
        /// </summary>
        /// <param name="input">Instance of JsonVerifyRequest to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(JsonVerifyRequest input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Binaryinput == input.Binaryinput ||
                    (this.Binaryinput != null &&
                    this.Binaryinput.Equals(input.Binaryinput))
                ) && 
                (
                    this.DstName == input.DstName ||
                    (this.DstName != null &&
                    this.DstName.Equals(input.DstName))
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
                    this.Stream == input.Stream ||
                    (this.Stream != null &&
                    this.Stream.Equals(input.Stream))
                ) && 
                (
                    this.Transport == input.Transport ||
                    (this.Transport != null &&
                    this.Transport.Equals(input.Transport))
                ) && 
                (
                    this.Type == input.Type ||
                    (this.Type != null &&
                    this.Type.Equals(input.Type))
                ) && 
                (
                    this.Verdate == input.Verdate ||
                    (this.Verdate != null &&
                    this.Verdate.Equals(input.Verdate))
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
                if (this.Binaryinput != null)
                    hashCode = hashCode * 59 + this.Binaryinput.GetHashCode();
                if (this.DstName != null)
                    hashCode = hashCode * 59 + this.DstName.GetHashCode();
                if (this.NotityId != null)
                    hashCode = hashCode * 59 + this.NotityId.GetHashCode();
                if (this.Notitymail != null)
                    hashCode = hashCode * 59 + this.Notitymail.GetHashCode();
                if (this.SrcName != null)
                    hashCode = hashCode * 59 + this.SrcName.GetHashCode();
                if (this.Stream != null)
                    hashCode = hashCode * 59 + this.Stream.GetHashCode();
                if (this.Transport != null)
                    hashCode = hashCode * 59 + this.Transport.GetHashCode();
                if (this.Type != null)
                    hashCode = hashCode * 59 + this.Type.GetHashCode();
                if (this.Verdate != null)
                    hashCode = hashCode * 59 + this.Verdate.GetHashCode();
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
