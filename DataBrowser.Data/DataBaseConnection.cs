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

    // DataBaseConnection
    [Table("DataBaseConnection", Schema = "dbo")]
    [System.CodeDom.Compiler.GeneratedCode("EF.Reverse.POCO.Generator", "2.36.1.0")]
    public partial class DataBaseConnection
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(@"Id", Order = 1, TypeName = "int")]
        [Index(@"PK_DataBaseConnection", 1, IsUnique = true, IsClustered = true)]
        [Required]
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; } // Id (Primary key)

        [Column(@"Name", Order = 2, TypeName = "nvarchar")]
        [MaxLength(250)]
        [StringLength(250)]
        [Display(Name = "Name")]
        public string Name { get; set; } // Name (length: 250)

        [Column(@"ServerInstanceName", Order = 3, TypeName = "nvarchar")]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Server instance name")]
        public string ServerInstanceName { get; set; } // ServerInstanceName (length: 50)

        [Column(@"UserName", Order = 4, TypeName = "nvarchar")]
        [MaxLength(50)]
        [StringLength(50)]
        [DataType(DataType.Text)]
        [Display(Name = "User name")]
        public string UserName { get; set; } // UserName (length: 50)

        [Column(@"Password", Order = 5, TypeName = "nvarchar")]
        [MaxLength(50)]
        [StringLength(50)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } // Password (length: 50)

        [Column(@"DataBaseName", Order = 6, TypeName = "nvarchar")]
        [MaxLength(50)]
        [StringLength(50)]
        [Display(Name = "Data base name")]
        public string DataBaseName { get; set; } // DataBaseName (length: 50)

        // Reverse navigation

        /// <summary>
        /// Child TableConfigurations where [TableConfiguration].[ConnectionId] point to this entity (FK_TableConfiguration_DataBaseConnection)
        /// </summary>
        //public virtual System.Collections.Generic.ICollection<TableConfiguration> TableConfigurations { get; set; } = new System.Collections.Generic.List<TableConfiguration>(); // TableConfiguration.FK_TableConfiguration_DataBaseConnection
    }

}
// </auto-generated>
