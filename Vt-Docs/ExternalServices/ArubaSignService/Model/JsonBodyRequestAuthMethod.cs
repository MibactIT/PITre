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
    /// Object containing the User Idendity
    /// </summary>
    [DataContract]
    public partial class JsonBodyRequestAuthMethod :  IEquatable<JsonBodyRequestAuthMethod>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonBodyRequestAuthMethod" /> class.
        /// </summary>
        /// <param name="identity">User Idendity for which you&#39;re requesting the signatures.</param>
        public JsonBodyRequestAuthMethod(JsonAuth identity = default(JsonAuth))
        {
            this.Identity = identity;
        }
        
        /// <summary>
        /// User Idendity for which you&#39;re requesting the signatures
        /// </summary>
        /// <value>User Idendity for which you&#39;re requesting the signatures</value>
        [DataMember(Name="identity", EmitDefaultValue=false)]
        public JsonAuth Identity { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class JsonBodyRequestAuthMethod {\n");
            sb.Append("  Identity: ").Append(Identity).Append("\n");
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
            return this.Equals(input as JsonBodyRequestAuthMethod);
        }

        /// <summary>
        /// Returns true if JsonBodyRequestAuthMethod instances are equal
        /// </summary>
        /// <param name="input">Instance of JsonBodyRequestAuthMethod to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(JsonBodyRequestAuthMethod input)
        {
            if (input == null)
                return false;

            return 
                (
                    this.Identity == input.Identity ||
                    (this.Identity != null &&
                    this.Identity.Equals(input.Identity))
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
