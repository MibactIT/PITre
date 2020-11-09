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
    /// Verify Signatures Object
    /// </summary>
    [DataContract]
    public partial class JsonVerifySignatures :  IEquatable<JsonVerifySignatures>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonVerifySignatures" /> class.
        /// </summary>
        /// <param name="description">Verification description..</param>
        /// <param name="mark">Temporal marks associated to result..</param>
        /// <param name="signer">Signers associated to result..</param>
        /// <param name="status">Verification status..</param>
        public JsonVerifySignatures(string description = default(string), List<JsonMark> mark = default(List<JsonMark>), List<JsonSigner> signer = default(List<JsonSigner>), string status = default(string))
        {
            this.Description = description;
            this.Mark = mark;
            this.Signer = signer;
            this.Status = status;
        }
        
        /// <summary>
        /// Verification description.
        /// </summary>
        /// <value>Verification description.</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// Temporal marks associated to result.
        /// </summary>
        /// <value>Temporal marks associated to result.</value>
        [DataMember(Name="mark", EmitDefaultValue=false)]
        public List<JsonMark> Mark { get; set; }

        /// <summary>
        /// Signers associated to result.
        /// </summary>
        /// <value>Signers associated to result.</value>
        [DataMember(Name="signer", EmitDefaultValue=false)]
        public List<JsonSigner> Signer { get; set; }

        /// <summary>
        /// Verification status.
        /// </summary>
        /// <value>Verification status.</value>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public string Status { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class JsonVerifySignatures {\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Mark: ").Append(Mark).Append("\n");
            sb.Append("  Signer: ").Append(Signer).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
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
            return this.Equals(input as JsonVerifySignatures);
        }

        /// <summary>
        /// Returns true if JsonVerifySignatures instances are equal
        /// </summary>
        /// <param name="input">Instance of JsonVerifySignatures to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(JsonVerifySignatures input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.Mark == input.Mark ||
                    this.Mark != null &&
                    this.Mark.SequenceEqual(input.Mark)
                ) && 
                (
                    this.Signer == input.Signer ||
                    this.Signer != null &&
                    this.Signer.SequenceEqual(input.Signer)
                ) && 
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
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
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.Mark != null)
                    hashCode = hashCode * 59 + this.Mark.GetHashCode();
                if (this.Signer != null)
                    hashCode = hashCode * 59 + this.Signer.GetHashCode();
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
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