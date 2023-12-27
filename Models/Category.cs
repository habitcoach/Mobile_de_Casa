using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mobile_de_Casa.Models
{
    public class Category
    {
        [Key]
      
        public int  Id { get; set; }
        [Required]
        [Display(Name = "Brand")]
        public string  Name { get; set; }

       List<SubCategory> subCategories = new List<SubCategory>();

        //public static implicit operator Category(List<Category> v)
        //{
        //    throw new NotImplementedException();
        //}
    }
}


