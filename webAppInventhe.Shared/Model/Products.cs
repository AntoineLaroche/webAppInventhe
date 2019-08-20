using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace webAppInventhe.Shared.Model
{
    public class Products :IValidatableObject
    {
        [Required]
        public double Quantity { get; set; } = 0.0;
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
