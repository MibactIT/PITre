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
    /// Object contains the result of signature
    /// </summary>
    [DataContract]
    public partial class JsonSignReturnV2 :  IEquatable<JsonSignReturnV2>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSignReturnV2" /> class.
        /// </summary>
        /// <param name="binaryoutput">Byte Array of signed file.</param>
        /// <param name="description">Description of generated error and/or details for error code returned.</param>
        /// <param name="dstPath">File path specified in request used by server&lt;br/&gt;  In case of collision, the server don&#39;t overwrites the files and uses files like (1..9).</param>
        /// <param name="returnCode">Enum that indicates error code:  &lt;ul&gt;  &lt;li&gt;\&quot;0001\&quot; -&gt; Generic Error&lt;/li&gt;  &lt;li&gt;\&quot;0002\&quot; -&gt; Incorrect parameters for trasportation type selected&lt;/li&gt;  &lt;li&gt;\&quot;0003\&quot; -&gt; Error after credential check&lt;/li&gt;  &lt;li&gt;\&quot;0004\&quot; -&gt; PIN Error. Details in description field)&lt;/li&gt;  &lt;li&gt;\&quot;0005\&quot; -&gt; Trasportation type not valid&lt;/li&gt;  &lt;li&gt;\&quot;0006\&quot; -&gt; Trasportation type not authorized&lt;/li&gt;  &lt;li&gt;\&quot;0007\&quot; -&gt; Profile of PDF Signature not valid&lt;/li&gt;  &lt;li&gt;\&quot;0008\&quot; -&gt; Impossible to complete time-stamp marking operation&lt;/li&gt;  &lt;li&gt;\&quot;0009\&quot; -&gt; Delegated credential not valid&lt;/li&gt;  &lt;li&gt;\&quot;0010\&quot; -&gt; User status not valid&lt;/li&gt;  &lt;/ul&gt;.</param>
        /// <param name="status">Result of signature:  &lt;ul&gt;  &lt;li&gt;OK&lt;/li&gt;  &lt;li&gt;{errore}: See description and return_code for more details&lt;/li&gt;  &lt;/ul&gt;.</param>
        /// <param name="stream">Stream used to download large size file (only for Transport &#x3D; STREAM).</param>
        /// <param name="code">For compatibility with new architecture ONLY. DO NOT USE!.</param>
        /// <param name="message">For compatibility with new architecture ONLY. DO NOT USE!.</param>
        public JsonSignReturnV2(string binaryoutput = default(string), string description = default(string), string dstPath = default(string), string returnCode = default(string), string status = default(string), string stream = default(string), string code = default(string), string message = default(string))
        {
            this.Binaryoutput = binaryoutput;
            this.Description = description;
            this.DstPath = dstPath;
            this.ReturnCode = returnCode;
            this.Status = status;
            this.Stream = stream;
            this.Code = code;
            this.Message = message;
        }
        
        /// <summary>
        /// Byte Array of signed file
        /// </summary>
        /// <value>Byte Array of signed file</value>
        [DataMember(Name="binaryoutput", EmitDefaultValue=false)]
        public string Binaryoutput { get; set; }

        /// <summary>
        /// Description of generated error and/or details for error code returned
        /// </summary>
        /// <value>Description of generated error and/or details for error code returned</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// File path specified in request used by server&lt;br/&gt;  In case of collision, the server don&#39;t overwrites the files and uses files like (1..9)
        /// </summary>
        /// <value>File path specified in request used by server&lt;br/&gt;  In case of collision, the server don&#39;t overwrites the files and uses files like (1..9)</value>
        [DataMember(Name="dstPath", EmitDefaultValue=false)]
        public string DstPath { get; set; }

        /// <summary>
        /// Enum that indicates error code:  &lt;ul&gt;  &lt;li&gt;\&quot;0001\&quot; -&gt; Generic Error&lt;/li&gt;  &lt;li&gt;\&quot;0002\&quot; -&gt; Incorrect parameters for trasportation type selected&lt;/li&gt;  &lt;li&gt;\&quot;0003\&quot; -&gt; Error after credential check&lt;/li&gt;  &lt;li&gt;\&quot;0004\&quot; -&gt; PIN Error. Details in description field)&lt;/li&gt;  &lt;li&gt;\&quot;0005\&quot; -&gt; Trasportation type not valid&lt;/li&gt;  &lt;li&gt;\&quot;0006\&quot; -&gt; Trasportation type not authorized&lt;/li&gt;  &lt;li&gt;\&quot;0007\&quot; -&gt; Profile of PDF Signature not valid&lt;/li&gt;  &lt;li&gt;\&quot;0008\&quot; -&gt; Impossible to complete time-stamp marking operation&lt;/li&gt;  &lt;li&gt;\&quot;0009\&quot; -&gt; Delegated credential not valid&lt;/li&gt;  &lt;li&gt;\&quot;0010\&quot; -&gt; User status not valid&lt;/li&gt;  &lt;/ul&gt;
        /// </summary>
        /// <value>Enum that indicates error code:  &lt;ul&gt;  &lt;li&gt;\&quot;0001\&quot; -&gt; Generic Error&lt;/li&gt;  &lt;li&gt;\&quot;0002\&quot; -&gt; Incorrect parameters for trasportation type selected&lt;/li&gt;  &lt;li&gt;\&quot;0003\&quot; -&gt; Error after credential check&lt;/li&gt;  &lt;li&gt;\&quot;0004\&quot; -&gt; PIN Error. Details in description field)&lt;/li&gt;  &lt;li&gt;\&quot;0005\&quot; -&gt; Trasportation type not valid&lt;/li&gt;  &lt;li&gt;\&quot;0006\&quot; -&gt; Trasportation type not authorized&lt;/li&gt;  &lt;li&gt;\&quot;0007\&quot; -&gt; Profile of PDF Signature not valid&lt;/li&gt;  &lt;li&gt;\&quot;0008\&quot; -&gt; Impossible to complete time-stamp marking operation&lt;/li&gt;  &lt;li&gt;\&quot;0009\&quot; -&gt; Delegated credential not valid&lt;/li&gt;  &lt;li&gt;\&quot;0010\&quot; -&gt; User status not valid&lt;/li&gt;  &lt;/ul&gt;</value>
        [DataMember(Name="return_code", EmitDefaultValue=false)]
        public string ReturnCode { get; set; }

        /// <summary>
        /// Result of signature:  &lt;ul&gt;  &lt;li&gt;OK&lt;/li&gt;  &lt;li&gt;{errore}: See description and return_code for more details&lt;/li&gt;  &lt;/ul&gt;
        /// </summary>
        /// <value>Result of signature:  &lt;ul&gt;  &lt;li&gt;OK&lt;/li&gt;  &lt;li&gt;{errore}: See description and return_code for more details&lt;/li&gt;  &lt;/ul&gt;</value>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public string Status { get; set; }

        /// <summary>
        /// Stream used to download large size file (only for Transport &#x3D; STREAM)
        /// </summary>
        /// <value>Stream used to download large size file (only for Transport &#x3D; STREAM)</value>
        [DataMember(Name="stream", EmitDefaultValue=false)]
        public string Stream { get; set; }

        /// <summary>
        /// For compatibility with new architecture ONLY. DO NOT USE!
        /// </summary>
        /// <value>For compatibility with new architecture ONLY. DO NOT USE!</value>
        [DataMember(Name="code", EmitDefaultValue=false)]
        public string Code { get; set; }

        /// <summary>
        /// For compatibility with new architecture ONLY. DO NOT USE!
        /// </summary>
        /// <value>For compatibility with new architecture ONLY. DO NOT USE!</value>
        [DataMember(Name="message", EmitDefaultValue=false)]
        public string Message { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class JsonSignReturnV2 {\n");
            sb.Append("  Binaryoutput: ").Append(Binaryoutput).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  DstPath: ").Append(DstPath).Append("\n");
            sb.Append("  ReturnCode: ").Append(ReturnCode).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Stream: ").Append(Stream).Append("\n");
            sb.Append("  Code: ").Append(Code).Append("\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
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
            return this.Equals(input as JsonSignReturnV2);
        }

        /// <summary>
        /// Returns true if JsonSignReturnV2 instances are equal
        /// </summary>
        /// <param name="input">Instance of JsonSignReturnV2 to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(JsonSignReturnV2 input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Binaryoutput == input.Binaryoutput ||
                    (this.Binaryoutput != null &&
                    this.Binaryoutput.Equals(input.Binaryoutput))
                ) && 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.DstPath == input.DstPath ||
                    (this.DstPath != null &&
                    this.DstPath.Equals(input.DstPath))
                ) && 
                (
                    this.ReturnCode == input.ReturnCode ||
                    (this.ReturnCode != null &&
                    this.ReturnCode.Equals(input.ReturnCode))
                ) && 
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
                ) && 
                (
                    this.Stream == input.Stream ||
                    (this.Stream != null &&
                    this.Stream.Equals(input.Stream))
                ) && 
                (
                    this.Code == input.Code ||
                    (this.Code != null &&
                    this.Code.Equals(input.Code))
                ) && 
                (
                    this.Message == input.Message ||
                    (this.Message != null &&
                    this.Message.Equals(input.Message))
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
                if (this.Binaryoutput != null)
                    hashCode = hashCode * 59 + this.Binaryoutput.GetHashCode();
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.DstPath != null)
                    hashCode = hashCode * 59 + this.DstPath.GetHashCode();
                if (this.ReturnCode != null)
                    hashCode = hashCode * 59 + this.ReturnCode.GetHashCode();
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
                if (this.Stream != null)
                    hashCode = hashCode * 59 + this.Stream.GetHashCode();
                if (this.Code != null)
                    hashCode = hashCode * 59 + this.Code.GetHashCode();
                if (this.Message != null)
                    hashCode = hashCode * 59 + this.Message.GetHashCode();
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
