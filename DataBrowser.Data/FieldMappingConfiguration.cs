// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable EmptyNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.6
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBrowser.Data
{

    // FieldMappingConfiguration
    [Table("FieldMappingConfiguration", Schema = "dbo")]
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.36.1.0")]
    public partial class FieldMappingConfiguration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(@"Id", Order = 1, TypeName = "int")]
        [Index(@"PK_FieldMappingConfiguration", 1, IsUnique = true, IsClustered = true)]
        [Required]
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; } // Id (Primary key)

        [Column(@"MapTableName", Order = 2, TypeName = "varchar")]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Map table name")]
        public string MapTableName { get; set; } // MapTableName (length: 50)

        [Column(@"MapColumnName", Order = 3, TypeName = "varchar")]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Map column name")]
        public string MapColumnName { get; set; } // MapColumnName (length: 50)

        [Column(@"FieldConfigurationId", Order = 4, TypeName = "int")]
        [Display(Name = "Field configuration ID")]
        public int? FieldConfigurationId { get; set; } // FieldConfigurationId

        [Column(@"MasterTableName", Order = 5, TypeName = "varchar")]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Master table name")]
        public string MasterTableName { get; set; } // MasterTableName (length: 50)

        // Foreign keys

        /// <summary>
        /// Parent FieldConfiguration pointed by [FieldMappingConfiguration].([FieldConfigurationId]) (FK_FieldMappingConfiguration_FieldConfiguration)
        /// </summary>
        [ForeignKey("FieldConfigurationId")] public virtual FieldConfiguration FieldConfiguration { get; set; } // FK_FieldMappingConfiguration_FieldConfiguration
    }

}
// </auto-generated>