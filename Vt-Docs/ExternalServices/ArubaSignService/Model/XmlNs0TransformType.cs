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
    /// Defines xml_ns0_transformType
    /// </summary>
    
    [JsonConverter(typeof(StringEnumConverter))]
    
    public enum XmlNs0TransformType
    {
        
        /// <summary>
        /// Enum CANONICALWITHCOMMENT for value: CANONICAL_WITH_COMMENT
        /// </summary>
        [EnumMember(Value = "CANONICAL_WITH_COMMENT")]
        CANONICALWITHCOMMENT = 1,
        
        /// <summary>
        /// Enum CANONICALOMITCOMMENT for value: CANONICAL_OMIT_COMMENT
        /// </summary>
        [EnumMember(Value = "CANONICAL_OMIT_COMMENT")]
        CANONICALOMITCOMMENT = 2,
        
        /// <summary>
        /// Enum BASE64 for value: BASE64
        /// </summary>
        [EnumMember(Value = "BASE64")]
        BASE64 = 3,
        
        /// <summary>
        /// Enum XPATH2INTERSECT for value: XPATH2_INTERSECT
        /// </summary>
        [EnumMember(Value = "XPATH2_INTERSECT")]
        XPATH2INTERSECT = 4,
        
        /// <summary>
        /// Enum XPATH2SUBTRACT for value: XPATH2_SUBTRACT
        /// </summary>
        [EnumMember(Value = "XPATH2_SUBTRACT")]
        XPATH2SUBTRACT = 5,
        
        /// <summary>
        /// Enum XPATH2UNION for value: XPATH2_UNION
        /// </summary>
        [EnumMember(Value = "XPATH2_UNION")]
        XPATH2UNION = 6,
        
        /// <summary>
        /// Enum XSLT for value: XSLT
        /// </summary>
        [EnumMember(Value = "XSLT")]
        XSLT = 7
    }

}
