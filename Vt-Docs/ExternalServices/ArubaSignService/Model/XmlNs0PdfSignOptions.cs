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
    /// Signature options for PDF.
    /// </summary>
    [DataContract]
    public partial class XmlNs0PdfSignOptions : XmlNs0SignOptions,  IEquatable<XmlNs0PdfSignOptions>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlNs0PdfSignOptions" /> class.
        /// </summary>
        /// <param name="appearance">The PDF appearance in case of visible signature..</param>
        /// <param name="dictSignedAttributes">The dictionary signed attributes..</param>
        /// <param name="pdfProfile">The PDF profile..</param>
        public XmlNs0PdfSignOptions(XmlNs0PdfSignApparence appearance = default(XmlNs0PdfSignApparence), XmlNs0DictionarySignedAttributes dictSignedAttributes = default(XmlNs0DictionarySignedAttributes), XmlNs0PDFProfile pdfProfile = default(XmlNs0PDFProfile), string certId = default(string), XmlNs0Auth identity = default(XmlNs0Auth), bool? requiredMark = default(bool?), string sessionId = default(string), string signingTime = default(string), XmlNs0TSAAuth tsaIdentity = default(XmlNs0TSAAuth)) : base()
        {
            this.Appearance = appearance;
            this.DictSignedAttributes = dictSignedAttributes;
            this.PdfProfile = pdfProfile;
        }
        
        /// <summary>
        /// The PDF appearance in case of visible signature.
        /// </summary>
        /// <value>The PDF appearance in case of visible signature.</value>
        [DataMember(Name="appearance", EmitDefaultValue=false)]
        public XmlNs0PdfSignApparence Appearance { get; set; }

        /// <summary>
        /// The dictionary signed attributes.
        /// </summary>
        /// <value>The dictionary signed attributes.</value>
        [DataMember(Name="dictSignedAttributes", EmitDefaultValue=false)]
        public XmlNs0DictionarySignedAttributes DictSignedAttributes { get; set; }

        /// <summary>
        /// The PDF profile.
        /// </summary>
        /// <value>The PDF profile.</value>
        [DataMember(Name="pdfProfile", EmitDefaultValue=false)]
        public XmlNs0PDFProfile PdfProfile { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class XmlNs0PdfSignOptions {\n");
            sb.Append("  ").Append(base.ToString().Replace("\n", "\n  ")).Append("\n");
            sb.Append("  Appearance: ").Append(Appearance).Append("\n");
            sb.Append("  DictSignedAttributes: ").Append(DictSignedAttributes).Append("\n");
            sb.Append("  PdfProfile: ").Append(PdfProfile).Append("\n");
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
            return this.Equals(input as XmlNs0PdfSignOptions);
        }

        /// <summary>
        /// Returns true if XmlNs0PdfSignOptions instances are equal
        /// </summary>
        /// <param name="input">Instance of XmlNs0PdfSignOptions to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(XmlNs0PdfSignOptions input)
        {
            if (input == null)
                return false;

            return base.Equals(input) && 
                (
                    this.Appearance == input.Appearance ||
                    (this.Appearance != null &&
                    this.Appearance.Equals(input.Appearance))
                ) && base.Equals(input) && 
                (
                    this.DictSignedAttributes == input.DictSignedAttributes ||
                    (this.DictSignedAttributes != null &&
                    this.DictSignedAttributes.Equals(input.DictSignedAttributes))
                ) && base.Equals(input) && 
                (
                    this.PdfProfile == input.PdfProfile ||
                    (this.PdfProfile != null &&
                    this.PdfProfile.Equals(input.PdfProfile))
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
                if (this.Appearance != null)
                    hashCode = hashCode * 59 + this.Appearance.GetHashCode();
                if (this.DictSignedAttributes != null)
                    hashCode = hashCode * 59 + this.DictSignedAttributes.GetHashCode();
                if (this.PdfProfile != null)
                    hashCode = hashCode * 59 + this.PdfProfile.GetHashCode();
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