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
    /// An object that contains the list of authentication methods
    /// </summary>
    [DataContract]
    public partial class JsonAuthMethodsReturn : JsonArssReturn,  IEquatable<JsonAuthMethodsReturn>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonAuthMethodsReturn" /> class.
        /// </summary>
        /// <param name="methods">List of authentication methods.</param>
        /// <param name="status">Result of signature:  &lt;ul&gt;  &lt;li&gt;OK&lt;/li&gt;  &lt;li&gt;{errore}: See description and return_code for more details&lt;/li&gt;  &lt;/ul&gt;.</param>
        /// <param name="description">Description of generated error and/or details for error code returned.</param>
        /// <param name="returnCode">String result code, \&quot;0000\&quot; in case of success.</param>
        public JsonAuthMethodsReturn(List<string> methods = default(List<string>), string status = default(string), string description = default(string), string returnCode = default(string)) : base()
        {
            this.Methods = methods;
            this.Status = status;
            this.Description = description;
            this.ReturnCode = returnCode;
        }
        
        /// <summary>
        /// List of authentication methods
        /// </summary>
        /// <value>List of authentication methods</value>
        [DataMember(Name="methods", EmitDefaultValue=false)]
        public List<string> Methods { get; set; }

        /// <summary>
        /// Result of signature:  &lt;ul&gt;  &lt;li&gt;OK&lt;/li&gt;  &lt;li&gt;{errore}: See description and return_code for more details&lt;/li&gt;  &lt;/ul&gt;
        /// </summary>
        /// <value>Result of signature:  &lt;ul&gt;  &lt;li&gt;OK&lt;/li&gt;  &lt;li&gt;{errore}: See description and return_code for more details&lt;/li&gt;  &lt;/ul&gt;</value>
        [DataMember(Name="status", EmitDefaultValue=false)]
        public string Status { get; set; }

        /// <summary>
        /// Description of generated error and/or details for error code returned
        /// </summary>
        /// <value>Description of generated error and/or details for error code returned</value>
        [DataMember(Name="description", EmitDefaultValue=false)]
        public string Description { get; set; }

        /// <summary>
        /// String result code, \&quot;0000\&quot; in case of success
        /// </summary>
        /// <value>String result code, \&quot;0000\&quot; in case of success</value>
        [DataMember(Name="return_code", EmitDefaultValue=false)]
        public string ReturnCode { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class JsonAuthMethodsReturn {\n");
            sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append("\n");
            sb.Append("  Methods: ").Append(Methods).Append("\n");
            sb.Append("  Status: ").Append(Status).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  ReturnCode: ").Append(ReturnCode).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public override string ToJson()
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
            return this.Equals(input as JsonAuthMethodsReturn);
        }

        /// <summary>
        /// Returns true if JsonAuthMethodsReturn instances are equal
        /// </summary>
        /// <param name="input">Instance of JsonAuthMethodsReturn to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(JsonAuthMethodsReturn input)
        {
            if (input == null)
                return false;

            return base.Equals(input) && 
                (
                    this.Methods == input.Methods ||
                    this.Methods != null &&
                    this.Methods.SequenceEqual(input.Methods)
                ) && base.Equals(input) && 
                (
                    this.Status == input.Status ||
                    (this.Status != null &&
                    this.Status.Equals(input.Status))
                ) && base.Equals(input) && 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && base.Equals(input) && 
                (
                    this.ReturnCode == input.ReturnCode ||
                    (this.ReturnCode != null &&
                    this.ReturnCode.Equals(input.ReturnCode))
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
                int hashCode = base.GetHashCode();
                if (this.Methods != null)
                    hashCode = hashCode * 59 + this.Methods.GetHashCode();
                if (this.Status != null)
                    hashCode = hashCode * 59 + this.Status.GetHashCode();
                if (this.Description != null)
                    hashCode = hashCode * 59 + this.Description.GetHashCode();
                if (this.ReturnCode != null)
                    hashCode = hashCode * 59 + this.ReturnCode.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        //IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        //{
        //    foreach(var x in BaseValidate(validationContext)) yield return x;
        //    yield break;
        //}
    }

}
