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
    /// Signer Object. Order is important for WSDL.
    /// </summary>
    [DataContract]
    public partial class JsonSigner :  IEquatable<JsonSigner>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSigner" /> class.
        /// </summary>
        /// <param name="serialnumber">The signer serial number..</param>
        /// <param name="signername">The signer name..</param>
        /// <param name="description">The signer description..</param>
        /// <param name="result">The signer result..</param>
        /// <param name="countersigners">The counter signers..</param>
        /// <param name="marks">The temporal marks..</param>
        public JsonSigner(string serialnumber = default(string), string signername = default(string), string description = default(string), string result = default(string), List<JsonSigner> countersigners = default(List<JsonSigner>), List<JsonMark> marks = default(List<JsonMark>))
        {
            this.Serialnumber = serialnumber;
            this.Signername = signername;
            this.Description = description;
            this.Result = result;
            this.Countersigners = countersigners;
            this.Marks = marks;
        }
        
        /// <summary>
        /// The signer serial number.
        /// </summary>
        /// <value>The signer serial number.</value>
        [DataMember(Name="serialnumber", EmitDefaultValue=false)]
        public string Serialnumber { get; set; }

        /// <summary>
        /// The signer name.
        /// </summary>
        /// <value>The signer name.</value>
        [DataMember(Name="signername", EmitDefaultValue=false)]
        public string Signername { get; set; }

        /// <summary>
        /// The signer description.
        /// </summary>
        /// <value>The signer description.</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// The signer result.
        /// </summary>
        /// <value>The signer result.</value>
        [DataMember(Name="result", EmitDefaultValue=false)]
        public string Result { get; set; }

        /// <summary>
        /// The counter signers.
        /// </summary>
        /// <value>The counter signers.</value>
        [DataMember(Name="countersigners", EmitDefaultValue=false)]
        public List<JsonSigner> Countersigners { get; set; }

        /// <summary>
        /// The temporal marks.
        /// </summary>
        /// <value>The temporal marks.</value>
        [DataMember(Name="marks", EmitDefaultValue=false)]
        public List<JsonMark> Marks { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class JsonSigner {\n");
            sb.Append("  Serialnumber: ").Append(Serialnumber).Append("\n");
            sb.Append("  Signername: ").Append(Signername).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  Result: ").Append(Result).Append("\n");
            sb.Append("  Countersigners: ").Append(Countersigners).Append("\n");
            sb.Append("  Marks: ").Append(Marks).Append("\n");
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
            return this.Equals(input as JsonSigner);
        }

        /// <summary>
        /// Returns true if JsonSigner instances are equal
        /// </summary>
        /// <param name="input">Instance of JsonSigner to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(JsonSigner input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Serialnumber == input.Serialnumber ||
                    (this.Serialnumber != null &&
                    this.Serialnumber.Equals(input.Serialnumber))
                ) && 
                (
                    this.Signername == input.Signername ||
                    (this.Signername != null &&
                    this.Signername.Equals(input.Signername))
                ) && 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.Result == input.Result ||
                    (this.Result != null &&
                    this.Result.Equals(input.Result))
                ) && 
                (
                    this.Countersigners == input.Countersigners ||
                    this.Countersigners != null &&
                    this.Countersigners.SequenceEqual(input.Countersigners)
                ) && 
                (
                    this.Marks == input.Marks ||
                    this.Marks != null &&
                    this.Marks.SequenceEqual(input.Marks)
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
                if (this.Serialnumber != null)
                    hashCode = hashCode * 59 + this.Serialnumber.GetHashCode();
                if (this.Signername != null)
                    hashCode = hashCode * 59 + this.Signername.GetHashCode();
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.Result != null)
                    hashCode = hashCode * 59 + this.Result.GetHashCode();
                if (this.Countersigners != null)
                    hashCode = hashCode * 59 + this.Countersigners.GetHashCode();
                if (this.Marks != null)
                    hashCode = hashCode * 59 + this.Marks.GetHashCode();
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
